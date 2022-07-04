CREATE TABLE [dbo].[ToDoList]
(
	[ToDoListId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ProjectId] INT NULL, 
    [Name] NVARCHAR(100) NOT NULL, 
    CONSTRAINT [FK_ToDoList_Project] FOREIGN KEY ([ProjectId]) REFERENCES [Project]([ProjectId])
)
