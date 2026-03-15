namespace MiniCadManager.Core.Models
{
    public class CadObject
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Layer { get; set; } = string.Empty;
        public double X { get; set; }
        public double Y { get; set; }

        public double DistanceTo(CadObject other)
        {
            double dx = X - other.X;
            double dy = Y - other.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        public override string ToString() => $"[{Type}] {Name} ({X:F2},{Y:F2})";
    }
}
