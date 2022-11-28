using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class ExtremeSegments : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            outPoints = new List<Point>();

            if (points.Count == 1)
            {
                outPoints = points;

                return;
            }

            for (int i = 0; i < points.Count; i++)
                for (int j = i + 1; j < points.Count; j++)
                    if (points[i].Equals(points[j])) points.RemoveAt(j);

            for (int i = 0; i < points.Count; i++)
                for (int j = 0; j < points.Count; j++)
                    if (j != i)
                    {
                        Enums.TurnType temp = Enums.TurnType.Left;
                        bool changed = false;

                        int k;
                        for (k = 0; k < points.Count; k++)
                            if (k != j && k != i)
                                if (!changed)
                                {
                                    temp = HelperMethods.CheckTurn(new Line(points[i], points[j]), points[k]);
                                    changed = true;
                                }
                                else if (!temp.Equals(HelperMethods.CheckTurn(new Line(points[i], points[j]), points[k])))
                                    if (HelperMethods.CheckTurn(new Line(points[i], points[j]), points[k]) != Enums.TurnType.Colinear) break;
                                    else if (!HelperMethods.PointOnSegment(points[k], points[i], points[j])) break;

                        if (k == points.Count)
                        {
                            if (!outPoints.Contains(points[i])) outPoints.Add(points[i]);
                            if (!outPoints.Contains(points[j])) outPoints.Add(points[j]);
                        }
                    }
        }

        public override string ToString()
        {
            return "Convex Hull - Extreme Segments";
        }
    }
}
