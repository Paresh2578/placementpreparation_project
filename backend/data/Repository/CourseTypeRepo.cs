using backend.data.Interface;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.data.Repository
{
    public class CourseTypeRepo : CourseTypeInterface
    {
        private readonly ApplicationDbContext _context;
        public CourseTypeRepo(ApplicationDbContext context)
        {
            _context = context;
        }
       public async Task<ResponseModel> AddCourseType(CourseTypeModel courseType)
        {
            try{
                await _context.courseTypes.AddAsync(courseType);
                await _context.SaveChangesAsync();
                return new ResponseModel { StatusCode= 201, Message = "Course Type Added Successfully" };
            }catch{
                return new ResponseModel { StatusCode= 500, Message = "Internal Server Error" };
            }
        }

       public async Task<ResponseModel> DeleteCourseType(CourseTypeModel courseType)
        {
            try{
                // Delete course type
                _context.courseTypes.Remove(courseType);
                    await _context.SaveChangesAsync();
                    return new ResponseModel { StatusCode= 200, Message = "Course Type Deleted Successfully" };
                
            }catch{
               return new ResponseModel { StatusCode= 500, Message = "Internal Server Error" };   
            }
        }

       public async Task<ResponseModel> GetAllCourseTypes()
        {
            try{
                var courseTypes = await _context.courseTypes.ToListAsync();
                return new ResponseModel { StatusCode= 200, Message = "Successfully Get All CourseTypes", Data = courseTypes };
            }catch{
                return new ResponseModel { StatusCode= 500, Message = "Internal Server Error" };
            }
        }

       public async Task<ResponseModel> GetCourseTypeById(Guid courseTypeId)
        {
            try{
                var courseType = await _context.courseTypes.FirstOrDefaultAsync(x => x.CourseTypeId == courseTypeId);
                if (courseType is null)
                {
                    return new ResponseModel { StatusCode= 404, Message = "Course Type Not Found" };
                }
                return new ResponseModel { StatusCode= 200, Message = "Successfully Get CourseType", Data = courseType };
            }catch{
                return new ResponseModel { StatusCode= 500, Message = "Internal Server Error" };
            }
        }

       public async Task<ResponseModel> UpdateCourseType(CourseTypeModel courseType)
        {
            try{
                // Check if course type exists
                var courseTypeExist = await _context.courseTypes.FirstOrDefaultAsync(x => x.CourseTypeId == courseType.CourseTypeId);
                if (courseTypeExist is null)
                {
                    return new ResponseModel { StatusCode= 404, Message = "Course Type Not Found" };
                }

                // Update course type
                courseTypeExist.CourseTypeName = courseType.CourseTypeName;
                await _context.SaveChangesAsync();
                return new ResponseModel { StatusCode= 200, Message = "Course Type Updated Successfully" };
            }catch{
                return new ResponseModel { StatusCode= 500, Message = "Internal Server Error" };
            }
        }
    }
}
