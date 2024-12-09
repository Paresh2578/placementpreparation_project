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
                await _context.CourseTypes.AddAsync(courseType);
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
                _context.CourseTypes.Remove(courseType);
                    await _context.SaveChangesAsync();
                    return new ResponseModel { StatusCode= 200, Message = "Course Type Deleted Successfully" };
                
            }catch{
               return new ResponseModel { StatusCode= 500, Message = "Internal Server Error" };   
            }
        }

        public async Task<ResponseModel> DeleteMultipleCourseType(List<Guid> courseTypeIds){
        try{
           // get all course Type to deleted
            var coursesToDelete = await _context.CourseTypes.Where(courseType => courseTypeIds.Contains(courseType.CourseTypeId)).ToListAsync();

            _context.CourseTypes.RemoveRange(coursesToDelete);
            await _context.SaveChangesAsync();
            
            return new ResponseModel{StatusCode=200 , Message="Successfully Deleted all Courses Type"};

        }catch(Exception ex){
            return new ResponseModel {StatusCode=500,Message=ex.Message};
        }
       }

       public async Task<ResponseModel> GetAllCourseTypes()
        {
            try{
                var courseTypes = await _context.CourseTypes.ToListAsync();
                return new ResponseModel { StatusCode= 200, Message = "Successfully Get All CourseTypes", Data = courseTypes };
            }catch{
                return new ResponseModel { StatusCode= 500, Message = "Internal Server Error" };
            }
        }

       public async Task<ResponseModel> GetCourseTypeById(Guid courseTypeId)
        {
            try{
                var courseType = await _context.CourseTypes.FirstOrDefaultAsync(x => x.CourseTypeId == courseTypeId);
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
                var courseTypeExist = await _context.CourseTypes.FirstOrDefaultAsync(x => x.CourseTypeId == courseType.CourseTypeId);
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
