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

        [HttpGet]
        public async Task<IReadOnlyCollection<BlobModel>> GetBlobsByUserId()
        {
            return await _blobService.GetBlobsByUserId(UserId);
        }

        [HttpPost("upload")]
        public async Task<BlobUploadModel> UploadBlob(IFormFile formFile)
        {
            await using var stream = formFile.OpenReadStream();

            return await _blobService.Upload(UserId, stream, formFile.ContentType);
        }

        [HttpDelete("{blobId:Guid}")]
        public async Task DeleteBlob(Guid blobId)
        {
            await _blobService.FullDelete(blobId);
        }
    }
}