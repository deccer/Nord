using System;
using System.Windows;
using AtlasStudio.ViewModels;

namespace AtlasStudio.Views
{
    public partial class MainView : Window
    {
        private readonly MainViewModel _mainViewModel;

        private MainView()
        {
            InitializeComponent();
        }

        public MainView(MainViewModel mainViewModel)
            : this()
        {
            _mainViewModel = mainViewModel;
            DataContext = mainViewModel;
        }

        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);

            _mainViewModel.PreviewViewModel.SetDimension((float)PreviewControl.ActualWidth, (float)PreviewControl.ActualHeight);
        }
    }
}
