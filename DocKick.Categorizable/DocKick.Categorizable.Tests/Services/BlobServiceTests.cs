using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using DocKick.Services.Blobs;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace DocKick.Categorizable.Tests.Services
{
    public class BlobServiceTests
    {
        private static readonly Guid _testBlobUserId = new("6f722fbe-cd44-4d27-b0ab-f1e54f6c1b96");
        private const string TestBlobName = "9bfe1f0f-c430-480e-b4da-449afe756b29.jpg";

        [Fact]
        public async Task Upload()
        {
            await using var fileStream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), "TestPictures/BlackCatWithFish.jpg"), FileMode.Open);

            var blobService = GetBlobService();

            var response = await blobService.Upload(_testBlobUserId, fileStream);
            
            Assert.NotNull(response);
        }

        [Fact]
        public async Task Download()
        {
            var service = GetBlobService();
            var response = await service.Download(_testBlobUserId, TestBlobName);

            Assert.NotNull(response);
        }

        private static BlobService GetBlobService()
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.Development.json").Build();

            var connString = builder.GetConnectionString("AzureBlobStorage");

            var blobClientService = new BlobServiceClient(connString);

            return new BlobService(blobClientService);
        }
    }
}