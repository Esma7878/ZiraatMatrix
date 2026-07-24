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

        public decimal TotalManDays => Month1ManDays + Month2ManDays + Month3ManDays;
    }

    public class UserCostBreakdownRow
    {
        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string DisplayNameWithRole => string.IsNullOrWhiteSpace(Role) ? FullName : $"{FullName} ({Role})";
        public decimal Month1ManDays { get; set; }
        public decimal Month2ManDays { get; set; }
        public decimal Month3ManDays { get; set; }
        public decimal TotalManDays => Month1ManDays + Month2ManDays + Month3ManDays;
    }

    public class ProjectDisplayItem : BaseViewModel
    {
        public Project Project { get; set; } = null!;

        private decimal _month1ManDaysTotal;
        public decimal Month1ManDaysTotal
        {
            get => _month1ManDaysTotal;
            set { _month1ManDaysTotal = value; OnPropertyChanged(); }
        }

        private decimal _month2ManDaysTotal;
        public decimal Month2ManDaysTotal
        {
            get => _month2ManDaysTotal;
            set { _month2ManDaysTotal = value; OnPropertyChanged(); }
        }

        private decimal _month3ManDaysTotal;
        public decimal Month3ManDaysTotal
        {
            get => _month3ManDaysTotal;
            set { _month3ManDaysTotal = value; OnPropertyChanged(); }
        }

        public decimal TotalInternalManDays => Month1ManDaysTotal + Month2ManDaysTotal + Month3ManDaysTotal;

        public List<UserCostBreakdownRow> UserBreakdownList { get; set; } = new List<UserCostBreakdownRow>();
    }

    public class ProjectsViewModel : BaseViewModel
    {
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

        public bool CanCreateOrEditProjects =>
            IsUserAdmin ||
            (!string.IsNullOrWhiteSpace(LoggedUserTitle) &&
             (LoggedUserTitle.Contains("Analist", StringComparison.OrdinalIgnoreCase) ||
              LoggedUserTitle.Contains("Yönetici", StringComparison.OrdinalIgnoreCase)));

        public Visibility CreateProjectButtonVisibility => CanCreateOrEditProjects ? Visibility.Visible : Visibility.Collapsed;

        // Form Visibility Controls (Modal / Collapsible Panel)
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

        private string _selectedTeam = "Takip";
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
            set { _projectStatus = value; OnPropertyChanged(); }
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

        public decimal TotalInternalManDays => PersonMonthlyCostRows.Sum(r => r.TotalManDays);
        public string TotalProjectCostFormatted => ExternalCost > 0 
            ? $"{TotalInternalManDays:N0} Adam/Gün (İç) | {ExternalCost:N0} (Dış)" 
            : $"{TotalInternalManDays:N0} Adam/Gün";

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

        // Commands
        public ICommand SaveProjectCommand { get; }
        public ICommand DeleteProjectCommand { get; }
        public ICommand ClearFormCommand { get; }
        public ICommand SelectQuarterCommand { get; }
        public ICommand OpenCreateFormCommand { get; }
        public ICommand OpenEditFormCommand { get; }
        public ICommand CloseFormCommand { get; }
        public ICommand ClearSearchCommand { get; }

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
                var userObj = _services.GetAllUsers().FirstOrDefault(u => u.FullName.Equals(loggedUserName, StringComparison.OrdinalIgnoreCase));
                if (userObj != null)
                {
                    LoggedUserTitle = userObj.Title ?? string.Empty;
                    if (userObj.IsAdmin) IsUserAdmin = true;
                    if (string.IsNullOrWhiteSpace(LoggedTeam)) LoggedTeam = userObj.Team ?? string.Empty;
                }
            }

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
                IsFormOpen = true;
            });

            OpenEditFormCommand = new RelayCommand(param =>
            {
                if (param is ProjectDisplayItem item)
                {
                    SelectedProjectDisplay = item;
                    IsFormOpen = true;
                }
                else if (param is Project proj)
                {
                    SelectedProject = proj;
                    IsFormOpen = true;
                }
            });

            CloseFormCommand = new RelayCommand(_ => IsFormOpen = false);

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
                var teamUsers = _allUsers;
                string targetTeam = !string.IsNullOrWhiteSpace(SelectedTeam) ? SelectedTeam : LoggedTeam;
                if (!string.IsNullOrWhiteSpace(targetTeam))
                {
                    var filtered = _allUsers.Where(u => string.Equals(u.Team, targetTeam, StringComparison.OrdinalIgnoreCase)).ToList();
                    if (filtered.Any()) teamUsers = filtered;
                }

                var analysts = teamUsers.Where(u => (u.Title ?? "").Contains("Analist", StringComparison.OrdinalIgnoreCase) || (u.Title ?? "").Contains("Yönetici", StringComparison.OrdinalIgnoreCase)).ToList();
                var developers = teamUsers.Where(u => (u.Title ?? "").Contains("Developer", StringComparison.OrdinalIgnoreCase) || (u.Title ?? "").Contains("Yazılımcı", StringComparison.OrdinalIgnoreCase) || (u.Title ?? "").Contains("Mühendis", StringComparison.OrdinalIgnoreCase)).ToList();

                if (!analysts.Any()) analysts = teamUsers.Take(2).ToList();
                if (!developers.Any()) developers = teamUsers.Skip(2).ToList();

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
            OnPropertyChanged(nameof(TotalInternalManDays));
            OnPropertyChanged(nameof(TotalProjectCostFormatted));
        }

        public void LoadProjects()
        {
            try
            {
                string filterTeam = (SelectedTeamFilter == "Tüm Ekipler" || string.IsNullOrWhiteSpace(SelectedTeamFilter))
                    ? (IsUserAdmin ? "" : LoggedTeam)
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

                    decimal m1Sum = dbCosts.Where(c => c.Month == m1).Sum(c => c.ManDays);
                    decimal m2Sum = dbCosts.Where(c => c.Month == m2).Sum(c => c.ManDays);
                    decimal m3Sum = dbCosts.Where(c => c.Month == m3).Sum(c => c.ManDays);

                    // Build user breakdown list for row details template
                    var userGroupIds = dbCosts.Select(c => c.UserId).Distinct().ToList();
                    var breakdown = new List<UserCostBreakdownRow>();

                    foreach (var uid in userGroupIds)
                    {
                        var u = _allUsers.FirstOrDefault(usr => usr.Id == uid);
                        string name = u?.FullName ?? $"Personel #{uid}";
                        string role = (u?.Title ?? "").Contains("Analist", StringComparison.OrdinalIgnoreCase) || (u?.Title ?? "").Contains("Yönetici", StringComparison.OrdinalIgnoreCase) ? "Analist" : "Yazılımcı";

                        decimal uM1 = dbCosts.FirstOrDefault(c => c.UserId == uid && c.Month == m1)?.ManDays ?? 0m;
                        decimal uM2 = dbCosts.FirstOrDefault(c => c.UserId == uid && c.Month == m2)?.ManDays ?? 0m;
                        decimal uM3 = dbCosts.FirstOrDefault(c => c.UserId == uid && c.Month == m3)?.ManDays ?? 0m;

                        breakdown.Add(new UserCostBreakdownRow
                        {
                            FullName = name,
                            Role = role,
                            Month1ManDays = uM1,
                            Month2ManDays = uM2,
                            Month3ManDays = uM3
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
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                ProjectsList = new ObservableCollection<ProjectDisplayItem>(_allProjectsList);
            }
            else
            {
                string query = SearchText.Trim().ToLowerInvariant();
                var filtered = _allProjectsList.Where(item =>
                    (item.Project.PergelNo > 0 && item.Project.PergelNo.ToString().Contains(query)) ||
                    (item.Project.ProjectName ?? "").ToLowerInvariant().Contains(query) ||
                    (item.Project.Summary ?? "").ToLowerInvariant().Contains(query) ||
                    (item.Project.ProjectType ?? "").ToLowerInvariant().Contains(query) ||
                    (item.Project.ExternalCompanyName ?? "").ToLowerInvariant().Contains(query) ||
                    (item.Project.AssignedAnalystNames ?? "").ToLowerInvariant().Contains(query) ||
                    (item.Project.AssignedDeveloperNames ?? "").ToLowerInvariant().Contains(query) ||
                    (item.Project.Gmy ?? "").ToLowerInvariant().Contains(query) ||
                    (item.Project.BusinessUnit ?? "").ToLowerInvariant().Contains(query)
                ).ToList();

                ProjectsList = new ObservableCollection<ProjectDisplayItem>(filtered);
            }
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

            // Load Monthly Costs
            var dbCosts = _services.GetProjectMonthlyCosts(SelectedProject.Id) ?? new List<ProjectMonthlyCost>();
            var selectedUserIds = AnalystUserItems.Concat(DeveloperUserItems).Where(i => i.IsSelected).Select(i => i.User.Id).ToHashSet();
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

                rows.Add(new ProjectPersonMonthlyCostRow
                {
                    User = userObj,
                    Month1ManDays = md1,
                    Month2ManDays = md2,
                    Month3ManDays = md3,
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
                p.TotalManDayBudget = TotalInternalManDays;

                int startMonth = (SelectedQuarter - 1) * 3 + 1;
                var monthlyCosts = new List<ProjectMonthlyCost>();

                foreach (var row in PersonMonthlyCostRows)
                {
                    monthlyCosts.Add(new ProjectMonthlyCost { UserId = row.User.Id, Month = startMonth, ManDays = row.Month1ManDays });
                    monthlyCosts.Add(new ProjectMonthlyCost { UserId = row.User.Id, Month = startMonth + 1, ManDays = row.Month2ManDays });
                    monthlyCosts.Add(new ProjectMonthlyCost { UserId = row.User.Id, Month = startMonth + 2, ManDays = row.Month3ManDays });
                }

                _services.SaveProjectWithMonthlyCosts(p, monthlyCosts);

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
