using ImageMagick;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Velentr.Collections.Collections;
using Velentr.Sprite.Helpers;
using Velentr.Sprite.Textures;
using Texture = Velentr.Sprite.Textures.Texture;

namespace Velentr.Sprite
{
    /// <summary>   Manager for textures. </summary>
    ///
    /// <seealso cref="IDisposable"/>
    public class TextureManager : IDisposable
    {
        /// <summary>   (Immutable) the default reach texture size. </summary>
        public const int DEFAULT_REACH_TEXTURE_SIZE = 2048;

        /// <summary>   (Immutable) the default hidef texture size. </summary>
        public const int DEFAULT_HIDEF_TEXTURE_SIZE = 4096;

        /// <summary>   (Immutable) the default surface format. </summary>
        public const SurfaceFormat DEFAULT_SURFACE_FORMAT = SurfaceFormat.Color;

        /// <summary>   The textures. </summary>
        internal Dictionary<string, Texture> _textures;

        /// <summary>   The last texture balancing order. </summary>
        private List<string> _lastTextureBalancingOrder;

        /// <summary>   The atlases. </summary>
        private List<TextureAtlas> _atlases;

        public Bank<string, Sprites.Base.VelentrSprite> RegisteredSprites;

        /// <summary>   Constructor. </summary>
        ///
        /// <param name="graphicsDevice">                                   The graphics device. </param>
        /// <param name="game">                                             (Optional) The game. If not set, balancing (if enabled) will be done on the main thread. </param>
        /// <param name="maxAtlasWidth">                                    (Optional) The width of the maximum atlas. Defaults to 2048 or 4096 depending on the GraphicsDevice.GraphicsProfile setting. </param>
        /// <param name="maxAtlasHeight">                                   (Optional) The height of the maximum atlas. Defaults to 2048 or 4096 depending on the GraphicsDevice.GraphicsProfile setting. </param>
        /// <param name="surfaceFormat">                                    (Optional) The surface format. Defaults to SurfaceFormat.Color.</param>
        /// <param name="autoTextureAtlasBalancingEnabled">                 (Optional) Whether texture atlas auto balancing is enabled. Defaults to false. </param>
        /// <param name="autoTextureAtlasBalancingIntervalMilliseconds">    (Optional) The time in milliseconds between texture auto-balancing attempts. Defaults to 300000 (five minutes). </param>
        public TextureManager(GraphicsDevice graphicsDevice, Game game = null, int? maxAtlasWidth = null, int? maxAtlasHeight = null, SurfaceFormat? surfaceFormat = null, bool autoTextureAtlasBalancingEnabled = false, uint autoTextureAtlasBalancingIntervalMilliseconds = 300000)
        {
            Game = game;
            GraphicsDevice = graphicsDevice;

            MaxAtlasWidth = maxAtlasWidth ?? (GraphicsDevice.GraphicsProfile == GraphicsProfile.Reach ? DEFAULT_REACH_TEXTURE_SIZE : DEFAULT_HIDEF_TEXTURE_SIZE);
            MaxAtlasHeight = maxAtlasHeight ?? (GraphicsDevice.GraphicsProfile == GraphicsProfile.Reach ? DEFAULT_REACH_TEXTURE_SIZE : DEFAULT_HIDEF_TEXTURE_SIZE);
            SurfaceFormat = surfaceFormat ?? DEFAULT_SURFACE_FORMAT;

            _textures = new Dictionary<string, Texture>();
            _atlases = new List<TextureAtlas>();

            _lastTextureBalancingOrder = new List<string>();
            _lastBalancingTime = TimeSpan.Zero;
            RegisteredSprites = new Bank<string, Sprites.Base.VelentrSprite>();

            AutoTextureAtlasBalancingEnabled = autoTextureAtlasBalancingEnabled;
            AutoTextureAtlasBalancingIntervalMilliseconds = autoTextureAtlasBalancingIntervalMilliseconds;
        }

        /// <summary>
        ///     Gets or sets the game.
        /// </summary>
        ///
        /// <value>
        ///     The game.
        /// </value>
        public Game Game { get; set; }

        /// <summary>   Gets or sets the graphics device. </summary>
        ///
        /// <value> The graphics device. </value>
        public GraphicsDevice GraphicsDevice { get; set; }

        /// <summary>   Gets or sets the width of the maximum atlas. </summary>
        ///
        /// <value> The width of the maximum atlas. </value>
        public int MaxAtlasWidth { get; set; }

        /// <summary>   Gets or sets the height of the maximum atlas. </summary>
        ///
        /// <value> The height of the maximum atlas. </value>
        public int MaxAtlasHeight { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether we allow texture atlas scaling for extra large
        /// textures.
        /// </summary>
        ///
        /// <value> True if allow texture atlas scaling for extra large textures, false if not. </value>
        public bool AllowTextureAtlasScalingForExtraLargeTextures { get; set; } = true;

        /// <summary>   True to enable, false to disable the automatic texture atlas balancing. </summary>
        private bool _autoTextureAtlasBalancingEnabled = false;

        /// <summary>
        /// Gets or sets a value indicating whether the automatic texture atlas balancing is enabled.
        /// </summary>
        ///
        /// <value> True if automatic texture atlas balancing enabled, false if not. </value>
        public bool AutoTextureAtlasBalancingEnabled
        {
            get => _autoTextureAtlasBalancingEnabled;
            set
            {
                _autoTextureAtlasBalancingEnabled = value;
                if (value)
                {
                    _lastTextureBalancingOrder = new List<string>();
                }
                else
                {
                    _lastTextureBalancingOrder = new List<string>();
                }
            }
        }

        /// <summary>   The last balancing time. </summary>
        private TimeSpan _lastBalancingTime;

        /// <summary>
        /// Gets or sets the automatic texture atlas balancing interval milliseconds.
        /// </summary>
        ///
        /// <value> The automatic texture atlas balancing interval milliseconds. </value>
        public uint AutoTextureAtlasBalancingIntervalMilliseconds { get; set; } = 300000; // defaults to 5 minutes

        /// <summary>   Gets or sets the atlas scaling for extra large texture ratio. </summary>
        ///
        /// <value> The atlas scaling for extra large texture ratio. </value>
        public double AtlasScalingForExtraLargeTextureRatio { get; set; } = 2;

        /// <summary>   Gets or sets the surface format. </summary>
        ///
        /// <value> The surface format. </value>
        public SurfaceFormat SurfaceFormat { get; set; }

        /// <summary>   Loads the textures. </summary>
        ///
        /// <param name="textureLoadInfo">  Information describing the texture load. </param>
        public void LoadTextures(List<TextureLoadInfo> textureLoadInfo)
        {
            for (var i = 0; i < textureLoadInfo.Count; i++)
            {
                LoadTexture(textureLoadInfo[i].Name, textureLoadInfo[i].Path, textureLoadInfo[i].TextureSize, textureLoadInfo[i].ForceOverride);
            }
        }

        /// <summary>   Loads a texture. </summary>
        ///
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>
        ///
        /// <param name="textureLoadInfo">  Information describing the texture load. </param>
        public void LoadTexture(TextureLoadInfo textureLoadInfo)
        {
            if (!textureLoadInfo.ForceOverride && _textures.ContainsKey(textureLoadInfo.Name))
            {
                throw new Exception($"A texture with the name [{textureLoadInfo.Name}] has already been loaded!");
            }

            InternalLoadTexture(textureLoadInfo, _atlases);
        }

        /// <summary>   Loads a texture. </summary>
        ///
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>
        ///
        /// <param name="name">             The name. </param>
        /// <param name="pathToTexture">    The path to texture. </param>
        /// <param name="textureSize">      (Optional) Size of the texture. </param>
        /// <param name="forceOverride">    (Optional) True to force override. </param>
        public void LoadTexture(string name, string pathToTexture, Point? textureSize = null, bool forceOverride = false)
        {
            if (!forceOverride && _textures.ContainsKey(name))
            {
                throw new Exception($"A texture with the name [{name}] has already been loaded!");
            }

            InternalLoadTexture(new TextureLoadInfo(name, pathToTexture, textureSize, forceOverride), _atlases);
        }

        /// <summary>   Internal load texture. </summary>
        ///
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>
        ///
        /// <param name="textureLoadInfo">  Information describing the texture load. </param>
        /// <param name="atlases">          The atlases. </param>
        ///
        /// <returns>   A List&lt;TextureAtlas&gt; </returns>
        private List<TextureAtlas> InternalLoadTexture(TextureLoadInfo textureLoadInfo, List<TextureAtlas> atlases)
        {
            var results = GetTexture(textureLoadInfo);
            var texture = results.Item1;
            var size = results.Item2;

            var working = atlases.Where(a => a.HasPotentialSlotForTexture(texture.Bounds)).ToList();

            if (working.Count == 0)
            {
                var atlas = CreateNewTextureAtlas(texture.Width, texture.Height);
                atlases.Add(atlas);
                working.Add(atlas);
            }

            Texture textureInfo;
            for (var i = 0; i < working.Count; i++)
            {
                if (working[i].AddTextureToAtlas(textureLoadInfo.Name, textureLoadInfo.Path, texture, size, out textureInfo))
                {
                    _textures[textureLoadInfo.Name] = textureInfo;
                    return atlases;
                }
            }

            var backupAtlas = CreateNewTextureAtlas(texture.Width, texture.Height);
            atlases.Add(backupAtlas);
            if (!backupAtlas.AddTextureToAtlas(textureLoadInfo.Name, textureLoadInfo.Path, texture, size, out textureInfo))
            {
                throw new Exception($"Failed to add texture [{textureLoadInfo.Name}] to any atlas!");
            }

            _textures[textureLoadInfo.Name] = textureInfo;
            return atlases;
        }

        /// <summary>   Gets a texture. </summary>
        ///
        /// <param name="textureLoadInfo">  Information describing the texture load. </param>
        ///
        /// <returns>   The texture. </returns>
        private (Texture2D, Point) GetTexture(TextureLoadInfo textureLoadInfo)
        {
            Texture2D texture;
            Point size;
            if (textureLoadInfo.TextureSize == null)
            {
#if MONOGAME
                texture = Texture2D.FromFile(GraphicsDevice, textureLoadInfo.Path);
#elif FNA
                using (var stream = File.OpenRead(textureLoadInfo.Path))
                {
                    texture = Texture2D.FromStream(GraphicsDevice, stream);
                }
#else
                throw new Exception("Unsupported platform!");
#endif
                size = new Point(texture.Width, texture.Height);
            }
            else
            {
                size = (Point)textureLoadInfo.TextureSize;
                Stream stream;

                using (var image = new MagickImage(textureLoadInfo.Path))
                {
                    var imageSize = new MagickGeometry(size.X, size.Y)
                    {
                        IgnoreAspectRatio = true
                    };
                    image.Resize(imageSize);

                    stream = new MemoryStream(image.ToByteArray(GetImageFormat(textureLoadInfo.Path)));
                }

                texture = Texture2D.FromStream(GraphicsDevice, stream);
                stream.Dispose();
            }

            return (texture, size);
        }

        /// <summary>   Gets image format. </summary>
        ///
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>
        ///
        /// <param name="path"> Full pathname of the file. </param>
        ///
        /// <returns>   The image format. </returns>
        private MagickFormat GetImageFormat(string path)
        {
            switch (Path.GetExtension(path))
            {
                case ".jpg":
                    return MagickFormat.Jpg;

                case ".png":
                    return MagickFormat.Png;

                case ".tif":
                    return MagickFormat.Tif;

                case ".gif":
                    return MagickFormat.Gif;

                case ".bmp":
                    return MagickFormat.Bmp;

                case ".dds":
                    return MagickFormat.Dds;

                default:
                    throw new Exception("Invalid file type! Currently supported file types: .jpg, .png, .tif, .gif, .bmp, .dds");
            }
        }

        /// <summary>   Creates new texture atlas. </summary>
        ///
        /// <param name="textureWidth">     Width of the texture. </param>
        /// <param name="textureHeight">    Height of the texture. </param>
        ///
        /// <returns>   The new new texture atlas. </returns>
        private TextureAtlas CreateNewTextureAtlas(int textureWidth, int textureHeight)
        {
            var widthRatio = MaxAtlasWidth / (double)textureWidth;
            var heightRatio = MaxAtlasHeight / (double)textureHeight;
            var width = MaxAtlasWidth;
            var height = MaxAtlasHeight;

            if (AllowTextureAtlasScalingForExtraLargeTextures)
            {
                if (widthRatio < 1)
                {
                    width = (int)Math.Ceiling(textureWidth * AtlasScalingForExtraLargeTextureRatio);
                }
                if (heightRatio < 1)
                {
                    height = (int)Math.Ceiling(textureHeight * AtlasScalingForExtraLargeTextureRatio);
                }
            }

            return new TextureAtlas(this, width, height, SurfaceFormat);
        }

        /// <summary>   Updates the given gameTime. </summary>
        ///
        /// <param name="gameTime"> The game time. </param>
        public void Update(GameTime gameTime)
        {
            // re-balance texture atlases if required
            TimeInMillisecondsSinceLastReBalance = !AutoTextureAtlasBalancingEnabled
                ? 0
                : TimeHelpers.ElapsedMilliSeconds(_lastBalancingTime, gameTime.TotalGameTime);
            if (AutoTextureAtlasBalancingEnabled && TimeInMillisecondsSinceLastReBalance > AutoTextureAtlasBalancingIntervalMilliseconds)
            {
                BalanceTextureAtlases();
                _lastBalancingTime = gameTime.TotalGameTime;
            }

            // update all of the registered sprites (if any)
            foreach (var sprite in RegisteredSprites)
            {
                sprite.Value.Update(gameTime);
            }
        }

        /// <summary>
        ///     Gets or sets the time in milliseconds since last re balance.
        /// </summary>
        ///
        /// <value>
        ///     The time in milliseconds since last re balance.
        /// </value>
        public uint TimeInMillisecondsSinceLastReBalance { get; private set; }

        /// <summary>
        ///     Gets the time in milliseconds until last re balance.
        /// </summary>
        ///
        /// <value>
        ///     The time in milliseconds until last re balance.
        /// </value>
        public uint TimeInMillisecondsUntilLastReBalance => AutoTextureAtlasBalancingIntervalMilliseconds - TimeInMillisecondsSinceLastReBalance;

        /// <summary>
        ///     Registers the sprite for updates described by sprite.
        /// </summary>
        ///
        /// <param name="sprite"> The sprite. </param>
        public void RegisterSpriteForUpdates(Sprites.Base.VelentrSprite sprite)
        {
            RegisteredSprites.AddItem(sprite.Name, sprite);
        }

        /// <summary>
        ///     Registers the sprites for updates described by sprites.
        /// </summary>
        ///
        /// <param name="sprites"> The sprites. </param>
        public void RegisterSpritesForUpdates(List<Sprites.Base.VelentrSprite> sprites)
        {
            for (var i = 0; i < sprites.Count; i++)
            {
                RegisteredSprites.AddItem(sprites[i].Name, sprites[i]);
            }
        }

        /// <summary>
        ///     Unregisters the sprite from updates described by name.
        /// </summary>
        ///
        /// <param name="name"> The name. </param>
        public void UnregisterSpriteFromUpdates(string name)
        {
            RegisteredSprites.RemoveItem(name);
        }

        /// <summary>
        ///     Unregisters the sprites from updates described by names.
        /// </summary>
        ///
        /// <param name="names"> The names. </param>
        public void UnregisterSpritesFromUpdates(List<string> names)
        {
            for (var i = 0; i < names.Count; i++)
            {
                RegisteredSprites.RemoveItem(names[i]);
            }
        }

        /// <summary>   Balance texture atlases. </summary>
        public void BalanceTextureAtlases()
        {
            // grab our textures and sort them by how often they've been used...
            var textures = _textures.Select(x => x.Value).ToList();
            CollectionHelpers.QuickSortTextures(textures);

            // check if we can return early by having an identical usage order...
            var alreadyBalanced = false;
            if (textures.Count == _lastTextureBalancingOrder.Count)
            {
                alreadyBalanced = true;

                for (var i = 0; i < textures.Count; i++)
                {
                    if (textures[i].Name != _lastTextureBalancingOrder[i])
                    {
                        alreadyBalanced = false;
                        break;
                    }
                }
            }
            if (alreadyBalanced)
            {
                return;
            }

            // Re-balance the texture atlases based on the draw usage order...
            var newAtlases = new List<TextureAtlas>(_atlases.Count);
            _lastTextureBalancingOrder = new List<string>(_textures.Count);
            for (var i = 0; i < textures.Count; i++)
            {
                newAtlases = InternalLoadTexture(_textures[textures[i].Name], newAtlases);
                _lastTextureBalancingOrder.Add(textures[i].Name);
            }

            var oldAtlases = _atlases;
            var oldTextures = _textures;
            var newTextures = new Dictionary<string, Texture>();
            for (var i = 0; i < newAtlases.Count; i++)
            {
                var atlasTextures = newAtlases[i].Textures;
                foreach (var texture in atlasTextures)
                {
                    newTextures.Add(texture.Key, texture.Value);
                }
            }

            lock (_atlases)
            {
                lock (_textures)
                {
                    _textures = newTextures;
                    _atlases = newAtlases;
                }
            }

            for (var i = 0; i < oldAtlases.Count; i++)
            {
                oldAtlases[i].Texture.Dispose();
            }

            oldTextures.Clear();
            oldTextures = null;
            oldAtlases = null;
        }

        /// <summary>   Gets atlas texture mapping. </summary>
        ///
        /// <returns>   The atlas texture mapping. </returns>
        public Dictionary<int, List<Texture>> GetAtlasTextureMapping()
        {
            var output = new Dictionary<int, List<Texture>>();
            for (var i = 0; i < _atlases.Count; i++)
            {
                output.Add(i, new List<Texture>(_atlases[i].Textures.Values));
            }

            return output;
        }

        /// <summary>   Gets textures for atlas. </summary>
        ///
        /// <param name="atlasIndex">   Zero-based index of the atlas. </param>
        ///
        /// <returns>   The textures for atlas. </returns>
        public List<Texture> GetTexturesForAtlas(int atlasIndex)
        {
            return new List<Texture>(_atlases[atlasIndex].Textures.Values);
        }

        /// <summary>   Gets the number of atlas. </summary>
        ///
        /// <value> The number of atlas. </value>
        public int AtlasCount => _atlases.Count;

        /// <summary>   Draw full atlas. </summary>
        ///
        /// <param name="spriteBatch">  The sprite batch. </param>
        /// <param name="atlasIndex">   Zero-based index of the atlas. </param>
        /// <param name="position">     The position. </param>
        /// <param name="color">        The color. </param>
        public void DrawFullAtlas(SpriteBatch spriteBatch, int atlasIndex, Vector2 position, Color color)
        {
            spriteBatch.Draw(_atlases[atlasIndex].Texture, position, color);
            if (AutoTextureAtlasBalancingEnabled)
            {
                foreach (var texture in _atlases[atlasIndex].Textures)
                {
                    _textures[texture.Key].UpdateDrawCount();
                }
            }
        }

        /// <summary>   Draw full atlas. </summary>
        ///
        /// <param name="spriteBatch">  The sprite batch. </param>
        /// <param name="atlasIndex">   Zero-based index of the atlas. </param>
        /// <param name="position">     The position. </param>
        /// <param name="color">        The color. </param>
        public void DrawFullAtlas(SpriteBatch spriteBatch, int atlasIndex, Rectangle position, Color color)
        {
            spriteBatch.Draw(_atlases[atlasIndex].Texture, position, _atlases[atlasIndex].Texture.Bounds, color);
            if (AutoTextureAtlasBalancingEnabled)
            {
                foreach (var texture in _atlases[atlasIndex].Textures)
                {
                    _textures[texture.Key].UpdateDrawCount();
                }
            }
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

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged
        /// resources.
        /// </summary>
        ///
        /// <seealso cref="IDisposable.Dispose()"/>
        public void Dispose()
        {
            for (var i = 0; i < _atlases.Count; i++)
            {
                _atlases[i].Texture.Dispose();
            }
        }
    }
}