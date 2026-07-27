using System.Windows.Input;
using ZiraatProje.Business;
using ZiraatProje.DataAccess;

namespace ZiraatProje.UI.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private readonly BusinessServices _services = new BusinessServices();
        private BaseViewModel _currentViewModel;
        public BaseViewModel CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsDashboardActive));
                OnPropertyChanged(nameof(IsUsersActive));
                OnPropertyChanged(nameof(IsShiftsActive));
                OnPropertyChanged(nameof(IsLeavesActive));
                OnPropertyChanged(nameof(IsProjectsActive));
            }
        }

        public bool IsDashboardActive => CurrentViewModel is DashboardViewModel;
        public bool IsUsersActive => CurrentViewModel is UsersViewModel;
        public bool IsShiftsActive => CurrentViewModel is ShiftsViewModel;
        public bool IsLeavesActive => CurrentViewModel is LeavesViewModel;
        public bool IsProjectsActive => CurrentViewModel is ProjectsViewModel;

        private bool _isLoggedIn;
        public bool IsLoggedIn
        {
            get => _isLoggedIn;
            set { _isLoggedIn = value; OnPropertyChanged(); }
        }

        private bool _isCurrentUserAdmin;
        public bool IsCurrentUserAdmin
        {
            get => _isCurrentUserAdmin;
            set { _isCurrentUserAdmin = value; OnPropertyChanged(); }
        }

        private string _selectedLoginTeam = string.Empty;
        public string SelectedLoginTeam
        {
            get => _selectedLoginTeam;
            set 
            { 
                _selectedLoginTeam = value; 
                OnPropertyChanged(); 
                OnPropertyChanged(nameof(CurrentUserTitle));
            }
        }

        // Credentials input
        private string _loginEmailPrefix = string.Empty;
        public string LoginEmailPrefix
        {
            get => _loginEmailPrefix;
            set { _loginEmailPrefix = value; OnPropertyChanged(); }
        }

        public string FullLoginEmail
        {
            get
            {
                if (string.IsNullOrWhiteSpace(LoginEmailPrefix)) return string.Empty;
                var clean = LoginEmailPrefix.Trim();
                if (clean.EndsWith("@ziraatteknoloji.com", StringComparison.OrdinalIgnoreCase))
                    return clean;
                if (clean.Contains("@"))
                    return clean;
                return $"{clean}@ziraatteknoloji.com";
            }
        }

        private string _loginPassword = string.Empty;
        public string LoginPassword
        {
            get => _loginPassword;
            set { _loginPassword = value; OnPropertyChanged(); }
        }

        private string _errorMessage = string.Empty;
        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; OnPropertyChanged(); }
        }

        private string _currentUserName = string.Empty;
        public string CurrentUserName
        {
            get => _currentUserName;
            set { _currentUserName = value; OnPropertyChanged(); }
        }

        public string CurrentUserTitle
        {
            get
            {
                if (string.IsNullOrWhiteSpace(SelectedLoginTeam) ||
                    SelectedLoginTeam.Equals("Departman Yönetimi", StringComparison.OrdinalIgnoreCase) ||
                    SelectedLoginTeam.Equals("Yönetim", StringComparison.OrdinalIgnoreCase) ||
                    SelectedLoginTeam.Equals("Tüm Ekipler", StringComparison.OrdinalIgnoreCase))
                {
                    return "Katılım Finansman Tahsis ve Takip - Departman Yöneticisi";
                }
                if (IsCurrentUserAdmin)
                {
                    return $"Katılım Finansman Tahsis ve Takip - {SelectedLoginTeam} Ekip Yöneticisi";
                }
                return $"Katılım Finansman Tahsis ve Takip - {SelectedLoginTeam} Ekip Üyesi";
            }
        }

        // Password Reset Properties
        private bool _isResetPasswordMode;
        public bool IsResetPasswordMode
        {
            get => _isResetPasswordMode;
            set { _isResetPasswordMode = value; OnPropertyChanged(); }
        }

        private string _resetEmailPrefix = string.Empty;
        public string ResetEmailPrefix
        {
            get => _resetEmailPrefix;
            set { _resetEmailPrefix = value; OnPropertyChanged(); }
        }

        public string FullResetEmail
        {
            get
            {
                if (string.IsNullOrWhiteSpace(ResetEmailPrefix)) return string.Empty;
                var clean = ResetEmailPrefix.Trim();
                if (clean.EndsWith("@ziraatteknoloji.com", StringComparison.OrdinalIgnoreCase))
                    return clean;
                if (clean.Contains("@"))
                    return clean;
                return $"{clean}@ziraatteknoloji.com";
            }
        }

        private string _resetPhone = string.Empty;
        public string ResetPhone
        {
            get => _resetPhone;
            set { _resetPhone = value; OnPropertyChanged(); }
        }

        private string _newPassword = string.Empty;
        public string NewPassword
        {
            get => _newPassword;
            set { _newPassword = value; OnPropertyChanged(); }
        }

        private string _newPasswordConfirm = string.Empty;
        public string NewPasswordConfirm
        {
            get => _newPasswordConfirm;
            set { _newPasswordConfirm = value; OnPropertyChanged(); }
        }

        private string _resetStatusMessage = string.Empty;
        public string ResetStatusMessage
        {
            get => _resetStatusMessage;
            set { _resetStatusMessage = value; OnPropertyChanged(); }
        }

        private bool _showResetPasswordText;
        public bool ShowResetPasswordText
        {
            get => _showResetPasswordText;
            set
            {
                _showResetPasswordText = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(EyeIcon));
            }
        }

        public string EyeIcon => ShowResetPasswordText ? "🙈" : "👁️";

        private bool _showLoginPasswordText;
        public bool ShowLoginPasswordText
        {
            get => _showLoginPasswordText;
            set
            {
                _showLoginPasswordText = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(LoginEyeIcon));
            }
        }

        public string LoginEyeIcon => ShowLoginPasswordText ? "🙈" : "👁️";

        private int _currentUserId;
        public int CurrentUserId
        {
            get => _currentUserId;
            set { _currentUserId = value; OnPropertyChanged(); }
        }

        private int _unreadChatCount;
        public int UnreadChatCount
        {
            get => _unreadChatCount;
            set 
            { 
                _unreadChatCount = value; 
                OnPropertyChanged(); 
                OnPropertyChanged(nameof(HasUnreadChat)); 
                OnPropertyChanged(nameof(ChatUnreadBadgeVisibility));
                OnPropertyChanged(nameof(ChatButtonText));
            }
        }

        public bool HasUnreadChat => UnreadChatCount > 0;
        public System.Windows.Visibility ChatUnreadBadgeVisibility => HasUnreadChat ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
        public string ChatButtonText => HasUnreadChat ? $"Chat ({UnreadChatCount})" : "Chat";

        public bool IsChatActive => CurrentViewModel is ChatViewModel;

        private readonly System.Windows.Threading.DispatcherTimer _chatNotificationTimer;

        // Commands
        public ICommand ShowDashboardCommand { get; }
        public ICommand ShowUsersCommand { get; }
        public ICommand ShowShiftsCommand { get; }
        public ICommand ShowLeavesCommand { get; }
        public ICommand ShowProjectsCommand { get; }
        public ICommand ShowChatCommand { get; }
        public ICommand LoginCommand { get; }
        public ICommand LogoutCommand { get; }
        public ICommand ShowResetPasswordCommand { get; }
        public ICommand CancelResetPasswordCommand { get; }
        public ICommand SubmitResetPasswordCommand { get; }
        public ICommand ToggleShowResetPasswordCommand { get; }
        public ICommand ToggleShowLoginPasswordCommand { get; }

        public MainWindowViewModel()
        {
            // Initial view state
            _currentViewModel = new DashboardViewModel();

            ToggleShowResetPasswordCommand = new RelayCommand(_ => ShowResetPasswordText = !ShowResetPasswordText);
            ToggleShowLoginPasswordCommand = new RelayCommand(_ => ShowLoginPasswordText = !ShowLoginPasswordText);

            LoginCommand = new RelayCommand(_ => {
                ErrorMessage = string.Empty;
                var emailToUse = FullLoginEmail;
                if (string.IsNullOrWhiteSpace(emailToUse) || string.IsNullOrWhiteSpace(LoginPassword))
                {
                    ErrorMessage = "Lütfen e-posta ve şifre giriniz.";
                    return;
                }

                var user = _services.Authenticate(emailToUse, LoginPassword);
                if (user != null)
                {
                    CurrentUserId = user.Id;
                    CurrentUserName = user.FullName;
                    SelectedLoginTeam = user.Team;
                    IsCurrentUserAdmin = user.IsAdmin;
                    IsLoggedIn = true;
                    ShowLoginPasswordText = false;
                    LoginEmailPrefix = string.Empty;
                    LoginPassword = string.Empty;

                    // Load initial dashboard for this team with full quick action delegates
                    var dashVm = CreateDashboardViewModel(IsCurrentUserAdmin, CurrentUserName);
                    CurrentViewModel = dashVm;
                }
                else
                {
                    ErrorMessage = "E-posta veya şifre yanlış, yeniden deneyiniz.";
                    LoginPassword = string.Empty;
                    ShowLoginPasswordText = false;
                }
            });

            ShowResetPasswordCommand = new RelayCommand(_ => {
                IsResetPasswordMode = true;
                ShowResetPasswordText = false;
                ResetEmailPrefix = LoginEmailPrefix;
                ResetPhone = string.Empty;
                NewPassword = string.Empty;
                NewPasswordConfirm = string.Empty;
                ResetStatusMessage = string.Empty;
            });

            CancelResetPasswordCommand = new RelayCommand(_ => {
                IsResetPasswordMode = false;
                ResetStatusMessage = string.Empty;
            });

            SubmitResetPasswordCommand = new RelayCommand(_ => {
                try
                {
                    ResetStatusMessage = string.Empty;
                    var emailToUse = FullResetEmail;
                    if (string.IsNullOrWhiteSpace(emailToUse))
                    {
                        ResetStatusMessage = "Lütfen e-posta adresinizi giriniz.";
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(NewPassword))
                    {
                        ResetStatusMessage = "Lütfen yeni şifrenizi giriniz.";
                        return;
                    }
                    if (NewPassword != NewPasswordConfirm)
                    {
                        ResetStatusMessage = "Yeni şifreler eşleşmiyor!";
                        return;
                    }

                    _services.ResetPassword(emailToUse, ResetPhone, NewPassword);
                    ResetStatusMessage = "✅ Şifreniz güncellendi! Giriş yapabilirsiniz.";
                }
                catch (System.Exception ex)
                {
                    ResetStatusMessage = $"⚠️ {ex.Message}";
                }
            });

            LogoutCommand = new RelayCommand(_ => {
                IsLoggedIn = false;
                IsCurrentUserAdmin = false;
                CurrentUserId = 0;
                CurrentUserName = string.Empty;
                SelectedLoginTeam = string.Empty;
                IsResetPasswordMode = false;
                LoginEmailPrefix = string.Empty;
                LoginPassword = string.Empty;
                ShowLoginPasswordText = false;
                ErrorMessage = string.Empty;
                ResetStatusMessage = string.Empty;
                CurrentViewModel = CreateDashboardViewModel(false, string.Empty);
            });

            ShowDashboardCommand = new RelayCommand(_ => {
                var dashVm = CreateDashboardViewModel(IsCurrentUserAdmin, CurrentUserName);
                CurrentViewModel = dashVm;
            });
            ShowUsersCommand = new RelayCommand(_ => NavigateToUsers());
            ShowShiftsCommand = new RelayCommand(_ => NavigateToShifts());
            ShowLeavesCommand = new RelayCommand(_ => NavigateToLeaves());
            ShowProjectsCommand = new RelayCommand(_ => NavigateToProjects());
            ShowChatCommand = new RelayCommand(_ => NavigateToChat());

            // Setup real-time chat notification badge timer (every 2 seconds)
            _chatNotificationTimer = new System.Windows.Threading.DispatcherTimer { Interval = System.TimeSpan.FromSeconds(2) };
            _chatNotificationTimer.Tick += (s, e) => PollChatUnreadCount();
            _chatNotificationTimer.Start();
        }

        private void PollChatUnreadCount()
        {
            if (!IsLoggedIn || CurrentUserId <= 0)
            {
                UnreadChatCount = 0;
                return;
            }

            try
            {
                int previousCount = UnreadChatCount;
                UnreadChatCount = _services.GetUnreadMessageCountForUser(CurrentUserId, SelectedLoginTeam);
                if (UnreadChatCount > previousCount && previousCount >= 0)
                {
                    try { System.Media.SystemSounds.Asterisk.Play(); } catch { }
                }
            }
            catch (System.Exception)
            {
                // Ignore background polling errors
            }
        }

        private void NavigateToChat()
        {
            CurrentViewModel = new ChatViewModel(CurrentUserId, CurrentUserName, SelectedLoginTeam, IsCurrentUserAdmin);
            PollChatUnreadCount();
        }

        private DashboardViewModel CreateDashboardViewModel(bool isAdmin, string userName)
        {
            var dashVm = new DashboardViewModel(
                isAdmin, 
                userName, 
                CreateLeaveRequest, 
                NavigateToShifts, 
                NavigateToProjects, 
                NavigateToUsers,
                CreateLeaveRequest,
                CreateShift,
                CreateProject,
                CreateUser);
            dashVm.RefreshDashboard(userName);
            return dashVm;
        }

        private void NavigateToShifts() => CurrentViewModel = new ShiftsViewModel(IsCurrentUserAdmin, CurrentUserName);
        private void NavigateToLeaves() => CurrentViewModel = new LeavesViewModel(IsCurrentUserAdmin, CurrentUserName);
        private void NavigateToProjects() => CurrentViewModel = new ProjectsViewModel(IsCurrentUserAdmin, CurrentUserName, SelectedLoginTeam);
        private void NavigateToUsers() => CurrentViewModel = new UsersViewModel(IsCurrentUserAdmin);

        private void CreateLeaveRequest() => CurrentViewModel = new LeavesViewModel(IsCurrentUserAdmin, CurrentUserName, initialTabIndex: 1);
        private void CreateShift() => CurrentViewModel = new ShiftsViewModel(IsCurrentUserAdmin, CurrentUserName, openForm: true);
        private void CreateProject() => CurrentViewModel = new ProjectsViewModel(IsCurrentUserAdmin, CurrentUserName, SelectedLoginTeam);
        private void CreateUser() => CurrentViewModel = new UsersViewModel(IsCurrentUserAdmin);
    }
}
