using System.ComponentModel;
using System.Text.Json.Serialization;

namespace TestPandape.Entity.Experiences
{
    public class ExperienceUpdateRequest
    {

        public string Company { get; set; } = string.Empty;
        public string Job { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Salary { get; set; } 
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; } 
    }
}
