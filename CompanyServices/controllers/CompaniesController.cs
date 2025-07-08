using Microsoft.AspNetCore.Mvc;
using CompanyServices.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;


namespace CompanyServices.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompaniesController : ControllerBase
    {
        private readonly CompanyContext _context;
        private readonly IDistributedCache _redis;
        private readonly IHttpClientFactory _httpClientFactory;

        private readonly IConfiguration _configuration;
        public CompaniesController(CompanyContext context, IDistributedCache redis, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _context = context;
            _redis = redis;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }
        private CompanyDTO ToDto(Companies company)
        {
            return new CompanyDTO
            {
                id = company.id,
                name = company.name,
                sector = company.sector,
                city = company.city,
                employeeCount = company.employeeCount,
                lastUpdated = company.lastUpdated
            };
        }

        [HttpGet]
        public async Task<IActionResult> GetCompanies()
        {
            var companies = await _context.Companies.ToListAsync();
            var dtos = companies.Select(c => new CompanyDTO
            {
                id = c.id,
                name = c.name,
                sector = c.sector,
                city = c.city,
                employeeCount = c.employeeCount,
                lastUpdated = c.lastUpdated
            }).ToList();

            return Ok(dtos);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCompany(int id, CompanyUpdateDTO dto)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company == null) return NotFound();

            company.name = dto.name;
            company.sector = dto.sector;
            company.city = dto.city;
            company.employeeCount = dto.employeeCount;
            company.lastUpdated = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            await _redis.RemoveAsync($"company:{id}");

            await WebSocketHandler.BroadcastAsync($"Company updated: {company.id}");

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            var company = await _context.Companies.FindAsync(id);

            if (company == null)
            {
                return NotFound();
            }

            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();
            await _redis.RemoveAsync($"company:{id}");


            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCompanyById(int id)
        {
            string cacheKey = $"company:{id}";

            var cachedCompany = await _redis.GetStringAsync(cacheKey);
            Companies dbCompany;
            if (cachedCompany != null)
            {
                dbCompany = JsonSerializer.Deserialize<Companies>(cachedCompany);
            }
            else
            {
                dbCompany = await _context.Companies.FindAsync(id);
                if (dbCompany == null) return NotFound();

                var serialized = JsonSerializer.Serialize(dbCompany);
                int ttlMinutes = _configuration.GetValue<int>("CacheSettings:CompanyTtlMinutes");
                await _redis.SetStringAsync(cacheKey, serialized, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(ttlMinutes)
                });
            }

            var dto = new CompanyDTO
            {
                id = dbCompany.id,
                name = dbCompany.name,
                sector = dbCompany.sector,
                city = dbCompany.city,
                employeeCount = dbCompany.employeeCount,
                lastUpdated = dbCompany.lastUpdated
            };

            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCompany(CompanyCreateDTO dto)
        {
            var company = new Companies
            {
                name = dto.name,
                sector = dto.sector,
                city = dto.city,
                employeeCount = dto.employeeCount,
                lastUpdated = DateTime.UtcNow
            };

            _context.Companies.Add(company);
            await _context.SaveChangesAsync();

            var companyDto = ToDto(company);
            return CreatedAtAction(nameof(GetCompanyById), new { id = company.id }, companyDto);
        }

        [HttpPost("import-random")]
        public async Task<IActionResult> ImportRandomCompany()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://randomuser.me/api/");

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, "API call is failed");

            var content = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(content);
            var user = doc.RootElement.GetProperty("results")[0];

            var randomUser = new RandomUserDTO
            {
                Gender = user.GetProperty("gender").GetString(),
                name = new RandomUserDTO.Name
                {
                    Title = user.GetProperty("name").GetProperty("title").GetString(),
                    First = user.GetProperty("name").GetProperty("first").GetString(),
                    Last = user.GetProperty("name").GetProperty("last").GetString()
                },
                location = new RandomUserDTO.Location
                {
                    City = user.GetProperty("location").GetProperty("city").GetString(),
                    Country = user.GetProperty("location").GetProperty("country").GetString()
                },
                Email = user.GetProperty("email").GetString()
            };

            var company = new Companies
            {
                name = $"{randomUser.name.First} {randomUser.name.Last} Company",
                sector = "Imported",
                city = randomUser.location.City,
                employeeCount = new Random().Next(10, 1000),
                lastUpdated = DateTime.UtcNow
            };

            _context.Companies.Add(company);
            await _context.SaveChangesAsync();

            var companyDto = ToDto(company);
            return Ok(companyDto);
        }

        [HttpGet("health")]
        public IActionResult HealthCheck()
        {
            return Ok("Service is healthy");
        }

    }
}
