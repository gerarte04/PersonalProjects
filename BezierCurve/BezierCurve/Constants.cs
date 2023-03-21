using System;
using System.Collections.Generic;
using System.Text;

namespace BezierCurve
{
    internal static class Constants
    {
        public static SFML.System.Vector2i WindowResolution { get; } = new SFML.System.Vector2i(800, 600);

        public static float ControlPointRadius { get; } = 4f;
        public static float RelativePosOffset { get; } = 0.05f;
        public static SFML.System.Vector2f NullVector { get; } = new SFML.System.Vector2f(-1, -1); 
    }
}
