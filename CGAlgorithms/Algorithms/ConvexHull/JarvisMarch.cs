using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class JarvisMarch : Algorithm
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
        public double convert(Point p1, Point p2)
        {
            return 180 + (180 - Math.Abs(get_angle(p1,p2)));
        }

        public double dotProduct(Point v1, Point v2)
        {
            return (v1.X * v2.X) + (v1.Y * v2.Y);
        }

        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            outPoints = new List<Point>();
            if (points.Count <= 3)
            {
                outPoints = points;
                return;
            }
            double min_y = 100000000;
            Point extrme = new Point(0, 0);
            int idx_min = -1, i = 0;
            foreach (Point p in points)
            {
                if (p.Y < min_y)
                {
                    min_y = p.Y;
                    extrme = p;
                    idx_min = i;
                }
                i++;
            }
            outPoints.Add(extrme);
            Line fr_ln = new Line(new Point(0,0), new Point(0,0) );
            Line sc_ln = new Line(new Point(0, 0), new Point(0, 0));
            while ( (outPoints.Count == 1)  || (outPoints[0] != outPoints[outPoints.Count-1]) )
            {
                double max_ang = -100000000;
                Point will_take = new Point(0,0);
                double MagnitudeV1 = 0;
                if (outPoints.Count == 1)
                {
                    Point Point_ex = new Point(extrme.X - 10, extrme.Y);
                    fr_ln = new Line(extrme, new Point(extrme.X - 10, extrme.Y));
                    MagnitudeV1 = Math.Sqrt( Math.Pow(extrme.Y - Point_ex.Y, 2) +
                     Math.Pow(extrme.X - Point_ex.X, 2));
                }
                else
                {
                    fr_ln = new Line(outPoints[outPoints.Count -1 ], outPoints[outPoints.Count - 2]);
                    MagnitudeV1 = Math.Sqrt( Math.Pow(outPoints[outPoints.Count - 1].Y - outPoints[outPoints.Count - 2].Y, 2) +
                  Math.Pow(outPoints[outPoints.Count - 1].X - outPoints[outPoints.Count - 2].X, 2));
                }
                var fr_vector = HelperMethods.GetVector(fr_ln);
              

                foreach (Point p in points)
                {
                    sc_ln = new Line(extrme,p);
                    double MagnitudeV2 = Math.Sqrt(Math.Pow(extrme.Y - p.Y, 2) + Math.Pow(extrme.X - p.X , 2));
                    var sc_vector = HelperMethods.GetVector(sc_ln);
                    double DotProduct = dotProduct(fr_vector , sc_vector);
                    double ang = Math.Acos(DotProduct / (MagnitudeV1 * MagnitudeV2));
                    ang = (180 / Math.PI) * ang;
                    if(ang > max_ang)
                    {
                        max_ang = ang;
                        will_take = p;
                    }
                    else if (ang == max_ang)
                    {
                        if(HelperMethods.PointOnSegment(p,extrme , will_take)==false)
                        {
                            will_take = p;
                        }
                    }
                    
                }
                outPoints.Add(will_take);
                extrme = will_take;
            }
            outPoints.RemoveAt(outPoints.Count - 1);
            foreach (var x in outPoints)
            {
                Console.WriteLine(x.X + " " + x.Y);
            }
        }

        public override string ToString()
        {
            return "Convex Hull - Jarvis March";
        }
    }
}
