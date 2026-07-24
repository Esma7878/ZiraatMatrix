using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using ZiraatProje.Business;
using ZiraatProje.DataAccess;

namespace ZiraatProje.UI.ViewModels
{
    public class DirectoryTeamGroup
    {
        public string TeamName { get; set; } = string.Empty;
        public ObservableCollection<User> Members { get; set; } = new ObservableCollection<User>();
    }

    public class UsersViewModel : BaseViewModel
    {
        private readonly BusinessServices _services = new BusinessServices();

        private bool _isCurrentUserAdmin;
        public bool IsCurrentUserAdmin
        {
            get => _isCurrentUserAdmin;
            set
            {
                _isCurrentUserAdmin = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(AdminPanelVisibility));
                OnPropertyChanged(nameof(UserDirectoryVisibility));
            }
        }

        public Visibility AdminPanelVisibility => IsCurrentUserAdmin ? Visibility.Visible : Visibility.Collapsed;
        public Visibility UserDirectoryVisibility => IsCurrentUserAdmin ? Visibility.Collapsed : Visibility.Visible;

        private DirectoryTeamGroup? _managerGroup;
        public DirectoryTeamGroup? ManagerGroup
        {
            get => _managerGroup;
            set { _managerGroup = value; OnPropertyChanged(); }
        }

        // Team Directory Groups for Regular Users (and View Directory mode)
        private ObservableCollection<DirectoryTeamGroup> _teamGroups = new ObservableCollection<DirectoryTeamGroup>();
        public ObservableCollection<DirectoryTeamGroup> TeamGroups
        {
            get => _teamGroups;
            set { _teamGroups = value; OnPropertyChanged(); }
        }

        // Contact Detail Modal / Card
        private User? _selectedContactUser;
        public User? SelectedContactUser
        {
            get => _selectedContactUser;
            set
            {
                _selectedContactUser = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasSelectedContact));
                OnPropertyChanged(nameof(ContactCardVisibility));
            }
        }

        public bool HasSelectedContact => SelectedContactUser != null;
        public Visibility ContactCardVisibility => HasSelectedContact ? Visibility.Visible : Visibility.Collapsed;

        private ObservableCollection<User> _usersList = new ObservableCollection<User>();
        public ObservableCollection<User> UsersList
        {
            get => _usersList;
            set { _usersList = value; OnPropertyChanged(); }
        }

        private ICollectionView? _usersViewCollection;
        public ICollectionView? UsersViewCollection
        {
            get => _usersViewCollection;
            set { _usersViewCollection = value; OnPropertyChanged(); }
        }

        private ObservableCollection<Team> _teamsList = new ObservableCollection<Team>();
        public ObservableCollection<Team> TeamsList
        {
            get => _teamsList;
            set { _teamsList = value; OnPropertyChanged(); }
        }

        private string _newTeamName = string.Empty;
        public string NewTeamName
        {
            get => _newTeamName;
            set { _newTeamName = value; OnPropertyChanged(); }
        }

        // Form Toggle States for Responsive Full-Width Grid
        private bool _isUserFormOpen;
        public bool IsUserFormOpen
        {
            get => _isUserFormOpen;
            set
            {
                _isUserFormOpen = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(UserFormVisibility));
                OnPropertyChanged(nameof(FormColumnWidth));
            }
        }

        private bool _isTeamFormOpen;
        public bool IsTeamFormOpen
        {
            get => _isTeamFormOpen;
            set
            {
                _isTeamFormOpen = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TeamFormVisibility));
                OnPropertyChanged(nameof(FormColumnWidth));
            }
        }

        public Visibility UserFormVisibility => IsUserFormOpen ? Visibility.Visible : Visibility.Collapsed;
        public Visibility TeamFormVisibility => IsTeamFormOpen ? Visibility.Visible : Visibility.Collapsed;
        public GridLength FormColumnWidth => (IsUserFormOpen || IsTeamFormOpen) ? new GridLength(370, GridUnitType.Pixel) : new GridLength(0, GridUnitType.Pixel);

        private User? _selectedUser;
        public User? SelectedUser
        {
            get => _selectedUser;
            set
            {
                if (_selectedUser == value) return;
                _selectedUser = value;
                OnPropertyChanged();
                LoadSelectedUser();
                if (_selectedUser != null)
                {
                    IsTeamFormOpen = false;
                    IsUserFormOpen = true;
                }
            }
        }

        // Form Fields
        private string _name = string.Empty;
        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        private string _surname = string.Empty;
        public string Surname
        {
            get => _surname;
            set { _surname = value; OnPropertyChanged(); }
        }

        private string _title = "Developer";
        public string Title
        {
            get => _title;
            set { _title = value; OnPropertyChanged(); }
        }

        private string _team = "Takip";
        public string Team
        {
            get => _team;
            set { _team = value; OnPropertyChanged(); }
        }

        private string _email = string.Empty;
        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); }
        }

        private string _userPhone = string.Empty;
        public string UserPhone
        {
            get => _userPhone;
            set 
            { 
                _userPhone = FormatTurkishPhoneNumber(value); 
                OnPropertyChanged(); 
            }
        }

        private string FormatTurkishPhoneNumber(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return string.Empty;

            string digits = new string(value.Where(char.IsDigit).ToArray());

            // If string starts with country code 90 (e.g. +90 pasted or from DB), strip 90
            if (digits.StartsWith("90") && digits.Length > 2)
            {
                digits = digits.Substring(2);
            }

            // Strip leading 0
            while (digits.StartsWith("0"))
            {
                digits = digits.Substring(1);
            }

            if (digits.Length > 10)
            {
                digits = digits.Substring(0, 10);
            }

            if (digits.Length == 0) return string.Empty;

            if (digits.Length <= 3)
            {
                return $"({digits}";
            }
            if (digits.Length <= 6)
            {
                return $"({digits.Substring(0, 3)}) {digits.Substring(3)}";
            }
            if (digits.Length <= 8)
            {
                return $"({digits.Substring(0, 3)}) {digits.Substring(3, 3)} {digits.Substring(6)}";
            }

            return $"({digits.Substring(0, 3)}) {digits.Substring(3, 3)} {digits.Substring(6, 2)} {digits.Substring(8)}";
        }

        private bool _isAdmin;
        public bool IsAdmin
        {
            get => _isAdmin;
            set { _isAdmin = value; OnPropertyChanged(); }
        }

        private bool _autoAssignPassword = true;
        public bool AutoAssignPassword
        {
            get => _autoAssignPassword;
            set { _autoAssignPassword = value; OnPropertyChanged(); }
        }

        private string _statusMessage = string.Empty;
        public string StatusMessage
        {
            get => _statusMessage;
            set { _statusMessage = value; OnPropertyChanged(); }
        }

        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand ClearFormCommand { get; }
        public ICommand AddTeamCommand { get; }
        public ICommand RemoveTeamCommand { get; }
        public ICommand SelectContactUserCommand { get; }
        public ICommand CloseContactCardCommand { get; }
        public ICommand OpenUserFormCommand { get; }
        public ICommand CloseUserFormCommand { get; }
        public ICommand OpenTeamFormCommand { get; }
        public ICommand CloseTeamFormCommand { get; }

        public UsersViewModel() : this(false) { }

        public UsersViewModel(bool isCurrentUserAdmin)
        {
            IsCurrentUserAdmin = isCurrentUserAdmin;

            SaveCommand = new RelayCommand(ExecuteSave);
            DeleteCommand = new RelayCommand(ExecuteDelete, CanDelete);
            ClearFormCommand = new RelayCommand(ExecuteClearForm);
            AddTeamCommand = new RelayCommand(ExecuteAddTeam);
            RemoveTeamCommand = new RelayCommand(ExecuteRemoveTeam);
            SelectContactUserCommand = new RelayCommand(param =>
            {
                if (param is User u)
                {
                    SelectedContactUser = u;
                }
            });
            CloseContactCardCommand = new RelayCommand(_ => SelectedContactUser = null);

            OpenUserFormCommand = new RelayCommand(_ =>
            {
                ExecuteClearForm(null);
                IsTeamFormOpen = false;
                IsUserFormOpen = true;
            });
            CloseUserFormCommand = new RelayCommand(_ => { IsUserFormOpen = false; });

            OpenTeamFormCommand = new RelayCommand(_ =>
            {
                IsUserFormOpen = false;
                IsTeamFormOpen = true;
            });
            CloseTeamFormCommand = new RelayCommand(_ => { IsTeamFormOpen = false; });

            LoadTeams();
            LoadUsers();
        }

        private void LoadTeams()
        {
            try
            {
                var teams = _services.GetAllTeams();
                TeamsList = new ObservableCollection<Team>(teams);
                if (string.IsNullOrEmpty(Team) && TeamsList.Count > 0)
                {
                    Team = TeamsList[0].TeamName;
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Ekip yükleme hatası: {ex.Message}";
            }
        }

        private void ExecuteAddTeam(object? param)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(NewTeamName))
                {
                    MessageBox.Show("Lütfen eklenecek ekip adını giriniz.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                _services.AddTeam(new Team { TeamName = NewTeamName.Trim() });
                StatusMessage = $"'{NewTeamName}' ekibi başarıyla eklendi.";
                NewTeamName = string.Empty;
                LoadTeams();
                LoadUsers();
            }
            catch (ValidationException vex)
            {
                MessageBox.Show(vex.Message, "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ekip eklenirken hata: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteRemoveTeam(object? param)
        {
            if (param is Team team)
            {
                var confirm = MessageBox.Show($"'{team.TeamName}' ekibini silmek istediğinize emin misiniz?", "Ekip Sil", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (confirm == MessageBoxResult.Yes)
                {
                    try
                    {
                        _services.DeleteTeam(team.Id);
                        StatusMessage = $"'{team.TeamName}' ekibi silindi.";
                        LoadTeams();
                        LoadUsers();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ekip silinirken hata: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void LoadUsers()
        {
            try
            {
                var users = _services.GetAllUsers().OrderBy(u => u.Team).ThenBy(u => u.Name).ToList();
                UsersList = new ObservableCollection<User>(users);

                var cvs = CollectionViewSource.GetDefaultView(UsersList);
                cvs.GroupDescriptions.Clear();
                cvs.GroupDescriptions.Add(new PropertyGroupDescription("Team"));
                UsersViewCollection = cvs;

                // Build Team Groups for Directory View
                var groups = new ObservableCollection<DirectoryTeamGroup>();

                // 1. Departman Yönetimi Group (ManagerGroup, centered at top)
                DirectoryTeamGroup? managerGroup = null;
                var deptMembers = users.Where(u => u.Team != null && u.Team.Equals("Departman Yönetimi", StringComparison.OrdinalIgnoreCase)).ToList();
                if (deptMembers.Any())
                {
                    managerGroup = new DirectoryTeamGroup
                    {
                        TeamName = "Departman Yönetimi",
                        Members = new ObservableCollection<User>(deptMembers)
                    };
                }
                ManagerGroup = managerGroup;

                // 2. Main Teams (Takip, Tahsis, Teminat etc.)
                var teams = _services.GetAllTeams();
                foreach (var t in teams)
                {
                    if (t.TeamName.Equals("Departman Yönetimi", StringComparison.OrdinalIgnoreCase)) continue;

                    var members = users.Where(u => u.Team != null && u.Team.Equals(t.TeamName, StringComparison.OrdinalIgnoreCase)).ToList();
                    groups.Add(new DirectoryTeamGroup
                    {
                        TeamName = t.TeamName,
                        Members = new ObservableCollection<User>(members)
                    });
                }

                // 3. Any other remaining users
                var knownNames = teams.Select(t => t.TeamName.ToLower()).ToList();
                knownNames.Add("departman yönetimi");
                var remainingUsers = users.Where(u => u.Team == null || !knownNames.Contains(u.Team.ToLower())).ToList();
                if (remainingUsers.Any())
                {
                    groups.Add(new DirectoryTeamGroup
                    {
                        TeamName = "Diğer Ekipler",
                        Members = new ObservableCollection<User>(remainingUsers)
                    });
                }

                TeamGroups = groups;
            }
            catch (Exception ex)
            {
                StatusMessage = $"Hata: {ex.Message}";
            }
        }

        private void LoadSelectedUser()
        {
            if (SelectedUser != null)
            {
                Name = SelectedUser.Name;
                Surname = SelectedUser.Surname;
                Title = SelectedUser.Title;
                Team = SelectedUser.Team;
                Email = SelectedUser.Email;
                UserPhone = SelectedUser.Phone ?? string.Empty;
                IsAdmin = SelectedUser.IsAdmin;
            }
            else
            {
                ClearFormFields();
            }
        }

        private void ClearFormFields()
        {
            Name = string.Empty;
            Surname = string.Empty;
            Title = "Developer";
            Team = TeamsList.Count > 0 ? TeamsList[0].TeamName : "Takip";
            Email = string.Empty;
            UserPhone = string.Empty;
            IsAdmin = false;
            StatusMessage = string.Empty;
        }

        private void ExecuteSave(object? param)
        {
            try
            {
                string formattedPhoneToSave = string.IsNullOrWhiteSpace(UserPhone) ? string.Empty : $"+90 {UserPhone}";

                if (SelectedUser == null)
                {
                    // Create New User
                    var initialPassword = AutoAssignPassword ? "1234" : "1234";
                    var newUser = new User
                    {
                        Name = Name,
                        Surname = Surname,
                        Title = Title,
                        Team = Team,
                        Email = Email,
                        Phone = formattedPhoneToSave,
                        Password = initialPassword,
                        IsAdmin = IsAdmin
                    };
                    _services.AddUser(newUser);
                    StatusMessage = $"Kullanıcı eklendi. (Geçici Şifre: {initialPassword})";
                }
                else
                {
                    // Update User
                    SelectedUser.Name = Name;
                    SelectedUser.Surname = Surname;
                    SelectedUser.Title = Title;
                    SelectedUser.Team = Team;
                    SelectedUser.Email = Email;
                    SelectedUser.Phone = formattedPhoneToSave;
                    SelectedUser.IsAdmin = IsAdmin;

                    _services.UpdateUser(SelectedUser);
                    StatusMessage = "Kullanıcı güncellendi.";
                }

                LoadUsers();
                ExecuteClearForm(null);
            }
            catch (ValidationException vex)
            {
                MessageBox.Show(vex.Message, "Doğrulama Hatası", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Sistemsel Hata: {ex.Message}", "Sistem Hatası", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteDelete(object? param)
        {
            if (SelectedUser == null) return;

            var result = MessageBox.Show($"{SelectedUser.FullName} isimli personeli silmek istediğinize emin misiniz?", "Personel Sil", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _services.DeleteUser(SelectedUser.Id);
                    StatusMessage = "Personel başarıyla silindi.";
                    LoadUsers();
                    ExecuteClearForm(null);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Silme hatası: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private bool CanDelete(object? param)
        {
            return SelectedUser != null;
        }

        private void ExecuteClearForm(object? param)
        {
            _selectedUser = null;
            OnPropertyChanged(nameof(SelectedUser));
            ClearFormFields();
        }
    }
}
