using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestPandape.Entity.Experiences;

namespace TestPandape.Entity.Candidates
{
    public class CandidateRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public DateTime Birthdate { get; set; }
        public string Email { get; set; } = string.Empty;
        public  ICollection<ExperienceRequest>? Experience { get; set; } = null;
    }
}
