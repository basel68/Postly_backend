using App1.API.Models.Domain;
using App1.API.Models.DTOs;
using App1.API.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;

namespace App1.API.Controllers
{
    [Route("admin/[controller]")]
    [ApiController]
    public class ImagesController : Controller
    {
        private readonly IImageRepository imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllImages()
        {
            var blogImages= await this.imageRepository.getAllAsync();
            var response=new List<BlogImageDto>();
            foreach (var blogImage in blogImages)
            {
                response.Add(new BlogImageDto
                {
                    Id = blogImage.Id,
                    FileExtension = blogImage.FileExtension,
                    FileName = blogImage.FileName,
                    Title = blogImage.Title,
                    DateCreated = blogImage.DateCreated,
                    Url = blogImage.Url
                });
              
            }
            return Ok(response);    

        }
        [HttpPost]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile file,[FromForm] string fileName,[FromForm] string title)
        {
            ValidateFileUpload(file);
            if (ModelState.IsValid)
            {
                var blogImage = new BlogImage
                {
                    FileExtension = Path.GetExtension(file.FileName).ToLower(),
                    FileName = fileName,
                    Title = title,
                    DateCreated = DateTime.Now,
                };
                blogImage=await imageRepository.upload(file, blogImage);
                //change the domain model to a dto for response
                var response = new BlogImageDto
                {
                    Id = blogImage.Id,
                    FileExtension = blogImage.FileExtension,
                    FileName = blogImage.FileName,
                    Title = blogImage.Title,
                    DateCreated = blogImage.DateCreated,
                    Url = blogImage.Url,
                };
                return Ok(response);

            }
            return BadRequest(ModelState);

        }
        private void ValidateFileUpload(IFormFile file)
        {
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };
            if (!allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower())) {
                ModelState.AddModelError("file", "File Extension is not supported");           
            }
            if (file.Length > 1048576)
            {
                ModelState.AddModelError("file", "Image size is very large");
            }
        }
    }
}
