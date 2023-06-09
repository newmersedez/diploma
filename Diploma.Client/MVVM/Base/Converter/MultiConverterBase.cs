using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Diploma.Client.Core.MVVM.Converter
{
    /// <summary>
    /// Базовый класс бинарного конвертера
    /// </summary>
    /// <typeparam name="TMultiConverter"></typeparam>
    public abstract class MultiConverterBase<TMultiConverter> : MarkupExtension, IMultiValueConverter
        where TMultiConverter: class, new()
    {
        private static TMultiConverter _valueToProvide;

        public abstract object Convert(object[] values, Type targetType, object parameter, CultureInfo culture);

        public virtual object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _valueToProvide ??= new TMultiConverter();
        }
    }
}