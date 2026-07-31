using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using ZiraatProje.UI.Helpers;

namespace ZiraatProje.UI.Views
{
    public partial class ProjectsView : UserControl
    {
        public ProjectsView()
        {
            InitializeComponent();
            Loaded += ProjectsView_Loaded;
        }

        private void ProjectsView_Loaded(object sender, RoutedEventArgs e)
        {
            var hwndSource = PresentationSource.FromVisual(this) as HwndSource;
            hwndSource?.AddHook(MouseWheelScrollHelper.HwndHook);
        }

        private void DataGrid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGrid grid)
            {
                var dependencyObject = e.OriginalSource as DependencyObject;

                // Allow DatePicker, TextBox, CheckBox, ComboBox, Calendar to handle their own mouse events
                var current = dependencyObject;
                while (current != null)
                {
                    if (current is DatePicker || current is TextBox || current is CheckBox || current is ComboBox || current is System.Windows.Controls.Calendar)
                    {
                        return;
                    }
                    if (current is DataGridRow)
                    {
                        break;
                    }
                    current = VisualTreeHelper.GetParent(current);
                }

                Button? clickedButton = null;
                dependencyObject = e.OriginalSource as DependencyObject;

                while (dependencyObject != null && !(dependencyObject is DataGridRow))
                {
                    if (dependencyObject is Button btn)
                    {
                        clickedButton = btn;
                    }
                    dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
                }

                if (clickedButton != null)
                {
                    if (clickedButton.Command != null && clickedButton.Command.CanExecute(clickedButton.CommandParameter))
                    {
                        clickedButton.Command.Execute(clickedButton.CommandParameter);
                        e.Handled = true;
                    }
                    return;
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

        private void CostTextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (sender is TextBox tb)
            {
                tb.Dispatcher.BeginInvoke(new Action(() =>
                {
                    tb.Focus();
                    Keyboard.Focus(tb);
                    tb.SelectAll();
                }), System.Windows.Threading.DispatcherPriority.Input);
            }
        }

        private void CostTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb)
            {
                tb.Dispatcher.BeginInvoke(new Action(() =>
                {
                    tb.Focus();
                    Keyboard.Focus(tb);
                    tb.SelectAll();
                }), System.Windows.Threading.DispatcherPriority.Input);
            }
        }

        private void CostTextBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBox tb && !tb.IsKeyboardFocusWithin)
            {
                e.Handled = true;
                tb.Focus();
                Keyboard.Focus(tb);
                tb.SelectAll();
            }
        }

        private void CostTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (sender is TextBox tb)
            {
                FocusNavigationDirection? dir = null;
                if (e.Key == Key.Right) dir = FocusNavigationDirection.Right;
                else if (e.Key == Key.Left) dir = FocusNavigationDirection.Left;
                else if (e.Key == Key.Down) dir = FocusNavigationDirection.Down;
                else if (e.Key == Key.Up) dir = FocusNavigationDirection.Up;
                else if (e.Key == Key.Enter) dir = FocusNavigationDirection.Next;

                if (dir.HasValue)
                {
                    e.Handled = true;
                    tb.MoveFocus(new TraversalRequest(dir.Value));

                    tb.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        if (Keyboard.FocusedElement is TextBox newTb)
                        {
                            newTb.Focus();
                            Keyboard.Focus(newTb);
                            newTb.SelectAll();
                        }
                    }), System.Windows.Threading.DispatcherPriority.Input);
                }
            }
        }
    }
}
