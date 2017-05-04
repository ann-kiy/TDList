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
using System.Windows.Media.Animation;
using System.Windows.Threading;




namespace ToDoList
{

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
       public DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
       const ushort intermediateTime = 900;

        DoubleAnimation anim;
        int left;
        int top;
        DependencyProperty prop;
        int end;
        System.Windows.Forms.NotifyIcon ni = new System.Windows.Forms.NotifyIcon();
        public MainWindow()
        {
            InitializeComponent();
            TrayPos tpos = new TrayPos();
            tpos.getXY((int)this.Width, (int)this.Height, out top, out left, out prop, out end);
            this.Top = top;
            this.Left = left;
            anim = new DoubleAnimation(end, TimeSpan.FromSeconds(1));
            ni.Icon = new System.Drawing.Icon("../../50-512.ico");

        }
        ArrayList textBox = new ArrayList();
        ArrayList label = new ArrayList();
        //ArrayList checkBox = new ArrayList();
        int i = 0, tab = 1;
        int n;
        string StrChe;

      
        List<Record> usr = new List<Record>();
        
        Record u1 = new Record();

        //public List<Rek> Data
        //{
        //    get
        //    {
        //        return usr1;
        //    }
        //}
       
        void AddTextBox(int i, ArrayList textBox, int h, int l)
        {

            textBox.Add(new TextBox());
            ((TextBox)textBox[i]).HorizontalAlignment = HorizontalAlignment.Left;
            ((TextBox)textBox[i]).Height = 35;
            ((TextBox)textBox[i]).Width = 350;
            ((TextBox)textBox[i]).VerticalAlignment = VerticalAlignment.Top;
            ((TextBox)textBox[i]).TextWrapping = TextWrapping.Wrap;
            ((TextBox)textBox[i]).Margin = new Thickness(h, l, 0, 0);
            ((TextBox)textBox[i]).VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            ((TextBox)textBox[i]).KeyUp += textBox1_KeyUp;
            ((TextBox)textBox[i]).BorderBrush = new SolidColorBrush();
            ((TextBox)textBox[i]).SelectionBrush = Brushes.White;           
            ((TextBox)textBox[i]).BorderThickness = new Thickness(2, 2, 2, 2);
            ((TextBox)textBox[i]).IsReadOnly = true;                   
            ((TextBox)textBox[i]).Opacity = 0.7;
            
            
            panel.Children.Add(((TextBox)textBox[i]));
        }


        void AddLabel(int i, ArrayList textBox, int h, int l, string date)
        {

            label.Add(new Label());
            ((Label)label[i]).HorizontalAlignment = HorizontalAlignment.Left;
            ((Label)label[i]).VerticalAlignment = VerticalAlignment.Top;
            ((Label)label[i]).Margin = new Thickness(h, l, 0, 0);
            ((Label)label[i]).FontSize = 12;
            ((Label)label[i]).FontWeight = FontWeights.Bold;
            ((Label)label[i]).Foreground = Brushes.Gray;
            ((Label)label[i]).Content = "———————————" + date + " ———————————";
            //MyGrid.Children.Add(((TextBox)textBox[i]));

            panel.Children.Add(((Label)label[i]));


        }
        void FileWrite(string text, string date)
        {

            FileStream file = new FileStream("../../dataBase.txt", FileMode.Append, FileAccess.Write);
            StreamWriter writer = new StreamWriter(file);

            writer.WriteLine(text + "/" + date);
            writer.Close();

        }
    


        List<Record> FillArray(List<Record> usr)
        {
            FileStream file = new FileStream("../../dataBase.txt", FileMode.OpenOrCreate, FileAccess.Read);
            StreamReader read = new StreamReader(file);
            string str;

            while ((!read.EndOfStream)) {

                str = read.ReadLine();
                if ((str == "") ||(new DateTime(int.Parse((str.Split('/')[1]).Split('.')[2].Split(' ')[0]), int.Parse((str.Split('/')[1]).Split('.')[1]), int.Parse((str.Split('/')[1]).Split('.')[0]))< DateTime.Now.Date))
                    continue;                
                    u1.date = new DateTime(int.Parse((str.Split('/')[1]).Split('.')[2].Split(' ')[0]), int.Parse((str.Split('/')[1]).Split('.')[1]), int.Parse((str.Split('/')[1]).Split('.')[0]));
                    u1.text = str.Split('/')[0];
                    usr.Add(u1);
                    u1 = new Record();
                    i++;            
               
            }
            read.Close();
            DataRecord.Value = usr;

            return usr;
            
        }





        void WriteList(List<Record> usr)
        {
            int j = 0;

            foreach (Record t in usr)
            {
                if (t.date >= DateTime.Now.Date)
                {
                    AddLabel(j, textBox, 1, tab, t.date.ToShortDateString().ToString());
                    AddTextBox(j, textBox, 1, tab);
                    if (t.date == DateTime.Now.Date)
                        ((TextBox)textBox[j]).Background = Brushes.OrangeRed;
                    else if ((t.date.DayOfYear - DateTime.Now.DayOfYear) >= 7)
                        ((TextBox)textBox[j]).Background = Brushes.Green;
                    else
                        ((TextBox)textBox[j]).Background = Brushes.Yellow;

                    ((TextBox)textBox[j]).Text = t.text;
                    j++;
                }

            }

            j = 0;
            n = textBox.Count;
           


        }




        void ClearDate()
        {
            textBox.Clear();
            label.Clear();
            //checkBox.Clear();
            usr.Clear();
            tab = 0;
        }




        private void Button_Click(object sender, RoutedEventArgs e)
        {
            combobox.SelectedIndex = -1;
          
            if ((DatePicker1.SelectedDate >= DateTime.Now.Date) && (textBox1.Text != ""))
            {
                //n = textBox.Count;
                ClearDate();

                FileWrite(textBox1.Text, DatePicker1.Text.ToString());

                
                panel.Children.Clear();


                WriteList(FillArray(usr));
                textBox1.Text = "";
            }
            else MessageBox.Show("Введите коректные данные!");


        }




        private void timerTick(object sender, EventArgs e)
        {
           new Window1().Show();
          //timer.Stop();
           // button1.Visibility = Visibility.Visible;
        }


        private void MyGrid_Loaded(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            //DataRec.SpotedTimer = true;
            AnimationClock clock = anim.CreateClock();
            this.ApplyAnimationClock(prop, clock);
            DatePicker1.Text = DateTime.Now.ToString();
            dat2.Text = "Выберите дату";
            //FillArr();
            //FileStream file = new FileStream("../../dataBase.txt", FileMode.Open, FileAccess.Write);
            //StreamWriter writer = new StreamWriter(file);
            //foreach (Rek t in usr)
            //{
            //    if (t.date >= DateTime.Now.Date)
            //        writer.WriteLine(t.text + "/" + t.date);

            //}


            //writer.Close();
            //ClearDate();
            //FillArr();

            WriteList(FillArray(usr));



        }


        int indOf(string str) {
            return (usr.IndexOf((usr.Find(u1 => u1.text == str)))); 
        }







        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {

                if (MessageBox.Show("Вы точно хотите удалить запись?", "Удаление", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation) == MessageBoxResult.OK)
                {

                    FileStream file = new FileStream("../../dataBase.txt", FileMode.Create, FileAccess.Write);
                    StreamWriter writer = new StreamWriter(file);
                    usr.RemoveAt(indOf(((TextBox)e.OriginalSource).Text));
                    foreach (Record t in usr)
                    {
                          writer.WriteLine(t.text + "/" + t.date);

                    }


                    writer.Close();
                    ClearDate();

                    //FillArr();
                    panel.Children.Clear();
                    WriteList(FillArray(usr));
                }
                }
            else if (e.Key == Key.F12)
            {

                MessageBox.Show(indOf(((TextBox)e.OriginalSource).Text).ToString());

                    DatePicker1.Text = (usr.Find(u1 => u1.text == ((TextBox)e.OriginalSource).Text).date).ToString();
                    b1.Visibility = Visibility.Hidden;
                    b3.Visibility = Visibility.Visible;
                    StrChe = ((TextBox)e.OriginalSource).Text;
                    textBox1.Text = ((TextBox)e.OriginalSource).Text;
                }

            

            }
        

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы точно хотите изменить  запись?", "Редактирование", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation) == MessageBoxResult.OK)
            {

                FileStream file = new FileStream("../../dataBase.txt", FileMode.Create, FileAccess.Write);
                StreamWriter writer = new StreamWriter(file);
                foreach (Record t in usr)
                {
                    if (t.text != StrChe)
                        writer.WriteLine(t.text + "/" + t.date);
                    else
                        writer.WriteLine(textBox1.Text + "/" + DatePicker1.Text.ToString());

                }
                b1.Visibility = Visibility.Visible;
                b3.Visibility = Visibility.Hidden;
                writer.Close();

                ClearDate();

               // FillArr();
                panel.Children.Clear();
                WriteList(FillArray(usr));
                textBox1.Text = "";
            }

        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
           
                ni.Visible = true;
                ni.Click += (sndr, args) =>
                {
                    this.Show();
                    ni.Visible = false;
                    this.WindowState = WindowState.Normal;
                };
                this.Hide();
                timer.Tick += new EventHandler(timerTick);
                timer.Interval = new TimeSpan(0, 0, intermediateTime);
                timer.Start();
                //DataRec.SpotedTimer = false;
                if (this.WindowState == WindowState.Normal)
                    timer.Stop();
           
           
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            ni.Visible = false;
            
        }

        private void textBox1_SelectionChanged(object sender, RoutedEventArgs e)
        {
            ((TextBox)e.OriginalSource).Opacity = 1;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (combobox.SelectedIndex == 1)
            {
                textBox.Clear();
                label.Clear();
                //checkBox.Clear();

                tab = 0;
                //FillArr();
                //MessageBox.Show(usr.OrderBy(u1 => u1.date == dat2.SelectedDate).Count().ToString());
                //usr = (usr.FindAll(u1 => u1.date == dat2.SelectedDate));
                panel.Children.Clear();
                WriteList((usr.FindAll(u1 => u1.date == dat2.SelectedDate)));

            }
            else if (combobox.SelectedIndex == 0)
            {
                panel.Children.Clear();
                usr = usr.OrderBy(u1 => u1.date).ToList();
                WriteList(usr);
            }

        }






  

    }

    //Comic Sans MS
}

