using App1.API.Models.Domain;
using App1.API.Models.DTOs;

namespace App1.API.Repositories.Interface
{
    public interface ICategoryRepository
    {
        
        public Task<Category> CreateAsync(Category category);
        public Task<IEnumerable<Category>> GetAllAsync(string? query = null, string? sortBy = null,
            string? sortDirection = null, int? pageNumber = 1, int? pageSize = 100);
        public Task<Category?> GetByIdAsync(Guid id);
        public Task<Category?> UpdateAsync(Category category);
        public Task<Category?> DeleteAsync(Guid id);
        public Task<int> getCountAsync();
    }
}
