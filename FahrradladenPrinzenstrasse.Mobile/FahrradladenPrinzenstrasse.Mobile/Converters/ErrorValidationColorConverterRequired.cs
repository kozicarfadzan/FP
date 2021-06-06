using System;
using System.Globalization;
using System.Text.RegularExpressions;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using FahrradladenPrinzenstrasse.Mobile.Controls;
using FahrradladenPrinzenstrasse.Mobile.ViewModels.PotvrdaNarudzbe;
using System.Threading.Tasks;

namespace FahrradladenPrinzenstrasse.Mobile.Converters
{
    /// <summary>
    /// This class have methods to convert the Boolean values to color objects. 
    /// This is needed to validate in the Entry controls. If the validation is failed, it will return the color code of error, otherwise it will be transparent.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class ErrorValidationColorConverterRequired : IValueConverter
    {

        /// <summary>
        /// This method is used to convert the bool to color.
        /// </summary>
        /// <param name="value">Gets the value.</param>
        /// <param name="targetType">Gets the target type.</param>
        /// <param name="parameter">Gets the parameter.</param>
        /// <param name="culture">Gets the culture.</param>
        /// <returns>Returns the color.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var entry = parameter as BorderlessEntry;

            var isFocused1 = (bool)value;
            bool IsInvalidEntry = false;

            if (!(entry?.BindingContext is AddContactViewModel))
            {
                if (entry != null && (entry.StyleId == "PokrajinaEntry" || entry.StyleId == "DrzavaEntry" || entry.StyleId == "PostanskiKodEntry"))
                    IsInvalidEntry = true;
            }
            else
            {
                IsInvalidEntry = !isFocused1 && string.IsNullOrEmpty(entry.Text);
            }

            if (isFocused1)
            {
                return Color.FromHex("#959eac");
            }

            return IsInvalidEntry ? Color.FromHex("#FF4A4A") : Color.FromHex("#ced2d9");

        }

        /// <summary>
        /// This method is used to convert the color to bool.
        /// </summary>
        /// <param name="value">Gets the value.</param>
        /// <param name="targetType">Gets the target type.</param>
        /// <param name="parameter">Gets the parameter.</param>
        /// <param name="culture">Gets the culture.</param>
        /// <returns>Returns the string.</returns>        
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

    }
}