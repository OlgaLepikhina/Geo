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
using System.Windows;
using System.Windows.Controls;

namespace Geo
{
    internal class MainWindowViewModel : ViewModelBase
    {
        private ViewerViewModel _ViewerViewModel;
        private HeaderMenuViewModel _headerMenuViewModel;
        private LeftPanelViewModel _leftPanelViewModel;

        public static Maps Maps { get; set; }

        public MainWindowViewModel()
        {
            Maps = Functions.LoadSettings();
            CreateHeaderMenuControl();
            CreateViewerControl();
            CreateLeftPanelControl();
            if (Maps.Items.Any())
            {
                foreach (var mifRegion in Maps.Items[0].Items)
                {
                    mifRegion.Geometry = Functions.CreateRegionGeometry(mifRegion.Points);
                }
                _ViewerViewModel.SetData(Maps.Items[0]);
            }
        }

        private void CreateLeftPanelControl()
        {
            LeftPanelControl = new LeftPanelView();
            _leftPanelViewModel = LeftPanelControl.DataContext as LeftPanelViewModel;
            _leftPanelViewModel.SetData(Maps.Items[0]);
            _leftPanelViewModel.IsLegendUpdated += IsLegendUpdated;
            _leftPanelViewModel.IsTaskUpdated += IsTaskUpdated;
            _leftPanelViewModel.IsKvartalUpdated += IsKvartalUpdated;
        }



        private void IsKvartalUpdated(int kvartalNum)
        {
            _ViewerViewModel.ShowKvartal(kvartalNum);
        }

        private void IsTaskUpdated(int taskNum)
        {
            _ViewerViewModel.ShowTask(taskNum);
        }

        private void IsLegendUpdated(int column)
        {
            _ViewerViewModel.UpdateLegend(column);
        }

        private void CreateHeaderMenuControl()
        {         
            HeaderMenuControl = new HeaderMenuView();
            _headerMenuViewModel = HeaderMenuControl.DataContext as HeaderMenuViewModel;
            _headerMenuViewModel.SetData(Maps);
            _headerMenuViewModel.IsMapLoaded += IsMapLoaded;
        }

        private void IsMapLoaded(Map map)
        {
            _ViewerViewModel.SetData(map); 
        }



        private void CreateViewerControl()
        {
            ViewerControl = new ViewerView();
            _ViewerViewModel = ViewerControl.DataContext as ViewerViewModel;
        }
        public ViewerView ViewerControl { get; set; }
        public LeftPanelView LeftPanelControl { get; set; }
        public HeaderMenuView HeaderMenuControl { get; set; }

    }
}
