using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace PasswordKeeper
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            //PasswordKeeper.Resources.Culture = new System.Globalization.CultureInfo["zh-CN"];
            Console.WriteLine(this.Resources.MergedDictionaries.Count);
        }
    }
}
