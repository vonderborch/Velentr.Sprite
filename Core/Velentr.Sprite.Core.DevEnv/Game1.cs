using System;
using System.Collections.Generic;
using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Velentr.Sprite;
using Velentr.Sprite.Textures;

namespace Velentr.Input.DevEnv
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Random r;
        private TextureManager manager;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            r = new Random();
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            manager = new TextureManager(GraphicsDevice);
            manager.AutoTextureAtlasBalancingEnabled = true;
            manager.AutoTextureAtlasBalancingIntervalMilliseconds = 1000;

            var textures = new List<string>()
            {
                "11-6.jpg",
                "beautiful-sunset-33.jpg",
                "chloe4.jpg",
                "gorgeous-f2-goldendoodle-puppies-5a7de534c591b.jpg",
                "OIP.jpg",
                "R3d398e62bb9b79f54b5c49b4e5f32925.jpg",
                "Rdb919eda1b98929d3a17685a6d3d7ef8.jpg",
                "Refd96265ac3da1f5b5dc5277f1082d15.jpg"
            };

            var texturesLoadInfos = new List<TextureLoadInfo>();
            for (var i = 0; i < textures.Count; i++)
            {
                texturesLoadInfos.Add(new TextureLoadInfo($"image{i}", $"Content\\{textures[i]}", ChooseRandomSize()));
            }

            manager.LoadTextures(texturesLoadInfos);

        }

        private Point? ChooseRandomSize()
        {
            switch (r.Next(0, 8))
            {
                case 0:
                    return new Point(128, 128);
                case 1:
                    return new Point(256, 256);
                case 2:
                    return new Point(512, 512);
                case 3:
                    return new Point(128, 256);
                case 4:
                    return new Point(128, 512);
                case 5:
                    return new Point(256, 128);
                case 6:
                    return new Point(512, 128);
                case 7:
                    return null;
            }

            return null;
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            manager.Dispose();
            base.OnExiting(sender, args);
        }

        protected override void Update(GameTime gameTime)
        {
            manager.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();


            manager.DrawFullAtlas(_spriteBatch, 0, GraphicsDevice.Viewport.Bounds, Color.White);
            _spriteBatch.Draw(manager, "image0", new Vector2(50, 50), Color.White);
            //_spriteBatch.Draw(manager, "image1", new Vector2(100, 100), Color.White);

            _spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
