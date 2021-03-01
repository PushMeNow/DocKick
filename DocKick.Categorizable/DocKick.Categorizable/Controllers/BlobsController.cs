﻿using System;
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
            ExceptionHelper.ThrowArgumentNullIfEmpty(formFile, nameof(formFile));
            ExceptionHelper.ThrowParameterInvalidIfTrue(!formFile.IsSupportedImageType(), "Unsupported file.");
            
            await using var stream = formFile.OpenReadStream();

            return await _blobService.Upload(UserId, formFile.FileName, stream, formFile.ContentType);
        }

        [HttpDelete("{blobId:Guid}")]
        public async Task DeleteBlob(Guid blobId)
        {
            await _blobService.Delete(blobId);
        }
    }
}