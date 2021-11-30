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
        public static Map GetRegions(string fileName)
        {
            string[] linesMif = File.ReadAllLines(fileName, Encoding.Default);
            var regions = new Map { 
                Items = new List<MifRegion>(), 
                DataHeaders = new List<DataHeader>() 
            };
            var fileInfo = new FileInfo(fileName);
            var midFileName = $"{fileInfo.Directory}{Path.DirectorySeparatorChar}{fileInfo.Name.Replace(".mif", ".mid").Replace(".MIF", ".MID")}";
            string[] linesMid = File.ReadAllLines(midFileName, Encoding.Default);

            MifRegion region = null;
            var isPoint = false;
            var readData = false;
            var midIndex = 0;
            for (int index = 0; index < linesMif.Length; index++)
            {
                string str = linesMif[index];
                if (str.Contains("CoordSys")) { 
                    regions.CoordSys = GetRegionsCoordSys(str); continue; 
                }

                if (str.Contains("Columns"))
                {
                    readData = true;
                    continue;
                }

                if (str == "Data")
                {
                    readData = false;
                    continue;
                }

                if (readData)
                {
                    var arr = str.TrimStart().Replace(", ",",").Split(' ');
                    regions.DataHeaders.Add(new DataHeader { 
                        Name = arr[0], 
                        Type = arr[1] 
                    });
                    continue;
                }
                
                if (str.Contains("Region"))
                {
                    region = new MifRegion { Points = new List<Point>() };
                    index++;
                    str = linesMif[index];
                    region.Count = int.Parse(str.Replace(" ", ""));
                    isPoint = true;
                    index++;
                    str = linesMif[index];
                }

                if (str.Contains("Pen"))
                {
                    isPoint = false;
                    str = linesMif[index];
                    index++;
                    str = linesMif[index];
                    index++;
                    str = linesMif[index];
                    region.Center = GetRegionsCenter(str);
                    region.Data = linesMid[midIndex].Split(',');
                    midIndex++;
                    regions.Items.Add(region);
                    region = new MifRegion();
                }

                if (isPoint)
                {
                    var arr = str.Split(' ');
                    var point = new Point(double.Parse(arr[0].Replace(".", ",")), double.Parse(arr[1].Replace(".", ",")));
                    
                    region.Points.Add(ConvertPointToLocalCoordinates(point, regions.CoordSys));
                    
                }
            }

            foreach (var reg in regions.Items)
            {
                reg.Center = ConvertPointToLocalCoordinates(reg.Center, regions.CoordSys);
            }

            double minX = 999999999;
            double minY = 999999999;
            double maxX = 0;
            double maxY = 0;

            foreach (var reg in regions.Items)
            {
                foreach (var point in reg.Points)
                {
                    if (minX > point.X) minX = point.X;
                    if (minY > point.Y) minY = point.Y;
                    if (maxX < point.X) maxX = point.X;
                    if (maxY < point.Y) maxY = point.Y;
                }
            }
            foreach (var reg in regions.Items)
            {
                for (int i = 0; i < reg.Points.Count; i++)
                {
                    reg.Points[i] = new Point(reg.Points[i].X - minX + 50, reg.Points[i].Y - minY + 50);
                }
                reg.Center = new Point(reg.Center.X - minX + 50, reg.Center.Y - minY + 50);
            }


            var rotateTransform = new ScaleTransform { ScaleY = -1, CenterX = (maxX - minX)/2, CenterY = (maxY - minY) / 2 };
            foreach (var reg in regions.Items)
            {
                for (int i = 0; i < reg.Points.Count; i++)
                {
                    reg.Points[i] = rotateTransform.Transform(reg.Points[i]);
                }
                reg.Center = rotateTransform.Transform(reg.Center);
            }
                

            



            foreach (var reg in regions.Items)
            {
                reg.Geometry = CreateRegionGeometry(reg.Points);
            }

            regions.MinPoint = new Point(minX, minY);
            regions.MaxPoint = new Point(maxX, maxY);
            return regions;
        }
    }
}
