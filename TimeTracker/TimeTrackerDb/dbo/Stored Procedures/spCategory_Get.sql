CREATE PROCEDURE [dbo].[spCategory_Get]
	@UserId NVARCHAR(128)
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT 
		[CategoryId],
		[UserId], 
		[Name],
		[DateCreated] 
	FROM 
		[dbo].[Category] 
	WHERE 
		UserId = @UserId;
END