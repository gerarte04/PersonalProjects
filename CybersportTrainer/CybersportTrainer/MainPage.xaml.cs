using System;
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
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CybersportTrainer
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        Window owner;
        ParametersManager pManager;

        Dictionary<StatsType, Statistics> playerStats;
        Dictionary<StatsType, Grid> statsGrids;
        Tasks dailyTasks;

        string currentStatsPath;
        double lowerArrowValue = 2.00;
        double higherArrowValue = 3.00;
        
        public MainPage(Window owner)
        {
            InitializeComponent();

            this.owner = owner;
            pManager = new ParametersManager(owner, Directory.GetCurrentDirectory() + "\\global_params.txt");

            var d = new DirectoryInfo("guides");
            var guides = d.GetFiles("*.txt");
            foreach(FileInfo guidePath in guides)
            {
                CreateGuideLink(guidePath.FullName);
            }

            playerStats = new Dictionary<StatsType, Statistics>();
            statsGrids = new Dictionary<StatsType, Grid>();
            currentStatsPath = Directory.GetCurrentDirectory() + "\\alltime_statistics\\";
            foreach (StatsType st in Enum.GetValues(typeof(StatsType)))
            {
                var stats = new Statistics();
                stats.UpdateStatsFromPath(currentStatsPath + st.ToString() + ".txt");
                playerStats.Add(st, stats);

                if (st == StatsType.AllStats)
                    statsGrids.Add(st, mainstats_grid);
                else
                {
                    Grid g = (Grid)this.TryFindResource("weapon_grid");
                    foreach (FrameworkElement f in g.Children)
                    {
                        if (f is Image)
                            (f as Image).Source = new BitmapImage(new Uri(Statistics.GetStatsImagePath(st)));

                        if (f is StackPanel)
                            foreach (TextBlock tb in (f as StackPanel).Children.OfType<TextBlock>())
                                tb.Text = Statistics.GetStatsWName(st);
                    }
                    statsGrids.Add(st, g);
                    stat_stack.Children.Add(g);
                }
            }

            dailyTasks = new Tasks();
            dailyTasks.UpdateTasksFromPath();

            UpdateScreenStats();
            UpdateScreenTasks();
        }

        private void this_Loaded(object sender, EventArgs e)
        {
            pManager.MakeFocusActions();
        }

        private void CreateGuideLink(string guidePath)
        {
            Border newLink = (Border)this.TryFindResource("link_b");
            newLink.Margin = new Thickness(0, 15, 0, 0);

            foreach(FrameworkElement c in (newLink.Child as StackPanel).Children)
            {
                if(c is TextBlock)
                {
                    (c as TextBlock).Text = GetTitle(guidePath);
                }
            }

            newLink.MouseLeftButtonUp += delegate
            {
                NavigationService.Navigate(new GuidePage(guidePath));
            };

            guide_stack.Children.Add(newLink);
        }

        private string GetTitle(string guidePath)
        {
            using (StreamReader sr = new StreamReader(guidePath))
            {
                string[] info = sr.ReadLine().Split('"');
                return info[1];
            }
        }

        private void UpdateScreenStats()
        {
            foreach (StatsType st in Enum.GetValues(typeof(StatsType)))
                foreach(Grid g in statsGrids[st].Children.OfType<Grid>())
                    foreach(StackPanel sp in g.Children.OfType<StackPanel>())
                        foreach(TextBlock tb in sp.Children.OfType<TextBlock>())
                        {
                            switch (tb.Uid) 
                            {
                                case "average_kd":
                                    tb.Text = playerStats[st].KD.ToString("F2").Replace(',', '.');

                                    var anim = new DoubleAnimation() { Duration = TimeSpan.FromSeconds(0.5), From = arrowAngle.Angle };
                                    anim.To = (playerStats[st].KD - lowerArrowValue) / (higherArrowValue - lowerArrowValue) * 180 - 90;
                                    anim.EasingFunction = new CircleEase();
                                    arrowAngle.BeginAnimation(RotateTransform.AngleProperty, anim);

                                    break;

                                case "total_kills":
                                    tb.Text = SpaceLongNum(playerStats[st].TotalKills);
                                    break;
                                case "headkills_percent":
                                    tb.Text = playerStats[st].HeadkillsPercent.ToString("F2").Replace(',', '.') + "%";
                                    break;
                                case "accuracy_percent":
                                    tb.Text = playerStats[st].Accuracy.ToString("F2").Replace(',', '.') + "%";
                                    break;

                                case "total_matches":
                                    tb.Text = SpaceLongNum(playerStats[st].TotalMatches);
                                    break;
                                case "wins_percent":
                                    tb.Text = playerStats[st].WinsPercent.ToString("F2").Replace(',', '.') + "%";
                                    break;
                                case "shots_for_kill":
                                    tb.Text = playerStats[st].ShotsForKill.ToString("F2").Replace(',', '.');
                                    break;
                            }
                        }
        }

        private void UpdateScreenTasks()
        {
            tasks_stack.Children.Clear();

            var ordered = dailyTasks.taskInfos.OrderBy(c => (c.Done < c.Need));

            foreach(TaskInfo t in ordered)
            {
                Border b = (Border)this.TryFindResource("task_b");
                b.Margin = new Thickness(0, 15, 0, 0);
                if (t.Done >= t.Need)
                    b.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#586e5c"));

                foreach(FrameworkElement f in (b.Child as Grid).Children)
                {
                    if (f is TextBlock)
                        (f as TextBlock).Text = t.Done.ToString() + "/" + t.Need.ToString();

                    if (f is StackPanel)
                    {
                        foreach (FrameworkElement f2 in (f as StackPanel).Children)
                        {
                            if (f2 is Image)
                                (f2 as Image).Source = new BitmapImage(new Uri(Tasks.GetTaskIconPath(t.Task), UriKind.RelativeOrAbsolute));

                            if (f2 is TextBlock)
                                (f2 as TextBlock).Text = Tasks.GetTaskPhrase(t.Task).Replace("*", t.Need.ToString());
                        }
                    }
                }

                tasks_stack.Children.Add(b);
            }
        }

        private string SpaceLongNum(double value)
        {
            string stringNum = value.ToString();

            for (int i = stringNum.Length - 3; i > 0; i -= 3)
                stringNum = stringNum.Insert(i, " ");

            return stringNum;
        }

        private void ChangeStatSliderColors(object sender)
        {
            foreach (FrameworkElement c in stat_slider.Children.OfType<Border>())
            {
                (c as Border).Opacity = 1;                
            }

            (sender as Border).Opacity = 0.7;
        }

        private void stat_by_alltime_b_Click(object sender, EventArgs e)
        {
            currentStatsPath = Directory.GetCurrentDirectory() + "\\alltime_statistics\\";

            foreach (StatsType st in Enum.GetValues(typeof(StatsType)))
            {
                playerStats[st].UpdateStatsFromPath(currentStatsPath + st.ToString() + ".txt");
            }

            UpdateScreenStats();
            ChangeStatSliderColors(sender);
        }

        private void stat_by_month_b_Click(object sender, EventArgs e)
        {
            currentStatsPath = Directory.GetCurrentDirectory() + "\\month_statistics\\";

            foreach (StatsType st in Enum.GetValues(typeof(StatsType)))
            {
                playerStats[st].UpdateStatsFromPath(currentStatsPath + st.ToString() + ".txt");
            }

            UpdateScreenStats();
            ChangeStatSliderColors(sender);
        }

        private void stat_by_day_b_Click(object sender, EventArgs e)
        {
            currentStatsPath = Directory.GetCurrentDirectory() + "\\day_statistics\\";

            foreach (StatsType st in Enum.GetValues(typeof(StatsType)))
            {
                playerStats[st].UpdateStatsFromPath(currentStatsPath + st.ToString() + ".txt");
            }

            UpdateScreenStats();
            ChangeStatSliderColors(sender);
        }

        private void stat_by_session_b_Click(object sender, EventArgs e)
        {
            MediaElement rickroll = new MediaElement() { Stretch = Stretch.Fill, 
                HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center};
            rickroll.Width = 6000;
            rickroll.IsMuted = false;
            rickroll.LoadedBehavior = MediaState.Manual;
            rickroll.UnloadedBehavior = MediaState.Manual;
            rickroll.Source = new Uri(@"rickroll.mp4", UriKind.Relative);
            rickroll.StretchDirection = StretchDirection.Both;

            Hider.SuperDuperExtraHideMainWindowElements();
            main_cont.Children.Add(rickroll);
            main_cont.UpdateLayout();
            rickroll.Play();
        }

        private void update_all(object sender, EventArgs e)
        {
            foreach (StatsType st in Enum.GetValues(typeof(StatsType)))
            {
                playerStats[st].UpdateStatsFromPath(currentStatsPath + st.ToString() + ".txt");
            }
            dailyTasks.UpdateTasksFromPath();

            UpdateScreenStats();
            UpdateScreenTasks();
            pManager.MakeFocusActions();
        }

        private void arrow_MouseEnter(object sender, EventArgs e)
        {
            //var anim = new DoubleAnimation() { From = arrowAngle.Angle, To = arrowAngle.Angle, Duration = TimeSpan.FromSeconds(0.4), AutoReverse = true};
            //anim.EasingFunction = new ElasticEase() { Springiness = 2 };
            //arrowAngle.BeginAnimation(RotateTransform.AngleProperty, anim);
        }
    }
}
