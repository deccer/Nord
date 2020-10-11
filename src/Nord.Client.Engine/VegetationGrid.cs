using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Nord.Client.Engine
{
    public sealed class VegetationGrid
    {
        private readonly GraphicsDevice _graphicsDevice;
        private readonly Chunk _chunk;
        private readonly IDictionary<Point3, string> _vegetationGrid;
        private readonly IList<VertexPositionNormalTexture> _vertices;

        private BasicEffect _effect;
        private VertexBuffer _vertexBuffer;

        public VegetationGrid(GraphicsDevice graphicsDevice, Chunk chunk)
        {
            _graphicsDevice = graphicsDevice;
            _chunk = chunk;

            _vegetationGrid = new Dictionary<Point3, string>(64);
            _vertices = new List<VertexPositionNormalTexture>(64);
        }

        public void AddTree(Point location, string treeName)
        {
            RebuildVertices();
            RebuildVertexBuffer();
        }

        public void LoadContent(ContentManager contentManager)
        {
        }

        public void Draw()
        {
        }

        private void RebuildVertices()
        {
            _vertices.Clear();

            const float Width = 1.0f;
            const float Depth = 1.0f;

            const float HalfWidth = Width * 0.5f;
            const float HalfDepth = Depth * 0.5f;

            foreach (var vegetationItem in _vegetationGrid)
            {
                var mapLocationZ = _chunk.GetHeight(vegetationItem.Key);
                var origin = new Vector3(vegetationItem.Key.X, mapLocationZ, vegetationItem.Key.Y);
                var height = 1.0f;

                _vertices.Add(new VertexPositionNormalTexture(origin + new Vector3(-HalfWidth, 0.0f, +HalfDepth), Vector3.Forward, new Vector2(0, 1)));
                _vertices.Add(new VertexPositionNormalTexture(origin + new Vector3(-HalfWidth, height, +HalfDepth), Vector3.Forward, new Vector2(0, 0)));
                _vertices.Add(new VertexPositionNormalTexture(origin + new Vector3(+HalfWidth, 0.0f, +HalfDepth), Vector3.Forward, new Vector2(1, 1)));

                _vertices.Add(new VertexPositionNormalTexture(origin + new Vector3(-HalfWidth, height, +HalfDepth), Vector3.Forward, new Vector2(0, 0)));
                _vertices.Add(new VertexPositionNormalTexture(origin + new Vector3(+HalfWidth, height, +HalfDepth), Vector3.Forward, new Vector2(1, 0)));
                _vertices.Add(new VertexPositionNormalTexture(origin + new Vector3(+HalfWidth, 0.0f, +HalfDepth), Vector3.Forward, new Vector2(1, 1)));
            }
        }

        private void RebuildVertexBuffer()
        {
            _vertexBuffer?.Dispose();
            _vertexBuffer = new VertexBuffer(_graphicsDevice, typeof(VertexPositionNormalTexture), _vertices.Count, BufferUsage.WriteOnly);
            _vertexBuffer.SetData(_vertices.ToArray());
        }
    }
}
