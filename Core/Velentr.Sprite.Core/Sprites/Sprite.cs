using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Velentr.Sprite.Animations;

namespace Velentr.Sprite.Sprites
{
    /// <summary>
    ///     A sprite that can represent any implementation of Base.Sprite
    /// </summary>
    ///
    /// <seealso cref="Velentr.Sprite.Sprites.Base.VelentrSprite"/>
    public class Sprite : Base.VelentrSprite
    {
        /// <summary>
        ///     The internal sprite.
        /// </summary>
        private readonly Base.VelentrSprite _internalSprite;

        /// <summary>
        ///     Constructor.
        /// </summary>
        ///
        /// <param name="manager"> The manager. </param>
        /// <param name="name">    The name. </param>
        /// <param name="texture"> The texture. </param>
        /// <param name="size">    The size. </param>
        public Sprite(TextureManager manager, string name, string texture, Vector2 size) : base(manager, name, size, SpriteType.Omni)
        {
            _internalSprite = new SimpleSprite(manager, name, texture, size);
            SpriteSubType = SpriteType.Simple;
        }

        /// <summary>
        ///     Constructor.
        /// </summary>
        ///
        /// <param name="manager">    The manager. </param>
        /// <param name="name">       The name. </param>
        /// <param name="texture">    The texture. </param>
        /// <param name="position">   The position. </param>
        /// <param name="size">       The size. </param>
        /// <param name="color">      (Optional) The color. </param>
        /// <param name="rotation">   (Optional) The rotation. </param>
        /// <param name="origin">     (Optional) The origin. </param>
        /// <param name="effects">    (Optional) The effects. </param>
        /// <param name="layerDepth"> (Optional) Depth of the layer. </param>
        public Sprite(TextureManager manager, string name, string texture, Vector2 position, Vector2 size, Color? color = null, float rotation = 0, Vector2? origin = null, SpriteEffects effects = SpriteEffects.None, float layerDepth = 0) : base(manager, name, position, size, SpriteType.Omni, color, rotation, origin, effects, layerDepth)
        {
            _internalSprite = new SimpleSprite(manager, name, texture, size);
            SpriteSubType = SpriteType.Simple;
        }

        /// <summary>
        ///     Constructor.
        /// </summary>
        ///
        /// <param name="manager">    The manager. </param>
        /// <param name="name">       The name. </param>
        /// <param name="size">       The size. </param>
        /// <param name="animations"> A variable-length parameters list containing animations. </param>
        public Sprite(TextureManager manager, string name, Vector2 size, params Animation[] animations) : base(manager, name, size, SpriteType.Animated)
        {
            _internalSprite = new AnimatedSprite(manager, name, size, animations);
            SpriteSubType = SpriteType.Animated;
        }

        /// <summary>
        ///     Constructor.
        /// </summary>
        ///
        /// <param name="manager">    The manager. </param>
        /// <param name="name">       The name. </param>
        /// <param name="position">   The position. </param>
        /// <param name="size">       The size. </param>
        /// <param name="color">      (Optional) The color. </param>
        /// <param name="rotation">   (Optional) The rotation. </param>
        /// <param name="origin">     (Optional) The origin. </param>
        /// <param name="effects">    (Optional) The effects. </param>
        /// <param name="layerDepth"> (Optional) Depth of the layer. </param>
        /// <param name="animations"> A variable-length parameters list containing animations. </param>
        public Sprite(TextureManager manager, string name, Vector2 position, Vector2 size, Color? color = null, float rotation = 0, Vector2? origin = null, SpriteEffects effects = SpriteEffects.None, float layerDepth = 0, params Animation[] animations) : base(manager, name, position, size, SpriteType.Animated, color, rotation, origin, effects, layerDepth)
        {
            _internalSprite = new AnimatedSprite(manager, name, position, size, color, rotation, origin, effects, layerDepth, animations);
            SpriteSubType = SpriteType.Animated;
        }

        /// <summary>
        ///     Constructor.
        /// </summary>
        ///
        /// <param name="manager"> The manager. </param>
        /// <param name="name">    The name. </param>
        /// <param name="size">    The size. </param>
        /// <param name="sprites"> The sprites. </param>
        public Sprite(TextureManager manager, string name, Vector2 size, params Base.VelentrSprite[] sprites) : base(manager, name, size, SpriteType.Composite)
        {
            _internalSprite = new CompositeSprite(manager, name, size, sprites);
            SpriteSubType = SpriteType.Composite;
        }

        /// <summary>
        ///     Constructor.
        /// </summary>
        ///
        /// <param name="manager">    The manager. </param>
        /// <param name="name">       The name. </param>
        /// <param name="position">   The position. </param>
        /// <param name="size">       The size. </param>
        /// <param name="color">      (Optional) The color. </param>
        /// <param name="rotation">   (Optional) The rotation. </param>
        /// <param name="origin">     (Optional) The origin. </param>
        /// <param name="effects">    (Optional) The effects. </param>
        /// <param name="layerDepth"> (Optional) Depth of the layer. </param>
        /// <param name="sprites">    The sprites. </param>
        public Sprite(TextureManager manager, string name, Vector2 position, Vector2 size, Color? color = null, float rotation = 0, Vector2? origin = null, SpriteEffects effects = SpriteEffects.None, float layerDepth = 0, params Base.VelentrSprite[] sprites) : base(manager, name, position, size, SpriteType.Composite, color, rotation, origin, effects, layerDepth)
        {
            _internalSprite = new CompositeSprite(manager, name, position, size, color, rotation, origin, effects, layerDepth, sprites);
            SpriteSubType = SpriteType.Composite;
        }

        /// <summary>
        ///     Constructor.
        /// </summary>
        ///
        /// <param name="manager">        The manager. </param>
        /// <param name="name">           The name. </param>
        /// <param name="size">           The size. </param>
        /// <param name="internalSprite"> The internal sprite. </param>
        public Sprite(TextureManager manager, string name, Vector2 size, Base.VelentrSprite internalSprite) : base(manager, name, size, SpriteType.Omni)
        {
            _internalSprite = internalSprite;
            SpriteSubType = _internalSprite.SpriteType;
        }

        /// <summary>
        ///     Constructor.
        /// </summary>
        ///
        /// <param name="manager">        The manager. </param>
        /// <param name="name">           The name. </param>
        /// <param name="position">       The position. </param>
        /// <param name="size">           The size. </param>
        /// <param name="internalSprite"> The internal sprite. </param>
        /// <param name="color">          (Optional) The color. </param>
        /// <param name="rotation">       (Optional) The rotation. </param>
        /// <param name="origin">         (Optional) The origin. </param>
        /// <param name="effects">        (Optional) The effects. </param>
        /// <param name="layerDepth">     (Optional) Depth of the layer. </param>
        public Sprite(TextureManager manager, string name, Vector2 position, Vector2 size, Base.VelentrSprite internalSprite, Color? color = null, float rotation = 0, Vector2? origin = null, SpriteEffects effects = SpriteEffects.None, float layerDepth = 0) : base(manager, name, position, size, SpriteType.Omni, color, rotation, origin, effects, layerDepth)
        {
            _internalSprite = internalSprite;
            SpriteSubType = _internalSprite.SpriteType;
        }

        /// <summary>
        ///     Gets the internal sprite.
        /// </summary>
        ///
        /// <value>
        ///     The internal sprite.
        /// </value>
        public Base.VelentrSprite InternalSprite => _internalSprite;

        /// <summary>
        ///     Gets the type of the sprite sub.
        /// </summary>
        ///
        /// <value>
        ///     The type of the sprite sub.
        /// </value>
        public SpriteType SpriteSubType { get; }

        /// <summary>
        ///     Gets the texture.
        /// </summary>
        ///
        /// <value>
        ///     The texture.
        /// </value>
        ///
        /// <seealso cref="Velentr.Sprite.Sprites.Base.VelentrSprite.Texture"/>
        internal override Textures.Texture Texture => _internalSprite.Texture;

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
        ///
        /// <seealso cref="Velentr.Sprite.Sprites.Base.VelentrSprite.Draw(SpriteBatch,Rectangle,Color,float,Vector2,SpriteEffects,float)"/>
        public override void Draw(SpriteBatch spriteBatch, Rectangle destinationRectangle, Color color, float rotation, Vector2 origin, SpriteEffects effects, float layerDepth)
        {
            _internalSprite.Draw(spriteBatch, destinationRectangle, color, rotation, origin, effects, layerDepth);
        }

        /// <summary>
        ///     Updates the given gameTime.
        /// </summary>
        ///
        /// <param name="gameTime"> The game time. </param>
        ///
        /// <seealso cref="Velentr.Sprite.Sprites.Base.VelentrSprite.Update(GameTime)"/>
        public override void Update(GameTime gameTime)
        {
            _internalSprite.Update(gameTime);
        }
    }
}