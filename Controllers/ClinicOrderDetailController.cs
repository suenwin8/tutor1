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
        // GET: api/<ClinicOrderDetail>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClinicOrderDetailDTO>>> GetDetails()
        {
            return Ok(MapDetailByID(null));
        }

        // GET: api/<ClinicOrderDetail>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ClinicOrderDetail>> GetDetailsByOrderID(int id)
        {
            var detail = await _context.ClinicOrderDetails
                .Include(d=>d.product)
                .Include(d=>d.Order)
                .Where(i => i.ClinicOrderID == id).ToListAsync();
            if (detail == null)
            {
                return NotFound(ErrorCode.RecordNotFound.ToString());
            }

            return Ok(detail);
        }

        // POST api/<ClinicOrderDetail>
        [HttpPost]
        public async Task<ActionResult<List<ClinicOrderDetail>>> Post([FromBody] List<ClinicOrderDetail> detail)
        {           
            if (detail != null)
            {
                try
                {
                    await DeleteActionAsync(detail.First().ClinicOrderID,null);
                    await _context.ClinicOrderDetails.AddRangeAsync(detail);
                    //foreach (ClinicOrderDetail row in detail)
                    //{                        
                    //    _context.ClinicOrderDetails.Add(row);
                    //}                
                    var result = await _context.SaveChangesAsync(); 
                }
                catch (DbUpdateConcurrencyException ex)
                {
                        throw ex.InnerException;                    
                }
            }
            return Ok(JsonConvert.SerializeObject(detail));
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
        public async Task<IActionResult> Put(int id, ClinicOrderDetail detail)
        {
            if (id != detail.ClinicOrderID)
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
                if (!OrderDetailExists(id,detail.ClinicOrderDetailID))
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


        #region DeleteActionAsync
        public async Task<ActionResult<ClinicOrderDetail>> DeleteActionAsync(int orderID, int? detailID)
        {
            int _detailID = detailID.HasValue ? detailID.Value : 0;
            if (orderID != 0 && _detailID != 0)
                await DeleteByOrderDetailIDAsync(orderID, _detailID);
            else
                await DeleteByOrderIDAsync(orderID);            
            return Ok();
        }

        public async Task<ActionResult<ClinicOrderDetail>> DeleteByOrderIDAsync(int orderID)
        {
            var detail = await _context.ClinicOrderDetails.Where(i => i.ClinicOrderID == orderID).ToListAsync();
            if (detail == null)
            {
                return NotFound(ErrorCode.RecordNotFound.ToString());
            }
            foreach (ClinicOrderDetail row in detail)
            {
                await DeleteByOrderDetailIDAsync(row.ClinicOrderID, row.ClinicOrderDetailID);
            }
            return Ok();
        }
        public async Task<ActionResult<ClinicOrderDetail>> DeleteByOrderDetailIDAsync(int orderID, int detailID)
        {
            var detail = await _context.ClinicOrderDetails.FirstOrDefaultAsync(i => i.ClinicOrderDetailID == detailID && i.ClinicOrderID == orderID);
            if (detail == null)
            {
                return NotFound(ErrorCode.RecordNotFound.ToString());
            }

            _context.ClinicOrderDetails.Remove(detail);
            //await _context.SaveChangesAsync();
            return detail;
        }

        // DELETE api/<ClinicOrderDetailController>/DeleteItem
        [HttpDelete("DeleteItem")]
        public async Task<ActionResult<ClinicOrderDetail>> DeleteItem(int orderID, int? detailID)
        {
            await DeleteActionAsync(orderID, detailID);
            await _context.SaveChangesAsync();
            return Ok();
        }
        #endregion

        // Project products to product DTOs.
        private IEnumerable<ClinicOrderDetailDTO> MapDetailByID(ClinicOrderDetailDTO detailDTO) 
        {
            if (detailDTO != null)
            {
                return from p in _context.ClinicOrderDetails.Include(d => d.product)
                       where p.ClinicOrderDetailID == detailDTO.ClinicOrderDetailID && p.ClinicOrderID == detailDTO.ClinicOrderID
                       select new ClinicOrderDetailDTO()
                       {
                           ClinicOrderDetailID = p.ClinicOrderDetailID,
                           ClinicOrderID = p.ClinicOrderID,
                           Price = p.Price,
                           Quantity = p.Quantity,
                           product = p.product,
                           ProductID = p.ProductID
                       };
            }
            else
            {
                return from p in _context.ClinicOrderDetails.Include(d => d.product)                       
                       select new ClinicOrderDetailDTO()
                       {
                           ClinicOrderDetailID = p.ClinicOrderDetailID,
                           ClinicOrderID = p.ClinicOrderID,
                           Price = p.Price,
                           Quantity = p.Quantity,
                           product = p.product,
                           ProductID = p.ProductID
                       };
            }            
        }

        private bool OrderDetailExists(int orderid, int detailid)
        {
            return _context.ClinicOrderDetails.Any(e => e.ClinicOrderID == orderid && e.ClinicOrderDetailID==detailid);
        }
    }
}
