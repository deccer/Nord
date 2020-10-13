using System.Windows;
using AtlasStudio.ViewModels;

namespace AtlasStudio.Views
{
    public partial class MainView : Window
    {
        private MainView()
        {
            InitializeComponent();
        }

        public MainView(MainViewModel mainViewModel)
            : this()
        {
            DataContext = mainViewModel;
        }
    }
}
