using Microsoft.AspNetCore.Mvc;

namespace Placement_Preparation.Areas.Admin.Data.Interface
{
    public interface SubTopicInterface
    {
        public Task<JsonResult> GetSubTopicsByTopicId(string topicId);  
    }
}
