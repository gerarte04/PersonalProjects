using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace CybersportTrainer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer changeFontTimer;
        int imageNumber = 1;

        public MainWindow()
        {
            InitializeComponent();

            changeFontTimer = new DispatcherTimer();
            changeFontTimer.Interval = TimeSpan.FromSeconds(10);
            changeFontTimer.Tick += FontAnimation;
            changeFontTimer.Start();

            Hider.SuperDuperExtraHideMainWindowElements = delegate {
                close_b.Visibility = Visibility.Hidden;
            };
        }

        private void this_Initialized(object sender, EventArgs e)
        {
            var mainp = new MainPage(this);
            central_frame.Content = mainp;
        }

        private void FontAnimation(object sender, EventArgs e)
        {
            var animation1 = new DoubleAnimation();
            animation1.Duration = TimeSpan.FromSeconds(0.25);
            animation1.From = dark_overlay.Opacity;
            animation1.To = 1;

            animation1.Completed += delegate
            {
                ChangeImage();

                var animation2 = new DoubleAnimation();
                animation2.Duration = TimeSpan.FromSeconds(0.25);
                animation2.From = dark_overlay.Opacity;
                animation2.To = 0.95;
                dark_overlay.BeginAnimation(Border.OpacityProperty, animation2);
            };
            dark_overlay.BeginAnimation(Border.OpacityProperty, animation1);
        }

        private void ChangeImage()
        {
            imageNumber = (imageNumber == 7) ? 1 : imageNumber + 1;
            back_image.Source = new BitmapImage(new Uri("pack://application:,,,/res/background_" + imageNumber.ToString() + ".jpg", UriKind.RelativeOrAbsolute));
        }

        private void close_b_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
