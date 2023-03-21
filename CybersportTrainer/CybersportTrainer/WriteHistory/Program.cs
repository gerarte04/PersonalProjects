using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WriteHistory
{
    class Program
    {
        static void Main(string[] args)
        {
            //string filePath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + "\\bin\\Debug\\matches_history.txt";
            string filePath = Directory.GetCurrentDirectory();
            Console.WriteLine(filePath);
            Console.ReadKey();
        }
    }
}
