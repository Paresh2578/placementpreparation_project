

-- get Dashbord data
ALTER PROCEDURE PR_GET_Dashbord_Data 
  @AdminId uniqueidentifier = null,
  @IsAdmin bit
AS
BEGIN
   With courses as (SELECT count(*) as TotalCourses FROM dbo.Courses),
   coursesQuestions as (SELECT count(*) as TotalCoursesQuestions FROM dbo.Questions ),
   coursesMcqs as (SELECT count(*) as TotalCoursesMcqs FROM dbo.Mcqs ),
   interviewQuestions as (SELECT count(*) as TotalInterviewQuestions FROM dbo.Questions where ((AddedBy is not null) and  (@IsAdmin = 1  OR AddedBy = @AdminId))),
  interviewMcqs as (SELECT count(*) as InterviewMcqs FROM dbo.Mcqs  where  ((AddedBy is not null) and  (@IsAdmin = 1  OR AddedBy = @AdminId)))
   select * from courses , coursesQuestions , coursesMcqs,interviewQuestions,interviewMcqs
END


exec PR_GET_Dashbord_Data '62EF593A-59D3-4A54-F3A7-08DD3307AB2B' ,0