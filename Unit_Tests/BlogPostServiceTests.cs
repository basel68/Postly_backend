using App1.API.Data;
using App1.API.Models.Domain;
using App1.API.Repositories.Implementation;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine.ClientProtocol;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Xunit.Abstractions;


namespace UnitTests
{
    public class BlogPostServiceTests
    {
        private readonly ITestOutputHelper output;

        public BlogPostServiceTests(ITestOutputHelper output)
        {
            this.output = output;
        }
        private ApplicationDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new ApplicationDbContext(options);
        }
        [Fact]
        public async Task CreateAsync_ValidBlogPost_AddsBlogPost()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var repository = new BlogPostRepository(dbContext);
            var mockBlogPost = new BlogPost
            {
                Id = Guid.NewGuid(),
                Title = "Sample Blog Post",
                ShortDescription = "This is a mock short description for the blog post.",
                Content = "This is mock content for the blog post. It is simple and easy to understand.",
                FeaturedImageUrl = "https://example.com/mock-image.jpg",
                UrlHandle = "sample-blog-post",
                PublishedDate = DateTime.Now,
                Author = "John Doe",
                IsVisible = true,
                Categories = new List<Category>
                {
                    new Category { Id = Guid.NewGuid(), Name = "Technology",UrlHandle="beso" },
                    new Category { Id = Guid.NewGuid(), Name = "Education",UrlHandle="beso" }
                }
            };
            // Act
            var result = await repository.CreateAsync(mockBlogPost);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(mockBlogPost.Id, result.Id);
            Assert.Equal(mockBlogPost.Title, result.Title);
            Assert.Single(dbContext.BlogPosts); // Verify it was added once
        }
        [Fact]
        public async Task GetAllAsync_ReturnsAllBlogPosts()
        {
            //Arrange
            var dbContext = GetInMemoryDbContext();
            dbContext.BlogPosts.AddRange(
                new BlogPost
                {
                    Id = Guid.NewGuid(),
                    Title = "Sample Blog Post 1",
                    ShortDescription = "This is a mock short description for the blog post.",
                    Content = "This is mock content for the blog post. It is simple and easy to understand.",
                    FeaturedImageUrl = "https://example.com/mock-image.jpg",
                    UrlHandle = "sample-blog-post-1",
                    PublishedDate = DateTime.Now,
                    Author = "John Doe",
                    IsVisible = true,
                    Categories = new List<Category>
                    {
                        new Category { Id = Guid.NewGuid(), Name = "Technology",UrlHandle="beso" },
                        new Category { Id = Guid.NewGuid(), Name = "Education",UrlHandle="beso" }
                    }
                },
                new BlogPost
                {
                    Id = Guid.NewGuid(),
                    Title = "Sample Blog Post 2",
                    ShortDescription = "This is a mock short description for the blog post.",
                    Content = "This is mock content for the blog post. It is simple and easy to understand.",
                    FeaturedImageUrl = "https://example.com/mock-image.jpg",
                    UrlHandle = "sample-blog-post-2",
                    PublishedDate = DateTime.Now,
                    Author = "John Doe",
                    IsVisible = true,
                    Categories = new List<Category>
                    {
                        new Category { Id = Guid.NewGuid(), Name = "Technology",UrlHandle="beso" },
                        new Category { Id = Guid.NewGuid(), Name = "Education",UrlHandle="beso" }
                    }
                }
            );
            await dbContext.SaveChangesAsync();
            //Act
            var repository = new BlogPostRepository(dbContext);
            var result = await repository.GetAllAsync();
            //Assert
            Assert.NotNull(result);
            Assert.True(result.Count() == 2);
        }
        [Fact]
        public async Task GetByIdAsync_ExistingId_ReturnsBlogPost()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var mockBlogPost = new BlogPost
            {
                Id = Guid.NewGuid(),
                Title = "Sample Blog Post",
                ShortDescription = "This is a mock short description for the blog post.",
                Content = "This is mock content for the blog post. It is simple and easy to understand.",
                FeaturedImageUrl = "https://example.com/mock-image.jpg",
                UrlHandle = "sample-blog-post",
                PublishedDate = DateTime.Now,
                Author = "John Doe",
                IsVisible = true,
                Categories = new List<Category>
                {
                    new Category { Id = Guid.NewGuid(), Name = "Technology",UrlHandle="beso" },
                    new Category { Id = Guid.NewGuid(), Name = "Education",UrlHandle="beso" }
                }
            };
            dbContext.BlogPosts.Add(mockBlogPost);
            await dbContext.SaveChangesAsync();
            var repository = new BlogPostRepository(dbContext);
            // Act
            var result = await repository.GetByIdAsync(mockBlogPost.Id);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(mockBlogPost.Id, result.Id);
            Assert.Equal(mockBlogPost.Title, result.Title);
        }
        [Fact]
        public async Task DeleteAsync_ExistingId_DeletesBlogPost()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var mockBlogPost = new BlogPost
            {
                Id = Guid.NewGuid(),
                Title = "Sample Blog Post",
                ShortDescription = "This is a mock short description for the blog post.",
                Content = "This is mock content for the blog post. It is simple and easy to understand.",
                FeaturedImageUrl = "https://example.com/mock-image.jpg",
                UrlHandle = "sample-blog-post",
                PublishedDate = DateTime.Now,
                Author = "John Doe",
                IsVisible = true,
                Categories = new List<Category>
                {
                    new Category { Id = Guid.NewGuid(), Name = "Technology",UrlHandle="beso" },
                    new Category { Id = Guid.NewGuid(), Name = "Education",UrlHandle="beso" }
                }
            };
            dbContext.BlogPosts.Add(mockBlogPost);
            await dbContext.SaveChangesAsync();
            var repository = new BlogPostRepository(dbContext);
            // Act
            var result = await repository.DeleteByIdAsync(mockBlogPost.Id);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(mockBlogPost.Id, result.Id);
            Assert.Empty(dbContext.BlogPosts); 
        }
        [Fact]
        public async Task UpdateAsync_ExistingBlogPost_UpdatesBlogPost()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var categories = new List<Category>
                {
                    new Category { Id = Guid.NewGuid(), Name = "Technology",UrlHandle="beso" },
                    new Category { Id = Guid.NewGuid(), Name = "Education",UrlHandle="beso" }
                };
            var mockBlogPost = new BlogPost
            {
                Id = Guid.NewGuid(),
                Title = "Sample Blog Post",
                ShortDescription = "This is a mock short description for the blog post.",
                Content = "This is mock content for the blog post. It is simple and easy to understand.",
                FeaturedImageUrl = "https://example.com/mock-image.jpg",
                UrlHandle = "sample-blog-post",
                PublishedDate = DateTime.Now,
                Author = "John Doe",
                IsVisible = true,
                Categories = categories
            };
            dbContext.BlogPosts.Add(mockBlogPost);
            await dbContext.SaveChangesAsync();
            var repository = new BlogPostRepository(dbContext);
            // Act
            var updatedBlogPost = new BlogPost
            {
                Id = mockBlogPost.Id,
                Title = "Updated Blog Post",
                ShortDescription = "This is an updated short description for the blog post.",
                Content = "This is updated content for the blog post. It is simple and easy to understand.",
                FeaturedImageUrl = "https://example.com/updated-image.jpg",
                UrlHandle = "updated-blog-post",
                PublishedDate = DateTime.Now,
                Author = "Jane Doe",
                IsVisible = true,
                Categories = categories
            };
            var result = await repository.UpdateAsync(updatedBlogPost);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updatedBlogPost.Id, result.Id);
            Assert.Equal(updatedBlogPost.Title, result.Title);
            Assert.Equal(updatedBlogPost.ShortDescription, result.ShortDescription);
            Assert.Equal(updatedBlogPost.Content, result.Content);
            Assert.Equal(updatedBlogPost.FeaturedImageUrl, result.FeaturedImageUrl);
            Assert.Equal(updatedBlogPost.UrlHandle, result.UrlHandle);
        }
        [Fact]
        public async Task GetByUrlAsync_ExistingUrl_ReturnsBlogPost()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var mockBlogPost = new BlogPost
            {
                Id = Guid.NewGuid(),
                Title = "Sample Blog Post",
                ShortDescription = "This is a mock short description for the blog post.",
                Content = "This is mock content for the blog post. It is simple and easy to understand.",
                FeaturedImageUrl = "https://example.com/mock-image.jpg",
                UrlHandle = "sample-blog-post",
                PublishedDate = DateTime.Now,
                Author = "John Doe",
                IsVisible = true,
                Categories = new List<Category>
                {
                    new Category { Id = Guid.NewGuid(), Name = "Technology",UrlHandle="beso" },
                    new Category { Id = Guid.NewGuid(), Name = "Education",UrlHandle="beso" }
                }
            };
            dbContext.BlogPosts.Add(mockBlogPost);
            await dbContext.SaveChangesAsync();
            var repository = new BlogPostRepository(dbContext);
            // Act
            var result = await repository.GetByUrlAsync(mockBlogPost.UrlHandle);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(mockBlogPost.Id, result.Id);
            Assert.Equal(mockBlogPost.Title, result.Title);
        }
    }
}
