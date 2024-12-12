-- Get Topic List by Course Id
alter procedure PR_Topic_GetById 'aec3b290-2ce8-4eba-b6e3-51d53fb56d8d'
   @courseId UNIQUEIDENTIFIER
as
begin
    select t.TopicId, t.TopicName , d.DifficultyLevelName from Topics as t
	inner join DifficultyLevels as d
	on t.DifficultyLevelId = d.DifficultyLevelId
	where t.CourseId = @courseId
end