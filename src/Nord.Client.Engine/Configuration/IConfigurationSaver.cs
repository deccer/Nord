namespace Nord.Client.Engine.Configuration
{
    public interface IConfigurationSaver
    {
        void SaveConfiguration<T>(string fileName, T configuration) where T : class;
    }
}
