using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using tutor1.Models.Context;
using tutor1.Models.DTO;
using tutor1.Models.Entity;

namespace tutor1.Services
{
    public interface IFileUploadService
    {
        public Task<FileUploadViewModel> LoadAllFiles();
        public Task<FileUploadViewModel> UploadToFileSystem(List<IFormFile> files, string description, string[] permittedExtensions, long sizeLimit);

        public bool CheckFileSize(IFormFile files, long sizeLimit);
        public Task<FileOnFileSystemModel> GetByID(int id);
        public Task<FileOnFileSystemModel> DeleteByID(int id);

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
       

        public FileUploadService(ClinicContext Context)
        {
            _context = Context;            
        }
        // If you require a check on specific characters in the IsValidFileExtensionAndSignature
        // method, supply the characters in the _allowedChars field.
        private static readonly byte[] _allowedChars = { };
        // For more file signatures, see the File Signatures Database (https://www.filesignatures.net/)
        // and the official specifications for the file types you wish to add.
        private static readonly Dictionary<string, List<byte[]>> _fileSignature = new Dictionary<string, List<byte[]>>
        {
            { ".gif", new List<byte[]> { new byte[] { 0x47, 0x49, 0x46, 0x38 } } },
            { ".png", new List<byte[]> { new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A } } },
            { ".jpeg", new List<byte[]>
                {
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE2 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE3 },
                }
            },
            { ".jpg", new List<byte[]>
                {
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE1 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE8 },
                }
            },
            { ".zip", new List<byte[]>
                {
                    new byte[] { 0x50, 0x4B, 0x03, 0x04 },
                    new byte[] { 0x50, 0x4B, 0x4C, 0x49, 0x54, 0x45 },
                    new byte[] { 0x50, 0x4B, 0x53, 0x70, 0x58 },
                    new byte[] { 0x50, 0x4B, 0x05, 0x06 },
                    new byte[] { 0x50, 0x4B, 0x07, 0x08 },
                    new byte[] { 0x57, 0x69, 0x6E, 0x5A, 0x69, 0x70 },
                }
            },
        };
        public async Task<FileUploadViewModel> LoadAllFiles()
        {
            var viewModel = new FileUploadViewModel();
            viewModel.FilesOnFileSystem = await _context.FileOnFileSystemModels.ToListAsync();
            return viewModel;
        }

        public async Task<FileOnFileSystemModel> GetByID(int id)
        {
            var viewModel = new FileOnFileSystemModel();
            viewModel = await _context.FileOnFileSystemModels.Where(x => x.Id == id).FirstOrDefaultAsync();
            return viewModel;
        }

        public async Task<FileUploadViewModel> UploadToFileSystem(List<IFormFile> files, string description, string[] permittedExtensions, long sizeLimit)
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


                var fieldDisplayName = string.Empty;

                // Use reflection to obtain the display name for the model
                // property associated with this IFormFile. If a display
                // name isn't found, error messages simply won't show
                // a display name.
                MemberInfo property =
                    typeof(IFormFile).GetProperty(
                        file.Name.Substring(file.Name.IndexOf(".",
                        StringComparison.Ordinal) + 1));

                if (property != null)
                {
                    if (property.GetCustomAttribute(typeof(DisplayAttribute)) is
                        DisplayAttribute displayAttribute)
                    {
                        fieldDisplayName = $"{displayAttribute.Name} ";
                    }
                }
                // Don't trust the file name sent by the client. To display
                // the file name, HTML-encode the value.
                var trustedFileNameForDisplay = WebUtility.HtmlEncode(
                    file.FileName);

                if (!CheckFileSize(file, sizeLimit))
                {
                    var megabyteSizeLimit = sizeLimit / 1048576;
                    throw new Exception(fileName + extension + $" exceeds " + $"{megabyteSizeLimit:N1} MB.");
                }

                //check the file content
                try
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await file.CopyToAsync(memoryStream);

                        // Check the content length in case the file's only
                        // content was a BOM and the content is actually
                        // empty after removing the BOM.
                        if (memoryStream.Length == 0)
                        {
                            throw new Exception($"{fieldDisplayName}({trustedFileNameForDisplay}) is empty.");                            
                        }

                        if (!IsValidFileExtensionAndSignature(
                            file.FileName, memoryStream, permittedExtensions))
                        {
                            throw new Exception($"{fieldDisplayName}({trustedFileNameForDisplay}) file " +
                                "type isn't permitted or the file's signature " +
                                "doesn't match the file's extension.");                          
                        }                        
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"{fieldDisplayName}({trustedFileNameForDisplay}) upload failed. " +
                        $"Please contact the Help Desk for support. Error: {ex.HResult}");                    
                    // Log the exception
                }

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

        private static bool IsValidFileExtensionAndSignature(string fileName, Stream data, string[] permittedExtensions)
        {
            if (string.IsNullOrEmpty(fileName) || data == null || data.Length == 0)
            {
                return false;
            }

            var ext = Path.GetExtension(fileName).ToLowerInvariant();

            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
            {
                return false;
            }

            data.Position = 0;

            using (var reader = new BinaryReader(data))
            {
                if (ext.Equals(".txt") || ext.Equals(".csv") || ext.Equals(".prn"))
                {
                    if (_allowedChars.Length == 0)
                    {
                        // Limits characters to ASCII encoding.
                        for (var i = 0; i < data.Length; i++)
                        {
                            if (reader.ReadByte() > sbyte.MaxValue)
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        // Limits characters to ASCII encoding and
                        // values of the _allowedChars array.
                        for (var i = 0; i < data.Length; i++)
                        {
                            var b = reader.ReadByte();
                            if (b > sbyte.MaxValue ||
                                !_allowedChars.Contains(b))
                            {
                                return false;
                            }
                        }
                    }

                    return true;
                }

                // Uncomment the following code block if you must permit
                // files whose signature isn't provided in the _fileSignature
                // dictionary. We recommend that you add file signatures
                // for files (when possible) for all file types you intend
                // to allow on the system and perform the file signature
                // check.
                /*
                if (!_fileSignature.ContainsKey(ext))
                {
                    return true;
                }
                */

                // File signature check
                // --------------------
                // With the file signatures provided in the _fileSignature
                // dictionary, the following code tests the input content's
                // file signature.
                var signatures = _fileSignature[ext];
                var headerBytes = reader.ReadBytes(signatures.Max(m => m.Length));

                return signatures.Any(signature =>
                    headerBytes.Take(signature.Length).SequenceEqual(signature));
            }
        }

        public bool CheckFileSize(IFormFile files, long sizeLimit)
        {
            if (files.Length > sizeLimit)
            {   
                return false;
            }
            return true;

        }

        public async Task<FileOnFileSystemModel> DeleteByID(int id)
        {
            FileOnFileSystemModel file = await GetByID(id);
            //if (file == null) return false;
            if (System.IO.File.Exists(file.FilePath))
            {
                System.IO.File.Delete(file.FilePath);
            }
            _context.FileOnFileSystemModels.Remove(file);
            _context.SaveChanges();
            return file;
        }

        //public async Task<FileUploadViewModel> UploadToFileSystem(List<IFormFile> files, string description)
        //{
        //    var viewModel = new FileUploadViewModel();
        //    List<FileOnFileSystemModel> _FilesOnFileSystem = new List<FileOnFileSystemModel>();


        //    foreach (var file in files)
        //    {
        //        var basePath = Path.Combine(Directory.GetCurrentDirectory() + "\\Files\\");
        //        bool basePathExists = System.IO.Directory.Exists(basePath);
        //        if (!basePathExists) Directory.CreateDirectory(basePath);
        //        var fileName = Path.GetFileNameWithoutExtension(file.FileName);
        //        var filePath = Path.Combine(basePath, file.FileName);
        //        var extension = Path.GetExtension(file.FileName);
        //        if (!System.IO.File.Exists(filePath))
        //        {
        //            using (var stream = new FileStream(filePath, FileMode.Create))
        //            {
        //                await file.CopyToAsync(stream);
        //            }
        //            var fileModel = new FileOnFileSystemModel
        //            {
        //                CreatedOn = DateTime.UtcNow,
        //                FileType = file.ContentType,
        //                Extension = extension,
        //                Name = fileName,
        //                Description = description,
        //                FilePath = filePath
        //            };
        //            _context.FileOnFileSystemModels.Add(fileModel);
        //            _context.SaveChanges();
        //            _FilesOnFileSystem.Add(fileModel);
        //        }
        //    }


        //    viewModel.FilesOnFileSystem = _FilesOnFileSystem;
        //    viewModel.UploadRemarks = description;
        //    return viewModel;
        //}
    }
}
