using AutoMapper;
using AutoMapper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using tutor1.Models.Context;
using tutor1.Models.Entity;

namespace tutor1.Services
{
    public interface IClinicOrderService
    {
        Task<T> GetAllAsync<T>(CancellationToken cancellationToken = default) where T : class, new();

        Task<T> GetByIDAsync<T>(int id, CancellationToken cancellationToken = default) where T : class, new();
        //Task<T> GetByConditionAsync<T>(HolidayParameters dto, CancellationToken cancellationToken = default) where T : class, new();
        Task UpdateAsync<T>(ClinicOrder order, CancellationToken cancellationToken = default) where T : class, new();
        Task DeleteAsync<T>(string id, CancellationToken cancellationToken = default) where T : class, new();
    }
    //public class ClinicOrderDetailService : IClinicOrderService
    //{
    //    private readonly IConfiguration _configuration;
    //    private ClinicContext _context;  
    //    private IMapper _mapper;
    //    public ClinicOrderDetailService(IConfiguration configuration, ClinicContext Context, IMapper mapper)
    //    {
    //        _configuration = configuration;
    //        _context = Context;
    //        _mapper = mapper;
    //    }
    //    public Task DeleteAsync<T>(string id, CancellationToken cancellationToken = default) where T : class, new()
    //    {
    //        throw new NotImplementedException();
    //    }
    //    #region DeleteActionAsync
    //    public async Task<ClinicOrderDetail> DeleteActionAsync(int orderID, int? detailID)
    //    {
    //        int _detailID = detailID.HasValue ? detailID.Value : 0;
    //        if (orderID != 0 && _detailID != 0)
    //         return   await DeleteByOrderDetailIDAsync(orderID, _detailID);
    //        else
    //         return   await DeleteByOrderIDAsync(orderID);
            
    //    }

    //    public async Task<ActionResult<ClinicOrderDetail>> DeleteByOrderIDAsync(int orderID)
    //    {
    //        var detail = await _context.ClinicOrderDetails.Where(i => i.ClinicOrderID == orderID).ToListAsync();
    //        if (detail == null)
    //        {
    //            return NotFound(ErrorCode.RecordNotFound.ToString());
    //        }
    //        foreach (ClinicOrderDetail row in detail)
    //        {
    //            await DeleteByOrderDetailIDAsync(row.ClinicOrderID, row.ClinicOrderDetailID);
    //        }
    //        return Ok();
    //    }
    //    public async Task<ActionResult<ClinicOrderDetail>> DeleteByOrderDetailIDAsync(int orderID, int detailID)
    //    {
    //        var detail = await _context.ClinicOrderDetails.FirstOrDefaultAsync(i => i.ClinicOrderDetailID == detailID && i.ClinicOrderID == orderID);
    //        if (detail == null)
    //        {
    //            return NotFound(ErrorCode.RecordNotFound.ToString());
    //        }

    //        _context.ClinicOrderDetails.Remove(detail);
    //        //await _context.SaveChangesAsync();
    //        return detail;
    //    }

    //    // DELETE api/<ClinicOrderDetailController>/DeleteItem
    //    [HttpDelete("DeleteItem")]
    //    public async Task<ActionResult<ClinicOrderDetail>> DeleteItem(int orderID, int? detailID)
    //    {
    //        await DeleteActionAsync(orderID, detailID);
    //        await _context.SaveChangesAsync();
    //        return Ok();
    //    }
    //    #endregion

    //    public Task<T> GetAllAsync<T>(CancellationToken cancellationToken = default) where T : class, new()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Task<T> GetByIDAsync<T>(int id, CancellationToken cancellationToken = default) where T : class, new()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Task UpdateAsync<T>(ClinicOrder order, CancellationToken cancellationToken = default) where T : class, new()
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
