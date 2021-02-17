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

            var response = await blobService.Upload(fixture.TestBlobUserId, fileStream);

            Assert.NotNull(response);
            Assert.True(await fixture.Context.Blobs.AnyAsync(q => q.UserId == fixture.TestBlobUserId));
        }

        [Fact]
        public async Task Download()
        {
            using var fixture = new BlobServiceFixture();
            var service = fixture.CreateService();
            var model = await service.Download(fixture.BlobId);

            using var info = model.BlobDownloadInfo;
            
            Assert.NotNull(model.BlobDownloadInfo);
            Assert.NotEmpty(model.BlobName);
        }
    }
}