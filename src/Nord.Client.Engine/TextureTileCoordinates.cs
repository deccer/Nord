using Microsoft.Xna.Framework;

namespace Nord.Client.Engine
{
    public readonly struct TextureTileCoordinates
    {
        public TextureTileCoordinates(Vector2 uv0, Vector2 uv1)
        {
            Uv0 = uv0;
            Uv1 = uv1;
        }

        public Vector2 Uv0 { get; }

        public Vector2 Uv1 { get; }
    }
}
