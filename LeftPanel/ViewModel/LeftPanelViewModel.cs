using Geo.LegendPanel;
using Geo.Structure;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Geo.LeftPanel
{
    internal class LeftPanelViewModel : ViewModelBase
    {
        private Map _map;


        public DelegateCommand UpdateLegendCommand { get; }
        public DelegateCommand UpdateTaskCommand { get; }
        public DelegateCommand UpdateKvartalCommand { get; }

        public delegate void LegendUpdated(int column);
        public event LegendUpdated IsLegendUpdated;

        public delegate void TaskUpdated(int column);
        public event TaskUpdated IsTaskUpdated;

        public delegate void KvartalUpdated(int column);
        public event TaskUpdated IsKvartalUpdated;

        public LeftPanelViewModel()
        {
            UpdateLegendCommand = new DelegateCommand(UpdateLegend);
            UpdateTaskCommand = new DelegateCommand(UpdateTask);
            UpdateKvartalCommand = new DelegateCommand(UpdateKvartal);
            ChartVisibility = Visibility.Visible;
        }

        private void UpdateKvartal()
        {
            IsKvartalUpdated(SelectedKvartal);
            WoodCollection = CreateWoodCollection();
        }

        private void UpdateTask()
        {
            IsTaskUpdated(SelectedTask);
            if (SelectedTask == 0)
            {
                ChartVisibility = Visibility.Visible;
            }
            else
            {
                ChartVisibility = Visibility.Collapsed;
            }
        }

        private void UpdateLegend()
        {
            IsLegendUpdated(LegendColumn);
        }



        public void SetData(Map map)
        {
            _map = map;
            LegendCollection = CreateLegendCollection();
            LegendColumn = 0;
            TaskCollection = CreateTaskCollection();
            SelectedTask = 0;
            WoodCollection = CreateWoodCollection();
            KvartalCollection = CreateKvartalCollection();
        }

        private ObservableCollection<ItemObservable> CreateKvartalCollection()
        {
            var collection = new ObservableCollection<ItemObservable>();
            collection.Add(new ItemObservable { Name = "Все кварталы", Num = 0 });
            collection.Add(new ItemObservable { Name = "Квартал 160", Num = 160 });
            collection.Add(new ItemObservable { Name = "Квартал 165", Num = 165 });
            collection.Add(new ItemObservable { Name = "Квартал 166", Num = 166 });
            collection.Add(new ItemObservable { Name = "Квартал 171", Num = 171 });
            collection.Add(new ItemObservable { Name = "Квартал 172", Num = 172 });
            return collection;
        }

        private ObservableCollection<ItemObservable> CreateTaskCollection()
        {
            var collection = new ObservableCollection<ItemObservable>();

            collection.Add(new ItemObservable { Name = "Расчет запаса древесных насаждений ", Num = 0 });
            collection.Add(new ItemObservable { Name = "Оценка риска возникновения пожаров на выделах", Num = 1 });
            collection.Add(new ItemObservable { Name = "Оценка риска вырубки деревьев", Num = 2});

            return collection;
        }


        private ObservableCollection<ChartItem> CreateWoodCollection()
        {
            var collection = new ObservableCollection<ChartItem>();
            var coef = CreareCoefs();
            double summ2011 = 0;
            double summ2021 = 0;
            double summ2031 = 0;
            foreach (var region in _map.Items)
            {
                if (SelectedKvartal != 0 && SelectedKvartal.ToString() != region.Data[1]) continue;
                var old2011 = double.Parse(region.Data[4].Replace(".", ","));
                var old2021 = double.Parse(region.Data[8].Replace(".", ","));
                var old2031 = double.Parse(region.Data[11].Replace(".", ","));

                // Y = a×ln(x) + b
                var a = GetAB(coef, region.Data[3], region.Data[7],"A");
                var b = GetAB(coef, region.Data[3], region.Data[7],"B");

                if (a == 0) continue;
                if (old2011 == 0) continue;
                if (old2021 == 0) continue;
                if (old2031 == 0) continue;

                var y2011 = a * Math.Log(old2011) + b;
                var y2021 = a * Math.Log(old2021) + b;
                var y2031 = a * Math.Log(old2031) + b;

                region.Data2 = new List<double>();
                region.Data2.Add(y2011);
                region.Data2.Add(y2021);
                region.Data2.Add(y2031);

                summ2011 = summ2011 + y2011;
                summ2021 = summ2021 + y2021;
                summ2031 = summ2031 + y2031;
            }

            
            summ2011 = 0; summ2021 = 0; summ2031 = 0;
            foreach (var region in _map.Items)
            {
                if (SelectedKvartal != 0 && SelectedKvartal.ToString() != region.Data[1]) continue;

                if (!region.Data2.Any()) continue;
                if (region.Data[3] != "\"Березняки\"") continue;
                summ2011 = summ2011 + region.Data2[0];
                summ2021 = summ2021 + region.Data2[1];
                summ2031 = summ2031 + region.Data2[2];
            }
            collection.Add(new ChartItem { Name = "Береза", Value0 = summ2011, Value1 = summ2021, Value2 = summ2031 });

            summ2011 = 0; summ2021 = 0; summ2031 = 0;
            foreach (var region in _map.Items)
            {
                if (SelectedKvartal != 0 && SelectedKvartal.ToString() != region.Data[1]) continue;

                if (!region.Data2.Any()) continue;
                if (region.Data[3] != "\"Ельники\"") continue;
                summ2011 = summ2011 + region.Data2[0];
                summ2021 = summ2021 + region.Data2[1];
                summ2031 = summ2031 + region.Data2[2];
            }
            collection.Add(new ChartItem { Name = "Ель", Value0 = summ2011, Value1 = summ2021, Value2 = summ2031 });

            summ2011 = 0; summ2021 = 0; summ2031 = 0;
            foreach (var region in _map.Items)
            {
                if (SelectedKvartal != 0 && SelectedKvartal.ToString() != region.Data[1]) continue;

                if (!region.Data2.Any()) continue;
                if (region.Data[3] != "\"Осинники\"") continue;
                summ2011 = summ2011 + region.Data2[0];
                summ2021 = summ2021 + region.Data2[1];
                summ2031 = summ2031 + region.Data2[2];
            }
            collection.Add(new ChartItem { Name = "Осина", Value0 = summ2011, Value1 = summ2021, Value2 = summ2031 });

            summ2011 = 0; summ2021 = 0; summ2031 = 0;
            foreach (var region in _map.Items)
            {
                if (SelectedKvartal != 0 && SelectedKvartal.ToString() != region.Data[1]) continue;

                if (!region.Data2.Any()) continue;
                if (region.Data[3] != "\"Сосняки\"") continue;
                summ2011 = summ2011 + region.Data2[0];
                summ2021 = summ2021 + region.Data2[1];
                summ2031 = summ2031 + region.Data2[2];
            }
            collection.Add(new ChartItem { Name = "Сосна", Value0 = summ2011, Value1 = summ2021, Value2 = summ2031 });



            return collection;

        }

        private double GetAB(List<Coef> coef, string woodType, string bonnitet, string mode)
        {
            var arrCoefs = CreareCoefs();
            foreach(var item in arrCoefs)
            {
                if(item.WoodTupe == woodType && item.Bonitet == bonnitet)
                {
                    if(mode == "A")
                    {
                        return item.A;
                    }
                    else
                    {
                        return item.B;
                    }                
                }
            }
            return 0;
        }


        private List<Coef> CreareCoefs()
        {
            var coef = new List<Coef>();
            coef.Add(new Coef { WoodTupe = "\"Ельники\"", Bonitet = "1", A = 305.00, B = -908.16 });
            coef.Add(new Coef { WoodTupe = "\"Ельники\"", Bonitet = "2", A = 257.62, B = -783.93 });
            coef.Add(new Coef { WoodTupe = "\"Ельники\"", Bonitet = "3", A = 191.81, B = -576.66 });
            coef.Add(new Coef { WoodTupe = "\"Ельники\"", Bonitet = "4", A = 128.33, B = -376.73 });
            coef.Add(new Coef { WoodTupe = "\"Ельники\"", Bonitet = "5", A = 89.72, B = -266.43 });
            coef.Add(new Coef { WoodTupe = "\"Березняки\"", Bonitet = "1", A = 198.67, B = -536.21 });
            coef.Add(new Coef { WoodTupe = "\"Березняки\"", Bonitet = "2", A = 172.98, B = -472.06 });
            coef.Add(new Coef { WoodTupe = "\"Березняки\"", Bonitet = "3", A = 131.28, B = -353.37 });
            coef.Add(new Coef { WoodTupe = "\"Березняки\"", Bonitet = "4", A = 94.10, B = -246.26 });
            coef.Add(new Coef { WoodTupe = "\"Березняки\"", Bonitet = "5", A = 48.68, B = -117.99 });
            coef.Add(new Coef { WoodTupe = "\"Осинники\"", Bonitet = "1", A = 244.96, B = -568.53 });
            coef.Add(new Coef { WoodTupe = "\"Осинники\"", Bonitet = "2", A = 163.54, B = -377.29 });
            coef.Add(new Coef { WoodTupe = "\"Осинники\"", Bonitet = "3", A = 127.09, B = -292.11 });
            coef.Add(new Coef { WoodTupe = "\"Осинники\"", Bonitet = "4", A = 97.65, B = -224.78 });
            coef.Add(new Coef { WoodTupe = "\"Осинники\"", Bonitet = "5", A = 72.06, B = -166.94 });
            coef.Add(new Coef { WoodTupe = "\"Сосняки\"", Bonitet = "1", A = 266.17, B = -764.42 });
            coef.Add(new Coef { WoodTupe = "\"Сосняки\"", Bonitet = "2", A = 225.97, B = -666.20 });
            coef.Add(new Coef { WoodTupe = "\"Сосняки\"", Bonitet = "3", A = 183.66, B = -543.71 });
            coef.Add(new Coef { WoodTupe = "\"Сосняки\"", Bonitet = "4", A = 122.21, B = -347.05 });
            coef.Add(new Coef { WoodTupe = "\"Сосняки\"", Bonitet = "5", A = 80.84, B = -228.14 });
            return coef;
        }

        private ObservableCollection<ItemObservable> CreateLegendCollection()
        {
            var collection = new ObservableCollection<ItemObservable>();
            for (int index = 0; index < _map.DataHeaders.Count; index++)
            {
                DataHeader dataHeader = _map.DataHeaders[index];
                var item = new ItemObservable
                {
                    Name = dataHeader.Name,
                    Num = index
                };
                collection.Add(item);
            }
            return collection;
        }
        public ObservableCollection<ItemObservable> LegendCollection { get; set; }
        public int LegendColumn { get; set; }
        public ObservableCollection<ItemObservable> TaskCollection { get; set; }
        public int SelectedTask { get; set; }
        public ObservableCollection<ItemObservable> KvartalCollection { get; set; }
        public int SelectedKvartal { get; set; }
        
        public ObservableCollection<ChartItem> WoodCollection { get; set; }

        public Visibility ChartVisibility { get; set; }


    }

    internal class Coef
    {
        public string WoodTupe { get; set; }
        public string Bonitet { get; set; }
        public double A { get; set; }
        public double B { get; set; }
    }

    public class ChartItem
    {
        public string Name { get; set; }
        public double Value0 { get; set; }
        public double Value1 { get; set; }
        public double Value2 { get; set; }
    }

    public class ItemObservable
    {
        public string Name { get; set; }
        public int Num { get; set; }
    }
}
