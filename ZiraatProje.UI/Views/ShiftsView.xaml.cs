using System.Windows.Controls;
using System.Windows.Input;

namespace ZiraatProje.UI.Views
{
    public partial class ShiftsView : UserControl
    {
        public ShiftsView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// "Bitti" butonuna tıklandığında mouse olayının DataGrid satır seçimine ulaşmasını engeller.
        /// Bu sayede form yanlışlıkla açılmaz; sadece komut tetiklenir.
        /// </summary>
        private void FinishButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true; // DataGrid'in satır seçim olayını engelle
            if (sender is System.Windows.Controls.Button btn
                && btn.Command?.CanExecute(btn.CommandParameter) == true)
            {
                btn.Command.Execute(btn.CommandParameter);
            }
        }

        private void BulkScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (sender is ScrollViewer scv)
            {
                scv.ScrollToVerticalOffset(scv.VerticalOffset - (e.Delta / 2.0));
                e.Handled = true;
            }
        }
    }
}
