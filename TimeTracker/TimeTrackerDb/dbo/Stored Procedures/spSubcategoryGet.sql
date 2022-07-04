CREATE PROCEDURE [dbo].[spSubcategoryGet]
	@CategoryId int
AS
BEGIN
	
	SELECT 
		[SubcategoryId], 
		[UserId],
		[Name],
		[CategoryId], 
		[DateCreated] 
	FROM 
		Subcategory 
	WHERE
		CategoryId = @CategoryId
	
END