using GalaSoft.MvvmLight;
using Geo.Structure;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace Geo.HeaderMenu
{
    internal class HeaderMenuViewModel : ViewModelBase
    {
        private Maps _maps;
        public DelegateCommand OpenFileDialogCommand { get; }
        public DelegateCommand OpenAboutDialogCommand { get; }

        public delegate void MapLoaded(Map map);
        public event MapLoaded IsMapLoaded;

        public HeaderMenuViewModel()
        {
            OpenFileDialogCommand = new DelegateCommand(OpenFileDialog);
            OpenAboutDialogCommand = new DelegateCommand(OpenAboutDialog);
        }

        private void OpenAboutDialog()
        {
            var view = new AboutView();
            view.ShowDialog();
        }

        public void SetData(Maps maps)
        {
            _maps = maps;
        }

        private void OpenFileDialog()
        {
            OpenFileDialog OPF = new OpenFileDialog
            {
                Filter = "Файлы mif|*.mif"
            };
            if (OPF.ShowDialog() == DialogResult.OK)
            {
                var map = Functions.GetRegions(OPF.FileName);
                _maps.Items = new List<Map>();
                _maps.Items.Add(map);
                Functions.SaveSettings(_maps);
                IsMapLoaded(map);
            }
        }
    }
}