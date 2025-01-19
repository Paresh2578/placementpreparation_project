using backend.BAL;
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
        private readonly IHttpContextAccessor _httpContextAccessor;
        public QuestionController(QuestionInterface questionInterface, IHttpContextAccessor httpContextAccessor)
        {
            _questionInterface = questionInterface;
            _httpContextAccessor = httpContextAccessor;
        }

        #region Get all questions
        [HttpGet]
        public async Task<IActionResult> GetAllQuestions([FromQuery] Guid? courseId , [FromQuery] Guid? topicId , [FromQuery] Guid? subTopicId, [FromQuery] int? pageNumber , [FromQuery] int? pageSize=5, [FromQuery] bool onlyActiveQuestions= false)
        {
            var response = await _questionInterface.GetAllQuestions(courseId:courseId , topicId : topicId , subTopicId : subTopicId,pageNumber:pageNumber,pageSize:pageSize , onlyActiveQuestions: onlyActiveQuestions);
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
        [CheckAccess]
        [HttpPost]
        public async Task<IActionResult> AddQuestion([FromBody] QuestionModel question)
        {
            // if course id is null when add addedd by id from token
            if(question.CourseId == null && question.TopicId == null){
                var userData = CV.GetUserDataFromToken(_httpContextAccessor.HttpContext.Request.Cookies["token"]);
                if(userData is null || !userData.ContainsKey("AdminUserId")){
                    return Unauthorized();
                }
                question.AddedBy = Guid.Parse(userData["AdminUserId"]);
            }

            var response = await _questionInterface.AddQuestion(question);
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region Update Question
        [CheckAccess]
        [HttpPut]
        public async Task<IActionResult> UpdateQuestion([FromBody] QuestionModel question)
        {
            var response = await _questionInterface.UpdateQuestion(question);
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region Delete Question
        [CheckAccess]
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
        [CheckAccess]
        public async Task<IActionResult> DeleteMultipleMcq([FromBody] List<Guid> questionIds){
            var response = await _questionInterface.DeleteMultipleQuestion(questionIds);
            return StatusCode(response.StatusCode , response);
        }
        #endregion

        #region Get Interview questions
        [HttpGet("InterviewQuestions")]
        public async Task<IActionResult> GetInterviewQuestions(Guid? addedById , [FromQuery] int? pageNumber ,[FromQuery] string? companyName , [FromQuery] string? techStack,[FromQuery] int? pageSize=5, [FromQuery] bool onlyActiveQuestions= false,[FromQuery]  bool withAddedByDetails = false,[FromQuery] bool onlyAcceptApprovalStatus=false) 
        {
            var response = await _questionInterface.GetInterviewQuestions(addedById,pageNumber:pageNumber,pageSize:pageSize , onlyActiveQuestions: onlyActiveQuestions,withAddedByDetails:withAddedByDetails,companyName:companyName??"".Trim(),techStack:techStack??"".Trim(),onlyAcceptApprovalStatus:onlyAcceptApprovalStatus);
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region Update new  Interview Question Request
       [HttpPut]
       [MainAdminAccess]
       [Route("UpdateNewInterviewQuestionRequestStatus/{id}/{Status}")]
       public async Task<IActionResult> UpdateNewInterviewQuestionRequestStatus(Guid id , string Status)
       {
           ResponseModel response = await _questionInterface.UpdateNewInterviewQuestionRequestStatus(id , Status);
           return StatusCode(response.StatusCode, response);
       }
       #endregion
    
    
       #region  get all uniqe company names and TechStack
        [HttpGet]
        [Route("GetAllUniqueCompanyNamesAndTechStack")]
        public async Task<IActionResult> GetAllUniqueCompanyNamesAndTechStack()
        {
            var response = await _questionInterface.GetAllUniqueCompanyNamesAndTechStack();
            return StatusCode(response.StatusCode, response);
        }
        #endregion
    }
}
