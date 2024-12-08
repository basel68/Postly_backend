using App1.API.Data;
using App1.API.Models.Domain;
using App1.API.Models.DTOs;
using App1.API.Repositories.Interface;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace App1.API.Repositories.Implementation
{
    public class CategoryRepository : ICategoryRepository
    {
        private ApplicationDbContext dbContext;
        public CategoryRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Category?> DeleteAsync(Guid id)
        {
            var category = dbContext.Categories.FirstOrDefault(c => c.Id == id);
            if(category is null)
            {
                return null;
            }
            dbContext.Categories.Remove(category);
            await dbContext.SaveChangesAsync();
            return category;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await dbContext.Categories.ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(Guid id)
        {
            return await dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Category?> UpdateAsync(Category category)
        {
            var toBeUpdated= await dbContext.Categories.FirstOrDefaultAsync(x=>x.Id==category.Id);
            if (toBeUpdated!=null)
            {
                dbContext.Entry(toBeUpdated).CurrentValues.SetValues(category);
                await dbContext.SaveChangesAsync();
                return category;
            }
            return null;
        }

        public async Task<Category> CreateAsync(Category category)
        {
            await dbContext.Categories.AddAsync(category);
            await dbContext.SaveChangesAsync();
            return category;
        }

    }
}
