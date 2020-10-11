namespace Nord.Client.Engine.Configuration
{
    public interface IAppSettingsProvider
    {
        AppSettings AppSettings { get; }

        void Save();

        void Load();
    }
}
