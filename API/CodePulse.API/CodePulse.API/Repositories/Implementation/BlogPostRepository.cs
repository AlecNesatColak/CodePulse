using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Repositories.Implementation
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public BlogPostRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<BlogPost> CreateAsync(BlogPost blogPost)
        {
            await _dbContext.BlogPosts.AddAsync(blogPost);
            await _dbContext.SaveChangesAsync();
            return blogPost;
        }

        public async Task<BlogPost?> DeleteAsync(Guid id)
        {
            var existingBlogPost = await _dbContext.BlogPosts.Include(x => x.Categories).FirstOrDefaultAsync(b => b.Id == id);

            if (existingBlogPost != null)
            {
                _dbContext.BlogPosts.Remove(existingBlogPost);
                await _dbContext.SaveChangesAsync();
                return existingBlogPost;
            }

            return null;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await _dbContext.BlogPosts.Include(x => x.Categories).ToListAsync();
        }

        public async Task<BlogPost> GetByIdAsync(Guid id)
        {
            var existingBlog = await _dbContext.BlogPosts.Include(c => c.Categories).FirstOrDefaultAsync(x => id == x.Id);

            if (existingBlog == null)
            {
                return null;
            }

            return existingBlog;
        }

        public async Task<BlogPost?> GetByUrlHandleAsync(string urlHandle)
        {
            var existingUrlHandle = await _dbContext.BlogPosts.Include(bp => bp.Categories).FirstOrDefaultAsync(u => u.UrlHandle == urlHandle);

            if (existingUrlHandle == null)
            {
                return null;
            }

            return existingUrlHandle;
        }

        public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
            var existingBlogPost = await _dbContext.BlogPosts
                .Include(x => x.Categories)
                .FirstOrDefaultAsync(x => x.Id == blogPost.Id);

            if (existingBlogPost == null)
                return null;

            _dbContext.Entry(existingBlogPost).CurrentValues.SetValues(blogPost);

            existingBlogPost.Categories = blogPost.Categories;

            await _dbContext.SaveChangesAsync();
            return existingBlogPost;
        }
    }
}
