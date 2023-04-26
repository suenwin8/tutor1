using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using tutor1.Models.Context;
using tutor1.Models.DTO;
using tutor1.Models.Entity;

namespace tutor1.Services
{
    public interface IFileUploadService
    {
        public Task<FileUploadViewModel> LoadAllFiles();
        public Task<FileUploadViewModel> UploadToFileSystem(List<IFormFile> files, string description);


        //public Task<List<AppSetting>> GetNewAppSetting(string variable_name, CancellationToken cancellationToken = default);
        //public string GetNewOrderSeqID(int seqid, CancellationToken cancellationToken = default);
        //Task<T> GetAllAsync<T>(CancellationToken cancellationToken = default) where T : class, new();

        //Task<T> GetByIDAsync<T>(int id, CancellationToken cancellationToken = default) where T : class, new();
        ////Task<T> GetByConditionAsync<T>(HolidayParameters dto, CancellationToken cancellationToken = default) where T : class, new();
        //Task UpdateAsync<T>(ClinicOrder order, CancellationToken cancellationToken = default) where T : class, new();
        //Task DeleteAsync<T>(string id, CancellationToken cancellationToken = default) where T : class, new();
    }
    public class FileUploadService : IFileUploadService
    {
        private ClinicContext _context;
        private readonly IFileUploadService _fileUploadService;

        public FileUploadService(ClinicContext Context,
            IFileUploadService fileUploadService)
        {
            _context = Context;
            _fileUploadService = fileUploadService;
        }
        public async Task<FileUploadViewModel> LoadAllFiles()
        {
            var viewModel = new FileUploadViewModel();
            viewModel.FilesOnFileSystem = await _context.FileOnFileSystemModels.ToListAsync();
            return viewModel;
        }

        public async Task<FileUploadViewModel> UploadToFileSystem(List<IFormFile> files, string description)
        {
            var viewModel = new FileUploadViewModel();
            List<FileOnFileSystemModel> _FilesOnFileSystem = new List<FileOnFileSystemModel>();


            foreach (var file in files)
            {
                var basePath = Path.Combine(Directory.GetCurrentDirectory() + "\\Files\\");
                bool basePathExists = System.IO.Directory.Exists(basePath);
                if (!basePathExists) Directory.CreateDirectory(basePath);
                var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                var filePath = Path.Combine(basePath, file.FileName);
                var extension = Path.GetExtension(file.FileName);
                if (!System.IO.File.Exists(filePath))
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    var fileModel = new FileOnFileSystemModel
                    {
                        CreatedOn = DateTime.UtcNow,
                        FileType = file.ContentType,
                        Extension = extension,
                        Name = fileName,
                        Description = description,
                        FilePath = filePath
                    };
                    _context.FileOnFileSystemModels.Add(fileModel);
                    _context.SaveChanges();
                    _FilesOnFileSystem.Add(fileModel);
                }
            }


            viewModel.FilesOnFileSystem = _FilesOnFileSystem;
            viewModel.UploadRemarks = description;
            return viewModel;
        }
    }
}
