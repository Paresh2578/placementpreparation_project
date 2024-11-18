using backend.Models;

namespace backend.data.Interface
{
    public interface CourseInterface
    {
        public  Task<ResponseModel> GetAllCourses();
        public Task<ResponseModel> GetCourseById(Guid courseId);
        public Task<ResponseModel> AddCourse(CourseModel course);
        public Task<ResponseModel> UpdateCourse(CourseModel course);
        public Task<ResponseModel> DeleteCourse(CourseModel course);
    }
}
