﻿using backend.Models;

namespace backend.data.Interface
{
    public interface TopicInterface
    {
        public Task<ResponseModel> GetAllTopics();
        public Task<ResponseModel> GetTopicById(Guid topicId);
        public Task<ResponseModel> AddTopic(TopicModel topic);
        public Task<ResponseModel> UpdateTopic(TopicModel topic);
        public Task<ResponseModel> DeleteTopic(TopicModel topic);
        public Task<ResponseModel> GetTopicsByCourseId(Guid courseId);

        public Task<ResponseModel> TopicDropdown(Guid? courseId);

        public Task<ResponseModel> DeleteMultipleTopic(List<Guid> topicIds); 
        public Task<ResponseModel> GetTopicsLengthByCourseId(Guid courseId);

        public Task<ResponseModel> GetSidebarDataByCourseIdAndTopicDocumentionByTopicId(Guid courseId,Guid topicId , Guid? subTopicId);
    }
}
