using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestPandape.Entity.Experiences;
using TestPandape.Entity.Pagination;
using TestPandape.Entity.UriServices;

namespace TestPandape.Business.IServices
{
    public interface IExperienceBL
    {
        Task<ExperienceResponse> CreateExperienceService(ExperienceRequest request);
        Task<ExperienceReadRequest?> GetExperienceService(int idExperience);
        Task<ExperienceResponse> UpdateExperienceService(int idExperience, ExperienceUpdateRequest updateRequest);
        Task<string> DeleteExperienceService(int idExperience);
        Task<Paged<ExperienceReadRequest>> GetAllExperienceService(ExperienceRequest searchRequest, Paginator paginator, Sorter sorter);
        Task<Paged<ExperienceReadRequest>> Search(ExperienceRequest searchRequest, Paginator paginator, Sorter sorter, IUriservice uriservice, string route);
    }
}
