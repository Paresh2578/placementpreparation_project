using backend.Constant;
using backend.data.Interface;
using backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly QuestionInterface _questionInterface;
        public QuestionController(QuestionInterface questionInterface)
        {
            _questionInterface = questionInterface;
        }

        #region Get all questions
        [HttpGet]
        public async Task<IActionResult> GetAllQuestions([FromQuery] Guid? courseId , [FromQuery] Guid? topicId , [FromQuery] Guid? subTopicId)
        {
            var response = await _questionInterface.GetAllQuestions(courseId:courseId , topicId : topicId , subTopicId : subTopicId);
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region Get question by id
        [HttpGet("{questionId}")]
        public async Task<IActionResult> GetQuestionById(Guid questionId)
        {
            var response = await _questionInterface.GetQuestionById(questionId);
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region Add Question
        // [CheckAccess]
        [HttpPost]
        public async Task<IActionResult> AddQuestion([FromBody] QuestionModel question)
        {
            var response = await _questionInterface.AddQuestion(question);
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region Update Question
        // [CheckAccess]
        [HttpPut]
        public async Task<IActionResult> UpdateQuestion([FromBody] QuestionModel question)
        {
            var response = await _questionInterface.UpdateQuestion(question);
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region Delete Question
        // [CheckAccess]
        [HttpDelete("{questionId}")]
        public async Task<IActionResult> DeleteQuestion(Guid questionId)
        {
            var question = await _questionInterface.GetQuestionById(questionId);
            if (question.StatusCode != 200)
            {
                return StatusCode(question.StatusCode, question);
            }
            var response = await _questionInterface.DeleteQuestion(question.Data);
            return StatusCode(response.StatusCode, response);
        }
        #endregion
    
        #region Delete Multiple Mcq
        [HttpDelete("DeleteMultiple")]
        public async Task<IActionResult> DeleteMultipleMcq([FromBody] List<Guid> questionIds){
            var response = await _questionInterface.DeleteMultipleQuestion(questionIds);
            return StatusCode(response.StatusCode , response);
        }
        #endregion
    }
}
