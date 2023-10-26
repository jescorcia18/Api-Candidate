using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPandape.Repository.DataModel
{
    public class CandidateExperiencesDataModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdCandidateExperience { get; set; }

        public int IdCandidate { get; set; }

        [Required]
        [StringLength(100)]
        public string Company { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Job { get; set; } = string.Empty;

        [Required]
        [StringLength(4000)]
        public string Description { get; set; } = string.Empty;

        [Column(TypeName = "numeric(8, 2)")]
        //[Precision(8, 2)]
        public decimal Salary { get; set; }

        [Required]
        public DateTime BeginDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required]
        public DateTime InsertDate { get; set; }

        public DateTime? ModifyDate { get; set; }

        [ForeignKey("IdCandidate")]
       public virtual CandidateDataModel Candidate { get; set; }

    }
}
