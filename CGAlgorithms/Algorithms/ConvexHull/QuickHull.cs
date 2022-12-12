using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class QuickHull : Algorithm
    {
        static List<Point> C_H;
        public  int is_Right(Point p, Point q, Point r)
        {
            double Px = p.X, Py = p.Y, Qx = q.X, Qy = q.Y, Rx = r.X, Ry = r.Y;
            double area = Px * (Qy - Ry) - Qx * (Py - Ry) + Rx * (Py - Qy);
            area /= 2;
            if (area < 0)
                return 1;
            else if (area > 0)
                return -1;
            else return 0;
        }

        public double Calc_dist(Point p1, Point p2,Point p)
        {
            return Math.Abs((p.Y - p1.Y) * (p2.X - p1.X) -
                       (p2.Y - p1.Y) * (p.X - p1.X));
        }

        public void solve(List<Point> points,  Point P1, Point P2,int direction)
        {
            int ch_idx = -1;
            double max_dist = 0;

            for(int i = 0; i< points.Count; i++)
            {
                double cur_dist = Calc_dist(P1, P2, points[i]);
                if(is_Right(P1,P2, points[i]) == direction && max_dist  < cur_dist)
                {
                    max_dist = cur_dist;
                    ch_idx = i;
                }
            }

            if(ch_idx == -1)
            {
                if(C_H.Contains(P1)==false)
                     C_H.Add(P1);
                if (C_H.Contains(P2) == false)
                    C_H.Add(P2);
                return;
            }

            solve(points, points[ch_idx],P1, -1  * is_Right(points[ch_idx], P1,P2));
            solve(points, points[ch_idx], P2, -1  * is_Right(points[ch_idx], P2,P1));

        }

        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {

            if (points.Count <= 3)
            {
                outPoints = points;
                return;
            }

            C_H = new List<Point>();
            int min_x = 0, min_y = 0, max_x = 0, max_y = 0;
            for(int i = 0 ; i < points.Count; i++)
            {
                if (points[min_x].X > points[i].X)
                    min_x = i;
                if (points[min_y].Y > points[i].Y)
                    min_y = i;
                if (points[max_x].X < points[i].X)
                    max_x = i;
                if (points[max_y].Y < points[i].Y)
                    max_y = i;
            }

            solve(points, points[min_x], points[min_y], -1);
            solve(points, points[max_x], points[max_y], -1);
            solve(points, points[min_x], points[max_y], -1);
            solve(points, points[max_x], points[min_y], 1);

            outPoints = C_H;

            foreach(var x in C_H)
            {
                Console.WriteLine(x.X+ " "+ x.Y);
            }
        }

        public override string ToString()
        {
            return "Convex Hull - Quick Hull";
        }
    }
}
