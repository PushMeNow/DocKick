using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DocKick.Categorizable.Tests.Services.Fixtures;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DocKick.Categorizable.Tests.Services
{
    public class BlobServiceTests
    {
        [Fact]
        public async Task Upload()
        {
            await using var fileStream = BlobServiceFixture.GetTestPicture();
            using var fixture = new BlobServiceFixture();
            var blobService = fixture.CreateService();

            var response = await blobService.Upload(fixture.TestBlobUserId, fixture.CategoryId, fileStream);

            Assert.NotNull(response);
            Assert.True(await fixture.Context.Blobs.AnyAsync(q => q.CategoryId == fixture.CategoryId && q.BlobContainer.UserId == fixture.TestBlobUserId));
            Assert.True(await fixture.Context.BlobContainers.AnyAsync(q => q.UserId == fixture.TestBlobUserId));
        }

        [Fact]
        public async Task Download()
        {
            using var fixture = new BlobServiceFixture();
            var service = fixture.CreateService();
            var (blobInfo, blobName) = await service.Download(fixture.TestBlobUserId, fixture.BlobId);

            Assert.NotNull(blobInfo);
            Assert.NotEmpty(blobName);

            var file = new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), $"BlobPictures/{blobName}"));

            if (file.Exists)
            {
                file.Delete();
            }

            await using var stream = file.Create();
            await blobInfo.Content.CopyToAsync(stream);
        }
    }
}