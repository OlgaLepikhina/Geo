using Geo.Structure;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Geo.LegendPanel
{
    internal class LegendPanelViewModel : ViewModelBase
    {
        private int _taskNum;

        public LegendPanelViewModel()
        {
            Task1Heigh = 100;
            Task1Heigh = 0;
            Task1Heigh = 0;
        }


        public int Task1Heigh { get; set; }
        public int Task2Heigh { get; set; }
        public int Task3Heigh { get; set; }

        public void SetData(int taskNum)
        {
            _taskNum = taskNum;

            if(_taskNum == 0)
            {
                Task1Heigh = 100;
                Task2Heigh = 0;
                Task3Heigh = 0;
            }
            else if (_taskNum == 1)
            {
                Task1Heigh = 0;
                Task2Heigh = 100;
                Task3Heigh = 0;
            }
            else if (_taskNum == 2)
            {
                Task1Heigh = 0;
                Task2Heigh = 0;
                Task3Heigh = 100;
            }
        }
    }
}
