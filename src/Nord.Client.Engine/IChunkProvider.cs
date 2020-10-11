using Microsoft.Xna.Framework;

namespace Nord.Client.Engine
{
    public interface IChunkProvider
    {
        Chunk GetChunk(Point location);
    }
}
