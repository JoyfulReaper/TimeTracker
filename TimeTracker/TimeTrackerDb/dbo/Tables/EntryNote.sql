CREATE TABLE [dbo].[EntryNote]
(
	[EntryNoteId] INT NOT NULL PRIMARY KEY IDENTITY,
    [EntryId] INT NOT NULL, 
    [NoteId] INT NOT NULL, 
    CONSTRAINT [FK_EntryNote_Entry] FOREIGN KEY ([EntryId]) REFERENCES [Entry]([EntryId]), 
    CONSTRAINT [FK_EntryNote_Note] FOREIGN KEY ([NoteId]) REFERENCES [Note]([NoteId])
)
