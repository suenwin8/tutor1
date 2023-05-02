using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using tutor1.Models.Context;
using tutor1.Models.DTO;
using tutor1.Services;

namespace tutor1.Controllers.APIController
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadAPIController : ControllerBase
    {
        private readonly ClinicContext _context;
        private readonly IFileUploadService _fileUploadService;
        private readonly IConfiguration _configuration;
        private readonly long _fileSizeLimit;
        private readonly string[] _permittedExtensions = { ".txt" };

        public FileUploadAPIController(ClinicContext context, IFileUploadService fileUploadService, IConfiguration configuration)
        {
            _context = context;
            _fileUploadService = fileUploadService;
            _configuration = configuration;
            _fileSizeLimit = _configuration.GetValue<long>("FileSizeLimit");
        }

        [HttpGet]
        public async Task<ActionResult<FileUploadViewModel>> GetAll()
        {
            return await LoadAllFiles();
        }

        private async Task<FileUploadViewModel> LoadAllFiles()
        {
            var viewModel = new FileUploadViewModel();
            viewModel.FilesOnFileSystem = await _context.FileOnFileSystemModels.ToListAsync();
            return viewModel;
        }

        [HttpPost("UploadItem")]
        public async Task<IActionResult> UploadToFileSystem(List<IFormFile> files, string description)
        {
            var viewModel = new FileUploadViewModel();
            string ErrorMsg = "";
            try
            {
                viewModel = await _fileUploadService.UploadToFileSystem(files, description, _permittedExtensions, _fileSizeLimit);
            }
            catch (Exception ex)
            {
                ErrorMsg = ex.Message;
            }

            if (viewModel != null && ErrorMsg == "")
            {
                return Ok("File successfully uploaded to File System.");
            }
            else
            {
                return BadRequest(ErrorMsg);
            }           
        }

        [HttpPost("DownloadItem")]
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

        [HttpPost("DeleteItem")]
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
            return Ok($"Removed {file.Name + file.Extension} successfully from File System.");            
        }
    }
}
