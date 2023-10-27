using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestPandape.Entity.Candidates;
using TestPandape.Repository.DataModel;
using TestPandape.Lib.IUtilities;
using System.Net.Mail;
using TestPandape.Entity.Experiences;
using System.Formats.Asn1;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace TestPandape.Lib.Utilities
{
    public class Utils : IUtils
    {
        #region Mappers Candidate
        public async Task<CandidateDataModel> MapperCandidateModelToEntity(CandidateRequest model, bool isCreate)
        {
            CandidateDataModel entity = new CandidateDataModel
            {
                Name = model.Name,
                Surname = model.Surname,
                Birthdate = model.Birthdate,
                Email = model.Email,
                ModifyDate = isCreate ? null : DateTime.Now
            };
            if (isCreate)
                entity.InsertDate = DateTime.Now;

            if (model.Experience != null)
            {
                entity.Experiences = model.Experience.Select(e => new CandidateExperiencesDataModel
                {
                    Company = e.Company,
                    Job = e.Job,
                    Description = e.Description,
                    Salary = e.Salary,
                    BeginDate = e.BeginDate,
                    EndDate = e.EndDate,
                    ModifyDate = isCreate ? null : DateTime.Now
                }).ToList();

                if (isCreate)
                    entity.Experiences.ToList().ForEach(e => e.InsertDate = DateTime.Now);
            }


            return await Task.FromResult(entity);
        }
        public async Task<CandidateDataModel> MapperCandidateReadToEntity(CandidateReadRequest model, bool isCreate)
        {
            CandidateDataModel entity = new CandidateDataModel
            {
                Name = model.Name,
                Surname = model.Surname,
                Birthdate = model.Birthdate,
                Email = model.Email,
                ModifyDate = isCreate ? null : DateTime.Now
            };
            if (isCreate)
                entity.InsertDate = DateTime.Now;

            if (model.Experiences != null)
            {
                entity.Experiences = model.Experiences.Select(e => new CandidateExperiencesDataModel
                {
                    Company = e.Company,
                    Job = e.Job,
                    Description = e.Description,
                    Salary = e.Salary,
                    BeginDate = e.BeginDate,
                    EndDate = e.EndDate,
                    ModifyDate = isCreate ? null : DateTime.Now
                }).ToList();

                if (isCreate)
                    entity.Experiences.ToList().ForEach(e => e.InsertDate = DateTime.Now);
            }


            return await Task.FromResult(entity);
        }
        public async Task<List<CandidateReadRequest>> MapperCandidateListEntitytoModel(List<CandidateDataModel> entity)
        {
            List<CandidateReadRequest> model = new List<CandidateReadRequest>();
            foreach (var obj in entity)
            {
                model.Add(new CandidateReadRequest
                {
                    IdCandidate = obj.IdCandidate,
                    Name = obj.Name,
                    Surname = obj.Surname,
                    Birthdate = obj.Birthdate,
                    Email = obj.Email,
                    InsertDate = obj.InsertDate,
                    ModifyDate = obj.ModifyDate,
                    Experiences =obj.Experiences !=null ? obj.Experiences.Select(e => new ExperienceReadRequest
                    {
                        IdCandidateExperience = e.IdCandidateExperience,
                        IdCandidate = e.IdCandidate,
                        Company = e.Company,
                        Job = e.Job,
                        Description = e.Description,
                        Salary= e.Salary,
                        BeginDate = e.BeginDate,
                        EndDate = e.EndDate,
                        InsertDate = e.InsertDate,
                        ModifyDate = e.ModifyDate
                    }).ToList() : null
                });
            }
            return await Task.FromResult(model);
        }
        public async Task<CandidateReadRequest> MapperCandidateEntitytoModel(CandidateDataModel entity)
        {
            CandidateReadRequest model = new CandidateReadRequest();
            model = new CandidateReadRequest
            {
                IdCandidate = entity.IdCandidate,
                Name = entity.Name,
                Surname = entity.Surname,
                Birthdate = entity.Birthdate,
                Email = entity.Email,
                InsertDate = entity.InsertDate,
                ModifyDate = entity.ModifyDate,
                Experiences = entity.Experiences.Select(e => new ExperienceReadRequest
                {
                    IdCandidateExperience = e.IdCandidateExperience,
                    IdCandidate = e.IdCandidate,
                    Company = e.Company,
                    Job = e.Job,
                    Description = e.Description,
                    Salary = e.Salary,
                    BeginDate = e.BeginDate,
                    EndDate = e.EndDate,
                    InsertDate = e.InsertDate,
                    ModifyDate = e.ModifyDate
                }).ToList()
            };
            return await Task.FromResult(model);
        }
        public async Task<CandidateDataModel> MapperCandidateUpdateToEntity(CandidateReadRequest modelRead, CandidateUpdateRequest modelUpdate)
        {
            CandidateDataModel entity = new CandidateDataModel
            {
                IdCandidate = modelRead.IdCandidate,
                Name = modelUpdate.Name,
                Surname = modelUpdate.Surname,
                Birthdate = modelUpdate.Birthdate,
                Email = modelUpdate.Email,
                InsertDate = modelRead.InsertDate,
                ModifyDate = DateTime.Now
            };
            return await Task.FromResult(entity);
        }
        #endregion

        #region Mappers Experience
        public async Task<List<ExperienceReadRequest>> MapperExperienceListEntitytoModel(List<CandidateExperiencesDataModel> entity)
        {
            List<ExperienceReadRequest> model = new List<ExperienceReadRequest>();
            foreach (var obj in entity)
            {
                model.Add(new ExperienceReadRequest
                {
                    IdCandidateExperience=obj.IdCandidateExperience,
                    IdCandidate = obj.IdCandidate,
                    Company = obj.Company,
                    Job = obj.Job,
                    Description = obj.Description,
                    Salary = obj.Salary,
                    BeginDate = obj.BeginDate,
                    EndDate = obj.EndDate,
                    InsertDate = obj.InsertDate,
                    ModifyDate = obj.ModifyDate
                });
            }
            return await Task.FromResult(model);
        }

        public async Task<CandidateExperiencesDataModel> MapperExperienceReadToEntity(ExperienceReadRequest modelRead, bool isCreate)
        {
            CandidateExperiencesDataModel entity = new CandidateExperiencesDataModel
            {
                IdCandidate = modelRead.IdCandidate,
                Company = modelRead.Company,
                Job = modelRead.Job,
                Description = modelRead.Description,
                Salary = modelRead.Salary,
                BeginDate = modelRead.BeginDate,
                EndDate = modelRead.EndDate,
                ModifyDate = DateTime.Now
            };
            if (isCreate)
                entity.InsertDate = DateTime.Now;

            return await Task.FromResult(entity);
        }
        public async Task<CandidateExperiencesDataModel> MapperExperienceModelToEntity(ExperienceRequest model, bool isCreate)
        {
            CandidateExperiencesDataModel entity = new CandidateExperiencesDataModel
            {
                IdCandidate = model.IdCandidate,
                Company = model.Company,
                Job = model.Job,
                Description = model.Description,
                Salary = model.Salary,
                BeginDate = model.BeginDate,
                EndDate = model.EndDate,
                ModifyDate = isCreate ? null : DateTime.Now
            };
            if (isCreate)
                entity.InsertDate = DateTime.Now;

            return await Task.FromResult(entity);
        }
        public async Task<ExperienceReadRequest> MapperExperienceEntitytoModel(CandidateExperiencesDataModel entity)
        {
            ExperienceReadRequest model = new ExperienceReadRequest();
            model = new ExperienceReadRequest
            {
                IdCandidateExperience=entity.IdCandidateExperience,
                IdCandidate = entity.IdCandidate,
                Company = entity.Company,
                Job = entity.Job,
                Description = entity.Description,
                Salary = entity.Salary,
                BeginDate = entity.BeginDate,
                EndDate = entity.EndDate,
                InsertDate = entity.InsertDate,
                ModifyDate = entity.ModifyDate,
            };
            return await Task.FromResult(model);
        }
        public async Task<CandidateExperiencesDataModel> MapperExperienceUpdateToEntity(ExperienceReadRequest modelRead, ExperienceUpdateRequest modelUpdate)
        {
            CandidateExperiencesDataModel entity = new CandidateExperiencesDataModel
            {
                IdCandidateExperience=modelRead.IdCandidateExperience,
                IdCandidate = modelRead.IdCandidate,
                Company = modelUpdate.Company,
                Job = modelUpdate.Job,
                Description = modelUpdate.Description,
                Salary = modelUpdate.Salary,
                BeginDate = modelUpdate.BeginDate,
                EndDate = modelUpdate.EndDate,
                InsertDate = modelRead.InsertDate,
                ModifyDate = DateTime.Now
            };
            return await Task.FromResult(entity);
        }
        #endregion

        #region Validations
        public async Task<bool> isValidEmail(string email)
        {
            try
            {
                MailAddress m = new MailAddress(email);

                return await Task.FromResult(true);
            }
            catch (FormatException)
            {
                return await Task.FromResult(false);
            }
        }
        public async Task<bool> isValidDatetime(string dateTime)
        {
            DateTime temp;
            if (DateTime.TryParse(dateTime, out temp))
                return await Task.FromResult(true);
            else
                return await Task.FromResult(false);
        }

        public async Task<bool> isValidDecimal(string valueDecimal)
        {
            decimal value;
            if (Decimal.TryParse(valueDecimal, out value))
                return await Task.FromResult(true);
            else
                return await Task.FromResult(false);

            //ToDo: verificar que tenga solo 2 decimales
        }
        #endregion

    }
}
