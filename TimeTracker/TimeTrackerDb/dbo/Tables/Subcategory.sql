CREATE TABLE [dbo].[Subcategory]
(
	[SubcategoryId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [UserId] NVARCHAR(128) NOT NULL,
    [Name] VARCHAR(75) NOT NULL, 
    [CategoryId] INT NOT NULL, 
    [DateCreated] DATETIME2 NOT NULL DEFAULT GETUTCDATE(), 
    CONSTRAINT [FK_Subcategory_Category] FOREIGN KEY ([CategoryId]) REFERENCES [Category]([CategoryId])
)
