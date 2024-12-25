
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




-- Count ALL Mcq
alter procedure PR_Mcq_COUNT null ,null , null , 1
	@CourseId UNIQUEIDENTIFIER = NULL,
    @TopicId UNIQUEIDENTIFIER = NULL,
    @SubTopicId UNIQUEIDENTIFIER = NULL,
	@onlyActiveGets BIT = 0
AS
BEGIN 
   SELECT  count(Case when @onlyActiveGets = 0  then 1 else (case when IsActive = 1 then 1 else null end)  end) as totelMcq
    FROM [dbo].Mcqs
    WHERE 
        (CourseId = @CourseId OR @CourseId IS NULL)
        AND (TopicId = @TopicId OR @TopicId IS NULL)
        AND (SubTopicId = @SubTopicId OR @SubTopicId IS NULL
		)
END