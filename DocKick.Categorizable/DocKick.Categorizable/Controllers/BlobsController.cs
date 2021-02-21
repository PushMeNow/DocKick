using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DocKick.Dtos.Blobs;
using DocKick.Services.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DocKick.Categorizable.Controllers
{
    public class BlobsController : BaseApiController
    {
        private readonly IBlobService _blobService;

        public BlobsController(IBlobService blobService, IMapper mapper) : base(mapper)
        {
            _blobService = blobService;
        }

        [HttpPost]
        public async IAsyncEnumerable<BlobUploadModel> UploadBlob(IFormFileCollection files)
        {
            foreach (var file in files)
            {
                await using var stream = file.OpenReadStream();

                yield return await _blobService.Upload(UserId, stream, file.ContentType);
            }
        }

        [HttpDelete("{blobId:Guid")]
        public async Task DeleteBlob(Guid blobId)
        {
            await _blobService.FullDelete(blobId);
        }
    }
}