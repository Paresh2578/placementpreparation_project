using backend.Models;

namespace backend.data.Interface
{
    public interface McqInterface
    {
        public Task<ResponseModel> GetAllMcq(Guid? courseId , Guid? topicId , Guid? subTopicId);
        public Task<ResponseModel> GetMcqById(Guid mcqId);
        public Task<ResponseModel> AddMcq(McqModel mcq);
        public Task<ResponseModel> UpdateMcq(McqModel mcq);
        public Task<ResponseModel> DeleteMcq(McqModel mcq);

    }
}
