-- SQL Script to Add TeamMeeting Table
-- Run this in SQL Server Management Studio or Azure Data Studio
-- Make sure you're connected to your TeamPro1 database

-- Create TeamMeetings Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'TeamMeetings')
BEGIN
    CREATE TABLE [dbo].[TeamMeetings] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [TeamId] INT NOT NULL,
        [MeetingNumber] INT NOT NULL,
        [MeetingDate] DATETIME2 NOT NULL DEFAULT GETDATE(),
        [CompletionPercentage] INT NOT NULL DEFAULT 0,
        [ProofUploads] NVARCHAR(500) NULL,
        [FacultyReview] NVARCHAR(MAX) NULL,
        [Status] NVARCHAR(50) NOT NULL DEFAULT 'Scheduled',
        [Notes] NVARCHAR(MAX) NULL,
        [CreatedAt] DATETIME2 NOT NULL DEFAULT GETDATE(),
        [LastUpdated] DATETIME2 NULL,
        
        -- Foreign Key
        CONSTRAINT [FK_TeamMeetings_Teams_TeamId] 
            FOREIGN KEY ([TeamId]) REFERENCES [Teams]([Id]) ON DELETE CASCADE,
        
        -- Constraints
        CONSTRAINT [CHK_TeamMeetings_MeetingNumber] CHECK ([MeetingNumber] >= 1 AND [MeetingNumber] <= 100),
        CONSTRAINT [CHK_TeamMeetings_CompletionPercentage] CHECK ([CompletionPercentage] >= 0 AND [CompletionPercentage] <= 100),
        
        -- Unique constraint: each team can only have one meeting with the same meeting number
        CONSTRAINT [UQ_TeamMeetings_TeamId_MeetingNumber] UNIQUE ([TeamId], [MeetingNumber])
    );
    
    -- Create Indexes
    CREATE INDEX [IX_TeamMeetings_TeamId] ON [TeamMeetings]([TeamId]);
    CREATE INDEX [IX_TeamMeetings_MeetingNumber] ON [TeamMeetings]([MeetingNumber]);
    
    PRINT 'TeamMeetings table created successfully with ProofUploads column';
END
ELSE
BEGIN
    PRINT 'TeamMeetings table already exists';
    
    -- Check if ProofUploads column exists, if not add it
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('TeamMeetings') AND name = 'ProofUploads')
    BEGIN
        ALTER TABLE [TeamMeetings] ADD [ProofUploads] NVARCHAR(500) NULL;
        PRINT 'ProofUploads column added to existing TeamMeetings table';
    END
END
GO

-- Verify table was created
SELECT 'TeamMeetings' AS TableName, COUNT(*) AS RecordCount FROM TeamMeetings;
GO

PRINT 'TeamMeetings table ready! You can now track multiple meetings with proof uploads for each team.';

