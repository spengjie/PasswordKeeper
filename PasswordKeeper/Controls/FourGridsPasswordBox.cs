using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PasswordKeeper
{
    /// <summary>
    /// 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
    ///
    /// 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:PasswordKeeper.Classes"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:PasswordKeeper.Classes;assembly=PasswordKeeper.Classes"
    ///
    /// 您还需要添加一个从 XAML 文件所在的项目到此项目的项目引用，
    /// 并重新生成以避免编译错误: 
    ///
    ///     在解决方案资源管理器中右击目标项目，然后依次单击
    ///     “添加引用”->“项目”->[浏览查找并选择此项目]
    ///
    ///
    /// 步骤 2)
    /// 继续操作并在 XAML 文件中使用控件。
    ///
    ///     <MyNamespace:FourGridsPasswordKeeper/>
    ///
    /// </summary>
    [TemplatePart(Name = ElementPassword1, Type = typeof(PasswordBox))]
    [TemplatePart(Name = ElementPassword2, Type = typeof(PasswordBox))]
    [TemplatePart(Name = ElementPassword3, Type = typeof(PasswordBox))]
    [TemplatePart(Name = ElementPassword4, Type = typeof(PasswordBox))]
    public class FourGridsPasswordBox : Control
    {
        #region Constants
        private const string ElementPassword1 = "PART_Password1";
        private const string ElementPassword2 = "PART_Password2";
        private const string ElementPassword3 = "PART_Password3";
        private const string ElementPassword4 = "PART_Password4";

        #endregion

        #region Data
        private PasswordBox PasswordBoxPassword1;
        private PasswordBox PasswordBoxPassword2;
        private PasswordBox PasswordBoxPassword3;
        private PasswordBox PasswordBoxPassword4;
        #endregion Data

        #region Public Methods
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();


            PasswordBoxPassword1 = GetTemplateChild(ElementPassword1) as PasswordBox;
            PasswordBoxPassword1.KeyUp += PasswordBoxPassword1_KeyUp;
            //PasswordBoxPassword1.Margin = new Thickness { Left = PasswordBoxSpace };

            PasswordBoxPassword2 = GetTemplateChild(ElementPassword2) as PasswordBox;
            PasswordBoxPassword2.KeyUp += PasswordBoxPassword2_KeyUp;
            PasswordBoxPassword2.Margin = new Thickness { Left = PasswordBoxSpace };

            PasswordBoxPassword3 = GetTemplateChild(ElementPassword3) as PasswordBox;
            PasswordBoxPassword3.KeyUp += PasswordBoxPassword3_KeyUp;
            PasswordBoxPassword3.Margin = new Thickness { Left = PasswordBoxSpace };

            PasswordBoxPassword4 = GetTemplateChild(ElementPassword4) as PasswordBox;
            PasswordBoxPassword4.KeyUp += PasswordBoxPassword4_KeyUp;
            PasswordBoxPassword4.Margin = new Thickness { Left = PasswordBoxSpace };

            this.GotFocus += This_GotFocus;

            if (IsFocused)
            {
                SetFocus();
            }
        }
        #endregion Public Methods

        #region Ctor
        static FourGridsPasswordBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FourGridsPasswordBox), new FrameworkPropertyMetadata(typeof(FourGridsPasswordBox)));
        }

        public FourGridsPasswordBox()
        {
            //Password = new PasswordBase("ABCD");
        }
        #endregion Ctor

        #region Public Properties
        public static DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(FourGridsPasswordBox));
        public static DependencyProperty PasswordBoxSpaceProperty = DependencyProperty.Register("PasswordBoxSpace", typeof(double), typeof(FourGridsPasswordBox));
        public static DependencyProperty PasswordBoxStyleProperty = DependencyProperty.Register("PasswordBoxStyle", typeof(Style), typeof(FourGridsPasswordBox));

        public ICommand Command
        {
            get
            {
                return (ICommand)GetValue(CommandProperty);
            }

            set
            {
                SetValue(CommandProperty, value);
            }
        }
        public double PasswordBoxSpace
        {
            get
            {
                return (double)GetValue(PasswordBoxSpaceProperty);
            }

            set
            {
                SetValue(PasswordBoxSpaceProperty, value);
            }
        }
        public Style PasswordBoxStyle
        {
            get
            {
                return (Style)GetValue(PasswordBoxStyleProperty);
            }

            set
            {
                SetValue(PasswordBoxStyleProperty, value);
            }
        }
        public string Password
        {
            get
            {
                return PasswordBoxPassword1.Password + PasswordBoxPassword2.Password + PasswordBoxPassword3.Password + PasswordBoxPassword4.Password;
            }
            set
            {
                if (value.Length == 4)
                {
                    PasswordBoxPassword1.Password = value[0].ToString();
                    PasswordBoxPassword2.Password = value[1].ToString();
                    PasswordBoxPassword3.Password = value[2].ToString();
                    PasswordBoxPassword4.Password = value[3].ToString();
                }
                else
                {
                    PasswordBoxPassword1.Password = "";
                    PasswordBoxPassword2.Password = "";
                    PasswordBoxPassword3.Password = "";
                    PasswordBoxPassword4.Password = "";
                }

            }
        }
        #endregion Public Properties

        private void PasswordBoxPassword1_KeyUp(object sender, KeyEventArgs e)
        {
            if (PasswordBoxPassword1.Password.Length == 1)
            {
                PasswordBoxPassword2.Focusable = true;
                PasswordBoxPassword2.Focus();
                PasswordBoxPassword1.Focusable = false;
            }
        }

        private void PasswordBoxPassword2_KeyUp(object sender, KeyEventArgs e)
        {
            if (PasswordBoxPassword2.Password.Length == 1)
            {
                PasswordBoxPassword3.Focusable = true;
                PasswordBoxPassword3.Focus();
                PasswordBoxPassword2.Focusable = false;
            }
            else
            {
                if (e.Key.Equals(Key.Back))
                {
                    PasswordBoxPassword1.Focusable = true;
                    PasswordBoxPassword1.Password = "";
                    PasswordBoxPassword2.Password = "";
                    PasswordBoxPassword1.Focus();
                    PasswordBoxPassword2.Focusable = false;
                }
            }
        }

        private void PasswordBoxPassword3_KeyUp(object sender, KeyEventArgs e)
        {
            if (PasswordBoxPassword3.Password.Length == 1)
            {
                PasswordBoxPassword4.Focusable = true;
                PasswordBoxPassword4.Focus();
                PasswordBoxPassword3.Focusable = false;
            }
            else
            {
                if (e.Key.Equals(Key.Back))
                {
                    PasswordBoxPassword2.Focusable = true;
                    PasswordBoxPassword2.Password = "";
                    PasswordBoxPassword3.Password = "";
                    PasswordBoxPassword2.Focus();
                    PasswordBoxPassword3.Focusable = false;
                }
            }
        }

        private void PasswordBoxPassword4_KeyUp(object sender, KeyEventArgs e)
        {
            if (PasswordBoxPassword4.Password.Length == 1)
            {
                if (Command != null)
                {
                    Command.Execute(Password);
                }
                this.Password = "";
                PasswordBoxPassword1.Focusable = true;
                PasswordBoxPassword1.Focus();
                PasswordBoxPassword4.Focusable = false;
            }
            else
            {
                if (e.Key.Equals(Key.Back))
                {
                    PasswordBoxPassword3.Focusable = true;
                    PasswordBoxPassword3.Password = "";
                    PasswordBoxPassword4.Password = "";
                    PasswordBoxPassword3.Focus();
                    PasswordBoxPassword4.Focusable = true;
                }
            }
        }

        private void SetFocus()
        {
            if (PasswordBoxPassword1.IsFocused || PasswordBoxPassword2.IsFocused || PasswordBoxPassword3.IsFocused || PasswordBoxPassword4.IsFocused)
            {
                return;
            }
            if (PasswordBoxPassword1 != null && PasswordBoxPassword1.Focusable)
            {
                PasswordBoxPassword1.Focus();
                return;
            }
            else if (PasswordBoxPassword2 != null && PasswordBoxPassword2.Focusable)
            {
                PasswordBoxPassword2.Focus();
                return;
            }
            else if (PasswordBoxPassword3 != null && PasswordBoxPassword3.Focusable)
            {
                PasswordBoxPassword3.Focus();
                return;
            }
            else if (PasswordBoxPassword4 != null && PasswordBoxPassword4.Focusable)
            {
                PasswordBoxPassword4.Focus();
                return;
            }
        }

        private void This_GotFocus(object sender, RoutedEventArgs e)
        {
            SetFocus();
        }
    }
}
