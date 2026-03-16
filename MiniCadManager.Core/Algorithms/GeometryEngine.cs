using System;
using System.Collections.Generic;
using System.Linq;
using MiniCadManager.Core.Models;

namespace MiniCadManager.Core.Algorithms
{
    public class BoundingBox
    {
        public double MinX { get; set; }
        public double MaxX { get; set; }
        public double MinY { get; set; }
        public double MaxY { get; set; }

        public override string ToString() => $"X:[{MinX:F2},{MaxX:F2}] Y:[{MinY:F2},{MaxY:F2}]";
    }

    public static class GeometryEngine
    {
        public static BoundingBox CalculateBoundingBox(IEnumerable<CadObject> objects)
        {
            if (!objects.Any()) return new BoundingBox();

            double minX = double.MaxValue;
            double maxX = double.MinValue;
            double minY = double.MaxValue;
            double maxY = double.MinValue;

            foreach (var obj in objects)
            {
                if (obj.Type.Equals("Line", StringComparison.OrdinalIgnoreCase))
                {
                    minX = Math.Min(minX, Math.Min(obj.X, obj.EndX));
                    maxX = Math.Max(maxX, Math.Max(obj.X, obj.EndX));
                    minY = Math.Min(minY, Math.Min(obj.Y, obj.EndY));
                    maxY = Math.Max(maxY, Math.Max(obj.Y, obj.EndY));
                }
                else if (obj.Type.Equals("Circle", StringComparison.OrdinalIgnoreCase))
                {
                    minX = Math.Min(minX, obj.X - obj.Radius);
                    maxX = Math.Max(maxX, obj.X + obj.Radius);
                    minY = Math.Min(minY, obj.Y - obj.Radius);
                    maxY = Math.Max(maxY, obj.Y + obj.Radius);
                }
                else
                {
                    minX = Math.Min(minX, obj.X);
                    maxX = Math.Max(maxX, obj.X);
                    minY = Math.Min(minY, obj.Y);
                    maxY = Math.Max(maxY, obj.Y);
                }
            }

            return new BoundingBox { MinX = minX, MaxX = maxX, MinY = minY, MaxY = maxY };
        }

        public static double CalculateTotalArea(IEnumerable<CadObject> objects)
        {
            double area = 0;
            foreach (var obj in objects)
            {
                if (obj.Type.Equals("Circle", StringComparison.OrdinalIgnoreCase))
                {
                    area += Math.PI * obj.Radius * obj.Radius;
                }
            }
            return area;
        }

        public static double CalculateTotalPerimeter(IEnumerable<CadObject> objects)
        {
            double perimeter = 0;
            foreach (var obj in objects)
            {
                if (obj.Type.Equals("Circle", StringComparison.OrdinalIgnoreCase))
                {
                    perimeter += 2 * Math.PI * obj.Radius;
                }
                else if (obj.Type.Equals("Line", StringComparison.OrdinalIgnoreCase))
                {
                    double dx = obj.EndX - obj.X;
                    double dy = obj.EndY - obj.Y;
                    perimeter += Math.Sqrt(dx * dx + dy * dy);
                }
            }
            return perimeter;
        }
        
        // Basic Intersection check for two lines
        public static bool DoLinesIntersect(CadObject line1, CadObject line2)
        {
            if (!line1.Type.Equals("Line", StringComparison.OrdinalIgnoreCase) || !line2.Type.Equals("Line", StringComparison.OrdinalIgnoreCase))
                return false;

            double p0_x = line1.X, p0_y = line1.Y;
            double p1_x = line1.EndX, p1_y = line1.EndY;
            double p2_x = line2.X, p2_y = line2.Y;
            double p3_x = line2.EndX, p3_y = line2.EndY;

            double s1_x, s1_y, s2_x, s2_y;
            s1_x = p1_x - p0_x;     s1_y = p1_y - p0_y;
            s2_x = p3_x - p2_x;     s2_y = p3_y - p2_y;

            double s, t;
            s = (-s1_y * (p0_x - p2_x) + s1_x * (p0_y - p2_y)) / (-s2_x * s1_y + s1_x * s2_y);
            t = ( s2_x * (p0_y - p2_y) - s2_y * (p0_x - p2_x)) / (-s2_x * s1_y + s1_x * s2_y);

            if (s >= 0 && s <= 1 && t >= 0 && t <= 1)
            {
                // Collision detected
                return true;
            }

            return false;
        }
    }
}
