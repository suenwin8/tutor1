using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using tutor1.Extension;
using tutor1.Models.Context;
using tutor1.Models.Entity;
using tutor1.Services;

namespace tutor1.Controllers
{
    public class ClinicOrdersController : Controller
    {
        private readonly ClinicContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;


        public ClinicOrdersController(ClinicContext context, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            var baseUrl = _httpContextAccessor.HttpContext?.Request.BaseUrl();
            ViewData["contentPath"] = baseUrl;
            _configuration = configuration;
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
                .Include(c => c.OrderDetails).ThenInclude(d => d.product)
                .FirstOrDefaultAsync(m => m.ClinicOrderId == id);
            if (clinicOrder == null)
            {
                return NotFound();
            }

            return View(clinicOrder);
        }

        // GET: ClinicOrders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ClinicOrders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClinicOrderId,clinicOrder_seqid,DateOfClinicOrder,seeDoctor,customer,Amount,LastUpdatedTime")] ClinicOrder clinicOrder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(clinicOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(clinicOrder);
        }

        // GET: ClinicOrders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            
            if (id == null)
            {
                return NotFound();
            }

            var clinicOrder = await _context.ClinicOrders.Include(c=>c.OrderDetails).ThenInclude(d=>d.product).FirstOrDefaultAsync(m => m.ClinicOrderId == id);
            if (clinicOrder == null)
            {
                return NotFound();
            }
            return View(clinicOrder);
        }

        // POST: ClinicOrders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ClinicOrderId,clinicOrder_seqid,DateOfClinicOrder,seeDoctor,customer,Amount,LastUpdatedTime,OrderDetails[]")] ClinicOrder clinicOrder)
        {
            if (id != clinicOrder.ClinicOrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(clinicOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClinicOrderExists(clinicOrder.ClinicOrderId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                //return RedirectToAction(nameof(Index));
                clinicOrder = await _context.ClinicOrders.Include(c => c.OrderDetails).ThenInclude(d => d.product).FirstOrDefaultAsync(m => m.ClinicOrderId == id);
                ViewBag.Message = DisplayMessage.ShowAlert(Alerts.Success, _configuration["HTMLDisplayWording:AlertMessage:Success"]);                                
            }
            
            return View(clinicOrder);
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

            return View(clinicOrder);
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
