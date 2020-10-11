namespace Nord.Client.Engine
{
    public readonly struct TextureTileDefinition
    {
        public TextureTileDefinition(float x, float y)
        {
            X = x;
            Y = y;
        }

        public float X { get; }

        public float Y { get; }
    }
}
