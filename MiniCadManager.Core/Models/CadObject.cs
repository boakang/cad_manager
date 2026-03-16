namespace MiniCadManager.Core.Models
{
    public class CadObject
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Layer { get; set; } = string.Empty;
        public double X { get; set; } // Center X or Start X
        public double Y { get; set; } // Center Y or Start Y
        public double EndX { get; set; } // Used for Line
        public double EndY { get; set; } // Used for Line
        public double Radius { get; set; } // Used for Circle

        // UI Helpers for Canvas drawing
        public double UiDiameter => Radius * 2;
        public double UiTopLeftX => X - Radius;
        public double UiTopLeftY => Y - Radius;

        public double DistanceTo(CadObject other)
        {
            double dx = X - other.X;
            double dy = Y - other.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        public override string ToString() => $"[{Type}] {Name} ({X:F2},{Y:F2})";
    }
}
