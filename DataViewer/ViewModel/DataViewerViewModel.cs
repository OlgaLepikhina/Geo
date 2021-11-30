using GalaSoft.MvvmLight;
using Geo.HeaderMenu;
using Geo.LeftPanel;
using Geo.Structure;
using Geo.Viewer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Geo
{
    internal class DataViewerViewModel : ViewModelBase
    {
        public static Maps Maps { get; set; }
        public DataViewerViewModel()
        {

        }

        internal void SetData(MifRegion region)
        {
            Number = region.Data[0];
            Kvartal = region.Data[1];
            Squere = region.Data[2];
            Wood = region.Data[3];
            Bonnitet = region.Data[7];
            Virub = region.Data[18];
            Age2011 = region.Data[4];
            Age2021 = region.Data[8];
            Age2031 = region.Data[11];
            Height2011 = region.Data[5];
            Нeight2021 = region.Data[9];
            Height2031 = region.Data[12];
            Diameter2011 = region.Data[6];
            Diameter2021 = region.Data[10];
            Diameter2031 = region.Data[13];
            StoreGa2011 = (Math.Round(region.Data2[0] / double.Parse(Squere.Replace(".", ",")), 2)).ToString("#.00");
            StoreGa2021 = (Math.Round(region.Data2[1] / double.Parse(Squere.Replace(".", ",")), 2)).ToString("#.00");
            StoreGa2031 = (Math.Round(region.Data2[2] / double.Parse(Squere.Replace(".", ",")), 2)).ToString("#.00");
            Store2011 = Math.Round(region.Data2[0], 2).ToString("#.00");
            Store2021 = Math.Round(region.Data2[1], 2).ToString("#.00");
            Store2031 = Math.Round(region.Data2[2], 2).ToString("#.00");
        }

        public string Number { get; set; }
        public string Kvartal { get; set; }
        public string Squere { get; set; }
        public string Wood { get; set; }
        public string Bonnitet { get; set; }
        public string Virub { get; set; }
        public string Age2011 { get; set; }
        public string Age2021 { get; set; }
        public string Age2031 { get; set; }
        public string Height2011 { get; set; }
        public string Нeight2021 { get; set; }
        public string Height2031 { get; set; }
        public string Diameter2011 { get; set; }
        public string Diameter2021 { get; set; }
        public string Diameter2031 { get; set; }
        public string StoreGa2011 { get; set; }
        public string StoreGa2021 { get; set; }
        public string StoreGa2031 { get; set; }
        public string Store2011 { get; set; }
        public string Store2021 { get; set; }
        public string Store2031 { get; set; }
    }
}
