using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZiraatProje.DataAccess
{
    public class Team
    {
        public int Id { get; set; }
        public string TeamName { get; set; } = string.Empty;
        public string Color { get; set; } = "#7c3aed";
    }

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty; // Developer, Analist, Yönetici
        public string Team { get; set; } = string.Empty;  // Takip, Tahsis, Finansman
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; } = string.Empty;
        public string Password { get; set; } = "1234";
        public bool IsAdmin { get; set; }

        // Navigation properties
        public virtual ICollection<Shift> Shifts { get; set; } = new List<Shift>();
        public virtual ICollection<Leave> Leaves { get; set; } = new List<Leave>();
        public virtual ICollection<ProjectAllocation> ProjectAllocations { get; set; } = new List<ProjectAllocation>();

        public string FullName => $"{Name} {Surname}";

        [NotMapped]
        public string PhoneFormatted { get => !string.IsNullOrWhiteSpace(Phone) ? Phone : $"+90 (212) 555 01{(Id > 0 ? (100 + Id) : 101)}"; set { } }

        [NotMapped]
        public string DisplayTeamName { get => (Team != null && (Team.Equals("Departman Yönetimi", StringComparison.OrdinalIgnoreCase) || Team.Equals("Yönetim", StringComparison.OrdinalIgnoreCase))) ? "Departman Yöneticisi" : (Team ?? string.Empty); set { } }

        [NotMapped]
        public bool HasTeam { get => !string.IsNullOrWhiteSpace(Team) && !string.Equals(Team, "Departman Yönetimi", StringComparison.OrdinalIgnoreCase) && !string.Equals(Team, "Yönetim", StringComparison.OrdinalIgnoreCase); set { } }

        [NotMapped]
        public string SubTitleDotTeam { get => HasTeam ? $" • {Team} Ekibi" : string.Empty; set { } }
    }

    public class ShiftType
    {
        public int Id { get; set; }
        public string ShiftName { get; set; } = string.Empty; // Firewall Geçiş, Server Geçiş, etc.

        // Navigation properties
        public virtual ICollection<Shift> Shifts { get; set; } = new List<Shift>();
    }

    public class Shift
    {
        public int Id { get; set; }
        public int ShiftTypeId { get; set; }
        public int UserId { get; set; }
        public DateTime ShiftDate { get; set; }
        public string JiraTicketNo { get; set; } = string.Empty;
        public string ExternalLink { get; set; } = string.Empty;

        // Navigation properties
        public virtual ShiftType? ShiftType { get; set; }
        public virtual User? User { get; set; }
    }

    public class Leave
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public short Year { get; set; }
        public byte Quarter { get; set; } // 1: Jan-Mar, 2: Apr-Jun, 3: Jul-Sep, 4: Oct-Dec

        public string Status { get; set; } = "Approved"; // "Approved" (Onaylandı), "Pending" (Onay Bekliyor), "Rejected" (Reddedildi)
        public bool IsSpecialRequest { get; set; } = false; // %50 rol çakışması durumunda Özel İzin talebi mi?
        public string? RequestNote { get; set; } = string.Empty;
        public DateTime? RequestedAt { get; set; } = DateTime.Now;
        public DateTime? ApprovedAt { get; set; }
        public string? ApprovedByUserName { get; set; }
        public DateTime? PreviousStartDate { get; set; }
        public DateTime? PreviousEndDate { get; set; }
        public bool IsNotificationSeen { get; set; } = true; // false = user has unseen approve/reject notification

        // Navigation properties
        public virtual User? User { get; set; }

        // Helper properties for UI display
        public bool IsHourly => (StartDate.TimeOfDay != TimeSpan.Zero || EndDate.TimeOfDay != TimeSpan.Zero);
        public string LeaveTypeDisplay => IsHourly ? "⏰ Saatlik İzin" : "📅 Tam Gün";
        public string FormattedDateRange => IsHourly
            ? $"{StartDate:dd.MM.yyyy} ({StartDate:HH:mm} - {EndDate:HH:mm})"
            : (StartDate.Date == EndDate.Date ? $"{StartDate:dd.MM.yyyy}" : $"{StartDate:dd.MM.yyyy} - {EndDate:dd.MM.yyyy}");
        public string FormattedTimeRange => IsHourly ? $"{StartDate:HH:mm} - {EndDate:HH:mm}" : "Tam Gün";
    }

    public class Project
    {
        public int Id { get; set; }
        public long PergelNo { get; set; } // 6-7 haneli pergel no
        public string ProjectName { get; set; } = string.Empty; // Talep Özeti / Adı
        public string Summary { get; set; } = string.Empty;
        public string ProjectStatus { get; set; } = "Planlandı"; // Planlandı, Devam Ediyor, Tamamlandı, İptal
        public string ProjectType { get; set; } = "Proje"; // Proje, KG, Paydaş Proje, Dış Firma
        public string? ExternalCompanyName { get; set; } = string.Empty; // Dış Firma türünde özel isim
        public decimal ExternalCost { get; set; } // Dış maliyet
        public string Team { get; set; } = string.Empty; // Takip, Tahsis, Teminat
        public short Year { get; set; } = 2026;
        public byte Quarter { get; set; } = 3; // 1, 2, 3, 4
        public string? Gmy { get; set; } = string.Empty; // GMY (Genel Müdür Yardımcılığı)
        public string? BusinessUnit { get; set; } = string.Empty; // İş Birimi (Bölüm Başkanlığı)
        public string? Stakeholders { get; set; } = string.Empty; // Paydaş departmanlar
        public string? Description { get; set; } = string.Empty; // Genel Açıklama / Notlar
        public string AssignedUserNames { get; set; } = string.Empty; // Tüm Atananlar
        public string AssignedAnalystNames { get; set; } = string.Empty; // Analistler ("Herkes" veya isimler)
        public string AssignedDeveloperNames { get; set; } = string.Empty; // Yazılımcılar ("Herkes" veya isimler)
        public string? AssignedUserIds { get; set; } = string.Empty;

        public decimal TotalManDayBudget { get; set; }
        public decimal ActualManDays { get; set; } = 0m; // Gerçekleşen efor (adam/gün)
        public DateTime StartDate { get; set; } = DateTime.Today;
        public DateTime EndDate { get; set; } = DateTime.Today.AddMonths(3);

        public string? CreatedByUserName { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public string? UpdatedByUserName { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? CompletedByUserName { get; set; }
        public DateTime? CompletedAt { get; set; }

        // Navigation properties
        public virtual ICollection<ProjectAllocation> ProjectAllocations { get; set; } = new List<ProjectAllocation>();
        public virtual ICollection<ProjectMonthlyCost> ProjectMonthlyCosts { get; set; } = new List<ProjectMonthlyCost>();
    }

    public class ProjectMonthlyCost
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int UserId { get; set; }
        public int Month { get; set; } // 1 - 12 (Ay numarası)
        public decimal ManDays { get; set; } // O ayki adam/gün maliyeti

        // Navigation properties
        public virtual Project? Project { get; set; }
        public virtual User? User { get; set; }
    }

    public class ProjectAllocation
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int UserId { get; set; }
        public decimal AllocatedManDay { get; set; }
        public decimal ActualManDay { get; set; } = 0m; // Kişinin gerçekleşen harcanan eforu (gün)

        // Navigation properties
        public virtual Project? Project { get; set; }
        public virtual User? User { get; set; }
    }

    public class ChatGroup
    {
        public int Id { get; set; }
        public string GroupName { get; set; } = string.Empty;
        public string MemberUserIds { get; set; } = string.Empty; // Comma-separated user IDs
        public int CreatedByUserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

    public class ChatMessage
    {
        public int Id { get; set; }
        public int SenderUserId { get; set; }
        public int? ReceiverUserId { get; set; } // Null if Team or General channel
        public string TargetType { get; set; } = "Direct"; // "Direct", "Team", "General"
        public string TargetTeam { get; set; } = string.Empty; // "Takip", "Tahsis", "Teminat", "Genel"
        public string MessageText { get; set; } = string.Empty;
        public DateTime SentAt { get; set; } = DateTime.Now;
        public bool IsRead { get; set; } = false;

        public virtual User? SenderUser { get; set; }
        public virtual User? ReceiverUser { get; set; }
    }

    public class ChatMessageReadState
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string ChannelKey { get; set; } = string.Empty;
        public int LastReadMessageId { get; set; }
        public DateTime LastReadAt { get; set; } = DateTime.Now;
    }

    public class MonthlyReleaseShift
    {
        public int Id { get; set; }
        public string MonthName { get; set; } = string.Empty;
        public DateTime ReleaseDate { get; set; }
        public string AssignedUsers { get; set; } = string.Empty;
        public string? JiraTicketNo { get; set; } = string.Empty;
        public string? ExternalLink { get; set; } = string.Empty;
        public string? Note { get; set; } = string.Empty;
        public string? CreatedByUserName { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public string? UpdatedByUserName { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsFinished { get; set; } = false;
    }

    public class CustomShift
    {
        public int Id { get; set; }
        public string Topic { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public string AssignedUsers { get; set; } = string.Empty;
        public DateTime ShiftDate { get; set; }
        public string? ExternalLink { get; set; } = string.Empty;
        public string? CreatedByUserName { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public string? UpdatedByUserName { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsFinished { get; set; } = false;
    }
}
