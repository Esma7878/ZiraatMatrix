using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ZiraatProje.UI.Helpers
{
    public static class MouseWheelScrollHelper
    {
        private const int WM_MOUSEHWHEEL = 0x020E;

        public static IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WM_MOUSEHWHEEL)
            {
                // In WM_MOUSEHWHEEL, high word of wParam is signed delta.
                int rawDelta = (short)((wParam.ToInt64() >> 16) & 0xFFFF);
                if (rawDelta != 0)
                {
                    // Find the UI element currently under the mouse cursor
                    var hitElement = Mouse.DirectlyOver as DependencyObject;
                    if (hitElement != null)
                    {
                        ScrollViewer? sv = hitElement as ScrollViewer ?? FindParentScrollViewer(hitElement);
                        if (sv != null && sv.ScrollableWidth > 0)
                        {
                            // Positive rawDelta is tilt right / swipe right, negative is left.
                            double scrollAmount = rawDelta * 0.5;
                            sv.ScrollToHorizontalOffset(sv.HorizontalOffset + scrollAmount);
                            handled = true;
                        }
                    }
                }
            }
            return IntPtr.Zero;
        }

        private static ScrollViewer? FindParentScrollViewer(DependencyObject child)
        {
            DependencyObject? parent = VisualTreeHelper.GetParent(child);
            while (parent != null)
            {
                if (parent is ScrollViewer sv)
                    return sv;
                parent = VisualTreeHelper.GetParent(parent);
            }
            return null;
        }

        public static readonly DependencyProperty EnableMouseWheelScrollProperty =
            DependencyProperty.RegisterAttached(
                "EnableMouseWheelScroll",
                typeof(bool),
                typeof(MouseWheelScrollHelper),
                new PropertyMetadata(false, OnEnableMouseWheelScrollChanged));

        public static bool GetEnableMouseWheelScroll(DependencyObject obj)
        {
            return (bool)obj.GetValue(EnableMouseWheelScrollProperty);
        }

        public static void SetEnableMouseWheelScroll(DependencyObject obj, bool value)
        {
            obj.SetValue(EnableMouseWheelScrollProperty, value);
        }

        private static void OnEnableMouseWheelScrollChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UIElement element)
            {
                if ((bool)e.NewValue)
                {
                    element.PreviewMouseWheel += Element_PreviewMouseWheel;
                }
                else
                {
                    element.PreviewMouseWheel -= Element_PreviewMouseWheel;
                }
            }
        }

        private static void Element_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (sender is DependencyObject current)
            {
                // Find the nearest ScrollViewer in the visual tree
                ScrollViewer? targetScrollViewer = current as ScrollViewer ?? FindParentScrollViewer(current);

                if (targetScrollViewer != null)
                {
                    // Check if Shift key is held down (standard shortcut for horizontal scroll)
                    bool isShiftPressed = (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift;

                    if (isShiftPressed)
                    {
                        if (targetScrollViewer.ScrollableWidth > 0)
                        {
                            targetScrollViewer.ScrollToHorizontalOffset(targetScrollViewer.HorizontalOffset - (e.Delta * 0.8));
                            e.Handled = true;
                        }
                    }
                    else
                    {
                        // Vertical scroll logic: Scroll inner target if possible, otherwise pass to parent ScrollViewer
                        double currentOffset = targetScrollViewer.VerticalOffset;
                        double maxOffset = targetScrollViewer.ScrollableHeight;

                        bool canScrollTarget = maxOffset > 0 &&
                            ((e.Delta < 0 && currentOffset < maxOffset) || (e.Delta > 0 && currentOffset > 0));

                        if (canScrollTarget)
                        {
                            targetScrollViewer.ScrollToVerticalOffset(currentOffset - (e.Delta * 0.8));
                            e.Handled = true;
                        }
                        else
                        {
                            // Outer parent ScrollViewer (e.g. Tab's main ScrollViewer)
                            ScrollViewer? parentSv = FindParentScrollViewer(targetScrollViewer);
                            if (parentSv != null && parentSv.ScrollableHeight > 0)
                            {
                                parentSv.ScrollToVerticalOffset(parentSv.VerticalOffset - (e.Delta * 0.8));
                                e.Handled = true;
                            }
                            else if (targetScrollViewer.ScrollableWidth > 0)
                            {
                                targetScrollViewer.ScrollToHorizontalOffset(targetScrollViewer.HorizontalOffset - (e.Delta * 0.8));
                                e.Handled = true;
                            }
                        }
                    }
                }
            }
        }
    }
}
