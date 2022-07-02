CREATE TABLE [dbo].[GoalNote]
(
	[GoalNoteId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [GoalId] INT NOT NULL, 
    [NoteId] INT NOT NULL, 
    CONSTRAINT [FK_GoalNote_Goal] FOREIGN KEY ([GoalId]) REFERENCES [Goal]([GoalId]), 
    CONSTRAINT [FK_GoalNote_Note] FOREIGN KEY ([NoteId]) REFERENCES [Note]([NoteId])
)
