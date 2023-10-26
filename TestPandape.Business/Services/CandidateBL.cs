using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TestPandape.Business.Services
{
    public class CandidateBL : ICandidateBL
    {
        #region Global Variables
        private readonly ICandidateRepository _repoCandidate;
        private readonly IUtils _utils;
        private readonly IUriservice _uriService;

        #endregion
        #region Constructor Method
        public CandidateBL(ICandidateRepository repoCandidate, IUtils utils, IUriservice uriServices)
        {
            _repoCandidate = repoCandidate;
            _utils = utils;
            _uriService = uriServices;
        }
        #endregion
        #region Public Methods
        public async Task<CandidateResponse> CreateCandidateService(CandidateRequest request)
        {
            try
            {
                if (!await ValidateFields(request))
                {
                    return new CandidateResponse
                    {
                        IdCandidate = null,
                        MessageResponse = new MessageResponse
                        {
                            message = "One or more fields do not have the allowed value",
                            success = false
                        }
                    };
                }

                int idExist = await IsExistCandidate(request, _uriService, string.Empty);

                if (idExist > 0)
                {
                    return new CandidateResponse
                    {
                        IdCandidate = idExist,
                        MessageResponse = new MessageResponse
                        {
                            message = "Candidate is already registered with this email.",
                            success = false
                        }
                    };

                }

                var entity = await _utils.MapperCandidateModelToEntity(request, true);

                var objResult = await _repoCandidate.Create(entity);

                if (objResult != null)
                {
                    return new CandidateResponse
                    {
                        IdCandidate = objResult.IdCandidate,
                        MessageResponse = new MessageResponse { message = "Candidate successfully registered.", success = true }
                    };
                }
                else
                    throw new Exception("Could not create Candidate.");
            }
            catch (Exception ex)
            {
                return new CandidateResponse
                {
                    IdCandidate = null,
                    MessageResponse = new MessageResponse { message = ex.Message, success = false }
                };
            }
        }
        public async Task<CandidateReadRequest?> GetCandidateService(int idCandidate)
        {
            try
            {
                var entity = await _repoCandidate.Read(idCandidate);
                if (entity != null)
                    return  await _utils.MapperCandidateEntitytoModel(entity);

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in the candidate Read service:" + ex.Message, ex);
            }
        }

        public async Task<CandidateResponse> UpdateCandidateService(int idCandidate, CandidateUpdateRequest updateRequest)
        {
            try
            {
                var objCandidate = await GetCandidateService(idCandidate);
                if (objCandidate == null)
                {
                    return new CandidateResponse
                    {
                        IdCandidate = null,
                        MessageResponse = new MessageResponse { message = "Candidate NotFound.", success = false }
                    };
                }
                else
                {
                    if (objCandidate.IdCandidate != idCandidate)
                    {
                        return new CandidateResponse
                        {
                            IdCandidate = null,
                            MessageResponse = new MessageResponse { message = "Id Candidate Not Match.", success = false }
                        };
                    }

                    if (objCandidate.Email != updateRequest.Email)
                    {
                        return new CandidateResponse
                        {
                            IdCandidate = null,
                            MessageResponse = new MessageResponse { message = "Email cannot be updated!", success = false }
                        };
                    }
                }

                var entity = await _utils.MapperCandidateUpdateToEntity(objCandidate, updateRequest);

                var result = await _repoCandidate.Update(idCandidate, entity);

                return new CandidateResponse
                {
                    IdCandidate = result.IdCandidate,
                    MessageResponse = new MessageResponse { message = "Candidate successfully updated.", success = true }
                };
            }
            catch (Exception ex)
            {
                return new CandidateResponse
                {
                    IdCandidate = null,
                    MessageResponse = new MessageResponse { message = ex.Message, success = false }
                };
            }
        }

        public async Task<Paged<CandidateReadRequest>> GetAllCandidatesService(CandidateRequest searchRequest, Paginator paginator, Sorter sorter)
        {
            try
            {
                return await Search(searchRequest, paginator, sorter, _uriService, false, string.Empty);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in the candidate GetAll service:" + ex.Message, ex);
            }
        }
        public async Task<Paged<CandidateReadRequest>> Search(CandidateRequest searchRequest, Paginator paginator, Sorter sorter, IUriservice uriservice, bool validateEmail, string route)
        {
            try
            {
                int totalRecords;
                if (string.IsNullOrWhiteSpace(sorter.SortBy) || sorter.SortBy == "id")
                {
                    sorter.SortBy = "email";
                }
                var entities = await _repoCandidate.Search(searchRequest, paginator, sorter, validateEmail);

                var candidateRead = await _utils.MapperCandidateListEntitytoModel(entities);

                if (validateEmail)
                    totalRecords = entities.Count;
                else
                    totalRecords = await _repoCandidate.TotalCount(searchRequest);

                var page = PaginationHelper.CreatePagedReponse<CandidateReadRequest>(candidateRead, paginator, totalRecords, uriservice, route);
                return page;

            }
            catch (Exception ex)
            {
                throw new Exception("Error in the candidate search service:" + ex.Message, ex);
            }
        }

        public async Task<string> DeleteCandidateService(int idCandidate)
        {
            try
            {
                var response = GetCandidateService(idCandidate);
                if (response != null)
                {
                    await _repoCandidate.Delete(idCandidate);
                    return "Deleted";
                }

                return "NotFound";
            }
            catch (Exception ex)
            {
                throw new Exception("Error in the candidate delete service:" + ex.Message, ex);
            }

        }

       

        #endregion

        #region Private Methods
        private async Task<bool> ValidateFields(CandidateRequest obj)
        {
            bool result = true;
            if (string.IsNullOrEmpty(obj.Name)) result = false;
            if (string.IsNullOrEmpty(obj.Surname)) result = false;
            if (!await _utils.isValidEmail(obj.Email)) result = false;
            if (!await _utils.isValidDatetime(obj.Birthdate.ToString())) result = false;

            return await Task.FromResult(result);
        }
        private async Task<int> IsExistCandidate(CandidateRequest request, IUriservice uriservice, string route)
        {
            try
            {
                Paginator paginator = new() { PageNumber = 1, PageSize = 1 };

                Sorter sorter = new() { SortBy = "email", SortOrder = "asc" };

                var candidateRead = await Search(request, paginator, sorter, uriservice, true, route);

                //Exist Candidate
                if (candidateRead.Items != null && candidateRead.Items.Count > 0 && candidateRead.Items.First().Email.Equals(request.Email.ToUpper()))
                    return candidateRead.Items.First().IdCandidate;
                else //Not Exist Cantidate
                    return 0;

            }
            catch (Exception ex)
            {
                throw new Exception("Error in the candidate search service:" + ex.Message, ex);
            }
        }


        #endregion
    }
}
