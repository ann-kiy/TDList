using System;
using System.Collections;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ToDoList
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
        public Window1()
        {
            InitializeComponent();
            TrayPos tpos = new TrayPos();
            tpos.getXY((int)this.Width, (int)this.Height, out top, out left, out prop, out end);
            this.Top = top;
            this.Left = left;
            anim = new DoubleAnimation(end, TimeSpan.FromSeconds(1));
            ni.Icon = new System.Drawing.Icon("../../50-512.ico");
        }
        DoubleAnimation anim;
        int left;
        int top;
        DependencyProperty prop;
        int end;
        System.Windows.Forms.NotifyIcon ni = new System.Windows.Forms.NotifyIcon();

        ArrayList textBox = new ArrayList();
        List<Record> usr = DataRecord.Value;


        private void timerTick(object sender, EventArgs e)
        {
            this.Close();
            timer.Stop();
            // button1.Visibility = Visibility.Visible;
        }



        void AddTextBox(int i, ArrayList textBox, int h, int l)
        {

            textBox.Add(new TextBox());
            ((TextBox)textBox[i]).HorizontalAlignment = HorizontalAlignment.Left;
            ((TextBox)textBox[i]).Height = 35;
            ((TextBox)textBox[i]).Width = 350;
            ((TextBox)textBox[i]).VerticalAlignment = VerticalAlignment.Top;
            ((TextBox)textBox[i]).TextWrapping = TextWrapping.Wrap;
            ((TextBox)textBox[i]).Margin = new System.Windows.Thickness(h, l, 0, 0);
            ((TextBox)textBox[i]).VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            ((TextBox)textBox[i]).Background = Brushes.OrangeRed;
            ((TextBox)textBox[i]).BorderBrush = Brushes.OrangeRed;
            ((TextBox)textBox[i]).SelectionBrush = Brushes.White;
            ((TextBox)textBox[i]).BorderThickness = new Thickness(2, 2, 2, 2);
            ((TextBox)textBox[i]).IsReadOnly = true;
            ((TextBox)textBox[i]).Opacity = 0.4;
            ((TextBox)textBox[i]).MouseDown += Grid_MouseDown;
                

            panel.Children.Add(((TextBox)textBox[i]));
        }
        void WriteList(List<Record> usr)
        {
            
            int j = 0;

            foreach (Record t in usr)
            {
                if (t.date == DateTime.Now.Date)
                {

                   
                    AddTextBox(j, textBox, 1, 1);
                    //if (t.date == DateTime.Now.Date)
                    //    ((TextBox)textBox[j]).Background = Brushes.OrangeRed;
                    //else if ((t.date.DayOfYear - DateTime.Now.DayOfYear) >= 7)
                    //    ((TextBox)textBox[j]).Background = Brushes.Green;
                    //else
                    //    ((TextBox)textBox[j]).Background = Brushes.Yellow;

                    ((TextBox)textBox[j]).Text = t.text;
                    j++;
                }

            }

            j = 0;




        }



        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            AnimationClock clock = anim.CreateClock();
            this.ApplyAnimationClock(prop, clock);
           MainWindow f = new MainWindow();
         // f.ShowDialog();
          usr= DataRecord.Value;
           //f.Close();

           WriteList(usr);
          

           timer.Tick += new EventHandler(timerTick);
           timer.Interval = new TimeSpan(0, 0, 10);
           timer.Start();
            
            
           
           

           
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            timer.Stop();
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            timer.Stop();
            this.Close();
           // DataRec.stateW = WindowState.Normal;
            //new MainWindow().WindowState=WindowState.Normal;

        }
      
    }
}
