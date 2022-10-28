using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Common.Extensions.WPF.AttachedDependencyProperties
{
    public class FrameAttach
    {
        /// <summary>
        /// 是否启用Frame导航
        /// </summary>
        public static readonly DependencyProperty IsEnabledFrameMapProperty;
        public static bool GetIsEnabledFrameMap(DependencyObject element) => (bool)element.GetValue(IsEnabledFrameMapProperty);
        public static void SetIsEnabledFrameMap(DependencyObject element, bool value) => element.SetCurrentValue(IsEnabledFrameMapProperty, value);

        /// <summary>
        /// 导航服务类关键字
        /// </summary>
        public static readonly DependencyProperty NavigationServiceKeyProperty;
        public static string GetNavigationServiceKey(DependencyObject element) => (string)element.GetValue(NavigationServiceKeyProperty);
        public static void SetNavigationServiceKey(DependencyObject element, string value) => element.SetCurrentValue(NavigationServiceKeyProperty, value);

        /// <summary>
        /// 禁用Frame的Backspace响应
        /// </summary>
        public static readonly DependencyProperty DisableBackspaceProperty;
        public static bool GetDisableBackspace(DependencyObject element) => (bool)element.GetValue(DisableBackspaceProperty);
        public static void SetDisableBackspace(DependencyObject element, bool value) => element.SetCurrentValue(DisableBackspaceProperty, value);


        private static ConcurrentDictionary<string, Frame> _frames { get; } = new ConcurrentDictionary<string, Frame>();

        static FrameAttach()
        {
            IsEnabledFrameMapProperty = DependencyProperty.RegisterAttached("IsEnabledFrameMap", typeof(bool), typeof(Frame), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits, IsEnabledNavigationServicePropertyChanged));
            NavigationServiceKeyProperty = DependencyProperty.RegisterAttached("NavigationServiceKey", typeof(string), typeof(System.Windows.Controls.Frame), new FrameworkPropertyMetadata(Guid.NewGuid().ToString(), FrameworkPropertyMetadataOptions.Inherits));
            DisableBackspaceProperty = DependencyProperty.RegisterAttached("DisableBackspace", typeof(bool), typeof(Frame), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits, DisableBackspacePropertyChanged));
        }

        private static void IsEnabledNavigationServicePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Frame frame)
            {
                string key = GetNavigationServiceKey(d);
                _frames.AddOrUpdate(key, frame, (key, value) => frame);
                frame.Unloaded += delegate (object sender, RoutedEventArgs e)
                {
                    _frames.Remove(key, out _);
                };
            }
        }

        private static void DisableBackspacePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Frame frame)
            {
                bool isDisableBackspace = GetDisableBackspace(d);
                frame.JournalOwnership = System.Windows.Navigation.JournalOwnership.OwnsJournal;
                frame.Navigated -= Frame_Navigated;
                if (isDisableBackspace)
                {
                    frame.Navigated += Frame_Navigated;
                }
            }
        }

        private static void Frame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            if (sender is Frame frame)
            {
                frame.RemoveBackEntry();
            }
        }

        public static Frame? GetDefaultFrame()
        {
            return _frames.Values.FirstOrDefault();
        }

        public static Frame? GetFrameByKey(string key)
        {
            return _frames.TryGetValue(key, out Frame? frame) ? frame : null;
        }
    }
}
