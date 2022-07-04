CREATE PROCEDURE [dbo].[spCategory_Delete]
	@CategoryId INT
AS
BEGIN
	IF (SELECT COUNT(*) FROM Project WHERE CategoryId = @CategoryId) > 0
		THROW 50001, 'The category cannot be deleted because it contains projects', 1 

	DELETE FROM [dbo].[Category]
	WHERE [CategoryId] = @CategoryId
END