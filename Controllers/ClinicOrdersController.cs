using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using tutor1.Extension;
using tutor1.Models.Context;
using tutor1.Models.DTO;
using tutor1.Models.Entity;
using tutor1.Services;

namespace tutor1.Controllers
{
    public class ClinicOrdersController : Controller
    {
        private readonly ClinicContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IClinicOrderService _clinicOrderService;

        public ClinicOrdersController(ClinicContext context, 
            IHttpContextAccessor httpContextAccessor, 
            IConfiguration configuration,
            IMapper mapper,
            IClinicOrderService clinicOrderService
            )
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            var baseUrl = _httpContextAccessor.HttpContext?.Request.BaseUrl();
            ViewData["contentPath"] = baseUrl;
            _configuration = configuration;
            _mapper = mapper;
            _clinicOrderService = clinicOrderService;
        }
        
        
        // GET: ClinicOrders
        public async Task<IActionResult> Index()
        {
            return View(await _context.ClinicOrders.ToListAsync());
        }

        // GET: ClinicOrders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clinicOrder = await _context.ClinicOrders
                .Include(c => c.OrderDetails)
                .ThenInclude(d => d.product)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ClinicOrderId == id);
            if (clinicOrder == null)
            {
                return NotFound();
            }

            ClinicOrderDTO orderDTO = _mapper.Map<ClinicOrder, ClinicOrderDTO>(clinicOrder);

            return View(orderDTO);
        }

        // GET: ClinicOrders/Create
        public async Task<IActionResult> Create()
        {
            var appSetting = await _clinicOrderService.GetNewAppSetting();
            ClinicOrderDTO orderDTO = new ClinicOrderDTO();
            orderDTO.ClinicOrderId = appSetting.ClinicOrderId;
            orderDTO.clinicOrder_seqid = _clinicOrderService.GetNewOrderSeqID(appSetting);
            orderDTO.DateOfClinicOrder = DateTime.Now;
            
            return View(orderDTO);
        }

        // POST: ClinicOrders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClinicOrderDTO view_clinicOrder)
        {
            view_clinicOrder.OrderDetails = JsonConvert.DeserializeObject<List<ClinicOrderDetail>>(view_clinicOrder.json_OrderDetails);
            if (ModelState.IsValid)
            {
                ClinicOrder clinicOrder = _mapper.Map<ClinicOrderDTO,ClinicOrder>(view_clinicOrder);
                clinicOrder.LastUpdatedTime = DateTime.Now;
                _context.Add(clinicOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(view_clinicOrder);
        }

        // GET: ClinicOrders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {            
            if (id == null)
            {
                return NotFound();
            }

            var clinicOrder = await _context.ClinicOrders                
                .Include(c=>c.OrderDetails)                
                .ThenInclude(d=>d.product)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ClinicOrderId == id);
            if (clinicOrder == null)
            {
                return NotFound();
            }

            ClinicOrderDTO orderDTO = _mapper.Map<ClinicOrder, ClinicOrderDTO>(clinicOrder);           

            return View(orderDTO);
        }        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ClinicOrderDTO view_clinicOrder)
        {
            if (view_clinicOrder.ClinicOrderId == int.MinValue)
            {
                return NotFound();
            }                      

            view_clinicOrder.OrderDetails = JsonConvert.DeserializeObject<List<ClinicOrderDetail>>(view_clinicOrder.json_OrderDetails);
            
            if (ModelState.IsValid)
            {
                try
                {
                    //ClinicOrder clinicOrder = new ClinicOrder();
                    //clinicOrder = _mapper.Map<ClinicOrderDTO, ClinicOrder>(view_clinicOrder);
                    ClinicOrder org_clinicOrder = await _context.ClinicOrders
                        .Include(c => c.OrderDetails)                                                
                        .FirstOrDefaultAsync(m => m.ClinicOrderId == id);
                    
                        foreach (ClinicOrderDetail d in org_clinicOrder.OrderDetails)
                        {
                            _context.ClinicOrderDetails.Remove(d);
                        }
                        _context.ClinicOrderDetails.AddRange(view_clinicOrder.OrderDetails);
                    
                                      
                    
                    org_clinicOrder.Amount = view_clinicOrder.Amount;
                    org_clinicOrder.customer = view_clinicOrder.customer;
                    org_clinicOrder.DateOfClinicOrder = view_clinicOrder.DateOfClinicOrder;
                    org_clinicOrder.LastUpdatedTime = DateTime.Now;
                    org_clinicOrder.seeDoctor = view_clinicOrder.seeDoctor;
                    
                    _context.Update(org_clinicOrder);
                    await _context.SaveChangesAsync();
                }
                //catch (DbUpdateConcurrencyException)
                catch (Exception ex)
                {
                    if (!ClinicOrderExists(view_clinicOrder.ClinicOrderId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }               
                ViewBag.Message = DisplayMessage.ShowAlert(Alerts.Success, _configuration["HTMLDisplayWording:AlertMessage:Success"]);
            }
            else
            {
                throw new Exception("Fail to validate the form.");
            }           

            return View(view_clinicOrder);
            //return Json(clinicOrder);
        }

        


        // GET: ClinicOrders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clinicOrder = await _context.ClinicOrders
                .Include(c => c.OrderDetails).ThenInclude(d => d.product)
                .FirstOrDefaultAsync(m => m.ClinicOrderId == id);
            if (clinicOrder == null)
            {
                return NotFound();
            }
            ClinicOrderDTO orderDTO = _mapper.Map<ClinicOrder, ClinicOrderDTO>(clinicOrder);
            return View(orderDTO);
        }

        // POST: ClinicOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var clinicOrder = await _context.ClinicOrders
                .Include(c => c.OrderDetails).ThenInclude(d => d.product).FirstOrDefaultAsync(m => m.ClinicOrderId == id);
            _context.ClinicOrders.Remove(clinicOrder);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClinicOrderExists(int id)
        {
            return _context.ClinicOrders.Any(e => e.ClinicOrderId == id);
        }
    }
}
