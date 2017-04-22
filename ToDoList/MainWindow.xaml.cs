using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ToDoList
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        ArrayList textBox = new ArrayList();
        int i = 0, tab = 1;
        void AddTextBox(int i, ArrayList textBox, int h, int l)
        {

            textBox.Add(new TextBox());
            ((TextBox)textBox[i]).HorizontalAlignment = HorizontalAlignment.Left;
            ((TextBox)textBox[i]).Height = 20;
            ((TextBox)textBox[i]).Width = 346;
            ((TextBox)textBox[i]).VerticalAlignment = VerticalAlignment.Top;
            ((TextBox)textBox[i]).TextWrapping = TextWrapping.Wrap;
            ((TextBox)textBox[i]).Margin = new Thickness(h, l, 0, 0);
            MyGrid.Children.Add(((TextBox)textBox[i]));

        }
        void FileWrite(string text, string date)
        {

            FileStream file = new FileStream("dataBase.txt", FileMode.Append, FileAccess.Write);
            StreamWriter writer = new StreamWriter(file);
            writer.Write(text + "/");
            writer.Write(date);

            writer.Write('\n');
            writer.Close();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FileWrite(textBox1.Text, DatePicker1.Text.ToString());

            AddTextBox(i, textBox, 1, tab);
            ((TextBox)textBox[i]).Text = textBox1.Text;
            tab += 30;
            i++;

        }
    }
}
