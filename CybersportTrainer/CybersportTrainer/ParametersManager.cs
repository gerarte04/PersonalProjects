using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;

namespace CybersportTrainer
{
    class ParametersManager
    {
        Dictionary<string, string> parameters;

        string path;

        Window owner;

        public ParametersManager(Window owner, string path)
        {
            this.owner = owner;
            this.path = path;

            parameters = new Dictionary<string, string>();
            ReadParameters();
        }

        public void ChangeParametersPath(string path)
        {
            SaveChanges();

            this.path = path;
            ReadParameters();
        }

        private void ReadParameters()
        {
            using (StreamReader sr = new StreamReader(path))
            {
                string str;

                while ((str = sr.ReadLine()) != null)
                {
                    string[] info = str.Split(':');
                    parameters[info[0]] = info[1];
                }
            }
        }

        public void SaveChanges()
        {
            using (StreamWriter sw = new StreamWriter(path, false))
            {
                foreach (string k in parameters.Keys)
                    sw.WriteLine(k + ":" + parameters[k]);
            }
        }

        public void MakeFocusActions()
        {
            UploadTaskManager();
        }

        private void UploadTaskManager()
        {
            if(parameters["FirstOpen"] == "true")
            {
                var w = new TaskManagerWindow(CurrentStatus.FirstEntry);
                w.Owner = owner;
                w.ShowInTaskbar = false;
                w.Visibility = Visibility.Visible;
                w.ShowDialog();

                parameters["FirstOpen"] = "false";
                SaveChanges();
            }
        }
    }
}
