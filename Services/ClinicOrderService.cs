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
        public Task<List<AppSetting>> GetNewAppSetting(string variable_name,CancellationToken cancellationToken = default);
        public string GetNewOrderSeqID(int seqid, CancellationToken cancellationToken = default);
        //Task<T> GetAllAsync<T>(CancellationToken cancellationToken = default) where T : class, new();

        //Task<T> GetByIDAsync<T>(int id, CancellationToken cancellationToken = default) where T : class, new();
        ////Task<T> GetByConditionAsync<T>(HolidayParameters dto, CancellationToken cancellationToken = default) where T : class, new();
        //Task UpdateAsync<T>(ClinicOrder order, CancellationToken cancellationToken = default) where T : class, new();
        //Task DeleteAsync<T>(string id, CancellationToken cancellationToken = default) where T : class, new();
    }
    public class ClinicOrderDetailService : IClinicOrderService
    {
        
        private ClinicContext _context;
        
        public ClinicOrderDetailService(ClinicContext Context)
        {           
            _context = Context;            
        }

        public async Task<List<AppSetting>> GetNewAppSetting(string variable_name, CancellationToken cancellationToken = default)
        {
            var appSetting = _context.appSettings.Where(a => a.VARGROUP == variable_name).ToList();
            foreach (AppSetting row in appSetting)
            {
                row.INTVALUE += 1;
                _context.Update(row);
            }           
            await _context.SaveChangesAsync();

            return appSetting;
        }
        public string GetNewOrderSeqID(int seqid, CancellationToken cancellationToken = default)
        {
            return DateTime.Now.ToString("yyyyMMdd") + seqid.ToString().PadLeft(5,'0');
        }


        //public Task DeleteAsync<T>(string id, CancellationToken cancellationToken = default) where T : class, new()
        //{
        //    throw new NotImplementedException();
        //}
        //#region DeleteActionAsync
        //public async Task<ClinicOrderDetail> DeleteActionAsync(int orderID, int? detailID)
        //{
        //    int _detailID = detailID.HasValue ? detailID.Value : 0;
        //    if (orderID != 0 && _detailID != 0)
        //        return await DeleteByOrderDetailIDAsync(orderID, _detailID);
        //    else
        //        return await DeleteByOrderIDAsync(orderID);

        //}

        //public async Task<ActionResult<ClinicOrderDetail>> DeleteByOrderIDAsync(int orderID)
        //{
        //    var detail = await _context.ClinicOrderDetails.Where(i => i.ClinicOrderID == orderID).ToListAsync();
        //    if (detail == null)
        //    {
        //        return NotFound(ErrorCode.RecordNotFound.ToString());
        //    }
        //    foreach (ClinicOrderDetail row in detail)
        //    {
        //        await DeleteByOrderDetailIDAsync(row.ClinicOrderID, row.ClinicOrderDetailID);
        //    }
        //    return Ok();
        //}
        //public async Task<ActionResult<ClinicOrderDetail>> DeleteByOrderDetailIDAsync(int orderID, int detailID)
        //{
        //    var detail = await _context.ClinicOrderDetails.FirstOrDefaultAsync(i => i.ClinicOrderDetailID == detailID && i.ClinicOrderID == orderID);
        //    if (detail == null)
        //    {
        //        return NotFound(ErrorCode.RecordNotFound.ToString());
        //    }

        //    _context.ClinicOrderDetails.Remove(detail);
        //    //await _context.SaveChangesAsync();
        //    return detail;
        //}

        //// DELETE api/<ClinicOrderDetailController>/DeleteItem
        //[HttpDelete("DeleteItem")]
        //public async Task<ActionResult<ClinicOrderDetail>> DeleteItem(int orderID, int? detailID)
        //{
        //    await DeleteActionAsync(orderID, detailID);
        //    await _context.SaveChangesAsync();
        //    return Ok();
        //}
        //#endregion

        //public Task<T> GetAllAsync<T>(CancellationToken cancellationToken = default) where T : class, new()
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<T> GetByIDAsync<T>(int id, CancellationToken cancellationToken = default) where T : class, new()
        //{
        //    throw new NotImplementedException();
        //}

        //public Task UpdateAsync<T>(ClinicOrder order, CancellationToken cancellationToken = default) where T : class, new()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
