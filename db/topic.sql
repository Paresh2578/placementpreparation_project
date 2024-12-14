-- Add Topic
ALTER PROCEDURE PR_Topic_AddTopic
  @TopicId UNIQUEIDENTIFIER,
  @TopicName VARCHAR(MAX),
  @Content VARCHAR(MAX) = null, 
  @CourseId UNIQUEIDENTIFIER, 
  @DifficultyLevelId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.Topics (TopicId, TopicName,Level, Content, CourseId, DifficultyLevelId)
    VALUES (@TopicId, @TopicName , (select count(*) from dbo.Topics where CourseId = @CourseId)+1, @Content, @CourseId, @DifficultyLevelId);

	return @@ROWCOUNT;
END;


DECLARE @NewTopicId UNIQUEIDENTIFIER = NEWID();
EXEC PR_Topic_AddTopic 
    @TopicId = @NewTopicId, 
    @TopicName = 'function', 
    @CourseId = '42b964fe-cd33-42bc-af5b-80610959b1cc', 
    @DifficultyLevelId = '2269e060-9a3b-4ffe-a5af-36e383026521';


------Get All Topic and SubTopic List By CourseId
Alter procedure PR_Topic_SubTopic_List_ByCourseId 
  @courseId UNIQUEIDENTIFIER
as
begin
   select t.TopicId , t.TopicName , t.Level , st.SubTopicId , st.SubTopicName , st.Level as SubTopicLevel from dbo.Topics as t
   full join dbo.SubTopics as st
   on t.TopicId = st.TopicId
   where t.CourseId = @courseId
   order by t.Level , st.Level
end

exec PR_Topic_SubTopic_List_ByCourseId 'aec3b290-2ce8-4eba-b6e3-51d53fb56d8d'

select * from dbo.Topics
where CourseId = 'AEC3B290-2CE8-4EBA-B6E3-51D53FB56D8D'

select top 1 * from dbo.SubTopics
where TopicId = '9ec7737b-0af6-4d07-a959-147b58e38ac7'
order by Level

