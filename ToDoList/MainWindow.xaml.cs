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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Collections;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using System.Media;

namespace ToDoList
{

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
       public DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
       System.Windows.Forms.NotifyIcon icon = new System.Windows.Forms.NotifyIcon();
        DoubleAnimation anim;
        int left;
        int top;
        DependencyProperty prop;
        int end;

        ArrayList textBox = new ArrayList();
        ArrayList label = new ArrayList();
        public int coutRecords = 0;
        int tabTexBox = 1;
        string StrChe;
        List<Record> records = new List<Record>();
        Record task = new Record();
        public Config config = new Config();
        IniFile ini = new IniFile("../../config.ini");
        
        public MainWindow()
        {
            InitializeComponent();
            TrayPos tpos = new TrayPos();
            tpos.getXY((int)this.Width, (int)this.Height, out top, out left, out prop, out end);
            this.Top = top;
            this.Left = left;
            anim = new DoubleAnimation(end, TimeSpan.FromSeconds(1));
            icon.Icon = new System.Drawing.Icon("../../50-512.ico");
        }

        private void Main_Initialized(object sender, EventArgs e)
        {
            ReadConfig();
        }

        private void Main_Loaded(object sender, RoutedEventArgs e)
        {
            ReadConfig();
            if (config.sounds == true)
            {
                SoundPlayer player = new SoundPlayer("../../sounds/paper.wav");
                player.Load();
                player.Play();
            }
        }


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
            ((TextBox)textBox[i]).GotFocus += text_GotFocus;
            ((TextBox)textBox[i]).BorderBrush = new SolidColorBrush();
            ((TextBox)textBox[i]).SelectionBrush = Brushes.White;           
            ((TextBox)textBox[i]).BorderThickness = new Thickness(2, 2, 2, 2);
            ((TextBox)textBox[i]).FontFamily = new FontFamily("Arial");
            ((TextBox)textBox[i]).FontSize = 14;
            ((TextBox)textBox[i]).Padding = new Thickness(3, 3, 3, 3);
            ((TextBox)textBox[i]).IsReadOnly = true;                   
            ((TextBox)textBox[i]).Opacity = 0.7;
            ((TextBox)textBox[i]).ToolTip = "Для удаления нажмите кнопку <Del>, для редактирования кнопку <F12>";
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
            ((Label)label[i]).Content = "————————————" + date + " ———————————";
            panel.Children.Add(((Label)label[i]));


        }

        public void FileWrite(string text, string date)
        {
            FileStream file = new FileStream("../../dataBase.txt", FileMode.Append, FileAccess.Write);
            if (!File.Exists("../../dataBase.txt")) { throw new Exception("file does not exist"); }
            else
            {
                StreamWriter writer = new StreamWriter(file);
                writer.WriteLine(text + "/" + date);
                writer.Close();
            }
        }

        public DateTime StringToDate(string str)
        {

            return new DateTime(int.Parse((str.Split('/')[1]).Split('.')[2].Split(' ')[0]), int.Parse((str.Split('/')[1]).Split('.')[1]), int.Parse((str.Split('/')[1]).Split('.')[0]));

        }


        public List<Record> ReedOfFileInArray(List<Record> usr){

            FileStream file = new FileStream("../../dataBase.txt", FileMode.OpenOrCreate, FileAccess.Read);
            StreamReader read = new StreamReader(file);
            string str;
            while ((!read.EndOfStream)) {

                str = read.ReadLine();
                if ((str == "") ||(StringToDate(str)< DateTime.Now.Date))
                    continue;
                task.date = StringToDate(str);
                task.text = str.Split('/')[0];
                usr.Add(task);
                task = new Record();
                        coutRecords++;            
               
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
                    AddLabel(j, textBox, 1, tabTexBox, t.date.ToShortDateString().ToString());
                    AddTextBox(j, textBox, 1, tabTexBox);
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

        }




        void ClearDate()
        {
            textBox.Clear();
            label.Clear();
            records.Clear();
            tabTexBox = 0;
        }




        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {

            if ((DatePicker1.SelectedDate >= DateTime.Now.Date) && (textInput.Text != "") && (textInput.Text != "Введите задачу"))
            {
               
                ClearDate();
                FileWrite(textInput.Text, DatePicker1.Text.ToString());                
                panel.Children.Clear();
                WriteList(ReedOfFileInArray(records));
                textInput.Text = "";
                if (config.sounds == true)
                {
                    SoundPlayer player = new SoundPlayer("../../sounds/pencil.wav");
                    player.Load();
                    player.Play();
                }
                Sorting();
            }
            else MessageBox.Show("Введите коректные данные!");
            textInput.Text = "Введите задачу";

        }




        private void timerTick(object sender, EventArgs e)
        {
            if ((records.FindAll(u1 => u1.date == DateTime.Now.Date)).Count!=0)
           new Window1().Show();      
           
        }


        private void MyGrid_Loaded(object sender, RoutedEventArgs e)
        {
            timer.Tick += new EventHandler(timerTick);
            timer.Interval = new TimeSpan(0, 0, Convert.ToUInt16(config.time));
            timer.Stop();
            AnimationClock clock = anim.CreateClock();
            this.ApplyAnimationClock(prop, clock);
            DatePicker1.Text = DateTime.Now.ToString();
            dat2.SelectedDate = DateTime.Now.Date;
            WriteList(ReedOfFileInArray(records));
            dat2.SelectedDateChanged += ComboBox_SelectionChanged;
            icon.Visible = false;

            }


        int indexSelectTextBox(string str) 
        {

            return (records.IndexOf((records.Find(u1 => u1.text == str)))); 

        }







        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {

                if (MessageBox.Show("Вы точно хотите удалить запись?", "Удаление", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation) == MessageBoxResult.OK)
                {

                    FileStream file = new FileStream("../../dataBase.txt", FileMode.Create, FileAccess.Write);
                    StreamWriter writer = new StreamWriter(file);
                            records.RemoveAt(indexSelectTextBox(((TextBox)e.OriginalSource).Text));
                            foreach (Record t in records)
                    {
                          writer.WriteLine(t.text + "/" + t.date);

                    }
                    writer.Close();
                    ClearDate();
                    panel.Children.Clear();
                            WriteList(ReedOfFileInArray(records));
                    if (config.sounds == true)
                    {
                        SoundPlayer player = new SoundPlayer("../../sounds/delete.wav");
                        player.Load();
                        player.Play();
                    }
                }
                combobox.SelectedIndex = -1;

                }
            else if (e.Key == Key.F12)
                if (MessageBox.Show("Вы точно хотите изменить  запись?", "Редактирование", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation) == MessageBoxResult.OK)
                {
                    {
                        DatePicker1.Text = (records.Find(u1 => u1.text == ((TextBox)e.OriginalSource).Text).date).ToString();
                        ButtonAdd.Visibility = Visibility.Hidden;
                        buttonChanges.Visibility = Visibility.Visible;
                        StrChe = ((TextBox)e.OriginalSource).Text;
                        textInput.Text = ((TextBox)e.OriginalSource).Text;
                    }
                }
            }
        

        private void buttonChanges_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Редактировать?", "Редактирование", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation) == MessageBoxResult.OK)
            {

                FileStream file = new FileStream("../../dataBase.txt", FileMode.Create, FileAccess.Write);
                StreamWriter writer = new StreamWriter(file);
                foreach (Record t in records)
                {
                    if (t.text != StrChe)
                        writer.WriteLine(t.text + "/" + t.date);
                    else
                                writer.WriteLine(textInput.Text + "/" + DatePicker1.Text.ToString());

                }
                ButtonAdd.Visibility = Visibility.Visible;
                buttonChanges.Visibility = Visibility.Hidden;
                writer.Close();
                ClearDate();
                panel.Children.Clear();
                WriteList(ReedOfFileInArray(records));
                textInput.Text = "Введите задачу";
                combobox.SelectedIndex = -1;
            }

        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
           
                icon.Visible = true;
                icon.Click += (sndr, args) =>
                {
                    this.Show();
                    icon.Visible = false;
                    this.WindowState = WindowState.Normal;
                    timer.Stop();
                };
                this.Hide();
               
                timer.Start();
                    

            
           
           
           
               
                
                    
               
           
           
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            icon.Visible = false;
            
        }

        void Sorting() 
        {
            if (combobox.SelectedIndex == 1)
            {
                textBox.Clear();
                label.Clear();
                panel.Children.Clear();
                WriteList((records.FindAll(u1 => u1.date == dat2.SelectedDate)));

            }
            else if (combobox.SelectedIndex == 0)
            {
                panel.Children.Clear();
                records = records.OrderBy(u1 => u1.date).ToList();
                WriteList(records);
            }
        }


        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Sorting();

        }




        private void textInput_GotFocus(object sender, RoutedEventArgs e)
        {
            if (textInput.Text == "Введите задачу")
                textInput.Text = "";
        }

        private void text_GotFocus(object sender, RoutedEventArgs e)
        {
            ButtonAdd.Visibility = Visibility.Visible;
            buttonChanges.Visibility = Visibility.Hidden;
            textInput.Text = "Введите задачу";
        }

        

        private void panel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            textInput.Text = "Введите задачу";
        }

      

        private void closedBut_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void textInput_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                if (buttonChanges.Visibility == Visibility.Hidden)
                    ButtonAdd_Click(this, e);
                else
                    buttonChanges_Click(this, e);
    
        }

        private void ButtonHide_Click(object sender, RoutedEventArgs e)
        {
            Window_StateChanged(this, e);
           // timer.Start();
        }

        public void ReadConfig()
        {
            config.sounds = Convert.ToBoolean(ini.IniReadValue("main", "sounds", "true"));
            config.time = Convert.ToInt16(ini.IniReadValue("main", "time", "15"));
            //*return config;
        }

        private void ButtonSettings_Click(object sender, RoutedEventArgs e)
        {
            Settings setWin = new Settings();
            setWin.Show();
        }
    }

 
}
