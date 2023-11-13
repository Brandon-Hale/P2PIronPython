using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DataWebService.Data;
using DataWebService.Models;

namespace DataWebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        [HttpGet]
        [Route("getall")]
        public IEnumerable<Client> GetAll()
        {
            return ClientManager.GetAll();
        }

        [HttpGet]
        [Route("username/{username}")]
        public IActionResult GetByUsername(string username)
        {
            Client client = ClientManager.GetByUsername(username);
            if (client == null)
            {
                return NotFound();
            }
            return Ok(client);
        }

        [HttpGet]
        [Route("ipaddress/{ipaddress}")]
        public IActionResult GetByIp(string ip)
        {
            Client client = ClientManager.GetByAddress(ip);
            if (client == null)
            {
                return NotFound();
            }
            return Ok(client);
        }

        [HttpPost]
        [Route("post")]
        public IActionResult PostClient([FromBody] Client client)
        {
            if (ClientManager.Insert(client))
            {
                return Ok("Successfully Inserted");
            }
            return BadRequest("Error in insertion");
        }

        [HttpPut]
        [Route("update")]
        public IActionResult UpdateClient(Client client)
        {
            if (ClientManager.Update(client))
            {
                return Ok("Successfully Updated");
            }
            return BadRequest("Error in update");
        }

        [HttpDelete]
        [Route("deletename/{username}")]
        public IActionResult DeleteClientUsername(string username)
        {
            if (ClientManager.DeleteByName(username))
            {
                return Ok("Successfully Deleted");
            }
            return BadRequest("Could not delete");
        }

        [HttpDelete]
        [Route("deleteaddress")]
        public IActionResult DeleteClientIP(string ip)
        {
            if (ClientManager.DeleteByIP(ip))
            {
                return Ok("Successfully Deleted");
            }
            return BadRequest("Could not delete");
        }




    }
}
