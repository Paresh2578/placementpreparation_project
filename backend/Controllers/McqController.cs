using backend.BAL;
using backend.data.Interface;
using backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class McqController : ControllerBase
    {
        private readonly McqInterface _mcqInterface;
        public McqController(McqInterface mcqInterface)
        {
            _mcqInterface = mcqInterface;
        }

         #region  Add Mcq
        [CheckAccess]
        [HttpPost]
        public async Task<IActionResult> AddMcq(McqModel mcq)
        {
             // if course id is null when add addedd by id from token
            if(mcq.CourseId == null && mcq.TopicId == null){
                var userData = CV.GetUserDataFromToken(HttpContext.Request.Cookies["token"]);
                if(userData is null || !userData.ContainsKey("AdminUserId")){
                    return Unauthorized();
                }
                mcq.AddedBy = Guid.Parse(userData["AdminUserId"]);
            }
            var response = await _mcqInterface.AddMcq(mcq);
            return StatusCode(response.StatusCode, response);
        }
        #endregion

         #region Delete Mcq
        [CheckAccess]
        [HttpDelete("{mcqId}")]
        public async Task<IActionResult> DeleteMcq(Guid mcqId)
        {
            var response = await _mcqInterface.GetMcqById(mcqId);
            if(response.StatusCode != 200){
                return StatusCode(response.StatusCode, response);
            }
             response = await _mcqInterface.DeleteMcq(response.Data);
            return StatusCode(response.StatusCode, response);
        }
        #endregion

         #region Get All Mcq
        [HttpGet]
        public async Task<IActionResult> GetAllMcq([FromQuery] Guid? courseId , [FromQuery] Guid? topicId , [FromQuery] Guid? subTopicId, [FromQuery] int? pageNumber, [FromQuery] int? pageSize = 5, [FromQuery] bool onlyActiveMcqs = false)
        {
            var response = await _mcqInterface.GetAllMcq(courseId:courseId , topicId : topicId , subTopicId : subTopicId , onlyActiveMcqs: onlyActiveMcqs,pageNumber:pageNumber , pageSize:pageSize);
                return StatusCode(response.StatusCode, response);
        }
        #endregion

        //#region Get All Active Mcq 
        //[HttpGet("Active")]
        //public async Task<IActionResult> GetAllActiveMcq([FromQuery] Guid? courseId, [FromQuery] Guid? topicId, [FromQuery] Guid? subTopicId)
        //{
        //    var response = await _mcqInterface.GetAllActiveMcq(courseId: courseId, topicId: topicId, subTopicId: subTopicId);
        //    return StatusCode(response.StatusCode, response);
        //}
        //#endregion

        #region Get Mcq By Id
        [HttpGet("{mcqId}")]
        public async Task<IActionResult> GetMcqById(Guid mcqId)
        {
            var response = await _mcqInterface.GetMcqById(mcqId);
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region Update Mcq
        [CheckAccess]
        [HttpPut]
        public async Task<IActionResult> UpdateMcq(McqModel mcq)
        {
            var response = await _mcqInterface.UpdateMcq(mcq);
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region Delete Multiple Mcq
        [HttpDelete("DeleteMultiple")]
        [CheckAccess]
        public async Task<IActionResult> DeleteMultipleMcq([FromBody] List<Guid> mcqIds){
            var response = await _mcqInterface.DeleteMultipleMcq(mcqIds);
            return StatusCode(response.StatusCode , response);
        }
        #endregion

        #region Get Interview Mcq
        [HttpGet("GetInterviewMcqs")]
        public async Task<IActionResult> GetInterviewMcqs()
        {
            var response = await _mcqInterface.GetInterviewMcqs();
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        
        #region Update new  Interview Mcq Request
       [HttpPut]
       [MainAdminAccess]
       [Route("UpdateNewInterviewMcqRequestStatus/{id}/{Status}")]
       public async Task<IActionResult> UpdateNewInterviewMcqRequestStatus(Guid id , string Status)
       {
           ResponseModel response = await _mcqInterface.UpdateNewInterviewMcqRequestStatus(id , Status);
           return StatusCode(response.StatusCode, response);
       }
       #endregion

       /* #region Get Mcqs by topic or subTopic Id
        [HttpGet("GetMcqsByTopicOrSubTopicId/{topicId}/{subTopicId?}")]
        public async Task<IActionResult> GetMcqsByTopicOrSubTopicId([FromRoute] Guid topicId ,[FromRoute] Guid? subTopicId=null)
        {
            ResponseModel response = await _mcqInterface.GetMcqsByTopicOrSubTopicId(topicId: topicId, subTopicId: subTopicId);
            return StatusCode(response.StatusCode, response);
        }
        #endregion
       */
    }
}
