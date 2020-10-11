namespace Nord.Client.Engine.Configuration
{
    public sealed class AppSettingsProvider : IAppSettingsProvider
    {
        private readonly IConfigurationLoader _configurationLoader;
        private readonly IConfigurationSaver _configurationSaver;

        public AppSettingsProvider(
            IConfigurationLoader configurationLoader,
            IConfigurationSaver configurationSaver)
        {
            _configurationLoader = configurationLoader;
            _configurationSaver = configurationSaver;
            AppSettings = new AppSettings();
            Load();
        }

        public AppSettings AppSettings { get; private set; }

        public void Load()
        {
            AppSettings = _configurationLoader.LoadConfiguration<AppSettings>("settings.json");
            if (AppSettings == null)
            {
                AppSettings = new AppSettings();
                Save();
            }
        }

        public void Save()
        {
            _configurationSaver.SaveConfiguration("setting.json", AppSettings);
        }
    }
}
