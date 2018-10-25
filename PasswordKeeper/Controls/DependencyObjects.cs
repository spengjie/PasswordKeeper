using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PasswordKeeper
{
    public class PathDataDependencyClass : DependencyObject
    {
        #region IsCheckedOnDataProperty

        public static readonly DependencyProperty PathProperty;

        public static void SetPath(DependencyObject DepObject, Path value)
        {
            DepObject.SetValue(PathProperty, value);
        }

        public static Geometry GetPath(DependencyObject DepObject)
        {
            return (Geometry)DepObject.GetValue(PathProperty);
        }

        #endregion
        #region IsCheckedOnSizeProperty

        public static readonly DependencyProperty SizeForPathProperty;

        public static void SetSizeForPath(DependencyObject DepObject, double value)
        {
            DepObject.SetValue(PathProperty, value);
        }

        public static double GetSizeForPath(DependencyObject DepObject)
        {
            return (double)DepObject.GetValue(SizeForPathProperty);
        }

        #endregion

        static PathDataDependencyClass()
        {
            PropertyMetadata pathDataPropertyMetadata = new PropertyMetadata();
            PathProperty = DependencyProperty.RegisterAttached("Path", typeof(Geometry), typeof(PathDataDependencyClass), pathDataPropertyMetadata);
            PropertyMetadata pathSizePropertyMetadata = new PropertyMetadata(20.0);
            SizeForPathProperty = DependencyProperty.RegisterAttached("SizeForPath", typeof(double), typeof(PathDataDependencyClass), pathSizePropertyMetadata);
        }
    }

    public class AlwaysShowToolTipDependencyClass : DependencyObject
    {
        #region IsCheckedOnDataProperty

        public static readonly DependencyProperty AlwaysShowToolTipProperty;

        public static void SetAlwaysShowToolTip(DependencyObject DepObject, bool value)
        {
            DepObject.SetValue(AlwaysShowToolTipProperty, value);
        }

        public static bool GetAlwaysShowToolTip(DependencyObject DepObject)
        {
            return (bool)DepObject.GetValue(AlwaysShowToolTipProperty);
        }

        #endregion

        static AlwaysShowToolTipDependencyClass()
        {
            PropertyMetadata MyPropertyMetadata = new PropertyMetadata(false);
            AlwaysShowToolTipProperty = DependencyProperty.RegisterAttached("AlwaysShowToolTip", typeof(bool), typeof(AlwaysShowToolTipDependencyClass), MyPropertyMetadata);
        }
    }
}
