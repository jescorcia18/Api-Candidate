using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestPandape.Entity.Candidates;
using TestPandape.Entity.Pagination;
using TestPandape.Repository.DataModel;

namespace TestPandape.Repository.IRepository
{
    public interface ICandidateRepository
    {
        Task<CandidateDataModel> Create(CandidateDataModel request);
        Task<List<CandidateDataModel>> Search(CandidateRequest searchRequest, Paginator paginator, Sorter sorter, bool validateEmail = false);
        Task<int> TotalCount(CandidateRequest searchRequest);
        Task<CandidateDataModel?> Read(int id);
        Task<CandidateDataModel> Update(int id, CandidateDataModel entity);
        Task Delete(int id);


        //Task<CandidateResponse> GetList(CandidateRequest request);

        //Task<CandidateResponse> Update(CandidateRequest request);

        //Task<CandidateResponse> Delete(CandidateRequest request);
    }
}
