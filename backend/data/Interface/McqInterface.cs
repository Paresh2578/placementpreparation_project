﻿using backend.Models;

namespace backend.data.Interface
{
    public interface McqInterface
    {
        public Task<ResponseModel> GetAllMcq(Guid? courseId , Guid? topicId , Guid? subTopicId, int? pageSize, int? pageNumber, bool onlyActiveMcqs);
      //  public Task<ResponseModel> GetAllActiveMcq(Guid? courseId, Guid? topicId, Guid? subTopicId);
        public Task<ResponseModel> GetMcqById(Guid mcqId);
        public Task<ResponseModel> AddMcq(McqModel mcq);
        public Task<ResponseModel> UpdateMcq(McqModel mcq);
        public Task<ResponseModel> DeleteMcq(McqModel mcq);

        public Task<ResponseModel> DeleteMultipleMcq(List<Guid> mcqIds);
        public Task<ResponseModel> GetMcqsByTopicOrSubTopicId(Guid topicId, Guid? subTopicId);
         public Task<ResponseModel> GetInterviewMcqs(int? pageNumber,int? pageSize,string? companyName , string? techStack,bool onlyActiveMcqs,bool? withAddedByDetails,bool onlyAcceptApprovalStatus); 
         public Task<ResponseModel> UpdateNewInterviewMcqRequestStatus(Guid id , String status); 
         public Task<ResponseModel> GetAllUniqueCompanyNamesAndTechStack(); 
         

    }
}
