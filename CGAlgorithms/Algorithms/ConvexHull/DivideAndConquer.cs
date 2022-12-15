using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel;
using System.Security.Cryptography;

namespace CGAlgorithms.Algorithms.ConvexHull

{
    public class SortbyAbscissa : IComparer<Point>
    {
        // sorting the points according to the x-coordinate

        public int Compare(Point a, Point b)
        {
            if (a.X < b.X || a.X == b.X && a.Y < b.Y)
                return -1;
            else if (a.X > b.X || a.X == b.X && a.Y > b.Y)
                return 1;
            else
                return 0;
        }
    }


    public class SortAntiClockwise : IComparer<Point>
    {
        // sorting the points in anti-clockwise order

        Point MidPoint;

        public void SetMidPoint(Point mid)
        {
            MidPoint = mid;
        }

        public int Quad(Point p)
        {
            if (p.X >= 0 && p.Y >= 0)
                return 1;
            if (p.X <= 0 && p.Y >= 0)
                return 2;
            if (p.X <= 0 && p.Y <= 0)
                return 3;
            else
                return 4;
        }

        public double RadiansToDegrees(double radians)
        {
            double degrees = (180.0 / Math.PI) * radians;
            return degrees;
        }

        public int Compare(Point a, Point b)
        {
            double a1 = (RadiansToDegrees(Math.Atan2(a.X - MidPoint.X, a.Y - MidPoint.Y)) + 360) % 360;
            double a2 = (RadiansToDegrees(Math.Atan2(b.X - MidPoint.X, b.Y - MidPoint.Y)) + 360) % 360;
            return (int)(a2 - a1);
        }


    }



    public class DivideAndConquer : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            //Form1 form1 = new Form1();
            //Form1 form2 = new Form1();

            //form1.Draw(points);

            //Application.Run(form1);


            SortbyAbscissa sortbyAbscissa = new SortbyAbscissa();
            points.Sort(sortbyAbscissa);

            RemoveDuplicates(ref points);

            if (points.Count < 4)
            {
                outPoints = points;
                return;
            }

            outPoints = FindConvexHull(points);

            // Print(ref outPoints);

            //form2.Draw(outPoints);

            //Application.Run(form2);
        }
         
        public void Print(ref List<Point> outpoints)
        {
            StreamWriter sw = new StreamWriter("D:\\test.txt");
            foreach (Point p in outpoints)
                sw.WriteLine(p.X + "    " + p.Y);
            sw.WriteLine("Done");
            sw.Close();
        }

        public void RemoveDuplicates(ref List<Point> points)
        {
            for (int i = 1; i < points.Count; i++)
            {
                if (points[i].X == points[i - 1].X && points[i].Y == points[i - 1].Y)
                {
                    points.RemoveAt(i);
                    i--;
                }
            }
        }


        public List<Point> Divide_and_conquer(List<Point> points)
        {
            if (points.Count <= 5)
                return FindConvexHull(points);

            List<Point> LeftPoints, RightPoints, LeftConvexHull, RightConvexHull;
            LeftPoints = points.GetRange(0, points.Count / 2);
            RightPoints = points.GetRange(points.Count / 2, points.Count - LeftPoints.Count);
            LeftConvexHull = Divide_and_conquer(LeftPoints);
            RightConvexHull = Divide_and_conquer(RightPoints);
            return Merge(LeftConvexHull, RightConvexHull);
        }


        public List<Point> Merge(List<Point> LCH, List<Point> RCH)
        {

            // finding the rightmost point of left convex hull
            int LCH_RMP = 0;
            for (int i = 1; i < LCH.Count; i++)
                if (LCH[i].X > LCH[LCH_RMP].X || LCH[i].X == LCH[LCH_RMP].X && LCH[i].Y > LCH[LCH_RMP].Y)
                    LCH_RMP = i;

            // finding the leftmost point of right convex hull
            int RCH_LMP = 0;
            for (int i = 1; i < RCH.Count; i++)
                if (RCH[i].X < RCH[RCH_LMP].X || RCH[i].X == RCH[RCH_LMP].X && RCH[i].Y > RCH[RCH_LMP].Y)
                    RCH_LMP = i;


            // finding the upper tangent between the two Convex Polygons
            int LCH_Upper = LCH_RMP, RCH_Upper = RCH_LMP;
            bool flag = false;
            while (!flag)
            {
                flag = true;
                while (HelperMethods.CheckTurn(new Line(RCH[RCH_Upper], LCH[LCH_Upper]), LCH[(LCH_Upper + 1) % LCH.Count]) == Enums.TurnType.Right)
                {
                    LCH_Upper = (LCH_Upper + 1) % LCH.Count;
                    flag = false;
                }

                while (HelperMethods.CheckTurn(new Line(LCH[LCH_Upper], RCH[RCH_Upper]), RCH[(RCH.Count + RCH_Upper - 1) % RCH.Count]) == Enums.TurnType.Left)
                {
                    RCH_Upper = (RCH.Count + RCH_Upper - 1) % RCH.Count;
                    flag = false;
                }
            }



            // finding the lower tangent between the two Convex Polygons
            int LCH_Lower = LCH_RMP, RCH_Lower = RCH_LMP;
            flag = false;
            while (!flag)
            {
                flag = true;
                while (HelperMethods.CheckTurn(new Line(RCH[RCH_Lower], LCH[LCH_Lower]), LCH[(LCH.Count + LCH_Lower - 1) % LCH.Count]) == Enums.TurnType.Left)
                {
                    LCH_Lower = (LCH.Count + LCH_Lower - 1) % LCH.Count;
                    flag = false;
                }

                while (HelperMethods.CheckTurn(new Line(LCH[LCH_Lower], RCH[RCH_Lower]), RCH[(RCH_Lower + 1) % RCH.Count]) == Enums.TurnType.Right)
                {
                    RCH_Lower = (RCH_Lower + 1) % RCH.Count;
                    flag = false;
                }
            }

            // delete the interior chains of LCH & RCH
            List<Point> Merged_CH = new List<Point>();
            int indx = LCH_Upper, LCH_Upper_Merged, LCH_Lower_Merged, RCH_Upper_Merged, RCH_Lower_Merged;

            Merged_CH.Add(LCH[LCH_Upper]);
            LCH_Upper_Merged = 0;
            while (indx != LCH_Lower)
            {
                indx = (indx + 1) % LCH.Count;
                Merged_CH.Add(LCH[indx]);
            }
            LCH_Lower_Merged = Merged_CH.Count - 1;

            indx = RCH_Lower;
            Merged_CH.Add(RCH[RCH_Lower]);
            RCH_Lower_Merged = Merged_CH.Count - 1;
            while (indx != RCH_Upper)
            {
                indx = (indx + 1) % RCH.Count;
                Merged_CH.Add(RCH[indx]);
            }
            RCH_Upper_Merged = Merged_CH.Count - 1;


            // delete any of the tangents points if the point lies on the same segment between its next point and its previous
            if (HelperMethods.PointOnSegment(Merged_CH[LCH_Upper_Merged], Merged_CH[(Merged_CH.Count + LCH_Upper_Merged - 1) % Merged_CH.Count], Merged_CH[(LCH_Upper_Merged + 1) % Merged_CH.Count]))
            {
                Merged_CH.RemoveAt(LCH_Upper_Merged);
                if (LCH_Lower_Merged > LCH_Upper_Merged) LCH_Lower_Merged--;
                if (RCH_Upper_Merged > LCH_Upper_Merged) RCH_Upper_Merged--;
                if (RCH_Lower_Merged > LCH_Upper_Merged) RCH_Lower_Merged--;
            }

            if (HelperMethods.PointOnSegment(Merged_CH[LCH_Lower_Merged], Merged_CH[(Merged_CH.Count + LCH_Lower_Merged - 1) % Merged_CH.Count], Merged_CH[(LCH_Lower_Merged + 1) % Merged_CH.Count]))
            {
                Merged_CH.RemoveAt(LCH_Lower_Merged);
                if (LCH_Upper_Merged > LCH_Lower_Merged) LCH_Upper_Merged--;
                if (RCH_Upper_Merged > LCH_Lower_Merged) RCH_Upper_Merged--;
                if (RCH_Lower_Merged > LCH_Lower_Merged) RCH_Lower_Merged--;
            }



            if (HelperMethods.PointOnSegment(Merged_CH[RCH_Upper_Merged], Merged_CH[(Merged_CH.Count + RCH_Upper_Merged - 1) % Merged_CH.Count], Merged_CH[(RCH_Upper_Merged + 1) % Merged_CH.Count]))
            {
                Merged_CH.RemoveAt(RCH_Upper_Merged);
                if (LCH_Upper_Merged > RCH_Upper_Merged) LCH_Upper_Merged--;
                if (LCH_Lower_Merged > RCH_Upper_Merged) LCH_Lower_Merged--;
                if (RCH_Lower_Merged > RCH_Upper_Merged) RCH_Lower_Merged--;
            }

            if (HelperMethods.PointOnSegment(Merged_CH[RCH_Lower_Merged], Merged_CH[(Merged_CH.Count + RCH_Lower_Merged - 1) % Merged_CH.Count], Merged_CH[(RCH_Lower_Merged + 1) % Merged_CH.Count]))
            {
                Merged_CH.RemoveAt(RCH_Lower_Merged);
                if (LCH_Upper_Merged > RCH_Lower_Merged) LCH_Upper_Merged--;
                if (LCH_Lower_Merged > RCH_Lower_Merged) LCH_Lower_Merged--;
                if (RCH_Upper_Merged > RCH_Lower_Merged) RCH_Upper_Merged--;
            }

        
            return Merged_CH;


        }


        public List<Point> FindConvexHull(List<Point> points)
        {

            JarvisMarch jarvismarch = new JarvisMarch();
            List<Point> outpoints = new List<Point>();
            List<Line> outlines = new List<Line>();
            List<Polygon> outpolygons = new List<Polygon>();
            jarvismarch.Run(points, null, null, ref outpoints, ref outlines, ref outpolygons);


            // sorting the outpoints in anti-clockwise order
            Point midPoint = new Point(0, 0);
            for (int i = 0; i < outpoints.Count; i++)
            {
                midPoint.X += outpoints[i].X;
                midPoint.Y += outpoints[i].Y;
            }
            midPoint.X /= outpoints.Count;
            midPoint.Y /= outpoints.Count;

            SortAntiClockwise sortAntiClockwise = new SortAntiClockwise();
            sortAntiClockwise.SetMidPoint(midPoint);
            outpoints.Sort(sortAntiClockwise);


            return outpoints;
        }


        public override string ToString()
        {
            return "Convex Hull - Divide & Conquer";
        }

    }
}
