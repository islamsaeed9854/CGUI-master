using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    
    public class GrahamScan : Algorithm
    {
        public double is_Right(Point p, Point q, Point r)
        {
            double Px = p.X, Py = p.Y, Qx = q.X, Qy = q.Y, Rx = r.X, Ry = r.Y;
            double area = Px * (Qy - Ry) - Qx * (Py - Ry) + Rx * (Py - Qy);
            area /= 2;
            return area;
        }
        public double get_angle(Point p1, Point p2)
        {
            double xDiff = p2.X - p1.X;
            double yDiff = p2.Y - p1.Y;
            return Math.Atan2(yDiff, xDiff) * 180.0 / Math.PI;
        }
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            outPoints = new List<Point>();
            if (points.Count == 1)
            {
                outPoints.Add(points[0]);
                return;
            }
            else if (points.Count == 2)
            {
                outPoints.Add(points[0]);
                outPoints.Add(points[1]);
                return;
            }
            double min_y = 100000000;
            Point extrme = new Point(0,0);
            int idx_min = -1,i = 0;
            foreach(Point p in points)
            {
                if(p.Y < min_y)
                {
                    min_y = p.Y;
                    extrme = p;
                    idx_min = i;
                }
                i++;
            }
            points.RemoveAt(idx_min);
            outPoints.Add(extrme);
            List<KeyValuePair< double ,int >> l = new List<KeyValuePair<double, int>>();
            i = 0;
            foreach (Point p in points)
            {
                KeyValuePair<double, int> cur = new KeyValuePair<double, int>( get_angle(extrme, p),i);
                l.Add(cur);
                i++;
            }
            l.Sort((x, y) => x.Key.CompareTo(y.Key));
            int idx = 1;
            outPoints.Add(new Point(100000000, min_y));
            foreach (var c_p in l)
            {
                Point p = outPoints[idx];
                Point q = outPoints[idx-1];
                Line ln = new Line(p, q);
                if((HelperMethods.CheckTurn(ln, points[c_p.Value]))==Enums.TurnType.Left)
                {
                    outPoints.RemoveAt(idx);
                    outPoints.Add(points[c_p.Value]);
                }
                else
                {
                    if (outPoints[idx].X == 100000000)
                    {
                        outPoints.RemoveAt(idx);
                        idx--;
                    }
                    outPoints.Add(points[c_p.Value]);
                    idx++;
                }
            }
            foreach(var x in outPoints)
            {
                Console.WriteLine(x.X + " " + x.Y);
            }
        }

        public override string ToString()
        {
            return "Convex Hull - Graham Scan";
        }
    }
}
