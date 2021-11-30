using System.Windows.Controls;
namespace Geo.Viewer
{
    /// <summary>
    /// Логика взаимодействия для Viewer.xaml
    /// </summary>
    public partial class ViewerView : UserControl
    {
        public ViewerView()
        {
            InitializeComponent();
        }

        private void Grid_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            var viewModel = DataContext as ViewerViewModel;
            var delta = e.Delta;
            if (delta > 0) viewModel.Scale = viewModel.Scale + 0.02;
            if (delta < 0) viewModel.Scale = viewModel.Scale - 0.02;
            if(delta != 0) e.Handled = true;
        }
    }
}
