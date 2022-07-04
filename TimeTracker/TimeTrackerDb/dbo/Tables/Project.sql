CREATE TABLE [dbo].[Project]
(
	[ProjectId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [UserId] NVARCHAR(128) NOT NULL,
    [Name] NVARCHAR(75) NOT NULL, 
    [CategoryId] INT NOT NULL, 
    [SubcategoryId] INT NULL, 
    [DateCreated] DATETIME2 NOT NULL DEFAULT GETUTCDATE(), 
    CONSTRAINT [FK_Project_Category] FOREIGN KEY ([CategoryId]) REFERENCES [Category]([CategoryId]), 
    CONSTRAINT [FK_Project_Subcategory] FOREIGN KEY ([SubcategoryId]) REFERENCES [Subcategory]([SubcategoryId]),
    CONSTRAINT [FK_Project_User] FOREIGN KEY ([UserId]) REFERENCES [User]([UserId])
)
