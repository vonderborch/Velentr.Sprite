using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Texture = Velentr.Sprite.Textures.Texture;

namespace Velentr.Sprite
{
    /// <summary>
    /// Extensions to SpriteBatch.Draw to handle Velentr.Sprites
    /// </summary>
    // ReSharper disable once CheckNamespace
    // ReSharper disable once UnusedMember.Global
    public static class DrawTextureExtensions
    {
        /// <summary>   Submit a sprite for drawing in the current batch. </summary>
        ///
        /// <param name="spriteBatch">  The spriteBatch to act on. </param>
        /// <param name="texture">      The texture. </param>
        /// <param name="position">     The position. </param>
        /// <param name="color">        The color. </param>
        public static void Draw(this SpriteBatch spriteBatch, Texture texture, Vector2 position, Color color)
        {
            texture.Draw(spriteBatch, position, color);
        }

        /// <summary>   Submit a sprite for drawing in the current batch. </summary>
        ///
        /// <param name="spriteBatch">          The spriteBatch to act on. </param>
        /// <param name="texture">              The texture. </param>
        /// <param name="destinationRectangle"> Destination rectangle. </param>
        /// <param name="sourceRectangle">      Source rectangle. </param>
        /// <param name="color">                The color. </param>
        public static void Draw(this SpriteBatch spriteBatch, Texture texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color)
        {
            texture.Draw(spriteBatch, destinationRectangle, sourceRectangle, color);
        }

        /// <summary>   Submit a sprite for drawing in the current batch. </summary>
        ///
        /// <param name="spriteBatch">      The spriteBatch to act on. </param>
        /// <param name="texture">          The texture. </param>
        /// <param name="position">         The position. </param>
        /// <param name="sourceRectangle">  Source rectangle. </param>
        /// <param name="color">            The color. </param>
        public static void Draw(this SpriteBatch spriteBatch, Texture texture, Vector2 position, Rectangle? sourceRectangle, Color color)
        {
            texture.Draw(spriteBatch, position, sourceRectangle, color);
        }

        /// <summary>   Submit a sprite for drawing in the current batch. </summary>
        ///
        /// <param name="spriteBatch">          The spriteBatch to act on. </param>
        /// <param name="texture">              The texture. </param>
        /// <param name="destinationRectangle"> Destination rectangle. </param>
        /// <param name="sourceRectangle">      Source rectangle. </param>
        /// <param name="color">                The color. </param>
        /// <param name="rotation">             The rotation. </param>
        /// <param name="origin">               The origin. </param>
        /// <param name="effects">              The effects. </param>
        /// <param name="layerDepth">           Depth of the layer. </param>
        public static void Draw(this SpriteBatch spriteBatch, Texture texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, SpriteEffects effects, float layerDepth)
        {
            texture.Draw(spriteBatch, destinationRectangle, sourceRectangle, color, rotation, origin, effects, layerDepth);
        }

        /// <summary>   Submit a sprite for drawing in the current batch. </summary>
        ///
        /// <param name="spriteBatch">      The spriteBatch to act on. </param>
        /// <param name="texture">          The texture. </param>
        /// <param name="position">         The position. </param>
        /// <param name="sourceRectangle">  Source rectangle. </param>
        /// <param name="color">            The color. </param>
        /// <param name="rotation">         The rotation. </param>
        /// <param name="origin">           The origin. </param>
        /// <param name="scale">            The scale. </param>
        /// <param name="effects">          The effects. </param>
        /// <param name="layerDepth">       Depth of the layer. </param>
        public static void Draw(this SpriteBatch spriteBatch, Texture texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
        {
            texture.Draw(spriteBatch, position, sourceRectangle, color, rotation, origin, scale, effects, layerDepth);
        }

        /// <summary>   Submit a sprite for drawing in the current batch. </summary>
        ///
        /// <param name="spriteBatch">      The spriteBatch to act on. </param>
        /// <param name="texture">          The texture. </param>
        /// <param name="position">         The position. </param>
        /// <param name="sourceRectangle">  Source rectangle. </param>
        /// <param name="color">            The color. </param>
        /// <param name="rotation">         The rotation. </param>
        /// <param name="origin">           The origin. </param>
        /// <param name="scale">            The scale. </param>
        /// <param name="effects">          The effects. </param>
        /// <param name="layerDepth">       Depth of the layer. </param>
        public static void Draw(this SpriteBatch spriteBatch, Texture texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
        {
            texture.Draw(spriteBatch, position, sourceRectangle, color, rotation, origin, scale, effects, layerDepth);
        }
    }
}