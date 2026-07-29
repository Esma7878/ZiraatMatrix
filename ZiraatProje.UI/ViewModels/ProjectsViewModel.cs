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
    public class QuarterTabModel
    {
        public byte Quarter { get; set; }
        public string Title { get; set; } = string.Empty;
        public string SubTitle { get; set; } = string.Empty;
        public string[] Months { get; set; } = Array.Empty<string>();
    }

    public class SelectableStakeholderItem : BaseViewModel
    {
        public string Name { get; set; } = string.Empty;
        public Action? OnSelectionChangedAction { get; set; }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected == value) return;
                _isSelected = value;
                OnPropertyChanged();
                OnSelectionChangedAction?.Invoke();
            }
        }
    }

    public class ProjectPersonMonthlyCostRow : BaseViewModel
    {
        public User User { get; set; } = null!;
        public Action? OnChangedAction { get; set; }

        private decimal _month1ManDays;
        public decimal Month1ManDays
        {
            get => _month1ManDays;
            set
            {
                _month1ManDays = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TotalManDays));
                OnChangedAction?.Invoke();
            }
        }

        private decimal _month2ManDays;
        public decimal Month2ManDays
        {
            get => _month2ManDays;
            set
            {
                _month2ManDays = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TotalManDays));
                OnChangedAction?.Invoke();
            }
        }

        private decimal _month3ManDays;
        public decimal Month3ManDays
        {
            get => _month3ManDays;
            set
            {
                _month3ManDays = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TotalManDays));
                OnChangedAction?.Invoke();
            }
        }

        private decimal _actualManDays;
        public decimal ActualManDays
        {
            get => _actualManDays;
            set
            {
                _actualManDays = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(OverrunDays));
                OnPropertyChanged(nameof(OverrunBadgeText));
                OnPropertyChanged(nameof(OverrunColor));
                OnPropertyChanged(nameof(OverrunBg));
                OnChangedAction?.Invoke();
            }
        }

        public decimal TotalManDays => Month1ManDays + Month2ManDays + Month3ManDays;
        public decimal OverrunDays => ActualManDays > 0m ? (ActualManDays - TotalManDays) : 0m;

        public string OverrunBadgeText
        {
            get
            {
                if (ActualManDays <= 0m) return "-";
                if (OverrunDays > 0.05m) return $"⚠️ +{OverrunDays:N0} Gün Aşım";
                if (OverrunDays < -0.05m) return $"✅ -{Math.Abs(OverrunDays):N0} Gün Tasarruf";
                return "✅ Tam Uygun";
            }
        }

        public string OverrunColor
        {
            get
            {
                if (ActualManDays <= 0m) return "#64748b";
                if (OverrunDays > 0.05m) return "#991b1b";
                if (OverrunDays < -0.05m) return "#166534";
                return "#1e40af";
            }
        }

        public string OverrunBg
        {
            get
            {
                if (ActualManDays <= 0m) return "#f1f5f9";
                if (OverrunDays > 0.05m) return "#fee2e2";
                if (OverrunDays < -0.05m) return "#dcfce7";
                return "#eff6ff";
            }
        }
    }

    public class UserCostBreakdownRow : BaseViewModel
    {
        public int UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string DisplayNameWithRole => string.IsNullOrWhiteSpace(Role) ? FullName : $"{FullName} ({Role})";
        public decimal Month1ManDays { get; set; }
        public decimal Month2ManDays { get; set; }
        public decimal Month3ManDays { get; set; }
        public decimal TotalManDays => Month1ManDays + Month2ManDays + Month3ManDays;

        public decimal ActualManDaysShare { get; set; }
        public bool IsCompletedProject { get; set; }
        public decimal OverrunDays => IsCompletedProject ? (ActualManDaysShare - TotalManDays) : 0m;

        public string ActualShareText => IsCompletedProject && ActualManDaysShare > 0m 
            ? $"{ActualManDaysShare:N0} Gün" 
            : "-";

        public string OverrunBadgeText
        {
            get
            {
                if (!IsCompletedProject) return "-";
                if (OverrunDays > 0.05m) return $"⚠️ +{OverrunDays:N0} Gün Aşım";
                if (OverrunDays < -0.05m) return $"✅ -{Math.Abs(OverrunDays):N0} Gün Tasarruf";
                return "✅ Tam Uygun";
            }
        }

        public string OverrunColor
        {
            get
            {
                if (!IsCompletedProject) return "#64748b";
                if (OverrunDays > 0.05m) return "#991b1b";
                if (OverrunDays < -0.05m) return "#166534";
                return "#1e40af";
            }
        }

        public string OverrunBg
        {
            get
            {
                if (!IsCompletedProject) return "#f1f5f9";
                if (OverrunDays > 0.05m) return "#fee2e2";
                if (OverrunDays < -0.05m) return "#dcfce7";
                return "#eff6ff";
            }
        }
    }

    public class ProjectDisplayItem : BaseViewModel
    {
        public Project Project { get; set; } = null!;

        private bool _isEditing;
        public bool IsEditing
        {
            get => _isEditing;
            set
            {
                _isEditing = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsReadOnly));
                OnPropertyChanged(nameof(EditVisibility));
                OnPropertyChanged(nameof(ReadVisibility));
            }
        }
        public bool IsReadOnly => !_isEditing;
        public Visibility EditVisibility => _isEditing ? Visibility.Visible : Visibility.Collapsed;
        public Visibility ReadVisibility => !_isEditing ? Visibility.Visible : Visibility.Collapsed;

        private decimal _month1ManDaysTotal;
        public decimal Month1ManDaysTotal
        {
            get => _month1ManDaysTotal;
            set
            {
                _month1ManDaysTotal = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TotalInternalManDays));
                OnPropertyChanged(nameof(EffectiveBudget));
                OnPropertyChanged(nameof(BudgetStatusBadge));
            }
        }

        private decimal _month2ManDaysTotal;
        public decimal Month2ManDaysTotal
        {
            get => _month2ManDaysTotal;
            set
            {
                _month2ManDaysTotal = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TotalInternalManDays));
                OnPropertyChanged(nameof(EffectiveBudget));
                OnPropertyChanged(nameof(BudgetStatusBadge));
            }
        }

        private decimal _month3ManDaysTotal;
        public decimal Month3ManDaysTotal
        {
            get => _month3ManDaysTotal;
            set
            {
                _month3ManDaysTotal = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TotalInternalManDays));
                OnPropertyChanged(nameof(EffectiveBudget));
                OnPropertyChanged(nameof(BudgetStatusBadge));
            }
        }

        public decimal TotalInternalManDays => Month1ManDaysTotal + Month2ManDaysTotal + Month3ManDaysTotal;

        public List<UserCostBreakdownRow> UserBreakdownList { get; set; } = new List<UserCostBreakdownRow>();

        public string FormattedAnalystNames => ProjectsViewModel.FormatNamesWithAbbreviatedSurname(Project?.AssignedAnalystNames);
        public string FormattedDeveloperNames => ProjectsViewModel.FormatNamesWithAbbreviatedSurname(Project?.AssignedDeveloperNames);

        private decimal? _overrideAnalystPlanned;
        public decimal AnalystPlannedManDays
        {
            get => _overrideAnalystPlanned ?? (UserBreakdownList?.Where(u => string.Equals(u.Role, "Analist", StringComparison.OrdinalIgnoreCase)).Sum(u => u.TotalManDays) ?? 0m);
            set
            {
                _overrideAnalystPlanned = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TotalInternalManDays));
            }
        }

        private decimal? _overrideDeveloperPlanned;
        public decimal DeveloperPlannedManDays
        {
            get => _overrideDeveloperPlanned ?? (UserBreakdownList?.Where(u => string.Equals(u.Role, "Yazılımcı", StringComparison.OrdinalIgnoreCase)).Sum(u => u.TotalManDays) ?? 0m);
            set
            {
                _overrideDeveloperPlanned = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TotalInternalManDays));
            }
        }

        private decimal? _overrideAnalystActual;
        public decimal AnalystActualManDays
        {
            get => _overrideAnalystActual ?? (UserBreakdownList?.Where(u => string.Equals(u.Role, "Analist", StringComparison.OrdinalIgnoreCase)).Sum(u => u.ActualManDaysShare) ?? 0m);
            set
            {
                _overrideAnalystActual = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ActualManDays));
                OnPropertyChanged(nameof(BudgetStatusBadge));
            }
        }

        private decimal? _overrideDeveloperActual;
        public decimal DeveloperActualManDays
        {
            get => _overrideDeveloperActual ?? (UserBreakdownList?.Where(u => string.Equals(u.Role, "Yazılımcı", StringComparison.OrdinalIgnoreCase)).Sum(u => u.ActualManDaysShare) ?? 0m);
            set
            {
                _overrideDeveloperActual = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ActualManDays));
                OnPropertyChanged(nameof(BudgetStatusBadge));
            }
        }

        public decimal ActualManDays
        {
            get => (_overrideAnalystActual.HasValue || _overrideDeveloperActual.HasValue) 
                ? (_overrideAnalystActual ?? 0m) + (_overrideDeveloperActual ?? 0m) 
                : (Project?.ActualManDays ?? 0m);
            set
            {
                if (Project != null) Project.ActualManDays = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(BudgetStatusBadge));
            }
        }
        public bool IsCompleted => string.Equals(Project?.ProjectStatus, "Tamamlandı", StringComparison.OrdinalIgnoreCase);
        public decimal EffectiveBudget => Project?.TotalManDayBudget > 0 ? Project.TotalManDayBudget : TotalInternalManDays;
        public decimal VarianceDays => ActualManDays - EffectiveBudget;

        public bool IsActualBudgetExceeded => IsCompleted && EffectiveBudget > 0 && ActualManDays > EffectiveBudget;
        public bool IsActualBudgetWithin => IsCompleted && EffectiveBudget > 0 && ActualManDays <= EffectiveBudget;

        public string BudgetStatusBadge
        {
            get
            {
                if (!IsCompleted)
                {
                    return EffectiveBudget > 0 ? $"Bütçe: {EffectiveBudget:N0} Gün" : "-";
                }
                if (ActualManDays == 0m)
                {
                    return "Efor Girilmedi";
                }
                if (ActualManDays > EffectiveBudget)
                {
                    decimal excess = ActualManDays - EffectiveBudget;
                    return $"⚠️ Bütçe Aşıldı (+{excess:N0} Gün)";
                }
                if (ActualManDays < EffectiveBudget)
                {
                    decimal savings = EffectiveBudget - ActualManDays;
                    return $"✅ Bütçe Uygun (-{savings:N0} Gün)";
                }
                return "✅ Bütçe Tam Uygun";
            }
        }

        public string BudgetStatusColor
        {
            get
            {
                if (!IsCompleted) return "#475569";
                if (ActualManDays == 0m) return "#d97706";
                if (ActualManDays > EffectiveBudget) return "#991b1b";
                return "#166534";
            }
        }

        public string BudgetStatusBg
        {
            get
            {
                if (!IsCompleted) return "#f1f5f9";
                if (ActualManDays == 0m) return "#fef3c7";
                if (ActualManDays > EffectiveBudget) return "#fee2e2";
                return "#dcfce7";
            }
        }

        public string OverrunSummaryText
        {
            get
            {
                if (!IsCompleted) return "Henüz Tamamlanmadı";
                if (EffectiveBudget <= 0m) return $"{ActualManDays:N0} Gün Harcandı";
                decimal diff = ActualManDays - EffectiveBudget;
                decimal pct = (diff / EffectiveBudget) * 100m;
                if (diff > 0m) return $"⚠️ +{diff:N0} Gün Aşım (%{pct:N1} Sapma)";
                if (diff < 0m) return $"✅ -{Math.Abs(diff):N0} Gün Tasarruf (%{Math.Abs(pct):N1} Altında)";
                return "✅ Bütçe Tam Uygun";
            }
        }

        public string TopOverrunLabel
        {
            get
            {
                if (!IsCompleted || UserBreakdownList == null || !UserBreakdownList.Any()) return "👤 Efor Durumu:";
                var topExceeded = UserBreakdownList.OrderByDescending(u => u.OverrunDays).FirstOrDefault();
                if (topExceeded != null && topExceeded.OverrunDays > 0.05m)
                {
                    return "🚨 En Çok Aşım Yapan:";
                }
                var topSavings = UserBreakdownList.OrderBy(u => u.OverrunDays).FirstOrDefault();
                if (topSavings != null && topSavings.OverrunDays < -0.05m)
                {
                    return "🌱 En Çok Tasarruf Sağlayan:";
                }
                return "✅ Bütçe Performansı:";
            }
        }

        public string TopOverrunPersonColor
        {
            get
            {
                if (!IsCompleted || UserBreakdownList == null || !UserBreakdownList.Any()) return "#64748b";
                var topExceeded = UserBreakdownList.OrderByDescending(u => u.OverrunDays).FirstOrDefault();
                if (topExceeded != null && topExceeded.OverrunDays > 0.05m)
                {
                    return "#dc2626";
                }
                var topSavings = UserBreakdownList.OrderBy(u => u.OverrunDays).FirstOrDefault();
                if (topSavings != null && topSavings.OverrunDays < -0.05m)
                {
                    return "#166534";
                }
                return "#1e40af";
            }
        }

        public string TopOverrunPersonText
        {
            get
            {
                if (!IsCompleted || UserBreakdownList == null || !UserBreakdownList.Any()) return "-";
                var topExceeded = UserBreakdownList.OrderByDescending(u => u.OverrunDays).FirstOrDefault();
                if (topExceeded != null && topExceeded.OverrunDays > 0.05m)
                {
                    return $"{topExceeded.FullName} ({topExceeded.Role}) ➔ +{topExceeded.OverrunDays:N0} Gün Aşım";
                }
                var topSavings = UserBreakdownList.OrderBy(u => u.OverrunDays).FirstOrDefault();
                if (topSavings != null && topSavings.OverrunDays < -0.05m)
                {
                    return $"{topSavings.FullName} ({topSavings.Role}) ➔ -{Math.Abs(topSavings.OverrunDays):N0} Gün Tasarruf";
                }
                return "Tüm çalışanlar bütçeye tam uygun.";
            }
        }

        public string AuditTooltipText
        {
            get
            {
                if (Project == null) return string.Empty;

                var createdBy = !string.IsNullOrWhiteSpace(Project.CreatedByUserName) ? Project.CreatedByUserName : "Sistem";
                var createdAtStr = Project.CreatedAt.HasValue ? Project.CreatedAt.Value.ToString("dd.MM.yyyy HH:mm") : "-";

                var updatedBy = !string.IsNullOrWhiteSpace(Project.UpdatedByUserName) ? Project.UpdatedByUserName : createdBy;
                var updatedAtStr = Project.UpdatedAt.HasValue ? Project.UpdatedAt.Value.ToString("dd.MM.yyyy HH:mm") : createdAtStr;

                var completedBy = !string.IsNullOrWhiteSpace(Project.CompletedByUserName) ? Project.CompletedByUserName : "-";
                var completedAtStr = Project.CompletedAt.HasValue ? Project.CompletedAt.Value.ToString("dd.MM.yyyy HH:mm") : "-";

                var sb = new System.Text.StringBuilder();
                sb.AppendLine($"👤 Oluşturan: {createdBy} ({createdAtStr})");
                sb.AppendLine($"✏️ Son Güncelleyen: {updatedBy} ({updatedAtStr})");
                if (IsCompleted)
                {
                    sb.AppendLine($"✅ Tamamlayan: {completedBy} ({completedAtStr})");
                }
                return sb.ToString().TrimEnd();
            }
        }
    }

    public class ProjectsViewModel : BaseViewModel
    {
        public static string FormatNamesWithAbbreviatedSurname(string? rawNames)
        {
            if (string.IsNullOrWhiteSpace(rawNames)) return "-";
            if (rawNames.Trim().Equals("Herkes", StringComparison.OrdinalIgnoreCase)) return "Herkes";

            var items = rawNames.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
            var result = new List<string>();

            foreach (var item in items)
            {
                var trimmed = item.Trim();
                if (string.IsNullOrWhiteSpace(trimmed)) continue;

                var parts = trimmed.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length > 1)
                {
                    string firstNames = string.Join(" ", parts.Take(parts.Length - 1));
                    string lastSurname = parts.Last();
                    string abbreviatedLast = lastSurname.Length > 0 ? $"{lastSurname[0]}." : lastSurname;
                    result.Add($"{firstNames} {abbreviatedLast}");
                }
                else
                {
                    result.Add(trimmed);
                }
            }

            return result.Any() ? string.Join(", ", result) : "-";
        }

        private readonly BusinessServices _services = new BusinessServices();

        private string _loggedUserName = string.Empty;
        public string LoggedUserName
        {
            get => _loggedUserName;
            set { _loggedUserName = value; OnPropertyChanged(); }
        }

        private string _loggedTeam = string.Empty;
        public string LoggedTeam
        {
            get => _loggedTeam;
            set { _loggedTeam = value; OnPropertyChanged(); }
        }

        private bool _isUserAdmin;
        public bool IsUserAdmin
        {
            get => _isUserAdmin;
            set
            {
                _isUserAdmin = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanCreateOrEditProjects));
                OnPropertyChanged(nameof(CreateProjectButtonVisibility));
            }
        }

        private string _loggedUserTitle = string.Empty;
        public string LoggedUserTitle
        {
            get => _loggedUserTitle;
            set
            {
                _loggedUserTitle = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanCreateOrEditProjects));
                OnPropertyChanged(nameof(CreateProjectButtonVisibility));
            }
        }

        // Tüm kullanıcılar tam yetkili
        public bool CanCreateOrEditProjects => true;
        public bool CanEditEffortAndStatus => true;
        public bool IsEffortAndStatusReadOnlyForUser => false;
        public bool IsNotAssignedBannerVisible => false;
        public bool IsAssignedBannerVisible => false;
        public Visibility CreateProjectButtonVisibility => Visibility.Visible;

        // --- INLINE EDIT / DETAIL PANEL STATE ---

        private bool _isDetailPanelVisible = false;
        public bool IsDetailPanelVisible
        {
            get => _isDetailPanelVisible;
            set { _isDetailPanelVisible = value; OnPropertyChanged(); OnPropertyChanged(nameof(DetailPanelVisibility)); }
        }
        public Visibility DetailPanelVisibility => IsDetailPanelVisible ? Visibility.Visible : Visibility.Collapsed;

        private bool _isEditMode = false;
        public bool IsEditMode
        {
            get => _isEditMode;
            set
            {
                _isEditMode = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsReadMode));
                OnPropertyChanged(nameof(EditModeVisibility));
                OnPropertyChanged(nameof(ReadModeVisibility));
            }
        }
        public bool IsReadMode => !_isEditMode;
        public Visibility EditModeVisibility => _isEditMode ? Visibility.Visible : Visibility.Collapsed;
        public Visibility ReadModeVisibility => !_isEditMode ? Visibility.Visible : Visibility.Collapsed;

        private bool _isNewProjectMode = false;
        public bool IsNewProjectMode
        {
            get => _isNewProjectMode;
            set { _isNewProjectMode = value; OnPropertyChanged(); OnPropertyChanged(nameof(DetailPanelTitle)); }
        }
        public string DetailPanelTitle => _isNewProjectMode ? "📤 Yeni Proje Oluştur" : "📄 Proje Detayı";

        // Form Visibility Controls (Modal / Collapsible Panel) — korunuyor (backward compat)
        private bool _isFormOpen;
        public bool IsFormOpen
        {
            get => _isFormOpen;
            set
            {
                _isFormOpen = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(FormVisibility));
            }
        }
        public Visibility FormVisibility => IsFormOpen ? Visibility.Visible : Visibility.Collapsed;

        public ObservableCollection<string> TeamFilterList { get; } = new ObservableCollection<string>
        {
            "Tüm Ekipler",
            "Takip",
            "Tahsis",
            "Teminat"
        };

        private string _selectedTeamFilter = "Tüm Ekipler";
        public string SelectedTeamFilter
        {
            get => _selectedTeamFilter;
            set
            {
                _selectedTeamFilter = value;
                OnPropertyChanged();
                LoadProjects();
            }
        }

        public ObservableCollection<string> StatusFilterList { get; } = new ObservableCollection<string>
        {
            "Tüm Durumlar",
            "Planlandı",
            "Devam Ediyor",
            "Tamamlandı",
            "İptal"
        };

        private string _selectedStatusFilter = "Tüm Durumlar";
        public string SelectedStatusFilter
        {
            get => _selectedStatusFilter;
            set
            {
                _selectedStatusFilter = value;
                OnPropertyChanged();
                FilterProjectsList();
            }
        }

        public ObservableCollection<string> BudgetFilterList { get; } = new ObservableCollection<string>
        {
            "Tüm Bütçe Durumları",
            "Bütçesi Aşılanlar",
            "Bütçesi Aşılmayanlar"
        };

        private string _selectedBudgetFilter = "Tüm Bütçe Durumları";
        public string SelectedBudgetFilter
        {
            get => _selectedBudgetFilter;
            set
            {
                _selectedBudgetFilter = value;
                OnPropertyChanged();
                FilterProjectsList();
            }
        }

        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                FilterProjectsList();
            }
        }

        public ObservableCollection<string> TeamList { get; } = new ObservableCollection<string>
        {
            "Takip",
            "Tahsis",
            "Teminat"
        };

        private string _selectedTeam = string.Empty;
        public string SelectedTeam
        {
            get => _selectedTeam;
            set
            {
                _selectedTeam = value;
                OnPropertyChanged();
                LoadUsersAndBuildAssigneeLists();
            }
        }

        // Quarter & Year Selection
        public List<short> Years { get; } = new List<short> { 2025, 2026, 2027 };

        private short _selectedYear = (short)DateTime.Today.Year;
        public short SelectedYear
        {
            get => _selectedYear;
            set
            {
                _selectedYear = value;
                OnPropertyChanged();
                LoadProjects();
            }
        }

        public List<QuarterTabModel> QuarterTabs { get; } = new List<QuarterTabModel>
        {
            new QuarterTabModel { Quarter = 1, Title = "Q1 Çeyreği", SubTitle = "Ocak - Mart", Months = new[] { "Ocak", "Şubat", "Mart" } },
            new QuarterTabModel { Quarter = 2, Title = "Q2 Çeyreği", SubTitle = "Nisan - Haziran", Months = new[] { "Nisan", "Mayıs", "Haziran" } },
            new QuarterTabModel { Quarter = 3, Title = "Q3 Çeyreği", SubTitle = "Temmuz - Eylül", Months = new[] { "Temmuz", "Ağustos", "Eylül" } },
            new QuarterTabModel { Quarter = 4, Title = "Q4 Çeyreği", SubTitle = "Ekim - Aralık", Months = new[] { "Ekim", "Kasım", "Aralık" } }
        };

        private byte _selectedQuarter = 3; // Default Q3
        public byte SelectedQuarter
        {
            get => _selectedQuarter;
            set
            {
                _selectedQuarter = value;
                OnPropertyChanged();
                UpdateQuarterMonthNames();
                LoadProjects();
            }
        }

        private string _month1Header = "1. Ay";
        public string Month1Header
        {
            get => _month1Header;
            set { _month1Header = value; OnPropertyChanged(); }
        }

        private string _month2Header = "2. Ay";
        public string Month2Header
        {
            get => _month2Header;
            set { _month2Header = value; OnPropertyChanged(); }
        }

        private string _month3Header = "3. Ay";
        public string Month3Header
        {
            get => _month3Header;
            set { _month3Header = value; OnPropertyChanged(); }
        }

        private void UpdateQuarterMonthNames()
        {
            var currentTab = QuarterTabs.FirstOrDefault(q => q.Quarter == SelectedQuarter) ?? QuarterTabs[2];
            Month1Header = currentTab.Months[0];
            Month2Header = currentTab.Months[1];
            Month3Header = currentTab.Months[2];
        }

        // Project Display List
        private ObservableCollection<ProjectDisplayItem> _projectsList = new ObservableCollection<ProjectDisplayItem>();
        public ObservableCollection<ProjectDisplayItem> ProjectsList
        {
            get => _projectsList;
            set { _projectsList = value; OnPropertyChanged(); }
        }

        private ProjectDisplayItem? _selectedProjectDisplay;
        public ProjectDisplayItem? SelectedProjectDisplay
        {
            get => _selectedProjectDisplay;
            set
            {
                if (_selectedProjectDisplay == value) return;
                _selectedProjectDisplay = value;
                OnPropertyChanged();
                if (_selectedProjectDisplay != null)
                {
                    SelectedProject = _selectedProjectDisplay.Project;
                    IsNewProjectMode = false;
                    LoadSelectedProject();
                }
                else
                {
                    IsEditMode = false;
                }
            }
        }

        private Project? _selectedProject;
        public Project? SelectedProject
        {
            get => _selectedProject;
            set
            {
                if (_selectedProject == value) return;
                _selectedProject = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(FormTitleText));
                OnPropertyChanged(nameof(SaveButtonText));
                if (_selectedProject != null)
                {
                    LoadSelectedProject();
                }
            }
        }

        // Form Options
        public List<string> StatusList { get; } = new List<string> { "Planlandı", "Devam Ediyor", "Tamamlandı", "İptal" };
        public List<string> TypeList { get; } = new List<string> { "Proje", "KG", "Paydaş Proje", "Dış Firma" };
        
        public List<string> StatusOptions => StatusList;
        public List<string> ProjectTypeOptions => TypeList;
        public List<string> GmyOptions => GmyList;
        public List<string> BusinessUnitOptions => BusinessUnitList;

        public List<string> GmyList { get; } = new List<string>
        {
            "Kredi Politikaları ve Risk Tasfiye GMY",
            "Kredi Tahsis ve Yönetimi GMY",
            "Ürün Yönetimi ve Dijital Bankacılık GMY",
            "Strateji Planlama ve İnsan Kaynakları Grup Başkanlığı",
            "Genel Müdürlük"
        };

        public List<string> BusinessUnitList { get; } = new List<string>
        {
            "Kredi Süreçleri Bölüm Başkanlığı",
            "Kurumsal ve Ticari Krediler Tahsis ve Yönetimi Bölüm Başkanlığı",
            "Kredi Risk İzleme Yapılandırma ve Tasfiye Bölüm Başkanlığı",
            "Finansman Ürünleri Yönetimi Bölüm Başkanlığı",
            "İnşaat ve Gayrimenkul Yönetimi Bölüm Başkanlığı",
            "Ziraat Teknoloji"
        };

        private string _gmy = string.Empty;
        public string Gmy
        {
            get => _gmy;
            set { _gmy = value; OnPropertyChanged(); }
        }

        private string _businessUnit = string.Empty;
        public string BusinessUnit
        {
            get => _businessUnit;
            set { _businessUnit = value; OnPropertyChanged(); }
        }

        // Form Fields
        private long _pergelNo = 1000000 + Random.Shared.Next(100000, 999999);
        public long PergelNo
        {
            get => _pergelNo;
            set { _pergelNo = value; OnPropertyChanged(); }
        }

        private string _projectName = string.Empty;
        public string ProjectName
        {
            get => _projectName;
            set { _projectName = value; OnPropertyChanged(); }
        }

        private string _summary = string.Empty;
        public string Summary
        {
            get => _summary;
            set { _summary = value; OnPropertyChanged(); }
        }

        private string _projectStatus = "Planlandı";
        public string ProjectStatus
        {
            get => _projectStatus;
            set
            {
                _projectStatus = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsCompletedStatus));
                OnPropertyChanged(nameof(ActualBudgetFeedbackMessage));
                OnPropertyChanged(nameof(ActualBudgetFeedbackColor));
                OnPropertyChanged(nameof(ActualBudgetFeedbackBg));
            }
        }

        public bool IsCompletedStatus => string.Equals(ProjectStatus, "Tamamlandı", StringComparison.OrdinalIgnoreCase);

        private decimal _actualManDays = 0m;
        public decimal ActualManDays
        {
            get => _actualManDays;
            set
            {
                _actualManDays = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ActualBudgetFeedbackMessage));
                OnPropertyChanged(nameof(ActualBudgetFeedbackColor));
                OnPropertyChanged(nameof(ActualBudgetFeedbackBg));
            }
        }

        public string ActualBudgetFeedbackMessage
        {
            get
            {
                if (!IsCompletedStatus) return string.Empty;

                decimal budget = TotalManDayBudget > 0 ? TotalManDayBudget : TotalInternalManDays;

                if (ActualManDays <= 0m)
                {
                    return $"ℹ️ Proje tamamlandı olarak işaretlendi. Lütfen yukarıdaki tablodan çalışanların gerçekleşen eforlarını (gün sayısı) giriniz. Gerçekleşen toplam efor otomatik hesaplanacaktır. (Tanımlı Bütçe: {(budget > 0 ? budget.ToString("N0") : "Henüz Belirtilmedi")} Adam/Gün)";
                }

                if (budget > 0m)
                {
                    if (ActualManDays > budget)
                    {
                        decimal diff = ActualManDays - budget;
                        return $"⚠️ BÜTÇE AŞILDI! Tanımlanan Bütçe: {budget:N0} Adam/Gün | Gerçekleşen Efor: {ActualManDays:N0} Adam/Gün. Projede {diff:N0} Gün BÜTÇE AŞIMINIZ VAR!";
                    }
                    else if (ActualManDays < budget)
                    {
                        decimal diff = budget - ActualManDays;
                        return $"✅ BÜTÇE AŞILMADI. Tanımlanan Bütçe: {budget:N0} Adam/Gün | Gerçekleşen Efor: {ActualManDays:N0} Adam/Gün. ({diff:N0} Gün bütçe tasarrufu sağlandı)";
                    }
                    else
                    {
                        return $"✅ BÜTÇE TAM UYGUN! Tanımlanan Bütçe: {budget:N0} Adam/Gün | Gerçekleşen Efor: {ActualManDays:N0} Adam/Gün.";
                    }
                }

                return $"ℹ️ Gerçekleşen Efor: {ActualManDays:N0} Adam/Gün.";
            }
        }

        public string ActualBudgetFeedbackColor
        {
            get
            {
                if (!IsCompletedStatus) return "#1e293b";
                decimal budget = TotalManDayBudget > 0 ? TotalManDayBudget : TotalInternalManDays;
                if (ActualManDays > 0m && budget > 0m && ActualManDays > budget) return "#991b1b";
                if (ActualManDays > 0m) return "#166534";
                return "#92400e";
            }
        }

        public string ActualBudgetFeedbackBg
        {
            get
            {
                if (!IsCompletedStatus) return "#f8fafc";
                decimal budget = TotalManDayBudget > 0 ? TotalManDayBudget : TotalInternalManDays;
                if (ActualManDays > 0m && budget > 0m && ActualManDays > budget) return "#fee2e2";
                if (ActualManDays > 0m) return "#dcfce7";
                return "#fef3c7";
            }
        }

        private string _projectType = "Proje";
        public string ProjectType
        {
            get => _projectType;
            set
            {
                _projectType = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsExternalCompanyVisible));
            }
        }

        public Visibility IsExternalCompanyVisible =>
            string.Equals(ProjectType, "Dış Firma", StringComparison.OrdinalIgnoreCase) ? Visibility.Visible : Visibility.Collapsed;

        private string _externalCompanyName = string.Empty;
        public string ExternalCompanyName
        {
            get => _externalCompanyName;
            set { _externalCompanyName = value; OnPropertyChanged(); }
        }

        private decimal _externalCost = 0m;
        public decimal ExternalCost
        {
            get => _externalCost;
            set
            {
                _externalCost = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TotalProjectCostFormatted));
            }
        }

        private string _description = string.Empty;
        public string Description
        {
            get => _description;
            set { _description = value; OnPropertyChanged(); }
        }

        // Assignees Selection Lists
        private ObservableCollection<SelectableUserItem> _analystUserItems = new ObservableCollection<SelectableUserItem>();
        public ObservableCollection<SelectableUserItem> AnalystUserItems
        {
            get => _analystUserItems;
            set { _analystUserItems = value; OnPropertyChanged(); }
        }

        private ObservableCollection<SelectableUserItem> _developerUserItems = new ObservableCollection<SelectableUserItem>();
        public ObservableCollection<SelectableUserItem> DeveloperUserItems
        {
            get => _developerUserItems;
            set { _developerUserItems = value; OnPropertyChanged(); }
        }

        private string _displayAnalystSummary = "Seçilmedi";
        public string DisplayAnalystSummary
        {
            get => _displayAnalystSummary;
            set { _displayAnalystSummary = value; OnPropertyChanged(); }
        }

        private string _displayDeveloperSummary = "Seçilmedi";
        public string DisplayDeveloperSummary
        {
            get => _displayDeveloperSummary;
            set { _displayDeveloperSummary = value; OnPropertyChanged(); }
        }

        // Stakeholders Selection List
        private ObservableCollection<SelectableStakeholderItem> _stakeholderItems = new ObservableCollection<SelectableStakeholderItem>();
        public ObservableCollection<SelectableStakeholderItem> StakeholderItems
        {
            get => _stakeholderItems;
            set { _stakeholderItems = value; OnPropertyChanged(); }
        }

        // Person Monthly Cost Allocation Rows
        private ObservableCollection<ProjectPersonMonthlyCostRow> _personMonthlyCostRows = new ObservableCollection<ProjectPersonMonthlyCostRow>();
        public ObservableCollection<ProjectPersonMonthlyCostRow> PersonMonthlyCostRows
        {
            get => _personMonthlyCostRows;
            set { _personMonthlyCostRows = value; OnPropertyChanged(); }
        }

        private decimal _totalManDayBudget;
        public decimal TotalManDayBudget
        {
            get => _totalManDayBudget;
            set
            {
                _totalManDayBudget = value;
                OnPropertyChanged();
                RecalculateTotals();
            }
        }

        public decimal TotalInternalManDays => PersonMonthlyCostRows.Sum(r => r.TotalManDays);
        
        public bool IsBudgetExceeded => TotalManDayBudget > 0 && TotalInternalManDays > TotalManDayBudget;

        public bool IsBudgetCritical => TotalManDayBudget > 0 && !IsBudgetExceeded && TotalInternalManDays >= (TotalManDayBudget * 0.85m);

        public bool HasBudgetWarning => IsBudgetExceeded || IsBudgetCritical;

        public string BudgetWarningMessage
        {
            get
            {
                if (IsBudgetExceeded)
                {
                    decimal excess = TotalInternalManDays - TotalManDayBudget;
                    return $"⚠️ BÜTÇE AŞIMI UYARISI: Girilen toplam adam/gün ({TotalInternalManDays:N0}), tanımlanan proje bütçesini ({TotalManDayBudget:N0}) AŞMAKTADIR! ({excess:N0} Adam/Gün Aşım)";
                }
                if (IsBudgetCritical)
                {
                    decimal pct = TotalManDayBudget > 0 ? (TotalInternalManDays / TotalManDayBudget) * 100m : 0m;
                    return $"⚡ KRİTİK BÜTÇE SEVİYESİ: Girilen toplam adam/gün ({TotalInternalManDays:N0}), proje bütçesinin ({TotalManDayBudget:N0}) %{pct:N0}'sine ulaştı.";
                }
                return string.Empty;
            }
        }

        public string TotalProjectCostFormatted => ExternalCost > 0 
            ? $"{TotalInternalManDays:N0} / {(TotalManDayBudget > 0 ? TotalManDayBudget.ToString("N0") : "Bütçe Yok")} Adam/Gün | {ExternalCost:N0} TL (Dış)" 
            : $"{TotalInternalManDays:N0} / {(TotalManDayBudget > 0 ? TotalManDayBudget.ToString("N0") : "Bütçe Yok")} Adam/Gün";

        // Status & Helper Texts
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

        public string FormTitleText => SelectedProject == null ? "🆕 Yeni Proje Talebi Oluştur" : "✏️ Proje Talebini Güncelle";
        public string SaveButtonText => SelectedProject == null ? "Proje Talebini Kaydet" : "Değişiklikleri Güncelle";

        // ── AI Geliştirici / Efor Öneri Asistanı Properties ───────────────
        private ObservableCollection<ProjectAllocationRecommendation> _aiProjectAllocations = new ObservableCollection<ProjectAllocationRecommendation>();
        public ObservableCollection<ProjectAllocationRecommendation> AiProjectAllocations
        {
            get => _aiProjectAllocations;
            set
            {
                _aiProjectAllocations = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasAiProjectAllocations));
            }
        }

        public bool HasAiProjectAllocations => AiProjectAllocations != null && AiProjectAllocations.Count > 0;

        private bool _isAiProjectCardExpanded = false;
        public bool IsAiProjectCardExpanded
        {
            get => _isAiProjectCardExpanded;
            set { _isAiProjectCardExpanded = value; OnPropertyChanged(); }
        }

        // AI Asistan – Ekip Filtresi
        public ObservableCollection<string> AiTeamFilterList { get; } = new ObservableCollection<string>
        {
            "Tüm Ekipler",
            "Takip",
            "Tahsis",
            "Teminat"
        };

        private string _selectedAiTeamFilter = "Tüm Ekipler";
        public string SelectedAiTeamFilter
        {
            get => _selectedAiTeamFilter;
            set
            {
                _selectedAiTeamFilter = value;
                OnPropertyChanged();
                // Kart açıksa ekip değişince otomatik yenile
                if (IsAiProjectCardExpanded)
                    ExecuteGetAIProjectAllocations(null);
            }
        }

        public ICommand GetAIProjectAllocationsCommand { get; set; }
        public ICommand ToggleAiProjectCardCommand { get; set; }

        private void ExecuteGetAIProjectAllocations(object? param)
        {
            try
            {
                var users = _services.GetAllUsers();
                var projects = _services.GetAllProjects();

                var teamFilter = SelectedAiTeamFilter == "Tüm Ekipler" ? null : SelectedAiTeamFilter;

                var recommendations = ZiraatMatrixAiEngine.Instance.Projects.GetDeveloperAllocationRecommendations(
                    string.Empty,
                    users,
                    projects,
                    teamFilter);

                AiProjectAllocations = new ObservableCollection<ProjectAllocationRecommendation>(recommendations.Take(8));
                IsAiProjectCardExpanded = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Akıllı Geliştirici Önerisi oluşturulurken hata: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Commands
        public ICommand SaveProjectCommand { get; }
        public ICommand DeleteProjectCommand { get; }
        public ICommand ClearFormCommand { get; }
        public ICommand SelectQuarterCommand { get; }
        public ICommand OpenCreateFormCommand { get; }
        public ICommand OpenEditFormCommand { get; }
        public ICommand CloseFormCommand { get; }
        public ICommand ClearSearchCommand { get; }
        public ICommand EditProjectCommand { get; }
        public ICommand SaveInlineCommand { get; }
        public ICommand CancelEditCommand { get; }
        public ICommand SelectProjectCommand { get; }
        public ICommand OpenDetailPanelCommand { get; }
        public ICommand ToggleRowEditCommand { get; }
        public ICommand SaveRowEditCommand { get; }
        public ICommand CancelRowEditCommand { get; }

        public ProjectsViewModel() : this(false, "", "") { }
        public ProjectsViewModel(string loggedTeam) : this(false, "", loggedTeam) { }

        public ProjectsViewModel(bool isUserAdmin, string loggedUserName = "", string loggedTeam = "")
        {
            IsUserAdmin = isUserAdmin;
            LoggedUserName = loggedUserName;
            LoggedTeam = loggedTeam;

            ClearSearchCommand = new RelayCommand(_ => SearchText = string.Empty);

            if (!string.IsNullOrWhiteSpace(loggedUserName))
            {
                var userObj = _services.GetAllUsers().FirstOrDefault(u =>
                    u.FullName.Equals(loggedUserName, StringComparison.OrdinalIgnoreCase) ||
                    (!string.IsNullOrWhiteSpace(u.Name) && loggedUserName.Contains(u.Name, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrWhiteSpace(u.Email) && u.Email.StartsWith(loggedUserName, StringComparison.OrdinalIgnoreCase)));

                if (userObj != null)
                {
                    LoggedUserTitle = userObj.Title ?? string.Empty;
                    IsUserAdmin = userObj.IsAdmin;
                    if (string.IsNullOrWhiteSpace(LoggedTeam)) LoggedTeam = userObj.Team ?? string.Empty;
                }
            }

            GetAIProjectAllocationsCommand = new RelayCommand(ExecuteGetAIProjectAllocations);
            ToggleAiProjectCardCommand = new RelayCommand(_ => { IsAiProjectCardExpanded = !IsAiProjectCardExpanded; });

            SaveProjectCommand = new RelayCommand(ExecuteSaveProject);
            DeleteProjectCommand = new RelayCommand(ExecuteDeleteProject, _ => SelectedProject != null);
            ClearFormCommand = new RelayCommand(_ => ClearForm());
            SelectQuarterCommand = new RelayCommand(param =>
            {
                if (param is byte q) SelectedQuarter = q;
                else if (param is string qStr && byte.TryParse(qStr, out byte parsedQ)) SelectedQuarter = parsedQ;
            });

            OpenCreateFormCommand = new RelayCommand(_ =>
            {
                ClearForm();
                if (SelectedTeamFilter != "Tüm Ekipler" && !string.IsNullOrWhiteSpace(SelectedTeamFilter))
                {
                    _selectedTeam = SelectedTeamFilter;
                    OnPropertyChanged(nameof(SelectedTeam));
                }
                else if (!string.IsNullOrWhiteSpace(LoggedTeam))
                {
                    _selectedTeam = LoggedTeam;
                    OnPropertyChanged(nameof(SelectedTeam));
                }
                LoadUsersAndBuildAssigneeLists();
                IsNewProjectMode = true;
                IsEditMode = true;
                IsDetailPanelVisible = true;
            });

            OpenEditFormCommand = new RelayCommand(param =>
            {
                if (param is ProjectDisplayItem item)
                {
                    SelectedProjectDisplay = item;
                    IsFormOpen = true; // eski modal uyumluı
                }
                else if (param is Project proj)
                {
                    SelectedProject = proj;
                    IsFormOpen = true;
                }
            });

            SelectProjectCommand = new RelayCommand(param =>
            {
                if (param is ProjectDisplayItem item)
                {
                    SelectedProjectDisplay = item;
                    IsNewProjectMode = false;
                    IsEditMode = false;
                    IsDetailPanelVisible = true;
                }
            });

            EditProjectCommand = new RelayCommand(_ =>
            {
                IsNewProjectMode = false;
                IsEditMode = true;
            });

            SaveInlineCommand = new RelayCommand(_ =>
            {
                ExecuteSaveProject(null);
                IsEditMode = false;
            });

            CancelEditCommand = new RelayCommand(_ =>
            {
                if (IsNewProjectMode)
                {
                    IsDetailPanelVisible = false;
                    IsNewProjectMode = false;
                }
                else if (SelectedProject != null)
                {
                    LoadSelectedProject(); // form alanlarını geri yükle
                }
                IsEditMode = false;
            });

            OpenDetailPanelCommand = new RelayCommand(param =>
            {
                if (param is ProjectDisplayItem item)
                {
                    SelectedProjectDisplay = item;
                    IsNewProjectMode = false;
                    IsEditMode = false;
                    IsDetailPanelVisible = true;
                }
            });

            ToggleRowEditCommand = new RelayCommand(param =>
            {
                if (param is ProjectDisplayItem item)
                {
                    SelectedProjectDisplay = item;
                    item.IsEditing = true;
                }
            });

            SaveRowEditCommand = new RelayCommand(param =>
            {
                if (param is ProjectDisplayItem item)
                {
                    ExecuteSaveRowItem(item);
                }
            });

            CancelRowEditCommand = new RelayCommand(param =>
            {
                if (param is ProjectDisplayItem item)
                {
                    item.IsEditing = false;
                    LoadProjects();
                }
            });

            CloseFormCommand = new RelayCommand(_ => { IsFormOpen = false; IsDetailPanelVisible = false; IsEditMode = false; });

            InitializeStakeholders();
            UpdateQuarterMonthNames();
            LoadUsersAndBuildAssigneeLists();
            LoadProjects();
        }

        private void InitializeStakeholders()
        {
            var defaultDeptNames = new[] { "Kredi Risk", "Bireysel Bankacılık", "BT Altyapı", "Raporlama & Veri", "Muhasebe & Finans", "Uyum & Mevzuat", "Dijital Bankacılık" };
            StakeholderItems = new ObservableCollection<SelectableStakeholderItem>(
                defaultDeptNames.Select(d => new SelectableStakeholderItem
                {
                    Name = d,
                    IsSelected = false,
                    OnSelectionChangedAction = () => OnPropertyChanged(nameof(StakeholderItems))
                })
            );
        }

        private List<User> _allUsers = new List<User>();

        private void LoadUsersAndBuildAssigneeLists()
        {
            try
            {
                _allUsers = _services.GetAllUsers() ?? new List<User>();

                if (string.IsNullOrWhiteSpace(SelectedTeam))
                {
                    AnalystUserItems = new ObservableCollection<SelectableUserItem>();
                    DeveloperUserItems = new ObservableCollection<SelectableUserItem>();
                    return;
                }

                var teamUsers = _allUsers.Where(u => string.Equals(u.Team, SelectedTeam, StringComparison.OrdinalIgnoreCase)).ToList();

                var analysts = teamUsers.Where(u => (u.Title ?? "").Contains("Analist", StringComparison.OrdinalIgnoreCase)).ToList();
                var developers = teamUsers.Where(u => (u.Title ?? "").Contains("Developer", StringComparison.OrdinalIgnoreCase) || (u.Title ?? "").Contains("Yazılımcı", StringComparison.OrdinalIgnoreCase) || (u.Title ?? "").Contains("Mühendis", StringComparison.OrdinalIgnoreCase)).ToList();

                AnalystUserItems = new ObservableCollection<SelectableUserItem>(
                    analysts.Select(u => new SelectableUserItem
                    {
                        User = u,
                        IsSelected = false,
                        OnSelectionChangedAction = RefreshAssigneesAndCostRows
                    })
                );

                DeveloperUserItems = new ObservableCollection<SelectableUserItem>(
                    developers.Select(u => new SelectableUserItem
                    {
                        User = u,
                        IsSelected = false,
                        OnSelectionChangedAction = RefreshAssigneesAndCostRows
                    })
                );
            }
            catch (Exception ex)
            {
                StatusMessage = $"Kullanıcı listesi hatası: {ex.Message}";
            }
        }

        private void RefreshAssigneesAndCostRows()
        {
            var selectedAnalysts = AnalystUserItems.Where(i => i.IsSelected).ToList();
            var selectedDevelopers = DeveloperUserItems.Where(i => i.IsSelected).ToList();

            // Analyst Summary ("Herkes" or names)
            if (AnalystUserItems.Any() && selectedAnalysts.Count == AnalystUserItems.Count)
                DisplayAnalystSummary = "Herkes";
            else if (selectedAnalysts.Any())
                DisplayAnalystSummary = string.Join(", ", selectedAnalysts.Select(i => i.User.FullName));
            else
                DisplayAnalystSummary = "Seçilmedi";

            // Developer Summary ("Herkes" or names)
            if (DeveloperUserItems.Any() && selectedDevelopers.Count == DeveloperUserItems.Count)
                DisplayDeveloperSummary = "Herkes";
            else if (selectedDevelopers.Any())
                DisplayDeveloperSummary = string.Join(", ", selectedDevelopers.Select(i => i.User.FullName));
            else
                DisplayDeveloperSummary = "Seçilmedi";

            var allSelectedUsers = selectedAnalysts.Concat(selectedDevelopers).Select(i => i.User).ToList();
            var existingRows = PersonMonthlyCostRows.ToList();
            var newRows = new ObservableCollection<ProjectPersonMonthlyCostRow>();

            foreach (var user in allSelectedUsers)
            {
                var existingRow = existingRows.FirstOrDefault(r => r.User.Id == user.Id);
                if (existingRow != null)
                {
                    newRows.Add(existingRow);
                }
                else
                {
                    // Default man-days to 0 so user enters manually
                    newRows.Add(new ProjectPersonMonthlyCostRow
                    {
                        User = user,
                        Month1ManDays = 0m,
                        Month2ManDays = 0m,
                        Month3ManDays = 0m,
                        OnChangedAction = RecalculateTotals
                    });
                }
            }

            PersonMonthlyCostRows = newRows;
            RecalculateTotals();
        }

        private void RecalculateTotals()
        {
            if (PersonMonthlyCostRows != null && PersonMonthlyCostRows.Any())
            {
                decimal totalPersonActual = PersonMonthlyCostRows.Sum(r => r.ActualManDays);
                _actualManDays = totalPersonActual;
                OnPropertyChanged(nameof(ActualManDays));
                OnPropertyChanged(nameof(ActualBudgetFeedbackMessage));
                OnPropertyChanged(nameof(ActualBudgetFeedbackColor));
                OnPropertyChanged(nameof(ActualBudgetFeedbackBg));
            }
            OnPropertyChanged(nameof(TotalInternalManDays));
            OnPropertyChanged(nameof(TotalProjectCostFormatted));
            OnPropertyChanged(nameof(IsBudgetExceeded));
            OnPropertyChanged(nameof(IsBudgetCritical));
            OnPropertyChanged(nameof(HasBudgetWarning));
            OnPropertyChanged(nameof(BudgetWarningMessage));
        }

        public void LoadProjects()
        {
            try
            {
                string filterTeam = (SelectedTeamFilter == "Tüm Ekipler" || string.IsNullOrWhiteSpace(SelectedTeamFilter))
                    ? ""
                    : SelectedTeamFilter;
                var list = _services.GetProjectsByQuarter(SelectedYear, SelectedQuarter, filterTeam) ?? new List<Project>();

                int startMonth = (SelectedQuarter - 1) * 3 + 1;
                int m1 = startMonth;
                int m2 = startMonth + 1;
                int m3 = startMonth + 2;

                var displayItems = new ObservableCollection<ProjectDisplayItem>();

                foreach (var p in list)
                {
                    p.ProjectName ??= string.Empty;
                    p.Summary ??= p.ProjectName;
                    p.ProjectStatus ??= "Planlandı";
                    p.ProjectType ??= "Proje";
                    p.ExternalCompanyName ??= string.Empty;
                    p.Stakeholders ??= string.Empty;
                    p.Gmy ??= string.Empty;
                    p.BusinessUnit ??= string.Empty;
                    p.Description ??= string.Empty;
                    p.AssignedUserNames ??= string.Empty;
                    p.AssignedAnalystNames ??= string.Empty;
                    p.AssignedDeveloperNames ??= string.Empty;

                    if (string.IsNullOrWhiteSpace(p.AssignedAnalystNames) && string.IsNullOrWhiteSpace(p.AssignedDeveloperNames))
                    {
                        p.AssignedAnalystNames = p.AssignedUserNames;
                        p.AssignedDeveloperNames = "-";
                    }

                    var dbCosts = _services.GetProjectMonthlyCosts(p.Id) ?? new List<ProjectMonthlyCost>();
                    var dbAllocations = _services.GetAllocationsByProject(p.Id) ?? new List<ProjectAllocation>();

                    decimal m1Sum = dbCosts.Where(c => c.Month == m1).Sum(c => c.ManDays);
                    decimal m2Sum = dbCosts.Where(c => c.Month == m2).Sum(c => c.ManDays);
                    decimal m3Sum = dbCosts.Where(c => c.Month == m3).Sum(c => c.ManDays);

                    // Build user breakdown list for row details template
                    var userGroupIds = dbCosts.Select(c => c.UserId).Union(dbAllocations.Select(a => a.UserId)).Distinct().ToList();
                    var breakdown = new List<UserCostBreakdownRow>();
                    decimal totalPlannedInternal = m1Sum + m2Sum + m3Sum;
                    bool isCompletedProject = string.Equals(p.ProjectStatus, "Tamamlandı", StringComparison.OrdinalIgnoreCase);

                    foreach (var uid in userGroupIds)
                    {
                        var u = _allUsers.FirstOrDefault(usr => usr.Id == uid);
                        string name = u?.FullName ?? $"Personel #{uid}";
                        string role = (u?.Title ?? "").Contains("Analist", StringComparison.OrdinalIgnoreCase) ? "Analist" : "Yazılımcı";

                        decimal uM1 = dbCosts.FirstOrDefault(c => c.UserId == uid && c.Month == m1)?.ManDays ?? 0m;
                        decimal uM2 = dbCosts.FirstOrDefault(c => c.UserId == uid && c.Month == m2)?.ManDays ?? 0m;
                        decimal uM3 = dbCosts.FirstOrDefault(c => c.UserId == uid && c.Month == m3)?.ManDays ?? 0m;
                        decimal uTotalPlanned = uM1 + uM2 + uM3;

                        var alloc = dbAllocations.FirstOrDefault(a => a.UserId == uid);
                        decimal uActual = alloc != null && alloc.ActualManDay > 0m ? alloc.ActualManDay : 0m;

                        if (uActual == 0m && isCompletedProject && p.ActualManDays > 0m && userGroupIds.Count > 0)
                        {
                            if (totalPlannedInternal > 0m)
                            {
                                uActual = Math.Round((uTotalPlanned / totalPlannedInternal) * p.ActualManDays, 0);
                            }
                            else
                            {
                                uActual = Math.Round(p.ActualManDays / userGroupIds.Count, 0);
                            }
                        }

                        breakdown.Add(new UserCostBreakdownRow
                        {
                            UserId = uid,
                            FullName = name,
                            Role = role,
                            Month1ManDays = uM1,
                            Month2ManDays = uM2,
                            Month3ManDays = uM3,
                            ActualManDaysShare = uActual,
                            IsCompletedProject = isCompletedProject
                        });
                    }

                    displayItems.Add(new ProjectDisplayItem
                    {
                        Project = p,
                        Month1ManDaysTotal = m1Sum,
                        Month2ManDaysTotal = m2Sum,
                        Month3ManDaysTotal = m3Sum,
                        UserBreakdownList = breakdown
                    });
                }

                _allProjectsList = displayItems.ToList();
                FilterProjectsList();
                StatusMessage = string.Empty;
            }
            catch (Exception ex)
            {
                StatusMessage = $"Proje yükleme hatası: {ex.Message}";
            }
        }

        private List<ProjectDisplayItem> _allProjectsList = new List<ProjectDisplayItem>();

        private void FilterProjectsList()
        {
            IEnumerable<ProjectDisplayItem> query = _allProjectsList;

            // 1. Search text filter
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                string text = SearchText.Trim().ToLowerInvariant();
                query = query.Where(item =>
                    (item.Project.PergelNo > 0 && item.Project.PergelNo.ToString().Contains(text)) ||
                    (item.Project.ProjectName ?? "").ToLowerInvariant().Contains(text) ||
                    (item.Project.Summary ?? "").ToLowerInvariant().Contains(text) ||
                    (item.Project.ProjectType ?? "").ToLowerInvariant().Contains(text) ||
                    (item.Project.ExternalCompanyName ?? "").ToLowerInvariant().Contains(text) ||
                    (item.Project.AssignedAnalystNames ?? "").ToLowerInvariant().Contains(text) ||
                    (item.Project.AssignedDeveloperNames ?? "").ToLowerInvariant().Contains(text) ||
                    (item.Project.Gmy ?? "").ToLowerInvariant().Contains(text) ||
                    (item.Project.BusinessUnit ?? "").ToLowerInvariant().Contains(text)
                );
            }

            // 2. Status filter
            if (!string.IsNullOrWhiteSpace(SelectedStatusFilter) && SelectedStatusFilter != "Tüm Durumlar")
            {
                query = query.Where(item => string.Equals(item.Project.ProjectStatus, SelectedStatusFilter, StringComparison.OrdinalIgnoreCase));
            }

            // 3. Budget filter
            if (!string.IsNullOrWhiteSpace(SelectedBudgetFilter) && SelectedBudgetFilter != "Tüm Bütçe Durumları")
            {
                if (SelectedBudgetFilter == "Bütçesi Aşılanlar")
                {
                    query = query.Where(item => item.IsActualBudgetExceeded);
                }
                else if (SelectedBudgetFilter == "Bütçesi Aşılmayanlar")
                {
                    query = query.Where(item => item.IsCompleted && !item.IsActualBudgetExceeded);
                }
            }

            ProjectsList = new ObservableCollection<ProjectDisplayItem>(query.ToList());
        }

        private void LoadSelectedProject()
        {
            if (SelectedProject == null) return;

            PergelNo = SelectedProject.PergelNo > 0 ? SelectedProject.PergelNo : (1000000 + Random.Shared.Next(100000, 999999));
            ProjectName = SelectedProject.ProjectName ?? string.Empty;
            Summary = SelectedProject.Summary ?? string.Empty;
            ProjectStatus = string.IsNullOrWhiteSpace(SelectedProject.ProjectStatus) ? "Planlandı" : SelectedProject.ProjectStatus;
            ProjectType = string.IsNullOrWhiteSpace(SelectedProject.ProjectType) ? "Proje" : SelectedProject.ProjectType;
            ExternalCompanyName = SelectedProject.ExternalCompanyName ?? string.Empty;
            ExternalCost = SelectedProject.ExternalCost;
            Gmy = SelectedProject.Gmy ?? string.Empty;
            BusinessUnit = SelectedProject.BusinessUnit ?? string.Empty;
            Description = SelectedProject.Description ?? string.Empty;
            TotalManDayBudget = SelectedProject.TotalManDayBudget;
            ActualManDays = SelectedProject.ActualManDays;

            SelectedTeam = SelectedProject.Team ?? string.Empty;

            var stakeholdersList = (SelectedProject.Stakeholders ?? "").Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToHashSet();
            foreach (var sh in StakeholderItems)
            {
                sh.IsSelected = stakeholdersList.Contains(sh.Name);
            }

            var analystNames = (SelectedProject.AssignedAnalystNames ?? "").Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToHashSet();
            bool isAnalystHerkes = string.Equals(SelectedProject.AssignedAnalystNames, "Herkes", StringComparison.OrdinalIgnoreCase);

            foreach (var item in AnalystUserItems)
            {
                item.IsSelected = isAnalystHerkes || analystNames.Contains(item.User.FullName);
            }

            var devNames = (SelectedProject.AssignedDeveloperNames ?? "").Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToHashSet();
            bool isDevHerkes = string.Equals(SelectedProject.AssignedDeveloperNames, "Herkes", StringComparison.OrdinalIgnoreCase);

            foreach (var item in DeveloperUserItems)
            {
                item.IsSelected = isDevHerkes || devNames.Contains(item.User.FullName);
            }

            // Load Monthly Costs and Allocations
            var dbCosts = _services.GetProjectMonthlyCosts(SelectedProject.Id) ?? new List<ProjectMonthlyCost>();
            var dbAllocations = _services.GetAllocationsByProject(SelectedProject.Id) ?? new List<ProjectAllocation>();

            var selectedUserIds = AnalystUserItems.Concat(DeveloperUserItems).Where(i => i.IsSelected).Select(i => i.User.Id).ToHashSet();

            bool isAssigned = false;
            if (!string.IsNullOrWhiteSpace(LoggedUserName))
            {
                var assignedStr = $"{SelectedProject?.AssignedAnalystNames} | {SelectedProject?.AssignedDeveloperNames} | {SelectedProject?.AssignedUserNames}";
                if (assignedStr.Contains(LoggedUserName, StringComparison.OrdinalIgnoreCase))
                {
                    isAssigned = true;
                }
                else
                {
                    var loggedUserObj = _allUsers.FirstOrDefault(u =>
                        u.FullName.Equals(LoggedUserName, StringComparison.OrdinalIgnoreCase) ||
                        (!string.IsNullOrWhiteSpace(u.Name) && LoggedUserName.Contains(u.Name, StringComparison.OrdinalIgnoreCase)));

                    if (loggedUserObj != null)
                    {
                        isAssigned = selectedUserIds.Contains(loggedUserObj.Id);
                    }
                }
            }
            else
            {
                isAssigned = true;
            }

            var rows = new ObservableCollection<ProjectPersonMonthlyCostRow>();

            int startMonth = (SelectedQuarter - 1) * 3 + 1;
            int m1 = startMonth;
            int m2 = startMonth + 1;
            int m3 = startMonth + 2;

            foreach (var uId in selectedUserIds)
            {
                var userObj = _allUsers.FirstOrDefault(u => u.Id == uId);
                if (userObj == null) continue;

                var userDbCosts = dbCosts.Where(c => c.UserId == uId).ToList();
                decimal md1 = userDbCosts.FirstOrDefault(c => c.Month == m1)?.ManDays ?? 0m;
                decimal md2 = userDbCosts.FirstOrDefault(c => c.Month == m2)?.ManDays ?? 0m;
                decimal md3 = userDbCosts.FirstOrDefault(c => c.Month == m3)?.ManDays ?? 0m;

                var alloc = dbAllocations.FirstOrDefault(a => a.UserId == uId);
                decimal actualMd = alloc?.ActualManDay ?? 0m;

                rows.Add(new ProjectPersonMonthlyCostRow
                {
                    User = userObj,
                    Month1ManDays = md1,
                    Month2ManDays = md2,
                    Month3ManDays = md3,
                    ActualManDays = actualMd,
                    OnChangedAction = RecalculateTotals
                });
            }

            PersonMonthlyCostRows = rows;
            RefreshAssigneesAndCostRows();
        }

        private void ExecuteSaveProject(object? param)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ProjectName))
                {
                    MessageBox.Show("Lütfen Proje Adı / Talep Özeti alanını doldurunuz.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                RefreshAssigneesAndCostRows();

                var selectedStakeholders = StakeholderItems.Where(s => s.IsSelected).Select(s => s.Name).ToList();
                string stakeholdersStr = string.Join(", ", selectedStakeholders);

                var p = SelectedProject ?? new Project();
                p.PergelNo = PergelNo;
                p.ProjectName = ProjectName;
                p.Summary = string.IsNullOrWhiteSpace(Summary) ? ProjectName : Summary;
                p.ProjectStatus = ProjectStatus;
                p.ProjectType = ProjectType;
                p.ExternalCompanyName = string.Equals(ProjectType, "Dış Firma", StringComparison.OrdinalIgnoreCase) ? ExternalCompanyName : string.Empty;
                p.ExternalCost = ExternalCost;
                p.Team = !string.IsNullOrWhiteSpace(SelectedTeam) ? SelectedTeam : (LoggedTeam ?? "Takip");
                p.Year = SelectedYear;
                p.Quarter = SelectedQuarter;
                p.Stakeholders = stakeholdersStr;
                p.Gmy = Gmy;
                p.BusinessUnit = BusinessUnit;
                p.Description = Description;
                p.AssignedAnalystNames = DisplayAnalystSummary;
                p.AssignedDeveloperNames = DisplayDeveloperSummary;
                p.AssignedUserNames = $"{DisplayAnalystSummary} | {DisplayDeveloperSummary}";
                p.ActualManDays = ActualManDays;

                // If TotalManDayBudget is not set manually, default to TotalInternalManDays
                if (TotalManDayBudget <= 0m && TotalInternalManDays > 0m)
                {
                    TotalManDayBudget = TotalInternalManDays;
                }
                p.TotalManDayBudget = TotalManDayBudget;

                if (IsCompletedStatus && p.ActualManDays > 0m)
                {
                    decimal budget = p.TotalManDayBudget > 0 ? p.TotalManDayBudget : TotalInternalManDays;
                    if (budget > 0m && p.ActualManDays > budget)
                    {
                        decimal excess = p.ActualManDays - budget;
                        var confirm = MessageBox.Show(
                            $"⚠️ TAMAMLANAN PROJEDE BÜTÇE AŞIMI VAR!\n\nProje Bütçesi: {budget:N0} Adam/Gün\nHarcanan Gerçekleşen Efor: {p.ActualManDays:N0} Adam/Gün\n\n{excess:N0} gün BÜTÇE AŞIMINIZ bulunmaktadır.\n\nYine de kaydetmek istiyor musunuz?",
                            "Bütçe Aşımı Bildirimi",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Warning);

                        if (confirm != MessageBoxResult.Yes) return;
                    }
                }

                if (IsBudgetExceeded)
                {
                    decimal excess = TotalInternalManDays - TotalManDayBudget;
                    var confirm = MessageBox.Show(
                        $"⚠️ BÜTÇE AŞIMI TESPİT EDİLDİ!\n\nGirilen toplam adam/gün maliyeti ({TotalInternalManDays:N0}), tanımlanan proje bütçesini ({TotalManDayBudget:N0}) {excess:N0} adam/gün AŞMAKTADIR.\n\nYine de bu kaydı bu şekilde onaylayıp kaydetmek istiyor musunuz?",
                        "Bütçe Aşımı Uyarısı",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning);

                    if (confirm != MessageBoxResult.Yes) return;
                }

                int startMonth = (SelectedQuarter - 1) * 3 + 1;
                var monthlyCosts = new List<ProjectMonthlyCost>();
                var allocations = new List<ProjectAllocation>();

                foreach (var row in PersonMonthlyCostRows)
                {
                    monthlyCosts.Add(new ProjectMonthlyCost { UserId = row.User.Id, Month = startMonth, ManDays = row.Month1ManDays });
                    monthlyCosts.Add(new ProjectMonthlyCost { UserId = row.User.Id, Month = startMonth + 1, ManDays = row.Month2ManDays });
                    monthlyCosts.Add(new ProjectMonthlyCost { UserId = row.User.Id, Month = startMonth + 2, ManDays = row.Month3ManDays });

                    allocations.Add(new ProjectAllocation
                    {
                        UserId = row.User.Id,
                        AllocatedManDay = row.TotalManDays,
                        ActualManDay = row.ActualManDays
                    });
                }

                _services.SaveProjectWithMonthlyCosts(p, monthlyCosts, allocations, LoggedUserName);

                StatusMessage = $"✅ '{p.ProjectName}' projesi başarıyla kaydedildi.";
                ClearForm();
                IsFormOpen = false;
                LoadProjects();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kaydetme Hatası: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteSaveRowItem(ProjectDisplayItem item)
        {
            try
            {
                if (item?.Project == null) return;
                var p = item.Project;
                if (string.IsNullOrWhiteSpace(p.ProjectName))
                {
                    MessageBox.Show("Lütfen Proje Adı alanını doldurunuz.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                int startMonth = (SelectedQuarter - 1) * 3 + 1;
                var monthlyCosts = new List<ProjectMonthlyCost>();
                var allocations = new List<ProjectAllocation>();

                if (item.UserBreakdownList != null && item.UserBreakdownList.Any())
                {
                    foreach (var row in item.UserBreakdownList)
                    {
                        monthlyCosts.Add(new ProjectMonthlyCost { UserId = row.UserId, Month = startMonth, ManDays = row.Month1ManDays });
                        monthlyCosts.Add(new ProjectMonthlyCost { UserId = row.UserId, Month = startMonth + 1, ManDays = row.Month2ManDays });
                        monthlyCosts.Add(new ProjectMonthlyCost { UserId = row.UserId, Month = startMonth + 2, ManDays = row.Month3ManDays });

                        allocations.Add(new ProjectAllocation
                        {
                            UserId = row.UserId,
                            AllocatedManDay = row.TotalManDays,
                            ActualManDay = row.ActualManDaysShare
                        });
                    }
                }

                _services.SaveProjectWithMonthlyCosts(p, monthlyCosts, allocations, LoggedUserName);
                StatusMessage = $"✅ '{p.ProjectName}' projesi başarıyla kaydedildi.";
                item.IsEditing = false;
                LoadProjects();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kaydetme Hatası: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteDeleteProject(object? param)
        {
            if (SelectedProject == null) return;
            var confirm = MessageBox.Show($"'{SelectedProject.ProjectName}' projesini silmek istediğinize emin misiniz?", "Silme Onayı", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (confirm == MessageBoxResult.Yes)
            {
                try
                {
                    _services.DeleteProject(SelectedProject.Id);
                    StatusMessage = "🗑️ Proje kaydı silindi.";
                    ClearForm();
                    IsFormOpen = false;
                    LoadProjects();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Silme Hatası: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ClearForm()
        {
            _selectedProject = null;
            _selectedProjectDisplay = null;
            OnPropertyChanged(nameof(SelectedProject));
            OnPropertyChanged(nameof(SelectedProjectDisplay));
            OnPropertyChanged(nameof(FormTitleText));
            OnPropertyChanged(nameof(SaveButtonText));

            PergelNo = 1000000 + Random.Shared.Next(100000, 999999);
            ProjectName = string.Empty;
            Summary = string.Empty;
            ProjectStatus = "Planlandı";
            ProjectType = "Proje";
            ExternalCompanyName = string.Empty;
            ExternalCost = 0m;
            Gmy = string.Empty;
            BusinessUnit = string.Empty;
            Description = string.Empty;
            SelectedTeam = string.Empty;
            TotalManDayBudget = 0m;
            ActualManDays = 0m;

            foreach (var item in AnalystUserItems) item.IsSelected = false;
            foreach (var item in DeveloperUserItems) item.IsSelected = false;
            foreach (var item in StakeholderItems) item.IsSelected = false;

            PersonMonthlyCostRows.Clear();
            DisplayAnalystSummary = "Seçilmedi";
            DisplayDeveloperSummary = "Seçilmedi";
            RecalculateTotals();
        }
    }
}
