using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Nord.Client
{
    internal class SpriteSheet
    {
        private readonly string _textureAtlasName;
        private Texture2D _textureAtlas;
        private readonly IDictionary<string, SpriteDefinition> _spriteDefinitions;

        public SpriteSheet(string textureAtlasName, IDictionary<string, SpriteDefinition> spriteDefinitions)
        {
            _textureAtlasName = textureAtlasName;
            _spriteDefinitions = spriteDefinitions;
        }

        public void LoadContent(ContentManager contentManager)
        {
            _textureAtlas = contentManager.Load<Texture2D>(_textureAtlasName);
        }

        public void DrawSprite(SpriteBatch spriteBatch, string name, Vector2 position)
        {
            if (!_spriteDefinitions.TryGetValue(name, out var spriteDefinition))
            {
                return;
            }

            spriteBatch.Draw(_textureAtlas, position, new Rectangle(spriteDefinition.X, spriteDefinition.Y, spriteDefinition.Width, spriteDefinition.Height), Color.White);
        }
    }
}
