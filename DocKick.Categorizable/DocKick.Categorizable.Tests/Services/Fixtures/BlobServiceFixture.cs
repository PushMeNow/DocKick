using System;
using System.IO;
using Azure.Storage.Blobs;
using DocKick.Data.Repositories;
using DocKick.Entities.Blobs;
using DocKick.Entities.Categories;
using DocKick.Services.Blobs;
using Microsoft.Extensions.Configuration;

namespace DocKick.Categorizable.Tests.Services.Fixtures
{
    public class BlobServiceFixture : BaseServiceFixture<BlobService>
    {
        private const string TestBlobName = "9bfe1f0f-c430-480e-b4da-449afe756b29.jpg";

        public Guid BlobId { get; private set; }

        public Guid CategoryId { get; private set; }

        public Guid TestBlobContainerId { get; private set; }
        public Guid TestBlobUserId { get; } = new("6f722fbe-cd44-4d27-b0ab-f1e54f6c1b96");

        public override BlobService CreateService()
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.Development.json")
                                                    .Build();

            var connString = builder.GetConnectionString("AzureBlobStorage");

            var blobClientService = new BlobServiceClient(connString);
            var blobContainerRepository = new BlobContainerRepository(Context);
            var categoryRepository = new CategoryRepository(Context);

            return new BlobService(blobClientService, blobContainerRepository, categoryRepository);
        }

        public static Stream GetTestPicture()
        {
            return new FileStream(Path.Combine(Directory.GetCurrentDirectory(), "TestPictures/BlackCatWithFish.jpg"), FileMode.Open);
        }

        protected override void InitDatabase()
        {
            base.InitDatabase();

            TestBlobContainerId = Context.BlobContainers.Add(new BlobContainer
                                                             {
                                                                 UserId = TestBlobUserId,
                                                                 Name = TestBlobUserId.ToString()
                                                             })
                                         .Entity.BlobContainerId;

            CategoryId = Context.Categories.Add(new Category
                                                {
                                                    UserId = TestBlobUserId,
                                                    Name = "Test Category"
                                                })
                                .Entity.CategoryId;

            BlobId = Context.Blobs.Add(new Blob
                                       {
                                           Name = TestBlobName,
                                           CategoryId = CategoryId,
                                           BlobContainerId = TestBlobContainerId
                                       })
                            .Entity.BlobId;
        }
    }
}