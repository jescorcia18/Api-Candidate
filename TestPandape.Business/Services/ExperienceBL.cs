using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestPandape.Business.IServices;
using TestPandape.Entity.Candidates;
using TestPandape.Entity.Experiences;
using TestPandape.Entity.Message;
using TestPandape.Entity.Pagination;
using TestPandape.Entity.UriServices;
using TestPandape.Lib.IUtilities;
using TestPandape.Repository.DataModel;
using TestPandape.Repository.IRepository;

namespace TestPandape.Business.Services
{
    public class ExperienceBL : IExperienceBL
    {
        #region Global Variables
        private readonly IExperienceRepository _repoExperience;
        private readonly IUtils _utils;
        private readonly IUriservice _uriService;
        private readonly ICandidateBL _candidateBL;

        #endregion

        #region Constructor Method
        public ExperienceBL(IExperienceRepository repoExperience, IUtils utils, IUriservice uriServices, ICandidateBL candidateBL)
        {
            _repoExperience = repoExperience;
            _utils = utils;
            _uriService = uriServices;
            _candidateBL = candidateBL;
        }
        #endregion

        #region Public Methods
        public async Task<ExperienceResponse> CreateExperienceService(ExperienceRequest request)
        {
            try
            {
                if (!await ValidateFields(request))
                {
                    return new ExperienceResponse
                    {
                        IdExperience = null,
                        IdCandidate = null,
                        MessageResponse = new MessageResponse
                        {
                            message = "One or more fields do not have the allowed value",
                            success = false
                        }
                    };
                }

                int idExistExperience = await IsExistExperience(request, _uriService, string.Empty);

                if (idExistExperience > 0)
                {
                    return new ExperienceResponse
                    {
                        IdExperience = idExistExperience,
                        IdCandidate = request.IdCandidate,
                        MessageResponse = new MessageResponse
                        {
                            message = "Experience is already registered.",
                            success = false
                        }
                    };

                }

                var ExistCandidate = await _candidateBL.GetCandidateService(request.IdCandidate);

                if (ExistCandidate == null)
                {
                    return new ExperienceResponse
                    {
                        IdExperience = null,
                        IdCandidate = request.IdCandidate,
                        MessageResponse = new MessageResponse
                        {
                            message = "Candidate is not registered.",
                            success = false
                        }
                    };

                }

                var entity = await _utils.MapperExperienceModelToEntity(request, true);

                var objResult = await _repoExperience.Create(entity);

                if (objResult != null)
                {
                    return new ExperienceResponse
                    {
                        IdExperience = objResult.IdCandidateExperience,
                        IdCandidate = objResult.IdCandidate,
                        MessageResponse = new MessageResponse { message = "Candidate successfully registered.", success = true }
                    };
                }
                else
                    throw new Exception("Could not create Candidate.");
            }
            catch (Exception ex)
            {
                return new ExperienceResponse
                {
                    IdExperience = null,
                    IdCandidate = null,
                    MessageResponse = new MessageResponse { message = ex.Message, success = false }
                };
            }
        }
        public async Task<ExperienceReadRequest?> GetExperienceService(int idExperience)
        {
            try
            {
                var entity = await _repoExperience.Read(idExperience);
                if (entity != null)
                    return await _utils.MapperExperienceEntitytoModel(entity);

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in the Experience Read service:" + ex.Message, ex);
            }
        }
        public async Task<ExperienceResponse> UpdateExperienceService(int idExperience, ExperienceUpdateRequest updateRequest)
        {
            try
            {
                var objExperience = await GetExperienceService(idExperience);
                if (objExperience == null)
                {
                    return new ExperienceResponse
                    {
                        IdExperience = null,
                        IdCandidate = null,
                        MessageResponse = new MessageResponse { message = "Experience NotFound.", success = false }
                    };
                }
                else
                {
                    if (objExperience.IdCandidateExperience != idExperience)
                    {
                        return new ExperienceResponse
                        {
                            IdExperience = null,
                            IdCandidate = null,
                            MessageResponse = new MessageResponse { message = "Id Experience Not Match.", success = false }
                        };
                    }
                }

                var entity = await _utils.MapperExperienceUpdateToEntity(objExperience, updateRequest);

                var result = await _repoExperience.Update(idExperience, entity);

                return new ExperienceResponse
                {
                    IdExperience = result.IdCandidateExperience,
                    IdCandidate = result.IdCandidate,
                    MessageResponse = new MessageResponse { message = "Candidate successfully updated.", success = true }
                };
            }
            catch (Exception ex)
            {
                return new ExperienceResponse
                {
                    IdCandidate = null,
                    MessageResponse = new MessageResponse { message = ex.Message, success = false }
                };
            }
        }
        public async Task<string> DeleteExperienceService(int idExperience)
        {
            try
            {
                var response = GetExperienceService(idExperience);
                if (response != null)
                {
                    await _repoExperience.Delete(idExperience);
                    return "Deleted";
                }

                return "NotFound";
            }
            catch (Exception ex)
            {
                throw new Exception("Error in the Experience delete service:" + ex.Message, ex);
            }

        }

        public async Task<Paged<ExperienceReadRequest>> GetAllExperienceService(ExperienceRequest searchRequest, Paginator paginator, Sorter sorter)
        {
            try
            {
                return await Search(searchRequest, paginator, sorter, _uriService, string.Empty);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in the Experience GetAll service:" + ex.Message, ex);
            }
        }
        public async Task<Paged<ExperienceReadRequest>> Search(ExperienceRequest searchRequest, Paginator paginator, Sorter sorter, IUriservice uriservice, string route)
        {
            try
            {
                int totalRecords;
                if (string.IsNullOrWhiteSpace(sorter.SortBy) || sorter.SortBy == "id")
                {
                    sorter.SortBy = "begindate";
                }
                var entities = await _repoExperience.Search(searchRequest, paginator, sorter);

                var candidateRead = await _utils.MapperExperienceListEntitytoModel(entities);

                totalRecords = await _repoExperience.TotalCount(searchRequest);

                var page = PaginationHelper.CreatePagedReponse<ExperienceReadRequest>(candidateRead, paginator, totalRecords, uriservice, route);
                return page;

            }
            catch (Exception ex)
            {
                throw new Exception("Error in the experience search service:" + ex.Message, ex);
            }
        }

        #endregion

        #region Private Methods
        private async Task<bool> ValidateFields(ExperienceRequest obj)
        {
            bool result = true;
            if (string.IsNullOrEmpty(obj.Company)) result = false;
            if (string.IsNullOrEmpty(obj.Job)) result = false;
            if (string.IsNullOrEmpty(obj.Description)) result = false;
            if (!await _utils.isValidDecimal(obj.Salary.ToString())) result = false;
            if (!await _utils.isValidDatetime(obj.BeginDate.ToString())) result = false;

            if (obj.EndDate != null)
                if (!await _utils.isValidDatetime(obj.EndDate.Value.ToString())) result = false;

            return await Task.FromResult(result);
        }

        private async Task<int> IsExistExperience(ExperienceRequest request, IUriservice uriservice, string route)
        {
            try
            {
                Paginator paginator = new() { PageNumber = 1, PageSize = 1 };

                Sorter sorter = new() { SortBy = "company", SortOrder = "asc" };

                var experienceRead = await Search(request, paginator, sorter, uriservice, route);

                //Exist experience
                if (experienceRead.Items != null && experienceRead.Items.Count > 0
                    && experienceRead.Items.First().IdCandidate.Equals(request.IdCandidate)
                    && (experienceRead.Items.First().Company.Equals(request.Company.ToUpper())
                        && experienceRead.Items.First().Job.Equals(request.Job.ToUpper())
                        && experienceRead.Items.First().Salary.Equals(request.Salary)
                        && experienceRead.Items.First().BeginDate.Equals(request.BeginDate)
                        && experienceRead.Items.First().EndDate.Equals(request.EndDate)
                        )
                    )
                    return experienceRead.Items.First().IdCandidateExperience;
                else //Not Exist experience
                    return 0;

            }
            catch (Exception ex)
            {
                throw new Exception("Error in the candidate search service:" + ex.Message, ex);
            }
        }

        //private async Task<int> IsExistCandidate (CandidateRequest request, IUriservice uriservice, string route)
        //{
        //    CandidateRequest entity = new CandidateRequest
        //    {
        //        IdCandidate = modelRead.IdCandidate,
        //        Company = modelRead.Company,
        //        Job = modelRead.Job,
        //        Description = modelRead.Description,
        //        Salary = modelRead.Salary,
        //        BeginDate = modelRead.BeginDate,
        //        EndDate = modelRead.EndDate,
        //        ModifyDate = DateTime.Now
        //    };
        //}
        #endregion
    }
}
