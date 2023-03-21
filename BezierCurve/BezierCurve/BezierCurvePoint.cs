using System;
using System.Collections.Generic;
using System.Text;
using SFML.System;

namespace BezierCurve
{
    class ObjectPoint
    {
        public Vector2f Position { get; set; }

        public ObjectPoint(Vector2f pos)
        {
            Position = pos;
        }
    }
}
