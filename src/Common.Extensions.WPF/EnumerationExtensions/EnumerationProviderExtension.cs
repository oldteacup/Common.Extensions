using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace Common.Extensions.WPF.EnumerationExtensions
{
    public class EnumerationProviderExtension : MarkupExtension
    {

        private Type _enumType;

        public Type EnumType
        {
            get { return _enumType; }
            private set
            {
                if (_enumType == value)
                    return;

                var enumType = Nullable.GetUnderlyingType(value) ?? value;

                if (enumType.IsEnum == false)
                    throw new ArgumentException("Type must be an Enum.");

                _enumType = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enumType"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <remarks>
        /// {Binding Source = { local:EnumerationProvider { x:Type local:Enum } }}
        /// </remarks>
        public EnumerationProviderExtension(Type enumType)
        {
            if (enumType == null)
                throw new ArgumentNullException("enumType");

            EnumType = enumType;
        }


        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var enums = Enum.GetValues(EnumType);

            List<object> results = new List<object>();
            if (enums != null)
            {
                foreach (object item in enums)
                {
                    EnumerationAttribute attribute = GetAttribute(item);
                    if (attribute.Visible)
                    {
                        results.Add(new
                        {
                            Name = attribute?.Name ?? item?.ToString(),
                            Value = attribute?.Value ?? item,
                            Source = attribute
                        });
                    }
                }
            }

            return results.ToArray();
        }

        private EnumerationAttribute GetAttribute(object @enum)
        {
            var attribute = EnumType.GetField(@enum.ToString())
              .GetCustomAttributes(typeof(EnumerationAttribute), false).FirstOrDefault() as EnumerationAttribute;


            return attribute;
        }
    }

    public class EnumerationAttribute : Attribute
    {
        public string Name { get; set; }

        public object Value { get; set; }

        public bool Visible { get; set; }

        public EnumerationAttribute(string name, object value = null, bool visible = true)
        {
            Name = name;
            Value = value;
            Visible = visible;
        }
    }
}
