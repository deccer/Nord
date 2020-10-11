using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nord.Client.Engine
{
    public sealed class Chunk
    {
        public const int ChunkSize = 32;
        private readonly IList<VertexPositionNormalTexture> _vertices;
        private readonly IDictionary<Point3, (float Height, string MaterialName)> _chunkDescription;
        private VertexBuffer _vertexBuffer;
        private bool _isModified;

        public Chunk(Point location)
        {
            Location = location;
            _chunkDescription = new Dictionary<Point3, (float Height, string MaterialName)>(64);
            _vertices = new List<VertexPositionNormalTexture>(64);
        }

        public Chunk(Point location, IDictionary<Point3, (float Height, string MaterialName)> chunkDescription)
        {
            Location = location;
            _chunkDescription = chunkDescription;
            _vertices = new List<VertexPositionNormalTexture>(64);
        }

        public BoundingBox BoundingBox { get; private set; }

        public Point Location { get; }

        public DateTime LoadedAt { get; set; }

        public void AddBlock(Point3 blockPosition, string materialName, float height)
        {
            _chunkDescription[blockPosition] = (height, materialName);
            _isModified = true;
        }

        public float GetHeight(Point3 location) => _chunkDescription.TryGetValue(location, out var gridItem)
            ? gridItem.Height
            : 0.0f;

        public void Write(BinaryWriter writer)
        {

        }

        private void RebuildVertices(TextureAtlas textureAtlas)
        {
            const float Width = 1.0f;
            const float Depth = 1.0f;

            const float HalfWidth = Width * 0.5f;
            const float HalfDepth = Depth * 0.5f;

            _vertices.Clear();
            foreach (var gridItem in _chunkDescription)
            {
                var origin = new Vector3(gridItem.Key.X, gridItem.Key.Y, gridItem.Key.Z);
                var height = gridItem.Value.Height;
                var materialName = gridItem.Value.MaterialName;

                var material = textureAtlas.Get(materialName);
                var tmc = material[0];

                // block top
                _vertices.Add(new VertexPositionNormalTexture(origin + new Vector3(-HalfWidth, height, +HalfDepth), Vector3.Up, new Vector2(tmc.Uv0.X, tmc.Uv1.Y)));
                _vertices.Add(new VertexPositionNormalTexture(origin + new Vector3(-HalfWidth, height, -HalfDepth), Vector3.Up, tmc.Uv0));
                _vertices.Add(new VertexPositionNormalTexture(origin + new Vector3(+HalfWidth, height, +HalfDepth), Vector3.Up, tmc.Uv1));

                _vertices.Add(new VertexPositionNormalTexture(origin + new Vector3(-HalfWidth, height, -HalfDepth), Vector3.Up, tmc.Uv0));
                _vertices.Add(new VertexPositionNormalTexture(origin + new Vector3(+HalfWidth, height, -HalfDepth), Vector3.Up, new Vector2(tmc.Uv1.X, tmc.Uv0.Y)));
                _vertices.Add(new VertexPositionNormalTexture(origin + new Vector3(+HalfWidth, height, +HalfDepth), Vector3.Up, tmc.Uv1));

                if (height > 0.05)
                {
                    // block front
                    var fmc = material[1];

                    _vertices.Add(new VertexPositionNormalTexture(origin + new Vector3(-HalfWidth, 0.0f, +HalfDepth), Vector3.Forward, new Vector2(fmc.Uv0.X, fmc.Uv1.Y)));
                    _vertices.Add(new VertexPositionNormalTexture(origin + new Vector3(-HalfWidth, height, +HalfDepth), Vector3.Forward, fmc.Uv0));
                    _vertices.Add(new VertexPositionNormalTexture(origin + new Vector3(+HalfWidth, 0.0f, +HalfDepth), Vector3.Forward, fmc.Uv1));

                    _vertices.Add(new VertexPositionNormalTexture(origin + new Vector3(-HalfWidth, height, +HalfDepth), Vector3.Forward, fmc.Uv0));
                    _vertices.Add(new VertexPositionNormalTexture(origin + new Vector3(+HalfWidth, height, +HalfDepth), Vector3.Forward, new Vector2(fmc.Uv1.X, fmc.Uv0.Y)));
                    _vertices.Add(new VertexPositionNormalTexture(origin + new Vector3(+HalfWidth, 0.0f, +HalfDepth), Vector3.Forward, fmc.Uv1));

                    var rmc = material[2];
                    // block right
                    _vertices.Add(new VertexPositionNormalTexture(origin + new Vector3(+HalfWidth, 0.0f, +HalfDepth), Vector3.Right, new Vector2(rmc.Uv0.X, rmc.Uv1.Y)));
                    _vertices.Add(new VertexPositionNormalTexture(origin + new Vector3(+HalfWidth, height, +HalfDepth), Vector3.Right, rmc.Uv0));
                    _vertices.Add(new VertexPositionNormalTexture(origin + new Vector3(+HalfWidth, 0.0f, -HalfDepth), Vector3.Right, rmc.Uv1));

                    _vertices.Add(new VertexPositionNormalTexture(origin + new Vector3(+HalfWidth, height, +HalfDepth), Vector3.Right, rmc.Uv0));
                    _vertices.Add(new VertexPositionNormalTexture(origin + new Vector3(+HalfWidth, height, -HalfDepth), Vector3.Right, new Vector2(rmc.Uv1.X, rmc.Uv0.Y)));
                    _vertices.Add(new VertexPositionNormalTexture(origin + new Vector3(+HalfWidth, 0.0f, -HalfDepth), Vector3.Right, rmc.Uv1));
                }
            }

            BoundingBox = BoundingBox.CreateFromPoints(_vertices.Select(vertex => vertex.Position));
        }

        private void RebuildVertexBuffer(GraphicsDevice graphicsDevice, TextureAtlas textureAtlas)
        {
            RebuildVertices(textureAtlas);
            _vertexBuffer?.Dispose();
            _vertexBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionNormalTexture), _vertices.Count, BufferUsage.WriteOnly);
            _vertexBuffer.SetData(_vertices.ToArray());
        }

        public void Draw(GraphicsDevice graphicsDevice, TextureAtlas textureAtlas)
        {
            if (_isModified)
            {
                RebuildVertexBuffer(graphicsDevice, textureAtlas);
                _isModified = false;
            }
            graphicsDevice.SetVertexBuffer(_vertexBuffer);
            graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, _vertices.Count / 3);
        }
    }
}
