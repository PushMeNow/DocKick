using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;

namespace DocKick.Services
{
    public class BlobService
    {
        private readonly BlobServiceClient _blobServiceClient;

        public BlobService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public async Task Upload(Stream fileStream)
        {
            // var blobClient = await _blobServiceClient.CreateBlobContainerAsync();
        }
    }
}