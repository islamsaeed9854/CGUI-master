using CGUtilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class ExtremePoints : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            for (int i = 0; i < points.Count; i++)
                for (int j = 0; j < points.Count; j++)
                    if (j != i)
                        for (int k = 0; k < points.Count; k++)
                            if (k != j && k != i)
                                for (int l = 0; l < points.Count; l++)
                                    if (l != k && l != j && l != i)
                                        if (HelperMethods.PointInTriangle(points[l], points[i], points[j], points[k]) != Enums.PointInPolygon.Outside)
                                        {
                                            points.Remove(points[l]);

                                            if (Shiftable(l, i)) i -= 1;
                                            if (Shiftable(l, j)) j -= 1;
                                            if (Shiftable(l, k)) k -= 1;
                                        }

            outPoints = points;
        }


        public override string ToString()
        {
            return "Convex Hull - Extreme Points";
        }

        private bool Shiftable(int anchor, int index)
        {
            return index >= anchor;
        }
    }
}
