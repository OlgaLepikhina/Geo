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
        public static Maps LoadSettings()
        {
            
            var path = $"{AppDomain.CurrentDomain.BaseDirectory}DATA{Path.DirectorySeparatorChar}Data.XML";
            if (!Directory.Exists($"{AppDomain.CurrentDomain.BaseDirectory}DATA{Path.DirectorySeparatorChar}"))
            {
                Directory.CreateDirectory($"{AppDomain.CurrentDomain.BaseDirectory}DATA{Path.DirectorySeparatorChar}");
            }
            var maps = XmlSerializerService.Deserialize<Maps>(path);
            if(maps == null) maps = new Maps { Items = new List<Map>() };
            return maps;
        }
    }
}
