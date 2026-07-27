using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Threading;
using ZiraatProje.Business;
using ZiraatProje.DataAccess;

namespace ZiraatProje.UI.ViewModels
{
    public class ChatChannelItem : BaseViewModel
    {
        public string Title { get; set; } = string.Empty;
        public string Subtitle { get; set; } = string.Empty;
        public string Icon { get; set; } = "💬";
        public string TargetType { get; set; } = "General"; // "General", "Team", "Direct"
        public int? TargetUserId { get; set; }
        public string TargetTeam { get; set; } = string.Empty;
        public string BadgeColor { get; set; } = "#64748b";

        private int _unreadCount;
        public int UnreadCount
        {
            get => _unreadCount;
            set 
            { 
                _unreadCount = value; 
                OnPropertyChanged(); 
                OnPropertyChanged(nameof(HasUnread)); 
                OnPropertyChanged(nameof(UnreadBadgeVisibility));
            }
        }
        public bool HasUnread => UnreadCount > 0;
        public System.Windows.Visibility UnreadBadgeVisibility => HasUnread ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set 
            { 
                _isSelected = value; 
                OnPropertyChanged();
                OnPropertyChanged(nameof(SelectedBackground));
                OnPropertyChanged(nameof(SelectedBorderBrush));
                OnPropertyChanged(nameof(SelectedBorderThickness));
            }
        }

        public string SelectedBackground
        {
            get
            {
                if (!IsSelected) return "#ffffff";
                return TargetTeam switch
                {
                    "Takip" => "#f5f3ff",
                    "Tahsis" => "#ecfdf5",
                    "Teminat" => "#eff6ff",
                    _ => TargetType == "General" ? "#fef2f2" : "#f8fafc"
                };
            }
        }

        public string SelectedBorderBrush
        {
            get
            {
                if (!IsSelected) return "#e2e8f0";
                return TargetTeam switch
                {
                    "Takip" => "#5b21b6",
                    "Tahsis" => "#065f46",
                    "Teminat" => "#1e40af",
                    _ => TargetType == "General" ? "#bc171d" : "#475569"
                };
            }
        }

        public string SelectedBorderThickness => IsSelected ? "1.5" : "0.5";
    }

    public class ChatMessageDisplayItem
    {
        public int Id { get; set; }
        public string SenderName { get; set; } = string.Empty;
        public string SenderTitle { get; set; } = string.Empty;
        public string MessageText { get; set; } = string.Empty;
        public string TimeText { get; set; } = string.Empty;
        public bool IsMyMessage { get; set; }
        public string BubbleBackground => IsMyMessage ? "#bc171d" : "#ffffff";
        public string BubbleForeground => IsMyMessage ? "#ffffff" : "#1e293b";
        public string BubbleBorder => IsMyMessage ? "#991116" : "#cbd5e1";
        public string Alignment => IsMyMessage ? "Right" : "Left";
        public string SenderBadgeBg => IsMyMessage ? "#ffffff" : "#f1f5f9";
    }

    public class UserSelectableChatItem : BaseViewModel
    {
        public User User { get; set; } = null!;
        public string FullName => User.FullName;
        public string TitleAndTeam => $"{User.Title} • {User.Team} Ekibi";
        public string Icon => User.IsAdmin ? "👑" : "👤";

        private bool _isSelectedForGroup;
        public bool IsSelectedForGroup
        {
            get => _isSelectedForGroup;
            set { _isSelectedForGroup = value; OnPropertyChanged(); }
        }
    }

    public class ChatViewModel : BaseViewModel
    {
        private readonly BusinessServices _services;
        private readonly DispatcherTimer _pollTimer;
        public int CurrentUserId { get; }
        public string CurrentUserName { get; }
        public string UserTeam { get; }
        public bool IsAdmin { get; }

        public ObservableCollection<ChatChannelItem> Channels { get; } = new ObservableCollection<ChatChannelItem>();
        public ObservableCollection<ChatMessageDisplayItem> Messages { get; } = new ObservableCollection<ChatMessageDisplayItem>();
        public ObservableCollection<UserSelectableChatItem> AllUsersList { get; } = new ObservableCollection<UserSelectableChatItem>();

        private ChatChannelItem? _selectedChannel;
        public ChatChannelItem? SelectedChannel
        {
            get => _selectedChannel;
            set
            {
                if (_selectedChannel != value)
                {
                    if (_selectedChannel != null) _selectedChannel.IsSelected = false;
                    _selectedChannel = value;
                    if (_selectedChannel != null) _selectedChannel.IsSelected = true;
                    NotificationBannerText = string.Empty;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(ActiveChannelTitle));
                    OnPropertyChanged(nameof(ActiveChannelSubtitle));
                    LoadMessagesForSelectedChannel();
                }
            }
        }

        private string _newMessageText = string.Empty;
        public string NewMessageText
        {
            get => _newMessageText;
            set { _newMessageText = value; OnPropertyChanged(); }
        }

        private bool _isNewChatModalOpen;
        public bool IsNewChatModalOpen
        {
            get => _isNewChatModalOpen;
            set 
            { 
                _isNewChatModalOpen = value; 
                OnPropertyChanged(); 
                OnPropertyChanged(nameof(NewChatModalVisibility));
            }
        }
        public System.Windows.Visibility NewChatModalVisibility => IsNewChatModalOpen ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;

        private bool _isGroupCreationTab;
        public bool IsGroupCreationTab
        {
            get => _isGroupCreationTab;
            set
            {
                _isGroupCreationTab = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsDirectChatTab));
                OnPropertyChanged(nameof(DirectTabBg));
                OnPropertyChanged(nameof(GroupTabBg));
                OnPropertyChanged(nameof(DirectTabVisibility));
                OnPropertyChanged(nameof(GroupTabVisibility));
            }
        }

        public bool IsDirectChatTab => !IsGroupCreationTab;
        public System.Windows.Visibility DirectTabVisibility => IsDirectChatTab ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
        public System.Windows.Visibility GroupTabVisibility => IsGroupCreationTab ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
        public string DirectTabBg => !IsGroupCreationTab ? "#bc171d" : "#f1f5f9";
        public string GroupTabBg => IsGroupCreationTab ? "#bc171d" : "#f1f5f9";

        private string _newGroupName = string.Empty;
        public string NewGroupName
        {
            get => _newGroupName;
            set { _newGroupName = value; OnPropertyChanged(); }
        }

        private string _userSearchQuery = string.Empty;
        public string UserSearchQuery
        {
            get => _userSearchQuery;
            set
            {
                _userSearchQuery = value;
                OnPropertyChanged();
                FilterUserList();
            }
        }

        public string ActiveChannelTitle => SelectedChannel?.Title ?? "Bir Sohbet Kanalı Seçin";
        public string ActiveChannelSubtitle => SelectedChannel?.Subtitle ?? "Kanal veya kişi seçerek mesajlaşmaya başlayın.";

        public ICommand SendMessageCommand { get; }
        public ICommand SelectChannelCommand { get; }
        public ICommand ClearChatCommand { get; }
        public ICommand OpenNewChatModalCommand { get; }
        public ICommand CloseNewChatModalCommand { get; }
        public ICommand SelectUserToChatCommand { get; }
        public ICommand SwitchToDirectTabCommand { get; }
        public ICommand SwitchToGroupTabCommand { get; }
        public ICommand CreateGroupCommand { get; }

        public ChatViewModel(int currentUserId, string currentUserName, string userTeam, bool isAdmin)
        {
            _services = new BusinessServices();
            CurrentUserId = currentUserId;
            CurrentUserName = currentUserName;
            UserTeam = string.IsNullOrWhiteSpace(userTeam) ? "Takip" : userTeam;
            IsAdmin = isAdmin;

            // Trigger auto-cleanup of previous days' chat messages on load
            _services.CleanupOldChatMessages();

            SendMessageCommand = new RelayCommand(_ => SendMessage(), _ => !string.IsNullOrWhiteSpace(NewMessageText));
            SelectChannelCommand = new RelayCommand(param => { if (param is ChatChannelItem item) SelectedChannel = item; });
            ClearChatCommand = new RelayCommand(_ => ClearAllChatHistory());
            OpenNewChatModalCommand = new RelayCommand(_ => { IsNewChatModalOpen = true; IsGroupCreationTab = false; NewGroupName = string.Empty; LoadUserList(); });
            CloseNewChatModalCommand = new RelayCommand(_ => IsNewChatModalOpen = false);
            SelectUserToChatCommand = new RelayCommand(param => { if (param is UserSelectableChatItem u) StartChatWithUser(u.User); });
            SwitchToDirectTabCommand = new RelayCommand(_ => IsGroupCreationTab = false);
            SwitchToGroupTabCommand = new RelayCommand(_ => IsGroupCreationTab = true);
            CreateGroupCommand = new RelayCommand(_ => ExecuteCreateGroup());

            BuildChannelList();

            // Default select General Channel
            SelectedChannel = Channels.FirstOrDefault(c => c.TargetType == "General");

            // Setup polling timer every 1.5 seconds for real-time offline chat
            _pollTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1.5) };
            _pollTimer.Tick += (s, e) => PollNewMessages();
            _pollTimer.Start();
        }

        private void ExecuteCreateGroup()
        {
            if (string.IsNullOrWhiteSpace(NewGroupName))
            {
                System.Windows.MessageBox.Show("Lütfen sohbet grubu için bir isim giriniz.", "Uyarı", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }

            var selectedUserIds = AllUsersList.Where(u => u.IsSelectedForGroup).Select(u => u.User.Id).ToList();
            if (selectedUserIds.Count == 0)
            {
                System.Windows.MessageBox.Show("Lütfen gruba eklenecek en az 1 kişi seçiniz.", "Uyarı", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }

            try
            {
                var createdGroup = _services.CreateChatGroup(NewGroupName, selectedUserIds, CurrentUserId);
                IsNewChatModalOpen = false;
                BuildChannelList();

                var newChannel = Channels.FirstOrDefault(c => c.TargetType == "Group" && c.TargetTeam == createdGroup.Id.ToString());
                if (newChannel != null)
                {
                    SelectedChannel = newChannel;
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Hata", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        private void BuildChannelList()
        {
            var selectedBackup = SelectedChannel;
            Channels.Clear();

            // 1. General Department Channel (Ziraat Red Theme)
            Channels.Add(new ChatChannelItem
            {
                Title = "📢 Genel Departman Sohbeti",
                Subtitle = "Katılım Finansman Tahsis ve Takip Ortak Kanalı",
                Icon = "🌐",
                TargetType = "General",
                BadgeColor = "#bc171d"
            });

            // 2. Team Channels
            if (!string.IsNullOrWhiteSpace(UserTeam))
            {
                var allDbTeams = _services.GetAllTeams();

                if (UserTeam.Equals("Departman Yönetimi", StringComparison.OrdinalIgnoreCase) || UserTeam.Equals("Yönetim", StringComparison.OrdinalIgnoreCase))
                {
                    foreach (var t in allDbTeams)
                    {
                        if (t.TeamName.Equals("Departman Yönetimi", StringComparison.OrdinalIgnoreCase)) continue;
                        Channels.Add(new ChatChannelItem
                        {
                            Title = $"📌 {t.TeamName} Ekibi Kanalı",
                            Subtitle = $"{t.TeamName} ekibi özel dahili yazışma odası",
                            Icon = "📌",
                            TargetType = "Team",
                            TargetTeam = t.TeamName,
                            BadgeColor = !string.IsNullOrWhiteSpace(t.Color) ? t.Color : "#7c3aed"
                        });
                    }
                }
                else
                {
                    var userTeamObj = allDbTeams.FirstOrDefault(t => t.TeamName.Equals(UserTeam, StringComparison.OrdinalIgnoreCase));
                    string badgeCol = userTeamObj?.Color ?? "#7c3aed";
                    Channels.Add(new ChatChannelItem
                    {
                        Title = $"📌 {UserTeam} Ekibi Kanalı",
                        Subtitle = $"{UserTeam} ekibi özel dahili yazışma odası",
                        Icon = "📌",
                        TargetType = "Team",
                        TargetTeam = UserTeam,
                        BadgeColor = badgeCol
                    });
                }
            }

            // 3. Custom WhatsApp-Style Group Channels for user
            var myGroups = _services.GetChatGroupsForUser(CurrentUserId);
            var allUsers = _services.GetAllUsers();
            foreach (var g in myGroups)
            {
                var memberIds = g.MemberUserIds.Split(',').Select(id => int.TryParse(id, out int parsed) ? parsed : 0).Where(id => id > 0).ToList();
                var memberNames = allUsers.Where(u => memberIds.Contains(u.Id)).Select(u => u.Name).ToList();
                string namesStr = string.Join(", ", memberNames.Take(3));
                if (memberNames.Count > 3) namesStr += $" +{memberNames.Count - 3} kişi";

                Channels.Add(new ChatChannelItem
                {
                    Title = $"👥 {g.GroupName}",
                    Subtitle = $"{memberIds.Count} Katılımcı • {namesStr}",
                    Icon = "👥",
                    TargetType = "Group",
                    TargetTeam = g.Id.ToString(),
                    BadgeColor = "#7c3aed"
                });
            }

            // 4. Load active direct chat users for today
            var activeDirectUsers = _services.GetActiveDirectChatUsersForUser(CurrentUserId);
            foreach (var u in activeDirectUsers)
            {
                AddUserToChannelList(u);
            }

            SortChannelsByLatestActivity();

            if (selectedBackup != null)
            {
                var match = Channels.FirstOrDefault(c => c.TargetType == selectedBackup.TargetType &&
                                                          c.TargetUserId == selectedBackup.TargetUserId &&
                                                          c.TargetTeam == selectedBackup.TargetTeam);
                if (match != null) SelectedChannel = match;
            }
        }

        private void LoadUserList()
        {
            UserSearchQuery = string.Empty;
            FilterUserList();
        }

        private void FilterUserList()
        {
            AllUsersList.Clear();
            var users = _services.GetAllUsers().Where(u => u.Id != CurrentUserId);
            if (!string.IsNullOrWhiteSpace(UserSearchQuery))
            {
                var q = UserSearchQuery.ToLower().Trim();
                users = users.Where(u => u.FullName.ToLower().Contains(q) || u.Title.ToLower().Contains(q) || u.Team.ToLower().Contains(q));
            }

            foreach (var u in users)
            {
                AllUsersList.Add(new UserSelectableChatItem { User = u });
            }
        }

        private void StartChatWithUser(User u)
        {
            IsNewChatModalOpen = false;

            var existing = Channels.FirstOrDefault(c => c.TargetType == "Direct" && c.TargetUserId == u.Id);
            if (existing == null)
            {
                existing = AddUserToChannelList(u);
            }

            SelectedChannel = existing;
            SortChannelsByLatestActivity();
        }

        private ChatChannelItem AddUserToChannelList(User u)
        {
            var item = new ChatChannelItem
            {
                Title = $"👤 {u.FullName}",
                Subtitle = $"{u.Title} • {u.Team} Ekibi",
                Icon = u.IsAdmin ? "👑" : "👤",
                TargetType = "Direct",
                TargetUserId = u.Id,
                BadgeColor = "#475569"
            };
            Channels.Add(item);
            return item;
        }

        private void SortChannelsByLatestActivity()
        {
            var list = Channels.ToList();
            var sorted = list.OrderByDescending(c => _services.GetLastMessageTimeForChannel(CurrentUserId, c.TargetType, c.TargetUserId, c.TargetTeam))
                             .ToList();

            for (int i = 0; i < sorted.Count; i++)
            {
                int oldIndex = Channels.IndexOf(sorted[i]);
                if (oldIndex != i && oldIndex >= 0)
                {
                    Channels.Move(oldIndex, i);
                }
            }
        }

        private void LoadMessagesForSelectedChannel()
        {
            if (SelectedChannel == null)
            {
                Messages.Clear();
                return;
            }

            // Mark unread messages as read
            _services.MarkMessagesAsRead(CurrentUserId, SelectedChannel.TargetType, SelectedChannel.TargetUserId, SelectedChannel.TargetTeam);
            SelectedChannel.UnreadCount = 0;

            var list = _services.GetChatMessagesForChannel(CurrentUserId, SelectedChannel.TargetType, SelectedChannel.TargetUserId, SelectedChannel.TargetTeam);

            Messages.Clear();
            foreach (var m in list)
            {
                bool isMine = m.SenderUserId == CurrentUserId;
                Messages.Add(new ChatMessageDisplayItem
                {
                    Id = m.Id,
                    SenderName = m.SenderUser?.FullName ?? (isMine ? CurrentUserName : "Personel"),
                    SenderTitle = m.SenderUser?.Title ?? "",
                    MessageText = m.MessageText,
                    TimeText = m.SentAt.ToString("HH:mm"),
                    IsMyMessage = isMine
                });
            }
        }

        private System.Windows.Threading.DispatcherTimer? _bannerDismissTimer;
        private string _notificationBannerText = string.Empty;
        public string NotificationBannerText
        {
            get => _notificationBannerText;
            set
            {
                _notificationBannerText = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasNotificationBanner));
                OnPropertyChanged(nameof(NotificationBannerVisibility));

                _bannerDismissTimer?.Stop();
                if (!string.IsNullOrWhiteSpace(_notificationBannerText))
                {
                    _bannerDismissTimer = new System.Windows.Threading.DispatcherTimer { Interval = TimeSpan.FromSeconds(4) };
                    _bannerDismissTimer.Tick += (s, e) =>
                    {
                        _bannerDismissTimer.Stop();
                        _notificationBannerText = string.Empty;
                        OnPropertyChanged(nameof(NotificationBannerText));
                        OnPropertyChanged(nameof(HasNotificationBanner));
                        OnPropertyChanged(nameof(NotificationBannerVisibility));
                    };
                    _bannerDismissTimer.Start();
                }
            }
        }

        public bool HasNotificationBanner => !string.IsNullOrWhiteSpace(NotificationBannerText);
        public System.Windows.Visibility NotificationBannerVisibility => HasNotificationBanner ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;

        public ICommand DismissNotificationBannerCommand => new RelayCommand(_ => NotificationBannerText = string.Empty);

        private bool IsSameChannel(ChatChannelItem a, ChatChannelItem? b)
        {
            if (b == null) return false;
            if (a.TargetType != b.TargetType) return false;

            if (a.TargetType == "Direct")
                return a.TargetUserId == b.TargetUserId;
            if (a.TargetType == "Team")
                return string.Equals(a.TargetTeam, b.TargetTeam, StringComparison.OrdinalIgnoreCase);
            if (a.TargetType == "Group")
                return string.Equals(a.TargetTeam, b.TargetTeam, StringComparison.OrdinalIgnoreCase);

            return true; // General channel
        }

        private void PollNewMessages()
        {
            // Check for new direct chat partners or new groups created
            var activeUsers = _services.GetActiveDirectChatUsersForUser(CurrentUserId);
            bool channelAdded = false;
            foreach (var u in activeUsers)
            {
                if (!Channels.Any(c => c.TargetType == "Direct" && c.TargetUserId == u.Id))
                {
                    AddUserToChannelList(u);
                    channelAdded = true;
                }
            }

            var myGroups = _services.GetChatGroupsForUser(CurrentUserId);
            var allUsers = _services.GetAllUsers();
            foreach (var g in myGroups)
            {
                if (!Channels.Any(c => c.TargetType == "Group" && c.TargetTeam == g.Id.ToString()))
                {
                    var memberIds = g.MemberUserIds.Split(',').Select(id => int.TryParse(id, out int parsed) ? parsed : 0).Where(id => id > 0).ToList();
                    var memberNames = allUsers.Where(u => memberIds.Contains(u.Id)).Select(u => u.Name).ToList();
                    string namesStr = string.Join(", ", memberNames.Take(3));
                    if (memberNames.Count > 3) namesStr += $" +{memberNames.Count - 3} kişi";

                    Channels.Add(new ChatChannelItem
                    {
                        Title = $"👥 {g.GroupName}",
                        Subtitle = $"{memberIds.Count} Katılımcı • {namesStr}",
                        Icon = "👥",
                        TargetType = "Group",
                        TargetTeam = g.Id.ToString(),
                        BadgeColor = "#7c3aed"
                    });
                    channelAdded = true;
                }
            }

            // Update unread counts on all channels
            foreach (var chan in Channels)
            {
                if (IsSameChannel(chan, SelectedChannel))
                {
                    chan.UnreadCount = 0;
                }
                else
                {
                    var msgs = _services.GetChatMessagesForChannel(CurrentUserId, chan.TargetType, chan.TargetUserId, chan.TargetTeam);
                    int prevUnread = chan.UnreadCount;
                    if (chan.TargetType == "Direct")
                    {
                        chan.UnreadCount = msgs.Count(m => m.ReceiverUserId == CurrentUserId && !m.IsRead);
                    }
                    else
                    {
                        chan.UnreadCount = msgs.Count(m => m.SenderUserId != CurrentUserId && !m.IsRead);
                    }

                    // Trigger notification banner ONLY if new unread message arrived in a DIFFERENT channel!
                    if (chan.UnreadCount > prevUnread && msgs.Any() && !IsSameChannel(chan, SelectedChannel))
                    {
                        var lastMsg = msgs.Last();
                        string senderName = lastMsg.SenderUser?.FullName ?? "Bir çalışma arkadaşınız";
                        string preview = lastMsg.MessageText.Length > 40 ? lastMsg.MessageText.Substring(0, 40) + "..." : lastMsg.MessageText;
                        NotificationBannerText = $"🔔 {senderName} yeni bir mesaj gönderdi: \"{preview}\"";
                    }
                }
            }

            if (channelAdded)
            {
                SortChannelsByLatestActivity();
            }

            if (SelectedChannel == null) return;

            var currentList = _services.GetChatMessagesForChannel(CurrentUserId, SelectedChannel.TargetType, SelectedChannel.TargetUserId, SelectedChannel.TargetTeam);

            // Check if any new message came in for selected channel
            if (currentList.Count != Messages.Count || (currentList.Any() && currentList.Last().Id != Messages.LastOrDefault()?.Id))
            {
                LoadMessagesForSelectedChannel();
                SortChannelsByLatestActivity();
            }
        }

        private void SendMessage()
        {
            if (SelectedChannel == null || string.IsNullOrWhiteSpace(NewMessageText)) return;

            var msg = new ChatMessage
            {
                SenderUserId = CurrentUserId,
                ReceiverUserId = SelectedChannel.TargetType == "Direct" ? SelectedChannel.TargetUserId : null,
                TargetType = SelectedChannel.TargetType,
                TargetTeam = SelectedChannel.TargetTeam,
                MessageText = NewMessageText.Trim(),
                SentAt = DateTime.Now,
                IsRead = false
            };

            _services.SendChatMessage(msg);
            NewMessageText = string.Empty;

            LoadMessagesForSelectedChannel();
            SortChannelsByLatestActivity();
        }

        private void ClearAllChatHistory()
        {
            _services.ClearAllChatMessages();
            LoadMessagesForSelectedChannel();
        }
    }
}
