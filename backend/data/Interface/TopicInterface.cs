using backend.Models;

namespace backend.data.Interface
{
    public interface TopicInterface
    {
        public Task<ResponseModel> GetAllTopics();
        public Task<ResponseModel> GetTopicById(Guid topicId);
        public Task<ResponseModel> AddTopic(TopicModel topic);
        public Task<ResponseModel> UpdateTopic(TopicModel topic);
        public Task<ResponseModel> DeleteTopic(TopicModel topic);
    }
}
