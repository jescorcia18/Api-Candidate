using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace TestPandape.Entity.Experiences
{
    public class ExperienceRequest
    {
        decimal _salary;
        public int IdCandidate { get; set; }
        public string Company { get; set; } = string.Empty;
        public string Job { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Salary 
        {
            get { return Math.Round(_salary, 2); } 
            set { _salary = value; }
        }
        public DateTime BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
