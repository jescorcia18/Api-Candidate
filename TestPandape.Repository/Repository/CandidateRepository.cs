using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestPandape.Entity.Candidates;
using TestPandape.Entity.Pagination;
using TestPandape.Repository.DataModel;
using TestPandape.Repository.DBContext;
using TestPandape.Repository.IRepository;

namespace TestPandape.Repository.Repository
{
    public class CandidateRepository : ICandidateRepository
    {
        private readonly DatabaseContext _context;
        #region Constructor Methods
        public CandidateRepository(DatabaseContext context)
        {
            _context = context;
        }
        #endregion

        #region Public Methods
        public async Task<CandidateDataModel> Create(CandidateDataModel entity)
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
        public async Task<CandidateDataModel?> Read(int id)
        {
            try
            {
                return await _context.Candidates.Where(c => c.IdCandidate == id).Include(e => e.Experiences).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in Repository: {ex.Message}");
            }
        }

        public async Task<CandidateDataModel> Update(int id,CandidateDataModel entity)
        {
            try
            {
                if (await _context.Candidates.FindAsync(id) is CandidateDataModel found)
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
                    await Task.FromResult( _context.Candidates.Remove(entity));
                    await _context.SaveChangesAsync();
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Error in Repository: {ex.Message}");
            }
        }

        public async Task<List<CandidateDataModel>> Search(CandidateRequest searchRequest, Paginator paginator, Sorter sorter, bool validateEmail = false)
        {
            try
            {
                var query = await GetQuery(searchRequest, validateEmail);
                query = await SortQuery(query, sorter);

                return await query.Skip((paginator.PageNumber - 1) * paginator.PageSize)
                   .Take(paginator.PageSize).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in Repository: {ex.Message}");
            }
        }

        public async Task<int> TotalCount(CandidateRequest searchRequest)
        {
            return await GetQuery(searchRequest).Result.CountAsync();
        }

        #endregion

        #region Private Methods
        private async Task<IQueryable<CandidateDataModel>> GetQuery(CandidateRequest searchRequest, bool validateEmail = false)
        {
            var query = await Task.Run(() => from Candidate in _context.Candidates
                                             join Experience in _context.Experiences
                                                on Candidate.IdCandidate equals Experience.IdCandidate
                                             select Candidate);

            if (query.ToList().Count() > 0)
            {
                var x = query.Select(a => a.Experiences).ToList();
            }

            if (validateEmail)
            {
                query = query.Where(c => c.Email.ToLower().Trim().Equals(searchRequest.Email.ToLower().Trim()));
                return query;
            }

            if (!string.IsNullOrEmpty(searchRequest.Name))
                query = query.Where(c => c.Name.ToLower().Trim().Contains(searchRequest.Name.ToLower().Trim()));

            if (!string.IsNullOrEmpty(searchRequest.Surname))
                query = query.Where(c => c.Surname.ToLower().Trim().Contains(searchRequest.Surname.ToLower().Trim()));

            if (!string.IsNullOrEmpty(searchRequest.Email))
                query = query.Where(c => c.Email.ToLower().Trim().Contains(searchRequest.Email.ToLower().Trim()));

            return await Task.FromResult(query);
        }

        private async Task<IQueryable<CandidateDataModel>> SortQuery(IQueryable<CandidateDataModel> query, Sorter sorter)
        {
            switch (sorter.SortBy)
            {
                case "name":
                    if (sorter.SortOrder.ToLower().Trim().Equals("asc"))
                        query = query.OrderBy(c => c.Name);
                    else
                        query = query.OrderByDescending(c => c.Name);
                    break;

                case "surname":
                    if (sorter.SortOrder.ToLower().Trim().Equals("asc"))
                        query = query.OrderBy(c => c.Surname);
                    else
                        query = query.OrderByDescending(c => c.Surname);
                    break;

                case "email":
                    if (sorter.SortOrder.ToLower().Trim().Equals("asc"))
                        query = query.OrderBy(c => c.Email);
                    else
                        query = query.OrderByDescending(c => c.Email);
                    break;
            }

            return await Task.FromResult(query);
        }
        #endregion
    }
}
