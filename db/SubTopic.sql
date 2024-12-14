-- Add Sub Topic
create PROCEDURE PR_SubTopic_AddSubTopic
  @SubTopicId UNIQUEIDENTIFIER,
  @SubTopicName VARCHAR(MAX),
  @Content VARCHAR(MAX) = null, 
  @CourseId UNIQUEIDENTIFIER, 
  @TopicId UNIQUEIDENTIFIER,
  @DifficultyLevelId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.SubTopics(SubTopicId, SubTopicName,Level, Content, CourseId,TopicId, DifficultyLevelId)
    VALUES (@SubTopicId, @SubTopicName, (select count(*) from dbo.SubTopics where TopicId = @TopicId)+1, @Content, @CourseId,@TopicId, @DifficultyLevelId);

	return @@ROWCOUNT;
END;