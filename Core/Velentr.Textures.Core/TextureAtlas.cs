using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Velentr.Input
{
    internal class TextureAtlas
    {

        internal Texture2D Texture;

        public TextureAtlas(TextureManager manager, int width, int height, SurfaceFormat surfaceFormat)
        {
            Manager = manager;
            Width = width;
            Height = height;
            SurfaceFormat = surfaceFormat;

            Texture = new Texture2D(manager.GraphicsDevice, Width, Height, false, SurfaceFormat);
        }

        public TextureManager Manager { get; }

        public int Width { get; }

        public int Height { get; }

        public bool Full { get; protected set; }

        public SurfaceFormat SurfaceFormat { get; }

        public bool AddTextureToAtlas(string name, Texture2D texture, out TextureInfo info)
        {

        }


    }
}
