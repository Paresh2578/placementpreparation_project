using System.Collections;
using backend.data.Interface;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.data.Repository
{
    public class CourseRepo : CourseInterface
    {
        private readonly ApplicationDbContext _context;

        public CourseRepo(ApplicationDbContext context)
        {
            _context = context;
        }
       public async  Task<ResponseModel> AddCourse(CourseModel course)
        {
            try{
               await  _context.Courses.AddAsync(course);
                await _context.SaveChangesAsync();
                return new ResponseModel { StatusCode = 201, Message = "Course added successfully." };

            }catch(Exception e){
                return new ResponseModel { StatusCode = 500, Message = e.Message };
            }
        }

       public async Task<ResponseModel> DeleteCourse(CourseModel course)
        {
            try{ 
                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();
                return new ResponseModel { StatusCode = 200, Message = "Course deleted successfully." };
            }catch(Exception e){
                return new ResponseModel { StatusCode = 500, Message = e.Message};
            }
        }

       public async Task<ResponseModel> GetAllCourses()
        {
            try{
                var courses = await _context.Courses.Include(b => b.Branch).Include(c => c.CourseType).ToListAsync();
                return new ResponseModel { StatusCode = 200, Data = courses  , Message = "Courses retrieved successfully." };
            }catch(Exception e){
                return new ResponseModel { StatusCode = 500, Message = e.Message };
            }
        }

       public async Task<ResponseModel> GetCourseById(Guid courseId)
        {
            try{
                var course = await _context.Courses.Include(b => b.Branch).Include(c => c.CourseType).FirstOrDefaultAsync(x => x.CourseId == courseId);
                if(course is null){
                    return new ResponseModel { StatusCode = 404, Message = "Course not found." };
                }
                return new ResponseModel { StatusCode = 200, Data = course, Message = "Course retrieved successfully." };
            }catch(Exception e){
                return new ResponseModel { StatusCode = 500, Message = e.Message };
            }
        }

       public async Task<ResponseModel> UpdateCourse(CourseModel course)
        {
            try{
                 // Validate if the course id is valid or not
                var courseExist = await _context.Courses.FirstOrDefaultAsync(x => x.CourseId == course.CourseId);
                if(courseExist is null){
                    return new ResponseModel { StatusCode = 404, Message = "Course not found." };
                }

                 _context.Entry(courseExist).CurrentValues.SetValues(course);
                await _context.SaveChangesAsync();
                return new ResponseModel { StatusCode = 200, Message = "Course updated successfully." };
            }catch(Exception ex){
                return new ResponseModel { StatusCode = 500, Message = ex.Message };
            }
        }

      public async Task<ResponseModel> CourseDropdown(){
        try{
           // Get all Course only name and id
           var course = await _context.Courses.Select(x => new {CourseId = x.CourseId,CourseName = x.CourseName }).ToListAsync();
            return new ResponseModel{StatusCode = 200 , Data = course , Message = "Courses  retrieved successfully."};
        }catch(Exception e){
            return new ResponseModel{StatusCode = 500 , Message = e.Message};
        }
      }
}
}
