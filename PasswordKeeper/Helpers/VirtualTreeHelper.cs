using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows;
using System.Windows.Media;

namespace PasswordKeeper
{
    class VirtualTreeHelper
    {
        public static T FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
        {
            if (obj == null)
            {
                return null;
            }
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);

                if (child is T)
                {
                    return (T)child;
                }
                else
                {
                    child = FindVisualChild<T>(child);
                    if (child != null)
                    {
                        return (T)child;
                    }
                }
            }
            return null;
        }

        public static T FindVisualAncestor<T>(DependencyObject obj) where T : DependencyObject
        {
            DependencyObject ancestor = VisualTreeHelper.GetParent(obj);
            if (ancestor != null)
            {
                if (ancestor is T)
                {
                    return (T)ancestor;
                }
                else
                {
                    ancestor = FindVisualAncestor<T>(ancestor);
                    if (ancestor != null)
                    {
                        return (T)ancestor;
                    }
                }
            }
            return null;
        }
    }
}
