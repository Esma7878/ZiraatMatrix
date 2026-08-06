using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ZiraatProje.DataAccess
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            context.Database.EnsureCreated();

            // Perform alter migrations if tables existed before schema updates
            try
            {
                context.Database.ExecuteSqlRaw(@"
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'MonthlyReleaseShifts') AND name = 'JiraTicketNo')
                BEGIN
                    ALTER TABLE MonthlyReleaseShifts ADD JiraTicketNo NVARCHAR(100) NULL;
                END

                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'MonthlyReleaseShifts') AND name = 'ExternalLink')
                BEGIN
                    ALTER TABLE MonthlyReleaseShifts ADD ExternalLink NVARCHAR(MAX) NULL;
                END

                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'MonthlyReleaseShifts') AND name = 'Note')
                BEGIN
                    ALTER TABLE MonthlyReleaseShifts ADD Note NVARCHAR(MAX) NULL;
                END

                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'Shifts') AND name = 'JiraTicketNo')
                BEGIN
                    ALTER TABLE Shifts ADD JiraTicketNo NVARCHAR(100) NULL;
                END

                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'Shifts') AND name = 'ExternalLink')
                BEGIN
                    ALTER TABLE Shifts ADD ExternalLink NVARCHAR(MAX) NULL;
                END

                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'Users') AND name = 'Phone')
                BEGIN
                    ALTER TABLE Users ADD Phone NVARCHAR(50) NULL;
                END

                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'MonthlyReleaseShifts') AND name = 'CreatedByUserName')
                BEGIN
                    ALTER TABLE MonthlyReleaseShifts ADD CreatedByUserName NVARCHAR(200) NULL;
                END

                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'Projects') AND name = 'ActualManDays')
                BEGIN
                    ALTER TABLE Projects ADD ActualManDays DECIMAL(18,2) NOT NULL DEFAULT 0;
                END

                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'Projects') AND name = 'CreatedByUserName')
                BEGIN
                    ALTER TABLE Projects ADD CreatedByUserName NVARCHAR(200) NULL;
                END
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'Projects') AND name = 'CreatedAt')
                BEGIN
                    ALTER TABLE Projects ADD CreatedAt DATETIME2 NULL;
                END
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'Projects') AND name = 'UpdatedByUserName')
                BEGIN
                    ALTER TABLE Projects ADD UpdatedByUserName NVARCHAR(200) NULL;
                END
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'Projects') AND name = 'UpdatedAt')
                BEGIN
                    ALTER TABLE Projects ADD UpdatedAt DATETIME2 NULL;
                END
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'Projects') AND name = 'CompletedByUserName')
                BEGIN
                    ALTER TABLE Projects ADD CompletedByUserName NVARCHAR(200) NULL;
                END
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'Projects') AND name = 'CompletedAt')
                BEGIN
                    ALTER TABLE Projects ADD CompletedAt DATETIME2 NULL;
                END

                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'ProjectAllocations') AND name = 'ActualManDay')
                BEGIN
                    ALTER TABLE ProjectAllocations ADD ActualManDay DECIMAL(18,2) NOT NULL DEFAULT 0;
                END

                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'CustomShifts')
                BEGIN
                    CREATE TABLE CustomShifts (
                        Id INT IDENTITY(1,1) PRIMARY KEY,
                        Topic NVARCHAR(200) NOT NULL,
                        Description NVARCHAR(MAX) NULL,
                        AssignedUsers NVARCHAR(MAX) NOT NULL,
                        ShiftDate DATETIME2 NOT NULL,
                        ExternalLink NVARCHAR(MAX) NULL,
                        CreatedByUserName NVARCHAR(200) NULL
                    );
                END
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'CustomShifts') AND name = 'CreatedByUserName')
                BEGIN
                    ALTER TABLE CustomShifts ADD CreatedByUserName NVARCHAR(200) NULL;
                END

                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'MonthlyReleaseShifts') AND name = 'UpdatedByUserName')
                BEGIN
                    ALTER TABLE MonthlyReleaseShifts ADD UpdatedByUserName NVARCHAR(200) NULL;
                END

                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'MonthlyReleaseShifts') AND name = 'UpdatedAt')
                BEGIN
                    ALTER TABLE MonthlyReleaseShifts ADD UpdatedAt DATETIME2 NULL;
                END

                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'CustomShifts') AND name = 'UpdatedByUserName')
                BEGIN
                    ALTER TABLE CustomShifts ADD UpdatedByUserName NVARCHAR(200) NULL;
                END

                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'CustomShifts') AND name = 'UpdatedAt')
                BEGIN
                    ALTER TABLE CustomShifts ADD UpdatedAt DATETIME2 NULL;
                END

                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'MonthlyReleaseShifts') AND name = 'CreatedAt')
                BEGIN
                    ALTER TABLE MonthlyReleaseShifts ADD CreatedAt DATETIME2 NULL;
                END

                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'CustomShifts') AND name = 'CreatedAt')
                BEGIN
                    ALTER TABLE CustomShifts ADD CreatedAt DATETIME2 NULL;
                END

                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'Leaves') AND name = 'Status')
                BEGIN
                    ALTER TABLE Leaves ADD Status NVARCHAR(50) NOT NULL DEFAULT 'Approved';
                END

                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'Leaves') AND name = 'IsSpecialRequest')
                BEGIN
                    ALTER TABLE Leaves ADD IsSpecialRequest BIT NOT NULL DEFAULT 0;
                END

                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'Leaves') AND name = 'RequestNote')
                BEGIN
                    ALTER TABLE Leaves ADD RequestNote NVARCHAR(MAX) NULL;
                END

                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'Leaves') AND name = 'RequestedAt')
                BEGIN
                    ALTER TABLE Leaves ADD RequestedAt DATETIME2 NULL;
                END

                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'Leaves') AND name = 'ApprovedAt')
                BEGIN
                    ALTER TABLE Leaves ADD ApprovedAt DATETIME2 NULL;
                END

                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'Leaves') AND name = 'ApprovedByUserName')
                BEGIN
                    ALTER TABLE Leaves ADD ApprovedByUserName NVARCHAR(200) NULL;
                END

                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ChatMessages')
                BEGIN
                    CREATE TABLE ChatMessages (
                        Id INT IDENTITY(1,1) PRIMARY KEY,
                        SenderUserId INT NOT NULL,
                        ReceiverUserId INT NULL,
                        TargetType NVARCHAR(50) NOT NULL,
                        TargetTeam NVARCHAR(50) NULL,
                        MessageText NVARCHAR(MAX) NOT NULL,
                        SentAt DATETIME2 NOT NULL,
                        IsRead BIT NOT NULL
                    );
                END

                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ChatMessageReadStates')
                BEGIN
                    CREATE TABLE ChatMessageReadStates (
                        Id INT IDENTITY(1,1) PRIMARY KEY,
                        UserId INT NOT NULL,
                        ChannelKey NVARCHAR(100) NOT NULL,
                        LastReadMessageId INT NOT NULL,
                        LastReadAt DATETIME2 NOT NULL
                    );
                END

                DELETE FROM Leaves 
                WHERE Id NOT IN (
                    SELECT MIN(Id) 
                    FROM Leaves 
                    GROUP BY UserId, CAST(StartDate AS DATE), CAST(EndDate AS DATE)
                );

                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'MonthlyReleaseShifts') AND name = 'IsFinished')
                BEGIN
                    ALTER TABLE MonthlyReleaseShifts ADD IsFinished BIT NOT NULL DEFAULT 0;
                END

                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'CustomShifts') AND name = 'IsFinished')
                BEGIN
                    ALTER TABLE CustomShifts ADD IsFinished BIT NOT NULL DEFAULT 0;
                END

                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'Projects') AND name = 'PergelNo')
                BEGIN
                    ALTER TABLE Projects ADD PergelNo BIGINT NOT NULL DEFAULT 0;
                END
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'Projects') AND name = 'Summary')
                BEGIN
                    ALTER TABLE Projects ADD Summary NVARCHAR(MAX) NULL;
                END
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'Projects') AND name = 'ProjectType')
                BEGIN
                    ALTER TABLE Projects ADD ProjectType NVARCHAR(100) NOT NULL DEFAULT 'Proje';
                END
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'Projects') AND name = 'ExternalCompanyName')
                BEGIN
                    ALTER TABLE Projects ADD ExternalCompanyName NVARCHAR(200) NULL;
                END
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'Projects') AND name = 'ExternalCost')
                BEGIN
                    ALTER TABLE Projects ADD ExternalCost DECIMAL(18,2) NOT NULL DEFAULT 0;
                END
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'Projects') AND name = 'Year')
                BEGIN
                    ALTER TABLE Projects ADD Year SMALLINT NOT NULL DEFAULT 2026;
                END
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'Projects') AND name = 'Quarter')
                BEGIN
                    ALTER TABLE Projects ADD Quarter TINYINT NOT NULL DEFAULT 3;
                END
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'Projects') AND name = 'Stakeholders')
                BEGIN
                    ALTER TABLE Projects ADD Stakeholders NVARCHAR(MAX) NULL;
                END
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'Projects') AND name = 'Description')
                BEGIN
                    ALTER TABLE Projects ADD Description NVARCHAR(MAX) NULL;
                END
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'Projects') AND name = 'AssignedUserNames')
                BEGIN
                    ALTER TABLE Projects ADD AssignedUserNames NVARCHAR(MAX) NULL;
                END
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'Projects') AND name = 'AssignedUserIds')
                BEGIN
                    ALTER TABLE Projects ADD AssignedUserIds NVARCHAR(MAX) NULL;
                END
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'Projects') AND name = 'AssignedAnalystNames')
                BEGIN
                    ALTER TABLE Projects ADD AssignedAnalystNames NVARCHAR(MAX) NULL;
                END
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'Projects') AND name = 'AssignedDeveloperNames')
                BEGIN
                    ALTER TABLE Projects ADD AssignedDeveloperNames NVARCHAR(MAX) NULL;
                END
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'Projects') AND name = 'Gmy')
                BEGIN
                    ALTER TABLE Projects ADD Gmy NVARCHAR(250) NULL;
                END
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'Projects') AND name = 'BusinessUnit')
                BEGIN
                    ALTER TABLE Projects ADD BusinessUnit NVARCHAR(250) NULL;
                END

                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ProjectMonthlyCosts')
                BEGIN
                    CREATE TABLE ProjectMonthlyCosts (
                        Id INT IDENTITY(1,1) PRIMARY KEY,
                        ProjectId INT NOT NULL,
                        UserId INT NOT NULL,
                        Month INT NOT NULL,
                        ManDays DECIMAL(18,2) NOT NULL
                    );
                END");

                context.Database.ExecuteSqlRaw("UPDATE Projects SET PergelNo = 100000 + Id WHERE PergelNo IS NULL OR PergelNo = 0");
                context.Database.ExecuteSqlRaw("UPDATE Projects SET Summary = ISNULL(Summary, ProjectName)");
                context.Database.ExecuteSqlRaw("UPDATE Projects SET ProjectType = ISNULL(ProjectType, 'Proje')");
                context.Database.ExecuteSqlRaw("UPDATE Projects SET ProjectStatus = ISNULL(ProjectStatus, 'Planlandı')");
                context.Database.ExecuteSqlRaw("UPDATE Projects SET ExternalCompanyName = ISNULL(ExternalCompanyName, '')");
                context.Database.ExecuteSqlRaw("UPDATE Projects SET ExternalCost = ISNULL(ExternalCost, 0)");
                context.Database.ExecuteSqlRaw("UPDATE Projects SET Year = CASE WHEN Year IS NULL OR Year = 0 THEN 2026 ELSE Year END");
                context.Database.ExecuteSqlRaw("UPDATE Projects SET Quarter = CASE WHEN Quarter IS NULL OR Quarter = 0 THEN 3 ELSE Quarter END");
                context.Database.ExecuteSqlRaw("UPDATE Projects SET Stakeholders = ISNULL(Stakeholders, '')");
                context.Database.ExecuteSqlRaw("UPDATE Projects SET Description = ISNULL(Description, '')");
                context.Database.ExecuteSqlRaw("UPDATE Projects SET AssignedUserNames = ISNULL(AssignedUserNames, '')");
                context.Database.ExecuteSqlRaw("UPDATE Projects SET AssignedAnalystNames = ISNULL(AssignedAnalystNames, '')");
                context.Database.ExecuteSqlRaw("UPDATE Projects SET AssignedDeveloperNames = ISNULL(AssignedDeveloperNames, '')");
                context.Database.ExecuteSqlRaw("UPDATE Projects SET AssignedUserIds = ISNULL(AssignedUserIds, '')");

                context.Database.ExecuteSqlRaw(@"
                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'Teams') AND name = 'Color')
                    BEGIN
                        ALTER TABLE Teams ADD Color NVARCHAR(50) NULL;
                    END
                ");

                context.Database.ExecuteSqlRaw(@"
                    UPDATE Teams SET Color = '#7c3aed' WHERE (Color IS NULL OR Color = '') AND LOWER(TeamName) = 'takip';
                    UPDATE Teams SET Color = '#059669' WHERE (Color IS NULL OR Color = '') AND LOWER(TeamName) = 'tahsis';
                    UPDATE Teams SET Color = '#2563eb' WHERE (Color IS NULL OR Color = '') AND LOWER(TeamName) = 'teminat';
                    UPDATE Teams SET Color = '#7c3aed' WHERE Color IS NULL OR Color = '';
                    UPDATE Users SET Title = 'Developer', IsAdmin = 0 WHERE (Name LIKE 'Esma%' OR Email LIKE 'esma%' OR Email LIKE 'eozturk%') AND IsAdmin = 0;
                    UPDATE Users SET IsAdmin = 1 WHERE Title LIKE '%Yönetici%' OR Team = 'Departman Yönetimi' OR Email LIKE 'nesli%' OR Email LIKE 'ahmet.yilmaz%' OR Email LIKE 'ali.celik%';
                ");
            }
            catch { }




            if (!context.ShiftTypes.Any(st => st.ShiftName.Contains("Yaygınlaştırma")))
            {
                var existingNames = context.ShiftTypes.Select(st => st.ShiftName).ToList();
                var defaultTypes = new[]
                {
                    "Firewall Geçiş Nöbeti",
                    "Server Geçiş Nöbeti",
                    "Acil Güvenlik Yaması Nöbeti",
                    "Haftalık Yaygınlaştırma Nöbeti",
                    "Aylık Yaygınlaştırma Nöbeti"
                };

                foreach (var dt in defaultTypes)
                {
                    if (!existingNames.Contains(dt))
                    {
                        context.ShiftTypes.Add(new ShiftType { ShiftName = dt });
                    }
                }
                context.SaveChanges();
            }

            var mgrUsers = context.Users.Where(u => u.Title == "Yönetici").ToList();
            if (mgrUsers.Any())
            {
                foreach (var mgr in mgrUsers)
                {
                    mgr.Title = mgr.Surname.Equals("Yılmaz", StringComparison.OrdinalIgnoreCase) ? "Analist" : "Developer";
                    mgr.IsAdmin = true;
                }
                context.SaveChanges();
            }



            if (context.Users.Any())
            {
                return;
            }

            // 1. Seed Teams
            var teams = new Team[]
            {
                new Team { TeamName = "Takip", Color = "#7c3aed" },
                new Team { TeamName = "Tahsis", Color = "#059669" },
                new Team { TeamName = "Teminat", Color = "#2563eb" },
            };
            if (!context.Teams.Any()) { context.Teams.AddRange(teams); context.SaveChanges(); }

            // 2. Seed Users
            var users = new User[]
            {
                new User { Name = "Ahmet", Surname = "Yılmaz", Title = "Analist", Team = "Takip", Email = "ahmet.yilmaz@ziraatteknoloji.com", Phone = "+90 (212) 555 0101", Password = "1234", IsAdmin = true },
                new User { Name = "Ali", Surname = "Çelik", Title = "Developer", Team = "Tahsis", Email = "ali.celik@ziraatteknoloji.com", Phone = "+90 (212) 555 01 02", Password = "1234", IsAdmin = true },
                new User { Name = "Esma", Surname = "Yılmaz", Title = "Developer", Team = "Tahsis", Email = "esma.yilmaz@ziraatteknoloji.com", Phone = "+90 (212) 555 01 03", Password = "1234", IsAdmin = false },
                new User { Name = "Mehmet", Surname = "Kaya", Title = "Analist", Team = "Tahsis", Email = "mehmet.kaya@ziraatteknoloji.com", Phone = "+90 (212) 555 0104", Password = "1234", IsAdmin = false },
                new User { Name = "Ayşe", Surname = "Demir", Title = "Analist", Team = "Teminat", Email = "ayse.demir@ziraatteknoloji.com", Phone = "+90 (212) 555 0105", Password = "1234", IsAdmin = false },
                new User { Name = "Fatma", Surname = "Şahin", Title = "Developer", Team = "Takip", Email = "fatma.sahin@ziraatteknoloji.com", Phone = "+90 (212) 555 0106", Password = "1234", IsAdmin = false },
                new User { Name = "Zeynep", Surname = "Yıldız", Title = "Developer", Team = "Teminat", Email = "zeynep.yildiz@ziraatteknoloji.com", Phone = "+90 (212) 555 0107", Password = "1234", IsAdmin = false },
                new User { Name = "Mustafa", Surname = "Öztürk", Title = "Developer", Team = "Takip", Email = "mustafa.ozturk@ziraatteknoloji.com", Phone = "+90 (212) 555 0108", Password = "1234", IsAdmin = false },
                new User { Name = "Elif", Surname = "Aydın", Title = "Analist", Team = "Tahsis", Email = "elif.aydin@ziraatteknoloji.com", Phone = "+90 (212) 555 0109", Password = "1234", IsAdmin = true },
                new User { Name = "Ömer", Surname = "Arslan", Title = "Analist", Team = "Teminat", Email = "omer.arslan@ziraatteknoloji.com", Phone = "+90 (212) 555 0110", Password = "1234", IsAdmin = false },
                new User { Name = "Selin", Surname = "Bulut", Title = "Developer", Team = "Takip", Email = "selin.bulut@ziraatteknoloji.com", Phone = "+90 (212) 555 0111", Password = "1234", IsAdmin = false },
                new User { Name = "Can", Surname = "Koç", Title = "Developer", Team = "Tahsis", Email = "can.koc@ziraatteknoloji.com", Phone = "+90 (212) 555 0112", Password = "1234", IsAdmin = false },
                new User { Name = "Esma İrem ", Surname = "Öztürk", Title = "Developer", Team = "Takip", Email = "eozturk@ziraatteknoloji.com", Phone = "+90 (535) 262 66 22", Password = "esmaa", IsAdmin = false },
                new User { Name = "Gülşah", Surname = "yavuz", Title = "Developer", Team = "Teminat", Email = "gulsah@ziraatteknoloji.com", Phone = "+90 (565) 666 22 55", Password = "1234", IsAdmin = false },
                new User { Name = "Neslihan", Surname = "Keske", Title = "Departman Yöneticisi", Team = "Departman Yönetimi", Email = "nesli@ziraatteknoloji.com", Phone = "+90 (566) 956 66", Password = "1234", IsAdmin = true },
                new User { Name = " Barbaros", Surname = "Hayrettin", Title = "Analist", Team = "Tahsis", Email = "barbo@ziraatteknoloji.com", Phone = "+90 (544) 547 88 99", Password = "1234", IsAdmin = false },
                new User { Name = "Ali", Surname = "Koç", Title = "Developer", Team = "Takip", Email = "ali@ziraatteknoloji.com", Phone = "+90 (233) 336 66 45", Password = "1234", IsAdmin = false },
            };
            if (!context.Users.Any()) { context.Users.AddRange(users); context.SaveChanges(); }

            // 3. Seed ShiftTypes
            var shiftTypes = new ShiftType[]
            {
                new ShiftType { ShiftName = "Firewall Geçiş" },
                new ShiftType { ShiftName = "Server Geçiş" },
                new ShiftType { ShiftName = "Acil Güvenlik Yaması" },
                new ShiftType { ShiftName = "Server & Altyapı Geçiş Nöbeti" },
                new ShiftType { ShiftName = "Server Geçiş Nöbeti" },
                new ShiftType { ShiftName = "Firewall Geçiş Nöbeti" },
                new ShiftType { ShiftName = "Acil Güvenlik Yaması Nöbeti" },
                new ShiftType { ShiftName = "Haftalık Yaygınlaştırma Nöbeti" },
                new ShiftType { ShiftName = "Aylık Yaygınlaştırma Nöbeti" },
                new ShiftType { ShiftName = "özel" },
            };
            foreach (var st in shiftTypes) { if (!context.ShiftTypes.Any(x => x.ShiftName == st.ShiftName)) context.ShiftTypes.Add(st); }
            context.SaveChanges();

            // 4. Seed MonthlyReleaseShifts
            if (!context.MonthlyReleaseShifts.Any())
            {
                var monthlyShifts = new MonthlyReleaseShift[]
                {
                    new MonthlyReleaseShift { MonthName = "Mart 2026 Yaygınlaştırması", ReleaseDate = new DateTime(2026, 3, 11, 0, 0, 0), AssignedUsers = "Ahmet Yılmaz, Fatma Şahin, Ali Çelik, Elif Aydın, Zeynep Yıldız", JiraTicketNo = "REL-2026-01", ExternalLink = "https://jira.ziraat.com/browse/REL-2026-01", Note = null, IsFinished = true, CreatedByUserName = "Ahmet Yılmaz", UpdatedByUserName = "Ahmet Yılmaz", CreatedAt = null, UpdatedAt = new DateTime(2026, 7, 21, 11, 38, 22) },
                    new MonthlyReleaseShift { MonthName = "Şubat 2026 Yaygınlaştırması", ReleaseDate = new DateTime(2026, 2, 12, 0, 0, 0), AssignedUsers = "Mehmet Kaya, Ayşe Demir, Mustafa Öztürk, Ömer Arslan", JiraTicketNo = "REL-2026-02", ExternalLink = "https://jira.ziraat.com/browse/REL-2026-02", Note = null, IsFinished = true, CreatedByUserName = "Ahmet Yılmaz", UpdatedByUserName = null, CreatedAt = null, UpdatedAt = null },
                    new MonthlyReleaseShift { MonthName = "Mart 2026 Yaygınlaştırması", ReleaseDate = new DateTime(2026, 3, 19, 0, 0, 0), AssignedUsers = "Zeynep Yıldız, Selin Bulut, Can Koç, Esma Yılmaz", JiraTicketNo = "REL-2026-03", ExternalLink = "https://jira.ziraat.com/browse/REL-2026-03", Note = null, IsFinished = true, CreatedByUserName = "Esma Yılmaz", UpdatedByUserName = null, CreatedAt = null, UpdatedAt = null },
                    new MonthlyReleaseShift { MonthName = "Nisan 2026 Yaygınlaştırması", ReleaseDate = new DateTime(2026, 4, 16, 0, 0, 0), AssignedUsers = "Fatma Şahin, Esma İrem  Öztürk, Ali Çelik, Mehmet Kaya, Elif Aydın", JiraTicketNo = "REL-2026-04", ExternalLink = "https://jira.ziraat.com/browse/REL-2026-04", Note = null, IsFinished = true, CreatedByUserName = "Ali Çelik", UpdatedByUserName = "Ahmet Yılmaz", CreatedAt = null, UpdatedAt = new DateTime(2026, 7, 20, 15, 56, 14) },
                    new MonthlyReleaseShift { MonthName = "Mayıs 2026 Yaygınlaştırması", ReleaseDate = new DateTime(2026, 5, 14, 0, 0, 0), AssignedUsers = "Ayşe Demir, Mustafa Öztürk, Ömer Arslan, Zeynep Yıldız", JiraTicketNo = "REL-2026-05", ExternalLink = "https://jira.ziraat.com/browse/REL-2026-05", Note = null, IsFinished = true, CreatedByUserName = "Ahmet Yılmaz", UpdatedByUserName = null, CreatedAt = null, UpdatedAt = null },
                    new MonthlyReleaseShift { MonthName = "Haziran 2026 Yaygınlaştırması", ReleaseDate = new DateTime(2026, 6, 11, 0, 0, 0), AssignedUsers = "Selin Bulut, Can Koç, Ahmet Yılmaz, Fatma Şahin", JiraTicketNo = "REL-2026-06", ExternalLink = "https://jira.ziraat.com/browse/REL-2026-06", Note = null, IsFinished = true, CreatedByUserName = "Esma Yılmaz", UpdatedByUserName = null, CreatedAt = null, UpdatedAt = null },
                    new MonthlyReleaseShift { MonthName = "Temmuz 2026 Yaygınlaştırması", ReleaseDate = new DateTime(2026, 7, 16, 0, 0, 0), AssignedUsers = "Ali Çelik, Elif Aydın, Mehmet Kaya, Ayşe Demir", JiraTicketNo = "REL-2026-07", ExternalLink = "https://jira.ziraat.com/browse/REL-2026-07", Note = null, IsFinished = true, CreatedByUserName = "Ali Çelik", UpdatedByUserName = null, CreatedAt = null, UpdatedAt = null },
                    new MonthlyReleaseShift { MonthName = "Ağustos 2026 Yaygınlaştırması", ReleaseDate = new DateTime(2026, 8, 13, 0, 0, 0), AssignedUsers = "Mustafa Öztürk, Ömer Arslan, Zeynep Yıldız, Selin Bulut", JiraTicketNo = "REL-2026-08", ExternalLink = "https://jira.ziraat.com/browse/REL-2026-08", Note = null, IsFinished = false, CreatedByUserName = "Ahmet Yılmaz", UpdatedByUserName = null, CreatedAt = null, UpdatedAt = null },
                    new MonthlyReleaseShift { MonthName = "Eylül 2026 Yaygınlaştırması", ReleaseDate = new DateTime(2026, 9, 17, 0, 0, 0), AssignedUsers = "Fatma Şahin, Can Koç", JiraTicketNo = "REL-2026-09", ExternalLink = "https://jira.ziraat.com/browse/REL-2026-09", Note = null, IsFinished = false, CreatedByUserName = "Ali Çelik", UpdatedByUserName = "Ahmet Yılmaz", CreatedAt = null, UpdatedAt = new DateTime(2026, 7, 20, 15, 41, 22) },
                    new MonthlyReleaseShift { MonthName = "Ekim 2026 Yaygınlaştırması", ReleaseDate = new DateTime(2026, 10, 15, 0, 0, 0), AssignedUsers = "Elif Aydın, Mehmet Kaya, Ayşe Demir, Mustafa Öztürk", JiraTicketNo = "REL-2026-10", ExternalLink = "https://jira.ziraat.com/browse/REL-2026-10", Note = null, IsFinished = false, CreatedByUserName = "Ahmet Yılmaz", UpdatedByUserName = null, CreatedAt = null, UpdatedAt = null },
                    new MonthlyReleaseShift { MonthName = "Kasım 2026 Yaygınlaştırması", ReleaseDate = new DateTime(2026, 11, 12, 0, 0, 0), AssignedUsers = "Ömer Arslan, Zeynep Yıldız, Selin Bulut, Can Koç", JiraTicketNo = "REL-2026-11", ExternalLink = "https://jira.ziraat.com/browse/REL-2026-11", Note = null, IsFinished = false, CreatedByUserName = "Esma Yılmaz", UpdatedByUserName = null, CreatedAt = null, UpdatedAt = null },
                    new MonthlyReleaseShift { MonthName = "Aralık 2026 Yaygınlaştırması", ReleaseDate = new DateTime(2026, 12, 17, 0, 0, 0), AssignedUsers = "Ahmet Yılmaz, Fatma Şahin, Ali Çelik, Mehmet Kaya", JiraTicketNo = "REL-2026-12", ExternalLink = "https://jira.ziraat.com/browse/REL-2026-12", Note = null, IsFinished = false, CreatedByUserName = "Ahmet Yılmaz", UpdatedByUserName = null, CreatedAt = null, UpdatedAt = null },
                    new MonthlyReleaseShift { MonthName = "Haftalık: Hafta 30 (20.07 - 26.07.2026)", ReleaseDate = new DateTime(2026, 7, 20, 0, 0, 0), AssignedUsers = "Ahmet Yılmaz, Selin Bulut", JiraTicketNo = null, ExternalLink = null, Note = null, IsFinished = true, CreatedByUserName = "Esma İrem  Öztürk", UpdatedByUserName = null, CreatedAt = new DateTime(2026, 7, 22, 15, 42, 25), UpdatedAt = null },
                    new MonthlyReleaseShift { MonthName = "Haftalık: Hafta 31 (27.07 - 02.08.2026)", ReleaseDate = new DateTime(2026, 7, 27, 0, 0, 0), AssignedUsers = "Esma Yılmaz, Mustafa Öztürk", JiraTicketNo = null, ExternalLink = null, Note = null, IsFinished = false, CreatedByUserName = "Esma İrem  Öztürk", UpdatedByUserName = null, CreatedAt = new DateTime(2026, 7, 22, 15, 42, 25), UpdatedAt = null },
                    new MonthlyReleaseShift { MonthName = "Haftalık: Hafta 32 (03.08 - 09.08.2026)", ReleaseDate = new DateTime(2026, 8, 3, 0, 0, 0), AssignedUsers = "Esma Yılmaz, Selin Bulut", JiraTicketNo = null, ExternalLink = null, Note = null, IsFinished = false, CreatedByUserName = "Esma İrem  Öztürk", UpdatedByUserName = null, CreatedAt = new DateTime(2026, 7, 22, 15, 42, 25), UpdatedAt = null },
                    new MonthlyReleaseShift { MonthName = "Haftalık: Hafta 33 (10.08 - 16.08.2026)", ReleaseDate = new DateTime(2026, 8, 10, 0, 0, 0), AssignedUsers = "Ahmet Yılmaz, Esma İrem  Öztürk", JiraTicketNo = null, ExternalLink = null, Note = null, IsFinished = false, CreatedByUserName = "Esma İrem  Öztürk", UpdatedByUserName = null, CreatedAt = new DateTime(2026, 7, 22, 15, 42, 25), UpdatedAt = null },
                    new MonthlyReleaseShift { MonthName = "Haftalık: Hafta 34 (17.08 - 23.08.2026)", ReleaseDate = new DateTime(2026, 8, 17, 0, 0, 0), AssignedUsers = "Ahmet Yılmaz, Esma İrem  Öztürk", JiraTicketNo = null, ExternalLink = null, Note = null, IsFinished = false, CreatedByUserName = "Esma İrem  Öztürk", UpdatedByUserName = null, CreatedAt = new DateTime(2026, 7, 22, 15, 42, 25), UpdatedAt = null },
                    new MonthlyReleaseShift { MonthName = "Haftalık: Hafta 35 (24.08 - 30.08.2026)", ReleaseDate = new DateTime(2026, 8, 24, 0, 0, 0), AssignedUsers = "Ahmet Yılmaz, Fatma Şahin", JiraTicketNo = null, ExternalLink = null, Note = null, IsFinished = false, CreatedByUserName = "Esma İrem  Öztürk", UpdatedByUserName = null, CreatedAt = new DateTime(2026, 7, 22, 15, 42, 25), UpdatedAt = null },
                    new MonthlyReleaseShift { MonthName = "Haftalık: Hafta 36 (31.08 - 06.09.2026)", ReleaseDate = new DateTime(2026, 8, 31, 0, 0, 0), AssignedUsers = "Ahmet Yılmaz, Selin Bulut", JiraTicketNo = null, ExternalLink = null, Note = null, IsFinished = false, CreatedByUserName = "Esma İrem  Öztürk", UpdatedByUserName = null, CreatedAt = new DateTime(2026, 7, 22, 15, 42, 25), UpdatedAt = null },
                    new MonthlyReleaseShift { MonthName = "Haftalık: Hafta 37 (07.09 - 13.09.2026)", ReleaseDate = new DateTime(2026, 9, 7, 0, 0, 0), AssignedUsers = "Esma Yılmaz, Selin Bulut", JiraTicketNo = null, ExternalLink = null, Note = null, IsFinished = false, CreatedByUserName = "Esma İrem  Öztürk", UpdatedByUserName = null, CreatedAt = new DateTime(2026, 7, 22, 15, 42, 25), UpdatedAt = null },
                    new MonthlyReleaseShift { MonthName = "Haftalık: Hafta 30 (20.07 - 26.07.2026)", ReleaseDate = new DateTime(2026, 7, 20, 0, 0, 0), AssignedUsers = " Barbaros Hayrettin, Ali Koç", JiraTicketNo = null, ExternalLink = null, Note = null, IsFinished = true, CreatedByUserName = "Neslihan Yilmaz", UpdatedByUserName = null, CreatedAt = new DateTime(2026, 7, 23, 13, 36, 22), UpdatedAt = null },
                    new MonthlyReleaseShift { MonthName = "Haftalık: Hafta 31 (27.07 - 02.08.2026)", ReleaseDate = new DateTime(2026, 7, 27, 0, 0, 0), AssignedUsers = "Elif Aydın, Can Koç", JiraTicketNo = null, ExternalLink = null, Note = null, IsFinished = false, CreatedByUserName = "Neslihan Yilmaz", UpdatedByUserName = null, CreatedAt = new DateTime(2026, 7, 23, 13, 36, 22), UpdatedAt = null },
                    new MonthlyReleaseShift { MonthName = "Haftalık: Hafta 32 (03.08 - 09.08.2026)", ReleaseDate = new DateTime(2026, 8, 3, 0, 0, 0), AssignedUsers = "Mehmet Kaya, Ali Çelik", JiraTicketNo = null, ExternalLink = null, Note = null, IsFinished = false, CreatedByUserName = "Neslihan Yilmaz", UpdatedByUserName = "Neslihan Yilmaz", CreatedAt = new DateTime(2026, 7, 23, 13, 36, 22), UpdatedAt = new DateTime(2026, 7, 23, 13, 37, 4) },
                    new MonthlyReleaseShift { MonthName = "Haftalık: Hafta 33 (10.08 - 16.08.2026)", ReleaseDate = new DateTime(2026, 8, 10, 0, 0, 0), AssignedUsers = "Elif Aydın, Can Koç", JiraTicketNo = null, ExternalLink = null, Note = null, IsFinished = false, CreatedByUserName = "Neslihan Yilmaz", UpdatedByUserName = null, CreatedAt = new DateTime(2026, 7, 23, 13, 36, 22), UpdatedAt = null },
                    new MonthlyReleaseShift { MonthName = "Haftalık: Hafta 34 (17.08 - 23.08.2026)", ReleaseDate = new DateTime(2026, 8, 17, 0, 0, 0), AssignedUsers = "Mehmet Kaya, Ali Koç", JiraTicketNo = null, ExternalLink = null, Note = null, IsFinished = false, CreatedByUserName = "Neslihan Yilmaz", UpdatedByUserName = null, CreatedAt = new DateTime(2026, 7, 23, 13, 36, 22), UpdatedAt = null },
                    new MonthlyReleaseShift { MonthName = "Haftalık: Hafta 35 (24.08 - 30.08.2026)", ReleaseDate = new DateTime(2026, 8, 24, 0, 0, 0), AssignedUsers = "Elif Aydın, Can Koç", JiraTicketNo = null, ExternalLink = null, Note = null, IsFinished = false, CreatedByUserName = "Neslihan Yilmaz", UpdatedByUserName = null, CreatedAt = new DateTime(2026, 7, 23, 13, 36, 22), UpdatedAt = null },
                    new MonthlyReleaseShift { MonthName = "Haftalık: Hafta 30 (20.07 - 26.07.2026)", ReleaseDate = new DateTime(2026, 7, 20, 0, 0, 0), AssignedUsers = "Ayşe Demir, Gülşah yavuz", JiraTicketNo = null, ExternalLink = null, Note = null, IsFinished = true, CreatedByUserName = "Neslihan Yilmaz", UpdatedByUserName = null, CreatedAt = new DateTime(2026, 7, 23, 13, 37, 26), UpdatedAt = null },
                    new MonthlyReleaseShift { MonthName = "Haftalık Yaygınlaştırma: Temmuz 2026 - Hafta 4 (20.07.2026 - 26.07.2026)", ReleaseDate = new DateTime(2026, 7, 25, 0, 0, 0), AssignedUsers = "Ahmet Yılmaz, Selin Bulut, Can Koç,  Barbaros Hayrettin, Ayşe Demir, Gülşah yavuz", JiraTicketNo = null, ExternalLink = null, Note = null, IsFinished = true, CreatedByUserName = "Esma İrem  Öztürk", UpdatedByUserName = null, CreatedAt = new DateTime(2026, 7, 23, 13, 56, 57), UpdatedAt = null },
                    new MonthlyReleaseShift { MonthName = "Haftalık: Hafta 31 (27.07 - 02.08.2026)", ReleaseDate = new DateTime(2026, 7, 27, 0, 0, 0), AssignedUsers = "Ömer Arslan, Gülşah yavuz", JiraTicketNo = null, ExternalLink = null, Note = null, IsFinished = false, CreatedByUserName = "Esma İrem  Öztürk", UpdatedByUserName = null, CreatedAt = new DateTime(2026, 7, 27, 11, 58, 1), UpdatedAt = null },
                    new MonthlyReleaseShift { MonthName = "Haftalık: Hafta 32 (03.08 - 09.08.2026)", ReleaseDate = new DateTime(2026, 8, 3, 0, 0, 0), AssignedUsers = "Ayşe Demir, Zeynep Yıldız", JiraTicketNo = null, ExternalLink = null, Note = null, IsFinished = false, CreatedByUserName = "Esma İrem  Öztürk", UpdatedByUserName = null, CreatedAt = new DateTime(2026, 7, 27, 11, 58, 14), UpdatedAt = null },
                    new MonthlyReleaseShift { MonthName = "Haftalık: Hafta 33 (10.08 - 16.08.2026)", ReleaseDate = new DateTime(2026, 8, 10, 0, 0, 0), AssignedUsers = "Ömer Arslan, Zeynep Yıldız", JiraTicketNo = null, ExternalLink = null, Note = null, IsFinished = false, CreatedByUserName = "Esma İrem  Öztürk", UpdatedByUserName = null, CreatedAt = new DateTime(2026, 7, 27, 11, 58, 14), UpdatedAt = null },
                    new MonthlyReleaseShift { MonthName = "Haftalık: Hafta 34 (17.08 - 23.08.2026)", ReleaseDate = new DateTime(2026, 8, 17, 0, 0, 0), AssignedUsers = "Ayşe Demir, Gülşah yavuz", JiraTicketNo = null, ExternalLink = null, Note = null, IsFinished = false, CreatedByUserName = "Esma İrem  Öztürk", UpdatedByUserName = null, CreatedAt = new DateTime(2026, 7, 27, 11, 58, 14), UpdatedAt = null },
                    new MonthlyReleaseShift { MonthName = "Haftalık: Hafta 35 (24.08 - 30.08.2026)", ReleaseDate = new DateTime(2026, 8, 24, 0, 0, 0), AssignedUsers = "Ayşe Demir, Zeynep Yıldız", JiraTicketNo = null, ExternalLink = null, Note = null, IsFinished = false, CreatedByUserName = "Esma İrem  Öztürk", UpdatedByUserName = null, CreatedAt = new DateTime(2026, 7, 27, 11, 58, 14), UpdatedAt = null },
                    new MonthlyReleaseShift { MonthName = "Haftalık: Hafta 36 (31.08 - 06.09.2026)", ReleaseDate = new DateTime(2026, 8, 31, 0, 0, 0), AssignedUsers = "Ayşe Demir, Gülşah yavuz", JiraTicketNo = null, ExternalLink = null, Note = null, IsFinished = false, CreatedByUserName = "Esma İrem  Öztürk", UpdatedByUserName = null, CreatedAt = new DateTime(2026, 7, 27, 11, 58, 14), UpdatedAt = null },
                };
                context.MonthlyReleaseShifts.AddRange(monthlyShifts);
                context.SaveChanges();
            }

            // 5. Seed CustomShifts
            if (!context.CustomShifts.Any())
            {
                var customShifts = new CustomShift[]
                {
                    new CustomShift { Topic = "Server & Altyapı Geçiş Nöbeti", Description = "CR-1019 Altyapı Sunucu Geçiş Nöbeti", AssignedUsers = "Ahmet Yılmaz, Esma Yılmaz", ShiftDate = new DateTime(2026, 7, 20, 0, 0, 0), ExternalLink = "https://jira.ziraat.com/browse/CR-1019 ALTYAPI SUNUCU GEÇİŞ NÖBETİ", IsFinished = true, CreatedByUserName = "Esma Yılmaz", UpdatedByUserName = "Ahmet Yılmaz", CreatedAt = null, UpdatedAt = new DateTime(2026, 7, 20, 15, 54, 52) },
                    new CustomShift { Topic = "Firewall Kural Güncellemesi", Description = "CR-1024 Firewall Güncelleme Nöbeti", AssignedUsers = "Ahmet Yılmaz, Ali Çelik, Elif Aydın, Ayşe Demir, Zeynep Yıldız", ShiftDate = new DateTime(2026, 7, 21, 0, 0, 0), ExternalLink = "https://jira.ziraat.com/browse/CR-1024 FİREWALL GÜNCELLEME NÖBETİ", IsFinished = true, CreatedByUserName = "Ahmet Yılmaz", UpdatedByUserName = "Ahmet Yılmaz", CreatedAt = null, UpdatedAt = new DateTime(2026, 7, 20, 14, 46, 47) },
                    new CustomShift { Topic = "Acil Güvenlik Yaması", Description = "CR-1027 Güvenlik Yama Yüklemesi", AssignedUsers = "Mehmet Kaya, Ayşe Demir, Zeynep Yıldız", ShiftDate = new DateTime(2026, 7, 22, 0, 0, 0), ExternalLink = "https://jira.ziraat.com/browse/CR-1027", IsFinished = true, CreatedByUserName = "Ali Çelik", UpdatedByUserName = null, CreatedAt = null, UpdatedAt = null },
                    new CustomShift { Topic = "Server Geçiş Nöbeti", Description = "CR-1019", AssignedUsers = "Esma İrem  Öztürk, Ali Çelik", ShiftDate = new DateTime(2026, 7, 21, 0, 0, 0), ExternalLink = "https://jira.ziraat.com/browse/CR-1019", IsFinished = true, CreatedByUserName = "Ahmet Yılmaz", UpdatedByUserName = null, CreatedAt = null, UpdatedAt = null },
                    new CustomShift { Topic = "Acil Güvenlik Yaması Nöbeti", Description = null, AssignedUsers = "Esma İrem  Öztürk", ShiftDate = new DateTime(2026, 7, 25, 0, 0, 0), ExternalLink = null, IsFinished = true, CreatedByUserName = "Esma İrem  Öztürk", UpdatedByUserName = null, CreatedAt = new DateTime(2026, 7, 22, 14, 42, 8), UpdatedAt = null },
                    new CustomShift { Topic = "Server Geçiş Nöbeti", Description = "zxcvf", AssignedUsers = "Mehmet Kaya", ShiftDate = new DateTime(2026, 7, 23, 0, 0, 0), ExternalLink = null, IsFinished = true, CreatedByUserName = "Neslihan Yilmaz", UpdatedByUserName = null, CreatedAt = new DateTime(2026, 7, 23, 13, 38, 30), UpdatedAt = null },
                    new CustomShift { Topic = "Server Geçiş Nöbeti", Description = null, AssignedUsers = "Elif Aydın, Ayşe Demir", ShiftDate = new DateTime(2026, 7, 24, 0, 0, 0), ExternalLink = null, IsFinished = true, CreatedByUserName = "Neslihan Yilmaz", UpdatedByUserName = null, CreatedAt = new DateTime(2026, 7, 23, 13, 39, 11), UpdatedAt = null },
                    new CustomShift { Topic = "Server Geçiş Nöbeti", Description = null, AssignedUsers = "Can Koç", ShiftDate = new DateTime(2026, 7, 27, 0, 0, 0), ExternalLink = null, IsFinished = false, CreatedByUserName = "Esma İrem  Öztürk", UpdatedByUserName = null, CreatedAt = new DateTime(2026, 7, 27, 11, 0, 13), UpdatedAt = null },
                    new CustomShift { Topic = "Yılbaşı Nöbeti", Description = "2022 Yılbaşı Geçiş ve Sistem Nöbeti", AssignedUsers = "Mehmet Kaya, Selin Bulut", ShiftDate = new DateTime(2022, 1, 1), IsFinished = true, CreatedByUserName = "Sistem", UpdatedByUserName = null, CreatedAt = new DateTime(2022, 1, 1), UpdatedAt = null },
                    new CustomShift { Topic = "Yılbaşı Nöbeti", Description = "2023 Yılbaşı Geçiş ve Sistem Nöbeti", AssignedUsers = "Ahmet Yılmaz, Fatma Şahin", ShiftDate = new DateTime(2023, 1, 1), IsFinished = true, CreatedByUserName = "Sistem", UpdatedByUserName = null, CreatedAt = new DateTime(2023, 1, 1), UpdatedAt = null },
                    new CustomShift { Topic = "Yılbaşı Nöbeti", Description = "2024 Yılbaşı Geçiş ve Sistem Nöbeti", AssignedUsers = "Can Koç, Gülşah Yavuz", ShiftDate = new DateTime(2024, 1, 1), IsFinished = true, CreatedByUserName = "Sistem", UpdatedByUserName = null, CreatedAt = new DateTime(2024, 1, 1), UpdatedAt = null },
                    new CustomShift { Topic = "Yılbaşı Nöbeti", Description = "2025 Yılbaşı Geçiş ve Sistem Nöbeti", AssignedUsers = "Ali Koç, Ömer Arslan", ShiftDate = new DateTime(2025, 1, 1), IsFinished = true, CreatedByUserName = "Sistem", UpdatedByUserName = null, CreatedAt = new DateTime(2025, 1, 1), UpdatedAt = null },
                    new CustomShift { Topic = "Yılbaşı Nöbeti", Description = "2026 Yılbaşı Geçiş ve Sistem Nöbeti", AssignedUsers = "Mustafa Öztürk, Esma İrem Öztürk", ShiftDate = new DateTime(2026, 1, 1), IsFinished = true, CreatedByUserName = "Sistem", UpdatedByUserName = null, CreatedAt = new DateTime(2026, 1, 1), UpdatedAt = null },
                };
                context.CustomShifts.AddRange(customShifts);
                context.SaveChanges();
            }

            // Ensure Yılbaşı Nöbeti seed records exist if table already had data
            if (!context.CustomShifts.Any(cs => cs.Topic == "Yılbaşı Nöbeti"))
            {
                var yilbasiShifts = new CustomShift[]
                {
                    new CustomShift { Topic = "Yılbaşı Nöbeti", Description = "2022 Yılbaşı Geçiş ve Sistem Nöbeti", AssignedUsers = "Mehmet Kaya, Selin Bulut", ShiftDate = new DateTime(2022, 1, 1), IsFinished = true, CreatedByUserName = "Sistem" },
                    new CustomShift { Topic = "Yılbaşı Nöbeti", Description = "2023 Yılbaşı Geçiş ve Sistem Nöbeti", AssignedUsers = "Ahmet Yılmaz, Fatma Şahin", ShiftDate = new DateTime(2023, 1, 1), IsFinished = true, CreatedByUserName = "Sistem" },
                    new CustomShift { Topic = "Yılbaşı Nöbeti", Description = "2024 Yılbaşı Geçiş ve Sistem Nöbeti", AssignedUsers = "Can Koç, Gülşah Yavuz", ShiftDate = new DateTime(2024, 1, 1), IsFinished = true, CreatedByUserName = "Sistem" },
                    new CustomShift { Topic = "Yılbaşı Nöbeti", Description = "2025 Yılbaşı Geçiş ve Sistem Nöbeti", AssignedUsers = "Ali Koç, Ömer Arslan", ShiftDate = new DateTime(2025, 1, 1), IsFinished = true, CreatedByUserName = "Sistem" },
                    new CustomShift { Topic = "Yılbaşı Nöbeti", Description = "2026 Yılbaşı Geçiş ve Sistem Nöbeti", AssignedUsers = "Mustafa Öztürk, Esma İrem Öztürk", ShiftDate = new DateTime(2026, 1, 1), IsFinished = true, CreatedByUserName = "Sistem" }
                };
                context.CustomShifts.AddRange(yilbasiShifts);
                context.SaveChanges();
            }

            // 6. Seed Leaves
            if (!context.Leaves.Any())
            {
                var leaves = new Leave[]
                {
                    new Leave { UserId = 69, StartDate = new DateTime(2026, 7, 25, 0, 0, 0), EndDate = new DateTime(2026, 7, 29, 0, 0, 0), Status = "Approved", IsSpecialRequest = false, RequestNote = null, RequestedAt = null, ApprovedAt = null, ApprovedByUserName = null },
                    new Leave { UserId = 70, StartDate = new DateTime(2026, 7, 21, 0, 0, 0), EndDate = new DateTime(2026, 7, 22, 0, 0, 0), Status = "Approved", IsSpecialRequest = false, RequestNote = null, RequestedAt = null, ApprovedAt = null, ApprovedByUserName = null },
                    new Leave { UserId = 71, StartDate = new DateTime(2026, 7, 30, 0, 0, 0), EndDate = new DateTime(2026, 8, 1, 0, 0, 0), Status = "Approved", IsSpecialRequest = false, RequestNote = null, RequestedAt = null, ApprovedAt = null, ApprovedByUserName = null },
                    new Leave { UserId = 79, StartDate = new DateTime(2026, 7, 22, 0, 0, 0), EndDate = new DateTime(2026, 7, 24, 0, 0, 0), Status = "Rejected", IsSpecialRequest = false, RequestNote = null, RequestedAt = new DateTime(2026, 7, 21, 13, 25, 3), ApprovedAt = null, ApprovedByUserName = null },
                    new Leave { UserId = 79, StartDate = new DateTime(2026, 7, 21, 0, 0, 0), EndDate = new DateTime(2026, 7, 21, 0, 0, 0), Status = "Rejected", IsSpecialRequest = false, RequestNote = null, RequestedAt = new DateTime(2026, 7, 21, 13, 25, 7), ApprovedAt = null, ApprovedByUserName = null },
                    new Leave { UserId = 67, StartDate = new DateTime(2026, 7, 31, 0, 0, 0), EndDate = new DateTime(2026, 8, 1, 0, 0, 0), Status = "Approved", IsSpecialRequest = true, RequestNote = "'Takip' ekibindeki 'Analist' rolündeki 2 personelden 1 kadarı (31.07.2026, 01.08.2026) tarihlerinde izinli görünmektedir (%50 veya üzeri rol çakışması). Özel izin gereklidir.", RequestedAt = new DateTime(2026, 7, 21, 14, 16, 1), ApprovedAt = new DateTime(2026, 7, 21, 14, 16, 1), ApprovedByUserName = "Ahmet Yılmaz" },
                    new Leave { UserId = 79, StartDate = new DateTime(2026, 7, 23, 0, 0, 0), EndDate = new DateTime(2026, 7, 28, 0, 0, 0), Status = "Rejected", IsSpecialRequest = false, RequestNote = null, RequestedAt = new DateTime(2026, 7, 21, 14, 31, 5), ApprovedAt = null, ApprovedByUserName = null },
                    new Leave { UserId = 79, StartDate = new DateTime(2026, 7, 22, 0, 0, 0), EndDate = new DateTime(2026, 7, 28, 0, 0, 0), Status = "Cancelled", IsSpecialRequest = false, RequestNote = "🔄 Onaylı izin değişikliği talep edildi (22.07.2026 - 28.07.2026)", RequestedAt = new DateTime(2026, 7, 21, 15, 24, 56), ApprovedAt = new DateTime(2026, 7, 21, 15, 25, 4), ApprovedByUserName = "Elif Aydın" },
                    new Leave { UserId = 75, StartDate = new DateTime(2026, 7, 23, 0, 0, 0), EndDate = new DateTime(2026, 7, 24, 0, 0, 0), Status = "Cancelled", IsSpecialRequest = false, RequestNote = null, RequestedAt = new DateTime(2026, 7, 21, 16, 19, 37), ApprovedAt = new DateTime(2026, 7, 21, 16, 19, 37), ApprovedByUserName = "Elif Aydın" },
                    new Leave { UserId = 79, StartDate = new DateTime(2026, 7, 22, 0, 0, 0), EndDate = new DateTime(2026, 7, 23, 0, 0, 0), Status = "Cancelled", IsSpecialRequest = false, RequestNote = null, RequestedAt = new DateTime(2026, 7, 21, 16, 21, 45), ApprovedAt = new DateTime(2026, 7, 21, 16, 23, 7), ApprovedByUserName = "Esma İrem  Öztürk" },
                    new Leave { UserId = 79, StartDate = new DateTime(2026, 7, 24, 10, 30, 0), EndDate = new DateTime(2026, 7, 24, 16, 0, 0), Status = "Cancelled", IsSpecialRequest = false, RequestNote = null, RequestedAt = new DateTime(2026, 7, 22, 11, 40, 48), ApprovedAt = new DateTime(2026, 7, 22, 11, 41, 17), ApprovedByUserName = "Esma İrem  Öztürk" },
                    new Leave { UserId = 79, StartDate = new DateTime(2026, 7, 22, 13, 0, 0), EndDate = new DateTime(2026, 7, 22, 13, 30, 0), Status = "Rejected", IsSpecialRequest = false, RequestNote = null, RequestedAt = new DateTime(2026, 7, 22, 11, 49, 11), ApprovedAt = null, ApprovedByUserName = "Elif Aydın" },
                    new Leave { UserId = 83, StartDate = new DateTime(2026, 7, 22, 0, 0, 0), EndDate = new DateTime(2026, 7, 22, 0, 0, 0), Status = "Approved", IsSpecialRequest = false, RequestNote = null, RequestedAt = new DateTime(2026, 7, 22, 17, 14, 56), ApprovedAt = new DateTime(2026, 7, 22, 17, 14, 56), ApprovedByUserName = "Neslihan Yilmaz" },
                    new Leave { UserId = 79, StartDate = new DateTime(2026, 7, 23, 16, 0, 0), EndDate = new DateTime(2026, 7, 23, 16, 30, 0), Status = "Approved", IsSpecialRequest = false, RequestNote = null, RequestedAt = new DateTime(2026, 7, 23, 15, 57, 50), ApprovedAt = new DateTime(2026, 7, 23, 15, 58, 18), ApprovedByUserName = "Neslihan Yilmaz" },
                    new Leave { UserId = 79, StartDate = new DateTime(2026, 7, 28, 0, 0, 0), EndDate = new DateTime(2026, 7, 28, 0, 0, 0), Status = "Approved", IsSpecialRequest = false, RequestNote = null, RequestedAt = new DateTime(2026, 7, 26, 1, 55, 43), ApprovedAt = new DateTime(2026, 7, 26, 2, 3, 1), ApprovedByUserName = "Neslihan Keske" },
                };
                context.Leaves.AddRange(leaves);
                context.SaveChanges();
            }
        }

        public static void EnsureMockProjectsSeeded(AppDbContext context)
        {
        }

        public static void EnsureQuarterProjectsSeeded(AppDbContext context)
        {
            return; // Disabled automatic re-seeding of deleted projects
            var users = context.Users.ToList();
            if (!users.Any()) return;

            // Cleanup: Non-completed status projects (Planlandı, Devam Ediyor etc.) should not have actual release date until completed
            var existingNonCompletedProjects = context.Projects.Where(p => p.ProjectStatus != "Tamamlandı" && p.ActualReleaseDate != null).ToList();
            if (existingNonCompletedProjects.Any())
            {
                foreach (var p in existingNonCompletedProjects)
                {
                    p.ActualReleaseDate = null;
                }
                context.SaveChanges();
            }

            var analysts = users.Where(u => u.Title != null && u.Title.Contains("Analist")).ToList();
            if (!analysts.Any()) analysts = users.Take(2).ToList();

            var developers = users.Where(u => u.Title != null && (u.Title.Contains("Developer") || u.Title.Contains("Yazılım"))).ToList();
            if (!developers.Any()) developers = users.Skip(2).Take(4).ToList();

            User getAnalystUser(int idx) => analysts[idx % analysts.Count];
            User getDevUser(int idx) => developers[idx % developers.Count];

            var quartersData = new[]
            {
                new { Q = 1, StartMonth = 1,
                      Projects = new[]
                      {
                          new { Name = "Kredi Kartı Limit Artırım Otomasyonu", Type = "Proje", Status = "Devam Ediyor", Ext = "", Team = "Takip", Budget = 60m, Actual = 40m, DelayDays = 0, PlannedDay = 15, Month = 2, Gmy = "Ürün Yönetimi ve Dijital Bankacılık GMY", Unit = "Bireysel Krediler Süreç Bölüm Bşk." },
                          new { Name = "Mobil Bankacılık QR İle Ödeme", Type = "Proje", Status = "Tamamlandı", Ext = "", Team = "Tahsis", Budget = 45m, Actual = 45m, DelayDays = 0, PlannedDay = 10, Month = 3, Gmy = "Ürün Yönetimi ve Dijital Bankacılık GMY", Unit = "Dijital Bankacılık Bölüm Bşk." },
                          new { Name = "POS Anlık Mutabakat Sistemi", Type = "KG", Status = "Tamamlandı", Ext = "", Team = "Teminat", Budget = 30m, Actual = 30m, DelayDays = 0, PlannedDay = 20, Month = 1, Gmy = "Operasyonel Risk ve Güvenlik GMY", Unit = "Ödeme Sistemleri Bölüm Bşk." },
                          new { Name = "Kurumsal Kredi Risk Skorlama", Type = "Proje", Status = "Tamamlandı", Ext = "", Team = "Tahsis", Budget = 80m, Actual = 85m, DelayDays = 7, PlannedDay = 15, Month = 2, Gmy = "Kredi Tahsis ve Yönetimi GMY", Unit = "Kurumsal Krediler Bölüm Bşk." },
                          new { Name = "KVKK Uyum & Veri Anonimleştirme", Type = "Proje", Status = "Tamamlandı", Ext = "", Team = "Takip", Budget = 50m, Actual = 50m, DelayDays = 0, PlannedDay = 28, Month = 3, Gmy = "Hukuk ve Uyum GMY", Unit = "Mevzuat ve Uyum Bölüm Bşk." },
                          new { Name = "Bireysel Kredi Başvuru Mikroservisi", Type = "KG", Status = "Devam Ediyor", Ext = "", Team = "Teminat", Budget = 40m, Actual = 25m, DelayDays = 0, PlannedDay = 18, Month = 3, Gmy = "Kredi Politikaları GMY", Unit = "Bireysel Krediler Tahsis Bşk." },
                          new { Name = "Dış Firma Garanti BBVA Entegrasyonu", Type = "Dış Firma", Status = "Tamamlandı", Ext = "Softtech", Team = "Tahsis", Budget = 90m, Actual = 90m, DelayDays = 0, PlannedDay = 12, Month = 2, Gmy = "Teknoloji ve Mimari GMY", Unit = "Dış Entegrasyonlar Bölüm Bşk." },
                          new { Name = "Hazine Döviz Alım-Satım Botu", Type = "Proje", Status = "Planlandı", Ext = "", Team = "Takip", Budget = 70m, Actual = 0m, DelayDays = 0, PlannedDay = 25, Month = 3, Gmy = "Hazine ve Sermaye Piyasaları GMY", Unit = "Hazine Operasyonları Bölüm Bşk." },
                          new { Name = "Açık Bankacılık API Gateway", Type = "Paydaş Proje", Status = "Devam Ediyor", Ext = "", Team = "Tahsis", Budget = 100m, Actual = 60m, DelayDays = 0, PlannedDay = 30, Month = 3, Gmy = "Ürün Yönetimi ve Dijital Bankacılık GMY", Unit = "Açık Bankacılık Bölüm Bşk." },
                          new { Name = "E-Fatura & E-Defter Entegrasyonu", Type = "KG", Status = "Tamamlandı", Ext = "", Team = "Teminat", Budget = 35m, Actual = 35m, DelayDays = 0, PlannedDay = 14, Month = 1, Gmy = "Mali İşler GMY", Unit = "Muhasebe Süreçleri Bölüm Bşk." },
                          new { Name = "Çok Faktörlü Kimlik Doğrulama (MFA)", Type = "Proje", Status = "Tamamlandı", Ext = "", Team = "Takip", Budget = 65m, Actual = 70m, DelayDays = 8, PlannedDay = 10, Month = 3, Gmy = "Siber Güvenlik GMY", Unit = "Kimlik Yönetimi Bölüm Bşk." },
                          new { Name = "ATM Nüfus Cüzdanı Okuyucu", Type = "Paydaş Proje", Status = "Planlandı", Ext = "", Team = "Tahsis", Budget = 55m, Actual = 0m, DelayDays = 0, PlannedDay = 20, Month = 2, Gmy = "Kanal Yönetimi GMY", Unit = "ATM ve Saha Operasyonları Bölüm Bşk." }
                      }
                },
                new { Q = 2, StartMonth = 4,
                      Projects = new[]
                      {
                          new { Name = "Şube Ekranları Modernizasyonu", Type = "Proje", Status = "Tamamlandı", Ext = "", Team = "Takip", Budget = 75m, Actual = 75m, DelayDays = 0, PlannedDay = 16, Month = 4, Gmy = "Şube Operasyonları GMY", Unit = "Şube Süreçleri Bölüm Bşk." },
                          new { Name = "Otomatik Tahsilat & İcra Takip", Type = "Proje", Status = "Devam Ediyor", Ext = "", Team = "Tahsis", Budget = 80m, Actual = 50m, DelayDays = 0, PlannedDay = 22, Month = 5, Gmy = "Kredi Tasfiye GMY", Unit = "İcra ve Tahsilat Bölüm Bşk." },
                          new { Name = "Teminat Mektubu Dijitalleşme", Type = "Paydaş Proje", Status = "Tamamlandı", Ext = "", Team = "Teminat", Budget = 50m, Actual = 50m, DelayDays = 0, PlannedDay = 11, Month = 6, Gmy = "Ticari Krediler GMY", Unit = "Teminat Süreçleri Bölüm Bşk." },
                          new { Name = "Dış Firma Veripark Mobil CRM", Type = "Dış Firma", Status = "Tamamlandı", Ext = "Veripark", Team = "Takip", Budget = 110m, Actual = 115m, DelayDays = 8, PlannedDay = 20, Month = 5, Gmy = "Pazarlama GMY", Unit = "CRM Süreçleri Bölüm Bşk." },
                          new { Name = "KOBİ Kredi Değerlendirme Motoru", Type = "Proje", Status = "Devam Ediyor", Ext = "", Team = "Tahsis", Budget = 95m, Actual = 60m, DelayDays = 0, PlannedDay = 25, Month = 6, Gmy = "KOBİ Bankacılığı GMY", Unit = "KOBİ Tahsis Bölüm Bşk." },
                          new { Name = "Swift Ödeme Mesajları Parse Botu", Type = "KG", Status = "Tamamlandı", Ext = "", Team = "Teminat", Budget = 30m, Actual = 30m, DelayDays = 0, PlannedDay = 15, Month = 4, Gmy = "Dış İşler Operasyon GMY", Unit = "Swift ve Transferler Bölüm Bşk." },
                          new { Name = "Kart Temassız Limit Güncelleme", Type = "KG", Status = "Planlandı", Ext = "", Team = "Takip", Budget = 25m, Actual = 0m, DelayDays = 0, PlannedDay = 18, Month = 5, Gmy = "Kartlı Sistemler GMY", Unit = "Kredi Kartı Ürün Yönetimi" },
                          new { Name = "Borsa & Hisse Senedi Alım Ön Yüzü", Type = "Proje", Status = "Devam Ediyor", Ext = "", Team = "Tahsis", Budget = 85m, Actual = 40m, DelayDays = 0, PlannedDay = 30, Month = 6, Gmy = "Yatırım Bankacılığı GMY", Unit = "Sermaye Piyasaları Bölüm Bşk." },
                          new { Name = "İnternet Şubesi Şifre Sıfırlama", Type = "Paydaş Proje", Status = "Tamamlandı", Ext = "", Team = "Teminat", Budget = 40m, Actual = 40m, DelayDays = 0, PlannedDay = 10, Month = 4, Gmy = "Dijital Bankacılık GMY", Unit = "Güvenlik Süreçleri Bölüm Bşk." },
                          new { Name = "Kara Liste & Masak Kontrol Servisi", Type = "Proje", Status = "Tamamlandı", Ext = "", Team = "Takip", Budget = 60m, Actual = 65m, DelayDays = 7, PlannedDay = 12, Month = 6, Gmy = "Mevzuat ve Suç Gelirleri GMY", Unit = "Masak Uyum Bölüm Bşk." },
                          new { Name = "Şirketim Mobil Bildirim Servisi", Type = "KG", Status = "Devam Ediyor", Ext = "", Team = "Tahsis", Budget = 35m, Actual = 20m, DelayDays = 0, PlannedDay = 24, Month = 5, Gmy = "Kurumsal Bankacılık GMY", Unit = "Kurumsal İletişim Bölüm Bşk." },
                          new { Name = "Ziraat Pay Sanal POS Entegrasyonu", Type = "Proje", Status = "Planlandı", Ext = "", Team = "Teminat", Budget = 70m, Actual = 0m, DelayDays = 0, PlannedDay = 28, Month = 6, Gmy = "Ödeme Hizmetleri GMY", Unit = "Sanal POS Bölüm Bşk." }
                      }
                },
                new { Q = 3, StartMonth = 7,
                      Projects = new[]
                      {
                          new { Name = "Ön Yüz Tasarımı & UX Yenileme", Type = "Paydaş Proje", Status = "Devam Ediyor", Ext = "", Team = "Tahsis", Budget = 80m, Actual = 45m, DelayDays = 0, PlannedDay = 20, Month = 8, Gmy = "Ürün Yönetimi GMY", Unit = "UX & Tasarım Bölüm Bşk." },
                          new { Name = "Müşteri Analitiği & Segmentasyon", Type = "Proje", Status = "Planlandı", Ext = "", Team = "Teminat", Budget = 90m, Actual = 0m, DelayDays = 0, PlannedDay = 25, Month = 9, Gmy = "Pazarlama GMY", Unit = "Müşteri Analitiği Bölüm Bşk." },
                          new { Name = "Çek / Senet Otomatik Okuma (OCR)", Type = "KG", Status = "Devam Ediyor", Ext = "", Team = "Takip", Budget = 40m, Actual = 20m, DelayDays = 0, PlannedDay = 15, Month = 7, Gmy = "Operasyon GMY", Unit = "Çek Senet Süreçleri Bölüm Bşk." },
                          new { Name = "Dış Firma Innova Bulut Depolama", Type = "Dış Firma", Status = "Devam Ediyor", Ext = "Innova", Team = "Tahsis", Budget = 100m, Actual = 60m, DelayDays = 0, PlannedDay = 18, Month = 8, Gmy = "Altyapı ve Sistem GMY", Unit = "Bulut Teknolojileri Bölüm Bşk." },
                          new { Name = "Genel Kurul & Paydaş Yönetimi", Type = "Paydaş Proje", Status = "Planlandı", Ext = "", Team = "Teminat", Budget = 50m, Actual = 0m, DelayDays = 0, PlannedDay = 30, Month = 9, Gmy = "Genel Sekreterlik GMY", Unit = "Paydaş İlişkileri Bölüm Bşk." },
                          new { Name = "Yapay Zeka Destekli Chatbot Asistanı", Type = "Proje", Status = "Devam Ediyor", Ext = "", Team = "Takip", Budget = 120m, Actual = 70m, DelayDays = 0, PlannedDay = 22, Month = 8, Gmy = "Yapay Zeka ve Veri GMY", Unit = "AI Çözümleri Bölüm Bşk." },
                          new { Name = "Sanal Kart Oluşturma & Limit Belirleme", Type = "KG", Status = "Tamamlandı", Ext = "", Team = "Tahsis", Budget = 30m, Actual = 30m, DelayDays = 0, PlannedDay = 10, Month = 7, Gmy = "Kart Operasyonları GMY", Unit = "Sanal Kart Süreçleri Bölüm Bşk." },
                          new { Name = "Kıymetli Maden & Altın Hesabı", Type = "Proje", Status = "Planlandı", Ext = "", Team = "Teminat", Budget = 60m, Actual = 0m, DelayDays = 0, PlannedDay = 28, Month = 9, Gmy = "Bireysel Bankacılık GMY", Unit = "Yatırım Ürünleri Bölüm Bşk." },
                          new { Name = "E-Devlet Kapısı Üzerinden Hesap Açma", Type = "Proje", Status = "Devam Ediyor", Ext = "", Team = "Takip", Budget = 75m, Actual = 82m, DelayDays = 7, PlannedDay = 15, Month = 8, Gmy = "Dijital Dönüşüm GMY", Unit = "Kamu Entegrasyonları Bölüm Bşk." },
                          new { Name = "Otomatik Fatura Ödeme Talimatı", Type = "KG", Status = "Planlandı", Ext = "", Team = "Tahsis", Budget = 35m, Actual = 0m, DelayDays = 0, PlannedDay = 14, Month = 9, Gmy = "Ödeme Sistemleri GMY", Unit = "Fatura Tahsilat Bölüm Bşk." },
                          new { Name = "Merkez Bankası FAST Ödeme Servisi", Type = "Paydaş Proje", Status = "Devam Ediyor", Ext = "", Team = "Teminat", Budget = 85m, Actual = 50m, DelayDays = 0, PlannedDay = 26, Month = 9, Gmy = "Merkez Bankası İlişkileri GMY", Unit = "FAST Ödeme Süreçleri Bölüm Bşk." }
                      }
                },
                new { Q = 4, StartMonth = 10,
                      Projects = new[]
                      {
                          new { Name = "Yıl Sonu Bütçe & Maliyet Konsolidasyonu", Type = "Proje", Status = "Planlandı", Ext = "", Team = "Takip", Budget = 80m, Actual = 0m, DelayDays = 0, PlannedDay = 15, Month = 12, Gmy = "Mali Analiz ve Bütçe GMY", Unit = "Bütçe ve Raporlama Bölüm Bşk." },
                          new { Name = "Yatırım Hesabı Halka Arz Katılım", Type = "Paydaş Proje", Status = "Planlandı", Ext = "", Team = "Tahsis", Budget = 60m, Actual = 0m, DelayDays = 0, PlannedDay = 20, Month = 10, Gmy = "Yatırım Hizmetleri GMY", Unit = "Halka Arz Süreçleri Bölüm Bşk." },
                          new { Name = "Dış Firma Obss Test Otomasyonu", Type = "Dış Firma", Status = "Planlandı", Ext = "Obss", Team = "Teminat", Budget = 90m, Actual = 0m, DelayDays = 0, PlannedDay = 10, Month = 11, Gmy = "Kalite ve Test GMY", Unit = "Yazılım Kalite Güvence Bölüm Bşk." },
                          new { Name = "Kurumsal Tahvil & Bono Halka Arzı", Type = "Proje", Status = "Planlandı", Ext = "", Team = "Takip", Budget = 70m, Actual = 0m, DelayDays = 0, PlannedDay = 25, Month = 11, Gmy = "Sermaye Piyasaları GMY", Unit = "Tahvil ve Bono İşlemleri Bölüm Bşk." },
                          new { Name = "Dış Ticaret Akreditif İşlemleri", Type = "KG", Status = "Planlandı", Ext = "", Team = "Tahsis", Budget = 45m, Actual = 0m, DelayDays = 0, PlannedDay = 18, Month = 10, Gmy = "Dış Ticaret Operasyon GMY", Unit = "Akreditif Bölüm Bşk." },
                          new { Name = "Yapay Zeka İle Dolandırıcılık Tespit", Type = "Proje", Status = "Planlandı", Ext = "", Team = "Teminat", Budget = 110m, Actual = 0m, DelayDays = 0, PlannedDay = 28, Month = 12, Gmy = "Risk Yönetimi GMY", Unit = "Fraud Tespit ve Önleme Bölüm Bşk." },
                          new { Name = "Bireysel Emeklilik (BES) Aktarım", Type = "Paydaş Proje", Status = "Planlandı", Ext = "", Team = "Takip", Budget = 50m, Actual = 0m, DelayDays = 0, PlannedDay = 12, Month = 11, Gmy = "Bireysel Bankacılık GMY", Unit = "BES ve Sigorta Bölüm Bşk." },
                          new { Name = "Kişisel Finansal Yönetim (PFM)", Type = "Proje", Status = "Planlandı", Ext = "", Team = "Tahsis", Budget = 85m, Actual = 0m, DelayDays = 0, PlannedDay = 22, Month = 12, Gmy = "Dijital Bankacılık GMY", Unit = "PFM Süreçleri Bölüm Bşk." },
                          new { Name = "Pos Cihazı Gün Sonu Otomasyonu", Type = "KG", Status = "Planlandı", Ext = "", Team = "Teminat", Budget = 35m, Actual = 0m, DelayDays = 0, PlannedDay = 14, Month = 10, Gmy = "Üye İşyeri Operasyon GMY", Unit = "POS Sistemleri Bölüm Bşk." },
                          new { Name = "Sigorta Poliçesi Teklif & Onay", Type = "Proje", Status = "Planlandı", Ext = "", Team = "Takip", Budget = 65m, Actual = 0m, DelayDays = 0, PlannedDay = 19, Month = 11, Gmy = "Sigorta Hizmetleri GMY", Unit = "Elementer Sigorta Bölüm Bşk." },
                          new { Name = "Kripto Varlık Beyan & Entegrasyon", Type = "KG", Status = "Planlandı", Ext = "", Team = "Tahsis", Budget = 55m, Actual = 0m, DelayDays = 0, PlannedDay = 30, Month = 12, Gmy = "Yenilikçi Teknolojiler GMY", Unit = "Blockchain Lab Bölüm Bşk." },
                          new { Name = "Şube İçi Nakit Akış Optimalizasyonu", Type = "Paydaş Proje", Status = "Planlandı", Ext = "", Team = "Teminat", Budget = 75m, Actual = 0m, DelayDays = 0, PlannedDay = 16, Month = 10, Gmy = "Nakit Yönetimi GMY", Unit = "Nakit Operasyonları Bölüm Bşk." }
                      }
                }
            };

            int projIndex = 1;
            foreach (var qd in quartersData)
            {
                foreach (var pDef in qd.Projects)
                {
                    if (context.Projects.Any(p => p.ProjectName == pDef.Name))
                        continue;

                    var analystUser = getAnalystUser(projIndex);
                    var devUser1 = getDevUser(projIndex);
                    var devUser2 = getDevUser(projIndex + 1);

                    var analystName = analystUser.FullName;
                    var devNames = $"{devUser1.FullName}, {devUser2.FullName}";
                    var allNames = $"{analystName}, {devNames}";

                    DateTime plannedDate = new DateTime(2026, pDef.Month, Math.Min(pDef.PlannedDay, 28));
                    DateTime? actualDate = string.Equals(pDef.Status, "Tamamlandı", StringComparison.OrdinalIgnoreCase) ? plannedDate.AddDays(pDef.DelayDays) : (DateTime?)null;

                    var newProj = new Project
                    {
                        PergelNo = 1213000 + (qd.Q * 100) + projIndex,
                        ProjectName = pDef.Name,
                        Summary = $"{pDef.Name} için alt geliştirme ve test çalışmaları.",
                        ProjectType = pDef.Type,
                        ProjectStatus = pDef.Status,
                        ExternalCompanyName = string.IsNullOrEmpty(pDef.Ext) ? null : pDef.Ext,
                        Team = pDef.Team,
                        Year = 2026,
                        Quarter = (byte)qd.Q,
                        Gmy = pDef.Gmy,
                        BusinessUnit = pDef.Unit,
                        Stakeholders = "Kredi Risk, BT Altyapı",
                        AssignedAnalystNames = analystName,
                        AssignedDeveloperNames = devNames,
                        AssignedUserNames = allNames,
                        PlannedReleaseDate = plannedDate,
                        ActualReleaseDate = actualDate,
                        TotalManDayBudget = pDef.Budget,
                        ActualManDays = pDef.Actual,
                        CreatedByUserName = "Sistem",
                        CreatedAt = DateTime.Now
                    };

                    context.Projects.Add(newProj);
                    context.SaveChanges();

                    decimal m1Cost = 0m;
                    decimal m2Cost = 0m;
                    decimal m3Cost = 0m;

                    context.ProjectMonthlyCosts.Add(new ProjectMonthlyCost { ProjectId = newProj.Id, UserId = analystUser.Id, Month = (byte)qd.StartMonth, ManDays = 0m });
                    context.ProjectMonthlyCosts.Add(new ProjectMonthlyCost { ProjectId = newProj.Id, UserId = analystUser.Id, Month = (byte)(qd.StartMonth + 1), ManDays = 0m });
                    context.ProjectMonthlyCosts.Add(new ProjectMonthlyCost { ProjectId = newProj.Id, UserId = analystUser.Id, Month = (byte)(qd.StartMonth + 2), ManDays = 0m });
                    context.ProjectAllocations.Add(new ProjectAllocation { ProjectId = newProj.Id, UserId = analystUser.Id, AllocatedManDay = 0m, ActualManDay = 0m });

                    context.ProjectMonthlyCosts.Add(new ProjectMonthlyCost { ProjectId = newProj.Id, UserId = devUser1.Id, Month = (byte)qd.StartMonth, ManDays = 0m });
                    context.ProjectMonthlyCosts.Add(new ProjectMonthlyCost { ProjectId = newProj.Id, UserId = devUser1.Id, Month = (byte)(qd.StartMonth + 1), ManDays = 0m });
                    context.ProjectMonthlyCosts.Add(new ProjectMonthlyCost { ProjectId = newProj.Id, UserId = devUser1.Id, Month = (byte)(qd.StartMonth + 2), ManDays = 0m });
                    context.ProjectAllocations.Add(new ProjectAllocation { ProjectId = newProj.Id, UserId = devUser1.Id, AllocatedManDay = 0m, ActualManDay = 0m });

                    context.ProjectMonthlyCosts.Add(new ProjectMonthlyCost { ProjectId = newProj.Id, UserId = devUser2.Id, Month = (byte)qd.StartMonth, ManDays = 0m });
                    context.ProjectMonthlyCosts.Add(new ProjectMonthlyCost { ProjectId = newProj.Id, UserId = devUser2.Id, Month = (byte)(qd.StartMonth + 1), ManDays = 0m });
                    context.ProjectMonthlyCosts.Add(new ProjectMonthlyCost { ProjectId = newProj.Id, UserId = devUser2.Id, Month = (byte)(qd.StartMonth + 2), ManDays = 0m });
                    context.ProjectAllocations.Add(new ProjectAllocation { ProjectId = newProj.Id, UserId = devUser2.Id, AllocatedManDay = 0m, ActualManDay = 0m });

                    context.SaveChanges();
                    projIndex++;
                }
            }
        }

        public static void NormalizeAllProjectData(AppDbContext context)
        {
            // Kullanıcı tarafından girilen proje verileri ve maliyetleri birebir korunur.
        }
    }
}
