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
    public class SelectableUserItem : BaseViewModel
    {
        public User User { get; set; } = null!;
        public string DisplayText => string.IsNullOrWhiteSpace(User?.Title)
            ? (User?.FullName ?? string.Empty)
            : $"{User?.FullName} ({User?.Title})";

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    if (value && OnSelectionValidatingFunc != null && !OnSelectionValidatingFunc(User))
                    {
                        return;
                    }
                    _isSelected = value;
                    OnPropertyChanged();
                    OnSelectionChangedAction?.Invoke();
                }
            }
        }
        public Func<User, bool>? OnSelectionValidatingFunc { get; set; }
        public Action? OnSelectionChangedAction { get; set; }
    }

    public class TeamUserGroup
    {
        public string TeamName { get; set; } = string.Empty;
        public ObservableCollection<SelectableUserItem> Users { get; set; } = new ObservableCollection<SelectableUserItem>();

        public string HeaderColor => TeamName switch
        {
            "Takip" => "#7c3aed",
            "Tahsis" => "#059669",
            "Teminat" => "#2563eb",
            _ => "#bc171d"
        };

        public string BgColor => TeamName switch
        {
            "Takip" => "#f5f3ff",
            "Tahsis" => "#ecfdf5",
            "Teminat" => "#eff6ff",
            _ => "#f8fafc"
        };

        public string BorderColor => TeamName switch
        {
            "Takip" => "#ddd6fe",
            "Tahsis" => "#a7f3d0",
            "Teminat" => "#bfdbfe",
            _ => "#cbd5e0"
        };
    }

    public class ShiftDetailTeamGroup
    {
        public string TeamName { get; set; } = string.Empty;
        public List<User> UserList { get; set; } = new List<User>();
        public bool HasUsers => UserList != null && UserList.Count > 0;

        public string HeaderColor => TeamName switch
        {
            "Takip" => "#5b21b6",
            "Tahsis" => "#065f46",
            "Teminat" => "#1e40af",
            _ => "#bc171d"
        };

        public string BgColor => TeamName switch
        {
            "Takip" => "#f5f3ff",
            "Tahsis" => "#ecfdf5",
            "Teminat" => "#eff6ff",
            _ => "#f8fafc"
        };

        public string BorderColor => TeamName switch
        {
            "Takip" => "#ddd6fe",
            "Tahsis" => "#a7f3d0",
            "Teminat" => "#bfdbfe",
            _ => "#cbd5e0"
        };
    }

    public class TeamAssignmentCompartment
    {
        public string TeamName { get; set; } = string.Empty;
        public string UserNames { get; set; } = string.Empty;
        public bool HasUsers => !string.IsNullOrWhiteSpace(UserNames);

        public string HeaderColor => TeamName switch
        {
            "Takip" => "#7c3aed",
            "Tahsis" => "#059669",
            "Teminat" => "#2563eb",
            _ => "#2d3748"
        };

        public string BgColor => TeamName switch
        {
            "Takip" => "#f5f3ff",
            "Tahsis" => "#ecfdf5",
            "Teminat" => "#eff6ff",
            _ => "#f8fafc"
        };

        public string BorderColor => TeamName switch
        {
            "Takip" => "#ddd6fe",
            "Tahsis" => "#a7f3d0",
            "Teminat" => "#bfdbfe",
            _ => "#cbd5e0"
        };

        public string TextColor => TeamName switch
        {
            "Takip" => "#5b21b6",
            "Tahsis" => "#065f46",
            "Teminat" => "#1e40af",
            _ => "#4a5568"
        };
    }

    public class DisplayMonthlyReleaseShift
    {
        public int Id { get; set; }
        public string MonthName { get; set; } = string.Empty;
        public DateTime ReleaseDate { get; set; }
        public string RawAssignedUsers { get; set; } = string.Empty;
        public string JiraTicketNo { get; set; } = string.Empty;
        public string ExternalLink { get; set; } = string.Empty;
        public string CreatedByUserName { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; }
        public string? UpdatedByUserName { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdateTooltip => !string.IsNullOrWhiteSpace(UpdatedByUserName)
            ? $"✏️ {UpdatedByUserName} tarafından güncellendi ({(UpdatedAt ?? DateTime.Now):dd.MM.yyyy HH:mm})"
            : $"📌 {CreatedByUserName} tarafından eklendi ({(CreatedAt ?? ReleaseDate):dd.MM.yyyy HH:mm})";
        public ObservableCollection<TeamAssignmentCompartment> TeamCompartments { get; set; } = new ObservableCollection<TeamAssignmentCompartment>();
        public bool IsFinished { get; set; }
        public bool IsReleaseDatePassed => ReleaseDate.Date <= DateTime.Today;
        public Visibility FinishShiftVisibility => (ReleaseDate.Date <= DateTime.Today && !IsFinished) ? Visibility.Visible : Visibility.Collapsed;

        public string DisplayWeekLabel
        {
            get
            {
                if (string.IsNullOrWhiteSpace(MonthName)) return string.Empty;

                string label = MonthName;
                string[] prefixes = new[] { "Haftalık Yaygınlaştırma: ", "Haftalık Yaygınlaştırma - ", "Haftalık Yaygınlaştırma ", "Haftalık: ", "Haftalık - ", "Haftalık ", "Haftalık:" };
                foreach (var prefix in prefixes)
                {
                    if (label.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                    {
                        label = label.Substring(prefix.Length).Trim();
                        break;
                    }
                }
                return label;
            }
        }
    }

    public class DisplayCustomShift
    {
        public int Id { get; set; }
        public string Topic { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime ShiftDate { get; set; }
        public string ExternalLink { get; set; } = string.Empty;
        public string RawAssignedUsers { get; set; } = string.Empty;
        public string CreatedByUserName { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; }
        public string? UpdatedByUserName { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdateTooltip => !string.IsNullOrWhiteSpace(UpdatedByUserName)
            ? $"✏️ {UpdatedByUserName} tarafından güncellendi ({(UpdatedAt ?? DateTime.Now):dd.MM.yyyy HH:mm})"
            : $"📌 {CreatedByUserName} tarafından eklendi ({(CreatedAt ?? ShiftDate):dd.MM.yyyy HH:mm})";
        public ObservableCollection<TeamAssignmentCompartment> TeamCompartments { get; set; } = new ObservableCollection<TeamAssignmentCompartment>();
        public bool IsFinished { get; set; }
        public bool IsShiftDatePassed => ShiftDate.Date <= DateTime.Today;
        public Visibility FinishShiftVisibility => (ShiftDate.Date <= DateTime.Today && !IsFinished) ? Visibility.Visible : Visibility.Collapsed;
    }

    /// <summary>Tek bir hafta için nöbet bilgisi (1 analist + 1 yazılımcı).</summary>
    public class WeeklyRotationWeekEntry
    {
        public int WeekNumber { get; set; }
        public DateTime WeekStart { get; set; }
        public DateTime WeekEnd { get; set; }
        public string WeekLabel => $"{WeekStart:dd.MM} – {WeekEnd:dd.MM.yyyy}  (Hafta {WeekNumber})";
        public string Analyst { get; set; } = string.Empty;
        public string Developer { get; set; } = string.Empty;
        public bool IsCurrentWeek { get; set; }

        // Visual helpers
        public string RowBackground => IsCurrentWeek ? "#f0fdf4" : "White";
        public string RowBorderColor => IsCurrentWeek ? "#22c55e" : "#e2e8f0";
        public string WeekLabelWeight => IsCurrentWeek ? "Bold" : "Normal";
        public string CurrentWeekBadge => IsCurrentWeek ? "🟢 Bu Hafta" : string.Empty;
        public Visibility CurrentWeekBadgeVis => IsCurrentWeek ? Visibility.Visible : Visibility.Collapsed;
    }

    /// <summary>Bir ekibin (Takip/Tahsis/Teminat) haftalık rotasyon kartı.</summary>
    public class WeeklyRotationTeamCard
    {
        public string TeamName { get; set; } = string.Empty;
        public ObservableCollection<WeeklyRotationWeekEntry> Weeks { get; set; } = new();

        public string HeaderColor => TeamName switch
        {
            "Takip" => "#7c3aed",
            "Tahsis" => "#059669",
            "Teminat" => "#2563eb",
            _ => "#374151"
        };
        public string HeaderBg => TeamName switch
        {
            "Takip" => "#f5f3ff",
            "Tahsis" => "#ecfdf5",
            "Teminat" => "#eff6ff",
            _ => "#f9fafb"
        };
        public string BorderColor => TeamName switch
        {
            "Takip" => "#ddd6fe",
            "Tahsis" => "#a7f3d0",
            "Teminat" => "#bfdbfe",
            _ => "#e5e7eb"
        };
        public string Emoji => TeamName switch
        {
            "Takip" => "📋",
            "Tahsis" => "💰",
            "Teminat" => "🛡️",
            _ => "👥"
        };
    }

    public class WeekOptionItem
    {
        public int WeekNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string DisplayText => $"Hafta {WeekNumber} ({StartDate:dd.MM.yyyy} – {EndDate:dd.MM.yyyy})";
    }

    public class MonthWeekOptionItem
    {
        public int WeekIndex { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string DisplayText => $"Hafta {WeekIndex} ({StartDate:dd.MM.yyyy} – {EndDate:dd.MM.yyyy})";
    }

    public class BulkWeeklyShiftRow : BaseViewModel
    {
        public Action? OnChangedAction { get; set; }

        public int WeekNumber { get; set; }
        public DateTime WeekStart { get; set; }
        public DateTime WeekEnd { get; set; }
        public string WeekLabel => $"{WeekStart:dd.MM} – {WeekEnd:dd.MM.yyyy} (Hafta {WeekNumber})";

        public ObservableCollection<User> Analysts { get; set; } = new ObservableCollection<User>();
        public ObservableCollection<User> Developers { get; set; } = new ObservableCollection<User>();
        public Func<User?, DateTime, DateTime, bool>? CheckLeaveConflictFunc { get; set; }

        private User? _selectedAnalyst;
        public User? SelectedAnalyst
        {
            get => _selectedAnalyst;
            set
            {
                if (value != null && CheckLeaveConflictFunc != null && CheckLeaveConflictFunc(value, WeekStart, WeekEnd))
                {
                    _selectedAnalyst = null;
                    OnPropertyChanged();
                    OnChangedAction?.Invoke();
                    return;
                }
                _selectedAnalyst = value;
                OnPropertyChanged();
                OnChangedAction?.Invoke();
            }
        }

        private User? _selectedDeveloper;
        public User? SelectedDeveloper
        {
            get => _selectedDeveloper;
            set
            {
                if (value != null && CheckLeaveConflictFunc != null && CheckLeaveConflictFunc(value, WeekStart, WeekEnd))
                {
                    _selectedDeveloper = null;
                    OnPropertyChanged();
                    OnChangedAction?.Invoke();
                    return;
                }
                _selectedDeveloper = value;
                OnPropertyChanged();
                OnChangedAction?.Invoke();
            }
        }

        private bool _isSelected = true;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged();
                OnChangedAction?.Invoke();
            }
        }
    }

    public class ShiftCalendarItem : BaseViewModel
    {
        public int Id { get; set; }
        public string Category { get; set; } = string.Empty; // "Aylık", "Haftalık", "Özel"
        public string Title { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string RawAssignedUsers { get; set; } = string.Empty;
        public string JiraTicketNo { get; set; } = string.Empty;
        public string ExternalLink { get; set; } = string.Empty;
        public string CreatedByUserName { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; }
        public string? UpdatedByUserName { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public ObservableCollection<TeamAssignmentCompartment> TeamCompartments { get; set; } = new ObservableCollection<TeamAssignmentCompartment>();

        public bool HasJiraTicket => !string.IsNullOrWhiteSpace(JiraTicketNo);
        public bool HasExternalLink => !string.IsNullOrWhiteSpace(ExternalLink);

        public string BadgeColor => Category switch
        {
            "Aylık" => "#6b46c1",     // Deep Purple
            "Haftalık" => "#2b6cb0",   // Steel Blue
            _ => "#d69e2e"             // Warm Amber
        };

        public string BadgeBackground => Category switch
        {
            "Aylık" => "#faf5ff",
            "Haftalık" => "#ebf8ff",
            _ => "#fffaf0"
        };

        public string BadgeBorder => Category switch
        {
            "Aylık" => "#e9d8fd",
            "Haftalık" => "#bee3f8",
            _ => "#feebc8"
        };

        public string CategoryIcon => Category switch
        {
            "Aylık" => "📆",
            "Haftalık" => "📅",
            _ => "⚡"
        };

        public string UpdateTooltip => !string.IsNullOrWhiteSpace(UpdatedByUserName)
            ? $"✏️ {UpdatedByUserName} tarafından güncellendi ({(UpdatedAt ?? DateTime.Now):dd.MM.yyyy HH:mm})"
            : $"📌 {CreatedByUserName} tarafından eklendi ({(CreatedAt ?? Date):dd.MM.yyyy HH:mm})";
    }

    public class ShiftCalendarDay : BaseViewModel
    {
        public DateTime Date { get; set; }
        public int DayNumber => Date.Day;
        public bool IsCurrentMonth { get; set; }
        public bool IsToday => Date.Date == DateTime.Today;
        public bool IsWeekend => Date.DayOfWeek == DayOfWeek.Saturday || Date.DayOfWeek == DayOfWeek.Sunday;

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set { _isSelected = value; OnPropertyChanged(); }
        }

        public ObservableCollection<ShiftCalendarItem> Items { get; set; } = new ObservableCollection<ShiftCalendarItem>();
        public bool HasShifts => Items.Count > 0;
        public int ShiftCount => Items.Count;
    }

    public class ShiftsViewModel : BaseViewModel
    {
        private readonly BusinessServices _services = new BusinessServices();

        // Display Collections for Tables
        private ObservableCollection<DisplayMonthlyReleaseShift> _monthlyReleaseList = new ObservableCollection<DisplayMonthlyReleaseShift>();
        public ObservableCollection<DisplayMonthlyReleaseShift> MonthlyReleaseList
        {
            get => _monthlyReleaseList;
            set { _monthlyReleaseList = value; OnPropertyChanged(); }
        }

        private ObservableCollection<DisplayMonthlyReleaseShift> _weeklyReleaseList = new ObservableCollection<DisplayMonthlyReleaseShift>();
        public ObservableCollection<DisplayMonthlyReleaseShift> WeeklyReleaseList
        {
            get => _weeklyReleaseList;
            set
            {
                _weeklyReleaseList = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasWeeklyReleases));
            }
        }

        private ObservableCollection<DisplayMonthlyReleaseShift> _teamWeeklyDutyList = new ObservableCollection<DisplayMonthlyReleaseShift>();
        public ObservableCollection<DisplayMonthlyReleaseShift> TeamWeeklyDutyList
        {
            get => _teamWeeklyDutyList;
            set
            {
                _teamWeeklyDutyList = value;
                OnPropertyChanged();
                UpdateFilteredWeeklyReleases();
            }
        }

        private ObservableCollection<DisplayMonthlyReleaseShift> _finishedReleaseList = new ObservableCollection<DisplayMonthlyReleaseShift>();
        public ObservableCollection<DisplayMonthlyReleaseShift> FinishedReleaseList
        {
            get => _finishedReleaseList;
            set { _finishedReleaseList = value; OnPropertyChanged(); }
        }

        private string _selectedFinishedFilter = "Tümü";
        public string SelectedFinishedFilter
        {
            get => _selectedFinishedFilter;
            set
            {
                if (_selectedFinishedFilter != value)
                {
                    _selectedFinishedFilter = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsFinishedAllSelected));
                    OnPropertyChanged(nameof(IsFinishedMonthlySelected));
                    OnPropertyChanged(nameof(IsFinishedWeeklySelected));
                    OnPropertyChanged(nameof(IsFinishedCustomSelected));
                    UpdateFilteredFinishedList();
                }
            }
        }

        public bool IsFinishedAllSelected => SelectedFinishedFilter == "Tümü";
        public bool IsFinishedMonthlySelected => SelectedFinishedFilter == "Aylık";
        public bool IsFinishedWeeklySelected => SelectedFinishedFilter == "Haftalık";
        public bool IsFinishedCustomSelected => SelectedFinishedFilter == "Özel";

        private ObservableCollection<DisplayMonthlyReleaseShift> _finishedMonthlyList = new ObservableCollection<DisplayMonthlyReleaseShift>();
        public ObservableCollection<DisplayMonthlyReleaseShift> FinishedMonthlyList
        {
            get => _finishedMonthlyList;
            set { _finishedMonthlyList = value; OnPropertyChanged(); }
        }

        private ObservableCollection<DisplayMonthlyReleaseShift> _finishedWeeklyList = new ObservableCollection<DisplayMonthlyReleaseShift>();
        public ObservableCollection<DisplayMonthlyReleaseShift> FinishedWeeklyList
        {
            get => _finishedWeeklyList;
            set { _finishedWeeklyList = value; OnPropertyChanged(); }
        }

        private ObservableCollection<DisplayMonthlyReleaseShift> _finishedCustomList = new ObservableCollection<DisplayMonthlyReleaseShift>();
        public ObservableCollection<DisplayMonthlyReleaseShift> FinishedCustomList
        {
            get => _finishedCustomList;
            set { _finishedCustomList = value; OnPropertyChanged(); }
        }

        private ObservableCollection<DisplayMonthlyReleaseShift> _filteredFinishedList = new ObservableCollection<DisplayMonthlyReleaseShift>();
        public ObservableCollection<DisplayMonthlyReleaseShift> FilteredFinishedList
        {
            get => _filteredFinishedList;
            set { _filteredFinishedList = value; OnPropertyChanged(); }
        }

        public void UpdateFilteredFinishedList()
        {
            List<DisplayMonthlyReleaseShift> filtered = SelectedFinishedFilter switch
            {
                "Aylık" => FinishedMonthlyList.ToList(),
                "Haftalık" => FinishedWeeklyList.ToList(),
                "Özel" => FinishedCustomList.ToList(),
                _ => FinishedReleaseList.ToList()
            };

            FilteredFinishedList = new ObservableCollection<DisplayMonthlyReleaseShift>(filtered.OrderByDescending(r => r.ReleaseDate));
        }

        public bool HasWeeklyReleases => WeeklyReleaseList.Count > 0;

        private ObservableCollection<DisplayCustomShift> _customShiftsList = new ObservableCollection<DisplayCustomShift>();
        public ObservableCollection<DisplayCustomShift> CustomShiftsList
        {
            get => _customShiftsList;
            set { _customShiftsList = value; OnPropertyChanged(); }
        }

        private ObservableCollection<WeeklyRotationTeamCard> _weeklyRotationCards = new ObservableCollection<WeeklyRotationTeamCard>();
        public ObservableCollection<WeeklyRotationTeamCard> WeeklyRotationCards
        {
            get => _weeklyRotationCards;
            set { _weeklyRotationCards = value; OnPropertyChanged(); }
        }

        // --- Weekly Team Filter Tabs (Takip, Tahsis, Teminat) ---
        private string _selectedWeeklyTeamFilter = "Takip";
        public string SelectedWeeklyTeamFilter
        {
            get => _selectedWeeklyTeamFilter;
            set
            {
                if (_selectedWeeklyTeamFilter != value)
                {
                    _selectedWeeklyTeamFilter = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsTakipFilterSelected));
                    OnPropertyChanged(nameof(IsTahsisFilterSelected));
                    OnPropertyChanged(nameof(IsTeminatFilterSelected));
                    UpdateFilteredWeeklyReleases();
                }
            }
        }

        public bool IsTakipFilterSelected => SelectedWeeklyTeamFilter == "Takip";
        public bool IsTahsisFilterSelected => SelectedWeeklyTeamFilter == "Tahsis";
        public bool IsTeminatFilterSelected => SelectedWeeklyTeamFilter == "Teminat";

        private ObservableCollection<DisplayMonthlyReleaseShift> _filteredWeeklyReleaseList = new ObservableCollection<DisplayMonthlyReleaseShift>();
        public ObservableCollection<DisplayMonthlyReleaseShift> FilteredWeeklyReleaseList
        {
            get => _filteredWeeklyReleaseList;
            set { _filteredWeeklyReleaseList = value; OnPropertyChanged(); }
        }

        private void UpdateFilteredWeeklyReleases()
        {
            if (TeamWeeklyDutyList == null) return;
            var filtered = TeamWeeklyDutyList.Where(w =>
                w.TeamCompartments.Any(tc => string.Equals(tc.TeamName, SelectedWeeklyTeamFilter, StringComparison.OrdinalIgnoreCase))
                || w.RawAssignedUsers.Contains(SelectedWeeklyTeamFilter, StringComparison.OrdinalIgnoreCase)
            ).ToList();

            FilteredWeeklyReleaseList = new ObservableCollection<DisplayMonthlyReleaseShift>(filtered);
        }

        // --- Weekly Shift Side Form Properties (Manual Analyst + Developer selection per team) ---
        public List<string> TeamsList { get; } = new List<string> { "Takip", "Tahsis", "Teminat" };

        private bool _isWeeklySaveEnabled = false;
        public bool IsWeeklySaveEnabled
        {
            get => _isWeeklySaveEnabled;
            set { _isWeeklySaveEnabled = value; OnPropertyChanged(); }
        }

        private bool _isWeeklySavedSuccess = false;

        public void ValidateWeeklyForm()
        {
            if (_isWeeklySavedSuccess)
            {
                IsWeeklySaveEnabled = false;
                OnPropertyChanged(nameof(WeeklySaveButtonText));
                OnPropertyChanged(nameof(IsWeeklySaveEnabled));
                return;
            }

            if (IsBulkWeeklyMode)
            {
                bool teamSelected = !string.IsNullOrWhiteSpace(WeeklyFormTeam);
                bool hasValidCheckedRow = BulkWeeklyShiftRows.Any(r => r.IsSelected && r.SelectedAnalyst != null && r.SelectedDeveloper != null);
                IsWeeklySaveEnabled = teamSelected && hasValidCheckedRow;
            }
            else
            {
                bool teamSelected = !string.IsNullOrWhiteSpace(WeeklyFormTeam);
                bool analystSelected = SelectedWeeklyFormAnalyst != null;
                bool devSelected = SelectedWeeklyFormDeveloper != null;
                bool weekSelected = SelectedWeekOption != null;

                IsWeeklySaveEnabled = teamSelected && analystSelected && devSelected && weekSelected;
            }

            OnPropertyChanged(nameof(WeeklySaveButtonText));
            OnPropertyChanged(nameof(IsWeeklySaveEnabled));
        }

        public void OnWeeklyFormChanged()
        {
            _isWeeklySavedSuccess = false;
            ValidateWeeklyForm();
        }

        private string _weeklyFormTeam = string.Empty;
        public string WeeklyFormTeam
        {
            get => _weeklyFormTeam;
            set
            {
                if (_weeklyFormTeam != value)
                {
                    _weeklyFormTeam = value ?? string.Empty;
                    OnPropertyChanged();
                    UpdateWeeklyFormUserLists();
                    UpdateAvailableWeeksList();
                    BuildBulkWeeklyShiftRows();
                    OnWeeklyFormChanged();
                }
            }
        }

        private bool _isBulkWeeklyMode = true;
        public bool IsBulkWeeklyMode
        {
            get => _isBulkWeeklyMode;
            set
            {
                _isBulkWeeklyMode = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(BulkWeeklyModeVisibility));
                OnPropertyChanged(nameof(SingleWeeklyModeVisibility));
                OnPropertyChanged(nameof(WeeklySaveButtonText));
                OnWeeklyFormChanged();
            }
        }

        public Visibility BulkWeeklyModeVisibility => IsBulkWeeklyMode ? Visibility.Visible : Visibility.Collapsed;
        public Visibility SingleWeeklyModeVisibility => !IsBulkWeeklyMode ? Visibility.Visible : Visibility.Collapsed;

        private ObservableCollection<BulkWeeklyShiftRow> _bulkWeeklyShiftRows = new ObservableCollection<BulkWeeklyShiftRow>();
        public ObservableCollection<BulkWeeklyShiftRow> BulkWeeklyShiftRows
        {
            get => _bulkWeeklyShiftRows;
            set { _bulkWeeklyShiftRows = value; OnPropertyChanged(); }
        }

        public bool CheckUserLeaveConflict(User? user, DateTime startDate, DateTime endDate)
        {
            if (user == null) return false;

            var leaves = _services.GetAllLeaves();
            var activeLeave = leaves.FirstOrDefault(l =>
                (l.UserId == user.Id || (l.User != null && string.Equals(l.User.FullName, user.FullName, StringComparison.OrdinalIgnoreCase)))
                && !string.Equals(l.Status, "Reddedildi", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(l.Status, "Rejected", StringComparison.OrdinalIgnoreCase)
                && l.StartDate.Date <= endDate.Date && l.EndDate.Date >= startDate.Date
            );

            if (activeLeave != null)
            {
                MessageBox.Show(
                    $"⚠️ PERİODİK İZİN ENGELİ!\n\n" +
                    $"'{user.FullName}' isimli personel {activeLeave.StartDate:dd.MM.yyyy} - {activeLeave.EndDate:dd.MM.yyyy} tarihleri arasında İZİNLİDİR!\n\n" +
                    $"İzinli bir personele bu tarihte nöbet ataması yapılamaz. Lütfen başka bir personel seçiniz.",
                    "İzinli Personel Uyarısı",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return true;
            }
            return false;
        }

        public void BuildBulkWeeklyShiftRows()
        {
            if (_allUsers == null || !_allUsers.Any() || string.IsNullOrWhiteSpace(WeeklyFormTeam))
            {
                BulkWeeklyShiftRows.Clear();
                ValidateWeeklyForm();
                return;
            }

            var teamUsers = _allUsers
                .Where(u => string.Equals(u.Team, WeeklyFormTeam, StringComparison.OrdinalIgnoreCase))
                .OrderBy(u => u.Id)
                .ToList();

            var analystTitles = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                { "Analist", "Analyst", "Kıdemli Analist", "Senior Analist" };
            var devTitles = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                { "Developer", "Yazılımcı", "Geliştirici", "Senior Developer", "Junior Developer" };

            var analysts = teamUsers.Where(u => analystTitles.Contains(u.Title)).ToList();
            if (!analysts.Any()) analysts = teamUsers.ToList();

            var devs = teamUsers.Where(u => devTitles.Contains(u.Title)).ToList();
            if (!devs.Any()) devs = teamUsers.ToList();

            var today = DateTime.Today;
            int dayOfWeek = (int)today.DayOfWeek;
            int offsetToMonday = dayOfWeek == 0 ? -6 : 1 - dayOfWeek;
            var thisMonday = today.AddDays(offsetToMonday);

            var trCulture = new System.Globalization.CultureInfo("tr-TR");
            var cal = trCulture.Calendar;

            var rows = new ObservableCollection<BulkWeeklyShiftRow>();

            for (int w = 0; w < 8; w++)
            {
                var monday = thisMonday.AddDays(w * 7);
                var sunday = monday.AddDays(6);
                int weekNo = cal.GetWeekOfYear(monday, System.Globalization.CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

                var rowItem = new BulkWeeklyShiftRow
                {
                    WeekNumber = weekNo,
                    WeekStart = monday,
                    WeekEnd = sunday,
                    Analysts = new ObservableCollection<User>(analysts),
                    Developers = new ObservableCollection<User>(devs),
                    CheckLeaveConflictFunc = CheckUserLeaveConflict,
                    SelectedAnalyst = null,
                    SelectedDeveloper = null,
                    IsSelected = false
                };
                rowItem.OnChangedAction = OnWeeklyFormChanged;
                rows.Add(rowItem);
            }

            BulkWeeklyShiftRows = rows;
            ValidateWeeklyForm();
        }

        private ObservableCollection<WeekOptionItem> _availableWeeksList = new ObservableCollection<WeekOptionItem>();
        public ObservableCollection<WeekOptionItem> AvailableWeeksList
        {
            get => _availableWeeksList;
            set { _availableWeeksList = value; OnPropertyChanged(); }
        }

        private WeekOptionItem? _selectedWeekOption;
        public WeekOptionItem? SelectedWeekOption
        {
            get => _selectedWeekOption;
            set
            {
                _selectedWeekOption = value;
                OnPropertyChanged();
                if (_selectedWeekOption != null)
                {
                    WeeklyFormDate = _selectedWeekOption.StartDate;
                    WeeklyFormDescription = _selectedWeekOption.DisplayText;
                }
                OnWeeklyFormChanged();
            }
        }

        private ObservableCollection<User> _weeklyTeamAnalysts = new ObservableCollection<User>();
        public ObservableCollection<User> WeeklyTeamAnalysts
        {
            get => _weeklyTeamAnalysts;
            set { _weeklyTeamAnalysts = value; OnPropertyChanged(); }
        }

        private ObservableCollection<User> _weeklyTeamDevelopers = new ObservableCollection<User>();
        public ObservableCollection<User> WeeklyTeamDevelopers
        {
            get => _weeklyTeamDevelopers;
            set { _weeklyTeamDevelopers = value; OnPropertyChanged(); }
        }

        private User? _selectedWeeklyFormAnalyst;
        public User? SelectedWeeklyFormAnalyst
        {
            get => _selectedWeeklyFormAnalyst;
            set
            {
                if (value != null && SelectedWeekOption != null)
                {
                    if (CheckUserLeaveConflict(value, SelectedWeekOption.StartDate, SelectedWeekOption.EndDate))
                    {
                        _selectedWeeklyFormAnalyst = null;
                        OnPropertyChanged();
                        OnWeeklyFormChanged();
                        return;
                    }
                }
                _selectedWeeklyFormAnalyst = value;
                OnPropertyChanged();
                OnWeeklyFormChanged();
            }
        }

        private User? _selectedWeeklyFormDeveloper;
        public User? SelectedWeeklyFormDeveloper
        {
            get => _selectedWeeklyFormDeveloper;
            set
            {
                if (value != null && SelectedWeekOption != null)
                {
                    if (CheckUserLeaveConflict(value, SelectedWeekOption.StartDate, SelectedWeekOption.EndDate))
                    {
                        _selectedWeeklyFormDeveloper = null;
                        OnPropertyChanged();
                        OnWeeklyFormChanged();
                        return;
                    }
                }
                _selectedWeeklyFormDeveloper = value;
                OnPropertyChanged();
                OnWeeklyFormChanged();
            }
        }

        private string _weeklyFormDescription = string.Empty;
        public string WeeklyFormDescription
        {
            get => _weeklyFormDescription;
            set
            {
                _weeklyFormDescription = value;
                OnPropertyChanged();
                OnWeeklyFormChanged();
            }
        }

        private DateTime _weeklyFormDate = DateTime.Today;
        public DateTime WeeklyFormDate
        {
            get => _weeklyFormDate;
            set
            {
                _weeklyFormDate = value;
                OnPropertyChanged();
                OnWeeklyFormChanged();
            }
        }

        private string _weeklyFormJiraNo = string.Empty;
        public string WeeklyFormJiraNo
        {
            get => _weeklyFormJiraNo;
            set
            {
                _weeklyFormJiraNo = value;
                OnPropertyChanged();
                OnWeeklyFormChanged();
            }
        }

        private string _weeklyFormLink = string.Empty;
        public string WeeklyFormLink
        {
            get => _weeklyFormLink;
            set
            {
                _weeklyFormLink = value;
                OnPropertyChanged();
                OnWeeklyFormChanged();
            }
        }

        public bool IsWeeklySelected => SelectedWeeklyRelease != null;
        public string WeeklySaveButtonText
        {
            get
            {
                if (_isWeeklySavedSuccess)
                {
                    return IsBulkWeeklyMode ? "✅ Kaydedildi" : "✅ Güncellendi";
                }
                return IsBulkWeeklyMode ? "💾 Toplu Kaydet" : "💾 Güncelle";
            }
        }

        private List<WeekOptionItem> GenerateAllWeeksForYear(int year = 2026)
        {
            var list = new List<WeekOptionItem>();
            var trCulture = new System.Globalization.CultureInfo("tr-TR");
            var cal = trCulture.Calendar;

            DateTime jan1 = new DateTime(year, 1, 1);
            int dayOfWeek = (int)jan1.DayOfWeek;
            int offset = dayOfWeek == 0 ? -6 : 1 - dayOfWeek;
            DateTime firstMonday = jan1.AddDays(offset);

            for (int i = 0; i < 52; i++)
            {
                DateTime monday = firstMonday.AddDays(i * 7);
                DateTime sunday = monday.AddDays(6);
                int weekNo = cal.GetWeekOfYear(monday, System.Globalization.CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

                list.Add(new WeekOptionItem
                {
                    WeekNumber = weekNo,
                    StartDate = monday,
                    EndDate = sunday
                });
            }

            return list;
        }

        public void UpdateAvailableWeeksList()
        {
            var allWeeks = GenerateAllWeeksForYear(2026);

            var assignedWeekMondays = new HashSet<DateTime>();
            if (WeeklyReleaseList != null)
            {
                foreach (var w in WeeklyReleaseList)
                {
                    if (SelectedWeeklyRelease != null && w.Id == SelectedWeeklyRelease.Id) continue;

                    bool belongsToTeam = w.TeamCompartments.Any(tc => string.Equals(tc.TeamName, WeeklyFormTeam, StringComparison.OrdinalIgnoreCase))
                                         || w.RawAssignedUsers.Contains(WeeklyFormTeam, StringComparison.OrdinalIgnoreCase);

                    if (belongsToTeam)
                    {
                        int dow = (int)w.ReleaseDate.DayOfWeek;
                        int off = dow == 0 ? -6 : 1 - dow;
                        assignedWeekMondays.Add(w.ReleaseDate.AddDays(off).Date);
                    }
                }
            }

            var available = allWeeks.Where(w => !assignedWeekMondays.Contains(w.StartDate.Date)).ToList();

            if (SelectedWeeklyRelease != null)
            {
                int dow = (int)SelectedWeeklyRelease.ReleaseDate.DayOfWeek;
                int off = dow == 0 ? -6 : 1 - dow;
                DateTime releaseMonday = SelectedWeeklyRelease.ReleaseDate.AddDays(off).Date;

                var existingOption = allWeeks.FirstOrDefault(w => w.StartDate.Date == releaseMonday);
                if (existingOption != null)
                {
                    if (!available.Any(w => w.StartDate.Date == releaseMonday))
                    {
                        available.Insert(0, existingOption);
                    }
                    AvailableWeeksList = new ObservableCollection<WeekOptionItem>(available);
                    SelectedWeekOption = existingOption;
                    return;
                }
            }

            AvailableWeeksList = new ObservableCollection<WeekOptionItem>(available);
            var upcoming = AvailableWeeksList.FirstOrDefault(w => w.EndDate.Date >= DateTime.Today);
            SelectedWeekOption = upcoming ?? AvailableWeeksList.FirstOrDefault();
        }

        public void UpdateWeeklyFormUserLists()
        {
            if (_allUsers == null || !_allUsers.Any()) return;

            var teamUsers = _allUsers
                .Where(u => string.Equals(u.Team, WeeklyFormTeam, StringComparison.OrdinalIgnoreCase))
                .ToList();

            var analystTitles = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                { "Analist", "Analyst", "Kıdemli Analist", "Senior Analist" };
            var devTitles = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                { "Developer", "Yazılımcı", "Geliştirici", "Senior Developer", "Junior Developer" };

            var analysts = teamUsers.Where(u => analystTitles.Contains(u.Title)).ToList();
            if (!analysts.Any()) analysts = teamUsers.ToList();

            var devs = teamUsers.Where(u => devTitles.Contains(u.Title)).ToList();
            if (!devs.Any()) devs = teamUsers.ToList();

            WeeklyTeamAnalysts = new ObservableCollection<User>(analysts);
            WeeklyTeamDevelopers = new ObservableCollection<User>(devs);

            if (SelectedWeeklyFormAnalyst == null || !analysts.Any(a => a.Id == SelectedWeeklyFormAnalyst.Id))
                SelectedWeeklyFormAnalyst = analysts.FirstOrDefault();

            if (SelectedWeeklyFormDeveloper == null || !devs.Any(d => d.Id == SelectedWeeklyFormDeveloper.Id))
                SelectedWeeklyFormDeveloper = devs.FirstOrDefault();
        }

        private List<User> _allUsers = new List<User>();

        // Admin Interactive Selection Checkboxes
        private ObservableCollection<TeamUserGroup> _releaseTeamUserGroups = new ObservableCollection<TeamUserGroup>();
        public ObservableCollection<TeamUserGroup> ReleaseTeamUserGroups
        {
            get => _releaseTeamUserGroups;
            set { _releaseTeamUserGroups = value; OnPropertyChanged(); }
        }

        private ObservableCollection<TeamUserGroup> _customTeamUserGroups = new ObservableCollection<TeamUserGroup>();
        public ObservableCollection<TeamUserGroup> CustomTeamUserGroups
        {
            get => _customTeamUserGroups;
            set { _customTeamUserGroups = value; OnPropertyChanged(); }
        }

        private int _selectedTabIndex = 0;
        public int SelectedTabIndex
        {
            get => _selectedTabIndex;
            set { _selectedTabIndex = value; OnPropertyChanged(); }
        }

        // Collapsible Form Side Panel Properties
        private bool _isFormOpen = false;
        public bool IsFormOpen
        {
            get => _isFormOpen;
            set
            {
                _isFormOpen = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(FormVisibility));
                OnPropertyChanged(nameof(ShiftScheduleColumnSpan));
            }
        }

        public Visibility FormVisibility => IsFormOpen ? Visibility.Visible : Visibility.Collapsed;
        public int ShiftScheduleColumnSpan => IsFormOpen ? 1 : 2;

        public ICommand OpenFormCommand { get; }
        public ICommand CloseFormCommand { get; }
        public ICommand ToggleFormCommand { get; }
        public ICommand AddNewShiftCommand { get; }

        // Selection Handlers
        private DisplayMonthlyReleaseShift? _selectedMonthlyRelease;
        public DisplayMonthlyReleaseShift? SelectedMonthlyRelease
        {
            get => _selectedMonthlyRelease;
            set
            {
                _selectedMonthlyRelease = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsMonthlySelected));
                OnPropertyChanged(nameof(MonthlySaveButtonText));

                if (_selectedMonthlyRelease != null)
                {
                    _selectedWeeklyRelease = null;
                    _selectedCustomShift = null;
                    OnPropertyChanged(nameof(SelectedWeeklyRelease));
                    OnPropertyChanged(nameof(SelectedCustomShift));
                    OnPropertyChanged(nameof(IsCustomSelected));
                    OnPropertyChanged(nameof(CustomSaveButtonText));

                    ReleaseMonthName = _selectedMonthlyRelease.MonthName;
                    ReleaseDate = _selectedMonthlyRelease.ReleaseDate;
                    ReleaseJiraNo = _selectedMonthlyRelease.JiraTicketNo;
                    ReleaseExternalLink = _selectedMonthlyRelease.ExternalLink;

                    string firstWord = _selectedMonthlyRelease.MonthName.Split(' ')[0];
                    if (MonthsList.Contains(firstWord))
                    {
                        _selectedMonth = firstWord;
                        OnPropertyChanged(nameof(SelectedMonth));
                    }

                    SyncCheckboxesFromUserString(_selectedMonthlyRelease.RawAssignedUsers, ReleaseTeamUserGroups);
                    SelectedTabIndex = 0; // Automatically switch to Yaygınlaştırma Nöbeti tab!
                    IsFormOpen = true; // Open form panel automatically on selection
                }
            }
        }

        private DisplayMonthlyReleaseShift? _selectedWeeklyRelease;
        public DisplayMonthlyReleaseShift? SelectedWeeklyRelease
        {
            get => _selectedWeeklyRelease;
            set
            {
                _selectedWeeklyRelease = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsWeeklySelected));
                OnPropertyChanged(nameof(WeeklySaveButtonText));

                if (_selectedWeeklyRelease != null)
                {
                    _selectedMonthlyRelease = null;
                    _selectedCustomShift = null;
                    OnPropertyChanged(nameof(SelectedMonthlyRelease));
                    OnPropertyChanged(nameof(SelectedCustomShift));
                    OnPropertyChanged(nameof(IsMonthlySelected));
                    OnPropertyChanged(nameof(IsCustomSelected));
                    OnPropertyChanged(nameof(MonthlySaveButtonText));
                    OnPropertyChanged(nameof(CustomSaveButtonText));

                    if (_selectedWeeklyRelease.MonthName.StartsWith("Haftalık Yaygınlaştırma", StringComparison.OrdinalIgnoreCase))
                    {
                        SelectedReleaseTypeOption = "Haftalık Yaygınlaştırma";
                        ReleaseMonthName = _selectedWeeklyRelease.MonthName;
                        ReleaseDate = _selectedWeeklyRelease.ReleaseDate;
                        ReleaseJiraNo = _selectedWeeklyRelease.JiraTicketNo;
                        ReleaseExternalLink = _selectedWeeklyRelease.ExternalLink;

                        foreach (var month in MonthsList)
                        {
                            if (_selectedWeeklyRelease.MonthName.Contains(month, StringComparison.OrdinalIgnoreCase))
                            {
                                _selectedMonth = month;
                                OnPropertyChanged(nameof(SelectedMonth));
                                UpdateMonthWeeksList();
                                break;
                            }
                        }

                        SyncCheckboxesFromUserString(_selectedWeeklyRelease.RawAssignedUsers, ReleaseTeamUserGroups);
                        SelectedTabIndex = 0; // Switch to Yaygınlaştırma Nöbeti tab
                        IsFormOpen = true;
                        return;
                    }

                    WeeklyFormDescription = _selectedWeeklyRelease.MonthName;
                    WeeklyFormDate = _selectedWeeklyRelease.ReleaseDate;
                    WeeklyFormJiraNo = _selectedWeeklyRelease.JiraTicketNo;
                    WeeklyFormLink = _selectedWeeklyRelease.ExternalLink;

                    var comp = _selectedWeeklyRelease.TeamCompartments.FirstOrDefault(c => c.HasUsers);
                    if (comp != null && TeamsList.Contains(comp.TeamName))
                    {
                        WeeklyFormTeam = comp.TeamName;
                    }

                    var names = _selectedWeeklyRelease.RawAssignedUsers.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries).Select(n => n.Trim()).ToList();
                    var analyst = WeeklyTeamAnalysts.FirstOrDefault(a => names.Any(n => string.Equals(a.FullName, n, StringComparison.OrdinalIgnoreCase)));
                    if (analyst != null) SelectedWeeklyFormAnalyst = analyst;

                    var dev = WeeklyTeamDevelopers.FirstOrDefault(d => names.Any(n => string.Equals(d.FullName, n, StringComparison.OrdinalIgnoreCase)));
                    if (dev != null) SelectedWeeklyFormDeveloper = dev;

                    UpdateAvailableWeeksList();
                    IsBulkWeeklyMode = false; // Switch to Single Edit Mode!
                    SelectedTabIndex = 2; // Switch to Haftalık Nöbet side form tab!
                    IsFormOpen = true;
                }
            }
        }

        private DisplayCustomShift? _selectedCustomShift;
        public DisplayCustomShift? SelectedCustomShift
        {
            get => _selectedCustomShift;
            set
            {
                _selectedCustomShift = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsCustomSelected));
                OnPropertyChanged(nameof(CustomSaveButtonText));

                if (_selectedCustomShift != null)
                {
                    _selectedMonthlyRelease = null;
                    _selectedWeeklyRelease = null;
                    OnPropertyChanged(nameof(SelectedMonthlyRelease));
                    OnPropertyChanged(nameof(SelectedWeeklyRelease));
                    OnPropertyChanged(nameof(IsMonthlySelected));
                    OnPropertyChanged(nameof(MonthlySaveButtonText));

                    CustomTopic = _selectedCustomShift.Topic;
                    CustomDescription = _selectedCustomShift.Description;
                    CustomShiftDate = _selectedCustomShift.ShiftDate;
                    CustomJiraNo = _selectedCustomShift.Description;
                    SyncCheckboxesFromUserString(_selectedCustomShift.RawAssignedUsers, CustomTeamUserGroups);
                    SelectedTabIndex = 1; // Automatically switch to Diğer / Özel Nöbet tab!
                    IsFormOpen = true;
                }
            }
        }

        public bool IsMonthlySelected => SelectedMonthlyRelease != null || SelectedWeeklyRelease != null;
        public string MonthlySaveButtonText => IsMonthlySelected ? "💾 Nöbeti Güncelle" : "➕ Yaygınlaştırma Kaydet";

        public bool IsCustomSelected => SelectedCustomShift != null;
        public string CustomSaveButtonText => IsCustomSelected ? "💾 Nöbeti Güncelle" : "➕ Özel Nöbet Kaydet";

        private string _selectedReleaseTypeOption = "Aylık Yaygınlaştırma";
        public string SelectedReleaseTypeOption
        {
            get => _selectedReleaseTypeOption;
            set
            {
                if (_selectedReleaseTypeOption != value && !string.IsNullOrWhiteSpace(value))
                {
                    _selectedReleaseTypeOption = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsWeeklyReleaseTypeSelected));
                    OnPropertyChanged(nameof(IsMonthlyReleaseTypeSelected));
                    UpdateMonthWeeksList();
                    UpdateReleaseMonthNameAndDate();
                }
            }
        }

        public bool IsWeeklyReleaseTypeSelected => SelectedReleaseTypeOption == "Haftalık Yaygınlaştırma";
        public bool IsMonthlyReleaseTypeSelected => SelectedReleaseTypeOption == "Aylık Yaygınlaştırma";

        private ObservableCollection<MonthWeekOptionItem> _monthWeeksList = new ObservableCollection<MonthWeekOptionItem>();
        public ObservableCollection<MonthWeekOptionItem> MonthWeeksList
        {
            get => _monthWeeksList;
            set { _monthWeeksList = value; OnPropertyChanged(); }
        }

        private MonthWeekOptionItem? _selectedMonthWeekOption;
        public MonthWeekOptionItem? SelectedMonthWeekOption
        {
            get => _selectedMonthWeekOption;
            set
            {
                _selectedMonthWeekOption = value;
                OnPropertyChanged();
                UpdateReleaseMonthNameAndDate();
            }
        }

        public void UpdateMonthWeeksList()
        {
            int monthIndex = MonthsList.IndexOf(SelectedMonth) + 1;
            if (monthIndex < 1 || monthIndex > 12)
            {
                monthIndex = DateTime.Today.Month;
            }
            int year = 2026;

            DateTime firstDay = new DateTime(year, monthIndex, 1);
            DateTime lastDay = firstDay.AddMonths(1).AddDays(-1);

            var weeks = new List<MonthWeekOptionItem>();
            DateTime current = firstDay;
            int weekIndex = 1;

            while (current <= lastDay)
            {
                DateTime weekStart = current;
                int daysUntilSunday = ((int)DayOfWeek.Sunday - (int)current.DayOfWeek + 7) % 7;
                DateTime weekEnd = current.AddDays(daysUntilSunday);
                if (weekEnd > lastDay) weekEnd = lastDay;

                weeks.Add(new MonthWeekOptionItem
                {
                    WeekIndex = weekIndex,
                    StartDate = weekStart,
                    EndDate = weekEnd
                });

                weekIndex++;
                current = weekEnd.AddDays(1);
            }

            MonthWeeksList = new ObservableCollection<MonthWeekOptionItem>(weeks);

            if (SelectedMonthWeekOption == null || !MonthWeeksList.Any(w => w.WeekIndex == SelectedMonthWeekOption.WeekIndex))
            {
                SelectedMonthWeekOption = MonthWeeksList.FirstOrDefault();
            }
        }

        public bool ValidateCurrentlySelectedUsers(ObservableCollection<TeamUserGroup> groupList, DateTime startDate, DateTime endDate)
        {
            bool hasConflict = false;
            foreach (var group in groupList)
            {
                foreach (var item in group.Users)
                {
                    if (item.IsSelected)
                    {
                        if (CheckUserLeaveConflict(item.User, startDate.Date, endDate.Date))
                        {
                            item.IsSelected = false;
                            hasConflict = true;
                        }
                    }
                }
            }
            return !hasConflict;
        }

        public void UpdateReleaseMonthNameAndDate()
        {
            DateTime startDate = ReleaseDate.Date;
            DateTime endDate = ReleaseDate.Date;

            if (IsWeeklyReleaseTypeSelected)
            {
                if (SelectedMonthWeekOption != null)
                {
                    ReleaseMonthName = $"Haftalık Yaygınlaştırma: {SelectedMonth} 2026 - Hafta {SelectedMonthWeekOption.WeekIndex} ({SelectedMonthWeekOption.StartDate:dd.MM.yyyy} - {SelectedMonthWeekOption.EndDate:dd.MM.yyyy})";
                    ReleaseDate = SelectedMonthWeekOption.StartDate;
                    startDate = SelectedMonthWeekOption.StartDate.Date;
                    endDate = SelectedMonthWeekOption.EndDate.Date;
                }
                else
                {
                    ReleaseMonthName = $"Haftalık Yaygınlaştırma: {SelectedMonth} 2026";
                }
            }
            else
            {
                ReleaseMonthName = $"{SelectedMonth} 2026 Yaygınlaştırması";
            }

            ValidateCurrentlySelectedUsers(ReleaseTeamUserGroups, startDate, endDate);
        }

        public ObservableCollection<string> MonthsList { get; } = new ObservableCollection<string>
        {
            "Ocak", "Şubat", "Mart", "Nisan", "Mayıs", "Haziran",
            "Temmuz", "Ağustos", "Eylül", "Ekim", "Kasım", "Aralık"
        };

        private string _selectedMonth = "Ocak";
        public string SelectedMonth
        {
            get => _selectedMonth;
            set
            {
                if (_selectedMonth != value && !string.IsNullOrWhiteSpace(value))
                {
                    _selectedMonth = value;
                    OnPropertyChanged();
                    UpdateMonthWeeksList();
                    UpdateReleaseMonthNameAndDate();
                }
            }
        }

        // Form Fields
        private string _releaseMonthName = "Ocak 2026 Yaygınlaştırması";
        public string ReleaseMonthName
        {
            get => _releaseMonthName;
            set { _releaseMonthName = value; OnPropertyChanged(); }
        }

        private DateTime _releaseDate = DateTime.Today;
        public DateTime ReleaseDate
        {
            get => _releaseDate;
            set
            {
                _releaseDate = value;
                OnPropertyChanged();
                DateTime startDate = _releaseDate.Date;
                DateTime endDate = _releaseDate.Date;
                if (IsWeeklyReleaseTypeSelected && SelectedMonthWeekOption != null)
                {
                    startDate = SelectedMonthWeekOption.StartDate.Date;
                    endDate = SelectedMonthWeekOption.EndDate.Date;
                }
                ValidateCurrentlySelectedUsers(ReleaseTeamUserGroups, startDate, endDate);
            }
        }

        private string _releaseJiraNo = string.Empty;
        public string ReleaseJiraNo
        {
            get => _releaseJiraNo;
            set
            {
                _releaseJiraNo = value;
                OnPropertyChanged();
                if (!string.IsNullOrWhiteSpace(_releaseJiraNo))
                {
                    ReleaseExternalLink = $"https://jira.ziraat.com/browse/{_releaseJiraNo.Trim().ToUpper()}";
                }
            }
        }

        private string _releaseExternalLink = string.Empty;
        public string ReleaseExternalLink
        {
            get => _releaseExternalLink;
            set { _releaseExternalLink = value; OnPropertyChanged(); }
        }

        public ObservableCollection<string> ShiftTopicsList { get; } = new ObservableCollection<string>
        {
            "Firewall Geçiş Nöbeti",
            "Server Geçiş Nöbeti",
            "Haftalık Yaygınlaştırma",
            "Aylık Yaygınlaştırma",
            "Acil Güvenlik Yaması",
            "Özel Geçiş Nöbeti"
        };

        private string _customTopic = "Server Geçiş Nöbeti";
        public string CustomTopic
        {
            get => _customTopic;
            set { _customTopic = value; OnPropertyChanged(); }
        }

        private string _customDescription = string.Empty;
        public string CustomDescription
        {
            get => _customDescription;
            set { _customDescription = value; OnPropertyChanged(); }
        }

        private DateTime _customShiftDate = DateTime.Today;
        public DateTime CustomShiftDate
        {
            get => _customShiftDate;
            set
            {
                _customShiftDate = value;
                OnPropertyChanged();
                ValidateCurrentlySelectedUsers(CustomTeamUserGroups, _customShiftDate, _customShiftDate);
            }
        }

        private string _customJiraNo = string.Empty;
        public string CustomJiraNo
        {
            get => _customJiraNo;
            set
            {
                _customJiraNo = value;
                OnPropertyChanged();
                if (!string.IsNullOrWhiteSpace(_customJiraNo))
                {
                    CustomLink = $"https://jira.ziraat.com/browse/{_customJiraNo.Trim().ToUpper()}";
                }
            }
        }

        private string _customLink = string.Empty;
        public string CustomLink
        {
            get => _customLink;
            set { _customLink = value; OnPropertyChanged(); }
        }

        private string _statusMessage = string.Empty;
        public string StatusMessage
        {
            get => _statusMessage;
            set { _statusMessage = value; OnPropertyChanged(); }
        }

        // Admin / Assignment Visibility (Visible for everyone to enter shifts!)
        private bool _isCurrentUserAdmin;
        public bool IsCurrentUserAdmin
        {
            get => _isCurrentUserAdmin;
            set
            {
                _isCurrentUserAdmin = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(AdminPanelVisibility));
                OnPropertyChanged(nameof(ShiftScheduleColumnSpan));
                OnPropertyChanged(nameof(CanCreateNewShiftType));
            }
        }

        public bool CanCreateNewShiftType => IsCurrentUserAdmin;

        public string CurrentUserName { get; set; } = string.Empty;

        public Visibility AdminPanelVisibility => Visibility.Visible;

        // Calendar Mode Properties & Commands
        private short _selectedCalendarYear = (short)DateTime.Today.Year;
        public short SelectedCalendarYear
        {
            get => _selectedCalendarYear;
            set
            {
                _selectedCalendarYear = value;
                OnPropertyChanged();
                BuildCalendarGrid();
            }
        }

        private int _selectedCalendarMonth = DateTime.Today.Month;
        public int SelectedCalendarMonth
        {
            get => _selectedCalendarMonth;
            set
            {
                _selectedCalendarMonth = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SelectedCalendarMonthName));
                BuildCalendarGrid();
            }
        }

        public string SelectedCalendarMonthName => new DateTime(SelectedCalendarYear, SelectedCalendarMonth, 1).ToString("MMMM yyyy", new System.Globalization.CultureInfo("tr-TR"));

        public List<short> CalendarYears { get; } = new List<short> { 2025, 2026, 2027 };
        public List<KeyValuePair<int, string>> CalendarMonths { get; } = new List<KeyValuePair<int, string>>
        {
            new KeyValuePair<int, string>(1, "Ocak"),
            new KeyValuePair<int, string>(2, "Şubat"),
            new KeyValuePair<int, string>(3, "Mart"),
            new KeyValuePair<int, string>(4, "Nisan"),
            new KeyValuePair<int, string>(5, "Mayıs"),
            new KeyValuePair<int, string>(6, "Haziran"),
            new KeyValuePair<int, string>(7, "Temmuz"),
            new KeyValuePair<int, string>(8, "Ağustos"),
            new KeyValuePair<int, string>(9, "Eylül"),
            new KeyValuePair<int, string>(10, "Ekim"),
            new KeyValuePair<int, string>(11, "Kasım"),
            new KeyValuePair<int, string>(12, "Aralık")
        };

        private ObservableCollection<ShiftCalendarDay> _calendarDays = new ObservableCollection<ShiftCalendarDay>();
        public ObservableCollection<ShiftCalendarDay> CalendarDays
        {
            get => _calendarDays;
            set { _calendarDays = value; OnPropertyChanged(); }
        }

        private ShiftCalendarDay? _selectedCalendarDay;
        public ShiftCalendarDay? SelectedCalendarDay
        {
            get => _selectedCalendarDay;
            set
            {
                if (_selectedCalendarDay == value) return;
                if (_selectedCalendarDay != null) _selectedCalendarDay.IsSelected = false;
                _selectedCalendarDay = value;
                if (_selectedCalendarDay != null) _selectedCalendarDay.IsSelected = true;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasSelectedDayShifts));
            }
        }

        public bool HasSelectedDayShifts => SelectedCalendarDay != null && SelectedCalendarDay.HasShifts;

        // Shift Detail Modal Bindings
        private bool _isShiftDetailModalOpen;
        public bool IsShiftDetailModalOpen
        {
            get => _isShiftDetailModalOpen;
            set { _isShiftDetailModalOpen = value; OnPropertyChanged(); }
        }

        private string _detailTitle = string.Empty;
        public string DetailTitle
        {
            get => _detailTitle;
            set { _detailTitle = value; OnPropertyChanged(); }
        }

        private DateTime _detailDate;
        public DateTime DetailDate
        {
            get => _detailDate;
            set { _detailDate = value; OnPropertyChanged(); }
        }

        private string _detailCategory = string.Empty;
        public string DetailCategory
        {
            get => _detailCategory;
            set { _detailCategory = value; OnPropertyChanged(); }
        }

        private string _detailJiraNo = string.Empty;
        public string DetailJiraNo
        {
            get => _detailJiraNo;
            set { _detailJiraNo = value; OnPropertyChanged(nameof(DetailJiraNo)); OnPropertyChanged(nameof(HasDetailJira)); }
        }

        public bool HasDetailJira => !string.IsNullOrWhiteSpace(DetailJiraNo);

        private string _detailLink = string.Empty;
        public string DetailLink
        {
            get => _detailLink;
            set { _detailLink = value; OnPropertyChanged(nameof(DetailLink)); OnPropertyChanged(nameof(HasDetailLink)); }
        }

        public bool HasDetailLink => !string.IsNullOrWhiteSpace(DetailLink);

        private string _detailAuditInfo = string.Empty;
        public string DetailAuditInfo
        {
            get => _detailAuditInfo;
            set { _detailAuditInfo = value; OnPropertyChanged(); }
        }

        private ObservableCollection<ShiftDetailTeamGroup> _detailTeamGroups = new ObservableCollection<ShiftDetailTeamGroup>();
        public ObservableCollection<ShiftDetailTeamGroup> DetailTeamGroups
        {
            get => _detailTeamGroups;
            set { _detailTeamGroups = value; OnPropertyChanged(); }
        }

        // Commands
        public ICommand SaveMonthlyReleaseCommand { get; }
        public ICommand DeleteMonthlyReleaseCommand { get; }
        public ICommand ClearMonthlyFormCommand { get; }
        public ICommand SaveCustomShiftCommand { get; }
        public ICommand DeleteCustomShiftCommand { get; }
        public ICommand ClearCustomFormCommand { get; }
        public ICommand SaveWeeklyShiftCommand { get; }
        public ICommand DeleteWeeklyShiftCommand { get; }
        public ICommand ClearWeeklyFormCommand { get; }
        public ICommand SelectWeeklyTeamFilterCommand { get; }
        public ICommand SelectFinishedFilterCommand { get; }
        public ICommand OpenLinkCommand { get; }
        public ICommand SelectCalendarDayCommand { get; }
        public ICommand OpenShiftDetailCommand { get; }
        public ICommand CloseShiftDetailCommand { get; }
        public ICommand PrevCalendarMonthCommand { get; }
        public ICommand NextCalendarMonthCommand { get; }
        public ICommand FinishMonthlyReleaseCommand { get; }
        public ICommand FinishCustomShiftCommand { get; }

        public ShiftsViewModel() : this(false, string.Empty, false) { }

        public ShiftsViewModel(bool isCurrentUserAdmin, string currentUserName = "", bool openForm = false)
        {
            IsCurrentUserAdmin = isCurrentUserAdmin;
            CurrentUserName = currentUserName;
            IsFormOpen = openForm;

            OpenShiftDetailCommand = new RelayCommand(ExecuteOpenShiftDetail);
            CloseShiftDetailCommand = new RelayCommand(_ => { IsShiftDetailModalOpen = false; });

            SaveMonthlyReleaseCommand = new RelayCommand(ExecuteSaveMonthlyRelease);
            DeleteMonthlyReleaseCommand = new RelayCommand(ExecuteDeleteMonthlyRelease);
            ClearMonthlyFormCommand = new RelayCommand(_ => ClearMonthlyForm());
            SaveCustomShiftCommand = new RelayCommand(ExecuteSaveCustomShift);
            DeleteCustomShiftCommand = new RelayCommand(ExecuteDeleteCustomShift);
            ClearCustomFormCommand = new RelayCommand(_ => ClearCustomForm());
            SaveWeeklyShiftCommand = new RelayCommand(ExecuteSaveWeeklyShift);
            DeleteWeeklyShiftCommand = new RelayCommand(ExecuteDeleteWeeklyShift);
            ClearWeeklyFormCommand = new RelayCommand(_ => ClearWeeklyForm());
            SelectWeeklyTeamFilterCommand = new RelayCommand(param =>
            {
                if (param is string teamName)
                {
                    SelectedWeeklyTeamFilter = teamName;
                }
            });

            SelectFinishedFilterCommand = new RelayCommand(param =>
            {
                if (param is string category)
                {
                    SelectedFinishedFilter = category;
                }
            });

            OpenLinkCommand = new RelayCommand(ExecuteOpenLink);
            PrevCalendarMonthCommand = new RelayCommand(_ => GoToPrevCalendarMonth());
            NextCalendarMonthCommand = new RelayCommand(_ => GoToNextCalendarMonth());
            
            OpenFormCommand = new RelayCommand(_ => { IsFormOpen = true; });
            CloseFormCommand = new RelayCommand(_ => { IsFormOpen = false; });
            ToggleFormCommand = new RelayCommand(_ => { IsFormOpen = !IsFormOpen; });
            AddNewShiftCommand = new RelayCommand(_ =>
            {
                ClearMonthlyForm();
                ClearCustomForm();
                ClearWeeklyForm();
                IsFormOpen = true;
            });

            FinishMonthlyReleaseCommand = new RelayCommand(ExecuteFinishMonthlyRelease);
            FinishCustomShiftCommand = new RelayCommand(ExecuteFinishCustomShift);

            SelectCalendarDayCommand = new RelayCommand(param =>
            {
                if (param is ShiftCalendarDay day)
                {
                    SelectedCalendarDay = day;
                }
            });

            LoadData();
        }

        private void LoadData()
        {
            try
            {
                _allUsers = _services.GetAllUsers();
                BuildTeamUserGroups();
                UpdateWeeklyFormUserLists();
                BuildBulkWeeklyShiftRows();

                // Load dynamic shift types from database (excluding release types which belong exclusively to Tab 1)
                var dbShiftTypes = _services.GetAllShiftTypes()
                    .Where(st => !st.ShiftName.Contains("Yaygınlaştırma", StringComparison.OrdinalIgnoreCase))
                    .ToList();

                ShiftTopicsList.Clear();
                foreach (var st in dbShiftTypes)
                {
                    ShiftTopicsList.Add(st.ShiftName);
                }
                if (!ShiftTopicsList.Any())
                {
                    ShiftTopicsList.Add("Firewall Geçiş Nöbeti");
                    ShiftTopicsList.Add("Server Geçiş Nöbeti");
                    ShiftTopicsList.Add("Acil Güvenlik Yaması");
                    ShiftTopicsList.Add("Özel Geçiş Nöbeti");
                }

                // Yearly Reset: Delete finished shifts from previous years
                int currentYear = DateTime.Today.Year;
                var rawDbReleases = _services.GetAllMonthlyReleaseShifts();
                var oldFinished = rawDbReleases.Where(r => r.IsFinished && r.ReleaseDate.Year < currentYear).ToList();
                foreach (var old in oldFinished)
                {
                    _services.DeleteMonthlyReleaseShift(old.Id);
                }

                // Sort all release shifts chronologically (nearest dates first!)
                var allReleases = _services.GetAllMonthlyReleaseShifts().OrderBy(r => r.ReleaseDate).ToList();

                var displayMonthly = new ObservableCollection<DisplayMonthlyReleaseShift>();
                var displayWeeklyRelease = new ObservableCollection<DisplayMonthlyReleaseShift>();
                var displayTeamWeeklyShift = new ObservableCollection<DisplayMonthlyReleaseShift>();
                var displayFinished = new ObservableCollection<DisplayMonthlyReleaseShift>();
                var displayFinishedMonthly = new ObservableCollection<DisplayMonthlyReleaseShift>();
                var displayFinishedWeekly = new ObservableCollection<DisplayMonthlyReleaseShift>();
                var displayFinishedCustom = new ObservableCollection<DisplayMonthlyReleaseShift>();
                var displayCustom = new ObservableCollection<DisplayCustomShift>();

                foreach (var ms in allReleases)
                {
                    var item = new DisplayMonthlyReleaseShift
                    {
                        Id = ms.Id,
                        MonthName = ms.MonthName,
                        ReleaseDate = ms.ReleaseDate,
                        RawAssignedUsers = ms.AssignedUsers,
                        JiraTicketNo = ms.JiraTicketNo ?? string.Empty,
                        ExternalLink = ms.ExternalLink ?? string.Empty,
                        CreatedByUserName = string.IsNullOrWhiteSpace(ms.CreatedByUserName) ? "-" : ms.CreatedByUserName,
                        CreatedAt = ms.CreatedAt,
                        UpdatedByUserName = ms.UpdatedByUserName,
                        UpdatedAt = ms.UpdatedAt,
                        IsFinished = ms.IsFinished,
                        TeamCompartments = BuildTeamCompartments(ms.AssignedUsers)
                    };

                    if (ms.IsFinished)
                    {
                        displayFinished.Add(item);
                        if (ms.MonthName.StartsWith("Haftalık Yaygınlaştırma", StringComparison.OrdinalIgnoreCase))
                        {
                            displayFinishedWeekly.Add(item);
                        }
                        else if (ms.MonthName.StartsWith("Haftalık", StringComparison.OrdinalIgnoreCase))
                        {
                            displayFinishedWeekly.Add(item);
                        }
                        else
                        {
                            displayFinishedMonthly.Add(item);
                        }
                    }
                    else
                    {
                        if (ms.MonthName.StartsWith("Haftalık Yaygınlaştırma", StringComparison.OrdinalIgnoreCase))
                        {
                            displayWeeklyRelease.Add(item);
                        }
                        else if (ms.MonthName.StartsWith("Haftalık", StringComparison.OrdinalIgnoreCase))
                        {
                            displayTeamWeeklyShift.Add(item);
                        }
                        else
                        {
                            displayMonthly.Add(item);
                        }
                    }
                }

                MonthlyReleaseList = displayMonthly;
                WeeklyReleaseList = displayWeeklyRelease;
                TeamWeeklyDutyList = displayTeamWeeklyShift;
                UpdateAvailableWeeksList();
                FinishedReleaseList = displayFinished;
                FinishedMonthlyList = displayFinishedMonthly;
                FinishedWeeklyList = displayFinishedWeekly;

                // Delete finished custom shifts from previous years
                var rawDbCustom = _services.GetAllCustomShifts();
                var oldFinishedCustom = rawDbCustom.Where(c => c.IsFinished && c.ShiftDate.Year < currentYear).ToList();
                foreach (var old in oldFinishedCustom)
                {
                    _services.DeleteCustomShift(old.Id);
                }

                // Sort all custom shifts chronologically (nearest dates first!)
                var customShifts = _services.GetAllCustomShifts().OrderBy(c => c.ShiftDate).ToList();
                foreach (var cs in customShifts)
                {
                    if (cs.IsFinished)
                    {
                        var finishedCustomItem = new DisplayMonthlyReleaseShift
                        {
                            Id = cs.Id,
                            MonthName = string.IsNullOrWhiteSpace(cs.Description) ? cs.Topic : $"{cs.Topic} - {cs.Description}",
                            ReleaseDate = cs.ShiftDate,
                            RawAssignedUsers = cs.AssignedUsers,
                            JiraTicketNo = string.Empty,
                            ExternalLink = cs.ExternalLink ?? string.Empty,
                            CreatedByUserName = string.IsNullOrWhiteSpace(cs.CreatedByUserName) ? "-" : cs.CreatedByUserName,
                            CreatedAt = cs.CreatedAt,
                            UpdatedByUserName = cs.UpdatedByUserName,
                            UpdatedAt = cs.UpdatedAt,
                            IsFinished = true,
                            TeamCompartments = BuildTeamCompartments(cs.AssignedUsers)
                        };
                        displayFinished.Add(finishedCustomItem);
                        displayFinishedCustom.Add(finishedCustomItem);
                    }
                    else
                    {
                        displayCustom.Add(new DisplayCustomShift
                        {
                            Id = cs.Id,
                            Topic = cs.Topic,
                            Description = cs.Description ?? string.Empty,
                            ShiftDate = cs.ShiftDate,
                            ExternalLink = cs.ExternalLink ?? string.Empty,
                            RawAssignedUsers = cs.AssignedUsers,
                            CreatedByUserName = string.IsNullOrWhiteSpace(cs.CreatedByUserName) ? "-" : cs.CreatedByUserName,
                            CreatedAt = cs.CreatedAt,
                            UpdatedByUserName = cs.UpdatedByUserName,
                            UpdatedAt = cs.UpdatedAt,
                            IsFinished = cs.IsFinished,
                            TeamCompartments = BuildTeamCompartments(cs.AssignedUsers)
                        });
                    }
                }
                FinishedCustomList = displayFinishedCustom;
                CustomShiftsList = displayCustom;
                UpdateFilteredFinishedList();
                BuildCalendarGrid();
                BuildWeeklyRotation();
            }
            catch (Exception ex)
            {
                StatusMessage = $"Yükleme Hatası: {ex.Message}";
            }
        }

        /// <summary>
        /// Her ekip için 1 Analist + 1 Developer/Yazılımcı haftalık rotasyon listesini oluşturur.
        /// Rotasyon, ISO hafta numarasına göre döngüsel olarak hesaplanır (DB'siz, otomatik).
        /// Cari hafta + sonraki 7 hafta gösterilir.
        /// </summary>
        private void BuildWeeklyRotation()
        {
            try
            {
                var teamNames = new[] { "Takip", "Tahsis", "Teminat" };
                var analystTitles = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                    { "Analist", "Analyst", "Kıdemli Analist", "Senior Analist" };
                var developerTitles = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                    { "Developer", "Yazılımcı", "Geliştirici", "Senior Developer", "Kıdemli Developer",
                      "Junior Developer", "Junior Yazılımcı" };

                var today = DateTime.Today;
                // ISO week start = Monday
                int dayOfWeek = (int)today.DayOfWeek; // 0=Sun
                int offsetToMonday = dayOfWeek == 0 ? -6 : 1 - dayOfWeek;
                var thisMonday = today.AddDays(offsetToMonday);

                // Get ISO week number (tr-TR locale)
                var trCulture = new System.Globalization.CultureInfo("tr-TR");
                var cal = trCulture.Calendar;

                var cards = new ObservableCollection<WeeklyRotationTeamCard>();

                foreach (var teamName in teamNames)
                {
                    var teamUsers = _allUsers
                        .Where(u => string.Equals(u.Team, teamName, StringComparison.OrdinalIgnoreCase))
                        .OrderBy(u => u.Id)
                        .ToList();

                    var analysts = teamUsers.Where(u => analystTitles.Contains(u.Title)).ToList();
                    var developers = teamUsers.Where(u => developerTitles.Contains(u.Title)).ToList();

                    // Fallback: if no role distinction, use all users as both
                    if (analysts.Count == 0 && developers.Count == 0)
                    {
                        analysts = teamUsers.Take(teamUsers.Count / 2 + teamUsers.Count % 2).ToList();
                        developers = teamUsers.Skip(teamUsers.Count / 2).ToList();
                    }
                    else if (analysts.Count == 0) analysts = developers;
                    else if (developers.Count == 0) developers = analysts;

                    var card = new WeeklyRotationTeamCard { TeamName = teamName };

                    for (int w = 0; w < 8; w++)
                    {
                        var weekMonday = thisMonday.AddDays(w * 7);
                        var weekSunday = weekMonday.AddDays(6);
                        int weekNo = cal.GetWeekOfYear(weekMonday,
                            System.Globalization.CalendarWeekRule.FirstFourDayWeek,
                            DayOfWeek.Monday);

                        // Use week index from year start for consistent rotation across weeks
                        int weekIndex = weekNo - 1 + (weekMonday.Year - 2026) * 52;

                        var analystName = analysts.Count > 0
                            ? analysts[((weekIndex % analysts.Count) + analysts.Count) % analysts.Count].FullName
                            : "—";
                        var devName = developers.Count > 0
                            ? developers[((weekIndex % developers.Count) + developers.Count) % developers.Count].FullName
                            : "—";

                        card.Weeks.Add(new WeeklyRotationWeekEntry
                        {
                            WeekNumber = weekNo,
                            WeekStart = weekMonday,
                            WeekEnd = weekSunday,
                            Analyst = analystName,
                            Developer = devName,
                            IsCurrentWeek = w == 0
                        });
                    }

                    cards.Add(card);
                }

                WeeklyRotationCards = cards;
            }
            catch (Exception ex)
            {
                StatusMessage = $"Haftalık rotasyon yükleme hatası: {ex.Message}";
            }
        }

        public void BuildCalendarGrid()
        {
            try
            {
                var days = new ObservableCollection<ShiftCalendarDay>();
                if (SelectedCalendarYear < 2000 || SelectedCalendarYear > 2100 || SelectedCalendarMonth < 1 || SelectedCalendarMonth > 12)
                    return;

                var firstDayOfMonth = new DateTime(SelectedCalendarYear, SelectedCalendarMonth, 1);
                int dayOfWeek = (int)firstDayOfMonth.DayOfWeek;
                int offset = dayOfWeek == 0 ? 6 : dayOfWeek - 1; // 0 for Monday, 6 for Sunday

                var startDate = firstDayOfMonth.AddDays(-offset);

                for (int i = 0; i < 42; i++)
                {
                    var currentDate = startDate.AddDays(i);
                    var dayItem = new ShiftCalendarDay
                    {
                        Date = currentDate,
                        IsCurrentMonth = currentDate.Month == SelectedCalendarMonth
                    };

                    // 1. Monthly releases
                    foreach (var m in MonthlyReleaseList)
                    {
                        if (m.ReleaseDate.Date == currentDate.Date)
                        {
                            dayItem.Items.Add(new ShiftCalendarItem
                            {
                                Id = m.Id,
                                Category = "Aylık",
                                Title = m.MonthName,
                                Date = m.ReleaseDate,
                                RawAssignedUsers = m.RawAssignedUsers,
                                JiraTicketNo = m.JiraTicketNo,
                                ExternalLink = m.ExternalLink,
                                CreatedByUserName = m.CreatedByUserName,
                                CreatedAt = m.CreatedAt,
                                UpdatedByUserName = m.UpdatedByUserName,
                                UpdatedAt = m.UpdatedAt,
                                TeamCompartments = m.TeamCompartments
                            });
                        }
                    }

                    // 2. Weekly releases
                    foreach (var w in WeeklyReleaseList)
                    {
                        if (w.ReleaseDate.Date == currentDate.Date)
                        {
                            dayItem.Items.Add(new ShiftCalendarItem
                            {
                                Id = w.Id,
                                Category = "Haftalık",
                                Title = w.MonthName,
                                Date = w.ReleaseDate,
                                RawAssignedUsers = w.RawAssignedUsers,
                                JiraTicketNo = w.JiraTicketNo,
                                ExternalLink = w.ExternalLink,
                                CreatedByUserName = w.CreatedByUserName,
                                CreatedAt = w.CreatedAt,
                                UpdatedByUserName = w.UpdatedByUserName,
                                UpdatedAt = w.UpdatedAt,
                                TeamCompartments = w.TeamCompartments
                            });
                        }
                    }

                    // 3. Custom shifts
                    foreach (var c in CustomShiftsList)
                    {
                        if (c.ShiftDate.Date == currentDate.Date)
                        {
                            dayItem.Items.Add(new ShiftCalendarItem
                            {
                                Id = c.Id,
                                Category = "Özel",
                                Title = c.Topic + (string.IsNullOrWhiteSpace(c.Description) ? "" : $" ({c.Description})"),
                                Date = c.ShiftDate,
                                RawAssignedUsers = c.RawAssignedUsers,
                                JiraTicketNo = c.Description,
                                ExternalLink = c.ExternalLink,
                                CreatedByUserName = c.CreatedByUserName,
                                CreatedAt = c.CreatedAt,
                                UpdatedByUserName = c.UpdatedByUserName,
                                UpdatedAt = c.UpdatedAt,
                                TeamCompartments = c.TeamCompartments
                            });
                        }
                    }

                    days.Add(dayItem);
                }

                CalendarDays = days;

                var defaultSelected = CalendarDays.FirstOrDefault(d => d.IsCurrentMonth && d.HasShifts)
                                      ?? CalendarDays.FirstOrDefault(d => d.IsToday)
                                      ?? CalendarDays.FirstOrDefault(d => d.IsCurrentMonth);
                if (defaultSelected != null)
                {
                    SelectedCalendarDay = defaultSelected;
                }
            }
            catch
            {
                // Ignore transient calendar errors during initialization
            }
        }

        private void BuildTeamUserGroups()
        {
            if (ReleaseTeamUserGroups.Any() && CustomTeamUserGroups.Any())
            {
                return; // Keep team user groups and UI bindings stable!
            }

            var teams = new[] { "Takip", "Tahsis", "Teminat" };

            var releaseGroups = new ObservableCollection<TeamUserGroup>();
            var customGroups = new ObservableCollection<TeamUserGroup>();

            foreach (var team in teams)
            {
                var teamUsers = _allUsers.Where(u => string.Equals(u.Team, team, StringComparison.OrdinalIgnoreCase)).ToList();

                var relGroup = new TeamUserGroup { TeamName = team };
                foreach (var u in teamUsers)
                {
                    var relItem = new SelectableUserItem
                    {
                        User = u,
                        IsSelected = false
                    };
                    relItem.OnSelectionValidatingFunc = (user) =>
                    {
                        DateTime startDate = ReleaseDate.Date;
                        DateTime endDate = ReleaseDate.Date;
                        if (IsWeeklyReleaseTypeSelected && SelectedMonthWeekOption != null)
                        {
                            startDate = SelectedMonthWeekOption.StartDate.Date;
                            endDate = SelectedMonthWeekOption.EndDate.Date;
                        }
                        return !CheckUserLeaveConflict(user, startDate, endDate);
                    };
                    relGroup.Users.Add(relItem);
                }
                releaseGroups.Add(relGroup);

                var custGroup = new TeamUserGroup { TeamName = team };
                foreach (var u in teamUsers)
                {
                    var custItem = new SelectableUserItem
                    {
                        User = u,
                        IsSelected = false
                    };
                    custItem.OnSelectionValidatingFunc = (user) =>
                    {
                        DateTime targetDate = CustomShiftDate.Date;
                        return !CheckUserLeaveConflict(user, targetDate, targetDate);
                    };
                    custGroup.Users.Add(custItem);
                }
                customGroups.Add(custGroup);
            }

            ReleaseTeamUserGroups = releaseGroups;
            CustomTeamUserGroups = customGroups;
        }

        private ObservableCollection<TeamAssignmentCompartment> BuildTeamCompartments(string assignedUserString)
        {
            var compartments = new ObservableCollection<TeamAssignmentCompartment>();
            if (string.IsNullOrWhiteSpace(assignedUserString)) return compartments;

            var userNames = assignedUserString.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                                              .Select(n => n.Trim())
                                              .Where(n => !string.IsNullOrWhiteSpace(n))
                                              .ToList();

            Func<string, string> normalize = s => string.Concat(s.Where(c => !char.IsWhiteSpace(c))).ToLowerInvariant();

            Func<string?, string, bool> teamMatches = (userTeam, targetTeam) =>
            {
                if (string.IsNullOrWhiteSpace(userTeam)) return false;
                string cleanUserTeam = userTeam.Trim();
                if (string.Equals(cleanUserTeam, targetTeam, StringComparison.OrdinalIgnoreCase)) return true;
                if (cleanUserTeam.StartsWith(targetTeam, StringComparison.OrdinalIgnoreCase)) return true;
                return false;
            };

            var teams = new[] { "Takip", "Tahsis", "Teminat", "Departman Yönetimi" };
            foreach (var team in teams)
            {
                var matched = new List<string>();
                foreach (var name in userNames)
                {
                    string normName = normalize(name);
                    var foundUser = _allUsers.FirstOrDefault(u =>
                        normalize(u.FullName) == normName ||
                        normalize($"{u.Name}{u.Surname}") == normName ||
                        string.Equals(u.FullName.Trim(), name, StringComparison.OrdinalIgnoreCase) ||
                        string.Equals(u.Name.Trim(), name, StringComparison.OrdinalIgnoreCase)
                    );

                    if (foundUser != null && teamMatches(foundUser.Team, team))
                    {
                        matched.Add(foundUser.FullName);
                    }
                }

                if (matched.Any())
                {
                    compartments.Add(new TeamAssignmentCompartment
                    {
                        TeamName = team.Equals("Departman Yönetimi", StringComparison.OrdinalIgnoreCase) ? "Yönetim" : team,
                        UserNames = string.Join(", ", matched)
                    });
                }
            }

            // Fallback for unmatched names: re-scan _allUsers to assign exact team before using "Diğer"
            var matchedAll = compartments.SelectMany(c => c.UserNames.Split(',')).Select(n => n.Trim()).ToHashSet();
            var unmatched = userNames.Where(n => !matchedAll.Contains(n)).ToList();
            if (unmatched.Any())
            {
                var stillUnmatched = new List<string>();
                foreach (var name in unmatched)
                {
                    string normName = normalize(name);
                    var foundUser = _allUsers.FirstOrDefault(u =>
                        normalize(u.FullName) == normName ||
                        normalize($"{u.Name}{u.Surname}") == normName ||
                        string.Equals(u.FullName.Trim(), name, StringComparison.OrdinalIgnoreCase)
                    );

                    if (foundUser != null && !string.IsNullOrWhiteSpace(foundUser.Team))
                    {
                        string userTeam = foundUser.Team.Trim();
                        string displayTeam = (userTeam.Equals("Departman Yönetimi", StringComparison.OrdinalIgnoreCase) || userTeam.Equals("Yönetim", StringComparison.OrdinalIgnoreCase)) ? "Yönetim" : userTeam;
                        var existingComp = compartments.FirstOrDefault(c => string.Equals(c.TeamName, displayTeam, StringComparison.OrdinalIgnoreCase));
                        if (existingComp != null)
                        {
                            existingComp.UserNames += $", {foundUser.FullName}";
                        }
                        else
                        {
                            compartments.Add(new TeamAssignmentCompartment
                            {
                                TeamName = displayTeam,
                                UserNames = foundUser.FullName
                            });
                        }
                    }
                    else
                    {
                        stillUnmatched.Add(name);
                    }
                }

                if (stillUnmatched.Any())
                {
                    compartments.Add(new TeamAssignmentCompartment
                    {
                        TeamName = "Diğer",
                        UserNames = string.Join(", ", stillUnmatched)
                    });
                }
            }

            return compartments;
        }

        private void ClearMonthlyForm()
        {
            _selectedMonthlyRelease = null;
            _selectedWeeklyRelease = null;
            OnPropertyChanged(nameof(SelectedMonthlyRelease));
            OnPropertyChanged(nameof(SelectedWeeklyRelease));
            OnPropertyChanged(nameof(IsMonthlySelected));
            OnPropertyChanged(nameof(MonthlySaveButtonText));

            ReleaseJiraNo = string.Empty;
            ReleaseExternalLink = string.Empty;
            ReleaseDate = DateTime.Today;
            foreach (var group in ReleaseTeamUserGroups)
            {
                foreach (var item in group.Users)
                {
                    item.IsSelected = false;
                }
            }
        }

        private void ClearCustomForm()
        {
            SelectedCustomShift = null;
            CustomTopic = "Server Geçiş Nöbeti";
            CustomDescription = string.Empty;
            CustomJiraNo = string.Empty;
            CustomLink = string.Empty;
            CustomShiftDate = DateTime.Today;
            foreach (var group in CustomTeamUserGroups)
            {
                foreach (var item in group.Users)
                {
                    item.IsSelected = false;
                }
            }
        }

        private void SyncCheckboxesFromUserString(string userString, ObservableCollection<TeamUserGroup> groupList)
        {
            var names = string.IsNullOrWhiteSpace(userString)
                ? new HashSet<string>()
                : userString.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(n => n.Trim())
                            .ToHashSet(StringComparer.OrdinalIgnoreCase);

            foreach (var group in groupList)
            {
                foreach (var item in group.Users)
                {
                    item.IsSelected = names.Contains(item.User.FullName);
                }
            }
        }

        private string GetSelectedUserNames(ObservableCollection<TeamUserGroup> groupList)
        {
            var selected = new List<string>();
            foreach (var group in groupList)
            {
                foreach (var item in group.Users)
                {
                    if (item.IsSelected)
                    {
                        selected.Add(item.User.FullName);
                    }
                }
            }
            return string.Join(", ", selected);
        }

        private List<string> GetReleaseShiftPersonnelWarnings(List<User> selectedUsers)
        {
            var warnings = new List<string>();
            string[] teams = new[] { "Takip", "Tahsis", "Teminat" };

            foreach (var team in teams)
            {
                var teamUsers = selectedUsers.Where(u => string.Equals(u.Team, team, StringComparison.OrdinalIgnoreCase)).ToList();

                bool hasDev = teamUsers.Any(u =>
                    !string.IsNullOrWhiteSpace(u.Title) && (
                    u.Title.Contains("Dev", StringComparison.OrdinalIgnoreCase) ||
                    u.Title.Contains("Yazılım", StringComparison.OrdinalIgnoreCase)));

                bool hasAnalist = teamUsers.Any(u =>
                    !string.IsNullOrWhiteSpace(u.Title) &&
                    u.Title.Contains("Analist", StringComparison.OrdinalIgnoreCase));

                if (!hasDev)
                {
                    warnings.Add($"• {team} Ekibi: Seçilen nöbetçiler arasında Yazılımcı / Developer eksik.");
                }
                if (!hasAnalist)
                {
                    warnings.Add($"• {team} Ekibi: Seçilen nöbetçiler arasında Analist eksik.");
                }
            }

            if (selectedUsers.Count != 6)
            {
                warnings.Add($"• Toplam Personel Sayısı: Standarda göre 6 kişi (3 Yazılımcı + 3 Analist) olması gerekirken şu an {selectedUsers.Count} kişi seçildi.");
            }

            return warnings;
        }

        private void ExecuteSaveMonthlyRelease(object? param)
        {
            try
            {
                DateTime targetStart = ReleaseDate.Date;
                DateTime targetEnd = ReleaseDate.Date;
                if (IsWeeklyReleaseTypeSelected && SelectedMonthWeekOption != null)
                {
                    targetStart = SelectedMonthWeekOption.StartDate.Date;
                    targetEnd = SelectedMonthWeekOption.EndDate.Date;
                }

                if (!ValidateCurrentlySelectedUsers(ReleaseTeamUserGroups, targetStart, targetEnd))
                {
                    return;
                }

                var selectedUserItems = ReleaseTeamUserGroups.SelectMany(g => g.Users).Where(u => u.IsSelected).ToList();
                var selectedUsersList = selectedUserItems.Select(u => u.User).ToList();

                var warnings = GetReleaseShiftPersonnelWarnings(selectedUsersList);
                if (warnings.Any())
                {
                    string warningMessage = "⚠️ Yaygınlaştırma Nöbet Kural Uyarısı:\n\n" +
                        string.Join("\n", warnings) +
                        "\n\nStandart Kural: Her 3 ekipten (Takip, Tahsis, Teminat) 1 Yazılımcı ve 1 Analist olmak üzere toplam 6 kişi seçilmesi tavsiye edilir.\n\nYine de bu nöbet kaydını bu şekilde onaylayıp kaydetmek istiyor musunuz?";

                    var dialogResult = MessageBox.Show(warningMessage, "Kural Dışı Nöbet Onayı", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (dialogResult != MessageBoxResult.Yes)
                    {
                        return; // User cancelled save to adjust personnel
                    }
                }

                string assignedUsers = GetSelectedUserNames(ReleaseTeamUserGroups);
                string creator = string.IsNullOrWhiteSpace(CurrentUserName) ? "Sistem Kullanıcısı" : CurrentUserName;

                var target = SelectedMonthlyRelease ?? SelectedWeeklyRelease;

                if (target == null)
                {
                    var item = new MonthlyReleaseShift
                    {
                        MonthName = ReleaseMonthName,
                        ReleaseDate = ReleaseDate,
                        AssignedUsers = assignedUsers,
                        JiraTicketNo = ReleaseJiraNo,
                        ExternalLink = ReleaseExternalLink,
                        CreatedByUserName = creator,
                        CreatedAt = DateTime.Now
                    };
                    _services.AddMonthlyReleaseShift(item);
                    StatusMessage = "Yaygınlaştırma nöbeti eklendi.";
                }
                else
                {
                    var item = new MonthlyReleaseShift
                    {
                        Id = target.Id,
                        MonthName = ReleaseMonthName,
                        ReleaseDate = ReleaseDate,
                        AssignedUsers = assignedUsers,
                        JiraTicketNo = ReleaseJiraNo,
                        ExternalLink = ReleaseExternalLink,
                        CreatedByUserName = string.IsNullOrWhiteSpace(target.CreatedByUserName) ? creator : target.CreatedByUserName,
                        UpdatedByUserName = creator,
                        UpdatedAt = DateTime.Now
                    };
                    _services.UpdateMonthlyReleaseShift(item);
                    StatusMessage = $"✅ Güncellendi — {creator} tarafından güncellendi.";
                }

                ClearMonthlyForm();
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hata", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ExecuteDeleteMonthlyRelease(object? param)
        {
            var target = SelectedMonthlyRelease ?? SelectedWeeklyRelease;
            if (target == null) return;
            var res = MessageBox.Show($"{target.MonthName} kaydını silmek istiyor musunuz?", "Sil", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res == MessageBoxResult.Yes)
            {
                _services.DeleteMonthlyReleaseShift(target.Id);
                ClearMonthlyForm();
                LoadData();
            }
        }

        private void ExecuteSaveCustomShift(object? param)
        {
            try
            {
                if (!ValidateCurrentlySelectedUsers(CustomTeamUserGroups, CustomShiftDate.Date, CustomShiftDate.Date))
                {
                    return;
                }

                string assignedUsers = GetSelectedUserNames(CustomTeamUserGroups);
                string creator = string.IsNullOrWhiteSpace(CurrentUserName) ? "Sistem Kullanıcısı" : CurrentUserName;

                if (string.IsNullOrWhiteSpace(CustomTopic))
                {
                    MessageBox.Show("Lütfen bir nöbet konusu / tipi seçiniz.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // If user types a brand new shift topic, save it so it's available in the dropdown for future use!
                bool isNewShiftType = !ShiftTopicsList.Any(t => string.Equals(t, CustomTopic.Trim(), StringComparison.OrdinalIgnoreCase));
                if (isNewShiftType)
                {
                    try
                    {
                        _services.AddShiftType(CustomTopic.Trim());
                    }
                    catch
                    {
                        // Ignore duplicate DB insert error if any
                    }

                    if (!ShiftTopicsList.Contains(CustomTopic.Trim()))
                    {
                        ShiftTopicsList.Add(CustomTopic.Trim());
                    }
                }

                // SMART ROUTING: If user selects Haftalık or Aylık Yaygınlaştırma, automatically route to Ana Yaygınlaştırma Nöbet Listesi!
                if (CustomTopic.IndexOf("Yaygınlaştırma", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    var releaseItem = new MonthlyReleaseShift
                    {
                        MonthName = $"{CustomTopic} ({CustomShiftDate:dd.MM.yyyy})",
                        ReleaseDate = CustomShiftDate,
                        AssignedUsers = assignedUsers,
                        JiraTicketNo = CustomJiraNo,
                        ExternalLink = CustomLink,
                        CreatedByUserName = creator
                    };
                    _services.AddMonthlyReleaseShift(releaseItem);
                    StatusMessage = $"{CustomTopic} başarıyla Ana Yaygınlaştırma Nöbet Listesine eklendi.";
                }
                else
                {
                    if (SelectedCustomShift == null)
                    {
                        var item = new CustomShift
                        {
                            Topic = CustomTopic,
                            Description = string.IsNullOrWhiteSpace(CustomDescription) ? CustomJiraNo : CustomDescription,
                            AssignedUsers = assignedUsers,
                            ShiftDate = CustomShiftDate,
                            ExternalLink = CustomLink,
                            CreatedByUserName = creator
                        };
                        _services.AddCustomShift(item);
                        StatusMessage = "Nöbet kaydı başarıyla Diğer Nöbet Listesine eklendi.";
                    }
                    else
                    {
                        var item = new CustomShift
                        {
                            Id = SelectedCustomShift.Id,
                            Topic = CustomTopic,
                            Description = string.IsNullOrWhiteSpace(CustomDescription) ? CustomJiraNo : CustomDescription,
                            AssignedUsers = assignedUsers,
                            ShiftDate = CustomShiftDate,
                            ExternalLink = CustomLink,
                            CreatedByUserName = string.IsNullOrWhiteSpace(SelectedCustomShift.CreatedByUserName) ? creator : SelectedCustomShift.CreatedByUserName,
                            UpdatedByUserName = creator,
                            UpdatedAt = DateTime.Now
                        };
                        _services.UpdateCustomShift(item);
                        StatusMessage = $"✅ Güncellendi — {creator} tarafından güncellendi.";
                    }
                }

                ClearCustomForm();
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hata", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ExecuteDeleteCustomShift(object? param)
        {
            if (SelectedCustomShift == null) return;
            var res = MessageBox.Show($"{SelectedCustomShift.Topic} kaydını silmek istiyor musunuz?", "Sil", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res == MessageBoxResult.Yes)
            {
                _services.DeleteCustomShift(SelectedCustomShift.Id);
                ClearCustomForm();
                LoadData();
            }
        }

        private void ClearWeeklyForm()
        {
            _selectedWeeklyRelease = null;
            IsBulkWeeklyMode = true;
            _isWeeklySavedSuccess = false;

            _weeklyFormTeam = string.Empty;
            _selectedWeeklyFormAnalyst = null;
            _selectedWeeklyFormDeveloper = null;
            _selectedWeekOption = null;
            _weeklyFormDate = DateTime.Today;
            _weeklyFormDescription = string.Empty;
            _weeklyFormJiraNo = string.Empty;
            _weeklyFormLink = string.Empty;

            OnPropertyChanged(nameof(SelectedWeeklyRelease));
            OnPropertyChanged(nameof(IsWeeklySelected));
            OnPropertyChanged(nameof(WeeklyFormTeam));
            OnPropertyChanged(nameof(SelectedWeeklyFormAnalyst));
            OnPropertyChanged(nameof(SelectedWeeklyFormDeveloper));
            OnPropertyChanged(nameof(SelectedWeekOption));
            OnPropertyChanged(nameof(WeeklyFormDescription));
            OnPropertyChanged(nameof(WeeklyFormJiraNo));
            OnPropertyChanged(nameof(WeeklyFormLink));
            OnPropertyChanged(nameof(WeeklySaveButtonText));

            WeeklyTeamAnalysts.Clear();
            WeeklyTeamDevelopers.Clear();
            BulkWeeklyShiftRows.Clear();

            ValidateWeeklyForm();
        }

        private void ExecuteSaveBulkWeeklyShifts(object? param)
        {
            if (!IsWeeklySaveEnabled) return;

            try
            {
                int count = 0;
                string creator = string.IsNullOrWhiteSpace(CurrentUserName) ? "Sistem Kullanıcısı" : CurrentUserName;

                foreach (var row in BulkWeeklyShiftRows)
                {
                    if (!row.IsSelected || row.SelectedAnalyst == null || row.SelectedDeveloper == null) continue;

                    string monthName = $"Haftalık: Hafta {row.WeekNumber} ({row.WeekStart:dd.MM} - {row.WeekEnd:dd.MM.yyyy})";
                    string assignedUsers = $"{row.SelectedAnalyst.FullName}, {row.SelectedDeveloper.FullName}";

                    var existing = _services.GetAllMonthlyReleaseShifts()
                        .FirstOrDefault(s => !s.IsFinished &&
                                             s.ReleaseDate.Date == row.WeekStart.Date &&
                                             (s.MonthName.Contains(WeeklyFormTeam) || s.AssignedUsers.Contains(row.SelectedAnalyst.FullName) || s.AssignedUsers.Contains(row.SelectedDeveloper.FullName)));

                    if (existing != null)
                    {
                        existing.MonthName = monthName;
                        existing.AssignedUsers = assignedUsers;
                        existing.UpdatedByUserName = creator;
                        existing.UpdatedAt = DateTime.Now;
                        _services.UpdateMonthlyReleaseShift(existing);
                    }
                    else
                    {
                        var newItem = new MonthlyReleaseShift
                        {
                            MonthName = monthName,
                            ReleaseDate = row.WeekStart,
                            AssignedUsers = assignedUsers,
                            JiraTicketNo = string.Empty,
                            ExternalLink = string.Empty,
                            CreatedByUserName = creator,
                            CreatedAt = DateTime.Now
                        };
                        _services.AddMonthlyReleaseShift(newItem);
                    }
                    count++;
                }

                StatusMessage = $"✅ {count} adet haftalık nöbet kaydı başarıyla kaydedildi.";
                LoadData();
                _isWeeklySavedSuccess = true;
                IsWeeklySaveEnabled = false;
                OnPropertyChanged(nameof(WeeklySaveButtonText));
                OnPropertyChanged(nameof(IsWeeklySaveEnabled));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Toplu nöbet kaydetme hatası: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ExecuteSaveWeeklyShift(object? param)
        {
            if (IsBulkWeeklyMode)
            {
                ExecuteSaveBulkWeeklyShifts(param);
                return;
            }

            if (!IsWeeklySaveEnabled) return;

            try
            {
                if (string.IsNullOrWhiteSpace(WeeklyFormTeam) || SelectedWeeklyFormAnalyst == null || SelectedWeeklyFormDeveloper == null || SelectedWeekOption == null)
                {
                    MessageBox.Show("Lütfen Ekip, Analist, Yazılımcı ve Hafta seçimlerinin tamamını eksiksiz yapınız.", "Eksik Bilgi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                string assignedUsers = $"{SelectedWeeklyFormAnalyst.FullName}, {SelectedWeeklyFormDeveloper.FullName}";
                string creator = string.IsNullOrWhiteSpace(CurrentUserName) ? "Sistem Kullanıcısı" : CurrentUserName;

                string desc = SelectedWeekOption.DisplayText;
                string monthName = desc.StartsWith("Haftalık", StringComparison.OrdinalIgnoreCase) ? desc : $"Haftalık: {desc}";

                int targetId = SelectedWeeklyRelease?.Id ?? 0;

                if (targetId == 0)
                {
                    var item = new MonthlyReleaseShift
                    {
                        MonthName = monthName,
                        ReleaseDate = WeeklyFormDate,
                        AssignedUsers = assignedUsers,
                        JiraTicketNo = WeeklyFormJiraNo,
                        ExternalLink = WeeklyFormLink,
                        CreatedByUserName = creator,
                        CreatedAt = DateTime.Now
                    };
                    _services.AddMonthlyReleaseShift(item);
                    targetId = item.Id;
                    StatusMessage = "Haftalık nöbet kaydı başarıyla eklendi.";
                }
                else
                {
                    var item = new MonthlyReleaseShift
                    {
                        Id = targetId,
                        MonthName = monthName,
                        ReleaseDate = WeeklyFormDate,
                        AssignedUsers = assignedUsers,
                        JiraTicketNo = WeeklyFormJiraNo,
                        ExternalLink = WeeklyFormLink,
                        CreatedByUserName = string.IsNullOrWhiteSpace(SelectedWeeklyRelease?.CreatedByUserName) ? creator : SelectedWeeklyRelease!.CreatedByUserName,
                        UpdatedByUserName = creator,
                        UpdatedAt = DateTime.Now
                    };
                    _services.UpdateMonthlyReleaseShift(item);
                    StatusMessage = $"✅ Güncellendi — {creator} tarafından güncellendi.";
                }

                LoadData();

                if (targetId > 0 && WeeklyReleaseList != null)
                {
                    SelectedWeeklyRelease = WeeklyReleaseList.FirstOrDefault(w => w.Id == targetId);
                }

                _isWeeklySavedSuccess = true;
                IsWeeklySaveEnabled = false;
                OnPropertyChanged(nameof(WeeklySaveButtonText));
                OnPropertyChanged(nameof(IsWeeklySaveEnabled));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Haftalık nöbet kaydetme hatası: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ExecuteDeleteWeeklyShift(object? param)
        {
            if (SelectedWeeklyRelease == null) return;
            var res = MessageBox.Show($"{SelectedWeeklyRelease.MonthName} haftalık nöbet kaydını silmek istiyor musunuz?", "Sil", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res == MessageBoxResult.Yes)
            {
                _services.DeleteMonthlyReleaseShift(SelectedWeeklyRelease.Id);
                ClearWeeklyForm();
                LoadData();
            }
        }

        private void ExecuteOpenLink(object? param)
        {
            string link = param as string ?? string.Empty;
            if (string.IsNullOrWhiteSpace(link)) return;

            try
            {
                _services.TriggerExternalLink(link);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bağlantı açılamadı: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteOpenShiftDetail(object? param)
        {
            if (param == null) return;

            string title = string.Empty;
            DateTime date = DateTime.Today;
            string category = "Yaygınlaştırma Nöbeti";
            string rawUsers = string.Empty;
            string jiraNo = string.Empty;
            string extLink = string.Empty;
            string auditInfo = string.Empty;

            if (param is DisplayMonthlyReleaseShift ms)
            {
                title = ms.MonthName;
                date = ms.ReleaseDate;
                category = ms.MonthName.StartsWith("Haftalık", StringComparison.OrdinalIgnoreCase) ? "Haftalık Yaygınlaştırma" : "Aylık Ana Yaygınlaştırma";
                rawUsers = ms.RawAssignedUsers;
                jiraNo = ms.JiraTicketNo;
                extLink = ms.ExternalLink;
                auditInfo = ms.UpdateTooltip;
            }
            else if (param is DisplayCustomShift cs)
            {
                title = cs.Topic;
                date = cs.ShiftDate;
                category = "Özel / Müdahale Nöbeti";
                rawUsers = cs.RawAssignedUsers;
                jiraNo = cs.Description;
                extLink = cs.ExternalLink;
                auditInfo = cs.UpdateTooltip;
            }
            else if (param is ShiftCalendarItem item)
            {
                title = item.Title;
                date = item.Date;
                category = $"{item.Category} Nöbeti";
                rawUsers = item.RawAssignedUsers;
                jiraNo = item.JiraTicketNo;
                extLink = item.ExternalLink;
                auditInfo = item.UpdateTooltip;
            }

            DetailTitle = title;
            DetailDate = date;
            DetailCategory = category;
            DetailJiraNo = jiraNo;
            DetailLink = extLink;
            DetailAuditInfo = auditInfo;

            // Resolve assigned users with full titles & teams from DB
            var allUsers = _services.GetAllUsers();
            var nameList = (rawUsers ?? string.Empty).Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(n => n.Trim()).ToList();

            var assignedUserObjects = new List<User>();
            foreach (var name in nameList)
            {
                var matched = allUsers.FirstOrDefault(u => string.Equals(u.FullName, name, StringComparison.OrdinalIgnoreCase));
                if (matched != null)
                {
                    assignedUserObjects.Add(matched);
                }
                else
                {
                    assignedUserObjects.Add(new User { Name = name, Surname = "", Team = "Diğer", Title = "Nöbetçi" });
                }
            }

            string[] teamNames = new[] { "Takip", "Tahsis", "Teminat" };
            var groups = new ObservableCollection<ShiftDetailTeamGroup>();

            foreach (var team in teamNames)
            {
                var teamUsers = assignedUserObjects.Where(u => string.Equals(u.Team, team, StringComparison.OrdinalIgnoreCase)).ToList();
                if (teamUsers.Any())
                {
                    groups.Add(new ShiftDetailTeamGroup
                    {
                        TeamName = team,
                        UserList = teamUsers
                    });
                }
            }

            var otherUsers = assignedUserObjects.Where(u => !teamNames.Contains(u.Team, StringComparer.OrdinalIgnoreCase)).ToList();
            if (otherUsers.Any())
            {
                groups.Add(new ShiftDetailTeamGroup
                {
                    TeamName = "Diğer",
                    UserList = otherUsers
                });
            }

            DetailTeamGroups = groups;
            IsShiftDetailModalOpen = true;
        }

        private void GoToPrevCalendarMonth()
        {
            if (SelectedCalendarMonth > 1)
            {
                SelectedCalendarMonth--;
            }
            else
            {
                if (CalendarYears.Contains((short)(SelectedCalendarYear - 1)))
                {
                    SelectedCalendarYear--;
                    SelectedCalendarMonth = 12;
                }
            }
        }

        private void GoToNextCalendarMonth()
        {
            if (SelectedCalendarMonth < 12)
            {
                SelectedCalendarMonth++;
            }
            else
            {
                if (CalendarYears.Contains((short)(SelectedCalendarYear + 1)))
                {
                    SelectedCalendarYear++;
                    SelectedCalendarMonth = 1;
                }
            }
        }

        private void ExecuteFinishMonthlyRelease(object? param)
        {
            if (param is DisplayMonthlyReleaseShift displayShift)
            {
                IsFormOpen = false;
                SelectedMonthlyRelease = null;
                SelectedWeeklyRelease = null;

                var confirm = System.Windows.MessageBox.Show(
                    $"'{displayShift.MonthName}' nöbetini bitirmek ve geçmiş nöbet kayıtlarına taşımak istiyor musunuz?",
                    "Nöbeti Bitir",
                    System.Windows.MessageBoxButton.YesNo,
                    System.Windows.MessageBoxImage.Question);

                if (confirm == System.Windows.MessageBoxResult.Yes)
                {
                    try
                    {
                        var shift = _services.GetAllMonthlyReleaseShifts().FirstOrDefault(s => s.Id == displayShift.Id);
                        if (shift != null)
                        {
                            shift.IsFinished = true;
                            _services.UpdateMonthlyReleaseShift(shift);
                            LoadData();
                            StatusMessage = $"'{displayShift.MonthName}' nöbeti başarıyla tamamlandı ve geçmiş kayıtlara taşındı.";
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Windows.MessageBox.Show($"Nöbet bitirme hatası: {ex.Message}", "Hata", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                    }
                }
            }
        }

        private void ExecuteFinishCustomShift(object? param)
        {
            if (param is DisplayCustomShift displayShift)
            {
                IsFormOpen = false;
                SelectedCustomShift = null;

                var confirm = System.Windows.MessageBox.Show(
                    $"'{displayShift.Topic}' nöbetini bitirmek ve geçmiş nöbet kayıtlarına taşımak istiyor musunuz?",
                    "Nöbeti Bitir",
                    System.Windows.MessageBoxButton.YesNo,
                    System.Windows.MessageBoxImage.Question);

                if (confirm == System.Windows.MessageBoxResult.Yes)
                {
                    try
                    {
                        var shift = _services.GetAllCustomShifts().FirstOrDefault(s => s.Id == displayShift.Id);
                        if (shift != null)
                        {
                            shift.IsFinished = true;
                            _services.UpdateCustomShift(shift);
                            LoadData();
                            StatusMessage = $"'{displayShift.Topic}' nöbeti başarıyla tamamlandı ve geçmiş kayıtlara taşındı.";
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Windows.MessageBox.Show($"Nöbet bitirme hatası: {ex.Message}", "Hata", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                    }
                }
            }
        }
    }
}
