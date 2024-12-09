using backend.Models;

namespace backend.data.Interface
{
    public interface CourseTypeInterface
    {
        public Task<ResponseModel> AddCourseType(CourseTypeModel courseType);
        public Task<ResponseModel> UpdateCourseType(CourseTypeModel courseType);
        public Task<ResponseModel> DeleteCourseType(CourseTypeModel courseType);
        public Task<ResponseModel> GetCourseTypeById(Guid courseTypeId);
        public Task<ResponseModel> GetAllCourseTypes();
        public Task<ResponseModel> DeleteMultipleCourseType(List<Guid> courseTypeIds);

    }
}
