using Geo.LegendPanel;
using Geo.Structure;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Geo.Viewer
{
    internal class ViewerViewModel : ViewModelBase
    {
        private Map _map;
        private LegendPanelViewModel _legendPanelViewModel;
        private int _taskNum;
        private Point _mouseStartposition;
        private double _verticalOffset;
        private double _horizontalOffset;
        private bool _moveMode;
        private object _globalVariables;

        public DelegateCommand<object> MouseLeftButtonDownCommand { get; }
        public DelegateCommand MouseLeftButtonUpCommand { get; }
        public DelegateCommand<object> MouseMoveCommand { get; }

        public ViewerViewModel()
        {
            Scale = 0.5;
            MouseLeftButtonDownCommand = new DelegateCommand<object>(MouseLeftButtonDown);
            MouseLeftButtonUpCommand = new DelegateCommand(MouseLeftButtonUp);
            MouseMoveCommand = new DelegateCommand<object>(MouseMove);
        }

        private void MouseMove(object obj)
        {
            var sw = obj as ScrollViewer;
            if (_moveMode)
            {
               var newPos = Mouse.GetPosition(sw);
               var delta = new Point(_mouseStartposition.X - newPos.X, _mouseStartposition.Y - newPos.Y);
               if(delta.X != 0)
               sw.ScrollToHorizontalOffset(_horizontalOffset + delta.X);
               if (delta.Y != 0)
               sw.ScrollToVerticalOffset(_verticalOffset + delta.Y);
            }
        }
        private void MouseLeftButtonUp()
        {
            Mouse.OverrideCursor = null;
            _verticalOffset =  0;
            _horizontalOffset = 0;
            _moveMode = false;
        }
        private void MouseLeftButtonDown(object obj)
        {
            var sw = obj as ScrollViewer;
            Mouse.OverrideCursor = Cursors.Hand;
            _mouseStartposition = Mouse.GetPosition(sw);
            _verticalOffset =  sw.VerticalOffset;
            _horizontalOffset = sw.HorizontalOffset;
            _moveMode = true;
        }


        private void CreateLegendPanelControl()
        {
            LegendPanelControl = new LegendPanelView();
            _legendPanelViewModel = LegendPanelControl.DataContext as LegendPanelViewModel;
            _legendPanelViewModel.SetData(_taskNum);

        }

        public void SetData(Map map)
        {
            _map = map;
            PathesCanvas = CreatePathesCollection();
            ViewerWith = _map.MaxPoint.X + 100;
            ViewerHeight = _map.MaxPoint.Y + 100;
            LabelsCanvas = CreateLegendLayer(0);
            ShowTask0();
            CreateLegendPanelControl();
        }

        private Canvas CreatePathesCollection()
        {
            var canvas = new Canvas
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
            };
            foreach(var region in _map.Items)
            {
               // var toolTipView = new ToolTipView();
                var ObservablePath = new Path
                {
                    Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFF7777")),
                    StrokeThickness = 4,
                    Data = region.Geometry,
                    Tag = region,
                  //  ToolTip = toolTipView, 
                };
             //   (toolTipView.DataContext as ToolTipViewModel).SetData(region);
                ObservablePath.MouseEnter += RegionMouseEnter;
                ObservablePath.MouseLeave += RegionMouseLeave;
                ObservablePath.MouseRightButtonDown += RegionMouseDown;
                canvas.Children.Add(ObservablePath);
            }

            return canvas;
        }



        private void RegionMouseDown(object sender, MouseButtonEventArgs e)
        {
            var region = (sender as Path).Tag as MifRegion;
            var view = new DataViewerView();
            var viewModel = view.DataContext as DataViewerViewModel;
            viewModel.SetData(region);
            view.ShowDialog();
        }

        internal void ShowKvartal(int kvartalNum)
        {
            foreach (Path path in PathesCanvas.Children)
            {
                var region = path.Tag as MifRegion;
                if(kvartalNum != 0 && kvartalNum.ToString() != region.Data[1])
                {
                    path.Visibility = Visibility.Collapsed;
                }
                else
                {
                    path.Visibility = Visibility.Visible;
                }
            }

            foreach (Label path in LabelsCanvas.Children)
            {
                var region = path.Tag as MifRegion;
                if (kvartalNum != 0 && kvartalNum.ToString() != region.Data[1])
                {
                    path.Visibility = Visibility.Collapsed;
                }
                else
                {
                    path.Visibility = Visibility.Visible;
                }
            }
        }

        internal void ShowTask(int taskNum)
        {
            _taskNum = taskNum;
            if (taskNum == 0) ShowTask0();
            if (taskNum == 1) ShowTask1();
            if (taskNum == 2) ShowTask2();
            _legendPanelViewModel.SetData(_taskNum);
        }

        private void ShowTask0()
        {
            foreach (Path path in PathesCanvas.Children)
            {
                var region = path.Tag as MifRegion;
                switch (region.Data[3])
                {
                    case "\"Березняки\"":
                        path.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFfff66a"));
                        break;
                    case "\"Осинники\"":
                        path.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFb68e13"));
                        break;
                    case "\"Сосняки\"":
                        path.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF1aaa0f"));
                        break;
                    case "\"Болото\"":
                        path.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF666666"));
                        break;
                    case "\"Ельники\"":
                        path.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF086001"));
                        break;
                }
            }
        }

        // Оценка риска возникновения пожаров на выделах
        private void ShowTask1()
        {
            var minX1 = GetMinMaxX(14, 0);
            var minX2 = GetMinMaxX(15, 0);
            var minX3 = GetMinMaxX(16, 0);
            var minX4 = GetMinMaxX(17, 0);
            var maxX1 = GetMinMaxX(14, 1);
            var maxX2 = GetMinMaxX(15, 1);
            var maxX3 = GetMinMaxX(16, 1);
            var maxX4 = GetMinMaxX(17, 1);

            var koefs = new List<double>(); 
            foreach (Path path in PathesCanvas.Children)
            {
                var region = path.Tag as MifRegion;
                
                var x1 = double.Parse(region.Data[14].Replace(".", ","));
                var x2 = double.Parse(region.Data[15].Replace(".", ","));
                var x3 = double.Parse(region.Data[16].Replace(".", ","));
                var x4 = double.Parse(region.Data[17].Replace(".", ","));

                var k1 = 1 - ((x1 - minX1) / (maxX1 - minX1));
                var k2 = 1 - ((x2 - minX2) / (maxX2 - minX2));
                var k3 = 1 - ((x3 - minX3) / (maxX3 - minX3));
                var k4 = 1 - ((x4 - minX4) / (maxX4 - minX4));

                var k = (k1 * 0.1) + (k2 * 0.2) + (k3 * 0.3) + (k4 * 0.4);


                if (k < 0.12)
                {
                    path.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
                }
                else if (0.12 < k & k < 0.58)
                {
                    path.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF55FF55"));
                }
                else if (0.58 < k & k < 0.74)
                {
                    path.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDDFF33"));
                }
                else if (0.74 < k & k < 0.91)
                {
                    path.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFEE5533"));
                }
                else if (0.91 < k)
                {
                    path.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
                }
                    koefs.Add(k);
            }
        }

        private double GetMinMaxX(int columnNum, int mode)
        {
            double minX = 999999999;
            double maxX = 0;
            foreach (var region in _map.Items)
            {
                var x = double.Parse(region.Data[columnNum].Replace(".", ","));

                if (x < minX) minX = x;
                if (x > maxX) maxX = x;
            }
            return mode == 0 ? minX : maxX;
        }



        // Показать 100% вырубки.
        private void ShowTask2()
        {
            foreach(Path path in PathesCanvas.Children)
            {
                var region = path.Tag as MifRegion;
                if(region.Data[18] == "100")
                {
                    path.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFF3333"));
                }
                else 
                {
                    path.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF33FF33"));
                }
            }
        }

        internal void UpdateLegend(int column)
        {
            LabelsCanvas = CreateLegendLayer(column);
        }

        private Canvas CreateLegendLayer(int column)
        {
            var canvas = new Canvas
            {
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
            };
            foreach (var region in _map.Items)
            {
                var label = new Label
                {
                    FontSize = 28,
                    Content = region.Data[column],
                    Margin = new Thickness(region.Center.X-20, region.Center.Y-20, 0,0),
                    Tag = region
                };
                canvas.Children.Add(label);
            }

            return canvas;
        }

        private void RegionMouseLeave(object sender, MouseEventArgs e)
        {
            var path = sender as Path;
            path.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFF0000"));
        }

        private void RegionMouseEnter(object sender, MouseEventArgs e)
        {
            var path = sender as Path;
            path.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF000000"));
        }

        public Canvas PathesCanvas { get; set; }
        public Canvas LabelsCanvas { get; set; }
        public double ViewerWith { get; set; }
        public double ViewerHeight { get; set; }
        public double Scale { get; set; }
        public LegendPanelView LegendPanelControl { get; set; }
        public int VerticalScrollOffset { get; set; }
        public int HorizontalScrollOffset { get; set; }
    }

    public class ObservablePath : ViewModelBase
    {
        public PathGeometry Geometry { get; set; }
    }
}
