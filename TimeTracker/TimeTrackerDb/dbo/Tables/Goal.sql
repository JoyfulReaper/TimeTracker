CREATE TABLE [dbo].[Goal]
(
	[GoalId] INT NOT NULL PRIMARY KEY IDENTITY, 
	[ProjectId] INT NOT NULL,
    [Name] NVARCHAR(75) NOT NULL, 
    CONSTRAINT [FK_Goal_Project] FOREIGN KEY ([ProjectId]) REFERENCES [Project]([ProjectId])
)
