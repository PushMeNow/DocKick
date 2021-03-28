using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DocKick.Categorizable.Extensions;
using DocKick.Dtos.Blobs;
using DocKick.Exceptions;
using DocKick.Services.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DocKick.Categorizable.Controllers
{
    public class BlobsController : BaseApiController
    {
        private readonly IBlobService _blobService;
        private readonly IBlobDataService _blobDataService;

        public BlobsController(IBlobService blobService, IBlobDataService blobDataService, IMapper mapper) : base(mapper)
        {
            _blobService = blobService;
            _blobDataService = blobDataService;
        }

        [HttpGet]
        public async Task<IReadOnlyCollection<BlobModel>> GetBlobsByUserId()
        {
            return await _blobService.GetBlobsByUserId(UserId);
        }

        [HttpPut("{blobId:Guid}")]
        public async Task<BlobModel> UpdateBlob([FromRoute] Guid blobId, BlobRequest request)
        {
            var model = Mapper.Map<BlobModel>(request);
            model.UserId = UserId;

            return await _blobDataService.Update(blobId, model);
        }

        [HttpPost("upload")]
        public async Task<BlobUploadModel> UploadBlob(IFormFile formFile)
        {
            ExceptionHelper.ThrowArgumentNullIfEmpty(formFile, nameof(formFile));
            ExceptionHelper.ThrowParameterInvalidIfTrue(!formFile.IsSupportedImageType(), "Unsupported file.");

            await using var stream = formFile.OpenReadStream();

            return await _blobService.Upload(UserId, formFile.FileName, stream, formFile.ContentType);
        }

        [HttpDelete("{blobId:Guid}")]
        public async Task DeleteBlob(Guid blobId)
        {
            await _blobDataService.Delete(blobId);
        }
    }
}