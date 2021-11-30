using Geo.Structure;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Geo
{
    public partial class Functions
    {
        public static PathGeometry CreateRegionGeometry(List<Point> points)
        {
            var geometry = new PathGeometry();
            var figure = new PathFigure {
                IsClosed = true,
                StartPoint = new Point(points[0].X, points[0].Y)
            };
            foreach (var point in points)
            {
                var scalePoint = new Point(point.X, point.Y);
                var lineSegment = new LineSegment(scalePoint, true);
                figure.Segments.Add(lineSegment);
            }
            geometry.Figures.Add(figure);
            return geometry;
        }
    }
}
