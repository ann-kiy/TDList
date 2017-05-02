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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ToDoList
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
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

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            AnimationClock clock = anim.CreateClock();
            this.ApplyAnimationClock(prop, clock);
        }
      
    }
}
