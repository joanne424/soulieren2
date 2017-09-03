

namespace DM2.Ent.Client.Views
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;


    /// <summary>
    /// 控件公共附加属性
    /// </summary>
    public static class ControlAttachProperty
    {
        /// <summary>
        /// 附加组件模板
        /// </summary>
        public static readonly DependencyProperty AttachContentProperty = DependencyProperty.RegisterAttached(
            "AttachContent", typeof(ControlTemplate), typeof(ControlAttachProperty), new FrameworkPropertyMetadata(null));

        public static ControlTemplate GetAttachContent(DependencyObject d)
        {
            return (ControlTemplate)d.GetValue(AttachContentProperty);
        }

        public static void SetAttachContent(DependencyObject obj, ControlTemplate value)
        {
            obj.SetValue(AttachContentProperty, value);
        }

        /// <summary>
        /// 最大化窗口命令
        /// </summary>
        public static RoutedUICommand MaximizeWindowCommand { get; private set; }

        /// <summary>
        /// 最大化窗口命令绑定事件
        /// </summary>
        private static readonly CommandBinding MaximizeWindowCommandBinding;

        /// <summary>
        /// 最大化窗口
        /// </summary>
        private static void OnWindowMaximized(object sender, ExecutedRoutedEventArgs e)
        {
            var window = e.Parameter as Window;
            if (window == null)
            {
                return;
            }

            if (window.WindowState == WindowState.Normal)
            {
                window.WindowState = WindowState.Maximized;
            }
            else
            {
                window.WindowState = WindowState.Normal;
            }
        }

        public static readonly DependencyProperty IsMaximizeWindowButtonBehaviorEnabledProperty = DependencyProperty.RegisterAttached("IsMaximizeWindowButtonBehaviorEnabled"
            , typeof(bool), typeof(ControlAttachProperty), new FrameworkPropertyMetadata(false, IsMaximizeWindowButtonBehaviorEnabledChanged));

        [AttachedPropertyBrowsableForType(typeof(Button))]
        public static bool GetIsMaximizeWindowButtonBehaviorEnabled(DependencyObject d)
        {
            return (bool)d.GetValue(IsMaximizeWindowButtonBehaviorEnabledProperty);
        }

        public static void SetIsMaximizeWindowButtonBehaviorEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(IsMaximizeWindowButtonBehaviorEnabledProperty, value);
        }

        private static void IsMaximizeWindowButtonBehaviorEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var button = d as Button;
            if (e.OldValue != e.NewValue && button != null)
            {
                button.CommandBindings.Add(MaximizeWindowCommandBinding);
            }
        }


        /// <summary>
        /// 最小化窗口命令
        /// </summary>
        public static RoutedUICommand MinimizeWindowCommand { get; private set; }

        /// <summary>
        /// 最小化窗口命令绑定事件
        /// </summary>
        private static readonly CommandBinding MinimizeWindowCommandBinding;

        /// <summary>
        /// 最小化窗口
        /// </summary>
        private static void OnWindowMinimized(object sender, ExecutedRoutedEventArgs e)
        {
            var window = e.Parameter as Window;
            if (window == null)
            {
                return;
            }

            window.WindowState = WindowState.Minimized;
        }

        public static readonly DependencyProperty IsMinimizeWindowButtonBehaviorEnabledProperty = DependencyProperty.RegisterAttached("IsMinimizeWindowButtonBehaviorEnabled"
            , typeof(bool), typeof(ControlAttachProperty), new FrameworkPropertyMetadata(false, IsMinimizeWindowButtonBehaviorEnabledChanged));

        [AttachedPropertyBrowsableForType(typeof(Button))]
        public static bool GetIsMinimizeWindowButtonBehaviorEnabled(DependencyObject d)
        {
            return (bool)d.GetValue(IsMinimizeWindowButtonBehaviorEnabledProperty);
        }

        public static void SetIsMinimizeWindowButtonBehaviorEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(IsMinimizeWindowButtonBehaviorEnabledProperty, value);
        }

        private static void IsMinimizeWindowButtonBehaviorEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var button = d as Button;
            if (e.OldValue != e.NewValue && button != null)
            {
                button.CommandBindings.Add(MinimizeWindowCommandBinding);
            }
        }

        static ControlAttachProperty()
        {
            MaximizeWindowCommand = new RoutedUICommand();
            MaximizeWindowCommandBinding = new CommandBinding(MaximizeWindowCommand);
            MaximizeWindowCommandBinding.Executed += OnWindowMaximized;

            MinimizeWindowCommand = new RoutedUICommand();
            MinimizeWindowCommandBinding = new CommandBinding(MinimizeWindowCommand);
            MinimizeWindowCommandBinding.Executed += OnWindowMinimized;
        }
    }
}
