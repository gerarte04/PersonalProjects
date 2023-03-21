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

namespace CybersportTrainer
{
    /// <summary>
    /// Логика взаимодействия для TaskManagerPage.xaml
    /// </summary>
    public partial class TaskManagerPage : Page
    {
        string path;
        int pageNumber;

        List<string> radioGroups;

        public DataAppender AppendData;

        public TaskManagerPage(string path, int pageNumber)
        {
            InitializeComponent();

            this.path = path;
            this.pageNumber = pageNumber;

            radioGroups = new List<string>();

            AddTMPObjects(path);
        }

        private void AddTMPObjects(string path)
        {
            using (StreamReader sr = new StreamReader(path + "/page" + pageNumber.ToString() + ".txt"))
            {
                string str;
                while ((str = sr.ReadLine()) != null)
                {
                    switch (GetTag(str))
                    {
                        case "par":
                            TextBlock paragraph = (TextBlock)this.TryFindResource("paragraph_tbl");
                            paragraph.Margin = new Thickness(0, 10, 0, 0);
                            paragraph.Text = GetContent(str);
                            container.Children.Add(paragraph);
                            break;

                        case "checkbox":
                            string lowStr;
                            while (GetTag(lowStr = sr.ReadLine()) != "/checkbox")
                            {
                                var args = GetContent(lowStr).Split(' ').ToList();
                                string paramName = GetContent(str);
                                radioGroups.Add(paramName);

                                switch (GetTag(lowStr))
                                {
                                    case "rb":
                                        var sp = new StackPanel() { Orientation = Orientation.Horizontal,
                                            Margin = new Thickness(0, 10, 0, 0) };

                                        string text = "", value = "";

                                        foreach (string arg in args)
                                        {
                                            string[] info = arg.Split('=');

                                            if (info[0] == "value")
                                                value = info[1];
                                            else if (info[0] == "text")
                                                text = info[1];
                                        }

                                        var rb = (RadioButton)this.TryFindResource("standard_rb");
                                        rb.VerticalAlignment = VerticalAlignment.Center;
                                        rb.GroupName = paramName;

                                        rb.Checked += delegate
                                        {
                                            AppendData(paramName, value);
                                        };

                                        sp.Children.Add(rb);

                                        var tbl = (TextBlock)this.TryFindResource("radiobutton_tbl");
                                        tbl.Margin = new Thickness(15, 2, 0, 0);
                                        tbl.VerticalAlignment = VerticalAlignment.Center;
                                        tbl.Text = text.Replace("_", " ");
                                        sp.Children.Add(tbl);

                                        container.Children.Add(sp);

                                        break;
                                }
                            }

                            break;
                    }
                }
            }
        }

        private string GetTag(string str)
        {
            return str.Split('>')[0].Remove(0, 1);
        }

        private string GetContent(string str)
        {
            return str.Split('"')[1];
        }

        public bool IsValuesSet()
        {
            foreach(string group in radioGroups)
            {
                bool set = false;
             
                foreach(StackPanel sp in container.Children.OfType<StackPanel>()) 
                    foreach(RadioButton rb in sp.Children.OfType<RadioButton>())
                        if (rb.GroupName == group && rb.IsChecked == true)
                            set = true;

                if (!set)
                    return false;
            }

            return true;
        }
    }
}
