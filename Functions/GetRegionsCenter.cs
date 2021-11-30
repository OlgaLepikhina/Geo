using Geo.Structure;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;

namespace Geo
{
    public partial class Functions
    {
        private static Point GetRegionsCenter(string str)
        {
            var arr = str.Replace("    Center ", "").Split(' ');        
            var point = new Point(double.Parse(arr[0].Replace(".",",")), double.Parse(arr[1].Replace(".", ",")));
            return point;
        }
    }
}
