using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Common.Extensions.WPF.AttachedDependencyProperties
{
    public class FrameAttach
    {
        public static readonly DependencyProperty IsEnabledFrameMapProperty;
        public static bool GetIsEnabledFrameMap(DependencyObject element) => (bool)element.GetValue(IsEnabledFrameMapProperty);
        public static void SetIsEnabledFrameMap(DependencyObject element, bool value) => element.SetCurrentValue(IsEnabledFrameMapProperty, value);


        public static readonly DependencyProperty NavigationServiceKeyProperty;
        public static string GetNavigationServiceKey(DependencyObject element) => (string)element.GetValue(NavigationServiceKeyProperty);
        public static void SetNavigationServiceKey(DependencyObject element, string value) => element.SetCurrentValue(NavigationServiceKeyProperty, value);

        private static ConcurrentDictionary<string, Frame> _frames { get; } = new ConcurrentDictionary<string, Frame>();

        static FrameAttach()
        {
            IsEnabledFrameMapProperty = DependencyProperty.RegisterAttached("IsEnabledFrameMap", typeof(bool), typeof(Frame), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits, IsEnabledNavigationServicePropertyChanged));
            NavigationServiceKeyProperty = DependencyProperty.RegisterAttached("NavigationServiceKey", typeof(string), typeof(System.Windows.Controls.Frame), new FrameworkPropertyMetadata(Guid.NewGuid().ToString(), FrameworkPropertyMetadataOptions.Inherits));
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
