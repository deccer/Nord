using Microsoft.Xna.Framework;

namespace AtlasStudio.Models
{
    public class BlockFace
    {
        public Face Face { get; }
        public Vector2 Uv0 { get; }
        public Vector2 Uv1 { get; }

        public BlockFace(Face face, Vector2 uv0, Vector2 uv1)
        {
            Face = face;
            Uv0 = uv0;
            Uv1 = uv1;
        }
    }
}
