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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CybersportTrainer
{
    /// <summary>
    /// Логика взаимодействия для GuidePage.xaml
    /// </summary>
    public partial class GuidePage : Page
    {
        TextBlock title;
        public GuidePage(string guidePath)
        {
            InitializeComponent();

            title = (TextBlock)this.TryFindResource("title_tbl");
            container.Children.Add(title);

            AddGuideObjects(guidePath);
        }

        private void AddGuideObjects(string guidePath)
        {
            using (StreamReader sr = new StreamReader(guidePath))
            {
                string str;
                while ((str = sr.ReadLine()) != null)
                {
                    string tag = str.Split('>')[0].Remove(0, 1);
                    string content = str.Split('"')[1];
                    switch (tag)
                    {
                        case "title":
                            title.Text = content;
                            break;

                        case "h":
                            TextBlock header = (TextBlock)this.TryFindResource("header_tbl");
                            FillTextBlock(header, content);
                            container.Children.Add(header);
                            break;

                        case "par":
                            TextBlock paragraph = (TextBlock)this.TryFindResource("paragraph_tbl");
                            FillTextBlock(paragraph, content);
                            container.Children.Add(paragraph);
                            break;

                        case "image":
                            string path = Directory.GetCurrentDirectory() + "/guides/images/" + content;
                            string extension = System.IO.Path.GetExtension(path);

                            if (extension == ".gif" || extension == ".mp4")
                            {
                                var media = (MediaElement)this.TryFindResource("standard_media");
                                media.Source = new Uri(path, UriKind.RelativeOrAbsolute);
                                container.Children.Add(media);
                            }
                            else if (extension == ".jpg" || extension == ".jpeg" || extension == ".png")
                            {
                                var image = (Image)this.TryFindResource("standard_image");
                                image.Source = new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute));
                                container.Children.Add(image);
                            }
                            break;
                    }
                }
            }

            var endSeparator = new Separator() { Height = 25, Background = Brushes.Transparent };
            container.Children.Add(endSeparator);
        }

        private void FillTextBlock(TextBlock tb, string content)
        {
            string[] inlines = content.Split('/');
            for (int i = 0; i < inlines.Length; i += 2)
            {
                tb.Inlines.Add(new Run() { Text = inlines[i] });
                if (i != inlines.Length - 1)
                    tb.Inlines.Add(new Run() { Text = inlines[i + 1] + " ", FontStyle = FontStyles.Italic, Foreground = Brushes.LightGray });
            }
        }

        private void myMediaEnded(object sender, EventArgs e)
        {
            (sender as MediaElement).Position = TimeSpan.FromMilliseconds(1);
            (sender as MediaElement).Play();

        }

        private void back_b_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
