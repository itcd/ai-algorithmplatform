using System;
using System.Collections.Generic;
using System.Windows.Data;
using Perspective.Core;
using System.Windows;
using Perspective.Wpf;

namespace PerspectiveDemo.Converters
{
    [ValueConversion(typeof(string), typeof(double))]
    public class ScreenSizeToScaleFactorConverter : IValueConverter
    {
        #region IValueConverter Membres

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double scaleFactor = 0.0;
            if (!String.IsNullOrEmpty((string)value))
            {
                double diagonalScreenSize = System.Convert.ToDouble(value, culture);
                Point scale = DipHelper.GetScreenIndependentScaleFactor(diagonalScreenSize);
                Point physicalDpi = DipHelper.GetPhysicalDpi(diagonalScreenSize);
                string sParameter = (string)parameter;
                if (sParameter == "ScaleY")
                {
                    scaleFactor = scale.Y;
                }
                else if (sParameter == "ScaleX")
                {
                    scaleFactor = scale.X;
                }
                else if (sParameter == "PhysicalDpi")
                {
                    scaleFactor = physicalDpi.X;
                }
            }
            return scaleFactor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
