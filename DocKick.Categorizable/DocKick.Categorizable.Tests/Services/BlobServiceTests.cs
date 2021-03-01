using System;
using System.Threading.Tasks;
using DocKick.Categorizable.Tests.Services.Fixtures;
using DocKick.Exceptions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DocKick.Categorizable.Tests.Services
{
    public class BlobServiceTests
    {
        // TODO: Need to mock whole blob azure service.
        [Fact]
        public async Task Upload_OK()
        {
            // Arrange
            await using var fileStream = BlobServiceFixture.GetTestPicture();
            using var fixture = new BlobServiceFixture();
            var blobService = fixture.CreateService();

            // Act
            var response = await blobService.Upload(fixture.TestBlobUserId, null, fileStream);

            //Assert
            Assert.NotNull(response);
            Assert.True(await fixture.Context.Blobs.AnyAsync(q => q.UserId == fixture.TestBlobUserId));
        }
        
        [Fact]
        public async Task Upload_BlobExists_OK()
        {
            // Arrange
            await using var fileStream = BlobServiceFixture.GetTestPicture();
            using var fixture = new BlobServiceFixture();
            var blobService = fixture.CreateService();
            const string blobName = "Test file";

            // Act, Assert
            await Assert.ThrowsAsync<ParameterInvalidException>(() => blobService.Upload(fixture.TestBlobUserId, blobName, fileStream));            
            Assert.True(await fixture.Context.Blobs.AnyAsync(q => q.Name == blobName));
        }

        [Fact]
        public async Task Download_OK()
        {
            // Arrange 
            using var fixture = new BlobServiceFixture();
            var service = fixture.CreateService();

            // Act
            var model = await service.Download(fixture.BlobId);

            // Assert
            using var info = model.BlobDownloadInfo;

            Assert.NotNull(model.BlobDownloadInfo);
            Assert.NotEmpty(model.Name);
        }

        [Fact]
        public async Task GenerateBlobLink_OK()
        {
            // Arrange
            using var fixture = new BlobServiceFixture();
            var service = fixture.CreateService();

            // Act
            var result = await service.GenerateBlobLink(fixture.BlobId);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result.BlobLink.Url);

            var blobLink = await fixture.Context.BlobLinks.FirstOrDefaultAsync(q => q.BlobId == fixture.BlobId);

            Assert.NotNull(blobLink);
            Assert.Equal(blobLink.ExpirationDate, result.BlobLink.ExpirationDate);
            Assert.Equal(blobLink.Url, result.BlobLink.Url);
        }

        [Fact]
        public async Task GetBlobsByUserId_OK()
        {
            // Arrange
            using var fixture = new BlobServiceFixture();
            var service = fixture.CreateService();

            // Act
            var result = await service.GetBlobsByUserId(fixture.TestBlobUserId);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);

            Assert.All(result,
                       q =>
                       {
                           Assert.NotNull(q.BlobLink);
                           Assert.NotEmpty(q.BlobLink.Url);
                           Assert.NotEqual(DateTimeOffset.MinValue, q.BlobLink.ExpirationDate);
                       });
        }
    }
}