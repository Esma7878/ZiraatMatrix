using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
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
            try
            {
                var hwndSource = PresentationSource.FromVisual(this) as HwndSource;
                hwndSource?.AddHook(MouseWheelScrollHelper.HwndHook);
            }
            catch { }

            try
            {
                Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Loaded, new Action(() =>
                {
                    MainScrollViewer?.ScrollToTop();
                }));
            }
            catch { }
        }

        private void DataGrid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
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
                            if (Keyboard.FocusedElement is TextBox focusedTb)
                            {
                                BindingOperations.GetBindingExpression(focusedTb, TextBox.TextProperty)?.UpdateSource();
                                Keyboard.ClearFocus();
                            }
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
            catch { }
        }

        private void DataGrid_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (e.OriginalSource is DataGridCell cell)
                {
                    var tb = FindVisualChild<TextBox>(cell);
                    if (tb != null && tb.Visibility == Visibility.Visible && tb.IsEnabled && !tb.IsKeyboardFocused)
                    {
                        tb.Focus();
                        tb.SelectAll();
                    }
                }
            }
            catch { }
        }

        private void DataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                bool isDigitKey = (e.Key >= Key.D0 && e.Key <= Key.D9) ||
                                  (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) ||
                                  e.Key == Key.Back || e.Key == Key.Delete;

                if (isDigitKey)
                {
                    var focused = Keyboard.FocusedElement as DependencyObject;
                    if (focused != null && !(focused is TextBox))
                    {
                        var tb = FindVisualChild<TextBox>(focused);
                        if (tb != null && tb.Visibility == Visibility.Visible && tb.IsEnabled && !tb.IsKeyboardFocused)
                        {
                            tb.Focus();
                            tb.SelectAll();
                        }
                    }
                }
            }
            catch { }
        }

        private static T? FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            if (parent == null) return null;

            try
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
                {
                    var child = VisualTreeHelper.GetChild(parent, i);
                    if (child is T typedChild)
                        return typedChild;

                    var childOfChild = FindVisualChild<T>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            catch { }
            return null;
        }

        private void CostTextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            try
            {
                if (sender is TextBox tb)
                {
                    tb.SelectAll();
                }
            }
            catch { }
        }

        private void CostTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is TextBox tb)
                {
                    tb.SelectAll();
                }
            }
            catch { }
        }

        private void CostTextBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (sender is TextBox tb && !tb.IsKeyboardFocusWithin)
                {
                    e.Handled = true;
                    tb.Focus();
                    tb.SelectAll();
                }
            }
            catch { }
        }

        private void CostTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (sender is TextBox tb)
                {
                    FocusNavigationDirection? dir = null;
                    if (e.Key == Key.Right) dir = FocusNavigationDirection.Right;
                    else if (e.Key == Key.Left) dir = FocusNavigationDirection.Left;
                    else if (e.Key == Key.Down) dir = FocusNavigationDirection.Down;
                    else if (e.Key == Key.Up) dir = FocusNavigationDirection.Up;
                    else if (e.Key == Key.Enter || e.Key == Key.Tab) dir = FocusNavigationDirection.Next;

                    if (dir.HasValue)
                    {
                        e.Handled = true;
                        tb.MoveFocus(new TraversalRequest(dir.Value));

                        var focusedElement = Keyboard.FocusedElement as DependencyObject;
                        if (focusedElement is TextBox newTb)
                        {
                            newTb.SelectAll();
                        }
                        else if (focusedElement is DataGridCell cell)
                        {
                            var childTb = FindVisualChild<TextBox>(cell);
                            if (childTb != null)
                            {
                                childTb.Focus();
                                childTb.SelectAll();
                            }
                        }
                    }
                }
            }
            catch { }
        }
        private void CostSpinBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (sender is TextBox tb)
                {
                    if (e.Key == Key.Up || e.Key == Key.Down)
                    {
                        e.Handled = true;
                        // Commit current binding first
                        BindingOperations.GetBindingExpression(tb, TextBox.TextProperty)?.UpdateSource();

                        // Change value by 1
                        if (decimal.TryParse(tb.Text, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal current)
                            || decimal.TryParse(tb.Text, out current))
                        {
                            decimal newVal = e.Key == Key.Up ? current + 1m : Math.Max(0m, current - 1m);
                            tb.Text = newVal.ToString("0.##");
                            BindingOperations.GetBindingExpression(tb, TextBox.TextProperty)?.UpdateSource();
                        }
                        tb.SelectAll();
                    }
                    else if (e.Key == Key.Right || e.Key == Key.Left ||
                             e.Key == Key.Tab || e.Key == Key.Enter)
                    {
                        FocusNavigationDirection dir = e.Key == Key.Right || e.Key == Key.Tab || e.Key == Key.Enter
                            ? FocusNavigationDirection.Next
                            : FocusNavigationDirection.Previous;
                        e.Handled = true;
                        BindingOperations.GetBindingExpression(tb, TextBox.TextProperty)?.UpdateSource();
                        tb.MoveFocus(new TraversalRequest(dir));
                        if (Keyboard.FocusedElement is TextBox newTb)
                            newTb.SelectAll();
                    }
                }
            }
            catch { }
        }

        private void SpinUp_Click(object sender, RoutedEventArgs e)
        {
            AdjustSpinValue(sender, +1m);
        }

        private void SpinDown_Click(object sender, RoutedEventArgs e)
        {
            AdjustSpinValue(sender, -1m);
        }

        private void AdjustSpinValue(object sender, decimal delta)
        {
            try
            {
                if (sender is RepeatButton btn && btn.Tag is string propName)
                {
                    var vm = DataContext as ZiraatProje.UI.ViewModels.ProjectsViewModel;
                    if (vm == null) return;

                    var prop = typeof(ZiraatProje.UI.ViewModels.ProjectsViewModel).GetProperty(propName);
                    if (prop == null || !prop.CanWrite) return;

                    var currentVal = (decimal)(prop.GetValue(vm) ?? 0m);
                    decimal newVal = Math.Max(0m, currentVal + delta);
                    prop.SetValue(vm, newVal);
                }
            }
            catch { }
        }
    }
}
