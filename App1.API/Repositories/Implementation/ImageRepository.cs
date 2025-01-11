using App1.API.Data;
using App1.API.Models.Domain;
using App1.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace App1.API.Repositories.Implementation
{
    public class ImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ApplicationDbContext dbContext;

        public ImageRepository(IWebHostEnvironment webHostEnvironment,IHttpContextAccessor httpContextAccessor,ApplicationDbContext dbContext)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<BlogImage>> getAllAsync()
        {
            var blogImages = new List<BlogImage>();
            return await dbContext.BlogImages.ToListAsync();
        }

        public async Task<BlogImage> upload(IFormFile file,BlogImage blogImage)
        {
            // 1- Upload the Image to API/Images
            var localPath = Path.Combine(webHostEnvironment.ContentRootPath, "Images", $"{blogImage.FileName}{blogImage.FileExtension}");
            using var stream = new FileStream(localPath, FileMode.Create);
            await file.CopyToAsync(stream);

            // 2-Update the database
            // https://localhost/images/somefilename.jpg
            var httpRequest = httpContextAccessor?.HttpContext?.Request;
            var urlPath = Path.Combine($"{httpRequest?.Scheme}://{httpRequest?.Host}{httpRequest?.PathBase}/Images/{blogImage.FileName}{blogImage.FileExtension}");
            
            blogImage.Url = urlPath;

            await dbContext.BlogImages.AddAsync( blogImage );
            await dbContext.SaveChangesAsync();

            return blogImage;

            
        }
    }
}
