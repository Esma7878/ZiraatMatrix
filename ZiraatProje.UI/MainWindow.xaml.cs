using System;
using System.Windows;
using ZiraatProje.DataAccess;
using ZiraatProje.UI.ViewModels;

namespace ZiraatProje.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            // Initialize Database and Seed Mock Data on App Launch
            try
            {
                using (var context = new AppDbContext())
                {
                    DbInitializer.Initialize(context);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Veritabanı bağlantı hatası: {ex.Message}\n\nLocalDB çalışıyor mu veya yüklü mü kontrol edin.", "Bağlantı Hatası", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            InitializeComponent();

            // Set DataContext to navigation manager viewmodel
            var vm = new MainWindowViewModel();
            vm.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(MainWindowViewModel.IsLoggedIn) && !vm.IsLoggedIn)
                {
                    if (TxtPassword != null) TxtPassword.Password = string.Empty;
                    if (TxtNewPassword != null) TxtNewPassword.Password = string.Empty;
                    if (TxtNewPasswordConfirm != null) TxtNewPasswordConfirm.Password = string.Empty;
                }
            };
            DataContext = vm;
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            if (PresentationSource.FromVisual(this) is System.Windows.Interop.HwndSource source)
            {
                source.AddHook(Helpers.MouseWheelScrollHelper.HwndHook);
            }
        }

        private void TxtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainWindowViewModel vm && sender is System.Windows.Controls.PasswordBox pb)
            {
                vm.LoginPassword = pb.Password;
            }
        }

        private void TxtNewPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainWindowViewModel vm && sender is System.Windows.Controls.PasswordBox pb)
            {
                vm.NewPassword = pb.Password;
            }
        }

        private void TxtNewPasswordConfirm_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainWindowViewModel vm && sender is System.Windows.Controls.PasswordBox pb)
            {
                vm.NewPasswordConfirm = pb.Password;
            }
        }

        private void TxtPassword_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is System.Windows.Controls.PasswordBox pb && pb.IsVisible)
            {
                if (DataContext is MainWindowViewModel vm)
                {
                    if (pb.Password != vm.LoginPassword)
                    {
                        pb.Password = vm.LoginPassword ?? string.Empty;
                    }
                }
            }
        }

        private void TxtNewPassword_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is System.Windows.Controls.PasswordBox pb && pb.IsVisible)
            {
                if (DataContext is MainWindowViewModel vm)
                {
                    if (pb.Password != vm.NewPassword)
                    {
                        pb.Password = vm.NewPassword ?? string.Empty;
                    }
                }
            }
        }

        private void TxtNewPasswordConfirm_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is System.Windows.Controls.PasswordBox pb && pb.IsVisible)
            {
                if (DataContext is MainWindowViewModel vm)
                {
                    if (pb.Password != vm.NewPasswordConfirm)
                    {
                        pb.Password = vm.NewPasswordConfirm ?? string.Empty;
                    }
                }
            }
        }

        private void Input_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                if (DataContext is MainWindowViewModel vm)
                {
                    if (vm.IsResetPasswordMode)
                    {
                        if (vm.SubmitResetPasswordCommand.CanExecute(null))
                        {
                            vm.SubmitResetPasswordCommand.Execute(null);
                        }
                    }
                    else
                    {
                        if (vm.LoginCommand.CanExecute(null))
                        {
                            vm.LoginCommand.Execute(null);
                        }
                    }
                }
            }
        }
    }
}