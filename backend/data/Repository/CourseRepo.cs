using System.Collections;
using System.Collections.Generic;
using System.Data;
using backend.Constant;
using backend.data.Interface;
using backend.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace backend.data.Repository
{
    public class CourseRepo : CourseInterface
    {
        private readonly ApplicationDbContext _context;
        private readonly DbHelper _dbHelper;


        public CourseRepo(ApplicationDbContext context,DbHelper dbHelper)
        {
            _context = context;
            _dbHelper = dbHelper;
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

        public async Task<ResponseModel> DeleteMultipleCourse(List<Guid> courseIds){
        try{
           // get all course to deleted
            var coursesToDelete = await _context.Courses.Where(course => courseIds.Contains(course.CourseId)).ToListAsync();

            _context.Courses.RemoveRange(coursesToDelete);
            await _context.SaveChangesAsync();
            
            return new ResponseModel{StatusCode=200 , Message="Successfully Deleted all Courses"};

        }catch(Exception ex){
            return new ResponseModel {StatusCode=500,Message=ex.Message};
        }
       }

       
       public async Task<ResponseModel> GetAllCourses(int? limit , string? courseType)
        {
            // remove spce in courseType Name
            if(courseType != null)
            {
                courseType = courseType.Trim();
            }
            try{
                // if limit of data
                var courses = new List<CourseModel>();
                if(limit is null){
                    courses = await _context.Courses.Include(b => b.Branch).Include(c => c.CourseType).Where(c => (c.CourseType.CourseTypeName == courseType || string.IsNullOrEmpty(courseType))).ToListAsync();
                }
                else
                {
                    courses = await _context.Courses.Include(b => b.Branch).Include(c => c.CourseType).Take(Convert.ToInt32(limit)).Where(c => (c.CourseType.CourseTypeName == courseType || string.IsNullOrEmpty(courseType))).ToListAsync();
                }
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

       public async Task<ResponseModel> GetCourseDetailsById(Guid courseId)
        {
            try{
                var courseDetalis = await _context.Courses.Where(x => x.CourseId == courseId).Select(s=> new {s.CourseId,s.Description,s.CourseName}).FirstOrDefaultAsync();
                if(courseDetalis is null){
                    return new ResponseModel { StatusCode = 404, Message = "Course not found." };
                }

                // Get all the topics of the course
                List<object> topics = new List<Object>{};

                // call procedure
                using(SqlCommand command = _dbHelper.getSqlCommand("PR_Topic_GetById")){
                    // add course id parameter
                    command.Parameters.Add("@courseId",SqlDbType.UniqueIdentifier).Value = (object)courseId;

                    // read all topic
                    using(SqlDataReader reader =  await command.ExecuteReaderAsync()){
                        while(await reader.ReadAsync()){
                            topics.Add(new {TopicId=reader.GetGuid(0),TopicName = reader.GetString(1),DifficultyLevel = reader.GetString(2)});
                        }
                    }
                }

                // combine course detais ans topic list
                Dictionary<String,dynamic> data = new Dictionary<string, dynamic>();
                data.Add("course", courseDetalis);
                data.Add("topicList", topics);

                return new ResponseModel { StatusCode = 200, Data = data, Message = "Course retrieved successfully." };
            }catch(Exception e){
                return new ResponseModel { StatusCode = 500, Message = e.Message };
            }
        }

        public async Task<ResponseModel> GetCoursesNameListThatMcqIsAvalible()
        {
            try
            {
                List<dynamic> courseList = new List<dynamic>();
                // get all courses
                using(SqlCommand command = _dbHelper.getSqlCommand("PR_Course_GetAllCourse_Mcq_Avalible"))
                {
                    using(SqlDataReader reader =await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            courseList.Add(new {courseId = reader.GetGuid(0) , courseName=reader.GetString(1) });
                        }
                    }
                }
                return new ResponseModel { StatusCode = 200, Data = courseList, Message = "Course retrieved successfully" };
            }
            catch (Exception e)
            {
                return new ResponseModel { StatusCode = 500, Message = e.Message };
            }
        }

        public async Task<ResponseModel> GetCoursesNameListThatQuestionIsAvalible()
        {
            try
            {
                List<dynamic> courseList = new List<dynamic>();
                // get all courses
                using (SqlCommand command = _dbHelper.getSqlCommand("PR_Course_GetAllCourse_Question_Avalible"))
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            courseList.Add(new { courseId = reader.GetGuid(0), courseName = reader.GetString(1) });
                        }
                    }
                }
                return new ResponseModel { StatusCode = 200, Data = courseList, Message = "Course retrieved successfully" };
            }
            catch (Exception e)
            {
                return new ResponseModel { StatusCode = 500, Message = e.Message };
            }
        }

        public async Task<ResponseModel> GetCoursesByBranchAndCourseType(Guid? branchId , Guid? courseTypeId)
        {
            try
            {
                var course = await _context.Courses.Include(b => b.Branch).Include(c => c.CourseType).Where(c => (c.BranchId.ToString().Trim() == branchId.ToString().Trim() || branchId == null) && (c.CourseTypeId.ToString().Trim() == courseTypeId.ToString().Trim() || courseTypeId== null)).ToListAsync();
                return new ResponseModel {StatusCode= 200 , Data = course , Message = "Course retrieved successfully"};
            }
            catch(Exception e)
            {
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
