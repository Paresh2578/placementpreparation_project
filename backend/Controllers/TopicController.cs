using backend.data.Interface;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using backend.Constant;
using System.Web.Http.Routing.Constraints;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopicController : ControllerBase
    {
        private readonly TopicInterface _topicInterface;
        public TopicController(TopicInterface topicInterface)
        {
            _topicInterface = topicInterface;
        }

        #region Get All Topics
        [HttpGet]
        public async Task<IActionResult> GetAllTopics()
        {
            var response = await _topicInterface.GetAllTopics();
            return StatusCode(response.StatusCode,response);
        }
        #endregion

        #region Get Topic By Id
        [HttpGet("{courseId}")]
        public async Task<IActionResult> GetTopicById(Guid courseId)
        {
            var response = await _topicInterface.GetTopicById(courseId);
            return StatusCode(response.StatusCode,response);
        }
        #endregion

        #region Add Topic
        // [CheckAccess]
        [HttpPost]
        public async Task<IActionResult> AddTopic([FromBody] TopicModel topic)
        {
            var response = await _topicInterface.AddTopic(topic);
            return StatusCode(response.StatusCode,response);
        }
        #endregion

        #region Update Topic
        // [CheckAccess]
        [HttpPut]
        public async Task<IActionResult> UpdateTopic([FromBody] TopicModel topic)
        {
            var response = await _topicInterface.UpdateTopic(topic);
            return StatusCode(response.StatusCode,response);
        }
        #endregion

        #region Delete Topic
        // [CheckAccess]
        [HttpDelete("{courseId}")]
        public async Task<IActionResult> DeleteTopic(Guid courseId)
        {
            // Validate if the topic exists or not
            ResponseModel topic = await _topicInterface.GetTopicById(courseId);
            if(topic.StatusCode != 200){
                return StatusCode(topic.StatusCode,topic);
            }

            var response = await _topicInterface.DeleteTopic(topic.Data);
            return StatusCode(response.StatusCode,response);
        }
        #endregion

        #region Get Topics By Course Id
        [HttpGet("GetTopicsByCourseId/{courseId}")]
        public async Task<IActionResult> GetTopicsByCourseId(Guid courseId)
        {
            var response = await _topicInterface.GetTopicsByCourseId(courseId);
            return StatusCode(response.StatusCode,response);
        }
        #endregion

        #region Topic Dropdown
        [HttpGet("Dropdown")]
        public async Task<IActionResult> TopicDropdown([FromQuery] Guid courseId)
        {
            var response = await _topicInterface.TopicDropdown(courseId:courseId);
            return StatusCode(response.StatusCode,response);
        }
        #endregion

        #region Delete Multiple Sub Topic
        [HttpDelete("DeleteMultiple")]
        public async Task<IActionResult> DeleteMultipleMcq([FromBody] List<Guid> topicIds){
            var response = await _topicInterface.DeleteMultipleTopic(topicIds);
            return StatusCode(response.StatusCode , response);
        }
        #endregion
        
        #region  Get Topics Length By Course Id
        [HttpGet("GetTopicsLengthByCourseId/{courseId}")]
        public async Task<IActionResult> GetTopicsLengthByCourseId(Guid courseId)
        {
            var response = await _topicInterface.GetTopicsLengthByCourseId(courseId);
            return StatusCode(response.StatusCode,response);
        }
        #endregion

        #region Get All Topic With SubTopic List By CourseId
        [HttpGet("GetSidebarDataByCourseIdAndTopicDocumentionByTopicId/{courseId}/{topicId}/{subTopicId?}")]
        public async Task<IActionResult> GetSidebarDataByCourseIdAndTopicDocumentionByTopicId(Guid courseId , Guid topicId , Guid? subTopicId)
        {
            ResponseModel response = await _topicInterface.GetSidebarDataByCourseIdAndTopicDocumentionByTopicId(courseId: courseId  , topicId:topicId , subTopicId : subTopicId);
            return StatusCode(response.StatusCode, response);
        }
        #endregion
    }
}
