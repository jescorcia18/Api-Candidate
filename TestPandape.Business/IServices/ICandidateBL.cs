using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestPandape.Entity.Candidates;
using TestPandape.Entity.Pagination;
using TestPandape.Entity.UriServices;

namespace TestPandape.Business.IServices
{
    public interface ICandidateBL
    {
        Task<CandidateResponse> CreateCandidateService(CandidateRequest request);
        Task<CandidateResponse> UpdateCandidateService(int idCandidate , CandidateUpdateRequest updateRequest);
        Task<Paged<CandidateReadRequest>> Search(CandidateRequest searchRequest, Paginator paginator, Sorter sorter, IUriservice uriservice, bool validateEmail, string route);
        Task<Paged<CandidateReadRequest>> GetAllCandidatesService(CandidateRequest searchRequest, Paginator paginator, Sorter sorter);
        Task<CandidateReadRequest?> GetCandidateService(int idCandidate);
        Task<string> DeleteCandidateService(int idCandidate);
    }
}
