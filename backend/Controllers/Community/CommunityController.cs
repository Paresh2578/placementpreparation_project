using backend.BAL;
using backend.data.Interface;
using backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommunityController : ControllerBase
    {
        private readonly CommunityInterface _communityRepo;

        public CommunityController(CommunityInterface communityRepo)
        {
            _communityRepo = communityRepo;
        }

        #region  get All Questions
        [HttpGet]
        public async Task<IActionResult> GetAllQuestions()
        {
            var response = await _communityRepo.GetAllQuestions();
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region get Question by id
        [HttpGet("{questionId:guid}")]
        public async Task<IActionResult> GetQuestionById(Guid questionId)
        {
            var response = await _communityRepo.GetQuestionById(questionId);
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region ask Question
        [HttpPost("AskQuestion")]
        // [CheckAccess]
        public async Task<IActionResult> AskQuestion([FromBody] PostModel question)
        {
            var response = await _communityRepo.AskQuestion(question);
            return StatusCode(response.StatusCode, response);
        }
        #endregion
    }
}