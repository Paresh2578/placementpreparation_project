-- Count ALL Qustion
ALTER PROCEDURE PR_Question_COUNT null ,null , null , 0 , 1
    @CourseId UNIQUEIDENTIFIER = NULL,
    @TopicId UNIQUEIDENTIFIER = NULL,
    @SubTopicId UNIQUEIDENTIFIER = NULL,
    @onlyActiveGets BIT = 0,
    @onlyInterviewQuestions BIT = 0
AS
BEGIN
    SELECT 
        COUNT(*) AS TotalQuestions
    FROM 
        [dbo].Questions
    WHERE 
        (@CourseId IS NULL OR CourseId = @CourseId)
        AND (@TopicId IS NULL OR TopicId = @TopicId)
        AND (@SubTopicId IS NULL OR SubTopicId = @SubTopicId)
        AND (@onlyActiveGets = 0 OR IsActive = 1)
        AND (@onlyInterviewQuestions = 0 OR AddedBy IS NOT NULL);
END
