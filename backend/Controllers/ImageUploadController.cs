using backend.Models;
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
         [HttpPost]
         public async Task<IActionResult> uploadImage(IFormFile image){
           try{
                //validation
                if(image == null || image.Length == 0)
                {
                    ResponseModel response = new ResponseModel(){
                        StatusCode = 400,
                        Message = "Image is required",
                    };
                    return StatusCode(response.StatusCode , response);
                }
                // Validate file size (3MB = 3 * 1024 * 1024 bytes)
                if (image.Length > 3 * 1024 * 1024)
                {
                    return StatusCode(400 , new ResponseModel
                    {
                        StatusCode = 400,
                        Message = "Image size must not exceed 3MB."
                    });
                }

                using(var stream = image.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(image.FileName, stream),
                    };
                    var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                    // create response
                    ResponseModel response = new ResponseModel(){
                        StatusCode = 200,
                        Message = "Image uploaded successfully.",
                        Data = uploadResult.Url.ToString()
                    };
                    return StatusCode(response.StatusCode, response);
                }
           }catch(Exception ex){
               return StatusCode(500, new ResponseModel{ Message = ex.Message , StatusCode = 500 });
           }
         }
         #endregion
    }
}
