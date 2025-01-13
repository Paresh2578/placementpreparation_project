using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using backend.Constant;

namespace Placement_Preparation.Areas.Admin.Models
{
    public class DashboardModel
    {
        public int? TotalCourses { get; set; }
        public int? TotalCoursesQuestions { get; set; }
        public int? TotalCoursesMcqs { get; set; }
        public int? TotalInterviewQuestions { get; set; }
        public int? TotalInterviewMcqs { get; set; }
        public List<Dictionary<string,dynamic>>? PaddingStudentRequest { get; set; }
        public List<McqModel>? PaddingMcqRequest { get; set; }
        public List<QuestionModel>? PaddingQuestionRequest { get; set; }
    }
}