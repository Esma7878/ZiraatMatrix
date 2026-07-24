using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ZiraatProje.UI.Views
{
    public partial class ProjectsView : UserControl
    {
        public ProjectsView()
        {
            InitializeComponent();
            ModalGrid.IsVisibleChanged += (s, e) =>
            {
                if (ModalGrid.Visibility == Visibility.Visible)
                {
                    RootScrollViewer.ScrollToTop();
                }
            };
        }

        private void DataGrid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGrid grid)
            {
                var dependencyObject = e.OriginalSource as DependencyObject;
                while (dependencyObject != null && !(dependencyObject is DataGridRow))
                {
                    // Allow buttons and textboxes inside row details or cells to be clicked normally
                    if (dependencyObject is Button || dependencyObject is TextBox || dependencyObject is CheckBox || dependencyObject is ComboBox)
                    {
                        return;
                    }
                    dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
                }

                if (dependencyObject is DataGridRow row)
                {
                    if (row.IsSelected)
                    {
                        grid.SelectedItem = null;
                        e.Handled = true;
                    }
                }
            }
        }
    }
}
