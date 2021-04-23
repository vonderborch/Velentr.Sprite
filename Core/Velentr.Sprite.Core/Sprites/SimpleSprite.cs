using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Velentr.Sprite.Sprites
{
    /// <summary>
    ///     A simple sprite.
    /// </summary>
    ///
    /// <seealso cref="Velentr.Sprite.Sprites.Base.VelentrSprite"/>
    public class SimpleSprite : Base.VelentrSprite
    {
        /// <summary>
        ///     Constructor.
        /// </summary>
        ///
        /// <param name="manager"> The manager. </param>
        /// <param name="name">    The name. </param>
        /// <param name="texture"> The texture. </param>
        /// <param name="size">    The size. </param>
        public SimpleSprite(TextureManager manager, string name, string texture, Vector2 size) : base(manager, name, size, SpriteType.Simple)
        {
            TextureName = texture;
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
        public SimpleSprite(TextureManager manager, string name, string texture, Vector2 position, Vector2 size, Color? color = null, float rotation = 0, Vector2? origin = null, SpriteEffects effects = SpriteEffects.None, float layerDepth = 0) : base(manager, name, position, size, SpriteType.Simple, color, rotation, origin, effects, layerDepth)
        {
            TextureName = texture;
        }

        /// <summary>
        ///     Gets the texture.
        /// </summary>
        ///
        /// <value>
        ///     The texture.
        /// </value>
        ///
        /// <seealso cref="Velentr.Sprite.Sprites.Base.VelentrSprite.Texture"/>
        internal override Textures.Texture Texture => Manager._textures[TextureName];

        /// <summary>
        ///     Gets or sets the name of the texture.
        /// </summary>
        ///
        /// <value>
        ///     The name of the texture.
        /// </value>
        public string TextureName { get; set; }

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
            Texture.Draw(spriteBatch, destinationRectangle, null, color, rotation, origin, effects, layerDepth);
        }

        /// <summary>
        ///     Updates the given gameTime.
        /// </summary>
        ///
        /// <param name="time"> The game time. </param>
        ///
        /// <seealso cref="Velentr.Sprite.Sprites.Base.VelentrSprite.Update(GameTime)"/>
        public override void Update(GameTime time)
        {
        }
    }
}