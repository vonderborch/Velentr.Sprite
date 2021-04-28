using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Velentr.Sprite.Helpers;

namespace Velentr.Sprite.Sprites.Base
{
    /// <summary>
    ///     Represents a Sprite object
    /// </summary>
    public abstract class VelentrSprite
    {
        /// <summary>
        ///     The position.
        /// </summary>
        private Vector2 _position;

        /// <summary>
        ///     The size.
        /// </summary>
        private Vector2 _size;

        /// <summary>
        ///     Specialized constructor for use only by derived class.
        /// </summary>
        ///
        /// <param name="manager">    The manager. </param>
        /// <param name="name">       The name. </param>
        /// <param name="size">       The size. </param>
        /// <param name="spriteType"> The type of the sprite. </param>
        protected VelentrSprite(TextureManager manager, string name, Vector2 size, SpriteType spriteType) : this(manager, name, Vector2.Zero, size, spriteType)
        {
        }

        /// <summary>
        ///     Specialized constructor for use only by derived class.
        /// </summary>
        ///
        /// <param name="manager">    The manager. </param>
        /// <param name="name">       The name. </param>
        /// <param name="position">   The position. </param>
        /// <param name="size">       The size. </param>
        /// <param name="spriteType"> The type of the sprite. </param>
        /// <param name="color">      (Optional) The color. </param>
        /// <param name="rotation">   (Optional) The rotation. </param>
        /// <param name="origin">     (Optional) The origin. </param>
        /// <param name="effects">    (Optional) The effects. </param>
        /// <param name="layerDepth"> (Optional) Depth of the layer. </param>
        protected VelentrSprite(TextureManager manager, string name, Vector2 position, Vector2 size, SpriteType spriteType, Color? color = null, float rotation = 0f, Vector2? origin = null, SpriteEffects effects = SpriteEffects.None, float layerDepth = 0f)
        {
            Manager = manager;
            Name = name;
            _position = position;
            _size = size;
            DestinationRectangle = new Rectangle((int)_position.X, (int)_position.Y, (int)_size.X, (int)_size.Y);
            Rotation = rotation;
            Origin = origin ?? Vector2.Zero;
            Effects = effects;
            LayerDepth = layerDepth;
            Color = color ?? Color.White;
            SpriteType = spriteType;
        }

        /// <summary>
        ///     Gets or sets the color.
        /// </summary>
        ///
        /// <value>
        ///     The color.
        /// </value>
        public Color Color { get; set; }

        /// <summary>
        ///     Gets or sets destination rectangle.
        /// </summary>
        ///
        /// <value>
        ///     The destination rectangle.
        /// </value>
        public Rectangle DestinationRectangle { get; private set; }

        /// <summary>
        ///     Gets or sets the effects.
        /// </summary>
        ///
        /// <value>
        ///     The effects.
        /// </value>
        public SpriteEffects Effects { get; set; }

        /// <summary>
        ///     Gets or sets the depth of the layer.
        /// </summary>
        ///
        /// <value>
        ///     The depth of the layer.
        /// </value>
        public float LayerDepth { get; set; }

        /// <summary>
        ///     Gets or sets the manager.
        /// </summary>
        ///
        /// <value>
        ///     The manager.
        /// </value>
        public TextureManager Manager { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        ///
        /// <value>
        ///     The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the origin.
        /// </summary>
        ///
        /// <value>
        ///     The origin.
        /// </value>
        public Vector2 Origin { get; set; }

        /// <summary>
        ///     Gets or sets the position.
        /// </summary>
        ///
        /// <value>
        ///     The position.
        /// </value>
        public Vector2 Position
        {
            get => _position;
            set
            {
                _position = value;
                DestinationRectangle = MathHelpers.GetRoundedRectangleFromVectors(_position, _size);
            }
        }

        /// <summary>
        ///     Gets or sets the rotation.
        /// </summary>
        ///
        /// <value>
        ///     The rotation.
        /// </value>
        public float Rotation { get; set; }

        /// <summary>
        ///     Gets or sets the size.
        /// </summary>
        ///
        /// <value>
        ///     The size.
        /// </value>
        public virtual Vector2 Size
        {
            get => _size;
            set
            {
                _size = value;
                DestinationRectangle = MathHelpers.GetRoundedRectangleFromVectors(_position, _size);
            }
        }

        /// <summary>
        ///     Gets the type of the sprite.
        /// </summary>
        ///
        /// <value>
        ///     The type of the sprite.
        /// </value>
        public SpriteType SpriteType { get; }

        /// <summary>
        ///     Gets the texture.
        /// </summary>
        ///
        /// <value>
        ///     The texture.
        /// </value>
        internal abstract Textures.Texture Texture { get; }

        /// <summary>
        ///     Draws.
        /// </summary>
        ///
        /// <param name="spriteBatch"> The sprite batch. </param>
        public void Draw(SpriteBatch spriteBatch)
        {
            Draw(spriteBatch, DestinationRectangle, Color, Rotation, Origin, Effects, LayerDepth);
        }

        /// <summary>
        ///     Draws.
        /// </summary>
        ///
        /// <param name="spriteBatch">          The sprite batch. </param>
        /// <param name="destinationRectangle"> Destination rectangle. </param>
        /// <param name="color">                The color. </param>
        /// <param name="rotation">             The rotation. </param>
        /// <param name="origin">               The origin. </param>
        /// <param name="effects">              The effects. </param>
        /// <param name="layerDepth">           Depth of the layer. </param>
        public abstract void Draw(SpriteBatch spriteBatch, Rectangle destinationRectangle, Color color, float rotation, Vector2 origin, SpriteEffects effects, float layerDepth);

        /// <summary>
        ///     Updates the given gameTime.
        /// </summary>
        ///
        /// <param name="gameTime"> The game time. </param>
        public abstract void Update(GameTime gameTime);
    }
}