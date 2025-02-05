using backend.data.Interface;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using backend.BAL;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class FeedbackController : ControllerBase{

        private readonly FeedbackInterface _feedbackRepo;

        public FeedbackController(FeedbackInterface feedbackRepo){
            _feedbackRepo = feedbackRepo;
        }

        #region Add Feedback
        [HttpPost]
        public async Task<IActionResult> AddFeedback([FromBody] FeedbackModel feedback){
            var response = await _feedbackRepo.AddFeedback(feedback);
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region Get All Feedback
        [HttpGet]
        [MainAdminAccess]
        public async Task<IActionResult> GetAllFeedback(){
            var response = await _feedbackRepo.GetAllFeedback();
            return StatusCode(response.StatusCode, response);
        }
        #endregion
    }
}