namespace Nord.Client.Configuration
{
    internal interface IConfigurationLoader
    {
        T LoadConfiguration<T>(string fileName) where T : class;
    }
}
