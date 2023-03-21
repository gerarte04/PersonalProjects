using System;
using System.Collections.Generic;
using System.Text;
using SFML.Window;
using SFML.Graphics;
using SFML.System;

namespace BezierCurve
{
    class DrawingSpace
    {
        RenderWindow window;

        List<ObjectPoint> controlPoints;
        List<Vector2f> bezierCurvePoints;
        ObjectPoint actionPoint;

        readonly int MaxPointsCount;

        bool isControlLinesVisible = true;
        bool linesMode = false;

        public DrawingSpace(Vector2i resolution)
        {
            controlPoints = new List<ObjectPoint>();
            bezierCurvePoints = new List<Vector2f>();
            MaxPointsCount = (int)(1 / Constants.RelativePosOffset) + 1;
            actionPoint = null;

            window = new RenderWindow(new VideoMode((uint)Constants.WindowResolution.X, (uint)Constants.WindowResolution.Y), "Bezier Curve");
            (window as Window).SetFramerateLimit(60);

            window.Closed += OnClose;
            window.KeyPressed += new EventHandler<KeyEventArgs>(KeyPressed);
            window.MouseButtonPressed += new EventHandler<MouseButtonEventArgs>(MouseButtonPressed);
            window.MouseButtonReleased += new EventHandler<MouseButtonEventArgs>(MouseButtonReleased);
            window.MouseMoved += new EventHandler<MouseMoveEventArgs>(MouseMoved);
        }

        public void Run()
        {
            while (window.IsOpen)
            {
                window.Clear();

                window.DispatchEvents();

                DrawCurve(window);
                if(isControlLinesVisible)
                    DrawControlLines(window);

                window.Display();
            }
        }

        private void DrawCurve(RenderWindow window)
        {
            var pt = (linesMode) ? PrimitiveType.Lines : PrimitiveType.Points;

            for (int i = 0; i < bezierCurvePoints.Count - 1; i++)
            {
                Vertex[] line = new Vertex[2];
                line[0] = new Vertex(bezierCurvePoints[i], Color.White);
                line[1] = new Vertex(bezierCurvePoints[i + 1], Color.White);
                window.Draw(line, pt);
            }
        }

        private void DrawControlLines(RenderWindow window)
        {
            foreach(ObjectPoint cp in controlPoints)
            {
                var circle = new CircleShape(Constants.ControlPointRadius);
                circle.Position = new Vector2f(cp.Position.X - Constants.ControlPointRadius, cp.Position.Y - Constants.ControlPointRadius);
                circle.FillColor = Color.Red;
                window.Draw(circle);
            }

            for(int i = 0; i < controlPoints.Count - 1; i++)
            {
                Vertex[] line = new Vertex[2];
                line[0] = new Vertex(controlPoints[i].Position, Color.Red);
                line[1] = new Vertex(controlPoints[i + 1].Position, Color.Red);
                window.Draw(line, PrimitiveType.Lines);
            }
        }

        private void CreateCurve(int pointsCount)
        {
            bezierCurvePoints.Clear();

            for(int i = 0; i < pointsCount; i++)
            {
                CreateNewPoint(i * Constants.RelativePosOffset);
            }
        }

        private void CreateNewPoint(float relPos)
        {
            if (controlPoints.Count >= 2)
            {
                List<Vector2f> tempPoints = new List<Vector2f>();
                foreach (ObjectPoint op in controlPoints)
                    tempPoints.Add(op.Position);

                while (tempPoints.Count > 1)
                {
                    var newTempPoints = new List<Vector2f>();

                    for (int i = 0; i < tempPoints.Count - 1; i++)
                    {
                        float x = tempPoints[i].X + (tempPoints[i + 1].X - tempPoints[i].X) * relPos;
                        float y = tempPoints[i].Y + (tempPoints[i + 1].Y - tempPoints[i].Y) * relPos;
                        newTempPoints.Add(new Vector2f(x, y));
                    }

                    tempPoints = newTempPoints;
                }

                bezierCurvePoints.Add(tempPoints[0]);
            }
        }

        private ObjectPoint GetPressedControlPoint(int mX, int mY)
        {
            foreach(ObjectPoint p in controlPoints)
            {
                if (Math.Abs(p.Position.X - mX) <= Constants.ControlPointRadius && Math.Abs(p.Position.Y - mY) <= Constants.ControlPointRadius)
                    return p;
            }

            return null;
        }

        private void KeyPressed(object sender, KeyEventArgs e)
        {
            switch (e.Code)
            {
                case Keyboard.Key.Num1:
                    CreateCurve(MaxPointsCount);
                    break;
                case Keyboard.Key.Num2:
                    isControlLinesVisible = !isControlLinesVisible;
                    break;
                case Keyboard.Key.Num3:
                    linesMode = !linesMode;
                    break;
                case Keyboard.Key.A:
                    if(bezierCurvePoints.Count < MaxPointsCount)
                        CreateCurve(bezierCurvePoints.Count + 1);
                    break;
                case Keyboard.Key.S:
                    if (bezierCurvePoints.Count > 0)
                        CreateCurve(bezierCurvePoints.Count - 1);
                    break;
                case Keyboard.Key.Delete:
                    bezierCurvePoints.Clear();
                    break;
                case Keyboard.Key.Backspace:
                    if (controlPoints.Count > 0)
                    {
                        controlPoints.Remove(controlPoints[controlPoints.Count - 1]);
                        CreateCurve(bezierCurvePoints.Count);
                    }
                    break;
                default:
                    break;
            }
        }

        private void MouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            if(e.Button == Mouse.Button.Left)
            {
                if (isControlLinesVisible)
                {
                    ObjectPoint cPoint = GetPressedControlPoint(e.X, e.Y);

                    if (cPoint != null)
                    {
                        actionPoint = cPoint;
                    }
                    else
                    {
                        controlPoints.Add(new ObjectPoint(new Vector2f(e.X, e.Y)));
                        CreateCurve(bezierCurvePoints.Count);
                    }
                }
            }
        }

        private void MouseButtonReleased(object sender, MouseButtonEventArgs e)
        {
            actionPoint = null;
        }

        private void MouseMoved(object sender, MouseMoveEventArgs e)
        {
            if(actionPoint != null)
            {
                actionPoint.Position = new Vector2f(e.X, e.Y);
                CreateCurve(bezierCurvePoints.Count);
            }
        }

        static void OnClose(object sender, EventArgs e)
        {
            RenderWindow window = sender as RenderWindow;
            window.Close();
        }
    }
}

// 1 - create full
// 2 - show curve/points
// 3 - show/hide control points & lines
// a, s - add/delete point
// Delete - delete full
