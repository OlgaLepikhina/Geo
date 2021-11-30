using Geo.Structure;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;

namespace Geo
{
    public partial class Functions
    {
        //CoordSys NonEarth Units "m" Bounds (2242658.49084, 434129.345198) (2245318.83514, 437958.597982)
        private static CoordSys GetRegionsCoordSys(string str)
        {
            var coordSys = new CoordSys();
            var arr = str.Split('(');

            var part0 = arr[1].Replace(")", "").Replace(" ", "");
            var arr2 = part0.Split(',');
            coordSys.StartPoint = new Point(double.Parse(arr2[0].Replace(".",",")), double.Parse(arr2[1].Replace(".", ",")));

            var part1 = arr[1].Replace(")", "").Replace(" ", "");
            var arr3 = part1.Split(',');
            coordSys.EndPoint = new Point(double.Parse(arr3[0].Replace(".", ",")), double.Parse(arr3[1].Replace(".", ",")));

            return coordSys;
        }
    }
}
