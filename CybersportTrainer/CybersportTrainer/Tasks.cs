using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CybersportTrainer
{
    enum TaskType { TrainingTime = 1, Kills, Headkills, ARKills, SRKills, MVP, None }

    class TaskInfo
    {
        public TaskType Task { get; private set; } = TaskType.None;
        public double Done { get; set; } = 0;
        public double Need { get; private set; } = 0;

        public TaskInfo(TaskType t, double d, double n)
        {
            Task = t;
            Done = d;
            Need = n;
        }
    }

    class Tasks
    {
        public List<TaskInfo> taskInfos { get; private set; }

        public Tasks()
        {
            taskInfos = new List<TaskInfo>();
        }

        public void UpdateTasksFromPath()
        {
            string filePath = Directory.GetCurrentDirectory() + "\\daily_tasks.txt";

            using (StreamReader sr = new StreamReader(filePath))
            {
                string str;
                string[] info;

                while((str = sr.ReadLine()) != null)
                {
                    info = str.Split(' ');

                    bool isExists = false;
                    TaskType t = (TaskType)Enum.Parse(typeof(TaskType), info[0]);

                    foreach (TaskInfo ti in taskInfos)
                    {
                        if (ti.Task == t)
                        {
                            ti.Done = Convert.ToDouble(info[1]);
                            isExists = true;
                        }
                    }

                    if (!isExists)
                        taskInfos.Add(new TaskInfo(t, Convert.ToDouble(info[1]), Convert.ToDouble(info[2])));
                }
            }
        }

        public static string GetTaskIconPath(TaskType t)
        {
            using (StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + "\\tasks_icons.txt"))
            {
                string str;
                string[] info;

                while ((str = sr.ReadLine()) != null)
                {
                    info = str.Split(':');
                    if(t == (TaskType)Enum.Parse(typeof(TaskType), info[0]))
                    {
                        return "pack://application:,,,/res/" + info[1];
                    }
                }

                return null;
            }
        }

        public static string GetTaskPhrase(TaskType t)
        {
            using (StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + "\\tasks_phrases.txt"))
            {
                string str;
                string[] info;

                while ((str = sr.ReadLine()) != null)
                {
                    info = str.Split(':');
                    if (t == (TaskType)Enum.Parse(typeof(TaskType), info[0]))
                    {
                        return info[1];
                    }
                }

                return null;
            }
        }
    }
}
