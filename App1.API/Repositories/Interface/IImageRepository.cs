using App1.API.Models.Domain;

namespace App1.API.Repositories.Interface
{
    public interface IImageRepository
    {
        public Task<BlogImage> upload(IFormFile file, BlogImage blogImage);
        public Task<IEnumerable<BlogImage>> getAllAsync();
    }
}
