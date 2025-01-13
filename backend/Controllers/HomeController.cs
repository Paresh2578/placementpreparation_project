using System.Data;
using backend.BAL;
using backend.Constant;
using backend.data;
using backend.data.Interface;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [CheckAccess]
    public class HomeController : ControllerBase
    {
        private readonly DbHelper _dbHelper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _context;
        public HomeController(DbHelper dbHelper, IHttpContextAccessor httpContextAccessor, ApplicationDbContext context)
        {
            _dbHelper = dbHelper;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        #region Get Dashboard Data
        /*
                 {
                  TotalCourse : 10, admin
                  TotalCoursesQuestions : 10, admin
                  TotalCoursesMcqs : 10, admin
                  TotalInterviewQuestions : 10,
                  TotalInterviewMcqs : 10,
                  PaddingStudentRequest : [] , admin
                  PaddingMcqRequest : [],
                  PaddingQuestionRequest : [] 
                 }
                */
        [HttpGet]
        [Route("GetDashboardData")]
        public async Task<IActionResult> GetDashboardData()
        {
            try{

                // get user data from token
                Dictionary<string,dynamic>? userData = CV.GetUserDataFromToken(_httpContextAccessor.HttpContext.Request.Cookies["token"]);
                if(userData is null){
                    return StatusCode(401, new ResponseModel { StatusCode = 401, Message = "Unauthorized" });
                }


                Dictionary<string, object> data = new Dictionary<string, object>();
                string adminUserId = userData["AdminUserId"];

                // get total course, total course question, total course mcq , total interview question, total interview mcq from database using strore procedure
                using(SqlCommand com = _dbHelper.getSqlCommand("PR_GET_Dashbord_Data"))
                {
                    com.Parameters.AddWithValue("@AdminId", SqlDbType.UniqueIdentifier).Value = adminUserId;
                    com.Parameters.AddWithValue("@IsAdmin", SqlDbType.Bit).Value = userData["IsAdmin"];

                    using(SqlDataReader reader = await com.ExecuteReaderAsync()){
                        while(reader.Read()){
                            data.Add("TotalCourses", reader["TotalCourses"]);
                            data.Add("TotalCoursesQuestions", reader["TotalCoursesQuestions"]);
                            data.Add("TotalCoursesMcqs", reader["TotalCoursesMcqs"]);
                            data.Add("TotalInterviewQuestions", reader["TotalInterviewQuestions"]);
                            data.Add("TotalInterviewMcqs", reader["TotalInterviewMcqs"]);
                        }
                    }
                }

                //add  padding New Student Request , Padding New Mcq Request , Padding New Question Request
                if(userData["IsAdmin"]){
                    List<AdminUserModel> paddingStudentRequest = await _context.AdminUsers.Where(user => user.ApproveStatus == "Pending").ToListAsync();
                    List<QuestionModel> paddingQuestionRequest = await _context.Questions.Where(question => question.ApproveStatus == "Pending").ToListAsync();
                    // List<McqModel> paddingMcqRequest = await _context.Mcqs.Where(mcq => mcq.ApproveStatus == "Pending").ToListAsync();
                    List<McqModel> paddingMcqRequest = new List<McqModel>{};
                    
                    data.Add("PaddingStudentRequest", paddingStudentRequest);
                    data.Add("PaddingQuestionRequest", paddingQuestionRequest);
                    data.Add("PaddingMcqRequest", paddingMcqRequest);
                }else{
                    List<QuestionModel> paddingQuestionRequest = await _context.Questions.Where(question => question.ApproveStatus == "Pending" && question.AddedBy.ToString() == adminUserId).ToListAsync();
                    List<McqModel> paddingMcqRequest = new List<McqModel>{};
                    // List<McqModel> paddingMcqRequest = await _context.Mcqs.Where(mcq => mcq.ApproveStatus == "Pending").ToListAsync();

                    data.Add("PaddingStudentRequest", null);
                    data.Add("PaddingQuestionRequest", paddingQuestionRequest);
                    data.Add("PaddingMcqRequest", paddingMcqRequest);
                }

                return Ok(new ResponseModel { StatusCode = 200, Message = "Successfully Get Data", Data = data });
            }catch(Exception ex){
                return StatusCode(500, new ResponseModel { StatusCode =500, Message = ex.Message });
            }
        }
        #endregion
    }
}