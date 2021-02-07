using System.Linq;
using System.Threading.Tasks;
using DocKick.Categorizable.Tests.Services.Fixtures;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DocKick.Categorizable.Tests.Services
{
    public class BlobServiceTests : IClassFixture<BlobServiceFixture>
    {
        private readonly BlobServiceFixture _fixture;

        public BlobServiceTests(BlobServiceFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Upload()
        {
            await using var fileStream = BlobServiceFixture.GetTestPicture();

            var blobService = _fixture.CreateService();

            var response = await blobService.Upload(_fixture.TestBlobUserId, _fixture.CategoryId, fileStream);

            Assert.NotNull(response);
            Assert.True(await _fixture.Context.Blobs.AnyAsync(q => q.CategoryId == _fixture.CategoryId && q.BlobContainer.UserId == _fixture.TestBlobUserId));
            Assert.True(await _fixture.Context.BlobContainers.AnyAsync(q => q.UserId == _fixture.TestBlobUserId));
        }

        [Fact]
        public async Task Download()
        {
            var service = _fixture.CreateService();
            var response = await service.Download(_fixture.TestBlobUserId, _fixture.BlobId);

            Assert.NotNull(response);
        }
    }
}