using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageUploadController : ControllerBase
    {
       private readonly Cloudinary _cloudinary;

        public ImageUploadController(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        }

         #region Upload Image
         [HttpPost("UploadImage")]
         public async Task<IActionResult> uploadImage(IFormFile image){
           try{
                //validation
                if(image == null || image.Length == 0)
                {
                    return BadRequest("Image is required");
                }

                using(var stream = image.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(image.FileName, stream),
                    };
                    var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                    return Ok(new {Url = uploadResult.Url});
                }
           }catch(Exception ex){
               return StatusCode(500, new { message = ex.Message });
           }
         }
         #endregion
    }
}
