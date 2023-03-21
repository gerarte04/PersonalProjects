using System;

namespace BezierCurve
{
    class Program
    {
        static void Main(string[] args)
        {
            var ds = new DrawingSpace(Constants.WindowResolution);

            ds.Run();
        }
    }
}
