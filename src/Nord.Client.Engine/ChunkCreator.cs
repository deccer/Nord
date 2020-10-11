using Microsoft.Xna.Framework;

namespace Nord.Client.Engine
{
    public sealed class ChunkCreator : IChunkCreator
    {
        public Chunk CreateChunk(Point chunkLocation) => new Chunk(chunkLocation);
    }
}
