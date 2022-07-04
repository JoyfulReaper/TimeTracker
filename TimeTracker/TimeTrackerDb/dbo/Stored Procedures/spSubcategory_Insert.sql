CREATE PROCEDURE [dbo].[spSubcategory_Insert]
	@UserId NVARCHAR(128),
	@Name NVARCHAR(75),
	@CategoryId INT,
	@DateCreated DATETIME2(7) = NULL
AS
BEGIN
	SET NOCOUNT ON;
	
	INSERT INTO [dbo].[Subcategory]
		([UserId],
		[Name],
		[CategoryId],
		[DateCreated])
	VALUES
		(@UserId, 
		@Name,
		@CategoryId, 
		@DateCreated);
END;