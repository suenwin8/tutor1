using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using tutor1.Models.DTO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace tutor1.Controllers.APIController
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClinicOrderAPIController : ControllerBase
    {
        // GET: api/<ClinicOrderAPIController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ClinicOrderAPIController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ClinicOrderAPIController>
        [HttpPost]
        public ActionResult<ClinicOrderDTO> Post([FromBody] ClinicOrderDTO value)
        {

            return Ok(value);
        }


        // PUT api/<ClinicOrderAPIController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ClinicOrderAPIController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
