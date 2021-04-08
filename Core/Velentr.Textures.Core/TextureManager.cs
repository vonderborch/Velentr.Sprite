using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Velentr.Input
{
    public class TextureManager
    {

        public static int DEFAULT_REACH_TEXTURE_SIZE = 1024;

        public static int DEFAULT_HIDEF_TEXTURE_SIZE = 2048;

        public static SurfaceFormat DEFAULT_SURFACE_FORMAT = SurfaceFormat.Bgra4444;

        private Dictionary<string, TextureInfo> _textures;

        private List<TextureAtlas> _atlases;

        public TextureManager(GraphicsDevice graphicsDevice, int? maxAtlasWidth = null, int? maxAtlasHeight = null, SurfaceFormat? surfaceFormat = null)
        {
            GraphicsDevice = graphicsDevice;

            MaxAtlasWidth = maxAtlasWidth ?? (GraphicsDevice.GraphicsProfile == GraphicsProfile.Reach ? DEFAULT_REACH_TEXTURE_SIZE : DEFAULT_HIDEF_TEXTURE_SIZE);
            MaxAtlasHeight = maxAtlasHeight ?? (GraphicsDevice.GraphicsProfile == GraphicsProfile.Reach ? DEFAULT_REACH_TEXTURE_SIZE : DEFAULT_HIDEF_TEXTURE_SIZE);
            SurfaceFormat = surfaceFormat ?? DEFAULT_SURFACE_FORMAT;

            _textures = new Dictionary<string, TextureInfo>();
            _atlases = new List<TextureAtlas>();
        }

        public GraphicsDevice GraphicsDevice { get; set; }

        public int MaxAtlasWidth { get; set; }

        public int MaxAtlasHeight { get; set; }

        public bool AllowTextureAtlasScalingForExtraLargeTextures { get; set; } = true;

        public bool AllowTextureAtlasRebalance { get; set; } = true;

        private GameTime lastRebalanceTime { get; set; }

        public int TextureAtlasRebalanceIntervalMilliseconds { get; set; } = 300000; // defaults to 5 minutes

        public double AtlasScalingForExtraLargeTextureRatio { get; set; } = 2;

        public SurfaceFormat SurfaceFormat { get; set; }

        public void Setup()
        {

        }

        public void LoadTexture(string name, string pathToTexture, bool forceOverride = false)
        {
            if (!forceOverride && _textures.ContainsKey(name))
            {
                throw new Exception($"A texture with the name [{name}] has already been loaded!");
            }

            var texture = Texture2D.FromFile(GraphicsDevice, pathToTexture);
            InternalLoadTexture(name, texture);
        }

        public void LoadTexture(string name, Stream fileStream, bool forceOverride = false)
        {
            if (!forceOverride && _textures.ContainsKey(name))
            {
                throw new Exception($"A texture with the name [{name}] has already been loaded!");
            }

            var texture = Texture2D.FromStream(GraphicsDevice, fileStream);
            InternalLoadTexture(name, texture);
        }

        private void InternalLoadTexture(string name, Texture2D texture)
        {
            var atlas = _atlases.FirstOrDefault(a => !a.Full);
            if (atlas == null)
            {
                atlas = CreateNewTextureAtlas(texture.Width, texture.Height);
                _atlases.Add(atlas);
            }

            if (!atlas.AddTextureToAtlas(name, texture, out var info))
            {
                atlas = CreateNewTextureAtlas(texture.Width, texture.Height);
                _atlases.Add(atlas);
                if (!atlas.AddTextureToAtlas(name, texture, out info))
                {
                    throw new Exception($"Failed to add texture [{name}] to any atlas!");
                }
            }

            _textures[name] = info;
        }

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

        public void Update(GameTime gameTime)
        {
            if (AllowTextureAtlasRebalance)
            {
                
            }
        }

        private void RebalanceTextureAtlii()
        {

        }
    }
}
