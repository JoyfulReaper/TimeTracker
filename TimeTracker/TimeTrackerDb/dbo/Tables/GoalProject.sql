CREATE TABLE [dbo].[GoalProject]
(
	[GoalProjectId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [GoalId] INT NOT NULL, 
    [ProjectId] INT NOT NULL, 
    CONSTRAINT [FK_GoalProject_Goal] FOREIGN KEY ([GoalId]) REFERENCES [Goal]([GoalId]), 
    CONSTRAINT [FK_GoalProject_Project] FOREIGN KEY ([ProjectId]) REFERENCES [Project]([ProjectId])
)
