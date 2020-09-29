using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nord.Client.Configuration;
using Serilog;

namespace Nord.Client
{
    internal sealed class MainGame : Game
    {
        private readonly ILogger _logger;
        private readonly IAppSettingsProvider _appSettingsProvider;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private IDictionary<string, SpriteDefinition> _spriteDefinitions;
        private SpriteSheet _spriteSheet;

        public MainGame([NotNull] ILogger logger, [NotNull] IAppSettingsProvider appSettingsProvider)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _appSettingsProvider = appSettingsProvider ?? throw new ArgumentNullException(nameof(appSettingsProvider));

            _graphics = new GraphicsDeviceManager(this);
            _spriteDefinitions = new Dictionary<string, SpriteDefinition>
            {
                { "Test", new SpriteDefinition(397, 1183, 132, 83) },
            };
            _spriteSheet = new SpriteSheet("Atlas/landscapeTiles_sheet", _spriteDefinitions);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferMultiSampling = true;
            _graphics.PreferredBackBufferWidth = _appSettingsProvider.AppSettings.Video.Width;
            _graphics.PreferredBackBufferHeight = _appSettingsProvider.AppSettings.Video.Height;
            _graphics.IsFullScreen = _appSettingsProvider.AppSettings.Video.IsFullScreen;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _spriteSheet.LoadContent(Content);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            _spriteSheet.DrawSprite(_spriteBatch, "Test", new Vector2(100, 100));
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
