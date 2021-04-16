using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Velentr.Font;
using Velentr.Sprite.Animations;
using Velentr.Sprite.Helpers;
using Velentr.Sprite.Sprites;
using Velentr.Sprite.Textures;

namespace Velentr.Sprite.Core.DevEnv
{
    public class Game1 : Game
    {

        public enum DevStates
        {
            TextureAtlasTesting1,
            TextureAtlasTesting2,
            TextureAtlasTesting3,
            AnimatedSpriteTesting,
            AnimatedMovingSpriteTesting,
            SimpleSpriteTesting,
            SimpleMovingSpriteTesting,
            CompositeSpriteTesting,
            CompositeMovingSpriteTesting,
            CompositeAnimatedSpriteTesting,
            CompositeMovingAnimatedSpriteTesting,
        }

        private KeyboardState lastState;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Random r;
        private TextureManager manager;

        private AnimatedSprite animatedSprite1;
        private AnimatedSprite animatedSprite2;

        private SimpleSprite sprite1;
        private SimpleSprite sprite2;

        private CompositeSprite compositeSprite1;
        private CompositeSprite compositeSprite2;

        private CompositeSprite compositeAnimatedSprite1;
        private CompositeSprite compositeAnimatedSprite2;

        private FontManager fontManager;
        private Font.Font font;
        private Font.Font font2;

        private TimeSpan lastMovingSpriteUpdate;

        private DevStates currentState;

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
            //// other stuffs
            lastState = Keyboard.GetState();
            currentState = DevStates.AnimatedSpriteTesting;
            lastMovingSpriteUpdate = TimeSpan.Zero;
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            fontManager = new FontManager(GraphicsDevice);
            font = fontManager.GetFont("Content\\PlayfairDisplayRegular-ywLOY.ttf", 32);
            font2 = fontManager.GetFont("Content\\PlayfairDisplayRegular-ywLOY.ttf", 16);



            //// create and configure our sprite manager
            manager = new TextureManager(GraphicsDevice);
            manager.AutoTextureAtlasBalancingIntervalMilliseconds = 5000;

            //// register all of the textures we'll be using
            var animationSize = new Point(400, 450);
            var hullSize = new Point(288, 192);
            var hullSizeVector = new Vector2(hullSize.X, hullSize.Y);
            var tireSize = new Point(45, 45);
            var tireSizeVector = new Vector2(tireSize.X, tireSize.Y);
            var textures = new List<(string, Point?, string?)>()
            {
                ("TextureAtlasTesting\\11-6.jpg", ChooseRandomSize(), null),
                ("TextureAtlasTesting\\beautiful-sunset-33.jpg", ChooseRandomSize(), null),
                ("TextureAtlasTesting\\chloe4.jpg", ChooseRandomSize(), null),
                ("TextureAtlasTesting\\gorgeous-f2-goldendoodle-puppies-5a7de534c591b.jpg", ChooseRandomSize(), null),
                ("TextureAtlasTesting\\OIP.jpg", ChooseRandomSize(), null),
                ("TextureAtlasTesting\\R3d398e62bb9b79f54b5c49b4e5f32925.jpg", ChooseRandomSize(), null),
                ("TextureAtlasTesting\\Rdb919eda1b98929d3a17685a6d3d7ef8.jpg", ChooseRandomSize(), null),
                ("TextureAtlasTesting\\Refd96265ac3da1f5b5dc5277f1082d15.jpg", ChooseRandomSize(), null),
                ("AnimationTesting\\woman-silhouette-walk-cycle-vector_1.jpg", animationSize, "woman1"),
                ("AnimationTesting\\woman-silhouette-walk-cycle-vector_2.jpg", animationSize, "woman2"),
                ("AnimationTesting\\woman-silhouette-walk-cycle-vector_3.jpg", animationSize, "woman3"),
                ("AnimationTesting\\woman-silhouette-walk-cycle-vector_4.jpg", animationSize, "woman4"),
                ("AnimationTesting\\woman-silhouette-walk-cycle-vector_5.jpg", animationSize, "woman5"),
                ("AnimationTesting\\woman-silhouette-walk-cycle-vector_6.jpg", animationSize, "woman6"),
                ("SpriteTesting\\spr_sportscar_0.png", hullSize, "static_car"),
                ("CompositeSpriteTesting\\spr_sportscar_0_hull.png", hullSize, "car_hull"),
                ("CompositeSpriteTesting\\spr_sportscar_0_tire.png", tireSize, "car_tire"),
                ("CompositeSpriteTesting\\spr_sportscar_0_tire_2.png", tireSize, "car_tire2"),
                ("CompositeSpriteTesting\\spr_sportscar_0_tire_3.png", tireSize, "car_tire3"),
                ("CompositeSpriteTesting\\spr_sportscar_0_tire_4.png", tireSize, "car_tire4")
            };
            var texturesLoadInfos = new List<TextureLoadInfo>();
            for (var i = 0; i < textures.Count; i++)
            {
                texturesLoadInfos.Add(new TextureLoadInfo(textures[i].Item3 ?? $"image{i}", $"Content\\{textures[i].Item1}", textures[i].Item2));
            }
            manager.LoadTextures(texturesLoadInfos);

            //// create sprites

            // Simple Sprites
            sprite1 = new SimpleSprite(manager, "static_car1", "static_car", hullSizeVector);
            sprite2 = new SimpleSprite(manager, "static_car2", "static_car", hullSizeVector);

            // Composite Sprites
            var tire1Location = new Vector2(43, 92);
            var tire2Location = new Vector2(213, 92);
            compositeSprite1 = new CompositeSprite(
                manager,
                "car_1",
                hullSizeVector,
                new SimpleSprite(manager, "hull", "car_hull", hullSizeVector),
                new SimpleSprite(manager, "tire1", "car_tire", tire1Location, Color.White, tireSizeVector, 0f, Vector2.Zero, SpriteEffects.None, 0f),
                new SimpleSprite(manager, "tire2", "car_tire", tire2Location, Color.White, tireSizeVector, 0f, Vector2.Zero, SpriteEffects.None, 0f)
            );
            compositeSprite2 = new CompositeSprite(
                manager,
                "car_2",
                hullSizeVector,
                new SimpleSprite(manager, "hull", "car_hull", hullSizeVector),
                new SimpleSprite(manager, "tire1", "car_tire", tire1Location, Color.White, tireSizeVector, 0f, Vector2.Zero, SpriteEffects.None, 0f),
                new SimpleSprite(manager, "tire2", "car_tire", tire2Location, Color.White, tireSizeVector, 0f, Vector2.Zero, SpriteEffects.None, 0f)
            );

            // Composite AnimatedSprites
            var tire_animation = new Animation(
                "tire",
                true,
                new Frame[]
                {
                    new Frame("t1", "car_tire", 100),
                    new Frame("t1", "car_tire2", 100),
                    new Frame("t1", "car_tire3", 100),
                    new Frame("t1", "car_tire4", 100)
                }
            );
            compositeAnimatedSprite1 = new CompositeSprite(
                manager,
                "a_car_1",
                hullSizeVector,
                new SimpleSprite(manager, "hull", "car_hull", hullSizeVector),
                new AnimatedSprite(
                    manager,
                    "tire1",
                    tire1Location,
                    Color.White,
                    tireSizeVector,
                    0f,
                    Vector2.Zero,
                    SpriteEffects.None,
                    0f,
                    new Animation(tire_animation)
                ),
                new AnimatedSprite(
                    manager,
                    "tire2",
                    tire2Location,
                    Color.White,
                    tireSizeVector,
                    0f,
                    Vector2.Zero,
                    SpriteEffects.None,
                    0f,
                    new Animation(tire_animation)
                )
            );
            compositeAnimatedSprite2 = new CompositeSprite(
                manager,
                "a_car_2",
                hullSizeVector,
                new SimpleSprite(manager, "hull", "car_hull", hullSizeVector),
                new AnimatedSprite(
                    manager,
                    "tire1",
                    tire1Location,
                    Color.White,
                    tireSizeVector,
                    0f,
                    Vector2.Zero,
                    SpriteEffects.None,
                    0f,
                    new Animation(tire_animation)
                ),
                new AnimatedSprite(
                    manager,
                    "tire2",
                    tire2Location,
                    Color.White,
                    tireSizeVector,
                    0f,
                    Vector2.Zero,
                    SpriteEffects.None,
                    0f,
                    new Animation(tire_animation)
                )
            );

            // Animated Sprites
            var frameTime = 75;
            animatedSprite1 = new AnimatedSprite(
                manager,
                "walking_women",
                Vector2.Zero,
                Color.White,
                new Vector2(animationSize.X, animationSize.Y),
                0f,
                Vector2.Zero,
                SpriteEffects.None,
                0f,
                new Animation(
                    "walking",
                    true,
                    new Frame[]
                    {
                        new Frame("f1", "woman1", frameTime),
                        new Frame("f2", "woman2", frameTime),
                        new Frame("f3", "woman3", frameTime),
                        new Frame("f4", "woman4", frameTime),
                        new Frame("f5", "woman5", frameTime),
                        new Frame("f6", "woman6", frameTime)
                    }
                )
            );
            animatedSprite2 = new AnimatedSprite(
                manager,
                "walking_women2",
                Vector2.Zero,
                Color.White,
                new Vector2(animationSize.X, animationSize.Y),
                0f,
                Vector2.Zero,
                SpriteEffects.None,
                0f,
                new Animation(
                    "walking",
                    true,
                    new Frame[]
                    {
                        new Frame("f1", "woman1", frameTime),
                        new Frame("f2", "woman2", frameTime),
                        new Frame("f3", "woman3", frameTime),
                        new Frame("f4", "woman4", frameTime),
                        new Frame("f5", "woman5", frameTime),
                        new Frame("f6", "woman6", frameTime)
                    }
                )
            );

            ChangeState(currentState, DevStates.SimpleSpriteTesting);
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

        private void ChangeState(DevStates newState, DevStates oldState)
        {
            switch (oldState)
            {
                case DevStates.AnimatedSpriteTesting:
                    manager.UnregisterSpriteFromUpdates("walking_women");
                    break;
                case DevStates.AnimatedMovingSpriteTesting:
                    manager.UnregisterSpriteFromUpdates("walking_women2");
                    break;
                case DevStates.CompositeAnimatedSpriteTesting:
                    manager.UnregisterSpriteFromUpdates("a_car_1");
                    break;
                case DevStates.CompositeMovingAnimatedSpriteTesting:
                    manager.UnregisterSpriteFromUpdates("a_car_2");
                    break;
            }

            switch (newState)
            {
                case DevStates.AnimatedSpriteTesting:
                    manager.RegisterSpriteForUpdates(animatedSprite1);
                    break;
                case DevStates.AnimatedMovingSpriteTesting:
                    manager.RegisterSpriteForUpdates(animatedSprite2);
                    animatedSprite2.Position = Vector2.Zero;
                    animatedSprite2.Rotation = 0f;
                    break;
                case DevStates.SimpleMovingSpriteTesting:
                    sprite2.Position = Vector2.Zero;
                    sprite2.Rotation = 0f;
                    break;
                case DevStates.CompositeMovingSpriteTesting:
                    compositeSprite2.Position = Vector2.Zero;
                    compositeSprite2.Rotation = 0f;
                    break;
                case DevStates.CompositeAnimatedSpriteTesting:
                    manager.RegisterSpriteForUpdates(compositeAnimatedSprite1);
                    break;
                case DevStates.CompositeMovingAnimatedSpriteTesting:
                    manager.RegisterSpriteForUpdates(compositeAnimatedSprite2);
                    compositeAnimatedSprite2.Position = Vector2.Zero;
                    compositeAnimatedSprite2.Rotation = 0f;
                    break;
            }
        }

        protected override void Update(GameTime gameTime)
        {
            manager.Update(gameTime);

            var current = Keyboard.GetState();

            if (current.IsKeyDown(Keys.Left) && lastState.IsKeyUp(Keys.Left))
            {
                var id = ((int) currentState) - 1;
                if (id < 0)
                {
                    id = Enum.GetNames(typeof(DevStates)).Length -1;
                }
                var last = currentState;
                currentState = (DevStates)id;
                lastMovingSpriteUpdate = gameTime.TotalGameTime;
                ChangeState(currentState, last);

            }
            else if (current.IsKeyDown(Keys.Right) && lastState.IsKeyUp(Keys.Right))
            {
                var id = ((int)currentState) + 1;
                if (id >= Enum.GetNames(typeof(DevStates)).Length)
                {
                    id = 0;
                }
                var last = currentState;
                currentState = (DevStates)id;
                lastMovingSpriteUpdate = gameTime.TotalGameTime;
                ChangeState(currentState, last);
            }
            else if (current.IsKeyDown(Keys.Up) && lastState.IsKeyUp(Keys.Up))
            {
                manager.AutoTextureAtlasBalancingEnabled = !manager.AutoTextureAtlasBalancingEnabled;
            }

            lastState = current;

            if (currentState == DevStates.AnimatedMovingSpriteTesting && TimeHelpers.ElapsedMilliSeconds(lastMovingSpriteUpdate, gameTime.TotalGameTime) > 1000)
            {
                var changes = GetRandomChanges();
                animatedSprite2.Position = changes.Item1;
                animatedSprite2.Rotation = changes.Item2;
                lastMovingSpriteUpdate = gameTime.TotalGameTime;
            }

            if (currentState == DevStates.SimpleMovingSpriteTesting && TimeHelpers.ElapsedMilliSeconds(lastMovingSpriteUpdate, gameTime.TotalGameTime) > 1000)
            {
                var changes = GetRandomChanges();
                sprite2.Position = changes.Item1;
                sprite2.Rotation = changes.Item2;
                lastMovingSpriteUpdate = gameTime.TotalGameTime;
            }

            if (currentState == DevStates.CompositeMovingSpriteTesting && TimeHelpers.ElapsedMilliSeconds(lastMovingSpriteUpdate, gameTime.TotalGameTime) > 1000)
            {
                var changes = GetRandomChanges();
                compositeSprite2.Position = changes.Item1;
                compositeSprite2.Rotation = changes.Item2;
                lastMovingSpriteUpdate = gameTime.TotalGameTime;
            }

            if (currentState == DevStates.CompositeMovingAnimatedSpriteTesting && TimeHelpers.ElapsedMilliSeconds(lastMovingSpriteUpdate, gameTime.TotalGameTime) > 1000)
            {
                var changes = GetRandomChanges();
                compositeAnimatedSprite2.Position = changes.Item1;
                compositeAnimatedSprite2.Rotation = changes.Item2;
                lastMovingSpriteUpdate = gameTime.TotalGameTime;
            }


            base.Update(gameTime);
        }

        private (Vector2, float) GetRandomChanges()
        {
            var coords = new Vector2(r.Next(0, Window.ClientBounds.Width - 150), r.Next(0, Window.ClientBounds.Height - 150));
            var rotation = (float)(r.NextDouble() * 2);

            return (coords, rotation);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            switch (currentState)
            {
                case DevStates.TextureAtlasTesting1:
                    manager.DrawFullAtlas(_spriteBatch, 0, GraphicsDevice.Viewport.Bounds, Color.White);
                    break;
                case DevStates.TextureAtlasTesting2:
                    _spriteBatch.Draw(manager, "image0", new Vector2(50, 50), Color.White);
                    _spriteBatch.Draw(manager, "image1", new Vector2(100, 100), Color.White);
                    break;
                case DevStates.TextureAtlasTesting3:
                    manager.DrawFullAtlas(_spriteBatch, 0, GraphicsDevice.Viewport.Bounds, Color.White);
                    _spriteBatch.Draw(manager, "image0", new Vector2(50, 50), Color.White);
                    _spriteBatch.Draw(manager, "image1", new Vector2(100, 100), Color.White);
                    break;
                case DevStates.AnimatedSpriteTesting:
                    animatedSprite1.Draw(_spriteBatch);
                    break;
                case DevStates.AnimatedMovingSpriteTesting:
                    animatedSprite2.Draw(_spriteBatch);
                    break;
                case DevStates.SimpleSpriteTesting:
                    sprite1.Draw(_spriteBatch);
                    break;
                case DevStates.SimpleMovingSpriteTesting:
                    sprite2.Draw(_spriteBatch);
                    break;
                case DevStates.CompositeSpriteTesting:
                    compositeSprite1.Draw(_spriteBatch);
                    break;
                case DevStates.CompositeMovingSpriteTesting:
                    compositeSprite2.Draw(_spriteBatch);
                    break;
                case DevStates.CompositeAnimatedSpriteTesting:
                    compositeAnimatedSprite1.Draw(_spriteBatch);
                    break;
                case DevStates.CompositeMovingAnimatedSpriteTesting:
                    compositeAnimatedSprite2.Draw(_spriteBatch);
                    break;
            }

            _spriteBatch.DrawString(font, currentState.ToString(), new Vector2(0, Window.ClientBounds.Height - 40), Color.Black);

            _spriteBatch.DrawString(font2, $"Atlas Re-balancing enabled: {manager.AutoTextureAtlasBalancingEnabled}", new Vector2(525, Window.ClientBounds.Height - 60), Color.Black);
            if (manager.AutoTextureAtlasBalancingEnabled)
            {
                _spriteBatch.DrawString(font2, $"Next Re-Balance: {Math.Round(manager.TimeInMillisecondsUntilLastReBalance / 1000f, 1)}s", new Vector2(525, Window.ClientBounds.Height - 40), Color.Black);
                _spriteBatch.DrawString(font2, $"Atlas Count: {manager.AtlasCount}", new Vector2(525, Window.ClientBounds.Height - 20), Color.Black);
            }

            _spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
