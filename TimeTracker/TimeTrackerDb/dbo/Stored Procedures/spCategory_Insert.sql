CREATE PROCEDURE [dbo].[spCategory_Insert]
	@UserId NVARCHAR(128),
	@Name NVARCHAR(75)
AS
BEGIN

	INSERT INTO [dbo].[Category]
		([UserId], 
		[Name])
	VALUES
		(@UserId, 
		@Name);
END