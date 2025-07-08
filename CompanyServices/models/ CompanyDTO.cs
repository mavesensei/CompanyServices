namespace CompanyServices.Models
{
    public class CompanyDTO
    {
        public int id { get; set; }
        public string name { get; set; }
        public string sector { get; set; }
        public string city { get; set; }
        public int employeeCount { get; set; }
        public DateTime lastUpdated { get; set; }
    }
}