using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using System.Reflection;

namespace TestTask
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void OpenDir_Click(object sender, RoutedEventArgs e)
        {
            string dirname = InputDir.Text;

            if (Directory.Exists(dirname))
            {
                Result.Text = "";
                string[] files = Directory.GetFiles(dirname);
                var file = files.Where(t => t.EndsWith(".dll"));
                if (!files.Any())
                    Result.Text = "Файлы DLL не найдены, попробуйте еще раз";
                foreach (string f in file)
                {
                    Assembly asm = Assembly.LoadFrom(assemblyFile: f);
                    Type[] typelist = asm.GetTypes();
                    foreach (Type tl in typelist)
                    {
                        Result.Text += tl.Name + '\n';
                        MethodInfo[] mi = tl.GetMethods(BindingFlags.Instance
                                   | BindingFlags.Public
                                   | BindingFlags.Static
                                   | BindingFlags.NonPublic
                                   | BindingFlags.DeclaredOnly);
                        var mitl = mi.Where(m => m.IsPublic | m.IsFamily);
                        foreach (var m in mitl)
                            Result.Text += "- " + m.Name + '\n';
                    }
                }
            }
            else Result.Text = "Папка не найдена, попробуйте еще раз";
        }
    }
}
