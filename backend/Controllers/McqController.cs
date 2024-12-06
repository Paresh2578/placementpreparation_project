using backend.Constant;
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
        //  [CheckAccess]
        [HttpPost]
        public async Task<IActionResult> AddMcq(McqModel mcq)
        {
            var response = await _mcqInterface.AddMcq(mcq);
            return StatusCode(response.StatusCode, response);
        }
        #endregion

         #region Delete Mcq
        //  [CheckAccess]
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
        public async Task<IActionResult> GetAllMcq([FromQuery] Guid? courseId , [FromQuery] Guid? topicId , [FromQuery] Guid? subTopicId)
        {
            var response = await _mcqInterface.GetAllMcq(courseId:courseId , topicId : topicId , subTopicId : subTopicId);
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region Get Mcq By Id
        [HttpGet("{mcqId}")]
        public async Task<IActionResult> GetMcqById(Guid mcqId)
        {
            var response = await _mcqInterface.GetMcqById(mcqId);
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region Update Mcq
        // [CheckAccess]
        [HttpPut]
        public async Task<IActionResult> UpdateMcq(McqModel mcq)
        {
            var response = await _mcqInterface.UpdateMcq(mcq);
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region Delete Multiple Mcq
        [HttpDelete("DeleteMultiple")]
        public async Task<IActionResult> DeleteMultipleMcq([FromBody] List<Guid> mcqIds){
            var response = await _mcqInterface.DeleteMultipleMcq(mcqIds);
            return StatusCode(response.StatusCode , response);
        }
        #endregion
    }
}
