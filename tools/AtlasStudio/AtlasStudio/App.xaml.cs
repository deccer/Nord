using System.Windows;
using AtlasStudio.ViewModels;
using AtlasStudio.Views;
using DryIoc;

namespace AtlasStudio
{
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var compositionRoot = CreateCompositionRoot();

            MainWindow = compositionRoot.Resolve<MainView>();
            MainWindow?.Show();

            base.OnStartup(e);
        }

        private static IResolver CreateCompositionRoot()
        {
            var container = new Container();

            container.Register<MainViewModel>(Reuse.Singleton);
            container.Register<MainView>(Reuse.Singleton);

            return container;
        }
    }
}
