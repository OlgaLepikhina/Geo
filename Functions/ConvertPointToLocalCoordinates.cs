using Geo.Structure;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;

namespace Geo
{
    public partial class Functions
    {
        private static Point ConvertPointToLocalCoordinates(Point point, CoordSys coordSys)
        {
            if(coordSys == null)
            {
                // Тут ошибка. не распознан CoordSys
                return new Point(0,0);
            }

            var localPoint = new Point(point.X - coordSys.StartPoint.X, point.Y - coordSys.StartPoint.Y);
            return localPoint;
        }
    }
}
