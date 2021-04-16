using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Velentr.Sprite.Helpers;

namespace Velentr.Sprite.Textures
{
    /// <summary>   Defines a Velentr.Texture Texture object. </summary>
    ///
    /// <seealso cref="IComparable{Texture}"/>
    [DebuggerDisplay("Name: {Name} ({DrawsSinceLastBalancing})")]
    public class Texture : IComparable<Texture>
    {
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="name">                 The name. </param>
        /// <param name="path">                 Full pathname of the file. </param>
        /// <param name="atlas">                The atlas. </param>
        /// <param name="textureBoundaries">    The texture boundaries. </param>
        /// <param name="textureSize">          Size of the texture. </param>
        internal Texture(string name, string path, TextureAtlas atlas, Rectangle textureBoundaries, Point textureSize)
        {
            Name = name;
            Path = path;
            Atlas = atlas;
            TextureBoundaries = textureBoundaries;
            TextureSize = textureSize;
            _drawsSinceLastBalancing = 0;
        }

        /// <summary>   Gets the name. </summary>
        ///
        /// <value> The name. </value>
        public string Name { get; }

        /// <summary>   Gets the full pathname of the file. </summary>
        ///
        /// <value> The full pathname of the file. </value>
        public string Path { get; }

        /// <summary>   The draws since last balancing. </summary>
        private long _drawsSinceLastBalancing;

        /// <summary>   Gets the draws since last balancing. </summary>
        ///
        /// <value> The draws since last balancing. </value>
        public long DrawsSinceLastBalancing => _drawsSinceLastBalancing;

        /// <summary>   Gets the atlas. </summary>
        ///
        /// <value> The atlas. </value>
        public TextureAtlas Atlas { get; }

        /// <summary>   Gets the texture size. </summary>
        ///
        /// <value> The size of the texture. </value>
        public Point TextureSize { get; }

        /// <summary>   Gets the width. </summary>
        ///
        /// <value> The width. </value>
        public int Width => TextureSize.X;

        /// <summary>   Gets the height. </summary>
        ///
        /// <value> The height. </value>
        public int Height => TextureSize.Y;

        /// <summary>   Gets the area. </summary>
        ///
        /// <value> The area. </value>
        public int Area => Width * Height;

        /// <summary>   Gets the texture boundaries. </summary>
        ///
        /// <value> The texture boundaries. </value>
        public Rectangle TextureBoundaries { get; }

        /// <summary>   Implicit cast that converts the given Texture to a TextureLoadInfo. </summary>
        ///
        /// <param name="value">    The value. </param>
        ///
        /// <returns>   The result of the operation. </returns>
        public static implicit operator TextureLoadInfo(Texture value)
        {
            return new TextureLoadInfo(value.Name, value.Path, value.TextureSize, true);
        }

        /// <summary>   Updates the draw count. </summary>
        ///
        /// <returns>   A long. </returns>
        public long UpdateDrawCount()
        {
            _drawsSinceLastBalancing += 1;
            return _drawsSinceLastBalancing;
        }

        /// <summary>   Submit a sprite for drawing in the current batch. </summary>
        ///
        /// <param name="spriteBatch">  The sprite batch. </param>
        /// <param name="position">     The position. </param>
        /// <param name="color">        The color. </param>
        public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color)
        {
            UpdateDrawCount();
            spriteBatch.Draw(Atlas.Texture, position, TextureBoundaries, color);
        }

        /// <summary>   Submit a sprite for drawing in the current batch. </summary>
        ///
        /// <param name="spriteBatch">          The sprite batch. </param>
        /// <param name="destinationRectangle"> Destination rectangle. </param>
        /// <param name="sourceRectangle">      Source rectangle. </param>
        /// <param name="color">                The color. </param>
        public void Draw(SpriteBatch spriteBatch, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color)
        {
            UpdateDrawCount();
            spriteBatch.Draw(Atlas.Texture, destinationRectangle, GetSourceRectangle(sourceRectangle), color);
        }

        /// <summary>   Submit a sprite for drawing in the current batch. </summary>
        ///
        /// <param name="spriteBatch">      The sprite batch. </param>
        /// <param name="position">         The position. </param>
        /// <param name="sourceRectangle">  Source rectangle. </param>
        /// <param name="color">            The color. </param>
        public void Draw(SpriteBatch spriteBatch, Vector2 position, Rectangle? sourceRectangle, Color color)
        {
            UpdateDrawCount();
            spriteBatch.Draw(Atlas.Texture, position, GetSourceRectangle(sourceRectangle), color);
        }

        /// <summary>   Submit a sprite for drawing in the current batch. </summary>
        ///
        /// <param name="spriteBatch">          The sprite batch. </param>
        /// <param name="destinationRectangle"> Destination rectangle. </param>
        /// <param name="sourceRectangle">      Source rectangle. </param>
        /// <param name="color">                The color. </param>
        /// <param name="rotation">             The rotation. </param>
        /// <param name="origin">               The origin. </param>
        /// <param name="effects">              The effects. </param>
        /// <param name="layerDepth">           Depth of the layer. </param>
        public void Draw(SpriteBatch spriteBatch, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, SpriteEffects effects, float layerDepth)
        {
            UpdateDrawCount();
            spriteBatch.Draw(Atlas.Texture, destinationRectangle, GetSourceRectangle(sourceRectangle), color, rotation, origin, effects, layerDepth);
        }

        /// <summary>   Submit a sprite for drawing in the current batch. </summary>
        ///
        /// <param name="spriteBatch">      The sprite batch. </param>
        /// <param name="position">         The position. </param>
        /// <param name="sourceRectangle">  Source rectangle. </param>
        /// <param name="color">            The color. </param>
        /// <param name="rotation">         The rotation. </param>
        /// <param name="origin">           The origin. </param>
        /// <param name="scale">            The scale. </param>
        /// <param name="effects">          The effects. </param>
        /// <param name="layerDepth">       Depth of the layer. </param>
        public void Draw(SpriteBatch spriteBatch, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
        {
            UpdateDrawCount();
            spriteBatch.Draw(Atlas.Texture, position, GetSourceRectangle(sourceRectangle), color, rotation, origin, scale, effects, layerDepth);
        }

        /// <summary>   Submit a sprite for drawing in the current batch. </summary>
        ///
        /// <param name="spriteBatch">      The sprite batch. </param>
        /// <param name="position">         The position. </param>
        /// <param name="sourceRectangle">  Source rectangle. </param>
        /// <param name="color">            The color. </param>
        /// <param name="rotation">         The rotation. </param>
        /// <param name="origin">           The origin. </param>
        /// <param name="scale">            The scale. </param>
        /// <param name="effects">          The effects. </param>
        /// <param name="layerDepth">       Depth of the layer. </param>
        public void Draw(SpriteBatch spriteBatch, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
        {
            UpdateDrawCount();
            spriteBatch.Draw(Atlas.Texture, position, GetSourceRectangle(sourceRectangle), color, rotation, origin, scale, effects, layerDepth);
        }

        /// <summary>   Gets source rectangle. </summary>
        ///
        /// <param name="sourceRectangle">  Source rectangle. </param>
        ///
        /// <returns>   The source rectangle. </returns>
        private Rectangle GetSourceRectangle(Rectangle? sourceRectangle)
        {
            return sourceRectangle == null
                ? TextureBoundaries
                : MathHelpers.ScaleRectangle((Rectangle)sourceRectangle, TextureBoundaries);
        }

        /// <summary>
        /// Compares this Texture object to another to determine their relative ordering.
        /// </summary>
        ///
        /// <param name="other">    Another instance to compare. </param>
        ///
        /// <returns>
        /// Negative if this object is less than the other, 0 if they are equal, or positive if this is
        /// greater.
        /// </returns>
        public int CompareTo(Texture other)
        {
            var comparisons = new List<(object, object, char)>()
            {
                (this.DrawsSinceLastBalancing, other.DrawsSinceLastBalancing, 'l'),
                (this.Width, other.Width, 'i'),
                (this.Height, other.Height, 'i'),
            };

            var c = 0;
            for (var i = 0; i < comparisons.Count; i++)
            {
                switch (comparisons[i].Item3)
                {
                    case 'l':
                        c = -((long)comparisons[i].Item1).CompareTo((long)comparisons[i].Item2);
                        break;
                    case 'i':
                        c = -((int)comparisons[i].Item1).CompareTo((int)comparisons[i].Item2);
                        break;
                }

                if (c != 0)
                {
                    return c;
                }
            }

            return this.Name.CompareTo(other.Name);
        }
    }
}