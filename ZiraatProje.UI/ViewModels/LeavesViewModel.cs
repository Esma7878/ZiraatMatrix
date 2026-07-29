using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ZiraatProje.Business;
using ZiraatProje.DataAccess;

namespace ZiraatProje.UI.ViewModels
{
    public class HolidayInfo
    {
        public bool IsHoliday { get; set; }
        public bool IsHalfDay { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public static class TurkeyHolidays
    {
        public static HolidayInfo GetHolidayInfo(DateTime date)
        {
            var info = new HolidayInfo();
            int month = date.Month;
            int day = date.Day;
            int year = date.Year;

            // Sabit Resmi Tatiller
            if (month == 1 && day == 1)
            {
                info.IsHoliday = true;
                info.Name = "Yılbaşı";
                return info;
            }
            if (month == 4 && day == 23)
            {
                info.IsHoliday = true;
                info.Name = "Ulusal Egemenlik ve Çocuk Bayramı";
                return info;
            }
            if (month == 5 && day == 1)
            {
                info.IsHoliday = true;
                info.Name = "Emek ve Dayanışma Günü";
                return info;
            }
            if (month == 5 && day == 19)
            {
                info.IsHoliday = true;
                info.Name = "Atatürk'ü Anma, Gençlik ve Spor Bayramı";
                return info;
            }
            if (month == 7 && day == 15)
            {
                info.IsHoliday = true;
                info.Name = "Demokrasi ve Milli Birlik Günü";
                return info;
            }
            if (month == 8 && day == 30)
            {
                info.IsHoliday = true;
                info.Name = "Zafer Bayramı";
                return info;
            }
            if (month == 10 && day == 28)
            {
                info.IsHoliday = true;
                info.IsHalfDay = true;
                info.Name = "Cumhuriyet Bayramı Arefesi (Yarım Gün)";
                return info;
            }
            if (month == 10 && day == 29)
            {
                info.IsHoliday = true;
                info.Name = "Cumhuriyet Bayramı";
                return info;
            }

            // Değişken Dini Tatiller (2025, 2026, 2027)
            if (year == 2025)
            {
                // Ramazan Bayramı
                if (month == 3 && day == 29) { info.IsHoliday = true; info.IsHalfDay = true; info.Name = "Ramazan Bayramı Arefesi (Yarım Gün)"; return info; }
                if (month == 3 && day == 30) { info.IsHoliday = true; info.Name = "Ramazan Bayramı 1. Gün"; return info; }
                if (month == 3 && day == 31) { info.IsHoliday = true; info.Name = "Ramazan Bayramı 2. Gün"; return info; }
                if (month == 4 && day == 1) { info.IsHoliday = true; info.Name = "Ramazan Bayramı 3. Gün"; return info; }

                // Kurban Bayramı
                if (month == 6 && day == 5) { info.IsHoliday = true; info.IsHalfDay = true; info.Name = "Kurban Bayramı Arefesi (Yarım Gün)"; return info; }
                if (month == 6 && day == 6) { info.IsHoliday = true; info.Name = "Kurban Bayramı 1. Gün"; return info; }
                if (month == 6 && day == 7) { info.IsHoliday = true; info.Name = "Kurban Bayramı 2. Gün"; return info; }
                if (month == 6 && day == 8) { info.IsHoliday = true; info.Name = "Kurban Bayramı 3. Gün"; return info; }
                if (month == 6 && day == 9) { info.IsHoliday = true; info.Name = "Kurban Bayramı 4. Gün"; return info; }
            }
            else if (year == 2026)
            {
                // Ramazan Bayramı
                if (month == 3 && day == 19) { info.IsHoliday = true; info.IsHalfDay = true; info.Name = "Ramazan Bayramı Arefesi (Yarım Gün)"; return info; }
                if (month == 3 && day == 20) { info.IsHoliday = true; info.Name = "Ramazan Bayramı 1. Gün"; return info; }
                if (month == 3 && day == 21) { info.IsHoliday = true; info.Name = "Ramazan Bayramı 2. Gün"; return info; }
                if (month == 3 && day == 22) { info.IsHoliday = true; info.Name = "Ramazan Bayramı 3. Gün"; return info; }

                // Kurban Bayramı
                if (month == 5 && day == 26) { info.IsHoliday = true; info.IsHalfDay = true; info.Name = "Kurban Bayramı Arefesi (Yarım Gün)"; return info; }
                if (month == 5 && day == 27) { info.IsHoliday = true; info.Name = "Kurban Bayramı 1. Gün"; return info; }
                if (month == 5 && day == 28) { info.IsHoliday = true; info.Name = "Kurban Bayramı 2. Gün"; return info; }
                if (month == 5 && day == 29) { info.IsHoliday = true; info.Name = "Kurban Bayramı 3. Gün"; return info; }
                if (month == 5 && day == 30) { info.IsHoliday = true; info.Name = "Kurban Bayramı 4. Gün"; return info; }
            }
            else if (year == 2027)
            {
                // Ramazan Bayramı
                if (month == 3 && day == 8) { info.IsHoliday = true; info.IsHalfDay = true; info.Name = "Ramazan Bayramı Arefesi (Yarım Gün)"; return info; }
                if (month == 3 && day == 9) { info.IsHoliday = true; info.Name = "Ramazan Bayramı 1. Gün"; return info; }
                if (month == 3 && day == 10) { info.IsHoliday = true; info.Name = "Ramazan Bayramı 2. Gün"; return info; }
                if (month == 3 && day == 11) { info.IsHoliday = true; info.Name = "Ramazan Bayramı 3. Gün"; return info; }

                // Kurban Bayramı
                if (month == 5 && day == 15) { info.IsHoliday = true; info.IsHalfDay = true; info.Name = "Kurban Bayramı Arefesi (Yarım Gün)"; return info; }
                if (month == 5 && day == 16) { info.IsHoliday = true; info.Name = "Kurban Bayramı 1. Gün"; return info; }
                if (month == 5 && day == 17) { info.IsHoliday = true; info.Name = "Kurban Bayramı 2. Gün"; return info; }
                if (month == 5 && day == 18) { info.IsHoliday = true; info.Name = "Kurban Bayramı 3. Gün"; return info; }
                if (month == 5 && day == 19) { info.IsHoliday = true; info.Name = "Kurban Bayramı 4. Gün (Gençlik ve Spor Bayramı ile çakışıyor)"; return info; }
            }

            return info;
        }
    }

    public class MatrixCell
    {
        public DateTime Date { get; set; }
        public bool IsOnLeave { get; set; }
        public bool IsHourly { get; set; } // Saatlik izin mi?
        public string Status { get; set; } = "Approved"; // "Approved", "Pending"
        public bool IsSpecialRequest { get; set; }
        public string TooltipText { get; set; } = string.Empty;
        public string TeamName { get; set; } = string.Empty;
        public int DayNumber => Date.Day;
        public bool IsWeekend => Date.DayOfWeek == DayOfWeek.Saturday || Date.DayOfWeek == DayOfWeek.Sunday;
        public string CellIcon => (IsOnLeave && IsHourly) ? "⏰" : string.Empty;

        public HolidayInfo Holiday => TurkeyHolidays.GetHolidayInfo(Date);
        public bool IsHoliday => Holiday.IsHoliday;
        public bool IsHalfDay => Holiday.IsHalfDay;

        public string CellBackground
        {
            get
            {
                if (IsWeekend) return "#cbd5e1"; // Hafta sonu her zaman gri (çalışılmıyor, izin boyaması yok)
                if (IsOnLeave)
                {
                    if (IsHourly)
                    {
                        // Açık tonlu renkler
                        return TeamName switch
                        {
                            "Takip" => "#ddd6fe", // Açık mor
                            "Tahsis" => "#a7f3d0", // Açık yeşil
                            "Teminat" => "#93c5fd", // Açık mavi
                            _ => "#fca5a5"
                        };
                    }
                    return LeaveColor;
                }
                if (IsHoliday)
                {
                    if (IsHalfDay) return "#fef3c7"; // Yarım gün resmi tatil (açık sarı)
                    return "#fee2e2"; // Resmi Tatil (açık kırmızı/pembe)
                }
                return "#f8fafc";                // Hafta içi çalışan gün
            }
        }

        public string CellBorderColor
        {
            get
            {
                if (IsWeekend) return "#94a3b8"; // Hafta sonu kenarlık
                if (IsOnLeave)
                {
                    if (IsHourly)
                    {
                        return TeamName switch
                        {
                            "Takip" => "#a78bfa",
                            "Tahsis" => "#34d399",
                            "Teminat" => "#60a5fa",
                            _ => "#f87171"
                        };
                    }
                    return LeaveColor;
                }
                if (IsHoliday)
                {
                    return IsHalfDay ? "#f59e0b" : "#f87171"; // Turuncu / Kırmızı kenarlık
                }
                return "#e2e8f0";
            }
        }

        public string LeaveColor
        {
            get
            {
                if (Status == "Pending") return "#ed8936"; // Orange color for pending requests
                return TeamName switch
                {
                    "Takip" => "#5b21b6",
                    "Tahsis" => "#065f46",
                    "Teminat" => "#1e40af",
                    _ => "#bc171d"
                };
            }
        }
    }

    public class MatrixDayHeader
    {
        public DateTime Date { get; set; }
        public int DayNumber => Date.Day;
        public bool IsWeekend => Date.DayOfWeek == DayOfWeek.Saturday || Date.DayOfWeek == DayOfWeek.Sunday;
        
        public HolidayInfo Holiday => TurkeyHolidays.GetHolidayInfo(Date);
        public bool IsHoliday => Holiday.IsHoliday;
        public bool IsHalfDay => Holiday.IsHalfDay;

        public string HeaderBackground
        {
            get
            {
                if (IsWeekend) return "#cbd5e1";
                if (IsHoliday)
                {
                    if (IsHalfDay) return "#fef3c7"; // Yarım gün (açık sarı)
                    return "#fee2e2"; // Resmi tatil (açık kırmızı)
                }
                return "#f1f5f9";
            }
        }

        public string HeaderTextColor
        {
            get
            {
                if (IsWeekend) return "#1e293b";
                if (IsHoliday) return "#991b1b"; // Koyu kırmızı yazı
                return "#64748b";
            }
        }

        public string HeaderBorderColor => IsWeekend ? "#94a3b8" : "#e2e8f0";

        public string TooltipText
        {
            get
            {
                string txt = Date.ToString("dd MMMM yyyy (dddd)");
                if (IsWeekend) txt += " - Hafta Sonu (Tatil)";
                if (IsHoliday) txt += $" - Resmi Tatil: {Holiday.Name}";
                return txt;
            }
        }
    }

    // Not: Bu class DashboardViewModel tarafından da kullanılmaktadır.
    public class NotificationItemModel
    {
        public int LeaveId { get; set; }
        public string Type { get; set; } = "Approved";
        public string Icon { get; set; } = "✅";
        public string Title { get; set; } = "BİLDİRİM";
        public string Message { get; set; } = string.Empty;

        public string BackgroundColor => Type switch
        {
            "Approved" => "#dcfce7",
            "Updated"  => "#fef3c7",
            "Rejected" => "#fee2e2",
            "Cancelled"=> "#ffedd5",
            _ => "#dcfce7"
        };

        public string BorderColor => Type switch
        {
            "Approved" => "#16a34a",
            "Updated"  => "#f59e0b",
            "Rejected" => "#dc2626",
            "Cancelled"=> "#ea580c",
            _ => "#16a34a"
        };

        public string HeaderTextColor => Type switch
        {
            "Approved" => "#14532d",
            "Updated"  => "#92400e",
            "Rejected" => "#991b1b",
            "Cancelled"=> "#9a3412",
            _ => "#14532d"
        };

        public string BodyTextColor => Type switch
        {
            "Approved" => "#166534",
            "Updated"  => "#78350f",
            "Rejected" => "#7f1d1d",
            "Cancelled"=> "#c2410c",
            _ => "#166534"
        };

        public string ButtonBackground => Type switch
        {
            "Approved" => "#bbf7d0",
            "Updated"  => "#fde047",
            "Rejected" => "#fecaca",
            "Cancelled"=> "#fed7aa",
            _ => "#bbf7d0"
        };

        public string ButtonForeground => HeaderTextColor;
    }

    public class MatrixRow
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string UserTitle { get; set; } = string.Empty;
        public string Team { get; set; } = string.Empty;
        public List<MatrixCell> Cells { get; set; } = new List<MatrixCell>();
    }

    public class MonthHeaderItem
    {
        public string MonthName { get; set; } = string.Empty;
        public int DayCount { get; set; }
        public double Width => DayCount * 26; // 24px box + 2px margin = 26px per day
        public string BackgroundColor { get; set; } = "#edf2f7";
        public string BorderColor { get; set; } = "#cbd5e0";
        public string TextColor { get; set; } = "#2d3748";
    }

    public class TeamGroup
    {
        public string TeamName { get; set; } = string.Empty;
        public ObservableCollection<MatrixRow> Rows { get; set; } = new ObservableCollection<MatrixRow>();

        public string HeaderBgColor => TeamName switch
        {
            "Departman Yönetimi" => "#fff5f5",
            "Takip" => "#f5f3ff",
            "Tahsis" => "#ecfdf5",
            "Teminat" => "#eff6ff",
            _ => "#edf2f7"
        };

        public string HeaderBorderColor => TeamName switch
        {
            "Departman Yönetimi" => "#fca5a5",
            "Takip" => "#ddd6fe",
            "Tahsis" => "#a7f3d0",
            "Teminat" => "#bfdbfe",
            _ => "#cbd5e0"
        };

        public string HeaderTextColor => TeamName switch
        {
            "Departman Yönetimi" => "#991b1b",
            "Takip" => "#5b21b6",
            "Tahsis" => "#065f46",
            "Teminat" => "#1e40af",
            _ => "#2d3748"
        };
    }

    public class LeavesViewModel : BaseViewModel
    {
        private readonly BusinessServices _services = new BusinessServices();

        private string _currentUserName = string.Empty;
        public string CurrentUserName
        {
            get => _currentUserName;
            set
            {
                _currentUserName = value;
                OnPropertyChanged();
                AutoSelectCurrentUser();
            }
        }

        // Data lists
        private ObservableCollection<Leave> _leavesList = new ObservableCollection<Leave>();
        public ObservableCollection<Leave> LeavesList
        {
            get => _leavesList;
            set { _leavesList = value; OnPropertyChanged(); }
        }



        private ObservableCollection<Leave> _takipLeaves = new ObservableCollection<Leave>();
        public ObservableCollection<Leave> TakipLeaves
        {
            get => _takipLeaves;
            set { _takipLeaves = value; OnPropertyChanged(); }
        }

        private ObservableCollection<Leave> _tahsisLeaves = new ObservableCollection<Leave>();
        public ObservableCollection<Leave> TahsisLeaves
        {
            get => _tahsisLeaves;
            set { _tahsisLeaves = value; OnPropertyChanged(); }
        }

        private ObservableCollection<Leave> _teminatLeaves = new ObservableCollection<Leave>();
        public ObservableCollection<Leave> TeminatLeaves
        {
            get => _teminatLeaves;
            set { _teminatLeaves = value; OnPropertyChanged(); }
        }

        private ObservableCollection<Leave> _departmanLeaves = new ObservableCollection<Leave>();
        public ObservableCollection<Leave> DepartmanLeaves
        {
            get => _departmanLeaves;
            set { _departmanLeaves = value; OnPropertyChanged(); }
        }

        // Leave History Archive (Past completed leaves)
        private ObservableCollection<Leave> _historyLeaves = new ObservableCollection<Leave>();
        public ObservableCollection<Leave> HistoryLeaves
        {
            get => _historyLeaves;
            set { _historyLeaves = value; OnPropertyChanged(); }
        }

        private short _historySelectedYear = (short)DateTime.Today.Year;
        public short HistorySelectedYear
        {
            get => _historySelectedYear;
            set
            {
                _historySelectedYear = value;
                OnPropertyChanged();
                UpdateHistoryLeaves();
            }
        }

        public void UpdateHistoryLeaves()
        {
            try
            {
                var allLeaves = _services.GetAllLeaves();
                var history = allLeaves.Where(l =>
                    l.EndDate.Date < DateTime.Today &&
                    (string.IsNullOrEmpty(l.Status) || string.Equals(l.Status, "Approved", StringComparison.OrdinalIgnoreCase)) &&
                    (l.StartDate.Year == HistorySelectedYear || l.EndDate.Year == HistorySelectedYear)
                )
                .OrderByDescending(l => l.EndDate)
                .ToList();

                HistoryLeaves = new ObservableCollection<Leave>(history);
            }
            catch
            {
                HistoryLeaves = new ObservableCollection<Leave>();
            }
        }

        private ObservableCollection<User> _usersList = new ObservableCollection<User>();
        public ObservableCollection<User> UsersList
        {
            get => _usersList;
            set { _usersList = value; OnPropertyChanged(); }
        }

        private Leave? _selectedLeave;
        public Leave? SelectedLeave
        {
            get => _selectedLeave;
            set
            {
                if (_selectedLeave == value) return;
                _selectedLeave = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SaveButtonText));
                OnPropertyChanged(nameof(FormTitleText));
                OnPropertyChanged(nameof(IsSaveButtonEnabled));
                LoadSelectedLeave();
            }
        }

        private User? _selectedUser;
        public User? SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                if (_selectedUser != null)
                {
                    _selectedUserId = _selectedUser.Id;
                }
                OnPropertyChanged();
                OnPropertyChanged(nameof(SelectedUserId));
            }
        }

        // Form Fields
        private int _selectedUserId;
        public int SelectedUserId
        {
            get => _selectedUserId;
            set
            {
                _selectedUserId = value;
                if (UsersList != null && _selectedUserId > 0)
                {
                    _selectedUser = UsersList.FirstOrDefault(u => u.Id == _selectedUserId);
                    OnPropertyChanged(nameof(SelectedUser));
                }
                OnPropertyChanged();
            }
        }

        private DateTime _startDate = DateTime.Today;
        public DateTime StartDate
        {
            get => _startDate;
            set { _startDate = value; OnPropertyChanged(); }
        }

        private DateTime _endDate = DateTime.Today;
        public DateTime EndDate
        {
            get => _endDate;
            set { _endDate = value; OnPropertyChanged(); }
        }

        private bool _isHourly;
        public bool IsHourly
        {
            get => _isHourly;
            set
            {
                _isHourly = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsDaily));
                OnPropertyChanged(nameof(IsDailyVisible));
                OnPropertyChanged(nameof(IsHourlyVisible));
            }
        }

        public Visibility IsDailyVisible => IsHourly ? Visibility.Collapsed : Visibility.Visible;
        public Visibility IsHourlyVisible => IsHourly ? Visibility.Visible : Visibility.Collapsed;

        public bool IsDaily
        {
            get => !IsHourly;
            set
            {
                IsHourly = !value;
                OnPropertyChanged();
            }
        }

        private DateTime _hourlyDate = DateTime.Today;
        public DateTime HourlyDate
        {
            get => _hourlyDate;
            set { _hourlyDate = value; OnPropertyChanged(); }
        }

        public List<string> StartHours { get; } = new List<string> { "09:00", "09:30", "10:00", "10:30", "11:00", "11:30", "12:00", "12:30", "13:00", "13:30", "14:00", "14:30", "15:00", "15:30", "16:00", "16:30", "17:00", "17:30" };
        public List<string> EndHours { get; } = new List<string> { "09:30", "10:00", "10:30", "11:00", "11:30", "12:00", "12:30", "13:00", "13:30", "14:00", "14:30", "15:00", "15:30", "16:00", "16:30", "17:00", "17:30", "18:00" };

        private string _selectedStartHour = "09:00";
        public string SelectedStartHour
        {
            get => _selectedStartHour;
            set
            {
                _selectedStartHour = value;
                OnPropertyChanged();
                UpdateEndHoursFilter();
            }
        }

        private string _selectedEndHour = "09:30";
        public string SelectedEndHour
        {
            get => _selectedEndHour;
            set { _selectedEndHour = value; OnPropertyChanged(); }
        }

        private ObservableCollection<string> _filteredEndHours = new ObservableCollection<string>();
        public ObservableCollection<string> FilteredEndHours
        {
            get => _filteredEndHours;
            set { _filteredEndHours = value; OnPropertyChanged(); }
        }

        private void UpdateEndHoursFilter()
        {
            var startIdx = StartHours.IndexOf(SelectedStartHour);
            if (startIdx >= 0)
            {
                var validEndHours = EndHours.Skip(startIdx).ToList();
                FilteredEndHours = new ObservableCollection<string>(validEndHours);
                if (!FilteredEndHours.Contains(SelectedEndHour))
                {
                    SelectedEndHour = FilteredEndHours.FirstOrDefault() ?? "18:00";
                }
            }
        }

        // Matrix Filtering
        private short _selectedYear = 2026;
        public short SelectedYear
        {
            get => _selectedYear;
            set
            {
                _selectedYear = value;
                OnPropertyChanged();
                RefreshMatrix();
            }
        }

        private byte _selectedQuarter = 3; // Default to Q3 as seeded
        public byte SelectedQuarter
        {
            get => _selectedQuarter;
            set
            {
                _selectedQuarter = value;
                OnPropertyChanged();
                RefreshMatrix();
            }
        }

        public List<short> Years { get; } = new List<short> { 2025, 2026, 2027 };
        public List<byte> Quarters { get; } = new List<byte> { 1, 2, 3, 4 };

        // Matrix Output Binding
        private List<MatrixDayHeader> _daysInQuarter = new List<MatrixDayHeader>();
        public List<MatrixDayHeader> DaysInQuarter
        {
            get => _daysInQuarter;
            set { _daysInQuarter = value; OnPropertyChanged(); }
        }

        private List<MonthHeaderItem> _monthHeaders = new List<MonthHeaderItem>();
        public List<MonthHeaderItem> MonthHeaders
        {
            get => _monthHeaders;
            set { _monthHeaders = value; OnPropertyChanged(); }
        }

        private ObservableCollection<TeamGroup> _matrixGroups = new ObservableCollection<TeamGroup>();
        public ObservableCollection<TeamGroup> MatrixGroups
        {
            get => _matrixGroups;
            set { _matrixGroups = value; OnPropertyChanged(); }
        }

        private string _statusMessage = string.Empty;
        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                _statusMessage = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasStatusMessage));
            }
        }

        public bool HasStatusMessage => !string.IsNullOrWhiteSpace(StatusMessage);


        private bool _isCurrentUserAdmin;
        public bool IsCurrentUserAdmin
        {
            get => _isCurrentUserAdmin;
            set
            {
                _isCurrentUserAdmin = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(AdminTabVisibility));
                OnPropertyChanged(nameof(IsUserSelectorEnabled));
            }
        }

        public bool IsUserSelectorEnabled => IsCurrentUserAdmin;

        public void AutoSelectCurrentUser()
        {
            if (UsersList == null || !UsersList.Any()) return;

            var matched = UsersList.FirstOrDefault(u => string.Equals(u.FullName, CurrentUserName, StringComparison.OrdinalIgnoreCase));
            if (matched != null)
            {
                SelectedUser = matched;
            }
            else if (UsersList.Any())
            {
                SelectedUser = UsersList.First();
            }
        }

        private int CurrentUserId
        {
            get
            {
                var found = UsersList.FirstOrDefault(u => u.FullName.Equals(CurrentUserName, StringComparison.OrdinalIgnoreCase));
                return found?.Id ?? 0;
            }
        }

        public bool IsSaveButtonEnabled => true;

        public Visibility AdminTabVisibility => IsCurrentUserAdmin ? Visibility.Visible : Visibility.Collapsed;

        public string SaveButtonText => SelectedLeave == null ? "✅ İzin Gir" : "✅ İzni Güncelle";

        public string FormTitleText => SelectedLeave == null ? "İzin Giriş Paneli" : "İzin Düzenleme Paneli";

        // ── Akıllı İzin Öneri Asistanı Properties ─────────────────────────
        private readonly SmartLeaveRecommendationService _aiRecommendationService = new SmartLeaveRecommendationService();

        private int _aiDesiredWorkingDays = 5;
        public int AiDesiredWorkingDays
        {
            get => _aiDesiredWorkingDays;
            set { _aiDesiredWorkingDays = value; OnPropertyChanged(); }
        }

        private int _aiSelectedTargetMonth = DateTime.Now.Month;
        public int AiSelectedTargetMonth
        {
            get => _aiSelectedTargetMonth;
            set { _aiSelectedTargetMonth = value; OnPropertyChanged(); }
        }

        private int _aiSelectedTargetYear = DateTime.Now.Year;
        public int AiSelectedTargetYear
        {
            get => _aiSelectedTargetYear;
            set { _aiSelectedTargetYear = value; OnPropertyChanged(); }
        }

        private ObservableCollection<LeaveRecommendationOption> _aiRecommendations = new ObservableCollection<LeaveRecommendationOption>();
        public ObservableCollection<LeaveRecommendationOption> AiRecommendations
        {
            get => _aiRecommendations;
            set
            {
                _aiRecommendations = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasAiRecommendations));
            }
        }

        public bool HasAiRecommendations => AiRecommendations != null && AiRecommendations.Count > 0;

        private bool _isAiCardExpanded = false;
        public bool IsAiCardExpanded
        {
            get => _isAiCardExpanded;
            set { _isAiCardExpanded = value; OnPropertyChanged(); }
        }

        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand ClearFormCommand { get; }
        public ICommand EditLeaveCommand { get; }

        public ICommand SelectTabCommand { get; }
        public ICommand GetAIRecommendationsCommand { get; }
        public ICommand ApplyRecommendationCommand { get; }
        public ICommand ToggleAiCardCommand { get; }

        private int _selectedTabIndex = 0;
        public int SelectedTabIndex
        {
            get => _selectedTabIndex;
            set
            {
                _selectedTabIndex = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsMatrixTabVisible));
                OnPropertyChanged(nameof(IsFormListTabVisible));
            }
        }

        public Visibility IsMatrixTabVisible => SelectedTabIndex == 0 ? Visibility.Visible : Visibility.Collapsed;
        public Visibility IsFormListTabVisible => SelectedTabIndex == 1 ? Visibility.Visible : Visibility.Collapsed;

        public LeavesViewModel() : this(false, string.Empty, 0) { }

        public LeavesViewModel(bool isCurrentUserAdmin, string currentUserName = "", int initialTabIndex = 0)
        {
            IsCurrentUserAdmin = isCurrentUserAdmin;
            CurrentUserName = currentUserName;
            SelectedTabIndex = initialTabIndex;

            SelectTabCommand = new RelayCommand(param =>
            {
                if (param != null && int.TryParse(param.ToString(), out int idx))
                {
                    SelectedTabIndex = idx;
                }
            });

            SaveCommand = new RelayCommand(ExecuteSave);
            DeleteCommand = new RelayCommand(ExecuteDelete, CanDelete);
            ClearFormCommand = new RelayCommand(ExecuteClearForm);
            EditLeaveCommand = new RelayCommand(ExecuteEditLeave);

            GetAIRecommendationsCommand = new RelayCommand(ExecuteGetAIRecommendations);
            ApplyRecommendationCommand = new RelayCommand(ExecuteApplyRecommendation);
            ToggleAiCardCommand = new RelayCommand(_ => { IsAiCardExpanded = !IsAiCardExpanded; });

            UpdateEndHoursFilter();
            LoadData();
        }

        private void ExecuteGetAIRecommendations(object? param)
        {
            try
            {
                var users = _services.GetAllUsers();
                var leaves = _services.GetAllLeaves();
                var shifts = _services.GetAllShifts();
                var currentUser = users.FirstOrDefault(u => string.Equals(u.FullName, CurrentUserName, StringComparison.OrdinalIgnoreCase));

                if (currentUser == null)
                {
                    currentUser = users.FirstOrDefault() ?? new User { Name = "Personel", Team = "Takip", Title = "Developer" };
                }

                var recommendations = _aiRecommendationService.GetRecommendations(
                    currentUser,
                    AiDesiredWorkingDays,
                    AiSelectedTargetYear,
                    AiSelectedTargetMonth,
                    users,
                    leaves,
                    shifts);

                AiRecommendations = new ObservableCollection<LeaveRecommendationOption>(recommendations);
                IsAiCardExpanded = true;
                if (!HasAiRecommendations)
                {
                    StatusMessage = "🤖 Belirtilen ay için uygun izin önerisi bulunamadı.";
                }
                else
                {
                    StatusMessage = $"🤖 {AiRecommendations.Count} adet akıllı izin önerisi oluşturuldu!";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Akıllı Öneri oluşturulurken hata: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteApplyRecommendation(object? param)
        {
            if (param is LeaveRecommendationOption option)
            {
                StartDate = option.StartDate;
                EndDate = option.EndDate;
                IsHourly = false;
                StatusMessage = $"🤖 Akıllı Asistan Önerisi Formda Seçildi: {option.FormattedRange}";
            }
        }

        private void LoadData()
        {
            try
            {
                var allUsers = _services.GetAllUsers();
                UsersList = new ObservableCollection<User>(allUsers);
                AutoSelectCurrentUser();
                var allLeaves = _services.GetAllLeaves();
                LeavesList = new ObservableCollection<Leave>(allLeaves);

                Func<Leave, bool> isNotExpired = l =>
                {
                    bool isHourly = l.StartDate.TimeOfDay != TimeSpan.Zero || l.EndDate.TimeOfDay != TimeSpan.Zero;
                    return isHourly ? l.EndDate >= DateTime.Now : l.EndDate.Date >= DateTime.Today;
                };

                // Active approved leaves for team sections
                var approvedLeaves = allLeaves.Where(l =>
                    l.Status != "Rejected" && l.Status != "Reddedildi" && l.Status != "Cancelled"
                ).ToList();
                var activeLeaves = approvedLeaves.Where(isNotExpired).ToList();

                DepartmanLeaves = new ObservableCollection<Leave>(activeLeaves.Where(l => l.User != null && (string.Equals(l.User.Team, "Departman Yönetimi", StringComparison.OrdinalIgnoreCase) || string.Equals(l.User.Team, "Yönetim", StringComparison.OrdinalIgnoreCase))));
                TakipLeaves = new ObservableCollection<Leave>(activeLeaves.Where(l => l.User != null && string.Equals(l.User.Team, "Takip", StringComparison.OrdinalIgnoreCase)));
                TahsisLeaves = new ObservableCollection<Leave>(activeLeaves.Where(l => l.User != null && string.Equals(l.User.Team, "Tahsis", StringComparison.OrdinalIgnoreCase)));
                TeminatLeaves = new ObservableCollection<Leave>(activeLeaves.Where(l => l.User != null && string.Equals(l.User.Team, "Teminat", StringComparison.OrdinalIgnoreCase)));

                // Populate Leave History Archive (Past completed leaves)
                UpdateHistoryLeaves();

                // Pre-select current user in form
                if (SelectedUserId == 0 && !string.IsNullOrWhiteSpace(CurrentUserName))
                {
                    var self = allUsers.FirstOrDefault(u => u.FullName.Equals(CurrentUserName, StringComparison.OrdinalIgnoreCase));
                    if (self != null) SelectedUserId = self.Id;
                }

                RefreshMatrix();
            }
            catch (Exception ex)
            {
                StatusMessage = $"Yükleme Hatası: {ex.Message}";
            }
        }

        private void LoadSelectedLeave()
        {
            if (SelectedLeave != null)
            {
                SelectedUserId = SelectedLeave.UserId;
                bool isHourly = (SelectedLeave.StartDate.TimeOfDay != TimeSpan.Zero || SelectedLeave.EndDate.TimeOfDay != TimeSpan.Zero);
                IsHourly = isHourly;
                if (isHourly)
                {
                    HourlyDate = SelectedLeave.StartDate.Date;
                    SelectedStartHour = SelectedLeave.StartDate.ToString("HH:mm");
                    SelectedEndHour = SelectedLeave.EndDate.ToString("HH:mm");
                }
                else
                {
                    StartDate = SelectedLeave.StartDate;
                    EndDate = SelectedLeave.EndDate;
                }
            }
            else
            {
                ExecuteClearForm(null);
            }
        }

        private void ExecuteEditLeave(object? param)
        {
            if (param is Leave leave)
            {
                SelectedLeave = leave;
                SelectedTabIndex = 1; // İzin Talebi ve Listeler sekmesine geç
                StatusMessage = $"✏️ '{leave.User?.FullName}' personeli için izin kaydı düzenleme moduna alındı. Tarihleri güncelleyip aşağıdaki buton ile kaydedebilirsiniz.";
            }
        }

        private void ExecuteSave(object? param)
        {
            try
            {
                int targetUserId = SelectedUserId;
                if (targetUserId <= 0 && !string.IsNullOrWhiteSpace(CurrentUserName))
                {
                    var found = UsersList.FirstOrDefault(u => u.FullName.Equals(CurrentUserName, StringComparison.OrdinalIgnoreCase));
                    if (found != null) targetUserId = found.Id;
                }

                if (targetUserId <= 0)
                {
                    MessageBox.Show("Lütfen izin talebi için bir personel seçiniz.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                DateTime finalStartDate = StartDate;
                DateTime finalEndDate = EndDate;

                if (IsHourly)
                {
                    TimeSpan startTime = TimeSpan.Parse(SelectedStartHour);
                    TimeSpan endTime = TimeSpan.Parse(SelectedEndHour);
                    finalStartDate = HourlyDate.Date + startTime;
                    finalEndDate = HourlyDate.Date + endTime;

                    if (finalEndDate <= finalStartDate)
                    {
                        MessageBox.Show("İzin bitiş saati başlangıç saatinden sonra olmalıdır.", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    if (finalStartDate < DateTime.Now)
                    {
                        MessageBox.Show("Geçmiş bir saat için izin talebi oluşturamazsınız. Lütfen gelecekteki bir zamanı seçiniz.", "Geçmiş Zaman Uyarısı", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }
                else
                {
                    finalStartDate = StartDate.Date;
                    finalEndDate = EndDate.Date;

                    if (finalEndDate < finalStartDate)
                    {
                        MessageBox.Show("İzin bitiş tarihi başlangıç tarihinden önce olmalıdır.", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }

                // Confirmation prompt before submitting/saving
                var targetUser = UsersList.FirstOrDefault(u => u.Id == targetUserId);
                string targetName = targetUser?.FullName ?? CurrentUserName;
                string dateRange = IsHourly 
                    ? $"{HourlyDate:dd.MM.yyyy} Saatlik İzin ({SelectedStartHour} - {SelectedEndHour})"
                    : $"{StartDate:dd.MM.yyyy} - {EndDate:dd.MM.yyyy}";

                string confirmMsg = SelectedLeave == null
                    ? $"📋 YENİ İZİN KAYDI\n\n" +
                      $"Personel       : {targetName}\n" +
                      $"Tarih/Saat     : {dateRange}\n\n" +
                      $"İzin sisteme kaydedilecektir. Onaylıyor musunuz?"
                    : $"✏️ İZİN GÜNCELLEMESİ\n\n" +
                      $"Personel       : {targetName}\n" +
                      $"Yeni Tarih/Saat: {dateRange}\n\n" +
                      $"İzin güncellencektir. Onaylıyor musunuz?";

                // CHECK FOR ACTIVE SHIFT CONFLICT
                var nonFinishedMonthly = _services.GetAllMonthlyReleaseShifts().Where(s => !s.IsFinished).ToList();
                var nonFinishedCustom = _services.GetAllCustomShifts().Where(c => !c.IsFinished).ToList();

                var conflictingMonthly = nonFinishedMonthly.FirstOrDefault(s =>
                    !string.IsNullOrWhiteSpace(s.AssignedUsers) &&
                    s.AssignedUsers.Contains(targetName, StringComparison.OrdinalIgnoreCase) &&
                    s.ReleaseDate.Date >= finalStartDate.Date && s.ReleaseDate.Date <= finalEndDate.Date
                );

                var conflictingCustom = nonFinishedCustom.FirstOrDefault(c =>
                    !string.IsNullOrWhiteSpace(c.AssignedUsers) &&
                    c.AssignedUsers.Contains(targetName, StringComparison.OrdinalIgnoreCase) &&
                    c.ShiftDate.Date >= finalStartDate.Date && c.ShiftDate.Date <= finalEndDate.Date
                );

                if (conflictingMonthly != null || conflictingCustom != null)
                {
                    string shiftName = conflictingMonthly != null ? conflictingMonthly.MonthName : conflictingCustom!.Topic;
                    DateTime shiftDate = conflictingMonthly != null ? conflictingMonthly.ReleaseDate : conflictingCustom!.ShiftDate;

                    var warnResult = MessageBox.Show(
                        $"⚠️ DİKKAT: NÖBET ÇAKIŞMASI UYARISI!\n\n" +
                        $"'{targetName}' personeline izin almak istediğiniz tarihler arasında ({shiftDate:dd.MM.yyyy}) aktif bir NÖBET KAYDI ({shiftName}) atanmış görünmektedir!\n\n" +
                        $"Nöbetinizin olduğu bu tarihte izin talebinize devam etmek istediğinizden emin misiniz?",
                        "Nöbet Çakışması Uyarısı",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning
                    );

                    if (warnResult != MessageBoxResult.Yes) return;
                }

                var confirmDlg = MessageBox.Show(confirmMsg, "İşlem Onayı", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (confirmDlg != MessageBoxResult.Yes) return;

                // Check if user already has a pending or approved leave for overlapping dates
                var existingLeave = _services.GetExistingLeaveForUser(targetUserId, finalStartDate, finalEndDate, SelectedLeave?.Id);

                if (existingLeave != null && SelectedLeave == null)
                {
                    SelectedLeave = existingLeave;
                    if (string.Equals(existingLeave.Status, "Pending", StringComparison.OrdinalIgnoreCase))
                    {
                        StatusMessage = $"⚠️ {finalStartDate:dd.MM.yyyy HH:mm} - {finalEndDate:dd.MM.yyyy HH:mm} tarihlerinde zaten bekleyen bir talebiniz var! Form, bu talebi güncellemeniz için 'Talebi Güncelle (Değişiklik Yap)' moduna geçirilmiştir.";
                    }
                    else
                    {
                        StatusMessage = $"⚠️ {finalStartDate:dd.MM.yyyy HH:mm} - {finalEndDate:dd.MM.yyyy HH:mm} tarihlerinde zaten onaylı bir izniniz var! Değişiklik yapmak için 'İzin Değişiklik Talebi Gönder (Re-Onay)' butonunu kullanabilirsiniz.";
                    }
                    return;
                }

                // Check 50% Role Capacity Overlap Rule (uyarı ver ama izni yine de kaydet)
                var checkResult = _services.CheckLeaveRoleCapacityOverlap(targetUserId, finalStartDate, finalEndDate, SelectedLeave?.Id);

                bool isSpecialRequest = false;
                string requestNote = string.Empty;

                if (checkResult.Has50PercentOverlap)
                {
                    var dlg = MessageBox.Show(
                        $"⚠️ DİKKAT: ROL İZİN ÇAKIŞMASI (%50+)\n\n" +
                        $"{checkResult.DetailedMessage}\n\n" +
                        $"İzni yine de kaydetmek istiyor musunuz?",
                        "Kapasite Çakışması Uyarısı",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning);

                    if (dlg != MessageBoxResult.Yes) return;
                    isSpecialRequest = true;
                    requestNote = checkResult.DetailedMessage;
                }

                string resultMessage = string.Empty;

                if (SelectedLeave == null)
                {
                    var newLeave = new Leave
                    {
                        UserId = targetUserId,
                        StartDate = finalStartDate,
                        EndDate = finalEndDate,
                        Status = "Approved",
                        IsSpecialRequest = isSpecialRequest,
                        RequestNote = requestNote,
                        RequestedAt = DateTime.Now,
                        ApprovedAt = DateTime.Now,
                        ApprovedByUserName = CurrentUserName,
                        IsNotificationSeen = true
                    };

                    _services.AddLeave(newLeave);
                    resultMessage = isSpecialRequest
                        ? "✅ İzin kaydedildi. (Not: %50 kapasite çakışması mevcut)"
                        : "✅ İzin başarıyla kaydedildi.";
                }
                else
                {
                    SelectedLeave.UserId = targetUserId;
                    SelectedLeave.StartDate = finalStartDate;
                    SelectedLeave.EndDate = finalEndDate;
                    SelectedLeave.Status = "Approved";
                    SelectedLeave.IsSpecialRequest = isSpecialRequest;
                    SelectedLeave.ApprovedByUserName = CurrentUserName;
                    SelectedLeave.ApprovedAt = DateTime.Now;
                    SelectedLeave.IsNotificationSeen = true;
                    if (!string.IsNullOrWhiteSpace(requestNote))
                        SelectedLeave.RequestNote = requestNote;

                    _services.UpdateLeave(SelectedLeave);
                    resultMessage = "✅ İzin kaydı güncellendi.";
                }

                LoadData();
                ExecuteClearForm(null);
                StatusMessage = resultMessage;
            }
            catch (ValidationException vex)
            {
                MessageBox.Show(vex.Message, "Doğrulama Hatası", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"İzin kaydetme hatası: {ex.Message}", "Sistem Hatası", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void ExecuteDelete(object? param)
        {
            if (SelectedLeave == null) return;

            var result = MessageBox.Show("Bu izin kaydını iptal etmek/silmek istediğinize emin misiniz?", "İzin İptal", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _services.DeleteLeave(SelectedLeave.Id, CurrentUserName);
                    StatusMessage = "İzin kaydı iptal edildi ve personeline bildirim gönderildi.";
                    LoadData();
                    ExecuteClearForm(null);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"İzin iptal hatası: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private bool CanDelete(object? param)
        {
            return SelectedLeave != null;
        }

        private void ExecuteClearForm(object? param)
        {
            SelectedLeave = null;
            SelectedUserId = 0;
            // Pre-select current user for both admin and non-admin (admin can still change it via ComboBox)
            if (!string.IsNullOrWhiteSpace(CurrentUserName))
            {
                var self = UsersList.FirstOrDefault(u => u.FullName.Equals(CurrentUserName, StringComparison.OrdinalIgnoreCase));
                if (self != null) SelectedUserId = self.Id;
            }
            StartDate = DateTime.Today;
            EndDate = DateTime.Today;
            HourlyDate = DateTime.Today;
            IsHourly = false;
            SelectedStartHour = "09:00";
            SelectedEndHour = "09:30";
        }

        // Generate the leaves matrix for the selected Year/Quarter
        public void RefreshMatrix()
        {
            try
            {
                int startMonth = ((SelectedQuarter - 1) * 3) + 1;
                var quarterStart = new DateTime(SelectedYear, startMonth, 1);
                var quarterEnd = quarterStart.AddMonths(3).AddDays(-1);

                var days = new List<MatrixDayHeader>();
                for (var day = quarterStart; day <= quarterEnd; day = day.AddDays(1))
                {
                    days.Add(new MatrixDayHeader { Date = day });
                }
                DaysInQuarter = days;

                var monthHeadersList = new List<MonthHeaderItem>();
                string[] trMonthNames = new[] { "", "OCAK", "ŞUBAT", "MART", "NİSAN", "MAYIS", "HAZİRAN", "TEMMUZ", "AĞUSTOS", "EYLÜL", "EKİM", "KASIM", "ARALIK" };

                for (int m = 0; m < 3; m++)
                {
                    int mNum = startMonth + m;
                    int daysInMonth = DateTime.DaysInMonth(SelectedYear, mNum);
                    monthHeadersList.Add(new MonthHeaderItem
                    {
                        MonthName = $"{trMonthNames[mNum]} {SelectedYear}",
                        DayCount = daysInMonth,
                        BackgroundColor = "#f8fafc",
                        BorderColor = "#cbd5e0",
                        TextColor = "#2d3748"
                    });
                }
                MonthHeaders = monthHeadersList;

                var users = _services.GetAllUsers();
                // Include both Approved and Pending daily leaves in Matrix display (Exclude hourly leaves from visual calendar matrix)
                var allLeaves = _services.GetAllLeaves().Where(l => 
                    l.Status != "Rejected" &&
                    l.StartDate.TimeOfDay == TimeSpan.Zero && l.EndDate.TimeOfDay == TimeSpan.Zero &&
                    (l.StartDate.Date <= quarterEnd && l.EndDate.Date >= quarterStart)).ToList();

                var groupsList = new List<TeamGroup>
                {
                    new TeamGroup { TeamName = "Departman Yönetimi" },
                    new TeamGroup { TeamName = "Takip" },
                    new TeamGroup { TeamName = "Tahsis" },
                    new TeamGroup { TeamName = "Teminat" }
                };

                foreach (var group in groupsList)
                {
                    var teamUsers = users.Where(u => string.Equals(u.Team, group.TeamName, StringComparison.OrdinalIgnoreCase));
                    foreach (var u in teamUsers)
                    {
                        var row = new MatrixRow
                        {
                            UserId = u.Id,
                            UserName = u.FullName,
                            UserTitle = u.Title,
                            Team = u.Team
                        };

                        var userLeaves = allLeaves.Where(l => l.UserId == u.Id).ToList();

                        foreach (var dayHeader in DaysInQuarter)
                        {
                            var d = dayHeader.Date;
                            bool isWeekend = dayHeader.IsWeekend;
                            var activeLeave = userLeaves.FirstOrDefault(l => d.Date >= l.StartDate.Date && d.Date <= l.EndDate.Date);

                            // Weekend days are non-working, so they are not painted with leave colors
                            bool isOnLeave = activeLeave != null && !isWeekend;

                            string status = activeLeave?.Status ?? "Approved";
                            bool isSpecial = activeLeave?.IsSpecialRequest ?? false;

                            bool isHourly = false;
                            string hourlyRange = string.Empty;
                            if (activeLeave != null)
                            {
                                isHourly = (activeLeave.StartDate.TimeOfDay != TimeSpan.Zero || activeLeave.EndDate.TimeOfDay != TimeSpan.Zero);
                                if (isHourly)
                                {
                                    hourlyRange = $"{activeLeave.StartDate:HH:mm} - {activeLeave.EndDate:HH:mm}";
                                }
                            }

                            string statusLabel;
                            if (isHourly)
                            {
                                statusLabel = status == "Pending"
                                    ? $" (⏳ Saatlik İzin Talebi {hourlyRange} - Onay Bekliyor)"
                                    : $" (Saatlik İzin {hourlyRange})";
                            }
                            else
                            {
                                statusLabel = status == "Pending"
                                    ? (isSpecial ? " (⏳ Özel İzin Talebi - Onay Bekliyor)" : " (⏳ İzin Talebi - Onay Bekliyor)")
                                    : (isSpecial ? " (⭐ Özel İzin - Onaylandı)" : " (İzinli)");
                            }

                            var holidayInfo = TurkeyHolidays.GetHolidayInfo(d);
                            bool isHoliday = holidayInfo.IsHoliday;

                            string tooltip;
                            if (isWeekend)
                            {
                                tooltip = $"{u.FullName} - {d:dd.MM.yyyy} (Hafta Sonu - Tatil)";
                                if (isHoliday) tooltip += $" [Resmi Tatil: {holidayInfo.Name}]";
                            }
                            else if (isOnLeave)
                            {
                                string holidayTag = isHoliday ? $" [Resmi Tatil: {holidayInfo.Name}]" : "";
                                if (isHourly)
                                {
                                    tooltip = $"{u.FullName} - {d:dd.MM.yyyy}{statusLabel}{holidayTag}";
                                }
                                else
                                {
                                    tooltip = $"{u.FullName} ({activeLeave!.StartDate:dd.MM.yyyy} - {activeLeave.EndDate:dd.MM.yyyy}{statusLabel}){holidayTag}";
                                }
                            }
                            else if (isHoliday)
                            {
                                tooltip = $"{u.FullName} - {d:dd.MM.yyyy} (Resmi Tatil: {holidayInfo.Name})";
                            }
                            else
                            {
                                tooltip = $"{u.FullName} - {d:dd.MM.yyyy} (Çalışıyor)";
                            }

                            row.Cells.Add(new MatrixCell
                            {
                                Date = d,
                                IsOnLeave = isOnLeave,
                                IsHourly = isHourly,
                                Status = status,
                                IsSpecialRequest = isSpecial,
                                TooltipText = tooltip,
                                TeamName = u.Team
                            });
                        }

                        group.Rows.Add(row);
                    }
                }

                MatrixGroups = new ObservableCollection<TeamGroup>(groupsList);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Matrix Error: {ex.Message}");
            }
        }
    }
}
