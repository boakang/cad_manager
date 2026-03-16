using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MiniCadManager.Core.Models;

namespace MiniCadManager.Core.Services
{
    public class DxfExportService
    {
        public void Export(IEnumerable<CadObject> objects, string filePath)
        {
            var sb = new StringBuilder();

            // DXF Header
            sb.AppendLine("  0");
            sb.AppendLine("SECTION");
            sb.AppendLine("  2");
            sb.AppendLine("ENTITIES");

            foreach (var obj in objects)
            {
                if (obj.Type.Equals("Line", StringComparison.OrdinalIgnoreCase))
                {
                    sb.AppendLine("  0");
                    sb.AppendLine("LINE");
                    sb.AppendLine("  8");
                    sb.AppendLine(string.IsNullOrWhiteSpace(obj.Layer) ? "0" : obj.Layer);
                    sb.AppendLine(" 10");
                    sb.AppendLine(obj.X.ToString(System.Globalization.CultureInfo.InvariantCulture));
                    sb.AppendLine(" 20");
                    sb.AppendLine(obj.Y.ToString(System.Globalization.CultureInfo.InvariantCulture));
                    sb.AppendLine(" 11");
                    sb.AppendLine(obj.EndX.ToString(System.Globalization.CultureInfo.InvariantCulture));
                    sb.AppendLine(" 21");
                    sb.AppendLine(obj.EndY.ToString(System.Globalization.CultureInfo.InvariantCulture));
                }
                else if (obj.Type.Equals("Circle", StringComparison.OrdinalIgnoreCase))
                {
                    sb.AppendLine("  0");
                    sb.AppendLine("CIRCLE");
                    sb.AppendLine("  8");
                    sb.AppendLine(string.IsNullOrWhiteSpace(obj.Layer) ? "0" : obj.Layer);
                    sb.AppendLine(" 10");
                    sb.AppendLine(obj.X.ToString(System.Globalization.CultureInfo.InvariantCulture));
                    sb.AppendLine(" 20");
                    sb.AppendLine(obj.Y.ToString(System.Globalization.CultureInfo.InvariantCulture));
                    sb.AppendLine(" 40");
                    sb.AppendLine(obj.Radius.ToString(System.Globalization.CultureInfo.InvariantCulture));
                }
                else if (obj.Type.Equals("Text", StringComparison.OrdinalIgnoreCase))
                {
                    sb.AppendLine("  0");
                    sb.AppendLine("TEXT");
                    sb.AppendLine("  8");
                    sb.AppendLine(string.IsNullOrWhiteSpace(obj.Layer) ? "0" : obj.Layer);
                    sb.AppendLine(" 10");
                    sb.AppendLine(obj.X.ToString(System.Globalization.CultureInfo.InvariantCulture));
                    sb.AppendLine(" 20");
                    sb.AppendLine(obj.Y.ToString(System.Globalization.CultureInfo.InvariantCulture));
                    sb.AppendLine(" 40");
                    sb.AppendLine("2.5"); // Default Text Height
                    sb.AppendLine("  1");
                    sb.AppendLine(obj.Name);
                }
            }

            // DXF Footer
            sb.AppendLine("  0");
            sb.AppendLine("ENDSEC");
            sb.AppendLine("  0");
            sb.AppendLine("EOF");

            File.WriteAllText(filePath, sb.ToString());
        }
    }
}
