CREATE TABLE [dbo].[ToDoListItem]
(
	[ToDoListItemId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ToDoListId] INT NOT NULL, 
    [Name] NVARCHAR(200) NOT NULL, 
    [Completed] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [FK_ToDoListItem_ToDoList] FOREIGN KEY ([ToDoListId]) REFERENCES [ToDoList]([ToDoListId])
)
