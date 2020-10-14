using System.Collections.Generic;
using System.Linq;
using System.Windows;
using AtlasStudio.Models;
using ImTools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.WpfCore.MonoGameControls;
using SharpDX.Direct3D11;
using Color = Microsoft.Xna.Framework.Color;
using Matrix = Microsoft.Xna.Framework.Matrix;
using SamplerState = Microsoft.Xna.Framework.Graphics.SamplerState;
using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;

namespace AtlasStudio.ViewModels
{
    public class PreviewViewModel : MonoGameViewModel
    {
        private Texture2D _texture;
        private Color _backgroundColor;

        private float _width;
        private float _height;

        private VertexBuffer _blockVertexBuffer;
        private BasicEffect _effect;
        private readonly IList<VertexPositionNormalTexture> _vertices = new List<VertexPositionNormalTexture>(18);

        private BlockDescription _blockDescription;
        public BlockDescription BlockDescription
        {
            get => _blockDescription;
            set => SetValue(ref _blockDescription, value, PreviewBlockUpdated);
        }

        public override void LoadContent()
        {
            var topFace = new BlockFace(Face.Top, Vector2.Zero, Vector2.One);
            var leftFace = new BlockFace(Face.Left, Vector2.Zero, Vector2.One);
            var rightFace = new BlockFace(Face.Right, Vector2.Zero, Vector2.One);
            BlockDescription = new BlockDescription("Test", topFace, leftFace, rightFace);

            var controlColor = SystemColors.ControlColor;
            _backgroundColor = new Color(controlColor.R, controlColor.G, controlColor.B);

            _texture = new Texture2D(GraphicsDevice, 32, 32, false, SurfaceFormat.Color);
            var colors = new Color[32 * 32];
            for (var i = 0; i < 32 * 32; ++i)
            {
                if (i > 128)
                {
                    colors[i] = Color.Beige;
                }

                else if (i > 256)
                {
                    colors[i] = Color.LightCyan;
                }

                else if (i > 512)
                {
                    colors[i] = Color.OrangeRed;
                }

                else
                {
                    colors[i] = Color.LawnGreen;
                }
            }
            _texture.SetData(colors);

            _effect = new BasicEffect(GraphicsDevice);
            _effect.EnableDefaultLighting();
            _effect.LightingEnabled = true;
            _effect.Texture = _texture;
            _effect.TextureEnabled = true;
            _effect.View = Matrix.CreateLookAt(new Vector3(2f, 2f, 2f), Vector3.Zero, Vector3.Up);
            //_effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, 4.0f / 3.0f,0.1f, 16.0f);
            _effect.World = Matrix.CreateTranslation(0, -0.5f, 0);
        }

        public override void Update(GameTime gameTime)
        {
            _effect.Projection = Matrix.CreateOrthographic(_width, _height, 0.1f, 320.0f) *
                                 Matrix.CreateScale(30);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(_backgroundColor);

            if (_blockVertexBuffer != null)
            {
                GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
                GraphicsDevice.Textures[0] = _texture;

                foreach (var pass in _effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    GraphicsDevice.SetVertexBuffer(_blockVertexBuffer);
                    GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, _vertices.Count / 3);
                }
            }
        }

        public void SetDimension(float width, float height)
        {
            _width = width;
            _height = height;
        }

        private void PreviewBlockUpdated()
        {
            RebuildVertices(BlockDescription);

            _blockVertexBuffer?.Dispose();
            _blockVertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionNormalTexture), 18, BufferUsage.WriteOnly);
            _blockVertexBuffer.SetData(_vertices.ToArray());
        }

        private void RebuildVertices(BlockDescription blockDescription)
        {
            const float Width = 1.0f;
            const float Depth = 1.0f;

            const float HalfWidth = Width * 0.5f;
            const float HalfDepth = Depth * 0.5f;

            _vertices.Clear();

            var origin = Vector3.Zero;
            var height = 1.0f;
            var materialName = string.Empty;

            var top = blockDescription.Top;

            // block top
            _vertices.Add(new VertexPositionNormalTexture(origin + new Vector3(-HalfWidth, height, +HalfDepth), Vector3.Up, new Vector2(top.Uv0.X, top.Uv1.Y)));
            _vertices.Add(new VertexPositionNormalTexture(origin + new Vector3(-HalfWidth, height, -HalfDepth), Vector3.Up, top.Uv0));
            _vertices.Add(new VertexPositionNormalTexture(origin + new Vector3(+HalfWidth, height, +HalfDepth), Vector3.Up, top.Uv1));

            _vertices.Add(new VertexPositionNormalTexture(origin + new Vector3(-HalfWidth, height, -HalfDepth), Vector3.Up, top.Uv0));
            _vertices.Add(new VertexPositionNormalTexture(origin + new Vector3(+HalfWidth, height, -HalfDepth), Vector3.Up, new Vector2(top.Uv1.X, top.Uv0.Y)));
            _vertices.Add(new VertexPositionNormalTexture(origin + new Vector3(+HalfWidth, height, +HalfDepth), Vector3.Up, top.Uv1));

            var left = blockDescription.Left;
            // block left

            _vertices.Add(new VertexPositionNormalTexture(origin + new Vector3(-HalfWidth, 0.0f, +HalfDepth), Vector3.Forward, new Vector2(left.Uv0.X, left.Uv1.Y)));
            _vertices.Add(new VertexPositionNormalTexture(origin + new Vector3(-HalfWidth, height, +HalfDepth), Vector3.Forward, left.Uv0));
            _vertices.Add(new VertexPositionNormalTexture(origin + new Vector3(+HalfWidth, 0.0f, +HalfDepth), Vector3.Forward, left.Uv1));

            _vertices.Add(new VertexPositionNormalTexture(origin + new Vector3(-HalfWidth, height, +HalfDepth), Vector3.Forward, left.Uv0));
            _vertices.Add(new VertexPositionNormalTexture(origin + new Vector3(+HalfWidth, height, +HalfDepth), Vector3.Forward, new Vector2(left.Uv1.X, left.Uv0.Y)));
            _vertices.Add(new VertexPositionNormalTexture(origin + new Vector3(+HalfWidth, 0.0f, +HalfDepth), Vector3.Forward, left.Uv1));

            var right = blockDescription.Right;
            // block right
            _vertices.Add(new VertexPositionNormalTexture(origin + new Vector3(+HalfWidth, 0.0f, +HalfDepth), Vector3.Right, new Vector2(right.Uv0.X, right.Uv1.Y)));
            _vertices.Add(new VertexPositionNormalTexture(origin + new Vector3(+HalfWidth, height, +HalfDepth), Vector3.Right, right.Uv0));
            _vertices.Add(new VertexPositionNormalTexture(origin + new Vector3(+HalfWidth, 0.0f, -HalfDepth), Vector3.Right, right.Uv1));

            _vertices.Add(new VertexPositionNormalTexture(origin + new Vector3(+HalfWidth, height, +HalfDepth), Vector3.Right, right.Uv0));
            _vertices.Add(new VertexPositionNormalTexture(origin + new Vector3(+HalfWidth, height, -HalfDepth), Vector3.Right, new Vector2(right.Uv1.X, right.Uv0.Y)));
            _vertices.Add(new VertexPositionNormalTexture(origin + new Vector3(+HalfWidth, 0.0f, -HalfDepth), Vector3.Right, right.Uv1));
        }
    }
}
