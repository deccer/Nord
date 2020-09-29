namespace Nord.Client.Configuration
{
    internal interface IAppSettingsProvider
    {
        AppSettings AppSettings { get; }

        void Save();

        void Load();
    }
}
