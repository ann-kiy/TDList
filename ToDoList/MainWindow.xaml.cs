﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Collections;




namespace ToDoList
{

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow :  System.Windows.Window

    {
        System.Windows.Forms.NotifyIcon ni = new System.Windows.Forms.NotifyIcon();
        public MainWindow()
        {
            InitializeComponent();
            InitializeComponent();
            ni.Icon = new System.Drawing.Icon("50-512.ico");

        }
        ArrayList textBox = new ArrayList();
        ArrayList label = new ArrayList();
        ArrayList checkBox = new ArrayList();
        int i = 0, tab = 1;
        int n;
        string StrChe;

        public class Rek
        {
            public DateTime date;
            public string text;
        }
        List<Rek> usr = new List<Rek>();
        Rek u1 = new Rek();


        void AddTextBox(int i, ArrayList textBox, int h, int l)
        {

            textBox.Add(new TextBox());
            ((TextBox)textBox[i]).HorizontalAlignment = HorizontalAlignment.Left;
             ((TextBox)textBox[i]).Height = 25;
            ((TextBox)textBox[i]).Width = 450;
            ((TextBox)textBox[i]).VerticalAlignment = VerticalAlignment.Top;
            ((TextBox)textBox[i]).TextWrapping = TextWrapping.Wrap;
            ((TextBox)textBox[i]).Margin = new Thickness(h, l, 0, 0);
            ((TextBox)textBox[i]).VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            ((TextBox)textBox[i]).KeyUp += textBox1_KeyUp;
            ((TextBox)textBox[i]).IsEnabled = false;
            panel.Children.Add(((TextBox)textBox[i]));
        }

        

        void AddLabel(int i, ArrayList textBox, int h, int l,string date)
        {

            label.Add(new Label());
            ((Label)label[i]).HorizontalAlignment = HorizontalAlignment.Left;
            ((Label)label[i]).VerticalAlignment = VerticalAlignment.Top;
            ((Label)label[i]).Margin = new Thickness(h, l, 0, 0);
            ((Label)label[i]).Content = "——————————————— " + date + " ———————————————";
            //MyGrid.Children.Add(((TextBox)textBox[i]));
            
            panel.Children.Add(((Label)label[i]));

            
        }
        void FileWrite(string text, string date)
        {

            FileStream file = new FileStream("dataBase.txt", FileMode.Append, FileAccess.Write);
            StreamWriter writer = new StreamWriter(file);

            writer.WriteLine(text+"/"+date);
            writer.Close();

        }

        void FillArr()
        {
            FileStream file = new FileStream("dataBase.txt", FileMode.Open, FileAccess.Read);
            StreamReader read = new StreamReader(file);
            string str;

            while (!read.EndOfStream) //Цикл длиться пока не будет достигнут конец файла
            {
                str = read.ReadLine();                
                u1.date = new DateTime(int.Parse((str.Split('/')[1]).Split('.')[2].Split(' ')[0]), int.Parse((str.Split('/')[1]).Split('.')[1]), int.Parse((str.Split('/')[1]).Split('.')[0]));
                u1.text = str.Split('/')[0];
                usr.Add(u1);
                u1 = new Rek();

                i++;
            }


            read.Close();
            

            usr = usr.OrderBy(u1 => u1.date).ToList( );
            

        }
       
        void WriteList(List<Rek> usr) {
            int j = 0;
            
            foreach (Rek t in usr)
            {

                    AddLabel(j, textBox, 5, tab,t.date.ToShortDateString().ToString());
                AddTextBox(j, textBox, 1, tab);
                if (t.date == DateTime.Now.Date)
                    ((TextBox)textBox[j]).Background = Brushes.Red;
                else if ((t.date.DayOfYear - DateTime.Now.DayOfYear) >= 7)
                    ((TextBox)textBox[j]).Background = Brushes.Green;
                else
                    ((TextBox)textBox[j]).Background = Brushes.Yellow;
                
                ((TextBox)textBox[j]).Text = t.text;
                j++;
               
            }
           
            j = 0;
            n = textBox.Count;
            //textBox.Clear();
            //label.Clear();
            //checkBox.Clear();
            //usr.Clear();
            //tab = 0;
           

        }




        void ClearDate() {
            textBox.Clear();
            label.Clear();
            checkBox.Clear();
            usr.Clear();
            tab = 0;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if ((DatePicker1.SelectedDate >DateTime.Now.Date)&&(textBox1.Text!=""))
            {
                //n = textBox.Count;
                ClearDate();

                FileWrite(textBox1.Text, DatePicker1.Text.ToString());

                FillArr();
                panel.Children.Clear();


                WriteList(usr);
                textBox1.Text = "";
            }
            else MessageBox.Show("Введите коректные данные!");
          
          
        }

        private void MyGrid_Loaded(object sender, RoutedEventArgs e)
        {
            DatePicker1.Text = DateTime.Now.ToString();
            FillArr();
            FileStream file = new FileStream("dataBase.txt", FileMode.Create, FileAccess.Write);
            StreamWriter writer = new StreamWriter(file);
            foreach (Rek t in usr)
            {
                if (t.date >=DateTime.Now.Date)
                    writer.WriteLine(t.text + "/" + t.date);

            }


            writer.Close();
            ClearDate();

            FillArr();
           
            WriteList(usr);

            
           
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
           
            MessageBox.Show(n.ToString());
            for (int i = 0; i<n; i++)
            {
                ((TextBox)textBox[i]).IsEnabled = true;
            }
            textBox1.Text = "";
        }

       




        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                MessageBox.Show(((TextBox)e.OriginalSource).Text);

                FileStream file = new FileStream("dataBase.txt", FileMode.Create, FileAccess.Write);
                StreamWriter writer = new StreamWriter(file);
                foreach (Rek t in usr)
                {
                   if ((t.text != ((TextBox)e.OriginalSource).Text))                      
                    writer.WriteLine(t.text + "/" + t.date);

                }


                writer.Close();
                ClearDate();
               
                FillArr();
                panel.Children.Clear();
                WriteList(usr);
            }
            else if (e.Key == Key.F12) {
             
                textBox1.Text = ((TextBox)e.OriginalSource).Text;
                //DatePicker1.Text = usr[textBox.IndexOf()].date;
                MessageBox.Show(((TextBox)e.OriginalSource).Text);
                b1.Visibility = Visibility.Hidden;
                b3.Visibility = Visibility.Visible;
                StrChe = ((TextBox)e.OriginalSource).Text;
               
            }

           
             
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            textBox1.Text= "";
            FileStream file = new FileStream("dataBase.txt", FileMode.Create, FileAccess.Write);
            StreamWriter writer = new StreamWriter(file);
            foreach (Rek t in usr)
            {
                if (t.text != StrChe)
                    writer.WriteLine(t.text + "/" + t.date);
                else
                    writer.WriteLine(textBox1.Text + "/" + DatePicker1.Text.ToString());

            }
            writer.Close();

            ClearDate();
            
            FillArr();
            panel.Children.Clear();
            WriteList(usr);

        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            ni.Visible = true;
            ni.Click += (sndr, args) =>
            {
                this.Show();
                this.WindowState = WindowState.Normal;
            };
            this.Hide();
        }

        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

       
    


       


    }


}

