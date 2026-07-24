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

    public class NotificationItemModel
    {
        public int LeaveId { get; set; }
        public string Type { get; set; } = "Approved"; // "Approved", "Updated", "Rejected", "Cancelled"
        public string Icon { get; set; } = "✅";
        public string Title { get; set; } = "İZİN TALEBİ BİLDİRİMİ";
        public string Message { get; set; } = string.Empty;

        public string BackgroundColor => Type switch
        {
            "Approved" => "#dcfce7",  // Açık yeşil (Onaylandı)
            "Updated" => "#fef3c7",   // Canlı Açık Sarı (Yönetici Güncelledi)
            "Rejected" => "#fee2e2",  // Açık kırmızı (Reddedildi)
            "Cancelled" => "#ffedd5", // Açık turuncu (İptal edildi)
            _ => "#dcfce7"
        };

        public string BorderColor => Type switch
        {
            "Approved" => "#16a34a",
            "Updated" => "#f59e0b",  // Canlı Kehribar Sarı Border
            "Rejected" => "#dc2626",
            "Cancelled" => "#ea580c",
            _ => "#16a34a"
        };

        public string HeaderTextColor => Type switch
        {
            "Approved" => "#14532d",
            "Updated" => "#92400e",  // Koyu Kehribar Sarı Başlık
            "Rejected" => "#991b1b",
            "Cancelled" => "#9a3412",
            _ => "#14532d"
        };

        public string BodyTextColor => Type switch
        {
            "Approved" => "#166534",
            "Updated" => "#78350f",  // Koyu Kahve/Sarı Gövde Metni
            "Rejected" => "#7f1d1d",
            "Cancelled" => "#c2410c",
            _ => "#166534"
        };

        public string ButtonBackground => Type switch
        {
            "Approved" => "#bbf7d0",
            "Updated" => "#fde047",  // Canlı Sarı Buton
            "Rejected" => "#fecaca",
            "Cancelled" => "#fed7aa",
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
            set { _currentUserName = value; OnPropertyChanged(); }
        }

        // Data lists
        private ObservableCollection<Leave> _leavesList = new ObservableCollection<Leave>();
        public ObservableCollection<Leave> LeavesList
        {
            get => _leavesList;
            set { _leavesList = value; OnPropertyChanged(); }
        }

        private ObservableCollection<Leave> _pendingLeaves = new ObservableCollection<Leave>();
        public ObservableCollection<Leave> PendingLeaves
        {
            get => _pendingLeaves;
            set 
            { 
                _pendingLeaves = value; 
                OnPropertyChanged(); 
                OnPropertyChanged(nameof(HasPendingLeaves)); 
                OnPropertyChanged(nameof(IsSaveButtonEnabled));
            }
        }

        public bool HasPendingLeaves => PendingLeaves != null && PendingLeaves.Count > 0;

        private ObservableCollection<Leave> _myPendingLeaves = new ObservableCollection<Leave>();
        public ObservableCollection<Leave> MyPendingLeaves
        {
            get => _myPendingLeaves;
            set
            {
                _myPendingLeaves = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(MyPendingLeavesVisibility));
                OnPropertyChanged(nameof(IsSaveButtonEnabled));
            }
        }

        public Visibility MyPendingLeavesVisibility =>
            (!IsCurrentUserAdmin && MyPendingLeaves != null && MyPendingLeaves.Count > 0)
                ? Visibility.Visible : Visibility.Collapsed;


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

        // Form Fields
        private int _selectedUserId;
        public int SelectedUserId
        {
            get => _selectedUserId;
            set { _selectedUserId = value; OnPropertyChanged(); }
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

        // ── Notification Banner Properties ──────────────────────────────────────
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

        public Visibility NotificationVisible =>
            (NotificationList != null && NotificationList.Count > 0) ? Visibility.Visible : Visibility.Collapsed;

        public ICommand DismissNotificationCommand { get; private set; } = new RelayCommand(_ => { });

        private bool _isCurrentUserAdmin;
        public bool IsCurrentUserAdmin
        {
            get => _isCurrentUserAdmin;
            set
            {
                _isCurrentUserAdmin = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(AdminTabVisibility));
                OnPropertyChanged(nameof(SaveButtonText));
                OnPropertyChanged(nameof(FormTitleText));
                OnPropertyChanged(nameof(IsSaveButtonEnabled));
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

        public bool IsSaveButtonEnabled
        {
            get
            {
                if (IsCurrentUserAdmin) return true;
                if (SelectedLeave != null) return true;

                // Non-admin with no leave selected: check if user already has a pending leave request!
                int myId = CurrentUserId;
                bool hasPending = PendingLeaves.Any(l => l.UserId == myId || (l.User != null && string.Equals(l.User.FullName, CurrentUserName, StringComparison.OrdinalIgnoreCase)));
                return !hasPending;
            }
        }

        public Visibility AdminTabVisibility => IsCurrentUserAdmin ? Visibility.Visible : Visibility.Collapsed;
        public string SaveButtonText
        {
            get
            {
                if (IsCurrentUserAdmin)
                {
                    return SelectedLeave == null ? "İzin Kaydet & Onayla" : "İznini Güncelle & Onayla";
                }
                else
                {
                    if (SelectedLeave == null) return "İzin Talebi Gönder";
                    if (string.Equals(SelectedLeave.Status, "Pending", StringComparison.OrdinalIgnoreCase))
                        return "Değişiklikleri Kaydet (Talep Güncelle)";
                    return "İzin Değişiklik Talebi Gönder (Re-Onay)";
                }
            }
        }

        public string FormTitleText
        {
            get
            {
                if (IsCurrentUserAdmin)
                {
                    return SelectedLeave == null ? "İzin Kayıt & Onay Paneli" : "İzin Kaydı Düzenleme Paneli";
                }
                else
                {
                    if (SelectedLeave == null) return "Yeni İzin Talep Paneli";
                    if (string.Equals(SelectedLeave.Status, "Pending", StringComparison.OrdinalIgnoreCase))
                        return "Bekleyen İzin Talebini Düzenle";
                    return "Onaylı İzni Değiştirme Talebi";
                }
            }
        }
        public bool IsUserSelectorEnabled => IsCurrentUserAdmin;

        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand ClearFormCommand { get; }
        public ICommand ApproveCommand { get; }
        public ICommand RejectCommand { get; }
        public ICommand EditLeaveCommand { get; }

        public ICommand SelectTabCommand { get; }

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

        private readonly System.Windows.Threading.DispatcherTimer _expiryTimer;

        public LeavesViewModel() : this(false, string.Empty, 0) { }

        public LeavesViewModel(bool isCurrentUserAdmin, string currentUserName = "", int initialTabIndex = 0)
        {
            IsCurrentUserAdmin = isCurrentUserAdmin;
            CurrentUserName = currentUserName;
            SelectedTabIndex = initialTabIndex;

            _expiryTimer = new System.Windows.Threading.DispatcherTimer { Interval = TimeSpan.FromSeconds(30) };
            _expiryTimer.Tick += (s, e) => LoadData();
            _expiryTimer.Start();

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
            ApproveCommand = new RelayCommand(ExecuteApprove);
            RejectCommand = new RelayCommand(ExecuteReject);
            EditLeaveCommand = new RelayCommand(ExecuteEditLeave);
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
                _services.MarkNotificationsSeenForUser(CurrentUserName);
            });

            UpdateEndHoursFilter();
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                var allUsers = _services.GetAllUsers();
                UsersList = new ObservableCollection<User>(allUsers);
                var allLeaves = _services.GetAllLeaves();
                LeavesList = new ObservableCollection<Leave>(allLeaves);

                Func<Leave, bool> isNotExpired = l =>
                {
                    bool isHourly = l.StartDate.TimeOfDay != TimeSpan.Zero || l.EndDate.TimeOfDay != TimeSpan.Zero;
                    if (isHourly)
                    {
                        return l.EndDate >= DateTime.Now;
                    }
                    else
                    {
                        return l.EndDate.Date >= DateTime.Today;
                    }
                };

                // Filter Pending Leaves for Admin approval (only non-expired)
                PendingLeaves = new ObservableCollection<Leave>(allLeaves.Where(l => string.Equals(l.Status, "Pending", StringComparison.OrdinalIgnoreCase) && isNotExpired(l)));

                // Filter MY pending leaves (non-admin user sees their own active pending requests)
                MyPendingLeaves = new ObservableCollection<Leave>(
                    allLeaves.Where(l =>
                        string.Equals(l.Status, "Pending", StringComparison.OrdinalIgnoreCase) &&
                        isNotExpired(l) &&
                        l.User != null &&
                        l.User.FullName.Equals(CurrentUserName, StringComparison.OrdinalIgnoreCase)
                    ));

                // Approved leaves for active team sections (ONLY leaves where EndDate is active and not expired)
                var approvedLeaves = allLeaves.Where(l => string.IsNullOrEmpty(l.Status) || string.Equals(l.Status, "Approved", StringComparison.OrdinalIgnoreCase)).ToList();
                var activeApprovedLeaves = approvedLeaves.Where(isNotExpired).ToList();

                DepartmanLeaves = new ObservableCollection<Leave>(activeApprovedLeaves.Where(l => l.User != null && (string.Equals(l.User.Team, "Departman Yönetimi", StringComparison.OrdinalIgnoreCase) || string.Equals(l.User.Team, "Yönetim", StringComparison.OrdinalIgnoreCase))));
                TakipLeaves = new ObservableCollection<Leave>(activeApprovedLeaves.Where(l => l.User != null && string.Equals(l.User.Team, "Takip", StringComparison.OrdinalIgnoreCase)));
                TahsisLeaves = new ObservableCollection<Leave>(activeApprovedLeaves.Where(l => l.User != null && string.Equals(l.User.Team, "Tahsis", StringComparison.OrdinalIgnoreCase)));
                TeminatLeaves = new ObservableCollection<Leave>(activeApprovedLeaves.Where(l => l.User != null && string.Equals(l.User.Team, "Teminat", StringComparison.OrdinalIgnoreCase)));

                // Populate Leave History Archive (Past completed leaves)
                UpdateHistoryLeaves();

                // Pre-select current user in form (both admin and non-admin start with themselves)
                if (SelectedUserId == 0 && !string.IsNullOrWhiteSpace(CurrentUserName))
                {
                    var self = allUsers.FirstOrDefault(u => u.FullName.Equals(CurrentUserName, StringComparison.OrdinalIgnoreCase));
                    if (self != null) SelectedUserId = self.Id;
                }

                // Show notification banner list for users with unseen approve/reject/cancel/update results
                if (!string.IsNullOrWhiteSpace(CurrentUserName))
                {
                    var unseenNotifications = _services.GetUnseenNotificationsForUser(CurrentUserName);
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

                string confirmMsg;
                if (IsCurrentUserAdmin)
                {
                    if (SelectedLeave == null)
                        confirmMsg = $"📋 YENİ İZİN KAYDI\n\n" +
                                     $"Personel  : {targetName}\n" +
                                     $"Tarih/Saat Aralığı : {dateRange}\n\n" +
                                     $"Bu izin direkt ONAYLI olarak sisteme kaydedilecektir.\nOnaylıyor musunuz?";
                    else
                        confirmMsg = $"✏️ İZİN DEĞİŞİKLİĞİ\n\n" +
                                     $"Personel  : {targetName}\n" +
                                     $"Yeni Tarih/Saat : {dateRange}\n\n" +
                                     $"Bu değişiklik direkt ONAYLI olarak kaydedilecektir.\nOnaylıyor musunuz?";
                }
                else
                {
                    if (SelectedLeave == null)
                        confirmMsg = $"📤 İZİN TALEBİ GÖNDERİLECEK\n\n" +
                                     $"Tarih/Saat Aralığı : {dateRange}\n\n" +
                                     $"İzin talebiniz yöneticinizin onayına gönderilecektir.\nOnaylıyor musunuz?";
                    else
                        confirmMsg = $"🔄 İZİN DEĞİŞİKLİĞİ TALEBİ\n\n" +
                                     $"Yeni Tarih/Saat : {dateRange}\n\n" +
                                     $"Değişiklik talebi yöneticinizin yeniden onayına sunulacaktır.\nOnaylıyor musunuz?";
                }

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

                // Check 50% Role Capacity Overlap Rule
                var checkResult = _services.CheckLeaveRoleCapacityOverlap(targetUserId, finalStartDate, finalEndDate, SelectedLeave?.Id);

                bool isSpecialRequest = false;
                string requestNote = string.Empty;

                if (checkResult.Has50PercentOverlap)
                {
                    if (IsCurrentUserAdmin)
                    {
                        var dlg = MessageBox.Show(
                            $"⚠️ DİKKAT: ROL İZİN ÇAKIŞMASI UYARISI (%50+)\n\n" +
                            $"{checkResult.DetailedMessage}\n\n" +
                            $"Yönetici yetkinizle bu izni yine de doğrudan ONAYLAYIP kaydetmek istiyor musunuz?",
                            "Özel İzin Çakışma Uyarısı",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Warning);

                        if (dlg != MessageBoxResult.Yes) return;
                        isSpecialRequest = true;
                        requestNote = checkResult.DetailedMessage;
                    }
                    else
                    {
                        var dlg = MessageBox.Show(
                            $"⚠️ DİKKAT: ROL İZİN ÇAKIŞMASI (%50+)\n\n" +
                            $"{checkResult.DetailedMessage}\n\n" +
                            $"İş kuralları gereği ekibinizdeki aynı rolün %50'sinden fazlası bu tarihlerde izinli olacağı için doğrudan izin oluşturamazsınız.\n\n" +
                            $"Talebinizi 'ÖZEL İZİN TALEBİ' olarak Yöneticinizin onayına göndermek istiyor musunuz?",
                            "Özel İzin Talebi Oluşturma",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Question);

                        if (dlg != MessageBoxResult.Yes) return;
                        isSpecialRequest = true;
                        requestNote = checkResult.DetailedMessage;
                    }
                }

                string resultMessage = string.Empty;

                if (SelectedLeave == null)
                {
                    var newLeave = new Leave
                    {
                        UserId = targetUserId,
                        StartDate = finalStartDate,
                        EndDate = finalEndDate,
                        Status = IsCurrentUserAdmin ? "Approved" : "Pending",
                        IsSpecialRequest = isSpecialRequest,
                        RequestNote = requestNote,
                        RequestedAt = DateTime.Now,
                        ApprovedAt = IsCurrentUserAdmin ? DateTime.Now : null,
                        ApprovedByUserName = IsCurrentUserAdmin ? CurrentUserName : null,
                        IsNotificationSeen = IsCurrentUserAdmin ? (targetUserId == CurrentUserId) : true
                    };

                    _services.AddLeave(newLeave);

                    if (IsCurrentUserAdmin)
                        resultMessage = isSpecialRequest ? "Özel izin kaydı yönetici tarafından onaylanarak eklendi." : "İzin kaydı başarıyla eklendi.";
                    else
                        resultMessage = isSpecialRequest ? "⚠️ Özel izin talebiniz çakışma notuyla yönetici onayına gönderildi." : "⏳ İzin talebiniz başarıyla oluşturuldu! Yönetici onayı bekleniyor.";
                }
                else
                {
                    bool wasApproved = string.Equals(SelectedLeave.Status, "Approved", StringComparison.OrdinalIgnoreCase);

                    SelectedLeave.UserId = targetUserId;
                    SelectedLeave.StartDate = finalStartDate;
                    SelectedLeave.EndDate = finalEndDate;
                    SelectedLeave.IsSpecialRequest = isSpecialRequest;

                    if (IsCurrentUserAdmin)
                    {
                        SelectedLeave.Status = "Approved";
                        SelectedLeave.ApprovedByUserName = CurrentUserName;
                        SelectedLeave.ApprovedAt = DateTime.Now;
                        SelectedLeave.IsNotificationSeen = (targetUserId == CurrentUserId); // false for target user if changed by admin
                        if (wasApproved || targetUserId != CurrentUserId)
                        {
                            SelectedLeave.RequestNote = $"🔄 Yöneticiniz ({CurrentUserName}) izninizi güncelledi ({finalStartDate:dd.MM.yyyy HH:mm} - {finalEndDate:dd.MM.yyyy HH:mm})";
                        }
                        else if (!string.IsNullOrWhiteSpace(requestNote))
                        {
                            SelectedLeave.RequestNote = requestNote;
                        }
                        resultMessage = "✅ İzin kaydı yönetici yetkisi ile güncellendi ve onaylandı.";
                    }
                    else
                    {
                        SelectedLeave.Status = "Pending"; // Reset status to Pending for Admin re-approval!
                        SelectedLeave.RequestedAt = DateTime.Now;

                        if (wasApproved)
                        {
                            SelectedLeave.PreviousStartDate = SelectedLeave.StartDate;
                            SelectedLeave.PreviousEndDate = SelectedLeave.EndDate;
                            SelectedLeave.RequestNote = string.IsNullOrWhiteSpace(requestNote)
                                ? $"🔄 Onaylı izin değişikliği talep edildi ({finalStartDate:dd.MM.yyyy HH:mm} - {finalEndDate:dd.MM.yyyy HH:mm})"
                                : $"🔄 Onaylı izin değişikliği (%50 çakışma): {requestNote}";

                            resultMessage = "⏳ Onaylanmış izniniz için değişiklik talebi gönderildi! Yönetici yeniden onayına sunuldu.";
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(requestNote)) SelectedLeave.RequestNote = requestNote;
                            resultMessage = "⏳ Bekleyen izin talebiniz güncellendi ve yönetici onayına sunuldu.";
                        }

                        SelectedLeave.IsNotificationSeen = true; // Clear previous notification card while waiting for re-approval!
                        SelectedLeave.StartDate = finalStartDate;
                        SelectedLeave.EndDate = finalEndDate;
                    }

                    _services.UpdateLeave(SelectedLeave);
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

        private void ExecuteApprove(object? param)
        {
            if (param is Leave leave)
            {
                try
                {
                    _services.ApproveLeave(leave.Id, CurrentUserName);
                    StatusMessage = $"✅ '{leave.User?.FullName}' için izin talebi başarıyla onaylandı.";
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Onaylama hatası: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ExecuteReject(object? param)
        {
            if (param is Leave leave)
            {
                var result = MessageBox.Show($"'{leave.User?.FullName}' isimli personelin izin talebini reddetmek istediğinize emin misiniz?", "Talebi Reddet", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        _services.RejectLeave(leave.Id, CurrentUserName);
                        StatusMessage = "İzin talebi reddedildi.";
                        LoadData();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Reddetme hatası: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
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
