using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostController : ControllerBase
    {
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly ICategoryRepository _categoryRepository;

        public BlogPostController(IBlogPostRepository blogPostRepository,
                ICategoryRepository categoryRepository)
        {
            _blogPostRepository = blogPostRepository;
            _categoryRepository = categoryRepository;
        }

        // POST: https://localhost:7033/api/BlogPost
        [HttpPost]
        public async Task<IActionResult> CreateBlogPost(CreateBlogPostDto blogPostRequest)
        {
            var blogPost = new BlogPost
            {
                Title = blogPostRequest.Title,
                ShortDescription = blogPostRequest.ShortDescription,
                Content = blogPostRequest.Content,
                FeaturedImageUrl = blogPostRequest.FeaturedImageUrl,
                UrlHandle = blogPostRequest.UrlHandle,
                PublishedDate = blogPostRequest.PublishedDate,
                Author = blogPostRequest.Author,
                isVisible = blogPostRequest.isVisible,
                Categories = new List<Category>()
            };

            foreach (var categoryGuid in blogPostRequest.Categories)
            {
                var existingCategory = await _categoryRepository.GetById(categoryGuid);
                if (existingCategory != null)
                {
                    blogPost.Categories.Add(existingCategory);
                }
            }

            await _blogPostRepository.CreateAsync(blogPost);

            var blogPostResponse = new BlogPostDto
            {
                Id = blogPost.Id,
                Title = blogPost.Title,
                ShortDescription = blogPost.ShortDescription,
                Content = blogPost.Content,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                UrlHandle = blogPost.UrlHandle,
                PublishedDate = blogPost.PublishedDate,
                Author = blogPost.Author,
                isVisible = blogPost.isVisible,
                Categories = blogPost.Categories.Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    UrlHandle = c.UrlHandle
                }).ToList()
            };

            return Ok(blogPostResponse);
        }

        // GET :  https://localhost:7033/api/BlogPost
        [HttpGet]
        public async Task<IActionResult> GetAllBlogPosts()
        {
            var blogPosts = await _blogPostRepository.GetAllAsync();

            var response = new List<BlogPostDto>();

            foreach (var blogPost in blogPosts)
            {
                var blogPostDto = new BlogPostDto
                {
                    Id = blogPost.Id,
                    Title = blogPost.Title,
                    ShortDescription = blogPost.ShortDescription,
                    Content = blogPost.Content,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    UrlHandle = blogPost.UrlHandle,
                    PublishedDate = blogPost.PublishedDate,
                    Author = blogPost.Author,
                    isVisible = blogPost.isVisible,
                    Categories = blogPost.Categories.Select(c => new CategoryDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        UrlHandle = c.UrlHandle
                    }).ToList()
                };

                response.Add(blogPostDto);
            }

            return Ok(response);
        }

        // GET BY ID: https://localhost:7033/api/BlogPost/{id}
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetBlogById([FromRoute] Guid id)
        {
            var existingBlog = await _blogPostRepository.GetByIdAsync(id);

            if (existingBlog is null)
            {
                return NotFound();
            }

            var blogPostDto = new BlogPostDto
            {
                Id = existingBlog.Id,
                Title = existingBlog.Title,
                ShortDescription = existingBlog.ShortDescription,
                Content = existingBlog.Content,
                FeaturedImageUrl = existingBlog.FeaturedImageUrl,
                UrlHandle = existingBlog.UrlHandle,
                PublishedDate = existingBlog.PublishedDate,
                Author = existingBlog.Author,
                isVisible = existingBlog.isVisible,
                Categories = existingBlog.Categories.Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    UrlHandle = c.UrlHandle
                }).ToList()
            };

            return Ok(blogPostDto);
        }

        // Update: https://localhost:7033/api/BlogPost/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateById([FromRoute] Guid id, UpdateBlogPostRequestDto updateBlogPostRequestDto)
        {
            var blogPost = new BlogPost
            {
                Id = id,
                Title = updateBlogPostRequestDto.Title,
                ShortDescription = updateBlogPostRequestDto.ShortDescription,
                Content = updateBlogPostRequestDto.Content,
                FeaturedImageUrl = updateBlogPostRequestDto.FeaturedImageUrl,
                UrlHandle = updateBlogPostRequestDto.UrlHandle,
                PublishedDate = updateBlogPostRequestDto.PublishedDate,
                Author = updateBlogPostRequestDto.Author,
                isVisible = updateBlogPostRequestDto.isVisible,
                Categories = new List<Category>()
            };

            foreach (var categoryGuid in updateBlogPostRequestDto.Categories)
            {
                var existingCategory = await _categoryRepository.GetById(categoryGuid);
                if (existingCategory != null)
                {
                    blogPost.Categories.Add(existingCategory);
                }
            }

            blogPost = await _blogPostRepository.UpdateAsync(blogPost);

            if (blogPost == null)
            {
                return NotFound();
            }

            var blogPostResponse = new BlogPostDto
            {
                Id = blogPost.Id,
                Title = blogPost.Title,
                ShortDescription = blogPost.ShortDescription,
                Content = blogPost.Content,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                UrlHandle = blogPost.UrlHandle,
                PublishedDate = blogPost.PublishedDate,
                Author = blogPost.Author,
                isVisible = blogPost.isVisible,
                Categories = blogPost.Categories.Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    UrlHandle = c.UrlHandle
                }).ToList()
            };
            return Ok(blogPostResponse);
        }

        // DELETE: https://localhost:7033/api/BlogPost/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteBlogById([FromRoute] Guid id)
        {
            var existingBlog = await _blogPostRepository.DeleteAsync(id);
            if (existingBlog == null)
            {
                return NotFound();
            }

            var blogPostResponse = new BlogPostDto
            {
                Id = existingBlog.Id,
                Title = existingBlog.Title,
                ShortDescription = existingBlog.ShortDescription,
                Content = existingBlog.Content,
                FeaturedImageUrl = existingBlog.FeaturedImageUrl,
                UrlHandle = existingBlog.UrlHandle,
                PublishedDate = existingBlog.PublishedDate,
                Author = existingBlog.Author,
                isVisible = existingBlog.isVisible,
                Categories = existingBlog.Categories.Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    UrlHandle = c.UrlHandle
                }).ToList()
            };


            return Ok(blogPostResponse);
        }

    }
}
