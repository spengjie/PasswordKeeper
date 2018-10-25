using System;

using System.Windows.Data;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace PasswordKeeper
{
    class TrueToVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool bValue = (bool)value;
            if (bValue)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    class FalseToVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool bValue = (bool)value;
            if (bValue)
            {
                return Visibility.Collapsed;
            }
            else
            {
                return Visibility.Visible;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    class EmptyStringToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string sValue = value.ToString();
            if (sValue == "")
            {
                return Visibility.Collapsed;
            }
            else
            {
                return Visibility.Visible;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    class LightColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Color color = (Color) value;
            Color resultColor = new Color()
            {
                R = color.R,
                G = color.G,
                B = color.B,
                A = 168
            };
            return resultColor;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Color color = (Color)value;
            Color resultColor = new Color()
            {
                R = color.R,
                G = color.G,
                B = color.B,
                A = 255
            };
            return resultColor;
        }
    }

    public class ImageToColorConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            BitmapImage img = (BitmapImage)value;
            if (img.UriSource == new Uri(@"pack://application:,,,/PasswordKeeper;component/Resources/DefaultFavicon.ico", UriKind.RelativeOrAbsolute))
            {
                return parameter;
            }
            Dictionary<Color, int> dic = new Dictionary<Color, int>();
            int h = img.PixelHeight;
            int w = img.PixelWidth;

            int nStride = (img.PixelWidth * img.Format.BitsPerPixel + 7) / 8;
            byte[] pixelByteArray = new byte[img.PixelHeight * nStride];
            byte[] pixelByteArray2 = new byte[img.PixelHeight * nStride];
            img.CopyPixels(pixelByteArray, nStride, 0);
            for (int i = 0; i < h * w; i++)
            {
                byte blue = pixelByteArray[i * 4];
                byte green = pixelByteArray[i * 4 + 1];
                byte red = pixelByteArray[i * 4 + 2];
                byte alpha = pixelByteArray[i * 4 + 3];
                if (alpha < 224)
                {
                    continue;
                }
                else if (blue + green + red == 0)
                {
                    continue;
                }
                else if (Math.Abs(red - green) <= 10 && Math.Abs(red - blue) <= 10 && red > 128)
                {
                    continue;
                }
                else if (red >= 230 && green >= 230 && blue >= 230)
                {
                    continue;
                }
                Color color = Color.FromArgb(alpha, red, green, blue);
                if (dic.ContainsKey(color))
                {
                    dic[color]++;
                }
                else
                {
                    dic[color] = 1;
                }
            }
            Color MostColor = new Color();
            int MostColorCount = 0;
            foreach (Color color in dic.Keys)
            {
                if (dic[color] > MostColorCount)
                {
                    MostColorCount = dic[color];
                    MostColor = color;
                }
            }
            if (MostColorCount == 0)
            {
                return parameter;
            }
            return MostColor.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    public class CenterToolTipConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values.FirstOrDefault(v => v == DependencyProperty.UnsetValue) != null)
            {
                return double.NaN;
            }
            double placementTargetWidth = (double)values[0];
            double toolTipWidth = (double)values[1];
            return (placementTargetWidth / 2.0) - (toolTipWidth / 2.0);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    public class TrimmedTextBlockVisibilityConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return Visibility.Visible;
            }

            if (value.GetType() == typeof(CopyableTextBlock))
            {
                CopyableTextBlock copyableTextBlock = (CopyableTextBlock)value;
                if ((bool)copyableTextBlock.GetValue(AlwaysShowToolTipDependencyClass.AlwaysShowToolTipProperty))
                {
                    return Visibility.Visible;
                }
                if (copyableTextBlock.TextTrimming == TextTrimming.None)
                {
                    return Visibility.Collapsed;
                }
                copyableTextBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                if (copyableTextBlock.ActualWidth < copyableTextBlock.DesiredSize.Width)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
            else if (value.GetType() == typeof(TextBlock))
            {
                TextBlock textBlock = (TextBlock)value;
                if ((bool)textBlock.GetValue(AlwaysShowToolTipDependencyClass.AlwaysShowToolTipProperty))
                {
                    return Visibility.Visible;
                }
                if (textBlock.TextTrimming == TextTrimming.None)
                {
                    return Visibility.Collapsed;
                }
                textBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                if (textBlock.ActualWidth < textBlock.DesiredSize.Width)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    public class TabItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ItemTemplate { get; set; }
        public DataTemplate NewButtonTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == CollectionView.NewItemPlaceholder)
            {
                return NewButtonTemplate;
            }
            else
            {
                return ItemTemplate;
            }
        }
    }
}
