using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DocKick.Data.Repositories;
using DocKick.Dtos.Blobs;
using DocKick.Entities.Blobs;
using DocKick.Exceptions;
using DocKick.Helpers.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DocKick.Services.Blobs
{
    public class BlobDataService : IBlobDataService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Blob> _repository;

        public BlobDataService(IRepository<Blob> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IReadOnlyCollection<BlobModel>> GetBlobsByUserId(Guid userId, BlobCallback blobCallback)
        {
            ExceptionHelper.ThrowArgumentNullIfEmpty(userId, nameof(userId));
            ExceptionHelper.ThrowParameterNullIfEmpty(blobCallback, "Blob callback");

            var blobs = await _repository.GetAll()
                                         .Include(q => q.BlobLink)
                                         .Where(q => q.UserId == userId)
                                         .ToArrayAsync();

            foreach (var blob in blobs.Where(blob => !IsValidBlobLink(blob)))
            {
                var (url, expirationDate) = blobCallback(blob.UserId, blob.Name);
                await GenerateBlobLinkAndSave(blob, url, expirationDate);
            }

            return _mapper.Map<BlobModel[]>(blobs);
        }

        public async Task<BlobModel> Create(BlobModel model, string blobName)
        {
            ExceptionHelper.ThrowParameterNullIfEmpty(model, "Blob model");
            ExceptionHelper.ThrowParameterNullIfEmpty(blobName, nameof(blobName));

            var mappedEntity = _mapper.Map<Blob>(model);

            mappedEntity.Name = blobName;

            var blob = await _repository.Create(mappedEntity);

            await _repository.Save();

            return _mapper.Map<BlobModel>(blob);
        }

        public async Task<BlobModel> Update(Guid id, BlobModel model)
        {
            var entity = await _repository.GetById(id);

            ExceptionHelper.ThrowNotFoundIfEmpty(entity, nameof(Blob));

            entity.CategoryId = model.CategoryId;

            var updatedEntity = _repository.Update(entity);

            await _repository.Save();

            return _mapper.Map<BlobModel>(updatedEntity);
        }

        public async Task Delete(Guid blobId, Func<Blob, Task> func = null)
        {
            ExceptionHelper.ThrowArgumentNullIfEmpty(blobId, nameof(blobId));

            await _repository.ExecuteInTransaction(async () =>
                                                   {
                                                       var blobEntity = await _repository.GetById(blobId);

                                                       if (blobEntity is null)
                                                       {
                                                           return;
                                                       }

                                                       if (!func.IsEmpty())
                                                       {
                                                           await func(blobEntity);
                                                       }


                                                       _repository.Delete(blobEntity);
                                                       await _repository.Save();
                                                   });

        }

        public async Task<BlobModel> GenerateBlobLink(Guid blobId, BlobCallback blobCallback)
        {
            ExceptionHelper.ThrowArgumentNullIfEmpty(blobId, nameof(blobId));

            var blob = await _repository.GetById(blobId);

            ExceptionHelper.ThrowNotFoundIfEmpty(blob, "Blob");

            if (IsValidBlobLink(blob))
            {
                return _mapper.Map<BlobModel>(blob);
            }

            var (url, expirationDate) = blobCallback(blob.UserId, blob.Name);

            blob = await GenerateBlobLinkAndSave(blob, url, expirationDate);

            return _mapper.Map<BlobModel>(blob);
        }

        private async Task<Blob> GenerateBlobLinkAndSave(Blob blob, string url, DateTimeOffset expirationDate)
        {
            blob.BlobLink ??= new BlobLink();
            blob.BlobLink.ExpirationDate = expirationDate;
            blob.BlobLink.Url = url;

            await _repository.Save();

            return blob;
        }

        private static bool IsValidBlobLink(Blob blob)
        {
            return blob.BlobLink is not null && !blob.BlobLink.Url.IsEmpty() && blob.BlobLink.ExpirationDate < DateTimeOffset.Now;
        }
    }
}