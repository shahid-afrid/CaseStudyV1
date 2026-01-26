# Fix: TeamMeetings Table Missing Error

## ?? Error Message
```
? An unexpected error occurred: Invalid object name 'TeamMeetings'. Please try again or contact support.
```

## ? Solution: Create TeamMeetings Table

### Option 1: Using SQL Server Management Studio (RECOMMENDED)

1. **Open SQL Server Management Studio (SSMS)** or **Azure Data Studio**

2. **Connect to your database server**
   - Server name: `(localdb)\\mssqllocaldb` or your SQL Server instance
   - Authentication: Windows Authentication

3. **Select your database**
   - Find `TeamPro1` database in Object Explorer
   - Right-click and select "New Query"

4. **Run the SQL Script**
   - Open file: `TeamPro1\Scripts\AddTeamMeetingTable.sql`
   - Copy all the SQL code
   - Paste into SSMS query window
   - Click **Execute** (or press F5)

5. **Verify Success**
   - You should see messages:
     ```
     TeamMeetings table created successfully with ProofUploads column
     TeamMeetings table ready! You can now track multiple meetings...
     ```

6. **Restart your application**
   - Stop the running application (Shift+F5)
   - Rebuild solution (Ctrl+Shift+B)
   - Start again (F5)

---

### Option 2: Using Package Manager Console (Entity Framework)

1. **Open Package Manager Console**
   - Tools ? NuGet Package Manager ? Package Manager Console

2. **Run Migration Command**
   ```powershell
   Add-Migration AddTeamMeetingsTable
   Update-Database
   ```

---

### Option 3: Quick SQL Command (Copy & Paste)

If you want to run it directly, copy this SQL and execute in SSMS:

```sql
-- Create TeamMeetings Table
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
    
    CONSTRAINT [FK_TeamMeetings_Teams_TeamId] 
        FOREIGN KEY ([TeamId]) REFERENCES [Teams]([Id]) ON DELETE CASCADE,
    
    CONSTRAINT [CHK_TeamMeetings_MeetingNumber] 
        CHECK ([MeetingNumber] >= 1 AND [MeetingNumber] <= 100),
    
    CONSTRAINT [CHK_TeamMeetings_CompletionPercentage] 
        CHECK ([CompletionPercentage] >= 0 AND [CompletionPercentage] <= 100),
    
    CONSTRAINT [UQ_TeamMeetings_TeamId_MeetingNumber] 
        UNIQUE ([TeamId], [MeetingNumber])
);

-- Create Indexes
CREATE INDEX [IX_TeamMeetings_TeamId] ON [TeamMeetings]([TeamId]);
CREATE INDEX [IX_TeamMeetings_MeetingNumber] ON [TeamMeetings]([MeetingNumber]);

SELECT 'TeamMeetings table created!' AS Message;
```

---

## ?? Table Structure

The TeamMeetings table stores meeting records for each team:

| Column | Type | Description |
|--------|------|-------------|
| Id | INT | Primary key (auto-increment) |
| TeamId | INT | Foreign key to Teams table |
| MeetingNumber | INT | Meeting number (1-100) |
| MeetingDate | DATETIME2 | Date of the meeting |
| CompletionPercentage | INT | Project completion % (0-100) |
| ProofUploads | NVARCHAR(500) | File path for uploaded proof |
| FacultyReview | NVARCHAR(MAX) | Faculty feedback/review |
| Status | NVARCHAR(50) | Meeting status (Scheduled/Completed) |
| Notes | NVARCHAR(MAX) | Meeting notes/agenda |
| CreatedAt | DATETIME2 | Record creation timestamp |
| LastUpdated | DATETIME2 | Last update timestamp |

---

## ?? Verify Table Creation

After running the script, verify the table exists:

```sql
-- Check if table exists
SELECT * FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_NAME = 'TeamMeetings';

-- View table structure
SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE 
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'TeamMeetings';

-- Check for any existing records
SELECT COUNT(*) AS TotalMeetings FROM TeamMeetings;
```

---

## ?? Troubleshooting

### Error: "Foreign key constraint failed"
**Solution:** Make sure the Teams table exists first. Run this to check:
```sql
SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Teams';
```

### Error: "Database 'TeamPro1' not found"
**Solution:** 
1. Check your connection string in `appsettings.json`
2. Create the database if it doesn't exist:
   ```sql
   CREATE DATABASE TeamPro1;
   ```

### Error: "Permission denied"
**Solution:** Make sure you're logged in as a user with CREATE TABLE permissions.

---

## ?? Connection String

Your connection string should be in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=TeamPro1;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

---

## ? After Fixing

Once the table is created, you'll be able to:
- ? Add meetings for teams
- ? Upload proof files
- ? View meeting history
- ? Edit first meeting after mentor/problem assignment
- ? Track project completion progress

---

## ?? Still Having Issues?

If you're still getting errors after running the script:

1. **Check the Output window** for detailed error messages
2. **Verify database connection** in appsettings.json
3. **Restart Visual Studio** and SQL Server
4. **Check SQL Server logs** for permission or connection issues

Need more help? Check the error message and:
- Make sure SQL Server is running
- Verify you're connected to the correct database
- Ensure you have CREATE TABLE permissions
