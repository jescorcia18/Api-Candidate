using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPandape.Repository.DataModel
{
    //[Table (name:"Candidate")]
    public class CandidateDataModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdCandidate { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(150)]
        public string Surname { get; set; } = string.Empty;

        [Required]
        public DateTime Birthdate { get; set; }

        [Required]
        [StringLength(250)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public DateTime InsertDate { get; set; }

        public DateTime? ModifyDate { get; set; }

        public virtual ICollection<CandidateExperiencesDataModel> Experiences { get; set; }
    }
}
