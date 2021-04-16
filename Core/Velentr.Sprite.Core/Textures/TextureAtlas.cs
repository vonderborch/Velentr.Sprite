using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;

namespace Velentr.Sprite.Textures
{
    /// <summary>   A texture atlas. </summary>
    [DebuggerDisplay("{RemainingAreaPercent}% remaining / {UsedAreaPercent}% used / {TextureCount} textures")]
    public class TextureAtlas
    {
        /// <summary>   The textures. </summary>
        private Dictionary<string, Texture> _textures;

        /// <summary>   The texture. </summary>
        public Texture2D Texture;

        /// <summary>   The occupied. </summary>
        private bool[,] _occupied;

        /// <summary>   The buffer. </summary>
        private static Color[] _buffer;

        /// <summary>   Constructor. </summary>
        ///
        /// <param name="manager">          The manager. </param>
        /// <param name="width">            The width. </param>
        /// <param name="height">           The height. </param>
        /// <param name="surfaceFormat">    The surface format. </param>
        public TextureAtlas(SpriteManager manager, int width, int height, SurfaceFormat surfaceFormat)
        {
            Manager = manager;
            TextureAtlasBoundaries = new Rectangle(0, 0, width, height);
            SurfaceFormat = surfaceFormat;

            _textures = new Dictionary<string, Texture>();
            _occupied = new bool[width, height];

            Texture = new Texture2D(manager.GraphicsDevice, Width, Height, false, SurfaceFormat.Color);
            RemainingArea = width * height;
            TotalArea = width * height;
        }

        /// <summary>   Gets the manager. </summary>
        ///
        /// <value> The manager. </value>
        public SpriteManager Manager { get; }

        /// <summary>   Gets the texture atlas boundaries. </summary>
        ///
        /// <value> The texture atlas boundaries. </value>
        private Rectangle TextureAtlasBoundaries { get; }

        /// <summary>   Gets the width. </summary>
        ///
        /// <value> The width. </value>
        public int Width => TextureAtlasBoundaries.Width;

        /// <summary>   Gets the height. </summary>
        ///
        /// <value> The height. </value>
        public int Height => TextureAtlasBoundaries.Height;

        /// <summary>   Gets the surface format. </summary>
        ///
        /// <value> The surface format. </value>
        public SurfaceFormat SurfaceFormat { get; }

        /// <summary>   Gets or sets the remaining area. </summary>
        ///
        /// <value> The remaining area. </value>
        public long RemainingArea { get; private set; }

        /// <summary>   Gets the total number of area. </summary>
        ///
        /// <value> The total number of area. </value>
        public long TotalArea { get; }

        /// <summary>   Gets the used area. </summary>
        ///
        /// <value> The used area. </value>
        public long UsedArea => TotalArea - RemainingArea;

        /// <summary>   Gets the remaining area percent. </summary>
        ///
        /// <value> The remaining area percent. </value>
        public double RemainingAreaPercent => Math.Round(RemainingArea / (double)TotalArea * 100, 2);

        /// <summary>   Gets the used area percent. </summary>
        ///
        /// <value> The used area percent. </value>
        public double UsedAreaPercent => Math.Round(UsedArea / (double)TotalArea * 100, 2);

        /// <summary>   Gets the number of textures. </summary>
        ///
        /// <value> The number of textures. </value>
        public int TextureCount => _textures.Count;

        /// <summary>   Gets the textures. </summary>
        ///
        /// <value> The textures. </value>
        public Dictionary<string, Texture> Textures => _textures;

        /// <summary>   Adds a texture to atlas. </summary>
        ///
        /// <param name="name">         The name. </param>
        /// <param name="path">         Full pathname of the file. </param>
        /// <param name="texture">      The texture. </param>
        /// <param name="textureSize">  Size of the texture. </param>
        /// <param name="info">         [out] The information. </param>
        ///
        /// <returns>   True if it succeeds, false if it fails. </returns>
        public bool AddTextureToAtlas(string name, string path, Texture2D texture, Point textureSize, out Texture info)
        {
            // find the first available slot that fits the texture's coordinates
            if (CanBePlaced(texture.Bounds, out var boundaries))
            {
                // indicate that we're occupying the area and reduce our remaining area...
                for (var x = 0; x < boundaries.Width; x++)
                {
                    for (var y = 0; y < boundaries.Height; y++)
                    {
                        _occupied[x + boundaries.X, y + boundaries.Y] = true;
                    }
                }
                RemainingArea -= boundaries.Width * boundaries.Height;

                // assign the texture's data to the bounds
                _buffer = new Color[(texture.Height * texture.Width)];
                texture.GetData<Color>(_buffer, 0, _buffer.Length);
                _textures.Add(name, new Texture(name, path, this, boundaries, textureSize));
                Texture.SetData<Color>(0, boundaries, _buffer, 0, _buffer.Length);

                // return back that we succeeded!
                info = new Texture(name, path, this, boundaries, textureSize);
                return true;
            }

            // return back that we failed!
            info = null;
            return false;
        }

        /// <summary>   Determine if a rectangle can be placed on this Atlas. </summary>
        ///
        /// <param name="boundaries">       The boundaries. </param>
        /// <param name="outBoundaries">    [out] The out boundaries. </param>
        ///
        /// <returns>   True if we can be placed, false if not. </returns>
        private bool CanBePlaced(Rectangle boundaries, out Rectangle outBoundaries)
        {
            outBoundaries = Rectangle.Empty;
            // return early if our atlas bounds are too small for the texture, or the atlas is full
            if (boundaries.Width > TextureAtlasBoundaries.Width || boundaries.Height > TextureAtlasBoundaries.Height)
            {
                return false;
            }

            var trial = boundaries;
            for (var y = 0; y < Height - boundaries.Height; y++)
            {
                for (var x = 0; x < Width - boundaries.Width; x++)
                {
                    trial.X = x;
                    trial.Y = y;

                    // if none of the four corners of our current area are occupied, we may have a valid area for our rectangle
                    if (
                        !_occupied[trial.Left, trial.Top]
                        && !_occupied[trial.Left, trial.Bottom]
                        && !_occupied[trial.Right, trial.Top]
                        && !_occupied[trial.Right, trial.Bottom]
                    )
                    {
                        // if we intersect any textures that have already been placed, we've got an invalid location and should move on to the next location...
                        var valid = true;
                        var rect = new Rectangle(x, y, boundaries.Width, boundaries.Height);
                        foreach (var texture in _textures)
                        {
                            if (rect.Intersects(texture.Value.TextureBoundaries))
                            {
                                valid = false;
                                break;
                            }
                        }

                        if (valid)
                        {
                            outBoundaries = trial;
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>   Query if we have slot for a texture. </summary>
        ///
        /// <param name="textureBoundaries">    The texture boundaries. </param>
        ///
        /// <returns>   True if slot for texture, false if not. </returns>
        public bool HasSlotForTexture(Rectangle textureBoundaries)
        {
            return CanBePlaced(textureBoundaries, out _);
        }

        /// <summary>   Query if we potentially have a slot for a texture. </summary>
        ///
        /// <param name="textureBoundaries">    The texture boundaries. </param>
        ///
        /// <returns>   True if potential slot for texture, false if not. </returns>
        public bool HasPotentialSlotForTexture(Rectangle textureBoundaries)
        {
            return (textureBoundaries.Width * textureBoundaries.Height) <= RemainingArea;
        }

        /// <summary>   Submit a sprite for drawing in the current batch. </summary>
        ///
        /// <param name="spriteBatch">  The sprite batch. </param>
        /// <param name="texture">      The texture. </param>
        /// <param name="position">     The position. </param>
        /// <param name="color">        The color. </param>
        public void Draw(SpriteBatch spriteBatch, string texture, Vector2 position, Color color)
        {
            _textures[texture].Draw(spriteBatch, position, color);
        }

        /// <summary>   Submit a sprite for drawing in the current batch. </summary>
        ///
        /// <param name="spriteBatch">          The sprite batch. </param>
        /// <param name="texture">              The texture. </param>
        /// <param name="destinationRectangle"> Destination rectangle. </param>
        /// <param name="sourceRectangle">      Source rectangle. </param>
        /// <param name="color">                The color. </param>
        public void Draw(SpriteBatch spriteBatch, string texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color)
        {
            _textures[texture].Draw(spriteBatch, destinationRectangle, sourceRectangle, color);
        }

        /// <summary>   Submit a sprite for drawing in the current batch. </summary>
        ///
        /// <param name="spriteBatch">      The sprite batch. </param>
        /// <param name="texture">          The texture. </param>
        /// <param name="position">         The position. </param>
        /// <param name="sourceRectangle">  Source rectangle. </param>
        /// <param name="color">            The color. </param>
        public void Draw(SpriteBatch spriteBatch, string texture, Vector2 position, Rectangle? sourceRectangle, Color color)
        {
            _textures[texture].Draw(spriteBatch, position, sourceRectangle, color);
        }

        /// <summary>   Submit a sprite for drawing in the current batch. </summary>
        ///
        /// <param name="spriteBatch">          The sprite batch. </param>
        /// <param name="texture">              The texture. </param>
        /// <param name="destinationRectangle"> Destination rectangle. </param>
        /// <param name="sourceRectangle">      Source rectangle. </param>
        /// <param name="color">                The color. </param>
        /// <param name="rotation">             The rotation. </param>
        /// <param name="origin">               The origin. </param>
        /// <param name="effects">              The effects. </param>
        /// <param name="layerDepth">           Depth of the layer. </param>
        public void Draw(SpriteBatch spriteBatch, string texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, SpriteEffects effects, float layerDepth)
        {
            _textures[texture].Draw(spriteBatch, destinationRectangle, sourceRectangle, color, rotation, origin, effects, layerDepth);
        }

        /// <summary>   Submit a sprite for drawing in the current batch. </summary>
        ///
        /// <param name="spriteBatch">      The sprite batch. </param>
        /// <param name="texture">          The texture. </param>
        /// <param name="position">         The position. </param>
        /// <param name="sourceRectangle">  Source rectangle. </param>
        /// <param name="color">            The color. </param>
        /// <param name="rotation">         The rotation. </param>
        /// <param name="origin">           The origin. </param>
        /// <param name="scale">            The scale. </param>
        /// <param name="effects">          The effects. </param>
        /// <param name="layerDepth">       Depth of the layer. </param>
        public void Draw(SpriteBatch spriteBatch, string texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
        {
            _textures[texture].Draw(spriteBatch, position, sourceRectangle, color, rotation, origin, scale, effects, layerDepth);
        }

        /// <summary>   Submit a sprite for drawing in the current batch. </summary>
        ///
        /// <param name="spriteBatch">      The sprite batch. </param>
        /// <param name="texture">          The texture. </param>
        /// <param name="position">         The position. </param>
        /// <param name="sourceRectangle">  Source rectangle. </param>
        /// <param name="color">            The color. </param>
        /// <param name="rotation">         The rotation. </param>
        /// <param name="origin">           The origin. </param>
        /// <param name="scale">            The scale. </param>
        /// <param name="effects">          The effects. </param>
        /// <param name="layerDepth">       Depth of the layer. </param>
        public void Draw(SpriteBatch spriteBatch, string texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
        {
            _textures[texture].Draw(spriteBatch, position, sourceRectangle, color, rotation, origin, scale, effects, layerDepth);
        }
    }
}