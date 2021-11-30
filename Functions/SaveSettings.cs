using Geo.Services;
using Geo.Structure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Geo
{
    public partial class Functions
    {
        public static void SaveSettings(Maps maps)
        {
            var path = $"{AppDomain.CurrentDomain.BaseDirectory}DATA{Path.DirectorySeparatorChar}Data.XML";
            XmlSerializerService.Serialize(path, maps);
        }
    }
}
