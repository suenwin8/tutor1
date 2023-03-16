using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using tutor1.Models.Context;
using tutor1.Models.DTO;
using tutor1.Models.Entity;
using tutor1.Models.Enum;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace tutor1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClinicOrderDetailController : ControllerBase
    {
        private readonly ClinicContext _context;

        public ClinicOrderDetailController(ClinicContext context)
        {
            _context = context;
        }
        // GET: api/<ClinicOrderDetailController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ClinicOrderDetail>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ClinicOrderDetail>
        [HttpPost]
        public async Task<ActionResult<List<ClinicOrderDetailDTO>>> Post([FromBody] List<ClinicOrderDetailDTO> detail)
        {
            var result = detail;
            return Ok(JsonConvert.SerializeObject(result));
        }

        // PUT api/<ClinicOrderDetail>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}
        // PUT: api/<ClinicOrderDetail>/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, List<ClinicOrderDetail> detail)
        {
            if (id != detail.First().ClinicOrderID)
            {
                return BadRequest();
            }

            _context.Entry(detail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderDetailExists(id,detail.First().ClinicOrderDetailID))
                {
                    return NotFound(ErrorCode.RecordNotFound.ToString());
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE api/<ClinicOrderDetailController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        private bool OrderDetailExists(int orderid, int detailid)
        {
            return _context.ClinicOrderDetails.Any(e => e.ClinicOrderID == orderid && e.ClinicOrderDetailID==detailid);
        }
    }
}
