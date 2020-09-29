using System;
using Microsoft.Extensions.DependencyInjection;
using Nord.Client.Configuration;
using Serilog;

namespace Nord.Client
{
    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            var compositionRoot = CreateCompositionRoot();

            using var game = compositionRoot.GetService<MainGame>();
            game.Run();
        }

        private static IServiceProvider CreateCompositionRoot()
        {
            Log.Logger = new LoggerConfiguration().CreateLogger();
            AppDomain.CurrentDomain.ProcessExit += (s, e) => Log.CloseAndFlush();

            var services = new ServiceCollection();

            services.AddSingleton(Log.Logger);
            services.AddSingleton<IConfigurationLoader, ConfigurationLoader>();
            services.AddSingleton<IConfigurationSaver, ConfigurationSaver>();
            services.AddSingleton<IAppSettingsProvider, AppSettingsProvider>();
            services.AddSingleton<MainGame>();
            return services.BuildServiceProvider();
        }
    }
}
