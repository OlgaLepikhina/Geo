using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Geo.Structure
{
    public class MifRegion
    {
        public int Count { get; set; }
        public List<Point> Points { get; set; }
        [XmlIgnore] public PathGeometry Geometry { get; set; }
        public Point Center { get; set; }
        public string[] Data { get; set; }
        public List<Double> Data2 { get;  set; }
    }
}
