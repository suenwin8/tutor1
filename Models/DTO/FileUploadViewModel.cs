using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tutor1.Models.Entity;

namespace tutor1.Models.DTO
{
    public class FileUploadViewModel
    {
        public List<FileOnFileSystemModel> FilesOnFileSystem { get; set; }  
        public string UploadRemarks { get; set; }
    }
}
