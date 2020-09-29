namespace Nord.Client.Configuration
{
    internal sealed class AppSettings
    {
        public VideoSettings Video { get; }

        public AppSettings()
        {
            Video = new VideoSettings();
        }
    }
}
