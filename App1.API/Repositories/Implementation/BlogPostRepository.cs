using App1.API.Data;
using App1.API.Models.Domain;
using App1.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace App1.API.Repositories.Implementation
{
    public class BlogPostRepository:IBlogPostRepository 
    {
        private readonly ApplicationDbContext dbContext;

        public BlogPostRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<BlogPost> CreateAsync(BlogPost blogPost)
        {
            await this.dbContext.BlogPosts.AddAsync(blogPost);
            await this.dbContext.SaveChangesAsync();
            return blogPost;
        }

        public async Task<BlogPost?> DeleteById(Guid id)
        {
            var existingBlogPost=await dbContext.BlogPosts.FirstOrDefaultAsync(b => b.Id == id);
            if (existingBlogPost != null) {
                dbContext.BlogPosts.Remove(existingBlogPost);
                await dbContext.SaveChangesAsync();
                return existingBlogPost;
            }
            return null;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
           return await this.dbContext.BlogPosts.Include(x =>x.Categories).ToListAsync();
        }

        public async Task<BlogPost?> GetById(Guid id)
        {
            return await dbContext.BlogPosts.Include(x=>x.Categories).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<BlogPost?> GetByUrl(string url)
        {
            return await dbContext.BlogPosts.Include(x => x.Categories).FirstOrDefaultAsync(x => x.UrlHandle == url);
        }

        public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
            var existingBlogPost = await this.dbContext.BlogPosts.Include(x => x.Categories).FirstOrDefaultAsync(x => x.Id == blogPost.Id);
            if (existingBlogPost != null)
            {
                dbContext.Entry(existingBlogPost).CurrentValues.SetValues(blogPost);
                existingBlogPost.Categories = blogPost.Categories;
                await dbContext.SaveChangesAsync();
                return blogPost;
            }
            return null;
        }
    }
}
