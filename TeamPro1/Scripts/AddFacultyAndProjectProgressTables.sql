-- SQL Script to Add Faculty and ProjectProgress Tables
-- Run this in SQL Server Management Studio or Azure Data Studio
-- Make sure you're connected to your TeamPro1 database

-- Create Faculties Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Faculties')
BEGIN
    CREATE TABLE [dbo].[Faculties] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [FullName] NVARCHAR(100) NOT NULL,
        [Email] NVARCHAR(100) NOT NULL,
        [Password] NVARCHAR(100) NOT NULL,
        [Department] NVARCHAR(100) NOT NULL,
        [CreatedAt] DATETIME2 NOT NULL DEFAULT GETDATE()
    );
    PRINT 'Faculties table created successfully';
END
ELSE
BEGIN
    PRINT 'Faculties table already exists';
END
GO

-- Create ProjectProgresses Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ProjectProgresses')
BEGIN
    CREATE TABLE [dbo].[ProjectProgresses] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [TeamId] INT NOT NULL,
        [AssignedFacultyId] INT NULL,
        [ProblemStatement] NVARCHAR(500) NULL,
        [CompletionPercentage] INT NOT NULL DEFAULT 0,
        [ProofUploads] NVARCHAR(MAX) NULL,
        [FacultyReview] NVARCHAR(MAX) NULL,
        [Status] NVARCHAR(50) NOT NULL DEFAULT 'Pending',
        [CreatedAt] DATETIME2 NOT NULL DEFAULT GETDATE(),
        [LastUpdated] DATETIME2 NULL,
        
        -- Foreign Keys
        CONSTRAINT [FK_ProjectProgresses_Teams_TeamId] 
            FOREIGN KEY ([TeamId]) REFERENCES [Teams]([Id]) ON DELETE CASCADE,
        
        CONSTRAINT [FK_ProjectProgresses_Faculties_AssignedFacultyId] 
            FOREIGN KEY ([AssignedFacultyId]) REFERENCES [Faculties]([Id]) ON DELETE SET NULL
    );
    
    -- Create Indexes
    CREATE INDEX [IX_ProjectProgresses_TeamId] ON [ProjectProgresses]([TeamId]);
    CREATE INDEX [IX_ProjectProgresses_AssignedFacultyId] ON [ProjectProgresses]([AssignedFacultyId]);
    
    PRINT 'ProjectProgresses table created successfully';
END
ELSE
BEGIN
    PRINT 'ProjectProgresses table already exists';
END
GO

-- Verify tables were created
SELECT 'Faculties' AS TableName, COUNT(*) AS RecordCount FROM Faculties
UNION ALL
SELECT 'ProjectProgresses' AS TableName, COUNT(*) AS RecordCount FROM ProjectProgresses;
GO

PRINT 'All tables created successfully! You can now use the Status Update feature.';
