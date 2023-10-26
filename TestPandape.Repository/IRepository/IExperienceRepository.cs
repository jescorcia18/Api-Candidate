using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestPandape.Entity.Experiences;
using TestPandape.Entity.Pagination;
using TestPandape.Repository.DataModel;

namespace TestPandape.Repository.IRepository
{
    public interface IExperienceRepository
    {
        Task<CandidateExperiencesDataModel> Create(CandidateExperiencesDataModel entity);
        Task<CandidateExperiencesDataModel?> Read(int id);
        Task<CandidateExperiencesDataModel> Update(int id, CandidateExperiencesDataModel entity);
        Task Delete(int id);
        Task<List<CandidateExperiencesDataModel>> Search(ExperienceRequest searchRequest, Paginator paginator, Sorter sorter);
        Task<int> TotalCount(ExperienceRequest searchRequest);
    }
}
