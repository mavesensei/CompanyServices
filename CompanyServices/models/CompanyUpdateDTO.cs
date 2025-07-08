using System.ComponentModel.DataAnnotations;

namespace CompanyServices.Models
{
    public class CompanyUpdateDTO
    {
        [Required]
        public string name { get; set; }
        
        [Required]
        public string sector { get; set; }
        
        [Required]
        public string city { get; set; }
        
        public int employeeCount { get; set; }
    }
}
