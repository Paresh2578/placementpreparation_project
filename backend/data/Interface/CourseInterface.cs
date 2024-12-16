using backend.Models;

namespace backend.data.Interface
{
    public interface CourseInterface
    {
        public  Task<ResponseModel> GetAllCourses(int? limit , string? courseType);
        public Task<ResponseModel> GetCourseById(Guid courseId);
        public Task<ResponseModel> AddCourse(CourseModel course);
        public Task<ResponseModel> UpdateCourse(CourseModel course);
        public Task<ResponseModel> DeleteCourse(CourseModel course);
        public Task<ResponseModel> CourseDropdown();
        public Task<ResponseModel> DeleteMultipleCourse(List<Guid> courseIds);
        public Task<ResponseModel> GetCoursesByBranchAndCourseType(Guid? branchId, Guid? courseTypeId);
        public Task<ResponseModel> GetCourseDetailsById(Guid courseId);
    }
}
