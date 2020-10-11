using System;
using Microsoft.Xna.Framework;

namespace Nord.Client.Engine
{
    public sealed class ChunkProvider : IChunkProvider
    {
        private readonly IChunkStorage _chunkStorage;
        private readonly IChunkCreator _chunkCreator;

        public ChunkProvider(IChunkStorage chunkStorage, IChunkCreator chunkCreator)
        {
            _chunkStorage = chunkStorage;
            _chunkCreator = chunkCreator;
        }

        public Chunk GetChunk(Point chunkLocation)
        {
            if (_chunkStorage.TryGetChunk(chunkLocation, out var chunk))
            {
                chunk.LoadedAt = DateTime.Now;
                return chunk;
            }

            chunk = _chunkCreator.CreateChunk(chunkLocation);
            _chunkStorage.AddChunk(chunk);
            return chunk;
        }
    }
}
