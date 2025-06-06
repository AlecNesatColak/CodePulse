using ClosedXML.Excel;
using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "Writer")]
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
        [Authorize(Roles = "Writer")]
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
        [Authorize(Roles = "Writer")]
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

        [HttpGet]
        [Route("blogpost-details/{urlHandle}")]

        public async Task<IActionResult> GetBlogPostByUrlHandle([FromRoute] string urlHandle)
        {
            var blogPost = await _blogPostRepository.GetByUrlHandleAsync(urlHandle);

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

        // Fix for the CS1061 error in the DownloadBlogAsExcel method
        [HttpGet("download/{id:Guid}")]
        public async Task<IActionResult> DownloadBlogAsExcel([FromRoute] Guid id)


        {
            // Await the task to get the actual BlogPost object
            var blogPost = await _blogPostRepository.GetByIdAsync(id);

            if (blogPost == null)
            {
                return NotFound();
            }

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("BlogPost");

            // Add headers
            worksheet.Cell(1, 1).Value = "Title";
            worksheet.Cell(1, 2).Value = "Short Description";
            worksheet.Cell(1, 3).Value = "Content";
            worksheet.Cell(1, 4).Value = "Featured Image URL";
            worksheet.Cell(1, 5).Value = "Url Handle";
            worksheet.Cell(1, 6).Value = "Published Date";
            worksheet.Cell(1, 7).Value = "Author";
            worksheet.Cell(1, 8).Value = "Is Visible";
            worksheet.Cell(1, 9).Value = "Categories";

            // Add blog post data
            worksheet.Cell(2, 1).Value = blogPost.Title;
            worksheet.Cell(2, 2).Value = blogPost.ShortDescription;
            worksheet.Cell(2, 3).Value = blogPost.Content;
            worksheet.Cell(2, 4).Value = blogPost.FeaturedImageUrl;
            worksheet.Cell(2, 5).Value = blogPost.UrlHandle;
            worksheet.Cell(2, 6).Value = blogPost.PublishedDate.ToString("yyyy-MM-dd");
            worksheet.Cell(2, 7).Value = blogPost.Author;
            worksheet.Cell(2, 8).Value = blogPost.isVisible ? "Yes" : "No";
            worksheet.Cell(2, 9).Value = string.Join(", ", blogPost.Categories.Select(c => c.Name));

            // Auto-fit columns
            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            var fileName = $"BlogPost_{blogPost.Id}.xlsx";
            return File(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        fileName);
        }

    }
}
