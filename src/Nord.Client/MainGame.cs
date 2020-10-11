using System;
using System.Collections.Generic;
using ImGuiNET;
using JetBrains.Annotations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nord.Client.Engine;
using Nord.Client.Engine.Configuration;
using Nord.Client.UI;
using Serilog;
using Num = System.Numerics;

namespace Nord.Client
{
    internal sealed class MainGame : Game
    {
        private readonly ILogger _logger;
        private readonly IAppSettingsProvider _appSettingsProvider;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private ImGuiRenderer _imGuiRenderer;
        private ImGuiInputHandler _imGuiInputHandler;

        // private readonly IDictionary<string, SpriteDefinition> _spriteDefinitions;
        // private readonly SpriteSheet _spriteSheet;

        private readonly Camera _camera;

        private Chunk _chunk;
        private BasicEffect _effect;
        private TextureAtlas _textureAtlas;
        private TextureAtlas _textureAtlas1;

        private VegetationGrid _vegetationGrid;
        private Model[] _treeModels;
        private IList<Matrix> _treeInstances;
        private Random _random = new Random();
        private KeyboardState _currentkeyboardState;
        private MouseState _currentMouseState;

        public MainGame([NotNull] ILogger logger, [NotNull] IAppSettingsProvider appSettingsProvider)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _appSettingsProvider = appSettingsProvider ?? throw new ArgumentNullException(nameof(appSettingsProvider));

            _graphics = new GraphicsDeviceManager(this);
            _camera = new PerspectiveCamera(
                _graphics.PreferredBackBufferWidth,
                _graphics.PreferredBackBufferHeight,
                0.1f,
                4096.0f,
                MathHelper.PiOver2
            );
            _camera.Zoom = 20;
            _camera.Position = new Vector3(24, 24, 24);
            _camera.Direction = Vector3.Normalize(Vector3.Zero - _camera.Position);

            /*
            _spriteDefinitions = new Dictionary<string, SpriteDefinition>
            {
                { "Test", new SpriteDefinition(397, 1183, 132, 83) },
            };
            _spriteSheet = new SpriteSheet("Atlas/landscapeTiles_sheet", _spriteDefinitions);
            */

            /*
            var textureDefinitions = new Dictionary<string, (CubeSide, TextureTileDefinition)[]>
            {
                {
                    "Cube", new[]
                    {
                        (CubeSide.Top, new TextureTileDefinition(7, 3)),
                        (CubeSide.Front, new TextureTileDefinition(26, 18)),
                        (CubeSide.Right, new TextureTileDefinition(26, 19))
                    }
                }
            };
            for (var i = 0; i < 12; i++)
            {
                textureDefinitions.Add(
                    $"Water{i}",
                    new[]
                    {
                        (CubeSide.Top, new TextureTileDefinition(23 + i, 4)),
                        (CubeSide.Front, new TextureTileDefinition(23 + i, 4)),
                        (CubeSide.Right, new TextureTileDefinition(23 + i, 4))
                    }
                );
            }

            for (var i = 0; i < 4; i++)
            {
                textureDefinitions.Add(
                    $"Dirt{i}",
                    new[]
                    {
                        (CubeSide.Top, new TextureTileDefinition(8 + i, 5)),
                        (CubeSide.Front, new TextureTileDefinition(8 + i, 5)),
                        (CubeSide.Right, new TextureTileDefinition(8 + i, 5))
                    }
                );
            }

            _textureAtlas = new TextureAtlas("Atlas/Atlas", 32, textureDefinitions);
            */

            var textureDefinitionsAtlas1 = new Dictionary<string, (CubeSide, TextureTileDefinition)[]>();
            for (var i = 0; i < 4; i++)
            {
                textureDefinitionsAtlas1.Add(
                    $"Water2_{i}",
                    new[]
                    {
                        (CubeSide.Top, new TextureTileDefinition(0 + i, 0)),
                        (CubeSide.Front, new TextureTileDefinition(0 + i, 0)),
                        (CubeSide.Right, new TextureTileDefinition(0 + i, 0))
                    }
                );
                textureDefinitionsAtlas1.Add(
                    $"Grass2_{i}",
                    new[]
                    {
                        (CubeSide.Top, new TextureTileDefinition(0 + i, 1)),
                        (CubeSide.Front, new TextureTileDefinition(0 + i, 1)),
                        (CubeSide.Right, new TextureTileDefinition(0 + i, 1))
                    }
                );
            }

            _textureAtlas1 = new TextureAtlas("Atlas/Atlas1", 32, textureDefinitionsAtlas1);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _currentkeyboardState = Keyboard.GetState();
            _currentMouseState = Mouse.GetState();

            _graphics.PreferMultiSampling = true;
            _graphics.PreferredBackBufferWidth = _appSettingsProvider.AppSettings.Video.Width;
            _graphics.PreferredBackBufferHeight = _appSettingsProvider.AppSettings.Video.Height;
            _graphics.IsFullScreen = _appSettingsProvider.AppSettings.Video.IsFullScreen;
            _graphics.GraphicsProfile = GraphicsProfile.HiDef;
            _graphics.ApplyChanges();

            _imGuiInputHandler = new ImGuiInputHandler();
            _imGuiRenderer = new ImGuiRenderer(this, _imGuiInputHandler);
            _imGuiRenderer.Initialize();
            _imGuiRenderer.RebuildFontAtlas();

            ImGui.GetIO().ConfigFlags = ImGuiConfigFlags.DockingEnable;

            _effect = new BasicEffect(GraphicsDevice);
            _effect.EnableDefaultLighting();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // _spriteSheet.LoadContent(Content);
            // _textureAtlas.LoadContent(Content);
            _textureAtlas1.LoadContent(Content);
            _effect.Texture = _textureAtlas1.Texture;
            _effect.TextureEnabled = true;

            _chunk = new Chunk(new Point(0, 0));

            for (var y = 0; y < 32; y++)
            {
                for (var x = 0; x < 32; x++)
                {
                    _chunk.AddBlock(
                        new Point3(x, 0.8f * (float)_random.NextDouble(), y),
                        $"Grass2_{_random.Next(0, 4)}",
                        _random.Next(1, 2)
                    );
                }
            }

            _chunk.AddBlock(new Point3(4, 3, 5), "Grass2_0", 1);
            _chunk.AddBlock(new Point3(4, 3, 6), "Grass2_0", 1);
            _chunk.AddBlock(new Point3(3, 3, 5), "Grass2_0", 1);

            /*
            _treeModels = new Model[2];
            _treeModels[0] = Content.Load<Model>("Models/LP_Tree");
            for (var i = 1; i < _treeModels.Length; ++i)
            {
                _treeModels[i] = Content.Load<Model>($"Models/tree{i}");
            }

            var treeCount = 10;
            _treeInstances = Enumerable.Range(0, treeCount)
                .Select(
                    i =>
                    {
                        var location = new Point(-22 + random.Next(1, 44), -22 + random.Next(1, 44));
                        var mapZ = _chunk.GetHeight(location);
                        return Matrix.CreateScale(0.01f) *
                               Matrix.CreateScale(0.7f + 0.1f * (float)random.Next(1, 4)) *
                               Matrix.CreateRotationY(MathHelper.ToRadians(random.Next(0, 180))) *
                               Matrix.CreateTranslation(location.X, mapZ, location.Y);
                    }
                )
                .ToList();
                */
        }

        protected override void Update(GameTime gameTime)
        {
            var previousKeyboardState = _currentkeyboardState;
            _currentkeyboardState = Keyboard.GetState();
            var previousMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();

            if (_currentkeyboardState.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            var boost = 0.5f;
            if (_currentkeyboardState.IsKeyDown(Keys.W))
            {
                _camera.Position += Vector3.Normalize(new Vector3(-1, 0, -1)) * boost;
            }

            if (_currentkeyboardState.IsKeyDown(Keys.S))
            {
                _camera.Position += Vector3.Normalize(new Vector3(1, 0, 1)) * boost;
            }

            if (_currentkeyboardState.IsKeyDown(Keys.A))
            {
                _camera.Position += Vector3.Normalize(new Vector3(-1, 0, 1)) * boost;
            }

            if (_currentkeyboardState.IsKeyDown(Keys.D))
            {
                _camera.Position += Vector3.Normalize(new Vector3(1, 0, -1)) * boost;
            }

            _effect.View = _camera.ViewMatrix;
            _effect.Projection = _camera.ProjectionMatrix;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            rotation++;

            _imGuiInputHandler.Update(GraphicsDevice, ref _currentkeyboardState, ref _currentMouseState);

            base.Update(gameTime);
        }

        private static float rotation = 0f;

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _effect.World = Matrix.Identity;
            float aspectRatio = _graphics.PreferredBackBufferWidth / (float)_graphics.PreferredBackBufferHeight;
            float fieldOfView = MathHelper.PiOver4;
            float nearClipPlane = _camera.NearPlane;
            float farClipPlane = _camera.FarPlane;

            var matrix1 = Matrix.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, nearClipPlane, farClipPlane);
            var matrix2 =
                Matrix.CreateOrthographic(
                    _graphics.PreferredBackBufferWidth,
                    _graphics.PreferredBackBufferHeight,
                    nearClipPlane,
                    farClipPlane) *
                Matrix.CreateScale(30);

            _effect.Projection = matrix1;

            GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
            GraphicsDevice.BlendState = BlendState.Opaque;

            // foreach (var treeInstance in _treeInstances)
            // {
            //     _treeModels[1].Draw(treeInstance, _effect.View, _effect.Projection);
            // }

            foreach (var pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                _chunk.Draw(GraphicsDevice, _textureAtlas1);
            }

            /*,
                _spriteBatch.Begin();
                _spriteSheet.DrawSprite(_spriteBatch, "Test", new Vector2(100, 100));
                _spriteBatch.End();
                */

            _imGuiRenderer.BeginLayout(gameTime);
            DrawUi();
            _imGuiRenderer.EndLayout();
            base.Draw(gameTime);
        }

        private void DrawUi()
        {
            var show = true;
            ImGui.SetNextWindowPos(new Num.Vector2(650, 20), ImGuiCond.FirstUseEver);
            ImGui.ShowDemoWindow(ref show);
        }
    }
}
