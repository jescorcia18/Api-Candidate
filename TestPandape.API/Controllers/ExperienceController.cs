using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestPandape.Business.IServices;
using TestPandape.Entity.Experiences;
using TestPandape.Entity.Pagination;

namespace TestPandape.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExperienceController : ControllerBase
    {
        #region Global Variables
        private readonly IExperienceBL _experienceBl;
        #endregion
        #region Constructor Method
        public ExperienceController(IExperienceBL experienceBl)
        {
            _experienceBl = experienceBl;
        }
        #endregion
        #region Controllers
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody] ExperienceRequest request)
        {
            try
            {
                return Ok(await _experienceBl.CreateExperienceService(request));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] ExperienceRequest searchRequest, [FromQuery] Paginator paginator, [FromQuery] Sorter sorter)
        {
            try
            {
                return Ok(await _experienceBl.GetAllExperienceService(searchRequest, paginator, sorter));

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
                var result = await _experienceBl.GetExperienceService(id);
                if (result != null)
                    return Ok(result);
                else
                    return Ok("Experience NotFound.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ExperienceUpdateRequest updateRequest)
        {
            try
            {
                return Ok(await _experienceBl.UpdateExperienceService(id, updateRequest));
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
                var result = await _experienceBl.DeleteExperienceService(id);

                if (result == "Deleted")
                    return Ok("Experience successfully eliminated.");
                else if (result == "NotFound")
                    return Ok("Experience was not found.");
                else
                    return BadRequest("Experience could not be eliminated.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion
    }
}
