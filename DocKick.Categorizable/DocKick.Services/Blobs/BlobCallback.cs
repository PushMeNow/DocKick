using System;

namespace DocKick.Services.Blobs
{
    public delegate (string url, DateTimeOffset expirationDate) BlobCallback(Guid userId, string blobName);
}