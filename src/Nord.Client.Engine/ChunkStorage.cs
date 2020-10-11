using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Nord.Client.Engine.Extensions;

namespace Nord.Client.Engine
{
    public sealed class ChunkStorage : IChunkStorage
    {
        private readonly IDictionary<Point, Chunk> _chunks;
        private readonly string _directory;

        public ChunkStorage(string directory)
        {
            _chunks = new Dictionary<Point, Chunk>();
            _directory = directory;
            if (!Directory.Exists(_directory))
            {
                Directory.CreateDirectory(_directory);
            }
        }

        public void AddChunk(Chunk chunk)
        {
            if (_chunks.ContainsKey(chunk.Location))
            {
                return;
            }

            var chunkFileName = GetChunkFileName(chunk.Location);

            using var fileStream = File.Create(chunkFileName);
            using var writer = new BinaryWriter(fileStream);
            chunk.Write(writer);

            _chunks.Add(chunk.Location, chunk);
        }

        public void Clean()
        {
            var files = Directory.GetFiles(_directory);
            foreach (var file in files)
            {
                File.Delete(file);
            }
        }

        public void CleanupUnusedChunks()
        {
            var chunksToBeUnloaded = _chunks.Values.Where(chunk => chunk.LoadedAt < DateTime.Now.AddSeconds(-60));
            foreach (var chunkToBeUnloaded in chunksToBeUnloaded)
            {
                _chunks.Remove(chunkToBeUnloaded.Location);
            }
        }

        public bool TryGetChunk(Point chunkPosition, out Chunk chunk)
        {
            if (_chunks.TryGetValue(chunkPosition, out chunk))
            {
                return true;
            }

            var chunkFileName = GetChunkFileName(chunkPosition);
            if (!File.Exists(chunkFileName))
            {
                return false;
            }

            using var fileStream = File.Open(chunkFileName, FileMode.Open);
            using var reader = new BinaryReader(fileStream);

            var chunkDescription = new Dictionary<Point3, (float Height, string MaterialName)>(Chunk.ChunkSize * Chunk.ChunkSize);
            var tileCount = reader.ReadInt32();
            for (var i = 0; i < tileCount; ++i)
            {
                var tilePosition = reader.ReadPoint3();
                var tileHeight = reader.ReadSingle();
                var materialName = reader.ReadString();

                chunkDescription.Add(tilePosition, (tileHeight, materialName));
            }

            chunk = new Chunk(chunkPosition, chunkDescription);
            _chunks.Add(chunkPosition, chunk);
            return true;
        }

        private string GetChunkFileName(Point chunkLocation) => Path.Combine(_directory, $"{chunkLocation.Y}-{chunkLocation.X}.chunk");
    }
}
