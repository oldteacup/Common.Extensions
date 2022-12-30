using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Common.Extensions.WPF.AttachedDependencyProperties
{
    public class TextBoxAttach
    {
        /// <summary>
        /// TextBox输入正则Pattern属性
        /// </summary>
        public static readonly DependencyProperty PatternProperty;
        public static string GetPattern(DependencyObject element) => (string)element.GetValue(PatternProperty);
        public static void SetPattern(DependencyObject element, string value) => element.SetCurrentValue(PatternProperty, value);

        static TextBoxAttach()
        {

            PatternProperty = DependencyProperty.RegisterAttached("Pattern", typeof(string), typeof(TextBoxAttach), new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.Inherits, PatternPropertyChangedCallback));
        }

        private static void PatternPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox textBox)
            {
                var pattern = GetPattern(d);

                TextCompositionEventHandler eventHandler = delegate (object sender, System.Windows.Input.TextCompositionEventArgs e)
                {
                    if (!string.IsNullOrWhiteSpace(pattern))
                    {
                        e.Handled = !Regex.IsMatch(e.Text, pattern);
                    }
                };

                textBox.PreviewTextInput -= eventHandler;
                if (!string.IsNullOrWhiteSpace(pattern))
                {
                    textBox.PreviewTextInput += eventHandler;
                }
            }
        }
    }
}
