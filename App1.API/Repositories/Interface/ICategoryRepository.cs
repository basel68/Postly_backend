using App1.API.Models.Domain;
using App1.API.Models.DTOs;

namespace App1.API.Repositories.Interface
{
    public interface ICategoryRepository
    {
        
        public Task<Category> CreateAsync(Category category);
        public Task<IEnumerable<Category>> GetAllAsync();
        public Task<Category?> GetByIdAsync(Guid id);
        public Task<Category?> UpdateAsync(Category category);
        public Task<Category?> DeleteAsync(Guid id);
    }
}
