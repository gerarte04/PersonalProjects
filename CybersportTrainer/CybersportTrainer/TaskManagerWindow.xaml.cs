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
using System.IO;

namespace CybersportTrainer
{
    /// <summary>
    /// Логика взаимодействия для TaskManagerWindow.xaml
    /// </summary>
    /// 
    public enum CurrentStatus { FirstEntry, DropStats, RiseStats }

    public delegate void DataAppender(string name, string value);

    public partial class TaskManagerWindow : Window
    {
        CurrentStatus currS = CurrentStatus.FirstEntry;
        int currentPage = 1;
        readonly int pagesCount;

        List<TaskManagerPage> pages;
        Dictionary<string, string> values;

        DataAppender AppendData;

        public TaskManagerWindow(CurrentStatus cs)
        {
            InitializeComponent();

            currS = cs;

            pagesCount = new DirectoryInfo(getPath(currS)).GetFiles("*.txt").Count();
            UpdateStageViewer(currentPage, pagesCount);

            values = new Dictionary<string, string>();
            using (StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + "/global_params.txt"))
            {
                foreach (string str in sr.ReadToEnd().Split('\n'))
                {
                    if (str != null && str != "")
                    {
                        string[] info = str.Split(':');
                        values[info[0]] = info[1];
                    }
                }
            }

            AppendData = (name, value) =>
            {
                values[name] = value;
            };

            pages = new List<TaskManagerPage>();
            for(int i = 0; i < pagesCount; i++)
            {
                pages.Add(null);
            }

            UpdateFrameContent(getPath(currS), currentPage);
        }

        private string getPath(CurrentStatus cs)
        {
            string s = Directory.GetCurrentDirectory() + "\\task_manager";

            switch(cs)
            {
                case CurrentStatus.FirstEntry:
                    s += "\\first";
                    break;
            }

            return s;
        }

        private void UpdateStageViewer(int completedStages, int totalStages)
        {
            stage_viewer.Children.Clear();
            for (int i = 1; i <= totalStages; i++)
            {
                if(i != 1)
                {
                    Border newLine = (Border)this.TryFindResource("sv_line");
                    newLine.Margin = new Thickness(38 * (i - 1) + 17 * (i - 2), 0, 0, 0);
                    stage_viewer.Children.Add(newLine);
                }

                Border newCircle = (Border)this.TryFindResource("sv_circle");
                newCircle.Margin = new Thickness(38 * (i - 1) + 17 * (i - 1), 0, 0, 0);
                if(i <= completedStages)
                    newCircle.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#586e5c"));

                var numb = new TextBlock() { Text = i.ToString(), Style = (Style)this.TryFindResource("text_style"), FontSize = 22 };
                numb.HorizontalAlignment = HorizontalAlignment.Center;
                numb.VerticalAlignment = VerticalAlignment.Center;
                newCircle.Child = numb;

                stage_viewer.Children.Add(newCircle);
            }
        }

        private void SaveChanges(string saveFilePath)
        {
            using (StreamWriter sw = new StreamWriter(saveFilePath, false))
            {
                foreach(string name in values.Keys)
                {
                    sw.Write(name + ":" + values[name] + '\n');
                }
            }
        }

        private void UpdateFrameContent(string path, int page)
        {
            var p = new TaskManagerPage(path, page) { AppendData = this.AppendData };
            pages[page - 1] = p;
            stage_frame.Content = p;
        }

        private void UpdateFrameContent(TaskManagerPage tmp)
        {
            stage_frame.Content = tmp;
        }

        private void ChangeArrowButtonsAppointments(int currPage)
        {
            if(currPage == 1)
                back_b.Visibility = Visibility.Hidden;
            else
                back_b.Visibility = Visibility.Visible;

            if(currPage == pagesCount)
            {
                next_b_tb.Text = "Готово";
                next_b.MouseLeftButtonUp += CloseTM;
            }
            else
            {
                next_b_tb.Text = "Далее";
                next_b.MouseLeftButtonUp -= CloseTM;
            }
        }

        private void CloseTM(object sender, EventArgs e)
        {
            if (pages[currentPage - 1].IsValuesSet())
            {
                SaveChanges(Directory.GetCurrentDirectory() + "/global_params.txt");

                this.Close();
            }
            else
                MessageBox.Show("Пожалуйста, выберите один из предложенных вариантов ответа.");
        }

        private void GoPrevPage(object sender, EventArgs e)
        {
            if (currentPage != 1)
            {
                currentPage--;

                if (pages[currentPage - 1] != null)
                    UpdateFrameContent(pages[currentPage - 1]);
                else
                    UpdateFrameContent(getPath(currS), currentPage);

                UpdateStageViewer(currentPage, pagesCount);
            }

            ChangeArrowButtonsAppointments(currentPage);
        }

        private void GoNextPage(object sender, EventArgs e)
        {
            if (!pages[currentPage - 1].IsValuesSet())
                MessageBox.Show("Пожалуйста, выберите один из предложенных вам вариантов ответа.");
            else if (currentPage != pagesCount)
            {
                currentPage++;

                if (pages[currentPage - 1] != null)
                {
                    UpdateFrameContent(pages[currentPage - 1]);
                }
                else
                    UpdateFrameContent(getPath(currS), currentPage);
                
                UpdateStageViewer(currentPage, pagesCount);
            }

            ChangeArrowButtonsAppointments(currentPage);
        }
    }
}
