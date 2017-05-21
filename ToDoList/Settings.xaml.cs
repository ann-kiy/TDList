using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ToDoList
{
    /// <summary>
    /// Логика взаимодействия для Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        MainWindow mw = new MainWindow();
        IniFile ini = new IniFile("../../config.ini");

        public Settings()
        {
            InitializeComponent();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            sounds_CB.IsChecked = mw.config.sounds;
            popup_time.Text = mw.config.time.ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ini.IniWriteValue("main", "sounds", sounds_CB.IsChecked.ToString());
            ini.IniWriteValue("main", "time", popup_time.Text);
            MessageBox.Show("Изменения вступят в силу после перезапуска программы.");
            this.Close();
        }
    }
}
