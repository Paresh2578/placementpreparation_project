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