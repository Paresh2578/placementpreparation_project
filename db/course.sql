-- Get Topic List by Course Id
alter procedure PR_Topic_GetById
   @courseId UNIQUEIDENTIFIER
as
begin
    select t.TopicId, t.TopicName , d.DifficultyLevelName from Topics as t
	inner join DifficultyLevels as d
	on t.DifficultyLevelId = d.DifficultyLevelId
	where t.CourseId = @courseId
	order by t.Level
end

-- Get All Course that MCQ are avalible
alter procedure PR_Course_GetAllCourse_Mcq_Avalible
as
begin
    select c.CourseId , c.CourseName from Courses  as c
	where (select count(*) from dbo.Questions as q where q.CourseId = c.CourseId and q.IsActive = 'true') > 0
end

-- Get All Course that Question are avalible
alter procedure PR_Course_GetAllCourse_Question_Avalible
as
begin
    select c.CourseId , c.CourseName from Courses  as c
	where (select count(*) from dbo.Mcqs as m where m.CourseId = c.CourseId and m.IsActive = 'true') > 0
end