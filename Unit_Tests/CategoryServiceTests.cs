using App1.API.Repositories;
using App1.API.Models.Domain;
using App1.API.Repositories.Implementation;
using App1.API.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;
namespace UnitTests
{
    

    public class CategoryRepositoryTests
    {
        private ApplicationDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task CreateAsync_ValidCategory_AddsCategory()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var repository = new CategoryRepository(dbContext);
            var category = new Category { Id = Guid.NewGuid(), Name = "Test Category", UrlHandle = "beso" };

            // Act
            var result = await repository.CreateAsync(category);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(category.Id, result.Id);
            Assert.Equal(category.Name, result.Name);
            Assert.Single(dbContext.Categories); // Verify it was added
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllCategories()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            dbContext.Categories.AddRange(
                new Category { Id = Guid.NewGuid(), Name = "Category 1",UrlHandle="beso" },
                new Category { Id = Guid.NewGuid(), Name = "Category 2", UrlHandle = "beso" }
            );
            await dbContext.SaveChangesAsync();
            var repository = new CategoryRepository(dbContext);

            // Act
            var result = await repository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ExistingId_ReturnsCategory()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var category = new Category { Id = Guid.NewGuid(), Name = "Test Category", UrlHandle = "beso" };
            dbContext.Categories.Add(category);
            await dbContext.SaveChangesAsync();
            var repository = new CategoryRepository(dbContext);

            // Act
            var result = await repository.GetByIdAsync(category.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(category.Id, result?.Id);
            Assert.Equal(category.Name, result?.Name);
        }

        [Fact]
        public async Task DeleteAsync_ExistingId_DeletesCategory()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var category = new Category { Id = Guid.NewGuid(), Name = "Test Category", UrlHandle = "beso" };
            dbContext.Categories.Add(category);
            await dbContext.SaveChangesAsync();
            var repository = new CategoryRepository(dbContext);

            // Act
            var result = await repository.DeleteAsync(category.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(category.Id, result?.Id);
            Assert.Empty(dbContext.Categories); // Verify it was removed
        }

        [Fact]
        public async Task UpdateAsync_ExistingCategory_UpdatesCategory()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var category = new Category { Id = Guid.NewGuid(), Name = "Old Name", UrlHandle = "beso" };
            dbContext.Categories.Add(category);
            await dbContext.SaveChangesAsync();
            var repository = new CategoryRepository(dbContext);
            var updatedCategory = new Category { Id = category.Id, Name = "New Name", UrlHandle = "beso" };

            // Act
            var result = await repository.UpdateAsync(updatedCategory);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("New Name", result?.Name);
            Assert.Equal(1, dbContext.Categories.Count());
        }

        [Fact]
        public async Task GetCountAsync_ReturnsCorrectCount()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            dbContext.Categories.AddRange(
                new Category { Id = Guid.NewGuid(), Name = "Category 1", UrlHandle = "beso" },
                new Category { Id = Guid.NewGuid(), Name = "Category 2", UrlHandle = "beso" }
            );
            await dbContext.SaveChangesAsync();
            var repository = new CategoryRepository(dbContext);

            // Act
            var count = await repository.getCountAsync();

            // Assert
            Assert.Equal(2, count);
        }
    }

}
