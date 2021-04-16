using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Velentr.Sprite.Animations;
using Velentr.Sprite.Helpers;

namespace Velentr.Sprite.Sprites
{
    public class AnimatedSprite : Sprite
    {

        private string _currentAnimation = string.Empty;

        private TimeSpan _currentFrameStartTime = TimeSpan.Zero;

        public AnimatedSprite(TextureManager manager, string name, Vector2 size, params Animation[] animations) : base(manager, name, size)
        {
            _currentAnimation = animations[0].Name;
            CurrentAnimationName = _currentAnimation;
            Animations = animations.ToDictionary(k => k.Name, v => v);
            AnimationOrder = animations.Select(x => x.Name).ToList();
        }

        public AnimatedSprite(TextureManager manager, string name, Vector2 position, Color color, Vector2 size, float rotation, Vector2 origin, SpriteEffects effects, float layerDepth, params Animation[] animations) : base(manager, name, position, color, size, rotation, origin, effects, layerDepth)
        {
            _currentAnimation = animations[0].Name;
            CurrentAnimationName = _currentAnimation;
            Animations = animations.ToDictionary(k => k.Name, v => v);
            AnimationOrder = animations.Select(x => x.Name).ToList();
        }

        public List<string> AnimationOrder { get; }

        public Dictionary<string, Animation> Animations { get; set; }

        public string CurrentAnimationName { get; set; }

        public Animation CurrentAnimation => Animations[CurrentAnimationName];

        protected override Textures.Texture Texture => Manager._textures[CurrentAnimation.CurrentFrame.Texture];

        public override void Update(GameTime gameTime)
        {
            if (_currentFrameStartTime == TimeSpan.Zero)
            {
                _currentFrameStartTime = gameTime.TotalGameTime;
            }

            // if we've changed animations, set us to the new animation and reset the old one
            if (!_currentAnimation.Equals(CurrentAnimationName))
            {
                Animations[_currentAnimation].Reset();
                _currentFrameStartTime = gameTime.TotalGameTime;
                _currentAnimation = CurrentAnimationName;
            }

            // update the new animation frame as required
            if (TimeHelpers.ElapsedMilliSeconds(_currentFrameStartTime, gameTime.TotalGameTime) > CurrentAnimation.CurrentFrame.DurationMilliseconds)
            {
                CurrentAnimation.IncrementFrameId();
                _currentFrameStartTime = gameTime.TotalGameTime;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Rectangle destinationRectangle, Color color, float rotation, Vector2 origin, SpriteEffects effects, float layerDepth)
        {
            InternalDraw(spriteBatch, destinationRectangle, color, rotation, origin, effects, layerDepth);
        }

    }
}
