CREATE PROCEDURE [dbo].[spUser_GetById]
	@UserId NVARCHAR(128)
AS
BEGIN
SET NOCOUNT ON;
	SELECT 
		[UserId], 
		[FirstName], 
		[LastName],
		[EmailAddress],
		[DateCreated] 
	FROM 
		[dbo].[User] 
	WHERE UserId = @UserId
END