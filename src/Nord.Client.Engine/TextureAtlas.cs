using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Nord.Client.Engine
{
    public sealed class TextureAtlas
    {
        private readonly string _textureAtlasName;
        private readonly int _gridSize;
        private int _textureWidth;
        private int _textureHeight;
        private readonly IDictionary<string, (CubeSide, TextureTileDefinition)[]> _textureDefinitions;

        public TextureAtlas(string textureAtlasName, int gridSize, IDictionary<string, (CubeSide, TextureTileDefinition)[]> textureDefinitions)
        {
            _textureAtlasName = textureAtlasName;
            _gridSize = gridSize;
            _textureDefinitions = textureDefinitions;
        }

        public Texture2D Texture { get; private set; }

        public TextureTileCoordinates[] Get(string name)
        {
            if (!_textureDefinitions.TryGetValue(name, out var textureDefinition))
            {
                return null;
            }

            var tileCountX = _textureWidth / _gridSize;
            var tileCountY = _textureHeight / _gridSize;

            var tileUnitX = 1.0f / tileCountX;
            var tileUnitY = 1.0f / tileCountY;

            var coordinates = new TextureTileCoordinates[3];
            for (var side = 0; side < coordinates.Length; side++)
            {
                var uv0 = new Vector2(textureDefinition[side].Item2.X * tileUnitX, textureDefinition[side].Item2.Y * tileUnitY);
                var uv1 = new Vector2((textureDefinition[side].Item2.X + 1) * tileUnitX, (textureDefinition[side].Item2.Y + 1) * tileUnitY);

                coordinates[side] = new TextureTileCoordinates(uv0, uv1);
            }

            return coordinates;
        }

        public void LoadContent(ContentManager contentManager)
        {
            Texture = contentManager.Load<Texture2D>(_textureAtlasName);
            _textureWidth = Texture.Width;
            _textureHeight = Texture.Height;
        }
    }
}
