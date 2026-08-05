using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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

        public string DisplayNameWithTitle => User != null 
            ? (string.IsNullOrWhiteSpace(User.Title) ? User.FullName : $"{User.FullName} ({User.Title})") 
            : string.Empty;

        public int MaxMonth1Days { get; set; } = 31;
        public int MaxMonth2Days { get; set; } = 31;
        public int MaxMonth3Days { get; set; } = 31;

        public string Month1Header { get; set; } = "1. Ay";
        public string Month2Header { get; set; } = "2. Ay";
        public string Month3Header { get; set; } = "3. Ay";

        private decimal _month1ManDays;
        public decimal Month1ManDays
        {
            get => _month1ManDays;
            set
            {
                if (value < 0m) value = 0m;

                if (_month1ManDays != value)
                {
                    _month1ManDays = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(TotalManDays));
                    OnChangedAction?.Invoke();
                }
            }
        }

        private decimal _month2ManDays;
        public decimal Month2ManDays
        {
            get => _month2ManDays;
            set
            {
                if (value < 0m) value = 0m;

                if (_month2ManDays != value)
                {
                    _month2ManDays = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(TotalManDays));
                    OnChangedAction?.Invoke();
                }
            }
        }

        private decimal _month3ManDays;
        public decimal Month3ManDays
        {
            get => _month3ManDays;
            set
            {
                if (value < 0m) value = 0m;

                if (_month3ManDays != value)
                {
                    _month3ManDays = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(TotalManDays));
                    OnChangedAction?.Invoke();
                }
            }
        }

        private decimal _actualManDays;
        public decimal ActualManDays
        {
            get => _actualManDays;
            set
            {
                if (value < 0m) value = 0m;
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
        public string ProjectStatus
        {
            get => Project?.ProjectStatus ?? "Planlandı";
            set
            {
                if (Project != null && Project.ProjectStatus != value)
                {
                    Project.ProjectStatus = value;
                    if (!string.Equals(value, "Tamamlandı", StringComparison.OrdinalIgnoreCase))
                    {
                        _overrideAnalystActual = 0m;
                        _overrideDeveloperActual = 0m;
                        _overrideTotalActual = 0m;
                        Project.ActualManDays = 0m;
                        Project.ActualReleaseDate = null;
                        if (UserBreakdownList != null)
                        {
                            foreach (var r in UserBreakdownList)
                            {
                                r.ActualManDaysShare = 0m;
                                r.IsCompletedProject = false;
                            }
                        }
                        OnPropertyChanged(nameof(ActualReleaseDate));
                    }
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsCompleted));
                    OnPropertyChanged(nameof(ActualManDays));
                    OnPropertyChanged(nameof(AnalystActualManDays));
                    OnPropertyChanged(nameof(DeveloperActualManDays));
                    OnPropertyChanged(nameof(BudgetStatusBadge));
                    OnPropertyChanged(nameof(BudgetStatusColor));
                    OnPropertyChanged(nameof(BudgetStatusBg));
                    OnPropertyChanged(nameof(IsAnyBudgetExceeded));
                    OnPropertyChanged(nameof(BudgetExcessDays));
                    OnPropertyChanged(nameof(IsCostOverrunWarningVisible));
                    OnPropertyChanged(nameof(CostWarningMessage));
                }
            }
        }

        public DateTime? PlannedReleaseDate
        {
            get => Project?.PlannedReleaseDate;
            set
            {
                if (Project != null && Project.PlannedReleaseDate != value)
                {
                    Project.PlannedReleaseDate = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(ReleaseDelayDays));
                    OnPropertyChanged(nameof(HasReleaseDelay));
                    OnPropertyChanged(nameof(ReleaseDelayMessage));
                    OnPropertyChanged(nameof(ReleaseDelayBannerText));
                }
            }
        }

        public DateTime? ActualReleaseDate
        {
            get => (Project != null && !string.Equals(Project.ProjectStatus, "Tamamlandı", StringComparison.OrdinalIgnoreCase)) ? null : Project?.ActualReleaseDate;
            set
            {
                if (Project != null && Project.ActualReleaseDate != value)
                {
                    Project.ActualReleaseDate = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(ReleaseDelayDays));
                    OnPropertyChanged(nameof(HasReleaseDelay));
                    OnPropertyChanged(nameof(ReleaseDelayMessage));
                    OnPropertyChanged(nameof(ReleaseDelayBannerText));
                }
            }
        }

        public int ReleaseDelayDays
        {
            get
            {
                if (PlannedReleaseDate.HasValue && ActualReleaseDate.HasValue && ActualReleaseDate.Value > PlannedReleaseDate.Value)
                {
                    return (int)(ActualReleaseDate.Value - PlannedReleaseDate.Value).TotalDays;
                }
                return 0;
            }
        }

        public bool HasReleaseDelay => ReleaseDelayDays > 0;

        public string ReleaseDelayMessage => HasReleaseDelay ? $"⚠️ {ReleaseDelayDays} gün gecikme var" : string.Empty;

        public string ReleaseDelayBannerText => HasReleaseDelay 
            ? $"⚠️ SÜRÜM GEÇİŞİ GECİKTİ: Planlanan sürüm tarihinden {ReleaseDelayDays} gün sonra geçiş yapılmıştır! (Planlanan: {PlannedReleaseDate:dd.MM.yyyy}, Geçiş: {ActualReleaseDate:dd.MM.yyyy})" 
            : string.Empty;

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
                if (IsEditing) TotalManDayBudget = Month1ManDaysTotal + Month2ManDaysTotal + Month3ManDaysTotal;
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
                if (IsEditing) TotalManDayBudget = Month1ManDaysTotal + Month2ManDaysTotal + Month3ManDaysTotal;
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
                if (IsEditing) TotalManDayBudget = Month1ManDaysTotal + Month2ManDaysTotal + Month3ManDaysTotal;
                OnPropertyChanged(nameof(TotalInternalManDays));
                OnPropertyChanged(nameof(EffectiveBudget));
                OnPropertyChanged(nameof(BudgetStatusBadge));
            }
        }

        private decimal? _overrideTotalCost;
        public decimal TotalInternalManDays
        {
            get
            {
                decimal monthSum = Month1ManDaysTotal + Month2ManDaysTotal + Month3ManDaysTotal;
                if (monthSum > 0m) return monthSum;
                if (_overrideTotalCost.HasValue) return _overrideTotalCost.Value;
                
                decimal roleSum = AnalystPlannedManDays + DeveloperPlannedManDays;
                if (roleSum > 0m) return roleSum;

                return TotalManDayBudget;
            }
            set
            {
                _overrideTotalCost = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(AllocatedPlannedCostSum));
                OnPropertyChanged(nameof(IsCostMismatched));
                OnPropertyChanged(nameof(CostMismatchWarningMessage));
                OnPropertyChanged(nameof(EffectiveBudget));
                OnPropertyChanged(nameof(IsCostOverrunWarningVisible));
                OnPropertyChanged(nameof(CostWarningMessage));
                OnPropertyChanged(nameof(BudgetStatusBadge));
            }
        }

        public decimal TotalManDayBudget
        {
            get => Project?.TotalManDayBudget ?? 0m;
            set
            {
                if (Project != null && Project.TotalManDayBudget != value)
                {
                    Project.TotalManDayBudget = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(TotalInternalManDays));
                    OnPropertyChanged(nameof(AllocatedPlannedCostSum));
                    OnPropertyChanged(nameof(IsCostMismatched));
                    OnPropertyChanged(nameof(CostMismatchWarningMessage));
                    OnPropertyChanged(nameof(EffectiveBudget));
                }
            }
        }

        public decimal AllocatedPlannedCostSum => AnalystPlannedManDays + DeveloperPlannedManDays;

        public bool IsCostMismatched
        {
            get
            {
                decimal target = _overrideTotalCost.HasValue ? _overrideTotalCost.Value : TotalInternalManDays;
                decimal allocated = AllocatedPlannedCostSum;
                return target > 0m && allocated > 0m && Math.Abs(target - allocated) > 0.5m;
            }
        }

        public string CostMismatchWarningMessage
        {
            get
            {
                if (IsCostMismatched)
                {
                    decimal target = _overrideTotalCost.HasValue ? _overrideTotalCost.Value : TotalInternalManDays;
                    decimal allocated = AllocatedPlannedCostSum;
                    decimal diff = target - allocated;
                    if (diff > 0)
                        return $"⚠️ Eksik Maliyet Girişi! (Hedef Toplam: {target:N0} Gün, Dağıtılan: {allocated:N0} Gün, Kalan Dağıtılacak: {diff:N0} Gün)";
                    else
                        return $"⚠️ Fazla Maliyet Girişi! (Hedef Toplam: {target:N0} Gün, Dağıtılan: {allocated:N0} Gün, Fazlalık: {Math.Abs(diff):N0} Gün)";
                }
                return string.Empty;
            }
        }

        public List<UserCostBreakdownRow> UserBreakdownList { get; set; } = new List<UserCostBreakdownRow>();

        public ObservableCollection<SelectableUserItem> AnalystUserItems { get; set; } = new ObservableCollection<SelectableUserItem>();
        public ObservableCollection<SelectableUserItem> DeveloperUserItems { get; set; } = new ObservableCollection<SelectableUserItem>();
        public ObservableCollection<SelectableStakeholderItem> StakeholderItems { get; set; } = new ObservableCollection<SelectableStakeholderItem>();

        public void UpdateStakeholdersFromItems()
        {
            var selected = StakeholderItems.Where(s => s.IsSelected).Select(s => s.Name).ToList();
            if (selected.Any())
            {
                Project.Stakeholders = string.Join(", ", selected);
            }
            else
            {
                Project.Stakeholders = string.Empty;
            }
            OnPropertyChanged(nameof(Project));
        }



        public void UpdateAssignedAnalystNamesFromItems()
        {
            var selected = AnalystUserItems.Where(i => i.IsSelected).Select(i => i.User.FullName).ToList();
            if (AnalystUserItems.Any() && selected.Count == AnalystUserItems.Count)
            {
                Project.AssignedAnalystNames = "Herkes";
            }
            else if (selected.Any())
            {
                Project.AssignedAnalystNames = string.Join(", ", selected);
            }
            else if (string.IsNullOrWhiteSpace(Project?.AssignedAnalystNames))
            {
                if (Project != null) Project.AssignedAnalystNames = "Seçilmedi";
            }
            OnPropertyChanged(nameof(AssignedAnalystNames));
            OnPropertyChanged(nameof(FormattedAnalystNames));
        }

        public void UpdateAssignedDeveloperNamesFromItems()
        {
            var selected = DeveloperUserItems.Where(i => i.IsSelected).Select(i => i.User.FullName).ToList();
            if (DeveloperUserItems.Any() && selected.Count == DeveloperUserItems.Count)
            {
                Project.AssignedDeveloperNames = "Herkes";
            }
            else if (selected.Any())
            {
                Project.AssignedDeveloperNames = string.Join(", ", selected);
            }
            else if (string.IsNullOrWhiteSpace(Project?.AssignedDeveloperNames))
            {
                if (Project != null) Project.AssignedDeveloperNames = "Seçilmedi";
            }
            OnPropertyChanged(nameof(AssignedDeveloperNames));
            OnPropertyChanged(nameof(FormattedDeveloperNames));
        }

        public string FormattedAnalystNames => ProjectsViewModel.FormatNamesWithAbbreviatedSurname(Project?.AssignedAnalystNames);
        public string FormattedDeveloperNames => ProjectsViewModel.FormatNamesWithAbbreviatedSurname(Project?.AssignedDeveloperNames);

        public string DisplayAnalystSummary
        {
            get
            {
                var names = FormattedAnalystNames;
                if (string.IsNullOrWhiteSpace(names) || names == "-")
                {
                    names = Project?.AssignedAnalystNames;
                }
                return string.IsNullOrWhiteSpace(names) || names == "-" ? "Seçilmedi" : names;
            }
        }

        public string DisplayDeveloperSummary
        {
            get
            {
                var names = FormattedDeveloperNames;
                if (string.IsNullOrWhiteSpace(names) || names == "-")
                {
                    names = Project?.AssignedDeveloperNames;
                }
                return string.IsNullOrWhiteSpace(names) || names == "-" ? "Seçilmedi" : names;
            }
        }

        public void NotifyAllPropertiesChanged()
        {
            OnPropertyChanged(nameof(Project));
            OnPropertyChanged(nameof(Month1ManDaysTotal));
            OnPropertyChanged(nameof(Month2ManDaysTotal));
            OnPropertyChanged(nameof(Month3ManDaysTotal));
            OnPropertyChanged(nameof(TotalInternalManDays));
            OnPropertyChanged(nameof(AnalystPlannedManDays));
            OnPropertyChanged(nameof(DeveloperPlannedManDays));
            OnPropertyChanged(nameof(AnalystActualManDays));
            OnPropertyChanged(nameof(DeveloperActualManDays));
            OnPropertyChanged(nameof(BudgetStatusBadge));
            OnPropertyChanged(nameof(BudgetStatusColor));
            OnPropertyChanged(nameof(BudgetStatusBg));
            OnPropertyChanged(nameof(FormattedAnalystNames));
            OnPropertyChanged(nameof(FormattedDeveloperNames));
            OnPropertyChanged(nameof(DisplayAnalystSummary));
            OnPropertyChanged(nameof(DisplayDeveloperSummary));
            OnPropertyChanged(nameof(UserBreakdownList));
        }

        public string AssignedAnalystNames
        {
            get => Project?.AssignedAnalystNames ?? string.Empty;
            set
            {
                if (Project != null) Project.AssignedAnalystNames = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(FormattedAnalystNames));
            }
        }

        public string AssignedDeveloperNames
        {
            get => Project?.AssignedDeveloperNames ?? string.Empty;
            set
            {
                if (Project != null) Project.AssignedDeveloperNames = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(FormattedDeveloperNames));
                OnPropertyChanged(nameof(AnalystPlannedManDays));
                OnPropertyChanged(nameof(DeveloperPlannedManDays));
            }
        }

        private decimal _analystPlannedManDays;
        public decimal AnalystPlannedManDays
        {
            get => _analystPlannedManDays;
            set
            {
                if (_analystPlannedManDays != value)
                {
                    _analystPlannedManDays = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(TotalInternalManDays));
                    OnPropertyChanged(nameof(AllocatedPlannedCostSum));
                    OnPropertyChanged(nameof(IsCostMismatched));
                    OnPropertyChanged(nameof(CostMismatchWarningMessage));
                    OnPropertyChanged(nameof(EffectiveBudget));
                    OnPropertyChanged(nameof(IsCostOverrunWarningVisible));
                    OnPropertyChanged(nameof(CostWarningMessage));
                    OnPropertyChanged(nameof(MaxActualOrPlannedCost));
                    OnPropertyChanged(nameof(IsAnyBudgetExceeded));
                    OnPropertyChanged(nameof(BudgetExcessDays));
                    OnPropertyChanged(nameof(BudgetStatusBadge));
                    OnPropertyChanged(nameof(BudgetStatusColor));
                    OnPropertyChanged(nameof(BudgetStatusBg));
                }
            }
        }

        private decimal _developerPlannedManDays;
        public decimal DeveloperPlannedManDays
        {
            get => _developerPlannedManDays;
            set
            {
                if (_developerPlannedManDays != value)
                {
                    _developerPlannedManDays = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(TotalInternalManDays));
                    OnPropertyChanged(nameof(AllocatedPlannedCostSum));
                    OnPropertyChanged(nameof(IsCostMismatched));
                    OnPropertyChanged(nameof(CostMismatchWarningMessage));
                    OnPropertyChanged(nameof(EffectiveBudget));
                    OnPropertyChanged(nameof(IsCostOverrunWarningVisible));
                    OnPropertyChanged(nameof(CostWarningMessage));
                    OnPropertyChanged(nameof(MaxActualOrPlannedCost));
                    OnPropertyChanged(nameof(IsAnyBudgetExceeded));
                    OnPropertyChanged(nameof(BudgetExcessDays));
                    OnPropertyChanged(nameof(BudgetStatusBadge));
                    OnPropertyChanged(nameof(BudgetStatusColor));
                    OnPropertyChanged(nameof(BudgetStatusBg));
                }
            }
        }

        private decimal? _overrideAnalystActual;
        public decimal AnalystActualManDays
        {
            get
            {
                if (_overrideAnalystActual.HasValue) return _overrideAnalystActual.Value;
                return Project?.AnalystActualManDays ?? 0m;
            }
            set
            {
                _overrideAnalystActual = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DeveloperActualManDays));
                OnPropertyChanged(nameof(ActualManDays));
                OnPropertyChanged(nameof(MaxActualOrPlannedCost));
                OnPropertyChanged(nameof(IsAnyBudgetExceeded));
                OnPropertyChanged(nameof(BudgetExcessDays));
                OnPropertyChanged(nameof(BudgetStatusBadge));
                OnPropertyChanged(nameof(BudgetStatusColor));
                OnPropertyChanged(nameof(BudgetStatusBg));
                OnPropertyChanged(nameof(OverrunSummaryText));
            }
        }

        private decimal? _overrideDeveloperActual;
        public decimal DeveloperActualManDays
        {
            get
            {
                if (_overrideDeveloperActual.HasValue) return _overrideDeveloperActual.Value;
                return Project?.DeveloperActualManDays ?? 0m;
            }
            set
            {
                _overrideDeveloperActual = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(AnalystActualManDays));
                OnPropertyChanged(nameof(ActualManDays));
                OnPropertyChanged(nameof(MaxActualOrPlannedCost));
                OnPropertyChanged(nameof(IsAnyBudgetExceeded));
                OnPropertyChanged(nameof(BudgetExcessDays));
                OnPropertyChanged(nameof(BudgetStatusBadge));
                OnPropertyChanged(nameof(BudgetStatusColor));
                OnPropertyChanged(nameof(BudgetStatusBg));
                OnPropertyChanged(nameof(OverrunSummaryText));
            }
        }

        public bool HasAnalystActualOverride => _overrideAnalystActual.HasValue;
        public bool HasDeveloperActualOverride => _overrideDeveloperActual.HasValue;

        private decimal? _overrideTotalActual;
        public decimal ActualManDays
        {
            get
            {
                if (_overrideTotalActual.HasValue) return _overrideTotalActual.Value;
                if (_overrideAnalystActual.HasValue || _overrideDeveloperActual.HasValue)
                {
                    return (_overrideAnalystActual ?? (Project?.AnalystActualManDays ?? 0m)) + (_overrideDeveloperActual ?? (Project?.DeveloperActualManDays ?? 0m));
                }
                return Project?.ActualManDays ?? 0m;
            }
            set
            {
                _overrideTotalActual = value;
                if (Project != null) Project.ActualManDays = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(AnalystActualManDays));
                OnPropertyChanged(nameof(DeveloperActualManDays));
                OnPropertyChanged(nameof(MaxActualOrPlannedCost));
                OnPropertyChanged(nameof(IsAnyBudgetExceeded));
                OnPropertyChanged(nameof(BudgetExcessDays));
                OnPropertyChanged(nameof(BudgetStatusBadge));
                OnPropertyChanged(nameof(BudgetStatusColor));
                OnPropertyChanged(nameof(BudgetStatusBg));
            }
        }

        public decimal TargetManDayBudget => Project?.TotalManDayBudget > 0 ? Project.TotalManDayBudget : TotalInternalManDays;

        public decimal MaxActualOrPlannedCost => Math.Max(TotalInternalManDays, ActualManDays);

        public bool IsAnyBudgetExceeded => EffectiveBudget > 0m && Math.Round(ActualManDays, 1) > Math.Round(EffectiveBudget, 1);

        public decimal BudgetExcessDays => IsAnyBudgetExceeded ? (Math.Round(ActualManDays, 1) - Math.Round(EffectiveBudget, 1)) : 0m;

        public bool IsCostOverrunWarningVisible => IsAnyBudgetExceeded;

        public string CostWarningMessage
        {
            get
            {
                if (IsCostOverrunWarningVisible)
                {
                    return $"⚠️ Gerçekleşen efor bütçeyi aştı! (Bütçe: {EffectiveBudget:N0} Gün, Harcanan Efor: {ActualManDays:N0} Gün, Aşım: +{BudgetExcessDays:N0} Gün)";
                }
                return string.Empty;
            }
        }
        public bool IsCompleted => string.Equals(Project?.ProjectStatus, "Tamamlandı", StringComparison.OrdinalIgnoreCase);
        public decimal EffectiveBudget => TargetManDayBudget;
        public decimal VarianceDays => ActualManDays - EffectiveBudget;

        public bool IsActualBudgetExceeded => IsAnyBudgetExceeded;
        public bool IsActualBudgetWithin => IsCompleted && EffectiveBudget > 0 && ActualManDays <= EffectiveBudget;

        public string BudgetStatusBadge
        {
            get
            {
                if (IsAnyBudgetExceeded)
                {
                    return $"⚠️ Bütçe Aşıldı (+{BudgetExcessDays:N0} Gün)";
                }

                decimal monthSum = Month1ManDaysTotal + Month2ManDaysTotal + Month3ManDaysTotal;
                decimal roleSum = AnalystPlannedManDays + DeveloperPlannedManDays;

                if (IsEditing && monthSum > 0 && roleSum > 0 && Math.Abs(monthSum - roleSum) > 0.01m)
                {
                    return $"⚠️ Dağılım Uyuşmazlığı ({monthSum:N0} / {roleSum:N0})";
                }

                if (IsCompleted)
                {
                    if (ActualManDays == 0m)
                    {
                        return "Efor Girilmedi";
                    }
                    if (ActualManDays < EffectiveBudget)
                    {
                        decimal savings = EffectiveBudget - ActualManDays;
                        return $"✅ Bütçe Uygun (-{savings:N0} Gün)";
                    }
                    return "✅ Bütçe Tam Uygun";
                }
                return EffectiveBudget > 0 ? $"Bütçe: {EffectiveBudget:N0} Gün" : "-";
            }
        }

        public string BudgetStatusColor
        {
            get
            {
                if (IsAnyBudgetExceeded)
                {
                    return "#dc2626";
                }
                decimal monthSum = Month1ManDaysTotal + Month2ManDaysTotal + Month3ManDaysTotal;
                decimal roleSum = AnalystPlannedManDays + DeveloperPlannedManDays;

                if (IsEditing && monthSum > 0 && roleSum > 0 && Math.Abs(monthSum - roleSum) > 0.01m)
                {
                    return "#d97706";
                }
                if (IsCompleted)
                {
                    if (ActualManDays == 0m) return "#d97706";
                    return "#166534";
                }
                return "#475569";
            }
        }

        public string BudgetStatusBg
        {
            get
            {
                if (IsAnyBudgetExceeded)
                {
                    return "#fee2e2";
                }
                decimal monthSum = Month1ManDaysTotal + Month2ManDaysTotal + Month3ManDaysTotal;
                decimal roleSum = AnalystPlannedManDays + DeveloperPlannedManDays;

                if (IsEditing && monthSum > 0 && roleSum > 0 && Math.Abs(monthSum - roleSum) > 0.01m)
                {
                    return "#fef3c7";
                }
                if (IsCompleted)
                {
                    if (ActualManDays == 0m) return "#fef3c7";
                    return "#dcfce7";
                }
                return "#f1f5f9";
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

        public static bool IsNameMatched(string fullName, IEnumerable<string> namesSet)
        {
            if (string.IsNullOrWhiteSpace(fullName) || namesSet == null) return false;
            string abbreviated = FormatNamesWithAbbreviatedSurname(fullName);
            foreach (var n in namesSet)
            {
                if (string.IsNullOrWhiteSpace(n)) continue;
                if (string.Equals(n, "Herkes", StringComparison.OrdinalIgnoreCase)) return true;
                if (string.Equals(fullName, n, StringComparison.OrdinalIgnoreCase)) return true;
                if (string.Equals(abbreviated, n, StringComparison.OrdinalIgnoreCase)) return true;

                var parts1 = fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var parts2 = n.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (parts1.Length > 0 && parts2.Length > 0 && string.Equals(parts1[0], parts2[0], StringComparison.OrdinalIgnoreCase))
                {
                    if (parts1.Length == 1 || parts2.Length == 1) return true;
                    if (parts1[1][0] == parts2[1][0]) return true;
                }
            }
            return false;
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

        public ObservableCollection<string> TeamOptions { get; } = new ObservableCollection<string>
        {
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

        public (int max1, int max2, int max3) GetQuarterMonthMaxDays()
        {
            int year = SelectedYear > 0 ? SelectedYear : DateTime.Today.Year;
            int q = SelectedQuarter >= 1 && SelectedQuarter <= 4 ? SelectedQuarter : 3;
            int m1 = (q - 1) * 3 + 1;
            int m2 = (q - 1) * 3 + 2;
            int m3 = (q - 1) * 3 + 3;
            return (
                DateTime.DaysInMonth(year, m1),
                DateTime.DaysInMonth(year, m2),
                DateTime.DaysInMonth(year, m3)
            );
        }

        private void UpdateQuarterMonthNames()
        {
            var currentTab = QuarterTabs.FirstOrDefault(q => q.Quarter == SelectedQuarter) ?? QuarterTabs[2];
            Month1Header = currentTab.Months[0];
            Month2Header = currentTab.Months[1];
            Month3Header = currentTab.Months[2];

            var (max1, max2, max3) = GetQuarterMonthMaxDays();
            if (PersonMonthlyCostRows != null)
            {
                foreach (var row in PersonMonthlyCostRows)
                {
                    row.MaxMonth1Days = max1;
                    row.MaxMonth2Days = max2;
                    row.MaxMonth3Days = max3;
                    row.Month1Header = Month1Header;
                    row.Month2Header = Month2Header;
                    row.Month3Header = Month3Header;
                }
            }
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
                OnPropertyChanged(nameof(UserBreakdownList));
                if (_selectedProjectDisplay != null)
                {
                    SelectedProject = _selectedProjectDisplay.Project;
                    if (!_selectedProjectDisplay.IsEditing)
                    {
                        IsDetailPanelVisible = true;
                    }
                    else
                    {
                        IsDetailPanelVisible = false;
                    }
                    IsNewProjectMode = false;
                    IsEditMode = false;
                }
                else
                {
                    IsDetailPanelVisible = false;
                    IsEditMode = false;
                }
            }
        }

        public List<UserCostBreakdownRow> UserBreakdownList => SelectedProjectDisplay?.UserBreakdownList ?? new List<UserCostBreakdownRow>();

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
        public ObservableCollection<string> StatusList { get; } = new ObservableCollection<string> { "Planlandı", "Devam Ediyor", "Tamamlandı", "İptal" };
        public ObservableCollection<string> TypeList { get; } = new ObservableCollection<string> { "Proje", "KG", "Paydaş Proje", "Dış Firma" };
        
        public ObservableCollection<string> StatusOptions => StatusList;
        public ObservableCollection<string> ProjectTypeOptions => TypeList;
        public ObservableCollection<string> GmyOptions => GmyList;
        public ObservableCollection<string> BusinessUnitOptions => BusinessUnitList;

        public ObservableCollection<string> GmyList { get; } = new ObservableCollection<string>
        {
            "Kredi Politikaları ve Risk Tasfiye GMY",
            "Kredi Tahsis ve Yönetimi GMY",
            "Ürün Yönetimi ve Dijital Bankacılık GMY",
            "Strateji Planlama ve İnsan Kaynakları Grup Başkanlığı",
            "Genel Müdürlük"
        };

        public ObservableCollection<string> BusinessUnitList { get; } = new ObservableCollection<string>
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

        private DateTime? _plannedReleaseDate;
        public DateTime? PlannedReleaseDate
        {
            get => _plannedReleaseDate;
            set
            {
                _plannedReleaseDate = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ReleaseDelayDays));
                OnPropertyChanged(nameof(HasReleaseDelay));
                OnPropertyChanged(nameof(ReleaseDelayMessage));
                OnPropertyChanged(nameof(ReleaseDelayBannerText));
            }
        }

        private DateTime? _actualReleaseDate;
        public DateTime? ActualReleaseDate
        {
            get => !string.Equals(ProjectStatus, "Tamamlandı", StringComparison.OrdinalIgnoreCase) ? null : _actualReleaseDate;
            set
            {
                _actualReleaseDate = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ReleaseDelayDays));
                OnPropertyChanged(nameof(HasReleaseDelay));
                OnPropertyChanged(nameof(ReleaseDelayMessage));
                OnPropertyChanged(nameof(ReleaseDelayBannerText));
            }
        }

        public int ReleaseDelayDays
        {
            get
            {
                if (PlannedReleaseDate.HasValue && ActualReleaseDate.HasValue && ActualReleaseDate.Value > PlannedReleaseDate.Value)
                {
                    return (int)(ActualReleaseDate.Value - PlannedReleaseDate.Value).TotalDays;
                }
                return 0;
            }
        }

        public bool HasReleaseDelay => ReleaseDelayDays > 0;

        public string ReleaseDelayMessage => HasReleaseDelay ? $"⚠️ {ReleaseDelayDays} gün gecikme var" : string.Empty;

        public string ReleaseDelayBannerText => HasReleaseDelay 
            ? $"⚠️ SÜRÜM GEÇİŞİ GECİKTİ: Planlanan sürüm tarihinden {ReleaseDelayDays} gün sonra geçiş yapılmıştır! (Planlanan: {PlannedReleaseDate:dd.MM.yyyy}, Geçiş: {ActualReleaseDate:dd.MM.yyyy})" 
            : string.Empty;

        private string _projectStatus = "Planlandı";
        public string ProjectStatus
        {
            get => _projectStatus;
            set
            {
                _projectStatus = value;
                if (!string.Equals(value, "Tamamlandı", StringComparison.OrdinalIgnoreCase))
                {
                    ActualManDays = 0m;
                    ActualReleaseDate = null;
                    if (PersonMonthlyCostRows != null)
                    {
                        foreach (var r in PersonMonthlyCostRows)
                        {
                            r.ActualManDays = 0m;
                        }
                    }
                }
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsCompletedStatus));
                OnPropertyChanged(nameof(ActualBudgetFeedbackMessage));
                OnPropertyChanged(nameof(ActualBudgetFeedbackColor));
                OnPropertyChanged(nameof(ActualBudgetFeedbackBg));
            }
        }

        public bool IsCompletedStatus => string.Equals(ProjectStatus, "Tamamlandı", StringComparison.OrdinalIgnoreCase) || string.Equals(ProjectStatus, "Bitti", StringComparison.OrdinalIgnoreCase);

        public decimal FormAnalystPlannedTotal => PersonMonthlyCostRows?
            .Where(r => (r.User?.Title ?? "").Contains("Analist", StringComparison.OrdinalIgnoreCase))
            .Sum(r => r.TotalManDays) ?? 0m;

        public decimal FormDeveloperPlannedTotal => PersonMonthlyCostRows?
            .Where(r => !(r.User?.Title ?? "").Contains("Analist", StringComparison.OrdinalIgnoreCase))
            .Sum(r => r.TotalManDays) ?? 0m;

        public decimal FormAnalystActualTotal => PersonMonthlyCostRows?
            .Where(r => (r.User?.Title ?? "").Contains("Analist", StringComparison.OrdinalIgnoreCase))
            .Sum(r => r.ActualManDays) ?? 0m;

        public decimal FormDeveloperActualTotal => PersonMonthlyCostRows?
            .Where(r => !(r.User?.Title ?? "").Contains("Analist", StringComparison.OrdinalIgnoreCase))
            .Sum(r => r.ActualManDays) ?? 0m;

        public bool IsCostMismatched
        {
            get
            {
                decimal monthSum = Month1ManDaysTotal + Month2ManDaysTotal + Month3ManDaysTotal;
                decimal roleSum = AnalystPlannedManDays + DeveloperPlannedManDays;
                if (monthSum > 0 && roleSum > 0)
                {
                    return Math.Abs(monthSum - roleSum) > 0.01m;
                }
                return false;
            }
        }

        public string CostMismatchWarningMessage
        {
            get
            {
                if (!IsCostMismatched) return string.Empty;
                decimal monthSum = Month1ManDaysTotal + Month2ManDaysTotal + Month3ManDaysTotal;
                decimal roleSum = AnalystPlannedManDays + DeveloperPlannedManDays;
                decimal diff = Math.Abs(monthSum - roleSum);
                if (monthSum > roleSum)
                {
                    return $"⚠️ BÜTÇE VE MALİYET UYUMSUZLUĞU: Girilen ay maliyetleri toplamı ({monthSum:N0} Gün), rol maliyetleri toplamını ({roleSum:N0} Gün) {diff:N0} Gün AŞMAKTADIR!";
                }
                else
                {
                    return $"⚠️ BÜTÇE VE MALİYET UYUMSUZLUĞU: Girilen ay maliyetleri toplamı ({monthSum:N0} Gün), rol maliyetleri toplamından ({roleSum:N0} Gün) {diff:N0} Gün EKSİKTİR!";
                }
            }
        }

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

                decimal analystPlanned = FormAnalystPlannedTotal;
                decimal devPlanned = FormDeveloperPlannedTotal;
                decimal analystActual = FormAnalystActualTotal;
                decimal devActual = FormDeveloperActualTotal;

                decimal analystDiff = analystActual - analystPlanned;
                decimal devDiff = devActual - devPlanned;

                string analystPart = analystDiff > 0 
                    ? $"• Analistler: +{analystDiff:N0} Gün Aşım (Plan: {analystPlanned:N0}, Gerçekleşen: {analystActual:N0})"
                    : (analystDiff < 0 
                        ? $"• Analistler: -{Math.Abs(analystDiff):N0} Gün Tasarruf (Plan: {analystPlanned:N0}, Gerçekleşen: {analystActual:N0})"
                        : $"• Analistler: Tam Dengeli (Plan: {analystPlanned:N0}, Gerçekleşen: {analystActual:N0})");

                string devPart = devDiff > 0 
                    ? $"• Yazılımcılar: +{devDiff:N0} Gün Aşım (Plan: {devPlanned:N0}, Gerçekleşen: {devActual:N0})"
                    : (devDiff < 0 
                        ? $"• Yazılımcılar: -{Math.Abs(devDiff):N0} Gün Tasarruf (Plan: {devPlanned:N0}, Gerçekleşen: {devActual:N0})"
                        : $"• Yazılımcılar: Tam Dengeli (Plan: {devPlanned:N0}, Gerçekleşen: {devActual:N0})");

                string totalPart = string.Empty;
                if (budget > 0m)
                {
                    if (ActualManDays > budget)
                    {
                        decimal diff = ActualManDays - budget;
                        totalPart = $"• ⚠️ TOPLAM BÜTÇE AŞILDI (+{diff:N0} Gün Aşım | Bütçe: {budget:N0}, Gerçekleşen: {ActualManDays:N0})";
                    }
                    else if (ActualManDays < budget)
                    {
                        decimal diff = budget - ActualManDays;
                        totalPart = $"• ✅ TOPLAM BÜTÇE KORUNDU (-{diff:N0} Gün Tasarruf | Bütçe: {budget:N0}, Gerçekleşen: {ActualManDays:N0})";
                    }
                    else
                    {
                        totalPart = $"• ✅ TOPLAM BÜTÇE TAM UYGUN (Bütçe: {budget:N0}, Gerçekleşen: {ActualManDays:N0})";
                    }
                }
                else
                {
                    totalPart = $"• ℹ️ Toplam Gerçekleşen Efor: {ActualManDays:N0} Adam/Gün";
                }

                return $"📊 EFOR VE BÜTÇE GERÇEKLEŞME RAPORU:\n{analystPart}\n{devPart}\n{totalPart}";
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
            get
            {
                if (string.IsNullOrWhiteSpace(_displayAnalystSummary) || _displayAnalystSummary == "Seçilmedi" || _displayAnalystSummary == "-")
                {
                    if (SelectedProject != null)
                    {
                        var names = FormatNamesWithAbbreviatedSurname(SelectedProject.AssignedAnalystNames);
                        if (!string.IsNullOrWhiteSpace(names) && names != "-") return names;
                    }
                }
                return string.IsNullOrWhiteSpace(_displayAnalystSummary) ? "Seçilmedi" : _displayAnalystSummary;
            }
            set { _displayAnalystSummary = value; OnPropertyChanged(); }
        }

        private string _displayDeveloperSummary = "Seçilmedi";
        public string DisplayDeveloperSummary
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_displayDeveloperSummary) || _displayDeveloperSummary == "Seçilmedi" || _displayDeveloperSummary == "-")
                {
                    if (SelectedProject != null)
                    {
                        var names = FormatNamesWithAbbreviatedSurname(SelectedProject.AssignedDeveloperNames);
                        if (!string.IsNullOrWhiteSpace(names) && names != "-") return names;
                    }
                }
                return string.IsNullOrWhiteSpace(_displayDeveloperSummary) ? "Seçilmedi" : _displayDeveloperSummary;
            }
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

        private bool _isManuallyEditedBudget = false;

        private decimal _totalManDayBudget;
        public decimal TotalManDayBudget
        {
            get => _totalManDayBudget;
            set
            {
                _totalManDayBudget = value;
                if (!_isLoadingProject)
                {
                    _isManuallyEditedBudget = (value > 0m);
                }
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsCostMismatched));
                OnPropertyChanged(nameof(CostMismatchWarningMessage));
                OnPropertyChanged(nameof(IsBudgetExceeded));
                OnPropertyChanged(nameof(IsBudgetCritical));
                OnPropertyChanged(nameof(HasBudgetWarning));
                OnPropertyChanged(nameof(BudgetWarningMessage));
                OnPropertyChanged(nameof(TotalProjectCostFormatted));
                OnPropertyChanged(nameof(ActualBudgetFeedbackMessage));
                OnPropertyChanged(nameof(ActualBudgetFeedbackColor));
                OnPropertyChanged(nameof(ActualBudgetFeedbackBg));
                OnPropertyChanged(nameof(IsSideCardBudgetExceeded));
                OnPropertyChanged(nameof(SideCardBudgetFeedbackMessage));
                OnPropertyChanged(nameof(SideCardVarianceText));
                OnPropertyChanged(nameof(SideCardVarianceColor));
            }
        }

        public decimal TotalInternalManDays
        {
            get
            {
                decimal monthSum = Month1ManDaysTotal + Month2ManDaysTotal + Month3ManDaysTotal;
                if (monthSum > 0m) return monthSum;

                decimal roleSum = AnalystPlannedManDays + DeveloperPlannedManDays;
                if (roleSum > 0m) return roleSum;

                return PersonMonthlyCostRows != null && PersonMonthlyCostRows.Any() ? PersonMonthlyCostRows.Sum(r => r.TotalManDays) : 0m;
            }
        }
        
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

        public ICommand AddProjectTypeCommand { get; }
        public ICommand AddGmyCommand { get; }
        public ICommand AddBusinessUnitCommand { get; }
        public ICommand AddStakeholderCommand { get; }

        public ICommand RemoveProjectTypeCommand { get; }
        public ICommand RemoveGmyCommand { get; }
        public ICommand RemoveBusinessUnitCommand { get; }

        public static string ShowInputDialog(string title, string promptText, string defaultValue = "")
        {
            string input = string.Empty;
            var win = new Window
            {
                Title = title,
                Width = 420,
                Height = 175,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ResizeMode = ResizeMode.NoResize,
                WindowStyle = WindowStyle.ToolWindow,
                Background = System.Windows.Media.Brushes.White
            };

            var grid = new Grid { Margin = new Thickness(15) };
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            var lbl = new TextBlock
            {
                Text = promptText,
                FontWeight = FontWeights.Bold,
                FontSize = 11.5,
                Foreground = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFrom("#1e293b")!,
                Margin = new Thickness(0, 0, 0, 8)
            };
            Grid.SetRow(lbl, 0);

            var txt = new TextBox
            {
                Text = defaultValue,
                Height = 28,
                FontSize = 11,
                Padding = new Thickness(4, 2, 4, 2),
                Margin = new Thickness(0, 0, 0, 12)
            };
            Grid.SetRow(txt, 1);

            var btnPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right
            };

            var btnOk = new Button
            {
                Content = "➕ Ekle ve Seç",
                Width = 135,
                Height = 28,
                IsDefault = true,
                FontWeight = FontWeights.Bold,
                Background = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFrom("#16a34a")!,
                Foreground = System.Windows.Media.Brushes.White,
                Margin = new Thickness(0, 0, 8, 0)
            };

            var btnCancel = new Button
            {
                Content = "İptal",
                Width = 65,
                Height = 28,
                IsCancel = true
            };

            btnOk.Click += (s, e) => { win.DialogResult = true; win.Close(); };
            btnCancel.Click += (s, e) => { win.DialogResult = false; win.Close(); };

            btnPanel.Children.Add(btnOk);
            btnPanel.Children.Add(btnCancel);
            Grid.SetRow(btnPanel, 2);

            grid.Children.Add(lbl);
            grid.Children.Add(txt);
            grid.Children.Add(btnPanel);

            win.Content = grid;
            if (win.ShowDialog() == true)
            {
                input = txt.Text.Trim();
            }
            return input;
        }

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

            AddProjectTypeCommand = new RelayCommand(_ =>
            {
                string val = ShowInputDialog("Yeni Proje Türü Ekleyin", "Eklenecek yeni proje türünün adını giriniz:");
                if (!string.IsNullOrWhiteSpace(val))
                {
                    if (!TypeList.Contains(val)) TypeList.Add(val);
                    ProjectType = val;
                }
            });

            AddGmyCommand = new RelayCommand(_ =>
            {
                string val = ShowInputDialog("Yeni GMY Birimi Ekleyin", "Eklenecek yeni GMY biriminin adını giriniz:");
                if (!string.IsNullOrWhiteSpace(val))
                {
                    if (!GmyList.Contains(val)) GmyList.Add(val);
                    Gmy = val;
                }
            });

            AddBusinessUnitCommand = new RelayCommand(_ =>
            {
                string val = ShowInputDialog("Yeni İş Birimi Ekleyin", "Eklenecek yeni iş biriminin adını giriniz:");
                if (!string.IsNullOrWhiteSpace(val))
                {
                    if (!BusinessUnitList.Contains(val)) BusinessUnitList.Add(val);
                    BusinessUnit = val;
                }
            });

            RemoveProjectTypeCommand = new RelayCommand(_ =>
            {
                if (string.IsNullOrWhiteSpace(ProjectType)) return;
                var confirm = MessageBox.Show($"Seçili '{ProjectType}' proje türünü seçenekler listesinden silmek istediğinize emin misiniz?", "Seçenek Silme Onayı", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (confirm == MessageBoxResult.Yes)
                {
                    string toRemove = ProjectType;
                    TypeList.Remove(toRemove);
                    ProjectType = TypeList.FirstOrDefault() ?? "Proje";
                }
            });

            RemoveGmyCommand = new RelayCommand(_ =>
            {
                if (string.IsNullOrWhiteSpace(Gmy)) return;
                var confirm = MessageBox.Show($"Seçili '{Gmy}' GMY birimini seçenekler listesinden silmek istediğinize emin misiniz?", "Seçenek Silme Onayı", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (confirm == MessageBoxResult.Yes)
                {
                    string toRemove = Gmy;
                    GmyList.Remove(toRemove);
                    Gmy = string.Empty;
                }
            });

            RemoveBusinessUnitCommand = new RelayCommand(_ =>
            {
                if (string.IsNullOrWhiteSpace(BusinessUnit)) return;
                var confirm = MessageBox.Show($"Seçili '{BusinessUnit}' iş birimini seçenekler listesinden silmek istediğinize emin misiniz?", "Seçenek Silme Onayı", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (confirm == MessageBoxResult.Yes)
                {
                    string toRemove = BusinessUnit;
                    BusinessUnitList.Remove(toRemove);
                    BusinessUnit = string.Empty;
                }
            });

            AddStakeholderCommand = new RelayCommand(_ =>
            {
                string val = ShowInputDialog("Yeni Paydaş Departman Ekleyin", "Eklenecek yeni paydaş departmanın adını giriniz:");
                if (!string.IsNullOrWhiteSpace(val))
                {
                    if (SelectedProjectDisplay != null)
                    {
                        var existing = SelectedProjectDisplay.StakeholderItems.FirstOrDefault(s => string.Equals(s.Name, val, StringComparison.OrdinalIgnoreCase));
                        if (existing != null)
                        {
                            existing.IsSelected = true;
                        }
                        else
                        {
                            var newItem = new SelectableStakeholderItem
                            {
                                Name = val,
                                IsSelected = true,
                                OnSelectionChangedAction = () => SelectedProjectDisplay.UpdateStakeholdersFromItems()
                            };
                            SelectedProjectDisplay.StakeholderItems.Add(newItem);
                            SelectedProjectDisplay.UpdateStakeholdersFromItems();
                        }
                    }

                    var formExisting = StakeholderItems.FirstOrDefault(s => string.Equals(s.Name, val, StringComparison.OrdinalIgnoreCase));
                    if (formExisting != null)
                    {
                        formExisting.IsSelected = true;
                    }
                    else
                    {
                        StakeholderItems.Add(new SelectableStakeholderItem
                        {
                            Name = val,
                            IsSelected = true,
                            OnSelectionChangedAction = RefreshAssigneesAndCostRows
                        });
                    }
                }
            });

            SaveProjectCommand = new RelayCommand(ExecuteSaveProject);
            DeleteProjectCommand = new RelayCommand(ExecuteDeleteProject);
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
                LoadSelectedProject();
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
                    item.IsEditing = !item.IsEditing;
                    if (item.IsEditing)
                    {
                        IsDetailPanelVisible = false;
                    }
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
                if (_allUsers == null || !_allUsers.Any())
                {
                    _allUsers = _services.GetAllUsers() ?? new List<User>();
                }

                var teamUsers = (string.IsNullOrWhiteSpace(SelectedTeam) || SelectedTeam == "Tüm Ekipler")
                    ? _allUsers
                    : _allUsers.Where(u => string.Equals(u.Team, SelectedTeam, StringComparison.OrdinalIgnoreCase)).ToList();

                if (!teamUsers.Any()) teamUsers = _allUsers;

                var analysts = teamUsers.Where(u => (u.Title ?? "").Contains("Analist", StringComparison.OrdinalIgnoreCase)).ToList();
                if (!analysts.Any())
                {
                    analysts = teamUsers.Take(Math.Max(1, teamUsers.Count / 2)).ToList();
                }

                var developers = teamUsers.Except(analysts).ToList();

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
            else if (SelectedProject != null && !string.IsNullOrWhiteSpace(SelectedProject.AssignedAnalystNames) && SelectedProject.AssignedAnalystNames != "Seçilmedi")
                DisplayAnalystSummary = SelectedProject.AssignedAnalystNames;
            else
                DisplayAnalystSummary = "Seçilmedi";

            // Developer Summary ("Herkes" or names)
            if (DeveloperUserItems.Any() && selectedDevelopers.Count == DeveloperUserItems.Count)
                DisplayDeveloperSummary = "Herkes";
            else if (selectedDevelopers.Any())
                DisplayDeveloperSummary = string.Join(", ", selectedDevelopers.Select(i => i.User.FullName));
            else if (SelectedProject != null && !string.IsNullOrWhiteSpace(SelectedProject.AssignedDeveloperNames) && SelectedProject.AssignedDeveloperNames != "Seçilmedi")
                DisplayDeveloperSummary = SelectedProject.AssignedDeveloperNames;
            else
                DisplayDeveloperSummary = "Seçilmedi";

            var allSelectedUsers = selectedAnalysts.Concat(selectedDevelopers).Select(i => i.User).ToList();
            var existingRows = PersonMonthlyCostRows.ToList();
            var newRows = new ObservableCollection<ProjectPersonMonthlyCostRow>();
            var maxDays = GetQuarterMonthMaxDays();

            foreach (var user in allSelectedUsers)
            {
                var existingRow = existingRows.FirstOrDefault(r => r.User.Id == user.Id);
                if (existingRow != null)
                {
                    existingRow.MaxMonth1Days = maxDays.max1;
                    existingRow.MaxMonth2Days = maxDays.max2;
                    existingRow.MaxMonth3Days = maxDays.max3;
                    existingRow.Month1Header = Month1Header;
                    existingRow.Month2Header = Month2Header;
                    existingRow.Month3Header = Month3Header;
                    newRows.Add(existingRow);
                }
                else
                {
                    // Default man-days to 0 so user enters manually
                    newRows.Add(new ProjectPersonMonthlyCostRow
                    {
                        User = user,
                        MaxMonth1Days = maxDays.max1,
                        MaxMonth2Days = maxDays.max2,
                        MaxMonth3Days = maxDays.max3,
                        Month1Header = Month1Header,
                        Month2Header = Month2Header,
                        Month3Header = Month3Header,
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

        public decimal TotalActualManDays => SelectedProjectDisplay?.ActualManDays ?? ActualManDays;

        public bool IsSideCardBudgetExceeded => TotalManDayBudget > 0m && Math.Round(TotalActualManDays, 1) > Math.Round(TotalManDayBudget, 1);

        public decimal SideCardBudgetExcessDays => IsSideCardBudgetExceeded ? (Math.Round(TotalActualManDays, 1) - Math.Round(TotalManDayBudget, 1)) : 0m;

        public string SideCardBudgetFeedbackMessage
        {
            get
            {
                if (IsSideCardBudgetExceeded)
                {
                    return $"⚠️ BÜTÇE AŞILDI! (Bütçe: {TotalManDayBudget:N0} Gün, Harcanan: {TotalActualManDays:N0} Gün, Aşım: +{SideCardBudgetExcessDays:N0} Gün)";
                }
                if (IsCompletedStatus && TotalManDayBudget > 0m && TotalActualManDays > 0m && TotalActualManDays <= TotalManDayBudget)
                {
                    decimal savings = TotalManDayBudget - TotalActualManDays;
                    if (savings > 0m) return $"✅ BÜTÇE UYGUN (Bütçe: {TotalManDayBudget:N0} Gün, Harcanan: {TotalActualManDays:N0} Gün, Tasarruf: -{savings:N0} Gün)";
                    return $"✅ BÜTÇE TAM UYGUN ({TotalManDayBudget:N0} Gün)";
                }
                return string.Empty;
            }
        }

        public string SideCardRoleBalanceFeedbackMessage
        {
            get
            {
                if (SelectedProjectDisplay == null) return string.Empty;

                decimal analystPlanned = SelectedProjectDisplay.AnalystPlannedManDays;
                decimal devPlanned = SelectedProjectDisplay.DeveloperPlannedManDays;
                decimal analystActual = SelectedProjectDisplay.AnalystActualManDays;
                decimal devActual = SelectedProjectDisplay.DeveloperActualManDays;

                if (analystActual == 0m && devActual == 0m) return string.Empty;

                decimal analystDiff = analystActual - analystPlanned;
                decimal devDiff = devActual - devPlanned;

                decimal totalPlanned = analystPlanned + devPlanned;
                decimal totalActual = analystActual + devActual;

                if (totalPlanned > 0m && Math.Abs(totalActual - totalPlanned) <= 0.01m && (Math.Abs(analystDiff) > 0.01m || Math.Abs(devDiff) > 0.01m))
                {
                    string analystStr = analystDiff > 0 ? $"Analist eforu +{analystDiff:N0} gün fazla" : (analystDiff < 0 ? $"Analist eforu -{Math.Abs(analystDiff):N0} gün eksik" : "Analist eforu dengeli");
                    string devStr = devDiff > 0 ? $"Yazılımcı eforu +{devDiff:N0} gün fazla" : (devDiff < 0 ? $"Yazılımcı eforu -{Math.Abs(devDiff):N0} gün eksik" : "Yazılımcı eforu dengeli");

                    return $"ℹ️ ROL DAĞILIM DENGESİ: {analystStr}, {devStr} gerçekleşmiştir. Toplam bütçe ({totalPlanned:N0} Adam/Gün) tam korunmuştur.";
                }

                return string.Empty;
            }
        }

        public bool HasRoleBalanceFeedback => !string.IsNullOrEmpty(SideCardRoleBalanceFeedbackMessage);

        public string SideCardVarianceText
        {
            get
            {
                if (TotalManDayBudget <= 0m || TotalActualManDays <= 0m) return "-";
                decimal diff = TotalActualManDays - TotalManDayBudget;
                if (diff > 0m) return $"⚠️ +{diff:N0} Gün (Aşım)";
                if (diff < 0m) return $"✅ -{Math.Abs(diff):N0} Gün (Tasarruf)";
                if (HasRoleBalanceFeedback) return $"✅ Toplam Bütçe Korundu ({TotalManDayBudget:N0} Gün)";
                return "✅ 0 Gün (Tam Sınır)";
            }
        }

        public string SideCardVarianceColor
        {
            get
            {
                if (TotalManDayBudget <= 0m || TotalActualManDays <= 0m) return "#475569";
                decimal diff = TotalActualManDays - TotalManDayBudget;
                if (diff > 0m) return "#dc2626";
                if (diff < 0m) return "#166534";
                return "#166534";
            }
        }

        private bool _isLoadingProject = false;

        private void RecalculateTotals()
        {
            if (PersonMonthlyCostRows != null && PersonMonthlyCostRows.Any())
            {
                decimal totalPersonActual = PersonMonthlyCostRows.Sum(r => r.ActualManDays);
                _actualManDays = totalPersonActual;
                OnPropertyChanged(nameof(ActualManDays));
                OnPropertyChanged(nameof(TotalActualManDays));
                OnPropertyChanged(nameof(IsSideCardBudgetExceeded));
                OnPropertyChanged(nameof(SideCardBudgetFeedbackMessage));
                OnPropertyChanged(nameof(SideCardVarianceText));
                OnPropertyChanged(nameof(SideCardVarianceColor));
                OnPropertyChanged(nameof(ActualBudgetFeedbackMessage));
                OnPropertyChanged(nameof(ActualBudgetFeedbackColor));
                OnPropertyChanged(nameof(ActualBudgetFeedbackBg));
            }

            OnPropertyChanged(nameof(TotalInternalManDays));

            if (SelectedProjectDisplay != null && PersonMonthlyCostRows != null)
            {
                var breakdown = new List<UserCostBreakdownRow>();
                var analystNameSet = (DisplayAnalystSummary ?? "").Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToHashSet();
                var devNameSet = (DisplayDeveloperSummary ?? "").Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToHashSet();
                bool analystIsHerkes = string.Equals(DisplayAnalystSummary, "Herkes", StringComparison.OrdinalIgnoreCase);
                bool devIsHerkes = string.Equals(DisplayDeveloperSummary, "Herkes", StringComparison.OrdinalIgnoreCase);

                foreach (var row in PersonMonthlyCostRows)
                {
                    if (row.User == null) continue;
                    bool isAnalyst = analystIsHerkes || IsNameMatched(row.User.FullName, analystNameSet);
                    bool isDev = devIsHerkes || IsNameMatched(row.User.FullName, devNameSet);
                    string role;
                    if (isAnalyst && !isDev) role = "Analist";
                    else if (isDev && !isAnalyst) role = "Yazılımcı";
                    else role = (row.User.Title ?? "").Contains("Analist", StringComparison.OrdinalIgnoreCase) ? "Analist" : "Yazılımcı";

                    breakdown.Add(new UserCostBreakdownRow
                    {
                        UserId = row.User.Id,
                        FullName = row.User.FullName,
                        Role = role,
                        Month1ManDays = row.Month1ManDays,
                        Month2ManDays = row.Month2ManDays,
                        Month3ManDays = row.Month3ManDays,
                        ActualManDaysShare = row.ActualManDays
                    });
                }
                SelectedProjectDisplay.UserBreakdownList = breakdown;
                SelectedProjectDisplay.NotifyAllPropertiesChanged();
            }

            if (!_isLoadingProject && !_isManuallyEditedBudget)
            {
                _totalManDayBudget = TotalInternalManDays;
                OnPropertyChanged(nameof(TotalManDayBudget));
            }

            OnPropertyChanged(nameof(IsCostMismatched));
            OnPropertyChanged(nameof(CostMismatchWarningMessage));
            OnPropertyChanged(nameof(FormAnalystPlannedTotal));
            OnPropertyChanged(nameof(FormDeveloperPlannedTotal));
            OnPropertyChanged(nameof(FormAnalystActualTotal));
            OnPropertyChanged(nameof(FormDeveloperActualTotal));
            OnPropertyChanged(nameof(TotalProjectCostFormatted));
            OnPropertyChanged(nameof(TotalActualManDays));
            OnPropertyChanged(nameof(IsSideCardBudgetExceeded));
            OnPropertyChanged(nameof(SideCardBudgetFeedbackMessage));
            OnPropertyChanged(nameof(SideCardVarianceText));
            OnPropertyChanged(nameof(SideCardVarianceColor));
            OnPropertyChanged(nameof(IsBudgetExceeded));
            OnPropertyChanged(nameof(IsBudgetCritical));
            OnPropertyChanged(nameof(HasBudgetWarning));
            OnPropertyChanged(nameof(BudgetWarningMessage));
        }

        private decimal _month1ManDaysTotal;
        public decimal Month1ManDaysTotal
        {
            get => _month1ManDaysTotal;
            set
            {
                _month1ManDaysTotal = value;
                OnPropertyChanged();
                if (IsEditMode) TotalManDayBudget = Month1ManDaysTotal + Month2ManDaysTotal + Month3ManDaysTotal > 0 ? Month1ManDaysTotal + Month2ManDaysTotal + Month3ManDaysTotal : AnalystPlannedManDays + DeveloperPlannedManDays;
                OnPropertyChanged(nameof(TotalInternalManDays));
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
                if (IsEditMode) TotalManDayBudget = Month1ManDaysTotal + Month2ManDaysTotal + Month3ManDaysTotal > 0 ? Month1ManDaysTotal + Month2ManDaysTotal + Month3ManDaysTotal : AnalystPlannedManDays + DeveloperPlannedManDays;
                OnPropertyChanged(nameof(TotalInternalManDays));
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
                if (IsEditMode) TotalManDayBudget = Month1ManDaysTotal + Month2ManDaysTotal + Month3ManDaysTotal > 0 ? Month1ManDaysTotal + Month2ManDaysTotal + Month3ManDaysTotal : AnalystPlannedManDays + DeveloperPlannedManDays;
                OnPropertyChanged(nameof(TotalInternalManDays));
            }
        }
        private decimal _analystPlannedManDays;
        public decimal AnalystPlannedManDays
        {
            get => _analystPlannedManDays;
            set
            {
                _analystPlannedManDays = value;
                OnPropertyChanged();
                if (IsEditMode) TotalManDayBudget = Month1ManDaysTotal + Month2ManDaysTotal + Month3ManDaysTotal > 0 ? Month1ManDaysTotal + Month2ManDaysTotal + Month3ManDaysTotal : AnalystPlannedManDays + DeveloperPlannedManDays;
                OnPropertyChanged(nameof(TotalInternalManDays));
            }
        }
        private decimal _developerPlannedManDays;
        public decimal DeveloperPlannedManDays
        {
            get => _developerPlannedManDays;
            set
            {
                _developerPlannedManDays = value;
                OnPropertyChanged();
                if (IsEditMode) TotalManDayBudget = Month1ManDaysTotal + Month2ManDaysTotal + Month3ManDaysTotal > 0 ? Month1ManDaysTotal + Month2ManDaysTotal + Month3ManDaysTotal : AnalystPlannedManDays + DeveloperPlannedManDays;
                OnPropertyChanged(nameof(TotalInternalManDays));
            }
        }

        private decimal _analystActualManDays;
        public decimal AnalystActualManDays
        {
            get => _analystActualManDays;
            set
            {
                _analystActualManDays = value;
                OnPropertyChanged();
            }
        }

        private decimal _developerActualManDays;
        public decimal DeveloperActualManDays
        {
            get => _developerActualManDays;
            set
            {
                _developerActualManDays = value;
                OnPropertyChanged();
            }
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

                var projectIds = list.Select(p => p.Id).ToList();
                var allProjectCosts = _services.GetMonthlyCostsForProjects(projectIds);
                var allProjectAllocations = _services.GetAllocationsForProjects(projectIds);
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

                    var dbCosts = allProjectCosts.Where(c => c.ProjectId == p.Id).ToList();
                    var dbAllocations = allProjectAllocations.Where(a => a.ProjectId == p.Id).ToList();

                    var analystsList = _allUsers.Where(u => (u.Title ?? "").Contains("Analist", StringComparison.OrdinalIgnoreCase)).ToList();
                    var devList = _allUsers.Where(u => (u.Title ?? "").Contains("Developer", StringComparison.OrdinalIgnoreCase) || (u.Title ?? "").Contains("Yazılımcı", StringComparison.OrdinalIgnoreCase) || (u.Title ?? "").Contains("Mühendis", StringComparison.OrdinalIgnoreCase)).ToList();

                    var rawAnalysts = p.AssignedAnalystNames ?? string.Empty;
                    var analystNamesSet = rawAnalysts.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToHashSet();
                    bool isAnalystHerkes = string.Equals(rawAnalysts, "Herkes", StringComparison.OrdinalIgnoreCase);

                    var rawDevs = p.AssignedDeveloperNames ?? string.Empty;
                    var devNamesSet = rawDevs.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToHashSet();
                    bool isDevHerkes = string.Equals(rawDevs, "Herkes", StringComparison.OrdinalIgnoreCase);

                    var assignedAnalystUsers = analystsList.Where(u => isAnalystHerkes || IsNameMatched(u.FullName, analystNamesSet)).ToList();
                    var assignedDevUsers = devList.Where(u => isDevHerkes || IsNameMatched(u.FullName, devNamesSet)).ToList();
                    var allAssignedUsers = assignedAnalystUsers.Concat(assignedDevUsers).DistinctBy(u => u.Id).ToList();

                    // Fallback 1: If monthly costs are empty but TotalManDayBudget > 0, generate monthly breakdown
                    if (!dbCosts.Any(c => c.ManDays > 0m) && p.TotalManDayBudget > 0m)
                    {
                        decimal b = p.TotalManDayBudget;
                        int numA = assignedAnalystUsers.Count > 0 ? assignedAnalystUsers.Count : 1;
                        int numD = assignedDevUsers.Count > 0 ? assignedDevUsers.Count : 1;
                        decimal aBudget = Math.Round(b * 0.30m, 1);
                        decimal dBudget = b - aBudget;

                        foreach (var au in assignedAnalystUsers)
                        {
                            decimal share = Math.Round(aBudget / numA, 1);
                            dbCosts.Add(new ProjectMonthlyCost { ProjectId = p.Id, UserId = au.Id, Month = (byte)m1, ManDays = Math.Round(share * 0.35m, 1) });
                            dbCosts.Add(new ProjectMonthlyCost { ProjectId = p.Id, UserId = au.Id, Month = (byte)m2, ManDays = Math.Round(share * 0.45m, 1) });
                            dbCosts.Add(new ProjectMonthlyCost { ProjectId = p.Id, UserId = au.Id, Month = (byte)m3, ManDays = Math.Round(share * 0.20m, 1) });
                        }
                        foreach (var du in assignedDevUsers)
                        {
                            decimal share = Math.Round(dBudget / numD, 1);
                            dbCosts.Add(new ProjectMonthlyCost { ProjectId = p.Id, UserId = du.Id, Month = (byte)m1, ManDays = Math.Round(share * 0.35m, 1) });
                            dbCosts.Add(new ProjectMonthlyCost { ProjectId = p.Id, UserId = du.Id, Month = (byte)m2, ManDays = Math.Round(share * 0.45m, 1) });
                            dbCosts.Add(new ProjectMonthlyCost { ProjectId = p.Id, UserId = du.Id, Month = (byte)m3, ManDays = Math.Round(share * 0.20m, 1) });
                        }
                    }

                    // Reset actual effort if project status is not completed
                    bool isCompletedProject = string.Equals(p.ProjectStatus, "Tamamlandı", StringComparison.OrdinalIgnoreCase);
                    if (!isCompletedProject)
                    {
                        p.ActualManDays = 0m;
                        p.ActualReleaseDate = null;
                        foreach (var a in dbAllocations)
                        {
                            a.ActualManDay = 0m;
                        }
                    }

                    decimal m1Sum = p.AnalistAy1 + p.YazilimciAy1;
                    decimal m2Sum = p.AnalistAy2 + p.YazilimciAy2;
                    decimal m3Sum = p.AnalistAy3 + p.YazilimciAy3;

                    // Build user breakdown list for row details template & detail card
                    var userGroupIds = dbCosts.Select(c => c.UserId)
                        .Union(dbAllocations.Select(a => a.UserId))
                        .Union(allAssignedUsers.Select(u => u.Id))
                        .Where(id => id > 0)
                        .Distinct()
                        .ToList();

                    var breakdown = new List<UserCostBreakdownRow>();
                    decimal totalPlannedInternal = m1Sum + m2Sum + m3Sum;

                    bool hasSpecificUserDbCosts = dbCosts.Any(c => allAssignedUsers.Any(au => au.Id == c.UserId) && c.ManDays > 0);

                    foreach (var uid in userGroupIds)
                    {
                        var u = _allUsers.FirstOrDefault(usr => usr.Id == uid);
                        string name = u?.FullName ?? $"Personel #{uid}";
                        
                        bool isDevAssigned = assignedDevUsers.Any(du => du.Id == uid);
                        bool isAnalystAssigned = assignedAnalystUsers.Any(au => au.Id == uid);

                        string role = "Yazılımcı";
                        if (isAnalystAssigned && !isDevAssigned)
                        {
                            role = "Analist";
                        }
                        else if (!isAnalystAssigned && isDevAssigned)
                        {
                            role = "Yazılımcı";
                        }
                        else
                        {
                            bool isAnalystTitle = (u?.Title ?? "").Contains("Analist", StringComparison.OrdinalIgnoreCase);
                            role = isAnalystTitle ? "Analist" : "Yazılımcı";
                        }

                        decimal uM1 = dbCosts.FirstOrDefault(c => c.UserId == uid && c.Month == m1)?.ManDays ?? 0m;
                        decimal uM2 = dbCosts.FirstOrDefault(c => c.UserId == uid && c.Month == m2)?.ManDays ?? 0m;
                        decimal uM3 = dbCosts.FirstOrDefault(c => c.UserId == uid && c.Month == m3)?.ManDays ?? 0m;

                        decimal uTotalPlanned = uM1 + uM2 + uM3;

                        var alloc = dbAllocations.FirstOrDefault(a => a.UserId == uid);
                        decimal uActual = alloc != null && alloc.ActualManDay > 0m ? alloc.ActualManDay : 0m;

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

                    var item = new ProjectDisplayItem
                    {
                        Project = p,
                        Month1ManDaysTotal = m1Sum,
                        Month2ManDaysTotal = m2Sum,
                        Month3ManDaysTotal = m3Sum,
                        UserBreakdownList = breakdown
                    };
                    
                    item.AnalystPlannedManDays = p.AnalistAy1 + p.AnalistAy2 + p.AnalistAy3;
                    item.DeveloperPlannedManDays = p.YazilimciAy1 + p.YazilimciAy2 + p.YazilimciAy3;

                    item.AnalystUserItems = new ObservableCollection<SelectableUserItem>(
                        analystsList.Select(u => new SelectableUserItem
                        {
                            User = u,
                            IsSelected = isAnalystHerkes || IsNameMatched(u.FullName, analystNamesSet),
                            OnSelectionChangedAction = () => item.UpdateAssignedAnalystNamesFromItems()
                        })
                    );

                    item.DeveloperUserItems = new ObservableCollection<SelectableUserItem>(
                        devList.Select(u => new SelectableUserItem
                        {
                            User = u,
                            IsSelected = isDevHerkes || IsNameMatched(u.FullName, devNamesSet),
                            OnSelectionChangedAction = () => item.UpdateAssignedDeveloperNamesFromItems()
                        })
                    );

                    var defaultDeptNames = new[] { 
                        "Kredi Risk", "Bireysel Bankacılık", "BT Altyapı", "Raporlama & Veri", 
                        "Muhasebe & Finans", "Uyum & Mevzuat", "Dijital Bankacılık", "Kurumsal Bankacılık", 
                        "Hazine, Finansal Kurumlar", "IT Güvenlik, Operasyon" 
                    };
                    var currentStakeholders = (p.Stakeholders ?? "").Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToHashSet();
                    var allDeptNames = defaultDeptNames.Union(currentStakeholders).Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();

                    item.StakeholderItems = new ObservableCollection<SelectableStakeholderItem>(
                        allDeptNames.Select(d => new SelectableStakeholderItem
                        {
                            Name = d,
                            IsSelected = currentStakeholders.Contains(d),
                            OnSelectionChangedAction = () => item.UpdateStakeholdersFromItems()
                        })
                    );

                    displayItems.Add(item);
                }

                _allProjectsList = displayItems.ToList();
                FilterProjectsList();
                StatusMessage = string.Empty;

                // Proje listesi boşsa detay panelini kapat ve formu sıfırla
                if (!_allProjectsList.Any())
                {
                    IsDetailPanelVisible = false;
                    IsEditMode = false;
                    IsNewProjectMode = false;
                    _selectedProject = null;
                    _selectedProjectDisplay = null;
                    OnPropertyChanged(nameof(SelectedProject));
                    OnPropertyChanged(nameof(SelectedProjectDisplay));
                }
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
            _isLoadingProject = true;

            try
            {
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
                _isManuallyEditedBudget = SelectedProject.TotalManDayBudget > 0m;
                Month1ManDaysTotal = SelectedProject.AnalistAy1 + SelectedProject.YazilimciAy1;
                Month2ManDaysTotal = SelectedProject.AnalistAy2 + SelectedProject.YazilimciAy2;
                Month3ManDaysTotal = SelectedProject.AnalistAy3 + SelectedProject.YazilimciAy3;
                AnalystPlannedManDays = SelectedProject.AnalistAy1 + SelectedProject.AnalistAy2 + SelectedProject.AnalistAy3;
                DeveloperPlannedManDays = SelectedProject.YazilimciAy1 + SelectedProject.YazilimciAy2 + SelectedProject.YazilimciAy3;

                ActualManDays = SelectedProject.ActualManDays;
                PlannedReleaseDate = SelectedProject.PlannedReleaseDate;
                ActualReleaseDate = string.Equals(ProjectStatus, "Tamamlandı", StringComparison.OrdinalIgnoreCase) ? SelectedProject.ActualReleaseDate : null;

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
                    item.IsSelected = isAnalystHerkes || IsNameMatched(item.User.FullName, analystNames);
                }

                var devNames = (SelectedProject.AssignedDeveloperNames ?? "").Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToHashSet();
                bool isDevHerkes = string.Equals(SelectedProject.AssignedDeveloperNames, "Herkes", StringComparison.OrdinalIgnoreCase);

                foreach (var item in DeveloperUserItems)
                {
                    item.IsSelected = isDevHerkes || IsNameMatched(item.User.FullName, devNames);
                }

                RefreshAssigneesAndCostRows();

                // Load Monthly Costs and Allocations
                var dbCosts = _services.GetProjectMonthlyCosts(SelectedProject.Id) ?? new List<ProjectMonthlyCost>();
                var dbAllocations = _services.GetAllocationsByProject(SelectedProject.Id) ?? new List<ProjectAllocation>();

                // "Herkes" seçilince sadece unvanı Analist olan kullanıcılar analist sayılır
                var analystUserIds = isAnalystHerkes
                    ? _allUsers.Where(u => (u.Title ?? "").Contains("Analist", StringComparison.OrdinalIgnoreCase)).Select(u => u.Id).ToHashSet()
                    : _allUsers.Where(u => IsNameMatched(u.FullName, analystNames)).Select(u => u.Id).ToHashSet();

                var devUserIds = isDevHerkes
                    ? _allUsers.Where(u => (u.Title ?? "").Contains("Developer", StringComparison.OrdinalIgnoreCase) || (u.Title ?? "").Contains("Yaz\u0131l\u0131mc\u0131", StringComparison.OrdinalIgnoreCase) || (u.Title ?? "").Contains("M\u00fchendis", StringComparison.OrdinalIgnoreCase)).Select(u => u.Id).ToHashSet()
                    : _allUsers.Where(u => IsNameMatched(u.FullName, devNames)).Select(u => u.Id).ToHashSet();

                AnalystActualManDays = dbAllocations.Where(a => analystUserIds.Contains(a.UserId)).Sum(a => a.ActualManDay);
                DeveloperActualManDays = dbAllocations.Where(a => devUserIds.Contains(a.UserId)).Sum(a => a.ActualManDay);

                var assignedUserIds = _allUsers.Where(u =>
                    (isAnalystHerkes && (u.Title ?? "").Contains("Analist", StringComparison.OrdinalIgnoreCase)) ||
                    (!isAnalystHerkes && IsNameMatched(u.FullName, analystNames)) ||
                    (isDevHerkes && ((u.Title ?? "").Contains("Developer", StringComparison.OrdinalIgnoreCase) || (u.Title ?? "").Contains("Yazılımcı", StringComparison.OrdinalIgnoreCase) || (u.Title ?? "").Contains("Mühendis", StringComparison.OrdinalIgnoreCase))) ||
                    (!isDevHerkes && IsNameMatched(u.FullName, devNames))
                ).Select(u => u.Id).ToHashSet();

                var selectedUserIds = AnalystUserItems.Concat(DeveloperUserItems)
                    .Where(i => i.IsSelected)
                    .Select(i => i.User.Id)
                    .Union(dbCosts.Select(c => c.UserId))
                    .Union(dbAllocations.Select(a => a.UserId))
                    .Union(assignedUserIds)
                    .Where(id => id > 0)
                    .ToHashSet();

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

                var maxDays = GetQuarterMonthMaxDays();

                // Compute total fallback budget/actual if dbCosts or dbAllocations are empty for this project
                decimal fallbackBudget = TotalManDayBudget > 0m ? TotalManDayBudget : (SelectedProject?.TotalManDayBudget ?? 0m);
                decimal fallbackActual = ActualManDays > 0m ? ActualManDays : (SelectedProject?.ActualManDays > 0m ? SelectedProject.ActualManDays : (string.Equals(ProjectStatus, "Tamamlandı", StringComparison.OrdinalIgnoreCase) ? fallbackBudget : 0m));

                int totalStaffCount = selectedUserIds.Count > 0 ? selectedUserIds.Count : 1;

                foreach (var uId in selectedUserIds)
                {
                    var userObj = _allUsers.FirstOrDefault(u => u.Id == uId);
                    if (userObj == null) continue;

                    var userDbCosts = dbCosts.Where(c => c.UserId == uId).ToList();
                    decimal md1 = userDbCosts.FirstOrDefault(c => c.Month == m1)?.ManDays ?? 0m;
                    decimal md2 = userDbCosts.FirstOrDefault(c => c.Month == m2)?.ManDays ?? 0m;
                    decimal md3 = userDbCosts.FirstOrDefault(c => c.Month == m3)?.ManDays ?? 0m;

                    // Fallback for monthly costs if 0
                    if (md1 == 0m && md2 == 0m && md3 == 0m && fallbackBudget > 0m)
                    {
                        decimal userBudgetShare = Math.Round(fallbackBudget / totalStaffCount, 1);
                        md1 = Math.Round(userBudgetShare * 0.35m, 1);
                        md2 = Math.Round(userBudgetShare * 0.45m, 1);
                        md3 = Math.Round(userBudgetShare * 0.20m, 1);
                    }

                    var alloc = dbAllocations.FirstOrDefault(a => a.UserId == uId);
                    decimal actualMd = alloc?.ActualManDay ?? 0m;

                    rows.Add(new ProjectPersonMonthlyCostRow
                    {
                        User = userObj,
                        MaxMonth1Days = maxDays.max1,
                        MaxMonth2Days = maxDays.max2,
                        MaxMonth3Days = maxDays.max3,
                        Month1Header = Month1Header,
                        Month2Header = Month2Header,
                        Month3Header = Month3Header,
                        Month1ManDays = md1,
                        Month2ManDays = md2,
                        Month3ManDays = md3,
                        ActualManDays = actualMd,
                        OnChangedAction = RecalculateTotals
                    });
                }

                PersonMonthlyCostRows = rows;
                RefreshAssigneesAndCostRows();

                // Update UserBreakdownList on SelectedProjectDisplay so Detail Card DataGrid & Cost Summary update live
                var userBreakdown = rows.Select(r =>
                {
                    bool isAnalyst = analystNames.Any(n => IsNameMatched(r.User.FullName, new[] { n }));
                    return new UserCostBreakdownRow
                    {
                        UserId = r.User.Id,
                        FullName = r.User.FullName,
                        Role = isAnalyst ? "Analist" : "Yazılımcı",
                        Month1ManDays = r.Month1ManDays,
                        Month2ManDays = r.Month2ManDays,
                        Month3ManDays = r.Month3ManDays,
                        ActualManDaysShare = r.ActualManDays,
                        IsCompletedProject = string.Equals(ProjectStatus, "Tamamlandı", StringComparison.OrdinalIgnoreCase)
                    };
                }).ToList();

                if (SelectedProjectDisplay != null)
                {
                    SelectedProjectDisplay.UserBreakdownList = userBreakdown;
                    SelectedProjectDisplay.NotifyAllPropertiesChanged();
                }
            }
            finally
            {
                _isLoadingProject = false;
            }
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
                p.PlannedReleaseDate = PlannedReleaseDate;
                p.ActualReleaseDate = string.Equals(ProjectStatus, "Tamamlandı", StringComparison.OrdinalIgnoreCase) ? ActualReleaseDate : null;

                // Sync project budget with form budget
                decimal monthSum = Month1ManDaysTotal + Month2ManDaysTotal + Month3ManDaysTotal;
                decimal roleSum = AnalystPlannedManDays + DeveloperPlannedManDays;

                decimal totalCost = monthSum > 0 ? monthSum : roleSum;
                if (totalCost > 0m)
                {
                    TotalManDayBudget = totalCost;
                }
                p.TotalManDayBudget = TotalManDayBudget;

                decimal m1Total = Month1ManDaysTotal;
                decimal m2Total = Month2ManDaysTotal;
                decimal m3Total = Month3ManDaysTotal;

                if (monthSum == 0 && roleSum > 0)
                {
                    p.AnalistAy1 = AnalystPlannedManDays;
                    p.AnalistAy2 = 0m; p.AnalistAy3 = 0m;
                    p.YazilimciAy1 = DeveloperPlannedManDays;
                    p.YazilimciAy2 = 0m; p.YazilimciAy3 = 0m;
                }
                else
                {
                    decimal remAnalyst = AnalystPlannedManDays;

                    p.AnalistAy1 = Math.Min(remAnalyst, m1Total);
                    p.YazilimciAy1 = m1Total - p.AnalistAy1;
                    remAnalyst -= p.AnalistAy1;

                    p.AnalistAy2 = Math.Min(remAnalyst, m2Total);
                    p.YazilimciAy2 = m2Total - p.AnalistAy2;
                    remAnalyst -= p.AnalistAy2;

                    p.AnalistAy3 = Math.Min(remAnalyst, m3Total);
                    p.YazilimciAy3 = m3Total - p.AnalistAy3;
                }

                int startMonth = (SelectedQuarter - 1) * 3 + 1;
                var monthlyCosts = new List<ProjectMonthlyCost>();
                var allocations = new List<ProjectAllocation>();

                bool isCompletedForm = string.Equals(ProjectStatus, "Tamamlandı", StringComparison.OrdinalIgnoreCase);

                p.AnalystActualManDays = SelectedProjectDisplay?.AnalystActualManDays ?? p.AnalystActualManDays;
                p.DeveloperActualManDays = SelectedProjectDisplay?.DeveloperActualManDays ?? p.DeveloperActualManDays;
                p.ActualManDays = p.AnalystActualManDays + p.DeveloperActualManDays;
                p.ActualReleaseDate = isCompletedForm ? ActualReleaseDate : null;

                var analystsList = _allUsers.Where(u => (u.Title ?? "").Contains("Analist", StringComparison.OrdinalIgnoreCase)).ToList();
                var devList = _allUsers.Where(u => (u.Title ?? "").Contains("Developer", StringComparison.OrdinalIgnoreCase) || (u.Title ?? "").Contains("Yazılımcı", StringComparison.OrdinalIgnoreCase) || (u.Title ?? "").Contains("Mühendis", StringComparison.OrdinalIgnoreCase)).ToList();

                var rawAnalysts = p.AssignedAnalystNames ?? string.Empty;
                var analystNamesSet = rawAnalysts.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToHashSet();
                bool isAnalystHerkes = string.Equals(rawAnalysts, "Herkes", StringComparison.OrdinalIgnoreCase);

                var rawDevs = p.AssignedDeveloperNames ?? string.Empty;
                var devNamesSet = rawDevs.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToHashSet();
                bool isDevHerkes = string.Equals(rawDevs, "Herkes", StringComparison.OrdinalIgnoreCase);

                var analystUsers = analystsList.Where(u => isAnalystHerkes || IsNameMatched(u.FullName, analystNamesSet)).ToList();
                var devUsers = devList.Where(u => isDevHerkes || IsNameMatched(u.FullName, devNamesSet)).ToList();

                if (!analystUsers.Any())
                {
                    var defaultAnalyst = analystsList.FirstOrDefault() ?? _allUsers.FirstOrDefault();
                    if (defaultAnalyst != null) analystUsers.Add(defaultAnalyst);
                }
                if (!devUsers.Any())
                {
                    var defaultDev = devList.FirstOrDefault() ?? _allUsers.LastOrDefault();
                    if (defaultDev != null) devUsers.Add(defaultDev);
                }

                int numAnalysts = analystUsers.Count;
                int numDevs = devUsers.Count;

                decimal sumAnalystM1 = 0m, sumAnalystM2 = 0m, sumAnalystM3 = 0m, sumAnalystAlloc = 0m;
                for (int i = 0; i < numAnalysts; i++)
                {
                    var u = analystUsers[i];
                    decimal m1 = (i == numAnalysts - 1) ? (p.AnalistAy1 - sumAnalystM1) : Math.Round(p.AnalistAy1 / numAnalysts, 2);
                    decimal m2 = (i == numAnalysts - 1) ? (p.AnalistAy2 - sumAnalystM2) : Math.Round(p.AnalistAy2 / numAnalysts, 2);
                    decimal m3 = (i == numAnalysts - 1) ? (p.AnalistAy3 - sumAnalystM3) : Math.Round(p.AnalistAy3 / numAnalysts, 2);
                    decimal uTotal = (i == numAnalysts - 1) ? (AnalystPlannedManDays - sumAnalystAlloc) : Math.Round(AnalystPlannedManDays / numAnalysts, 2);
                    
                    sumAnalystM1 += m1; sumAnalystM2 += m2; sumAnalystM3 += m3; sumAnalystAlloc += uTotal;

                    monthlyCosts.Add(new ProjectMonthlyCost { UserId = u.Id, Month = startMonth, ManDays = m1 });
                    monthlyCosts.Add(new ProjectMonthlyCost { UserId = u.Id, Month = startMonth + 1, ManDays = m2 });
                    monthlyCosts.Add(new ProjectMonthlyCost { UserId = u.Id, Month = startMonth + 2, ManDays = m3 });
                    allocations.Add(new ProjectAllocation { UserId = u.Id, AllocatedManDay = uTotal, ActualManDay = 0m }); // Allocation üzerinde ActualManDay tutmuyoruz artık
                }

                decimal sumDevM1 = 0m, sumDevM2 = 0m, sumDevM3 = 0m, sumDevAlloc = 0m;
                for (int i = 0; i < numDevs; i++)
                {
                    var u = devUsers[i];
                    decimal m1 = (i == numDevs - 1) ? (p.YazilimciAy1 - sumDevM1) : Math.Round(p.YazilimciAy1 / numDevs, 2);
                    decimal m2 = (i == numDevs - 1) ? (p.YazilimciAy2 - sumDevM2) : Math.Round(p.YazilimciAy2 / numDevs, 2);
                    decimal m3 = (i == numDevs - 1) ? (p.YazilimciAy3 - sumDevM3) : Math.Round(p.YazilimciAy3 / numDevs, 2);
                    decimal uTotal = (i == numDevs - 1) ? (DeveloperPlannedManDays - sumDevAlloc) : Math.Round(DeveloperPlannedManDays / numDevs, 2);
                    
                    sumDevM1 += m1; sumDevM2 += m2; sumDevM3 += m3; sumDevAlloc += uTotal;

                    monthlyCosts.Add(new ProjectMonthlyCost { UserId = u.Id, Month = startMonth, ManDays = m1 });
                    monthlyCosts.Add(new ProjectMonthlyCost { UserId = u.Id, Month = startMonth + 1, ManDays = m2 });
                    monthlyCosts.Add(new ProjectMonthlyCost { UserId = u.Id, Month = startMonth + 2, ManDays = m3 });
                    allocations.Add(new ProjectAllocation { UserId = u.Id, AllocatedManDay = uTotal, ActualManDay = 0m }); // Allocation üzerinde ActualManDay tutmuyoruz artık
                }

                _services.SaveProjectWithMonthlyCosts(p, monthlyCosts, allocations, LoggedUserName);

                StatusMessage = $"✅ '{p.ProjectName}' projesinin bütçe ve efor bilgileri başarıyla kaydedildi.";
                IsEditMode = false;
                IsFormOpen = false;

                long savedProjectId = p.Id;
                LoadProjects();

                // Kaydet dedikten sonra detay panelini kapatıp satır görünümüne dönüyoruz
                IsDetailPanelVisible = false;
                IsEditMode = false;
                IsNewProjectMode = false;
                IsFormOpen = false;
                _selectedProjectDisplay = null;
                OnPropertyChanged(nameof(SelectedProjectDisplay));
                OnPropertyChanged(nameof(UserBreakdownList));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kaydetme Hatası: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public List<string> AllAnalystNames => _allUsers
            .Where(u => (u.Title ?? "").Contains("Analist", StringComparison.OrdinalIgnoreCase))
            .Select(u => u.FullName)
            .Distinct()
            .ToList();

        public List<string> AllDeveloperNames => _allUsers
            .Where(u => (u.Title ?? "").Contains("Developer", StringComparison.OrdinalIgnoreCase) || (u.Title ?? "").Contains("Yazılımcı", StringComparison.OrdinalIgnoreCase) || (u.Title ?? "").Contains("Mühendis", StringComparison.OrdinalIgnoreCase))
            .Select(u => u.FullName)
            .Distinct()
            .ToList();

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

                // Sync status and actual effort onto project object
                p.ProjectStatus = item.ProjectStatus;
                p.PlannedReleaseDate = item.PlannedReleaseDate;
                p.ActualReleaseDate = item.ActualReleaseDate;

                decimal monthSum = item.Month1ManDaysTotal + item.Month2ManDaysTotal + item.Month3ManDaysTotal;
                decimal roleSum = item.AnalystPlannedManDays + item.DeveloperPlannedManDays;

                decimal totalCost = monthSum > 0 ? monthSum : roleSum;
                if (totalCost > 0m)
                {
                    p.TotalManDayBudget = totalCost;
                }

                if (monthSum == 0 && roleSum > 0)
                {
                    p.AnalistAy1 = item.AnalystPlannedManDays;
                    p.AnalistAy2 = 0m; p.AnalistAy3 = 0m;
                    p.YazilimciAy1 = item.DeveloperPlannedManDays;
                    p.YazilimciAy2 = 0m; p.YazilimciAy3 = 0m;
                }
                else
                {
                    decimal remAnalyst = item.AnalystPlannedManDays;

                    p.AnalistAy1 = Math.Min(remAnalyst, item.Month1ManDaysTotal);
                    p.YazilimciAy1 = item.Month1ManDaysTotal - p.AnalistAy1;
                    remAnalyst -= p.AnalistAy1;

                    p.AnalistAy2 = Math.Min(remAnalyst, item.Month2ManDaysTotal);
                    p.YazilimciAy2 = item.Month2ManDaysTotal - p.AnalistAy2;
                    remAnalyst -= p.AnalistAy2;

                    p.AnalistAy3 = Math.Min(remAnalyst, item.Month3ManDaysTotal);
                    p.YazilimciAy3 = item.Month3ManDaysTotal - p.AnalistAy3;
                }
                
                p.AnalystPlannedManDays = item.AnalystPlannedManDays;
                p.DeveloperPlannedManDays = item.DeveloperPlannedManDays;

                // Gerçekleşen eforları doğrudan kaydet — ne girildiyse o
                bool isCompletedForm = string.Equals(p.ProjectStatus, "Tamamlandı", StringComparison.OrdinalIgnoreCase);

                p.AnalystActualManDays = item.AnalystActualManDays;
                p.DeveloperActualManDays = item.DeveloperActualManDays;
                p.ActualManDays = p.AnalystActualManDays + p.DeveloperActualManDays;

                // Mevcut monthly costs ve allocations'ı koru
                var existingMonthlyCosts = _services.GetProjectMonthlyCosts(p.Id) ?? new List<ProjectMonthlyCost>();
                var existingAllocations = _services.GetAllocationsByProject(p.Id) ?? new List<ProjectAllocation>();

                _services.SaveProjectWithMonthlyCosts(p, existingMonthlyCosts, existingAllocations, LoggedUserName);
                StatusMessage = $"✅ '{p.ProjectName}' projesinin bütçesi ve durumu GÜNCELLENDİ.";
                item.IsEditing = false;
                item.NotifyAllPropertiesChanged();

                long savedProjectId = p.Id;
                LoadProjects();

                // Kaydet butonuna basılınca bilgi kartı açılmasın
                IsDetailPanelVisible = false;
                _selectedProjectDisplay = null;
                OnPropertyChanged(nameof(SelectedProjectDisplay));
                OnPropertyChanged(nameof(UserBreakdownList));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kaydetme Hatası: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteDeleteProject(object? param)
        {
            Project? projToDelete = null;
            if (param is ProjectDisplayItem pdi)
            {
                projToDelete = pdi.Project;
            }
            else if (param is Project p)
            {
                projToDelete = p;
            }
            else
            {
                projToDelete = SelectedProject ?? SelectedProjectDisplay?.Project;
            }

            if (projToDelete == null) return;

            var confirm = MessageBox.Show($"'{projToDelete.ProjectName}' projesini silmek istediğinize emin misiniz?", "Proje Silme Onayı", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (confirm == MessageBoxResult.Yes)
            {
                try
                {
                    _services.DeleteProject(projToDelete.Id);
                    StatusMessage = $"🗑️ '{projToDelete.ProjectName}' projesi başarıyla silindi.";

                    IsDetailPanelVisible = false;
                    IsFormOpen = false;
                    IsEditMode = false;
                    IsNewProjectMode = false;
                    _selectedProject = null;
                    _selectedProjectDisplay = null;
                    OnPropertyChanged(nameof(SelectedProject));
                    OnPropertyChanged(nameof(SelectedProjectDisplay));
                    OnPropertyChanged(nameof(UserBreakdownList));

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
            Month1ManDaysTotal = 0m;
            Month2ManDaysTotal = 0m;
            Month3ManDaysTotal = 0m;
            AnalystPlannedManDays = 0m;
            DeveloperPlannedManDays = 0m;

            foreach (var item in AnalystUserItems) item.IsSelected = false;
            foreach (var item in DeveloperUserItems) item.IsSelected = false;
            foreach (var item in StakeholderItems) item.IsSelected = false;

            _isManuallyEditedBudget = false;
            PersonMonthlyCostRows.Clear();
            DisplayAnalystSummary = "Seçilmedi";
            DisplayDeveloperSummary = "Seçilmedi";
            RecalculateTotals();
        }
    }
}
