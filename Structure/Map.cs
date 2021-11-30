using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Geo.Structure
{
    public class Map
    {
        public CoordSys CoordSys { get; set; }
        public List<MifRegion> Items { get; set; }
        public List<DataHeader> DataHeaders { get; set; }
        public Point MinPoint { get; set; }
        public Point MaxPoint { get; set; }
    }
}
