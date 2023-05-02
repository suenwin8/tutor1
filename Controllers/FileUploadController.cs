using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using tutor1.Models.Context;
using tutor1.Models.DTO;
using tutor1.Models.Entity;
using tutor1.Services;

namespace tutor1.Controllers
{
    public class FileUploadController : Controller
    {
        private readonly ClinicContext _context;
        private readonly IFileUploadService _fileUploadService;
        private readonly IConfiguration _configuration;
        private readonly long _fileSizeLimit;
        private readonly string[] _permittedExtensions = { ".txt" };
        public FileUploadController(ClinicContext context, IFileUploadService fileUploadService, IConfiguration configuration)
        {
            _context = context;
            _fileUploadService = fileUploadService;
            _configuration = configuration;
            _fileSizeLimit = _configuration.GetValue<long>("FileSizeLimit");
        }

        // GET: FileUpload
        public async Task<IActionResult> Index()
        {
            var fileuploadViewModel = await LoadAllFiles();
            ViewBag.Message = TempData["Message"];
            return View(fileuploadViewModel);
        }

        private async Task<FileUploadViewModel> LoadAllFiles()
        {
            var viewModel = new FileUploadViewModel();            
            viewModel.FilesOnFileSystem = await _context.FileOnFileSystemModels.ToListAsync();
            return viewModel;
        }

        [HttpPost]
        public async Task<IActionResult> UploadToFileSystem(List<IFormFile> files, string description)
        {
            var viewModel = new FileUploadViewModel();
            string ErrorMsg = "";
            try {
                viewModel = await _fileUploadService.UploadToFileSystem(files, description,_permittedExtensions, _fileSizeLimit);
            }
            catch (Exception ex)
            {
                ErrorMsg = ex.Message;
            }
            
            if (viewModel !=null && ErrorMsg=="")
            {
                TempData["Message"] = "File successfully uploaded to File System.";
            }
            else
            {
                TempData["Message"] = ErrorMsg;
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DownloadFileFromFileSystem(int id)
        {
            var file = await _fileUploadService.GetByID(id);
            if (file == null) return null;
            var memory = new MemoryStream();
            using (var stream = new FileStream(file.FilePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, file.FileType, file.Name + file.Extension);
        }

        public async Task<IActionResult> DeleteFileFromFileSystem(int id)
        {
            var file = await _fileUploadService.GetByID(id);
            if (file == null) return null;
            if (System.IO.File.Exists(file.FilePath))
            {
                System.IO.File.Delete(file.FilePath);
            }
            _context.FileOnFileSystemModels.Remove(file);
            _context.SaveChanges();
            TempData["Message"] = $"Removed {file.Name + file.Extension} successfully from File System.";
            return RedirectToAction("Index");
        }

        // GET: FileUpload/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fileModel = await _context.FileOnFileSystemModels
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fileModel == null)
            {
                return NotFound();
            }

            return View(fileModel);
        }

        // GET: FileUpload/Create
        public IActionResult Create()
        {
            return View();
        }

        //// POST: FileUpload/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Name,FileType,Extension,Description,UploadedBy,CreatedOn")] FileModel fileModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(fileModel);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(fileModel);
        //}

        //// GET: FileUpload/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var fileModel = await _context.FileOnFileSystemModels.FindAsync(id);
        //    if (fileModel == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(fileModel);
        //}

        //// POST: FileUpload/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,Name,FileType,Extension,Description,UploadedBy,CreatedOn")] FileOnFileSystemModels fileModel)
        //{
        //    if (id != fileModel.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(fileModel);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!FileModelExists(fileModel.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(fileModel);
        //}

        //// GET: FileUpload/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var fileModel = await _context.FileOnFileSystemModels
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (fileModel == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(fileModel);
        //}

        //// POST: FileUpload/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var fileModel = await _context.FileOnFileSystemModels.FindAsync(id);
        //    _context.FileOnFileSystemModels.Remove(fileModel);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool FileModelExists(int id)
        {
            return _context.FileOnFileSystemModels.Any(e => e.Id == id);
        }
    }
}
