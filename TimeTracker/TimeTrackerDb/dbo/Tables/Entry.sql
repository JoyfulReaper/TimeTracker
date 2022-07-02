CREATE TABLE [dbo].[Entry]
(
	[EntryId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ProjectId] INT NOT NULL,
    [EntryDate] DATETIME2 NOT NULL DEFAULT GETUTCDATE(), 
    [HoursSpent] DECIMAL(8, 2) NULL, 
    CONSTRAINT [FK_Entry_Project] FOREIGN KEY ([ProjectId]) REFERENCES [Project]([ProjectId])
)
