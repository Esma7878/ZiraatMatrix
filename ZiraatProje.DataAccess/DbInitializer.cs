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

            // Always ensure 20 projects per team (60 projects total) are seeded for Q3!
            EnsureMockProjectsSeeded(context);

            if (context.Users.Any())
            {
                return;
            }

            // 1. Seed Teams
            var teams = new Team[]
            {
                new Team { TeamName = "Takip", Color = "#7c3aed" },
                new Team { TeamName = "Tahsis", Color = "#059669" },
                new Team { TeamName = "Teminat", Color = "#2563eb" }
            };
            context.Teams.AddRange(teams);
            context.SaveChanges();

            // 2. Seed Users & Managers
            var users = new User[]
            {
                new User { Name = "Ahmet", Surname = "Yılmaz", Title = "Analist", Team = "Takip", Email = "ahmet.yilmaz@ziraatteknoloji.com", Phone = "+90 (212) 555 0101", Password = "1234", IsAdmin = true },
                new User { Name = "Ali", Surname = "Çelik", Title = "Developer", Team = "Tahsis", Email = "ali.celik@ziraatteknoloji.com", Phone = "+90 (212) 555 0102", Password = "1234", IsAdmin = true },
                new User { Name = "Esma", Surname = "Yılmaz", Title = "Developer", Team = "Takip", Email = "esma.yilmaz@ziraatteknoloji.com", Phone = "+90 (212) 555 0103", Password = "1234", IsAdmin = false },
                new User { Name = "Mehmet", Surname = "Kaya", Title = "Developer", Team = "Tahsis", Email = "mehmet.kaya@ziraatteknoloji.com", Phone = "+90 (212) 555 0104", Password = "1234", IsAdmin = false },
                new User { Name = "Ayşe", Surname = "Demir", Title = "Analist", Team = "Teminat", Email = "ayse.demir@ziraatteknoloji.com", Phone = "+90 (212) 555 0105", Password = "1234", IsAdmin = false },
                new User { Name = "Fatma", Surname = "Şahin", Title = "Developer", Team = "Takip", Email = "fatma.sahin@ziraatteknoloji.com", Phone = "+90 (212) 555 0106", Password = "1234", IsAdmin = false },
                new User { Name = "Zeynep", Surname = "Yıldız", Title = "Analist", Team = "Teminat", Email = "zeynep.yildiz@ziraatteknoloji.com", Phone = "+90 (212) 555 0107", Password = "1234", IsAdmin = false },
                new User { Name = "Mustafa", Surname = "Öztürk", Title = "Developer", Team = "Takip", Email = "mustafa.ozturk@ziraatteknoloji.com", Phone = "+90 (212) 555 0108", Password = "1234", IsAdmin = false },
                new User { Name = "Elif", Surname = "Aydın", Title = "Developer", Team = "Tahsis", Email = "elif.aydin@ziraatteknoloji.com", Phone = "+90 (212) 555 0109", Password = "1234", IsAdmin = false },
                new User { Name = "Ömer", Surname = "Arslan", Title = "Analist", Team = "Teminat", Email = "omer.arslan@ziraatteknoloji.com", Phone = "+90 (212) 555 0110", Password = "1234", IsAdmin = false },
                new User { Name = "Selin", Surname = "Bulut", Title = "Developer", Team = "Takip", Email = "selin.bulut@ziraatteknoloji.com", Phone = "+90 (212) 555 0111", Password = "1234", IsAdmin = false },
                new User { Name = "Can", Surname = "Koç", Title = "Developer", Team = "Tahsis", Email = "can.koc@ziraatteknoloji.com", Phone = "+90 (212) 555 0112", Password = "1234", IsAdmin = false }
            };
            context.Users.AddRange(users);
            context.SaveChanges();

            // 3. Seed Shift Types
            if (!context.ShiftTypes.Any())
            {
                var shiftTypes = new ShiftType[]
                {
                    new ShiftType { ShiftName = "Firewall Geçiş" },
                    new ShiftType { ShiftName = "Server Geçiş" },
                    new ShiftType { ShiftName = "Haftalık Yaygınlaştırma" },
                    new ShiftType { ShiftName = "Aylık Yaygınlaştırma" },
                    new ShiftType { ShiftName = "Acil Güvenlik Yaması" }
                };
                context.ShiftTypes.AddRange(shiftTypes);
                context.SaveChanges();
            }

            // 4. Seed Monthly Release Shifts
            var monthlyShifts = new MonthlyReleaseShift[]
            {
                new MonthlyReleaseShift { MonthName = "Ocak 2026 Yaygınlaştırması", ReleaseDate = new DateTime(2026, 1, 15), AssignedUsers = "Ahmet Yılmaz, Fatma Şahin, Ali Çelik, Elif Aydın", JiraTicketNo = "REL-2026-01", ExternalLink = "https://jira.ziraat.com/browse/REL-2026-01", CreatedByUserName = "Ahmet Yılmaz" },
                new MonthlyReleaseShift { MonthName = "Şubat 2026 Yaygınlaştırması", ReleaseDate = new DateTime(2026, 2, 12), AssignedUsers = "Mehmet Kaya, Ayşe Demir, Mustafa Öztürk, Ömer Arslan", JiraTicketNo = "REL-2026-02", ExternalLink = "https://jira.ziraat.com/browse/REL-2026-02", CreatedByUserName = "Ahmet Yılmaz" },
                new MonthlyReleaseShift { MonthName = "Mart 2026 Yaygınlaştırması", ReleaseDate = new DateTime(2026, 3, 19), AssignedUsers = "Zeynep Yıldız, Selin Bulut, Can Koç, Esma Yılmaz", JiraTicketNo = "REL-2026-03", ExternalLink = "https://jira.ziraat.com/browse/REL-2026-03", CreatedByUserName = "Esma Yılmaz" }
            };
            context.MonthlyReleaseShifts.AddRange(monthlyShifts);
            context.SaveChanges();
        }

        public static void EnsureMockProjectsSeeded(AppDbContext context)
        {
            if (context.Projects.Count() >= 20) return;

            var users = context.Users.ToList();
            if (!users.Any()) return;

            var takipUsers = users.Where(u => u.Team == "Takip").ToList();
            var tahsisUsers = users.Where(u => u.Team == "Tahsis").ToList();
            var teminatUsers = users.Where(u => u.Team == "Teminat").ToList();

            var gmyOptions = new[]
            {
                "Kredi Politikaları ve Risk Tasfiye GMY",
                "Kredi Tahsis ve Yönetimi GMY",
                "Ürün Yönetimi ve Dijital Bankacılık GMY",
                "Strateji Planlama ve İnsan Kaynakları Grup Başkanlığı",
                "Genel Müdürlük"
            };

            var buOptions = new[]
            {
                "Kredi Süreçleri Bölüm Başkanlığı",
                "Kurumsal ve Ticari Krediler Tahsis ve Yönetimi Bölüm Başkanlığı",
                "Kredi Risk İzleme Yapılandırma ve Tasfiye Bölüm Başkanlığı",
                "Finansman Ürünleri Yönetimi Bölüm Başkanlığı",
                "İnşaat ve Gayrimenkul Yönetimi Bölüm Başkanlığı",
                "Ziraat Teknoloji"
            };

            var statusOptions = new[] { "Devam Ediyor", "Planlandı", "Tamamlandı" };
            var typeOptions = new[] { "Proje", "KG", "Paydaş Proje", "Dış Firma" };
            var extCompanies = new[] { "Ziraat Teknoloji A.Ş.", "FinTech Solutions", "SoftTech Yazılım", "Bitis Sistemleri" };
            var stakeholdersList = new[] { "Bireysel Bankacılık, Risk Yönetimi", "Kurumsal Bankacılık, Hukuk", "IT Güvenlik, Operasyon", "Hazine, Finansal Kurumlar" };

            var rand = new Random(42);

            // 1. TAKİP PROJECTS (20 Items)
            var takipProjectNames = new[]
            {
                "Bireysel Kredi Takip ve Tahsilat Modülü",
                "Kanuni Takip Portalı Entegrasyonu",
                "Erken Uyarı Sistemleri (EUS) Risk Takip Engine",
                "Kredi Risk İzleme ve Yapılandırma Otomasyonu",
                "KOBİ Kredileri İzleme Dashboadı",
                "Takip İcra ve Hukuk Süreçleri Servisleri",
                "Ticari Portföy Risk İzleme Modülü",
                "Otomatik İhtarname ve Tebligat Oluşturucu",
                "BDDK Takip Oranları Raporlama Servisi",
                "Gecikmeli Kredi Alacak Tahsilat Uygulaması",
                "Varlık Yönetim Şirketleri Devir Portalı",
                "Takip Süreçleri Yapay Zeka Skorlama Motoru",
                "Kredi Yapılandırma Hesaplama Çerçevesi",
                "Bireysel Kart Takip Servisi",
                "Yeniden Yapılandırma Protokol Otomasyonu",
                "Takip Portföyü Kara Liste & KKB Entegrasyonu",
                "Taahhütlü Alacak Takip Modülü",
                "Borç Yapılandırma Kampanya Yönetimi",
                "Tahsilat Çağrı Merkezi Entegrasyon Servisleri",
                "Kredi Riski Gecikme Bildirim Motoru"
            };

            // 2. TAHSİS PROJECTS (20 Items)
            var tahsisProjectNames = new[]
            {
                "Bireysel Kredi Tahsis Otomasyonu",
                "Kurumsal ve Ticari Krediler Değerlendirme Sistemi",
                "Otomatik Kredi Onay & Karar Destek Motoru",
                "Mikro KOBİ Hızlı Tahsis Ekranları",
                "Kredi Derecelendirme ve Rating Entegrasyonu",
                "Teminatsız Kredi Tahsis Sınırlandırma Servisi",
                "Bilanço Analiz ve Mali Tablo Çözümleme Modülü",
                "Taşıt ve Konut Kredisi Tahsis Servisleri",
                "Tarım Kredileri Özel Tahsis Akışları",
                "E-Devlet Gelir Doğrulama & Tahsis Entegrasyonu",
                "Kredi Limiti Tahsis ve Risk Grubu Yönetimi",
                "Ticari Kredi Tahsis Komite Ekranı",
                "Vergi Dairesi E-Beyanname Tahsis Servisi",
                "Kredi Kartı Limit Tahsis Otomasyonu",
                "Proje Finansmanı Tahsis ve Uygunluk Modülü",
                "Faktoring & Forfaiting Tahsis Akışı",
                "Katılım Bankacılığı Murabaha Tahsis Servisi",
                "Otomatik Kredi Skor Kart Dönüşüm Engine",
                "Kredi Limit Revizyon ve Yenileme Otomasyonu",
                "Grup Şirketleri Kredi Tahsis Limit Kontrolü"
            };

            // 3. TEMİNAT PROJECTS (20 Items)
            var teminatProjectNames = new[]
            {
                "Gayrimenkul Ekspertiz ve Teminat Otomasyonu",
                "Teminat Mektubu ve Akreditif Yönetim Sistemi",
                "İpotek Tescil ve Tapu Kadastro Entegrasyonu",
                "Araç Rehnü ve E-Ekspertiz Servisleri",
                "Mevduat ve Menkul Rehnü Yönetim Modülü",
                "Kredi Teminat Oranı (LTV) Hesaplama Motoru",
                "Teminat Riski ve Sigorta Takip Servisi",
                "Hazine Bonosu & Devlet Tahvili Teminat Portalı",
                "Şartlı Teminat & Müşterek İpotek Ekranları",
                "Kredi Teminat Serbest Bırakma Otomasyonu",
                "KGF (Kredi Garanti Fonu) Teminat Entegrasyonu",
                "Fiziki Teminat Depolama ve Kasalama Servisi",
                "Stok & Emtia Rehnü Değerleme Modülü",
                "Teminat Mektubu Doğrulama & QR Kod Engine",
                "Ticari İşletme Rehnü Yönetim Portalı",
                "Ekspertiz Şirketleri Web Servis Entegrasyonu",
                "Dövizli Teminat Kur Farkı Güncelleme Servisi",
                "Teminat Sigorta Poliçe Yenileme Otomasyonu",
                "Keşideci & Çek/Senet Teminat Yönetim Ekranı",
                "Müşterek ve Müteselsil Kefalet Takip Modülü"
            };

            var teamProjectBatches = new (string TeamName, string[] Names, List<User> TeamUsers)[]
            {
                ("Takip", takipProjectNames, takipUsers),
                ("Tahsis", tahsisProjectNames, tahsisUsers),
                ("Teminat", teminatProjectNames, teminatUsers)
            };

            int pergelCounter = 1045000;

            foreach (var batch in teamProjectBatches)
            {
                var teamUsrs = batch.TeamUsers.Count > 0 ? batch.TeamUsers : users;
                var analysts = teamUsrs.Where(u => u.Title == "Analist").ToList();
                var devs = teamUsrs.Where(u => u.Title == "Developer" || u.Title == "Yazılımcı").ToList();

                if (!analysts.Any()) analysts = teamUsrs.Take(1).ToList();
                if (!devs.Any()) devs = teamUsrs.Skip(1).ToList();

                foreach (var pName in batch.Names)
                {
                    pergelCounter += rand.Next(1, 15);
                    string pType = typeOptions[rand.Next(typeOptions.Length)];
                    string extComp = pType == "Dış Firma" ? extCompanies[rand.Next(extCompanies.Length)] : "";
                    decimal extCost = pType == "Dış Firma" ? (decimal)rand.Next(30, 250) : 0m;

                    var selAnalyst = analysts[rand.Next(analysts.Count)];
                    var selDev = devs[rand.Next(devs.Count)];

                    var project = new Project
                    {
                        PergelNo = pergelCounter,
                        ProjectName = pName,
                        Summary = pName,
                        ProjectStatus = statusOptions[rand.Next(statusOptions.Length)],
                        ProjectType = pType,
                        ExternalCompanyName = extComp,
                        ExternalCost = extCost,
                        Team = batch.TeamName,
                        Year = 2026,
                        Quarter = 3,
                        Gmy = gmyOptions[rand.Next(gmyOptions.Length)],
                        BusinessUnit = buOptions[rand.Next(buOptions.Length)],
                        Stakeholders = stakeholdersList[rand.Next(stakeholdersList.Length)],
                        Description = $"{pName} projesi kapsamındaki 2026 Q3 çeyreklik canlı geçiş ve analiz hedefleri.",
                        AssignedAnalystNames = selAnalyst.FullName,
                        AssignedDeveloperNames = selDev.FullName,
                        AssignedUserNames = $"{selAnalyst.FullName} | {selDev.FullName}",
                        AssignedUserIds = $"{selAnalyst.Id},{selDev.Id}"
                    };

                    context.Projects.Add(project);
                    context.SaveChanges();

                    // Seed monthly costs (Months 7, 8, 9 for Q3)
                    decimal m7_1 = rand.Next(5, 20);
                    decimal m8_1 = rand.Next(5, 20);
                    decimal m9_1 = rand.Next(5, 20);

                    decimal m7_2 = rand.Next(8, 22);
                    decimal m8_2 = rand.Next(8, 22);
                    decimal m9_2 = rand.Next(8, 22);

                    context.ProjectMonthlyCosts.Add(new ProjectMonthlyCost { ProjectId = project.Id, UserId = selAnalyst.Id, Month = 7, ManDays = m7_1 });
                    context.ProjectMonthlyCosts.Add(new ProjectMonthlyCost { ProjectId = project.Id, UserId = selAnalyst.Id, Month = 8, ManDays = m8_1 });
                    context.ProjectMonthlyCosts.Add(new ProjectMonthlyCost { ProjectId = project.Id, UserId = selAnalyst.Id, Month = 9, ManDays = m9_1 });

                    context.ProjectMonthlyCosts.Add(new ProjectMonthlyCost { ProjectId = project.Id, UserId = selDev.Id, Month = 7, ManDays = m7_2 });
                    context.ProjectMonthlyCosts.Add(new ProjectMonthlyCost { ProjectId = project.Id, UserId = selDev.Id, Month = 8, ManDays = m8_2 });
                    context.ProjectMonthlyCosts.Add(new ProjectMonthlyCost { ProjectId = project.Id, UserId = selDev.Id, Month = 9, ManDays = m9_2 });

                    project.TotalManDayBudget = m7_1 + m8_1 + m9_1 + m7_2 + m8_2 + m9_2;
                }
            }

            context.SaveChanges();
        }
    }
}
