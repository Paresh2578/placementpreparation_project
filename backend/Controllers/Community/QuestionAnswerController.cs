using backend.BAL;
using backend.data.Interface;
using backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionAnswerController : ControllerBase
    {
        private readonly QuestionAnswerInterface _questionAnswerRepo;

        public QuestionAnswerController(QuestionAnswerInterface questionAnswerRepo)
        {
            _questionAnswerRepo = questionAnswerRepo;
        }

        [HttpGet("{questionId}")]
        public async Task<IActionResult> GetAllAnswersByQuestionId(Guid questionId)
        {
            var response = await _questionAnswerRepo.GetAllAnswerByQuestionId(questionId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        // [CheckAccess]
        public async Task<IActionResult> PostAnswer([FromBody] QuestionAnswerModel answer)
        {
            var response = await _questionAnswerRepo.PostAnswer(answer);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("{answerId}")]
        // [CheckAccess]
        public async Task<IActionResult> DeleteAnswer(Guid answerId)
        {
            var response = await _questionAnswerRepo.DeleteAnswer(answerId);
            return StatusCode(response.StatusCode, response);
        }
    }
}