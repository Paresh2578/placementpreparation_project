using backend.Models;

namespace backend.data.Interface
{
    public interface SubTopicInterface 
    {
        public Task<ResponseModel> GetAllSubTopics();
        public Task<ResponseModel> GetSubTopicById(Guid subTopicId);
        public Task<ResponseModel> CreateSubTopic(SubTopicModel subTopic);
        public Task<ResponseModel> UpdateSubTopic(SubTopicModel subTopic);
        public Task<ResponseModel> DeleteSubTopic(SubTopicModel subTopic);
        public Task<ResponseModel> GetSubTopicsByTopicId(Guid topicId);
        public Task<ResponseModel> GetSubTopicsByCourseId(Guid courseId);
        public Task<ResponseModel> SubTopicDropdown(Guid? courseId , Guid? topicId);
    }
}
