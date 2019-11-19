using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Abp.Auditing;
using Microsoft.AspNetCore.Mvc;
using Snow.Template.Dto;
using Snow.Template.Storage;

namespace Snow.Template.Controllers
{
    public class FileController : AbpZeroTemplateControllerBase
    {
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly IBinaryObjectManager _binaryObjectManager;

        public FileController(
            ITempFileCacheManager tempFileCacheManager,
            IBinaryObjectManager binaryObjectManager
        )
        {
            _tempFileCacheManager = tempFileCacheManager;
            _binaryObjectManager = binaryObjectManager;
        }

        [DisableAuditing]
        public ActionResult DownloadTempFile(FileDto file)
        {
            var fileBytes = _tempFileCacheManager.GetFile(file.FileToken);
            if (fileBytes == null)
            {
                return NotFound(L("RequestedFileDoesNotExists"));
            }

            return File(fileBytes, file.FileType, file.FileName);
        }
    }
}