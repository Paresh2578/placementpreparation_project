
-- Get All Mcq
alter PROCEDURE PR_Mcq_GetAll 
    @CourseId UNIQUEIDENTIFIER = NULL,
    @TopicId UNIQUEIDENTIFIER = NULL,
    @SubTopicId UNIQUEIDENTIFIER = NULL
AS
BEGIN
    SELECT *
    FROM [dbo].[Mcqs]
    WHERE 
        (CourseId = @CourseId OR @CourseId IS NULL)
        AND (TopicId = @TopicId OR @TopicId IS NULL)
        AND (SubTopicId = @SubTopicId OR @SubTopicId IS NULL);
END;

