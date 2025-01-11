using App1.API.Data;
using App1.API.Repositories.Implementation;
using Microsoft.EntityFrameworkCore;
using App1.API.Models.Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Moq;

namespace UnitTests
{
    public class ImageServiceTests
    {
        private readonly Mock<IWebHostEnvironment> _mockWebHostEnvironment;
        private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
        private readonly ApplicationDbContext _dbContext;
        private ApplicationDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new ApplicationDbContext(options);
        }
      

        public ImageServiceTests()
        {
            // Mock IWebHostEnvironment
            _mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            _mockWebHostEnvironment.Setup(m => m.ContentRootPath).Returns("M:/Temp");

            // Mock IHttpContextAccessor
            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Scheme = "http";
            httpContext.Request.Host = new HostString("localhost:5046");
            httpContext.Request.PathBase = "/api";
            _mockHttpContextAccessor.Setup(m => m.HttpContext).Returns(httpContext);
            //Use in memory db
            _dbContext = GetInMemoryDbContext();
        }

        [Fact]
        public async Task Upload_ShouldSaveFileAndReturnBlogImage()
        {
            // Arrange
            var repository = new ImageRepository(_mockWebHostEnvironment.Object, _mockHttpContextAccessor.Object, _dbContext);

            // Mock the IFormFile
            var fileMock = new Mock<IFormFile>();
            var content = "Test file content";
            var fileName = "testfile.jpg";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;

            fileMock.Setup(f => f.OpenReadStream()).Returns(ms);
            fileMock.Setup(f => f.FileName).Returns(fileName);
            fileMock.Setup(f => f.Length).Returns(ms.Length);

            var blogImage = new BlogImage
            {
                Id = Guid.NewGuid(),
                FileName = "testfile",
                FileExtension = ".jpg",
                Title = "Test title",
                DateCreated = DateTime.Now,
            };

            // Act
            var result = await repository.upload(fileMock.Object, blogImage);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("testfile", result.FileName);
            Assert.True(result.Url.Contains("http://localhost:5046/api/Images/testfile.jpg"));
            Assert.Single(_dbContext.BlogImages); // Verify the record was added to the database
        }

        [Fact]
        public async Task GetAll_ReturnAllBlogImages()
        {
            // Arrange
            var repository = new ImageRepository(_mockWebHostEnvironment.Object, _mockHttpContextAccessor.Object, _dbContext);

            // Seed the database
            _dbContext.BlogImages.Add(new BlogImage
            {
                Id = Guid.NewGuid(),
                FileName = "testfile",
                FileExtension = ".jpg",
                Title = "Test title",
                DateCreated = DateTime.Now,
                Url = "http://localhost:5046/api/Images/testfile1.jpg"
            });
            _dbContext.BlogImages.Add(new BlogImage
            {
                Id = Guid.NewGuid(),
                FileName = "testfile2",
                FileExtension = ".jpg",
                Title = "Test title 2",
                DateCreated = DateTime.Now,
                Url= "http://localhost:5046/api/Images/testfile2.jpg"
            });
            _dbContext.SaveChanges();

            // Act
            var result = await repository.getAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }


    }
}
