using Microsoft.AspNetCore.Mvc.Rendering;
using Placement_Preparation.Areas.Admin.Models;

namespace Placement_Preparation.Utils
{
    public class AllDropDown
    {
        public static List<SelectListItem> CourseType()
        {
            List<SelectListItem> courseList = new List<SelectListItem>
                {
                    new SelectListItem { Value = "1", Text = "Computer Science" },
                    new SelectListItem { Value = "2", Text = "Mechanical Engineering" },
                    new SelectListItem { Value = "3", Text = "Civil Engineering" },
                    new SelectListItem { Value = "4", Text = "Electrical Engineering" },
                    new SelectListItem { Value = "5", Text = "Business Administration" },
                    new SelectListItem { Value = "6", Text = "Data Science" },
                    new SelectListItem { Value = "7", Text = "Psychology" },
                    new SelectListItem { Value = "8", Text = "Marketing" },
                    new SelectListItem { Value = "9", Text = "Graphic Design" },
                    new SelectListItem { Value = "10", Text = "Artificial Intelligence" }
                };
            return courseList;
        }

        public static List<SelectListItem> Branch()
        {
            List<SelectListItem> branchList = new List<SelectListItem>
                {
                  new SelectListItem {Value = "1", Text= "Computer Science" },
                new SelectListItem  { Value = "2", Text= "Mechanical Engineering" },
                new SelectListItem  { Value = "3", Text= "Civil Engineering" },
                new SelectListItem  { Value = "4", Text= "Electrical Engineering" },
                new SelectListItem  { Value = "5", Text= "Business Administration" },
                new SelectListItem  { Value = "6", Text= "Data Science" },
                new SelectListItem  { Value = "7", Text= "Psychology" },
                new SelectListItem  { Value = "8", Text= "Marketing" },
                new SelectListItem  { Value = "9", Text= "Graphic Design" },
                new SelectListItem  { Value = "10",Text= "Artificial Intelligence" }
                };
            return branchList;
        }
    
}
}
