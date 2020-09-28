using System;
using Microsoft.Extensions.DependencyInjection;

namespace Nord.Client
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            var compositionRoot = CreateCompositionRoot();

            using var game = compositionRoot.GetService<MainGame>();
            game.Run();
        }

        private static IServiceProvider CreateCompositionRoot()
        {
            var services = new ServiceCollection();
            services.AddSingleton<MainGame>();
            return services.BuildServiceProvider();
        }
    }
}
