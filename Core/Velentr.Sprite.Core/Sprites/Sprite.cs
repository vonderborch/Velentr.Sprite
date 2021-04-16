using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Velentr.Sprite.Sprites
{
    public abstract class Sprite
    {
        private Vector2 _position;

        private Vector2 _size;

        protected Sprite(TextureManager manager, string name, Vector2 size) : this(manager, name, Vector2.Zero, Color.White, size, 0f, Vector2.Zero, SpriteEffects.None, 0f)
        {
        }

        protected Sprite(TextureManager manager, string name, Vector2 position, Color color, Vector2 size, float rotation, Vector2 origin, SpriteEffects effects, float layerDepth)
        {
            Manager = manager;
            Name = name;
            _position = position;
            _size = size;
            DestinationRectangle = new Rectangle((int)_position.X, (int)_position.Y, (int)_size.X, (int)_size.Y);
            Rotation = rotation;
            Origin = origin;
            Effects = effects;
            LayerDepth = layerDepth;
            Color = color;
        }

        public string Name { get; set; }

        public TextureManager Manager { get; set; }

        protected abstract Textures.Texture Texture { get; }

        public Vector2 Position
        {
            get => _position;
            set
            {
                _position = value;
                DestinationRectangle = new Rectangle((int)_position.X, (int)_position.Y, (int)_size.X, (int)_size.Y);
            }
        }

        public virtual Vector2 Size
        {
            get => _size;
            set
            {
                _size = value;
                DestinationRectangle = new Rectangle((int)_position.X, (int)_position.Y, (int)_size.X, (int)_size.Y);
            }
        }

        public Rectangle DestinationRectangle { get; private set; }

        public float Rotation { get; set; }

        public Vector2 Origin { get; set; }

        public SpriteEffects Effects { get; set; }

        public float LayerDepth { get; set; }

        public Color Color { get; set; }

        public abstract void Update(GameTime gameTime);

        public void Draw(SpriteBatch spriteBatch)
        {
            Draw(spriteBatch, DestinationRectangle, Color, Rotation, Origin, Effects, LayerDepth);
        }

        public abstract void Draw(SpriteBatch spriteBatch, Rectangle destinationRectangle, Color color, float rotation, Vector2 origin, SpriteEffects effects, float layerDepth);

        protected void InternalDraw(SpriteBatch spriteBatch, Rectangle destinationRectangle, Color color, float rotation, Vector2 origin, SpriteEffects effects, float layerDepth)
        {

            Texture.Draw(spriteBatch, destinationRectangle, null, color, rotation, origin, effects, layerDepth);
        }
    }
}
