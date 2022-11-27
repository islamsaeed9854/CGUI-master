using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class ExtremeSegments : Algorithm
    {
        HelperMethods h = new HelperMethods();
        public double is_Right(Point p ,Point q ,Point r)
        {
            double Px = p.X , Py = p.Y, Qx = q.X , Qy =q.Y , Rx = r.X , Ry = r.Y;
            double area =  Px * (Qy - Ry ) - Qx * (Py - Ry) + Rx * (Py - Qy);
            area /= 2;
            return area ;
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
            Point first = new Point(0,0);
            Point second = new Point(0, 0); ;
            Dictionary<int, bool> taked = new Dictionary<int,bool>();
            Dictionary< Tuple<double, double>  , int > found = new Dictionary<Tuple<double, double>, int >();
            List<Point> newList = new List<Point>(points);
            points = new List<Point>();
            foreach(Point i in newList)
            {
                Tuple<double, double> t = new Tuple<double, double>(i.X, i.Y);
                if (found.ContainsKey(t))
                    continue;
                points.Add(i);
                found.Add(t,1);
            }
            bool end = false;
            int last_idx = -1;
            for (int i =0; i< points.Count; i++)
            {
                for(int j = 0; j < points.Count; j++)
                {
                    int cnt_r = 0 , cnt_l=0;
                    if (i == j)
                        continue;
                    for(int k = 0; k < points.Count; k++)
                    {
                        if (i == j || j == k || i == k)
                            continue;
                        if (is_Right(points[i], points[j], points[k]) < 0)
                        {
                            cnt_r++;
                        }
                        else cnt_l++;
                        if (HelperMethods.PointOnSegment(points[i], points[j], points[k] ) || HelperMethods.PointOnSegment(points[j], points[i], points[k]))
                        {
                            cnt_l += 100;
                            cnt_r += 100;
                            break;
                        }
                    }
                    if(cnt_r == 0 || cnt_l == 0)
                    {
                        first = new Point(points[i].X, points[i].Y);
                        second = new Point(points[j].X, points[j].Y);
                        end = true;
                        taked.Add(i, true);
                        taked.Add(j, true);
                        outPoints.Add(first);
                        outPoints.Add(second);
                        last_idx = j;
                        break;
                    }
                }
                if (end == true)
                    break;
            }
            while (true) {
                bool en = false;
                for (int i = 0; i < points.Count; i++)
                {
                    if (taked.ContainsKey(i))
                        continue;
                    
                        int cnt_r = 0, cnt_l = 0;
                    for (int j = 0; j < points.Count; j++)
                    {
                        if (i == j || last_idx == j || last_idx == i )
                            continue;

                        if (is_Right(second, points[i], points[j]) < 0)
                        {
                            cnt_r++;
                        }
                        else if (is_Right(second, points[i], points[j]) > 0) cnt_l++;
                        else {
                            if (HelperMethods.PointOnSegment(points[i] , points[j], second))
                            {
                                en = true;
                                taked.Add(i, true);
                                break;
                            }
                            
                        }
                    }
                    if (en == true)
                        break;
                    if (cnt_r == 0 || cnt_l == 0)
                    {
                        second = new Point(points[i].X, points[i].Y);
                        taked.Add(i, true);
                        outPoints.Add(points[i]);
                            last_idx = i;
                           en = true;
                        break;
                    }
                }
                if (en == false)
                    break;
            }
            
           
        }

        public override string ToString()
        {
            return "Convex Hull - Extreme Segments";
        }
    }
}
