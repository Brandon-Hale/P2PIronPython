using DataWebService.Data;
using DataWebService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DataWebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        [HttpGet]
        [Route("getall")]
        public IEnumerable<Job> GetAll()
        {
            return JobManager.GetAll();
        }

        [HttpGet]
        [Route("id/{id}")]
        public IActionResult GetByID(int id)
        {
            Job job = JobManager.GetByID(id);
            if (job == null)
            {
                return NotFound();
            }
            return Ok(job);
        }

        [HttpPost]
        [Route("post")]
        public IActionResult PostJob([FromBody] Job job)
        {
            if (JobManager.Insert(job))
            {
                return Ok("Successfully Inserted");
            }
            return BadRequest("Error in insertion");
        }

        [HttpPut]
        [Route("update")]
        public IActionResult UpdateJob(Job job)
        {
            if (JobManager.Update(job))
            {
                return Ok("Successfully Updated");
            }
            return BadRequest("Error in update");
        }

        [HttpDelete]
        [Route("delete")]
        public IActionResult DeleteJob(int id)
        {
            if (JobManager.Delete(id))
            {
                return Ok("Successfully Deleted");
            }
            return BadRequest("Could not delete");
        }
    }
}
