using Microsoft.Xna.Framework;

namespace Nord.Client.Engine
{
    public interface IChunkCreator
    {
        Chunk CreateChunk(Point chunkLocation);
    }
}
