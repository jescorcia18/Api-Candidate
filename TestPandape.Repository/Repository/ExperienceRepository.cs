using Microsoft.EntityFrameworkCore;
using TestPandape.Entity.Experiences;
using TestPandape.Entity.Pagination;
using TestPandape.Repository.DataModel;
using TestPandape.Repository.DBContext;
using TestPandape.Repository.IRepository;

namespace TestPandape.Repository.Repository
{
    public class ExperienceRepository : IExperienceRepository
    {
        private readonly DatabaseContext _context;

        #region Constructor Methods
        public ExperienceRepository(DatabaseContext context)
        {
            _context = context;
        }
        #endregion

        #region Public Methods
        public async Task<CandidateExperiencesDataModel> Create(CandidateExperiencesDataModel entity)
        {
            try
            {
                var result = await _context.AddAsync(entity);
                await _context.SaveChangesAsync();
                return result.Entity;

            }
            catch (Exception ex)
            {
                throw new Exception($"Error in Repository: {ex.Message}");
            }
        }

        public async Task<CandidateExperiencesDataModel?> Read(int id)
        {
            try
            {
                return await _context.Experiences.Where(c => c.IdCandidateExperience == id).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in Repository: {ex.Message}");
            }
        }

        public async Task<CandidateExperiencesDataModel> Update(int id, CandidateExperiencesDataModel entity)
        {
            try
            {
                if (await _context.Experiences.FindAsync(id) is CandidateExperiencesDataModel found)
                {
                    _context.Entry(found).CurrentValues.SetValues(entity);
                    await _context.SaveChangesAsync();
                }
                return entity;

            }
            catch (Exception ex)
            {
                throw new Exception($"Error in Repository: {ex.Message}");
            }
        }

        public async Task Delete(int id)
        {
            try
            {
                var entity = await Read(id);
                if (entity != null)
                {
                    await Task.FromResult(_context.Experiences.Remove(entity));
                    await _context.SaveChangesAsync();
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Error in Repository: {ex.Message}");
            }
        }
        public async Task<List<CandidateExperiencesDataModel>> Search(ExperienceRequest searchRequest, Paginator paginator, Sorter sorter)
        {
            try
            {
                var query = await GetQuery(searchRequest);
                query = await SortQuery(query, sorter);

                return await query.Skip((paginator.PageNumber - 1) * paginator.PageSize)
                   .Take(paginator.PageSize).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in Repository: {ex.Message}");
            }
        }

        public async Task<int> TotalCount(ExperienceRequest searchRequest)
        {
            return await GetQuery(searchRequest).Result.CountAsync();
        }
        #endregion

        #region Private Methods
        private async Task<IQueryable<CandidateExperiencesDataModel>> GetQuery(ExperienceRequest searchRequest)
        {
            var query = await Task.Run(() => from Experience in _context.Experiences select Experience);

            if (searchRequest.IdCandidate>0)
                query = query.Where(c => c.IdCandidate.Equals(searchRequest.IdCandidate));

            if (!string.IsNullOrEmpty(searchRequest.Company))
                query = query.Where(c => c.Company.ToLower().Trim().Contains(searchRequest.Company.ToLower().Trim()));

            if (!string.IsNullOrEmpty(searchRequest.Job))
                query = query.Where(c => c.Job.ToLower().Trim().Contains(searchRequest.Job.ToLower().Trim()));

            if (!string.IsNullOrEmpty(searchRequest.Description))
                query = query.Where(c => c.Description.ToLower().Trim().Contains(searchRequest.Description.ToLower().Trim()));

            if (searchRequest.Salary>0)
                query = query.Where(c => c.Salary.Equals(searchRequest.Salary));

            if (searchRequest.BeginDate > DateTime.MinValue)
                query = query.Where(c => c.BeginDate.Equals(searchRequest.BeginDate));

            if (searchRequest.EndDate > DateTime.MinValue)
                query = query.Where(c => c.EndDate.Equals(searchRequest.EndDate));


            return await Task.FromResult(query);
        }

        private async Task<IQueryable<CandidateExperiencesDataModel>> SortQuery(IQueryable<CandidateExperiencesDataModel> query, Sorter sorter)
        {
            switch (sorter.SortBy)
            {
                case "company":
                    if (sorter.SortOrder.ToLower().Trim().Equals("asc"))
                        query = query.OrderBy(c => c.Company);
                    else
                        query = query.OrderByDescending(c => c.Company);
                    break;

                case "job":
                    if (sorter.SortOrder.ToLower().Trim().Equals("asc"))
                        query = query.OrderBy(c => c.Job);
                    else
                        query = query.OrderByDescending(c => c.Job);
                    break;

                case "description":
                    if (sorter.SortOrder.ToLower().Trim().Equals("asc"))
                        query = query.OrderBy(c => c.Description);
                    else
                        query = query.OrderByDescending(c => c.Description);
                    break;

                case "salary":
                    if (sorter.SortOrder.ToLower().Trim().Equals("asc"))
                        query = query.OrderBy(c => c.Salary);
                    else
                        query = query.OrderByDescending(c => c.Salary);
                    break;

                case "begindate":
                    if (sorter.SortOrder.ToLower().Trim().Equals("asc"))
                        query = query.OrderBy(c => c.BeginDate);
                    else
                        query = query.OrderByDescending(c => c.BeginDate);
                    break;

                case "enddate":
                    if (sorter.SortOrder.ToLower().Trim().Equals("asc"))
                        query = query.OrderBy(c => c.EndDate);
                    else
                        query = query.OrderByDescending(c => c.EndDate);
                    break;
            }

            return await Task.FromResult(query);
        }
        #endregion

    }
}
