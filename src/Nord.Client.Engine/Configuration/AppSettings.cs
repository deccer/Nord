namespace Nord.Client.Engine.Configuration
{
    public sealed class AppSettings
    {
        public VideoSettings Video { get; }

        public AppSettings()
        {
            Video = new VideoSettings();
        }
    }
}
