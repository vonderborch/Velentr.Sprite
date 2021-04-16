using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Velentr.Sprite.Sprites
{
    public class SimpleSprite : Sprite
    {

        public SimpleSprite(SpriteManager manager, string name, string texture, Vector2 size) : base(manager, name, size)
        {
            TextureName = texture;
        }

        public SimpleSprite(SpriteManager manager, string name, string texture, Vector2 position, Color color, Vector2 size, float rotation, Vector2 origin, SpriteEffects effects, float layerDepth) : base(manager, name, position, color, size, rotation, origin, effects, layerDepth)
        {
            TextureName = texture;
        }

        public string TextureName { get; set; }

        protected override Textures.Texture Texture => Manager._textures[TextureName];

        public override void Update(GameTime time)
        {

        }

        public override void Draw(SpriteBatch spriteBatch, Rectangle destinationRectangle, Color color, float rotation, Vector2 origin, SpriteEffects effects, float layerDepth)
        {
            InternalDraw(spriteBatch, destinationRectangle, color, rotation, origin, effects, layerDepth);
        }

    }
}
