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
    [TemplatePart(Name = ElementCopyButton, Type = typeof(Button))]
    public class CopyableTextBlock : TextBox
    {
        #region Constants
        private const string ElementCopyButton = "PART_CopyButton";

        #endregion

        #region Data

        #endregion Data

        #region Public Methods
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.IsReadOnly = true;
            CopyButton = GetTemplateChild(ElementCopyButton) as Button;
            CopyButton.Click += (sender, e) => {
                Clipboard.SetDataObject(this.Text);
                //Clipboard.SetText(this.Text);
            };
        }
        #endregion Public Methods

        #region Ctor
        static CopyableTextBlock()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CopyableTextBlock), new FrameworkPropertyMetadata(typeof(CopyableTextBlock)));
        }
        #endregion Ctor

        #region Public Properties
        public static DependencyProperty CopyButtonProperty = DependencyProperty.Register("CopyButton", typeof(Button), typeof(CopyableTextBlock));
        public static DependencyProperty CopyButtonStyleProperty = DependencyProperty.Register("CopyButtonStyle", typeof(Style), typeof(CopyableTextBlock));
        public static DependencyProperty TextTrimmingProperty = DependencyProperty.Register("TextTrimming", typeof(TextTrimming), typeof(CopyableTextBlock));

        public Button CopyButton
        {
            get
            {
                return (Button)GetValue(CopyButtonProperty);
            }

            set
            {
                SetValue(CopyButtonProperty, value);
            }
        }

        public Style CopyButtonStyle
        {
            get
            {
                return (Style)GetValue(CopyButtonStyleProperty);
            }

            set
            {
                SetValue(CopyButtonStyleProperty, value);
            }
        }

        public TextTrimming TextTrimming
        {
            get
            {
                return (TextTrimming)GetValue(TextTrimmingProperty);
            }

            set
            {
                SetValue(TextTrimmingProperty, value);
            }
        }
        #endregion Public Properties
    }
}
