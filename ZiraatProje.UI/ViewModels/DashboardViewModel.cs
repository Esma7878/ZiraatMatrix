using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using ZiraatProje.Business;
using ZiraatProje.DataAccess;

namespace ZiraatProje.UI.ViewModels
{
    public class TodayShiftItem
    {
        public string PersonnelName { get; set; } = string.Empty;
        public string ShiftTitle { get; set; } = string.Empty;
        public string Team { get; set; } = string.Empty;
        public string JiraTicketNo { get; set; } = string.Empty;
        public bool HasJira => !string.IsNullOrWhiteSpace(JiraTicketNo);

        public string TeamBadgeColor => Team switch
        {
            "Takip" => "#5b21b6",
            "Tahsis" => "#065f46",
            "Teminat" => "#1e40af",
            _ => "#bc171d"
        };

        public string TeamBadgeBg => Team switch
        {
            "Takip" => "#ede9fe",
            "Tahsis" => "#d1fae5",
            "Teminat" => "#dbeafe",
            _ => "#fee2e2"
        };
    }

    public class TodayLeaveItem
    {
        public string PersonnelName { get; set; } = string.Empty;
        public string Team { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string DateRangeText => $"{StartDate:dd.MM.yyyy} - {EndDate:dd.MM.yyyy}";

        public string TeamBadgeColor => Team switch
        {
            "Takip" => "#5b21b6",
            "Tahsis" => "#065f46",
            "Teminat" => "#1e40af",
            _ => "#bc171d"
        };

        public string TeamBadgeBg => Team switch
        {
            "Takip" => "#ede9fe",
            "Tahsis" => "#d1fae5",
            "Teminat" => "#dbeafe",
            _ => "#fee2e2"
        };
    }

    public class ShiftConflictNotificationModel
    {
        public int ShiftId { get; set; }
        public string ShiftCategory { get; set; } = "MonthlyRelease";
        public string PersonnelName { get; set; } = string.Empty;
        public string LeaveDatesText { get; set; } = string.Empty;
        public string ShiftTitleText { get; set; } = string.Empty;
        public string Title => "🚨 KRİTİK NÖBET-İZİN ÇAKIŞMASI TESPİT EDİLDİ!";
        public string Message => $"'{PersonnelName}' isimli personel {LeaveDatesText} tarihleri arasında İZİNLİDİR! Ancak bu tarihlerde nöbetçi ({ShiftTitleText}) olarak kayıtlı görünmektedir. Düzeltmek için tıklayınız.";
    }

    public class UpcomingReleaseItem
    {
        public DateTime ReleaseDate { get; set; }
        public string ReleaseName { get; set; } = string.Empty;
        public string AssignedUsers { get; set; } = string.Empty;
        public string JiraTicketNo { get; set; } = string.Empty;
        public string DateText => ReleaseDate.ToString("dd MMMM yyyy (dddd)");
    }

    public class TeamSummaryItem
    {
        public string TeamName { get; set; } = string.Empty;
        public int TotalMemberCount { get; set; }
        public int ActiveLeaveCount { get; set; }
        public int OnDutyCount { get; set; }
        public string BadgeColor { get; set; } = "#bc171d";
        public string BadgeBg { get; set; } = "#fee2e2";
    }

    public class DashboardViewModel : BaseViewModel
    {
        private readonly BusinessServices _services = new BusinessServices();

        private bool _hasShiftNotification;
        public bool HasShiftNotification
        {
            get => _hasShiftNotification;
            set
            {
                _hasShiftNotification = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ShiftNotificationVisibility));
            }
        }

        public Visibility ShiftNotificationVisibility => HasShiftNotification ? Visibility.Visible : Visibility.Collapsed;

        private string _shiftNotificationText = string.Empty;
        public string ShiftNotificationText
        {
            get => _shiftNotificationText;
            set { _shiftNotificationText = value; OnPropertyChanged(); }
        }

        private int _weeklyShiftCount;
        public int WeeklyShiftCount
        {
            get => _weeklyShiftCount;
            set { _weeklyShiftCount = value; OnPropertyChanged(); }
        }

        private int _activeLeaveCount;
        public int ActiveLeaveCount
        {
            get => _activeLeaveCount;
            set { _activeLeaveCount = value; OnPropertyChanged(); }
        }

        private int _ongoingProjectsCount;
        public int OngoingProjectsCount
        {
            get => _ongoingProjectsCount;
            set { _ongoingProjectsCount = value; OnPropertyChanged(); }
        }

        private int _criticalBudgetProjectsCount;
        public int CriticalBudgetProjectsCount
        {
            get => _criticalBudgetProjectsCount;
            set { _criticalBudgetProjectsCount = value; OnPropertyChanged(); }
        }

        private int _totalProjectsCount;
        public int TotalProjectsCount
        {
            get => _totalProjectsCount;
            set { _totalProjectsCount = value; OnPropertyChanged(); }
        }

        private string _currentQuarterText = "Q3 (Temmuz-Eylül)";
        public string CurrentQuarterText
        {
            get => _currentQuarterText;
            set { _currentQuarterText = value; OnPropertyChanged(); }
        }

        // ── Personal Profile & Team Properties ─────────────────────────────────────
        private string _userFullName = string.Empty;
        public string UserFullName
        {
            get => _userFullName;
            set { _userFullName = value; OnPropertyChanged(); }
        }

        private string _userTitle = string.Empty;
        public string UserTitle
        {
            get => _userTitle;
            set { _userTitle = value; OnPropertyChanged(); }
        }

        private string _userTeam = string.Empty;
        public string UserTeam
        {
            get => _userTeam;
            set
            {
                _userTeam = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(UserTeamBadgeColor));
                OnPropertyChanged(nameof(UserTeamBadgeBg));
            }
        }

        public string UserTeamBadgeColor => UserTeam switch
        {
            "Takip" => "#5b21b6",
            "Tahsis" => "#065f46",
            "Teminat" => "#1e40af",
            "Departman Yönetimi" => "#991b1b",
            "Yönetim" => "#991b1b",
            _ => "#bc171d"
        };

        public string UserTeamBadgeBg => UserTeam switch
        {
            "Takip" => "#ede9fe",
            "Tahsis" => "#d1fae5",
            "Teminat" => "#dbeafe",
            "Departman Yönetimi" => "#ffeef0",
            "Yönetim" => "#ffeef0",
            _ => "#fee2e2"
        };

        private string _myNextShiftText = "📌 Kayıtlı nöbetiniz yok";
        public string MyNextShiftText
        {
            get => _myNextShiftText;
            set { _myNextShiftText = value; OnPropertyChanged(); }
        }

        private string _myNextLeaveText = "🌴 Planlanmış izniniz yok";
        public string MyNextLeaveText
        {
            get => _myNextLeaveText;
            set { _myNextLeaveText = value; OnPropertyChanged(); }
        }

        private string _myTeammatesText = string.Empty;
        public string MyTeammatesText
        {
            get => _myTeammatesText;
            set { _myTeammatesText = value; OnPropertyChanged(); }
        }

        // ── Leave Notifications & Navigation Properties ─────────────────────────
        private Action? _onNavigateToLeaves;
        private Action? _onNavigateToShifts;
        private Action<string?>? _onNavigateToProjects;
        private Action? _onNavigateToUsers;
        private Action? _onCreateLeaveRequest;
        private Action? _onCreateShift;
        private Action? _onCreateProject;
        private Action? _onCreateUser;

        private ObservableCollection<TodayShiftItem> _todayShiftList = new ObservableCollection<TodayShiftItem>();
        public ObservableCollection<TodayShiftItem> TodayShiftList
        {
            get => _todayShiftList;
            set { _todayShiftList = value; OnPropertyChanged(); OnPropertyChanged(nameof(HasTodayShifts)); }
        }
        public bool HasTodayShifts => TodayShiftList != null && TodayShiftList.Count > 0;

        private ObservableCollection<TodayLeaveItem> _todayLeaveList = new ObservableCollection<TodayLeaveItem>();
        public ObservableCollection<TodayLeaveItem> TodayLeaveList
        {
            get => _todayLeaveList;
            set { _todayLeaveList = value; OnPropertyChanged(); OnPropertyChanged(nameof(HasTodayLeaves)); }
        }
        public bool HasTodayLeaves => TodayLeaveList != null && TodayLeaveList.Count > 0;

        private ObservableCollection<UpcomingReleaseItem> _upcomingReleaseList = new ObservableCollection<UpcomingReleaseItem>();
        public ObservableCollection<UpcomingReleaseItem> UpcomingReleaseList
        {
            get => _upcomingReleaseList;
            set { _upcomingReleaseList = value; OnPropertyChanged(); }
        }

        private ExecutiveDigestReport? _aiExecutiveReport;
        public ExecutiveDigestReport? AiExecutiveReport
        {
            get => _aiExecutiveReport;
            set
            {
                _aiExecutiveReport = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasAiExecutiveReport));
            }
        }

        public bool HasAiExecutiveReport => AiExecutiveReport != null;

        private ObservableCollection<TeamSummaryItem> _teamSummaryList = new ObservableCollection<TeamSummaryItem>();
        public ObservableCollection<TeamSummaryItem> TeamSummaryList
        {
            get => _teamSummaryList;
            set { _teamSummaryList = value; OnPropertyChanged(); }
        }

        private ObservableCollection<NotificationItemModel> _notificationList = new ObservableCollection<NotificationItemModel>();
        public ObservableCollection<NotificationItemModel> NotificationList
        {
            get => _notificationList;
            set
            {
                _notificationList = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(NotificationVisible));
            }
        }

        private Action<int, string>? _onNavigateToShiftConflict;

        private ObservableCollection<ShiftConflictNotificationModel> _shiftConflictList = new ObservableCollection<ShiftConflictNotificationModel>();
        public ObservableCollection<ShiftConflictNotificationModel> ShiftConflictList
        {
            get => _shiftConflictList;
            set
            {
                _shiftConflictList = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasShiftConflicts));
            }
        }
        public bool HasShiftConflicts => ShiftConflictList != null && ShiftConflictList.Count > 0;

        public Visibility NotificationVisible =>
            (NotificationList != null && NotificationList.Count > 0) ? Visibility.Visible : Visibility.Collapsed;

        private int _pendingApprovalCount;
        public int PendingApprovalCount
        {
            get => _pendingApprovalCount;
            set
            {
                _pendingApprovalCount = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(PendingApprovalNotificationVisibility));
                OnPropertyChanged(nameof(PendingApprovalNotificationText));
            }
        }

        private bool _isAdmin;
        public bool IsAdmin
        {
            get => _isAdmin;
            set
            {
                _isAdmin = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(PendingApprovalNotificationVisibility));
                OnPropertyChanged(nameof(ProjectShortcutTitle));
                OnPropertyChanged(nameof(ProjectShortcutSubtitle));
                OnPropertyChanged(nameof(ProjectShortcutIcon));
                OnPropertyChanged(nameof(UserShortcutTitle));
                OnPropertyChanged(nameof(UserShortcutSubtitle));
                OnPropertyChanged(nameof(UserShortcutIcon));
            }
        }

        public string ProjectShortcutTitle => IsAdmin ? "Proje Kaydı Ekle" : "Projeleri Görüntüle";
        public string ProjectShortcutSubtitle => IsAdmin ? "Yeni proje tanımla" : "Proje listesini incele";
        public string ProjectShortcutIcon => IsAdmin ? "\uE710 \uE9D2" : "\uE9D2";

        public string UserShortcutTitle => IsAdmin ? "Personel / Ekip Tanımla" : "Kullanıcıları Görüntüle";
        public string UserShortcutSubtitle => IsAdmin ? "Personel ve rol yetkileri" : "Personel ve ekip listesi";
        public string UserShortcutIcon => IsAdmin ? "\uE710 \uE716" : "\uE716";

        public Visibility PendingApprovalNotificationVisibility =>
            (IsAdmin && PendingApprovalCount > 0) ? Visibility.Visible : Visibility.Collapsed;

        public string PendingApprovalNotificationText =>
            $"⏳ {PendingApprovalCount} adet onay bekleyen izin talebi bulunmaktadır. Onaylamak için tıklayınız.";

        public System.Windows.Input.ICommand NavigateToLeavesCommand { get; }
        public System.Windows.Input.ICommand NavigateToShiftsCommand { get; }
        public System.Windows.Input.ICommand NavigateToProjectsCommand { get; }
        public System.Windows.Input.ICommand NavigateToUsersCommand { get; }
        public System.Windows.Input.ICommand NavigateToConflictingShiftCommand { get; }
        public System.Windows.Input.ICommand CreateLeaveRequestCommand { get; }
        public System.Windows.Input.ICommand CreateShiftCommand { get; }
        public System.Windows.Input.ICommand CreateProjectCommand { get; }
        public System.Windows.Input.ICommand CreateUserCommand { get; }
        public System.Windows.Input.ICommand DismissNotificationCommand { get; }

        public DashboardViewModel() : this(false, string.Empty, null, null, null, null, null, null, null, null, null) { }

        public DashboardViewModel(
            bool isAdmin, 
            string currentUserName = "", 
            Action? onNavigateToLeaves = null,
            Action? onNavigateToShifts = null,
            Action<string?>? onNavigateToProjects = null,
            Action? onNavigateToUsers = null,
            Action? onCreateLeaveRequest = null,
            Action? onCreateShift = null,
            Action? onCreateProject = null,
            Action? onCreateUser = null,
            Action<int, string>? onNavigateToShiftConflict = null)
        {
            IsAdmin = isAdmin;
            _onNavigateToLeaves = onNavigateToLeaves;
            _onNavigateToShifts = onNavigateToShifts;
            _onNavigateToProjects = onNavigateToProjects;
            _onNavigateToUsers = onNavigateToUsers;
            _onCreateLeaveRequest = onCreateLeaveRequest;
            _onCreateShift = onCreateShift;
            _onCreateProject = onCreateProject;
            _onCreateUser = onCreateUser;
            _onNavigateToShiftConflict = onNavigateToShiftConflict;

            NavigateToLeavesCommand = new RelayCommand(_ => _onNavigateToLeaves?.Invoke());
            NavigateToShiftsCommand = new RelayCommand(_ => _onNavigateToShifts?.Invoke());
            NavigateToProjectsCommand = new RelayCommand(param => _onNavigateToProjects?.Invoke(param as string));
            NavigateToUsersCommand = new RelayCommand(_ => _onNavigateToUsers?.Invoke());
            NavigateToConflictingShiftCommand = new RelayCommand(param =>
            {
                if (param is ShiftConflictNotificationModel conflict)
                {
                    _onNavigateToShiftConflict?.Invoke(conflict.ShiftId, conflict.ShiftCategory);
                }
                else if (_onNavigateToShifts != null)
                {
                    _onNavigateToShifts.Invoke();
                }
            });

            CreateLeaveRequestCommand = new RelayCommand(_ => _onCreateLeaveRequest?.Invoke());
            CreateShiftCommand = new RelayCommand(_ => _onCreateShift?.Invoke());
            CreateProjectCommand = new RelayCommand(_ =>
            {
                if (IsAdmin) _onCreateProject?.Invoke();
                else _onNavigateToProjects?.Invoke(null);
            });
            CreateUserCommand = new RelayCommand(_ =>
            {
                if (IsAdmin) _onCreateUser?.Invoke();
                else _onNavigateToUsers?.Invoke();
            });

            DismissNotificationCommand = new RelayCommand(param =>
            {
                if (param is NotificationItemModel item)
                {
                    NotificationList.Remove(item);
                    OnPropertyChanged(nameof(NotificationVisible));
                }
                else
                {
                    NotificationList.Clear();
                    OnPropertyChanged(nameof(NotificationVisible));
                }
                _services.MarkNotificationsSeenForUser(currentUserName);
            });

            RefreshDashboard(currentUserName);
        }

        public void RefreshDashboard(string currentUserName = "")
        {
            try
            {
                var today = DateTime.Today;

                // 1. Weekly Shift Count (Shifts in current calendar week - Monday to Sunday)
                int diff = (7 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;
                var startOfWeek = today.AddDays(-1 * diff).Date;
                var endOfWeek = startOfWeek.AddDays(7).Date;

                var shifts = _services.GetAllShifts();
                var monthlyReleasesAll = _services.GetAllMonthlyReleaseShifts().Where(m => !m.IsFinished).ToList();
                int releaseWeeklyPersonnelCount = monthlyReleasesAll
                    .Where(m => (m.ReleaseDate.Date >= startOfWeek && m.ReleaseDate.Date < endOfWeek) || (!string.IsNullOrWhiteSpace(m.MonthName) && m.MonthName.Contains("Haftalık")))
                    .SelectMany(m => m.AssignedUsers.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries))
                    .Select(n => n.Contains(':') ? n.Substring(n.IndexOf(':') + 1).Trim() : n.Trim())
                    .Where(n => !string.IsNullOrWhiteSpace(n))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .Count();

                WeeklyShiftCount = Math.Max(shifts.Count(s => s.ShiftDate.Date >= startOfWeek && s.ShiftDate.Date < endOfWeek), releaseWeeklyPersonnelCount);

                // 2. Active Leave Count (Approved leaves active today)
                var leaves = _services.GetAllLeaves() ?? new List<Leave>();
                ActiveLeaveCount = leaves.Count(l =>
                    (string.IsNullOrEmpty(l.Status) || 
                     string.Equals(l.Status, "Approved", StringComparison.OrdinalIgnoreCase) || 
                     string.Equals(l.Status, "Onaylandı", StringComparison.OrdinalIgnoreCase)) &&
                    !string.Equals(l.Status, "Pending", StringComparison.OrdinalIgnoreCase) &&
                    !string.Equals(l.Status, "Rejected", StringComparison.OrdinalIgnoreCase) &&
                    !string.Equals(l.Status, "Cancelled", StringComparison.OrdinalIgnoreCase) &&
                    today >= l.StartDate.Date && today <= l.EndDate.Date
                );

                // 3. Ongoing Projects for Current Quarter
                int currentMonth = today.Month;
                byte currentQuarter = (byte)((currentMonth - 1) / 3 + 1);
                short currentYear = (short)today.Year;

                CurrentQuarterText = currentQuarter switch
                {
                    1 => "Q1 (Ocak-Mart)",
                    2 => "Q2 (Nisan-Haziran)",
                    3 => "Q3 (Temmuz-Eylül)",
                    4 => "Q4 (Ekim-Aralık)",
                    _ => $"Q{currentQuarter}"
                };

                var allProjects = _services.GetAllProjects() ?? new List<Project>();

                // Filter for Current Quarter
                var currentQuarterProjects = allProjects.Where(p => 
                    p.Quarter == currentQuarter && (p.Year == 0 || p.Year == currentYear)
                ).ToList();

                if (!currentQuarterProjects.Any())
                {
                    currentQuarterProjects = allProjects.Where(p => p.Quarter == currentQuarter).ToList();
                }

                // Filter Ongoing (Active) Projects for this Quarter (Not Completed, Not Cancelled)
                var ongoingCurrentQuarterProjects = currentQuarterProjects.Where(p => 
                    p.ProjectStatus != "Tamamlandı" && p.ProjectStatus != "İptal"
                ).ToList();

                // Devam eden projeler: Sadece "Devam Ediyor" durumundaki aktif projeler
                TotalProjectsCount = currentQuarterProjects.Count(p => string.Equals(p.ProjectStatus, "Devam Ediyor", StringComparison.OrdinalIgnoreCase));
                // Proje talepleri: Aktif iş süreçleri (Tamamlanmamış ve iptal edilmemiş tüm talepler)
                OngoingProjectsCount = ongoingCurrentQuarterProjects.Count;

                // 4. Critical Budget Projects (Allocations >= 85% of TotalManDayBudget)
                int criticalCount = 0;
                foreach (var proj in allProjects)
                {
                    decimal totalAllocated = proj.ProjectAllocations.Sum(a => a.AllocatedManDay);
                    if (proj.TotalManDayBudget > 0 && (totalAllocated / proj.TotalManDayBudget) >= 0.85m)
                    {
                        criticalCount++;
                    }
                }
                CriticalBudgetProjectsCount = criticalCount;

                // 5. Shift Notifications for Logged-In User (TODAY, TOMORROW & UPCOMING)
                CheckShiftNotifications(currentUserName);

                // 6. User Leave Result Notifications (Unseen, max 30 days old)
                if (!string.IsNullOrWhiteSpace(currentUserName))
                {
                    var unseenNotifications = _services.GetUnseenNotificationsForUser(currentUserName);
                    var list = new ObservableCollection<NotificationItemModel>();

                    foreach (var n in unseenNotifications)
                    {
                        string dateRange = $"{n.StartDate:dd.MM.yyyy} - {n.EndDate:dd.MM.yyyy}";
                        string adminWho = !string.IsNullOrWhiteSpace(n.ApprovedByUserName) ? n.ApprovedByUserName : "Yönetici";

                        if (string.Equals(n.Status, "Cancelled", StringComparison.OrdinalIgnoreCase))
                        {
                            list.Add(new NotificationItemModel
                            {
                                LeaveId = n.Id,
                                Type = "Cancelled",
                                Icon = "⚠️",
                                Title = "İZİN İPTAL BİLDİRİMİ",
                                Message = $"{dateRange} tarihli onaylı izniniz yöneticiniz ({adminWho}) tarafından İPTAL EDİLDİ!"
                            });
                        }
                        else if (string.Equals(n.Status, "Rejected", StringComparison.OrdinalIgnoreCase))
                        {
                            list.Add(new NotificationItemModel
                            {
                                LeaveId = n.Id,
                                Type = "Rejected",
                                Icon = "❌",
                                Title = "İZİN RED BİLDİRİMİ",
                                Message = $"{dateRange} tarihli izin talebiniz yöneticiniz ({adminWho}) tarafından REDDEDİLDİ!"
                            });
                        }
                        else if (string.Equals(n.Status, "Approved", StringComparison.OrdinalIgnoreCase) && n.RequestNote != null && n.RequestNote.Contains("❌ Değişiklik talebi reddedildi"))
                        {
                            list.Add(new NotificationItemModel
                            {
                                LeaveId = n.Id,
                                Type = "Rejected",
                                Icon = "❌",
                                Title = "İZİN DEĞİŞİKLİK TALEBİ REDDEDİLDİ",
                                Message = $"Değişiklik talebiniz yöneticiniz ({adminWho}) tarafından REDDEDİLDİ! Önceki onaylı izniniz ({dateRange}) geçerli kalmaya devam etmektedir."
                            });
                        }
                        else if (n.RequestNote != null && n.RequestNote.Contains("izninizi güncelledi"))
                        {
                            list.Add(new NotificationItemModel
                            {
                                LeaveId = n.Id,
                                Type = "Updated",
                                Icon = "🔄",
                                Title = "İZİN DEĞİŞİKLİK BİLDİRİMİ",
                                Message = $"İzninizin tarihi yöneticiniz ({adminWho}) tarafından {dateRange} olarak GÜNCELLENDİ."
                            });
                        }
                        else if (string.Equals(n.Status, "Approved", StringComparison.OrdinalIgnoreCase))
                        {
                            list.Add(new NotificationItemModel
                            {
                                LeaveId = n.Id,
                                Type = "Approved",
                                Icon = "✅",
                                Title = "İZİN TALEBİ BİLDİRİMİ",
                                Message = $"{dateRange} tarihli izin talebiniz yöneticiniz ({adminWho}) tarafından ONAYLANDI!"
                            });
                        }
                    }

                    NotificationList = list;
                }

                // 7. Admin Pending Approvals Notification
                if (IsAdmin)
                {
                    PendingApprovalCount = leaves.Count(l => string.Equals(l.Status, "Pending", StringComparison.OrdinalIgnoreCase));
                }
                else
                {
                    PendingApprovalCount = 0;
                }

                // 8. POPULATE LIVE OPERATIONAL DASHBOARD WIDGETS
                // A. Today On-Duty Personnel
                var allUsers = _services.GetAllUsers();
                var todayShifts = new List<TodayShiftItem>();
                int weekDiff = (7 + ((int)today.DayOfWeek - (int)DayOfWeek.Monday)) % 7;
                DateTime currentWeekStart = today.AddDays(-weekDiff).Date;
                DateTime currentWeekEnd = currentWeekStart.AddDays(6).Date;

                var monthlyReleases = _services.GetAllMonthlyReleaseShifts().Where(x => !x.IsFinished).ToList();
                foreach (var m in monthlyReleases)
                {
                    bool isWeekly = (!string.IsNullOrWhiteSpace(m.MonthName) && m.MonthName.Contains("Haftalık", StringComparison.OrdinalIgnoreCase)) ||
                                    (m.ReleaseDate.Date >= currentWeekStart && m.ReleaseDate.Date <= currentWeekEnd);

                    bool isActiveToday = isWeekly
                        ? ((m.ReleaseDate.Date >= currentWeekStart && m.ReleaseDate.Date <= currentWeekEnd) || (m.ReleaseDate.Date <= today && today <= m.ReleaseDate.Date.AddDays(6)))
                        : (m.ReleaseDate.Date == today);

                    if (isActiveToday && !string.IsNullOrWhiteSpace(m.AssignedUsers))
                    {
                        var names = m.AssignedUsers.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries).Select(n => n.Trim());
                        foreach (var name in names)
                        {
                            string cleanName = name.Contains(':') ? name.Substring(name.IndexOf(':') + 1).Trim() : name.Trim();
                            var userObj = allUsers.FirstOrDefault(u => u.FullName.Equals(cleanName, StringComparison.OrdinalIgnoreCase));
                            string teamName = userObj?.Team ?? "Genel / Ortak";

                            todayShifts.Add(new TodayShiftItem
                            {
                                PersonnelName = cleanName,
                                ShiftTitle = string.IsNullOrWhiteSpace(m.MonthName) ? "Yaygınlaştırma Nöbeti" : m.MonthName,
                                Team = teamName,
                                JiraTicketNo = m.JiraTicketNo ?? ""
                            });
                        }
                    }
                }

                var customShifts = _services.GetAllCustomShifts().Where(x => !x.IsFinished).ToList();
                foreach (var c in customShifts)
                {
                    bool isWeekly = (!string.IsNullOrWhiteSpace(c.Topic) && c.Topic.Contains("Haftalık", StringComparison.OrdinalIgnoreCase)) ||
                                    (!string.IsNullOrWhiteSpace(c.Description) && c.Description.Contains("Haftalık", StringComparison.OrdinalIgnoreCase));

                    bool isActiveToday = isWeekly
                        ? (c.ShiftDate.Date >= currentWeekStart && c.ShiftDate.Date <= currentWeekEnd)
                        : (c.ShiftDate.Date == today);

                    if (isActiveToday && !string.IsNullOrWhiteSpace(c.AssignedUsers))
                    {
                        var names = c.AssignedUsers.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries).Select(n => n.Trim());
                        foreach (var name in names)
                        {
                            string cleanName = name.Contains(':') ? name.Substring(name.IndexOf(':') + 1).Trim() : name.Trim();
                            var userObj = allUsers.FirstOrDefault(u => u.FullName.Equals(cleanName, StringComparison.OrdinalIgnoreCase));
                            string teamName = userObj?.Team ?? "Özel / Geçiş";

                            todayShifts.Add(new TodayShiftItem
                            {
                                PersonnelName = cleanName,
                                ShiftTitle = string.IsNullOrWhiteSpace(c.Topic) ? "Özel Nöbet" : c.Topic,
                                Team = teamName,
                                JiraTicketNo = c.Description ?? ""
                            });
                        }
                    }
                }

                foreach (var s in shifts.Where(x => x.User != null))
                {
                    bool isWeekly = (s.ShiftType != null && s.ShiftType.ShiftName.Contains("Haftalık", StringComparison.OrdinalIgnoreCase));
                    bool isActiveToday = isWeekly
                        ? (s.ShiftDate.Date >= currentWeekStart && s.ShiftDate.Date <= currentWeekEnd)
                        : (s.ShiftDate.Date == today);

                    if (isActiveToday)
                    {
                        todayShifts.Add(new TodayShiftItem
                        {
                            PersonnelName = s.User?.FullName ?? "",
                            ShiftTitle = s.ShiftType?.ShiftName ?? "Nöbet",
                            Team = s.User?.Team ?? "",
                            JiraTicketNo = s.JiraTicketNo ?? ""
                        });
                    }
                }

                TodayShiftList = new ObservableCollection<TodayShiftItem>(todayShifts);
                int activeDutyCount = todayShifts.Select(s => s.PersonnelName.Trim()).Where(n => !string.IsNullOrWhiteSpace(n)).Distinct(StringComparer.OrdinalIgnoreCase).Count();
                if (activeDutyCount > 0)
                {
                    WeeklyShiftCount = activeDutyCount;
                }

                // Detect active shift-leave conflicts for upcoming/active releases and custom shifts
                var conflictList = new List<ShiftConflictNotificationModel>();
                var activeLeavesForScan = leaves.Where(l => l.EndDate.Date >= today && (string.IsNullOrEmpty(l.Status) || string.Equals(l.Status, "Approved", StringComparison.OrdinalIgnoreCase) || string.Equals(l.Status, "Onaylandı", StringComparison.OrdinalIgnoreCase)) && !string.Equals(l.Status, "Rejected", StringComparison.OrdinalIgnoreCase) && !string.Equals(l.Status, "Reddedildi", StringComparison.OrdinalIgnoreCase)).ToList();

                foreach (var m in monthlyReleasesAll)
                {
                    if (m.IsFinished || string.IsNullOrWhiteSpace(m.AssignedUsers)) continue;

                    bool isWeekly = (!string.IsNullOrWhiteSpace(m.MonthName) && m.MonthName.Contains("Haftalık", StringComparison.OrdinalIgnoreCase));
                    DateTime mStart = m.ReleaseDate.Date;
                    DateTime mEnd = mStart;
                    if (isWeekly)
                    {
                        int mDiff = (7 + ((int)m.ReleaseDate.DayOfWeek - (int)DayOfWeek.Monday)) % 7;
                        mStart = m.ReleaseDate.Date.AddDays(-mDiff);
                        mEnd = mStart.AddDays(6).Date;
                    }

                    if (mEnd < today) continue; // Ignore past shifts

                    var assignedNames = m.AssignedUsers.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                                                       .Select(n => n.Contains(':') ? n.Substring(n.IndexOf(':') + 1).Trim() : n.Trim());

                    foreach (var name in assignedNames)
                    {
                        var userObj = allUsers.FirstOrDefault(u => u.FullName.Equals(name, StringComparison.OrdinalIgnoreCase));
                        if (userObj != null)
                        {
                            var userLeaves = activeLeavesForScan.Where(l => (l.UserId == userObj.Id || (l.User != null && l.User.FullName.Equals(userObj.FullName, StringComparison.OrdinalIgnoreCase))) && l.StartDate.Date <= mEnd && l.EndDate.Date >= mStart).ToList();
                            foreach (var leave in userLeaves)
                            {
                                conflictList.Add(new ShiftConflictNotificationModel
                                {
                                    ShiftId = m.Id,
                                    ShiftCategory = "MonthlyRelease",
                                    PersonnelName = userObj.FullName,
                                    LeaveDatesText = $"{leave.StartDate:dd.MM.yyyy} - {leave.EndDate:dd.MM.yyyy}",
                                    ShiftTitleText = string.IsNullOrWhiteSpace(m.MonthName) ? "Yaygınlaştırma Nöbeti" : m.MonthName
                                });
                            }
                        }
                    }
                }

                foreach (var c in customShifts)
                {
                    if (c.IsFinished || string.IsNullOrWhiteSpace(c.AssignedUsers)) continue;

                    bool isWeekly = (!string.IsNullOrWhiteSpace(c.Topic) && c.Topic.Contains("Haftalık", StringComparison.OrdinalIgnoreCase)) ||
                                    (!string.IsNullOrWhiteSpace(c.Description) && c.Description.Contains("Haftalık", StringComparison.OrdinalIgnoreCase));
                    DateTime cStart = c.ShiftDate.Date;
                    DateTime cEnd = cStart;
                    if (isWeekly)
                    {
                        int cDiff = (7 + ((int)c.ShiftDate.DayOfWeek - (int)DayOfWeek.Monday)) % 7;
                        cStart = c.ShiftDate.Date.AddDays(-cDiff);
                        cEnd = cStart.AddDays(6).Date;
                    }

                    if (cEnd < today) continue; // Ignore past shifts

                    var assignedNames = c.AssignedUsers.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                                                       .Select(n => n.Contains(':') ? n.Substring(n.IndexOf(':') + 1).Trim() : n.Trim());

                    foreach (var name in assignedNames)
                    {
                        var userObj = allUsers.FirstOrDefault(u => u.FullName.Equals(name, StringComparison.OrdinalIgnoreCase));
                        if (userObj != null)
                        {
                            var userLeaves = activeLeavesForScan.Where(l => (l.UserId == userObj.Id || (l.User != null && l.User.FullName.Equals(userObj.FullName, StringComparison.OrdinalIgnoreCase))) && l.StartDate.Date <= cEnd && l.EndDate.Date >= cStart).ToList();
                            foreach (var leave in userLeaves)
                            {
                                conflictList.Add(new ShiftConflictNotificationModel
                                {
                                    ShiftId = c.Id,
                                    ShiftCategory = "Custom",
                                    PersonnelName = userObj.FullName,
                                    LeaveDatesText = $"{leave.StartDate:dd.MM.yyyy} - {leave.EndDate:dd.MM.yyyy}",
                                    ShiftTitleText = string.IsNullOrWhiteSpace(c.Topic) ? "Özel Nöbet" : c.Topic
                                });
                            }
                        }
                    }
                }

                ShiftConflictList = new ObservableCollection<ShiftConflictNotificationModel>(conflictList);

                // B. Today Active Approved Leaves
                var activeTodayLeaves = leaves.Where(l => l.Status == "Approved" && l.User != null && today >= l.StartDate.Date && today <= l.EndDate.Date)
                    .Select(l => new TodayLeaveItem
                    {
                        PersonnelName = l.User?.FullName ?? "",
                        Team = l.User?.Team ?? "",
                        Title = l.User?.Title ?? "",
                        StartDate = l.StartDate,
                        EndDate = l.EndDate
                    }).ToList();

                TodayLeaveList = new ObservableCollection<TodayLeaveItem>(activeTodayLeaves);

                // C. Upcoming Monthly Releases
                var upcomingReleases = monthlyReleases
                    .Where(m => m.ReleaseDate.Date >= today)
                    .OrderBy(m => m.ReleaseDate)
                    .Take(4)
                    .Select(m => new UpcomingReleaseItem
                    {
                        ReleaseDate = m.ReleaseDate,
                        ReleaseName = string.IsNullOrWhiteSpace(m.MonthName) ? "Yaygınlaştırma Nöbeti" : m.MonthName,
                        AssignedUsers = m.AssignedUsers,
                        JiraTicketNo = m.JiraTicketNo ?? ""
                    }).ToList();

                UpcomingReleaseList = new ObservableCollection<UpcomingReleaseItem>(upcomingReleases);

                // D. Team Department Summary
                var teamSummaries = new List<TeamSummaryItem>
                {
                    new TeamSummaryItem
                    {
                        TeamName = "Takip Ekibi",
                        TotalMemberCount = allUsers.Count(u => string.Equals(u.Team, "Takip", StringComparison.OrdinalIgnoreCase)),
                        ActiveLeaveCount = activeTodayLeaves.Count(l => string.Equals(l.Team, "Takip", StringComparison.OrdinalIgnoreCase)),
                        OnDutyCount = todayShifts.Count(s => string.Equals(s.Team, "Takip", StringComparison.OrdinalIgnoreCase)),
                        BadgeColor = "#5b21b6",
                        BadgeBg = "#ede9fe"
                    },
                    new TeamSummaryItem
                    {
                        TeamName = "Tahsis Ekibi",
                        TotalMemberCount = allUsers.Count(u => string.Equals(u.Team, "Tahsis", StringComparison.OrdinalIgnoreCase)),
                        ActiveLeaveCount = activeTodayLeaves.Count(l => string.Equals(l.Team, "Tahsis", StringComparison.OrdinalIgnoreCase)),
                        OnDutyCount = todayShifts.Count(s => string.Equals(s.Team, "Tahsis", StringComparison.OrdinalIgnoreCase)),
                        BadgeColor = "#065f46",
                        BadgeBg = "#d1fae5"
                    },
                    new TeamSummaryItem
                    {
                        TeamName = "Teminat Ekibi",
                        TotalMemberCount = allUsers.Count(u => string.Equals(u.Team, "Teminat", StringComparison.OrdinalIgnoreCase)),
                        ActiveLeaveCount = activeTodayLeaves.Count(l => string.Equals(l.Team, "Teminat", StringComparison.OrdinalIgnoreCase)),
                        OnDutyCount = todayShifts.Count(s => string.Equals(s.Team, "Teminat", StringComparison.OrdinalIgnoreCase)),
                        BadgeColor = "#1e40af",
                        BadgeBg = "#dbeafe"
                    }
                };

                TeamSummaryList = new ObservableCollection<TeamSummaryItem>(teamSummaries);

                // E. Populate Personal Profile & Team Context
                if (!string.IsNullOrWhiteSpace(currentUserName))
                {
                    var currentUser = allUsers.FirstOrDefault(u => u.FullName.Equals(currentUserName, StringComparison.OrdinalIgnoreCase) || u.Name.Contains(currentUserName));
                    if (currentUser != null)
                    {
                        UserFullName = currentUser.FullName;
                        UserTitle = currentUser.Title;
                        UserTeam = currentUser.Team;

                        // My Next Shift
                        bool isOnDutyToday = todayShifts.Any(s => string.Equals(s.PersonnelName, currentUser.FullName, StringComparison.OrdinalIgnoreCase));
                        if (isOnDutyToday)
                        {
                            MyNextShiftText = "🚨 BUGÜN NÖBETÇİSİNİZ!";
                        }
                        else
                        {
                            var myShifts = shifts.Where(s => s.UserId == currentUser.Id && s.ShiftDate.Date >= today).OrderBy(s => s.ShiftDate).ToList();
                            if (myShifts.Any())
                            {
                                MyNextShiftText = $"📅 Gelecek Nöbetiniz: {myShifts.First().ShiftDate:dd.MM.yyyy}";
                            }
                            else
                            {
                                MyNextShiftText = "📌 Kayıtlı nöbetiniz yok";
                            }
                        }

                        // My Next Leave
                        var myLeaves = leaves.Where(l => l.UserId == currentUser.Id && l.Status == "Approved" && l.EndDate.Date >= today).OrderBy(l => l.StartDate).ToList();
                        if (myLeaves.Any(l => today >= l.StartDate.Date && today <= l.EndDate.Date))
                        {
                            MyNextLeaveText = "🌴 Şu An İzinlisiniz";
                        }
                        else if (myLeaves.Any())
                        {
                            MyNextLeaveText = $"🌴 Gelecek İzniniz: {myLeaves.First().StartDate:dd.MM.yyyy} - {myLeaves.First().EndDate:dd.MM.yyyy}";
                        }
                        else
                        {
                            MyNextLeaveText = "🌴 Planlanmış izniniz yok";
                        }

                        // Teammates in same team (or All Teams for Department Manager)
                        if (currentUser.Team != null && (currentUser.Team.Equals("Departman Yönetimi", StringComparison.OrdinalIgnoreCase) || currentUser.Team.Equals("Yönetim", StringComparison.OrdinalIgnoreCase)))
                        {
                            MyTeammatesText = "🏢 Katılım Finansman Departmanı (Tüm Ekipler)";
                        }
                        else
                        {
                            var teammates = allUsers
                                .Where(u => u.Team != null && u.Team.Equals(currentUser.Team, StringComparison.OrdinalIgnoreCase) && u.Id != currentUser.Id)
                                .Select(u => u.FullName)
                                .ToList();
                            MyTeammatesText = teammates.Any() ? string.Join(", ", teammates) : "Ekip üyesi bulunmuyor";
                        }
                    }
                }

                // Generate AI Executive Digest & Risk Summary
                AiExecutiveReport = ZiraatMatrixAiEngine.Instance.Digest.GenerateDailyDigest(allUsers, leaves, shifts, allProjects, monthlyReleases, customShifts);
            }
            catch (Exception)
            {
                WeeklyShiftCount = 0;
                ActiveLeaveCount = 0;
                OngoingProjectsCount = 0;
                CriticalBudgetProjectsCount = 0;
                HasShiftNotification = false;
                PendingApprovalCount = 0;
            }
        }

        private void CheckShiftNotifications(string currentUserName)
        {
            if (string.IsNullOrWhiteSpace(currentUserName))
            {
                HasShiftNotification = false;
                ShiftNotificationText = string.Empty;
                return;
            }

            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            var monthlyReleases = _services.GetAllMonthlyReleaseShifts().Where(x => !x.IsFinished).ToList();
            var customShifts = _services.GetAllCustomShifts();
            var standardShifts = _services.GetAllShifts();

            int weekDiff = (7 + ((int)today.DayOfWeek - (int)DayOfWeek.Monday)) % 7;
            DateTime currentWeekStart = today.AddDays(-weekDiff).Date;
            DateTime currentWeekEnd = currentWeekStart.AddDays(6).Date;

            var userShiftsToday = new List<string>();
            var userShiftsTomorrow = new List<string>();
            var upcomingUserShifts = new List<(DateTime Date, string Title, string Jira)>();

            // 1. Check Monthly & Weekly Release Shifts
            foreach (var m in monthlyReleases)
            {
                if (IsUserAssigned(currentUserName, m.AssignedUsers))
                {
                    var title = string.IsNullOrWhiteSpace(m.MonthName) ? "Yaygınlaştırma Nöbeti" : m.MonthName;
                    var jira = string.IsNullOrWhiteSpace(m.JiraTicketNo) ? "" : $"Jira: {m.JiraTicketNo}";
                    var detail = string.IsNullOrWhiteSpace(jira) ? title : $"{title} | {jira}";

                    bool isWeekly = (!string.IsNullOrWhiteSpace(m.MonthName) && m.MonthName.Contains("Haftalık", StringComparison.OrdinalIgnoreCase)) ||
                                    (m.ReleaseDate.Date >= currentWeekStart && m.ReleaseDate.Date <= currentWeekEnd);

                    bool isActiveToday = isWeekly
                        ? ((m.ReleaseDate.Date >= currentWeekStart && m.ReleaseDate.Date <= currentWeekEnd) || (m.ReleaseDate.Date <= today && today <= m.ReleaseDate.Date.AddDays(6)))
                        : (m.ReleaseDate.Date == today);

                    if (isActiveToday)
                        userShiftsToday.Add(detail);
                    else if (m.ReleaseDate.Date == tomorrow)
                        userShiftsTomorrow.Add(detail);
                    else if (m.ReleaseDate.Date > today)
                        upcomingUserShifts.Add((m.ReleaseDate.Date, title, m.JiraTicketNo ?? ""));
                }
            }

            // 2. Check Custom & Transition Shifts
            foreach (var c in customShifts)
            {
                if (IsUserAssigned(currentUserName, c.AssignedUsers))
                {
                    var title = string.IsNullOrWhiteSpace(c.Topic) ? "Özel Nöbet" : c.Topic;
                    var jira = string.IsNullOrWhiteSpace(c.Description) ? "" : $"Açıklama: {c.Description}";
                    var detail = string.IsNullOrWhiteSpace(jira) ? title : $"{title} | {jira}";

                    bool isWeekly = (!string.IsNullOrWhiteSpace(c.Topic) && c.Topic.Contains("Haftalık", StringComparison.OrdinalIgnoreCase)) ||
                                    (!string.IsNullOrWhiteSpace(c.Description) && c.Description.Contains("Haftalık", StringComparison.OrdinalIgnoreCase));

                    bool isActiveToday = isWeekly
                        ? (c.ShiftDate.Date >= currentWeekStart && c.ShiftDate.Date <= currentWeekEnd)
                        : (c.ShiftDate.Date == today);

                    if (isActiveToday)
                        userShiftsToday.Add(detail);
                    else if (c.ShiftDate.Date == tomorrow)
                        userShiftsTomorrow.Add(detail);
                    else if (c.ShiftDate.Date > today)
                        upcomingUserShifts.Add((c.ShiftDate.Date, title, c.Description ?? ""));
                }
            }

            // 3. Check Standard Individual Shifts
            foreach (var s in standardShifts)
            {
                if (s.User != null && IsUserAssigned(currentUserName, s.User.FullName))
                {
                    var title = s.ShiftType?.ShiftName ?? "Nöbet";
                    var jira = string.IsNullOrWhiteSpace(s.JiraTicketNo) ? "" : $"Jira: {s.JiraTicketNo}";
                    var detail = string.IsNullOrWhiteSpace(jira) ? title : $"{title} | {jira}";

                    bool isWeekly = (s.ShiftType != null && s.ShiftType.ShiftName.Contains("Haftalık", StringComparison.OrdinalIgnoreCase));
                    bool isActiveToday = isWeekly
                        ? (s.ShiftDate.Date >= currentWeekStart && s.ShiftDate.Date <= currentWeekEnd)
                        : (s.ShiftDate.Date == today);

                    if (isActiveToday)
                        userShiftsToday.Add(detail);
                    else if (s.ShiftDate.Date == tomorrow)
                        userShiftsTomorrow.Add(detail);
                    else if (s.ShiftDate.Date > today)
                        upcomingUserShifts.Add((s.ShiftDate.Date, title, s.JiraTicketNo));
                }
            }

            var messages = new List<string>();

            if (userShiftsToday.Any())
            {
                messages.Add($"🚨 BUGÜN ( {today:dd.MM.yyyy} ) NÖBETÇİSİNİZ! [ {string.Join("   •   ", userShiftsToday)} ]");
            }

            if (userShiftsTomorrow.Any())
            {
                messages.Add($"⏰ YARIN ( {tomorrow:dd.MM.yyyy} ) NÖBETÇİSİNİZ! [ {string.Join("   •   ", userShiftsTomorrow)} ]");
            }

            // If no shift today or tomorrow, check next upcoming shift
            if (!messages.Any() && upcomingUserShifts.Any())
            {
                var nextShift = upcomingUserShifts.OrderBy(s => s.Date).First();
                var daysLeft = (nextShift.Date - today).Days;
                var jira = string.IsNullOrWhiteSpace(nextShift.Jira) ? "" : $" | Jira: {nextShift.Jira}";
                messages.Add($"📅 YAKLAŞAN NÖBETİNİZ: {nextShift.Date:dd.MM.yyyy} ({daysLeft} gün sonra) [ {nextShift.Title}{jira} ]");
            }

            if (messages.Any())
            {
                HasShiftNotification = true;
                ShiftNotificationText = string.Join("      •      ", messages);
            }
            else
            {
                HasShiftNotification = false;
                ShiftNotificationText = string.Empty;
            }
        }

        private bool IsUserAssigned(string currentUserName, string? assignedUsersString)
        {
            if (string.IsNullOrWhiteSpace(currentUserName) || string.IsNullOrWhiteSpace(assignedUsersString))
                return false;

            var targetClean = currentUserName.Trim();

            var names = assignedUsersString.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                                           .Select(n => n.Trim());

            foreach (var name in names)
            {
                if (string.Equals(name, targetClean, StringComparison.OrdinalIgnoreCase))
                    return true;

                // Match first name and last name if middle names are present or omitted
                var userTokens = targetClean.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var nameTokens = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (userTokens.Length >= 2 && nameTokens.Length >= 2)
                {
                    if (string.Equals(userTokens[0], nameTokens[0], StringComparison.OrdinalIgnoreCase) &&
                        string.Equals(userTokens[userTokens.Length - 1], nameTokens[nameTokens.Length - 1], StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
