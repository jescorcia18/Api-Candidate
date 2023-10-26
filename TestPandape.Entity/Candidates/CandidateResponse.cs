using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestPandape.Entity.Message;

namespace TestPandape.Entity.Candidates
{
    public class CandidateResponse
    {
        public int? IdCandidate { get; set; }
        public MessageResponse MessageResponse { get; set; } = new MessageResponse();
    }
}
