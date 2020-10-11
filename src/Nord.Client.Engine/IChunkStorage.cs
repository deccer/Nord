using Microsoft.Xna.Framework;

namespace Nord.Client.Engine
{
    public interface IChunkStorage
    {
        void AddChunk(Chunk chunk);

        bool TryGetChunk(Point chunkPosition, out Chunk chunk);
    }
}
