using Microsoft.AspNetCore.Mvc;

namespace Placement_Preparation.Areas.Admin.Data.Interface
{
    public interface TopicInterface
    {
        public Task<JsonResult> GetTopicsByCourseId(string courseId);  
        public Task<JsonResult> GetTopicsLengthByCourseId(string courseId);
    }
}
