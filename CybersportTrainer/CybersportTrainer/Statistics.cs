using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CybersportTrainer
{
    public enum StatParam { TotalKills = 1, TotalHeadkills, TotalHits, TotalShots, TotalDeaths, TotalMatches, TotalWins };

    public enum StatsType { AllStats, AWPStats, ARStats, PistolStats }
    class Statistics
    {
        private Dictionary<StatParam, double> MainStats;

        public double KD { get { return MainStats[StatParam.TotalKills] / MainStats[StatParam.TotalDeaths]; } }
        public double TotalKills { get { return MainStats[StatParam.TotalKills]; } }
        public double HeadkillsPercent { get { return MainStats[StatParam.TotalHeadkills] / MainStats[StatParam.TotalKills] * 100; } }
        public double Accuracy { get { return MainStats[StatParam.TotalHits] / MainStats[StatParam.TotalShots] * 100; } }
        public double TotalMatches { get { return MainStats[StatParam.TotalMatches]; } }
        public double WinsPercent { get { return MainStats[StatParam.TotalWins] / MainStats[StatParam.TotalMatches] * 100; } }
        public double ShotsForKill { get { return MainStats[StatParam.TotalShots] / MainStats[StatParam.TotalKills]; } }

        public Statistics()
        {
            MainStats = new Dictionary<StatParam, double>();

            foreach (StatParam sp in Enum.GetValues(typeof(StatParam))) 
            {
                MainStats.Add(sp, 0);
            }
        }

        public void UpdateStatsFromPath (string filePath)
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                string[] info;
                string str;

                while((str = sr.ReadLine()) != null)
                {
                    info = str.Split(' ');
                    MainStats[(StatParam)Enum.Parse(typeof(StatParam), info[0])] = Convert.ToDouble(info[1]);
                }
            }
        }

        public static string GetStatsImagePath(StatsType st)
        {
            using (StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + "\\stats_images.txt"))
            {
                string str;
                string[] info;

                while ((str = sr.ReadLine()) != null)
                {
                    info = str.Split(':');
                    if (st == (StatsType)Enum.Parse(typeof(StatsType), info[0]))
                    {
                        return "pack://application:,,,/res/" + info[1];
                    }
                }

                return null;
            }
        }

        public static string GetStatsWName(StatsType st)
        {
            using (StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + "\\stats_wnames.txt"))
            {
                string str;
                string[] info;

                while ((str = sr.ReadLine()) != null)
                {
                    info = str.Split(':');
                    if (st == (StatsType)Enum.Parse(typeof(StatsType), info[0]))
                    {
                        return info[1];
                    }
                }

                return null;
            }
        }
    }
}
