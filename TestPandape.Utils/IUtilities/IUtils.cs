using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestPandape.Entity.Candidates;
using TestPandape.Entity.Experiences;
using TestPandape.Repository.DataModel;

namespace TestPandape.Lib.IUtilities
{
    public interface IUtils
    {
        Task<CandidateDataModel> MapperCandidateModelToEntity(CandidateRequest model, bool isCreate);
        Task<CandidateDataModel> MapperCandidateReadToEntity(CandidateReadRequest model, bool isCreate);
        Task<List<CandidateReadRequest>> MapperCandidateListEntitytoModel(List<CandidateDataModel> entity);
        Task<CandidateReadRequest> MapperCandidateEntitytoModel(CandidateDataModel entity);
        Task<CandidateDataModel> MapperCandidateUpdateToEntity(CandidateReadRequest modelRead, CandidateUpdateRequest modelUpdate);

        Task<List<ExperienceReadRequest>> MapperExperienceListEntitytoModel(List<CandidateExperiencesDataModel> entity);
        Task<CandidateExperiencesDataModel> MapperExperienceModelToEntity(ExperienceRequest model, bool isCreate);

        Task<CandidateExperiencesDataModel> MapperExperienceReadToEntity(ExperienceReadRequest model, bool isCreate);
        Task<ExperienceReadRequest> MapperExperienceEntitytoModel(CandidateExperiencesDataModel entity);
        Task<CandidateExperiencesDataModel> MapperExperienceUpdateToEntity(ExperienceReadRequest modelRead, ExperienceUpdateRequest modelUpdate);

        Task<bool> isValidEmail(string email);
        Task<bool> isValidDatetime(string dateTime);
        Task<bool> isValidDecimal(string valueDecimal);
    }
}
