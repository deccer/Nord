namespace Nord.Client.Engine.Configuration
{
    public interface IConfigurationLoader
    {
        T LoadConfiguration<T>(string fileName) where T : class;
    }
}
