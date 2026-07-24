using System;
using System.Windows;
using System.Windows.Threading;

namespace ZiraatProje.UI
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            DispatcherUnhandledException += App_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show($"Uygulama Hatası (Dispatcher):\n\n{e.Exception.Message}\n\nDetay:\n{e.Exception.InnerException?.Message}\n\nStack:\n{e.Exception.StackTrace}",
                "Hata Yakalandı", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
            {
                MessageBox.Show($"Uygulama Hatası (Domain):\n\n{ex.Message}\n\nDetay:\n{ex.InnerException?.Message}\n\nStack:\n{ex.StackTrace}",
                    "Kritik Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
