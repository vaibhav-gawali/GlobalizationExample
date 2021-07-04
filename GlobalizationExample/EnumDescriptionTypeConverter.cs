using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WpfPlayground
{
    public class EnumDescriptionTypeConverter : EnumConverter
    {
        public EnumDescriptionTypeConverter(Type enumType) : base(enumType)
        {

        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType != typeof(string)) { return BaseConvertTo(); }
            if (value == null) { return BaseConvertTo(); }

            FieldInfo fi = value.GetType().GetField(value.ToString());
            if (fi == null) { return BaseConvertTo(); }

            DescriptionAttribute[] descriptionAttributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (descriptionAttributes == null || descriptionAttributes.Length < 1) { return value.ToString(); }
            if (string.IsNullOrWhiteSpace(descriptionAttributes[0].Description)) { return value.ToString(); }

            return descriptionAttributes[0].Description;

            object BaseConvertTo()
            {
                return base.ConvertTo(context, culture, value, destinationType);
            }
        }
    }
}
