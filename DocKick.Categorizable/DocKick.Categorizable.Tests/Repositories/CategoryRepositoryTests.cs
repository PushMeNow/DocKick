using System;
using System.Threading.Tasks;
using DocKick.Categorizable.Tests.Repositories.Fixtures;
using DocKick.Entities.Categories;
using DocKick.Exceptions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DocKick.Categorizable.Tests.Repositories
{
    public class CategoryRepositoryTests : IDisposable
    {
        public static object[][] IncorrectIds =
        {
            new object[]
            {
                Guid.Empty,
                typeof(ArgumentNullException)
            },
            new object[]
            {
                Guid.NewGuid(),
                typeof(ParameterNullException)
            }
        };
        
        private readonly CategoryRepositoryFixture _fixture;

        public CategoryRepositoryTests()
        {
            // Arrange
            _fixture = new CategoryRepositoryFixture();
        }

        [Fact]
        public async Task GetAll_OK()
        {
            // Act
            var result = _fixture.Repository.GetAll();

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(2, await result.CountAsync());
        }

        [Fact]
        public async Task GetById_OK()
        {
            //Act
            var result = await _fixture.Repository.GetById(_fixture.UpdateCategoryId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(_fixture.UpdateCategoryId, result.CategoryId);
            Assert.Equal("Updating category", result.Name);
        }

        [Fact]
        public async Task GetById_IncorrectData_Fail()
        {
            // Act, Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _fixture.Repository.GetById(Guid.Empty));
        }

        [Fact]
        public async Task Create_OK()
        {
            // Arrange
            var createdEntity = new Category
                                {
                                    Name = "Created category",
                                    UserId = _fixture.UserId
                                };

            // Act
            var result = await _fixture.Repository.Create(createdEntity);

            await _fixture.SaveContext();

            // Assert
            Assert.NotNull(result);
            Assert.NotEqual(Guid.Empty, result.CategoryId);
            Assert.Equal(createdEntity.Name, result.Name);
            Assert.Equal(createdEntity.UserId, result.UserId);
        }
        
        [Fact]
        public async Task Create_IncorrectData_Fail()
        {
            // Act, Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _fixture.Repository.Create(null));
        }

        [Fact]
        public async Task Update_OK()
        {
            // Arrange
            const string newName = "Updated category";
            var updatedEntity = await _fixture.Context.Categories.FindAsync(_fixture.UpdateCategoryId);
            updatedEntity.Name = newName;

            // Act
            var result = _fixture.Repository.Update(updatedEntity);

            await _fixture.SaveContext();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(_fixture.UpdateCategoryId, result.CategoryId);
            Assert.Equal(_fixture.UserId, result.UserId);
            Assert.Equal(newName, result.Name);
        }
        
        [Fact]
        public void Update_IncorrectData_Fail()
        {
            // Act, Assert
            Assert.Throws<ArgumentNullException>(() => _fixture.Repository.Update(null));
        }

        [Fact]
        public async Task Delete_OK()
        {
            // Act
            await _fixture.Repository.Delete(_fixture.DeleteCategoryId);

            // Assert
            Assert.NotNull(await _fixture.Context.Categories.FindAsync(_fixture.DeleteCategoryId));
        }
        
        [Theory]
        [MemberData(nameof(IncorrectIds))]
        public async Task Delete_IncorrectData_Fail(Guid id, Type exceptionType)
        {
            // Act, Assert
            await Assert.ThrowsAsync(exceptionType, () => _fixture.Repository.Delete(id));
        }

        #region Dispose

        public void Dispose()
        {
            _fixture.Dispose();
        }

        #endregion
    }
}