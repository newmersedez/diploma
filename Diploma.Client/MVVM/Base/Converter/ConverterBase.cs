using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Diploma.Client.Core.MVVM.Converter
{
    /// <summary>
    /// Базовый класс унарного конвертера
    /// </summary>
    /// <typeparam name="TConverter"></typeparam>
    public abstract class ConverterBase<TConverter> : MarkupExtension, IValueConverter 
        where TConverter: class, new()
    {
        private static TConverter _valueToProvide;

        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _valueToProvide ??= new TConverter();
        }
    }
}