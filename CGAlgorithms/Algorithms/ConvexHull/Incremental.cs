using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class Incremental : Algorithm
    {
        public double is_Right(Point p, Point q, Point r)
        {
            double Px = p.X, Py = p.Y, Qx = q.X, Qy = q.Y, Rx = r.X, Ry = r.Y;
            double area = Px * (Qy - Ry) - Qx * (Py - Ry) + Rx * (Py - Qy);
            area /= 2;
            return area;
        }
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            if (points.Count <= 3)
            {
                outPoints = points;
                return;
            }
            List<Point> sorted_points = points.OrderBy(item => item.X).ThenBy(item => item.Y).ToList();
            List<Point> stack = new List<Point>();
            for (int i = 0; i < 5555; i++)
                stack.Add(new Point(-1, -1));
            int stack_pointer = 0;


            stack[stack_pointer++] = sorted_points[0];
            stack[stack_pointer++] = sorted_points[1];
            

            for (int i = 2; i < sorted_points.Count; i++)
            {
                while( (stack_pointer >= 2) &&  (is_Right(stack[stack_pointer - 1], stack[stack_pointer - 2], sorted_points[i]) >= 0) )
                {
                    stack_pointer--;
                }
                stack[stack_pointer++] = sorted_points[i];
            }

            sorted_points.Reverse();

            List<Point> stack_reversed = new List<Point>();
            for (int i = 0; i < 5555; i++)
                stack_reversed.Add(new Point(-1, -1));
            int stack_reversed_pointer = 0;
            stack_reversed[stack_reversed_pointer++] = sorted_points[0];
            stack_reversed[stack_reversed_pointer++] = sorted_points[1];

            for (int i = 2; i < sorted_points.Count; i++)
            {
                while ((stack_reversed_pointer >= 2) && (is_Right(stack_reversed[stack_reversed_pointer - 1], stack_reversed[stack_reversed_pointer - 2], sorted_points[i]) >= 0) )
                {
                    stack_reversed_pointer--;
                }
                stack_reversed[stack_reversed_pointer++] = sorted_points[i];
            }

            outPoints = new List<Point>();

            for(int i = 0; i < stack_pointer; i++)
            {
                if (outPoints.Contains(stack[i])==false)
                   outPoints.Add(stack[i]);
            }
            for (int i = 0; i < stack_reversed_pointer; i++)
            {
                if (outPoints.Contains(stack_reversed[i]) == false)
                    outPoints.Add(stack_reversed[i]);
            }

            foreach (var x in outPoints)
            {
                Console.WriteLine(x.X + " " + x.Y);
            }
        }

        public override string ToString()
        {
            return "Convex Hull - Incremental";
        }
    }
}
