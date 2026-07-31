using System;
using System.IO;
using System.Windows;
using System.Windows.Threading;
using ZiraatProje.Business;

namespace ZiraatProje.UI
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            // Clean synthetic data ONCE for the user
            string flagFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wiped_synthetic_costs.txt");
            if (!File.Exists(flagFile))
            {
                try
                {
                    var services = new BusinessServices();
                    services.ClearAllSyntheticCosts();
                    File.WriteAllText(flagFile, "Wiped on " + DateTime.Now.ToString());
                }
                catch { }
            }

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
