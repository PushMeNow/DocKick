using System;
using System.IO;
using Azure.Storage.Blobs;
using DocKick.Categorizable.Tests.Helpers;
using DocKick.Data.Repositories;
using DocKick.Entities.Blobs;
using DocKick.Entities.Categories;
using DocKick.Services.Blobs;
using Microsoft.Extensions.Configuration;

namespace DocKick.Categorizable.Tests.Services.Fixtures
{
    public class BlobServiceFixture : BaseServiceFixture<IBlobService>
    {
        private const string TestBlobName = "464bb061-ef5d-424f-91e0-fdb9dc7dbc6f.jpg";

        public Guid BlobId { get; private set; }

        public Guid CategoryId { get; private set; }

        public Guid TestBlobUserId { get; } = new("6f722fbe-cd44-4d27-b0ab-f1e54f6c1b96");

        // TODO: Need to replace by Moq.
        public override IBlobService CreateService()
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.Development.json")
                                                    .Build();

            var connString = builder.GetConnectionString("AzureBlobStorage");
            var blobClientService = new BlobServiceClient(connString);
            
            var blobRepository = new BlobRepository(Context);
            var blobDataService = new BlobDataService(blobRepository, Mapper);

            return new BlobService(blobClientService, blobDataService);
        }

        public static Stream GetTestPicture()
        {
            return new FileStream(Path.Combine(Directory.GetCurrentDirectory(), "TestPictures/BlackCatWithFish.jpg"), FileMode.Open);
        }

        protected override void InitDatabase()
        {
            base.InitDatabase();

            CategoryId = Context.Categories.Add(new Category
                                                {
                                                    UserId = TestBlobUserId,
                                                    Name = "Test Category"
                                                })
                                .Entity.CategoryId;

            BlobId = Context.Blobs.Add(new Blob
                                       {
                                           ImageName = TestBlobName,
                                           CategoryId = CategoryId,
                                           UserId = TestBlobUserId
                                       })
                            .Entity.BlobId;
        }
    }
}