using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;


namespace CompanyServices.Models
{
    public class Companies
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string name { get; set; }
        public string sector { get; set; }
        public string city { get; set; }
        public int employeeCount { get; set; }
        public DateTime lastUpdated { get; set; }
    }
}