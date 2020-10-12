using System.Windows;
using SimplexStudio.ViewModels;

namespace SimplexStudio.Views
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
