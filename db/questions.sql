-- Count ALL Qustion
alter procedure PR_Question_COUNT null , null , null , 1
	@CourseId UNIQUEIDENTIFIER = NULL,
    @TopicId UNIQUEIDENTIFIER = NULL,
    @SubTopicId UNIQUEIDENTIFIER = NULL,
	@onlyActiveGets BIT = 0
AS
BEGIN 
   SELECT  count(Case when @onlyActiveGets = 0  then 1 else (case when IsActive = 1 then 1 else null end)  end) as totelQuestion
    FROM [dbo].Questions
    WHERE 
        (CourseId = @CourseId OR @CourseId IS NULL)
        AND (TopicId = @TopicId OR @TopicId IS NULL)
        AND (SubTopicId = @SubTopicId OR @SubTopicId IS NULL
		)
END