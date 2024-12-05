using backend.Constant;
using backend.data.Interface;
using backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubTopicController : ControllerBase
    {
        private readonly SubTopicInterface _subTopicInterface;
        public SubTopicController(SubTopicInterface subTopicInterface)
        {
            _subTopicInterface = subTopicInterface;
        }

        #region Get All SubTopics
        [HttpGet]
        public async Task<IActionResult> GetAllSubTopics()
        {
            var response = await _subTopicInterface.GetAllSubTopics();
            return StatusCode(response.StatusCode, response);
        }
        #endregion
      
        #region Get SubTopic By Id
        [HttpGet("{subTopicId}")]
        public async Task<IActionResult> GetSubTopicById(Guid subTopicId)
        {
            var response = await _subTopicInterface.GetSubTopicById(subTopicId);
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region Create SubTopic
        // [CheckAccess]
        [HttpPost]
        public async Task<IActionResult> CreateSubTopic([FromBody] SubTopicModel  subTopic)
        {
             var response = await _subTopicInterface.CreateSubTopic(subTopic);
            return StatusCode(response.StatusCode, response);



           // return StatusCode(200 , new ResponseModel() { StatusCode = 201 , Message="Done"});
        }
        #endregion

        #region Update SubTopic
        // [CheckAccess]
        [HttpPut]
        public async Task<IActionResult> UpdateSubTopic([FromBody] SubTopicModel subTopic)
        {
            var response = await _subTopicInterface.UpdateSubTopic(subTopic);
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region Delete SubTopic
        // [CheckAccess]
        [HttpDelete("{subTopicId}")]
        public async Task<IActionResult> DeleteSubTopic(Guid subTopicId)
        {
            ResponseModel response = await _subTopicInterface.GetSubTopicById(subTopicId);
            if (response.StatusCode != 200)
            {
                return StatusCode(response.StatusCode, response);
            }

             response = await _subTopicInterface.DeleteSubTopic(response.Data);
            return StatusCode(response.StatusCode, response);
        }
        #endregion


        # region GetSubTopicsByTopicId
        [HttpGet("GetSubTopicsByTopicId/{topicId}")]
        public async Task<IActionResult> GetSubTopicsByTopicId(Guid topicId)
        {
            var response = await _subTopicInterface.GetSubTopicsByTopicId(topicId);
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region  Get Sub Topics by Course
        [HttpGet("GetSubTopicsByCourseId/{courseId}")]
        public async Task<IActionResult> GetSubTopicsByCourseId(Guid courseId){
            var response = await _subTopicInterface.GetSubTopicsByCourseId(courseId);
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region SubTopicDropdown
        [HttpGet("Dropdown")]
        public async Task<IActionResult> SubTopicDropdown([FromQuery] Guid courseId , [FromQuery] Guid topicId)
        {
            var response = await _subTopicInterface.SubTopicDropdown(courseId: courseId,topicId: topicId);
            return StatusCode(response.StatusCode, response);
        }
        #endregion
    }
}
