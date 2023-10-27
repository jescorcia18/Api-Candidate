using Microsoft.AspNetCore.Mvc;
using TestPandape.Business.IServices;
using TestPandape.Entity.Candidates;
using TestPandape.Entity.Pagination;

namespace TestPandape.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandidatesController : ControllerBase
    {

        #region Global Variables
        private readonly ICandidateBL _candidateBl;
        #endregion
        #region Constructor Method
        public CandidatesController(ICandidateBL candidateBl)
        {
            _candidateBl = candidateBl;
        }
        #endregion
        #region Controllers
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody] CandidateRequest request)
        {
            try
            {
                return Ok(await _candidateBl.CreateCandidateService(request));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] CandidateRequest searchRequest, [FromQuery] Paginator paginator, [FromQuery] Sorter sorter)
        {
            try
            {
               return Ok(await _candidateBl.GetAllCandidatesService(searchRequest, paginator, sorter));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("Get/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var result = await _candidateBl.GetCandidateService(id);
                if (result != null)
                    return Ok(result);
                else
                    return Ok("Candidate NotFound.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CandidateUpdateRequest updateRequest)
        {
            try
            {
                return Ok(await _candidateBl.UpdateCandidateService(id, updateRequest));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _candidateBl.DeleteCandidateService(id);

                if (result == "Deleted")
                    return Ok("candidate successfully eliminated.");
                else if(result == "NotFound")
                    return Ok("Candidate was not found.");
                else
                    return BadRequest("Candidate could not be eliminated.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion
    }
}
