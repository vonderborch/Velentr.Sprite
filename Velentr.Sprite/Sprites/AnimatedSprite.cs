using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Velentr.Sprite.Animations;
using Velentr.Sprite.Helpers;

namespace Velentr.Sprite.Sprites
{
    /// <summary>
    ///     An animated sprite.
    /// </summary>
    ///
    /// <seealso cref="Velentr.Sprite.Sprites.Base.VelentrSprite"/>
    public class AnimatedSprite : Base.VelentrSprite
    {
        /// <summary>
        ///     The current animation.
        /// </summary>
        private string _currentAnimation;

        /// <summary>
        ///     The current frame start time.
        /// </summary>
        private TimeSpan _currentFrameStartTime = TimeSpan.Zero;

        /// <summary>
        ///     Constructor.
        /// </summary>
        ///
        /// <param name="manager">    The manager. </param>
        /// <param name="name">       The name. </param>
        /// <param name="size">       The size. </param>
        /// <param name="animations"> The animations. </param>
        public AnimatedSprite(TextureManager manager, string name, Vector2 size, params Animation[] animations) : base(manager, name, size, SpriteType.Animated)
        {
            _currentAnimation = animations[0].Name;
            CurrentAnimationName = _currentAnimation;
            Animations = animations.ToDictionary(k => k.Name, v => v);
            AnimationOrder = animations.Select(x => x.Name).ToList();
        }

        /// <summary>
        ///     Constructor.
        /// </summary>
        ///
        /// <param name="manager">    The manager. </param>
        /// <param name="name">       The name. </param>
        /// <param name="position">   The position. </param>
        /// <param name="size">       The size. </param>
        /// <param name="color">      (Optional) The color. </param>
        /// <param name="rotation">   (Optional) The rotation. </param>
        /// <param name="origin">     (Optional) The origin. </param>
        /// <param name="effects">    (Optional) The effects. </param>
        /// <param name="layerDepth"> (Optional) Depth of the layer. </param>
        /// <param name="animations"> The animations. </param>
        public AnimatedSprite(TextureManager manager, string name, Vector2 position, Vector2 size, Color? color = null, float rotation = 0, Vector2? origin = null, SpriteEffects effects = SpriteEffects.None, float layerDepth = 0, params Animation[] animations) : base(manager, name, position, size, SpriteType.Animated, color, rotation, origin, effects, layerDepth)
        {
            _currentAnimation = animations[0].Name;
            CurrentAnimationName = _currentAnimation;
            Animations = animations.ToDictionary(k => k.Name, v => v);
            AnimationOrder = animations.Select(x => x.Name).ToList();
        }

        /// <summary>
        ///     Gets the animation order.
        /// </summary>
        ///
        /// <value>
        ///     The animation order.
        /// </value>
        public List<string> AnimationOrder { get; }

        /// <summary>
        ///     Gets or sets the animations.
        /// </summary>
        ///
        /// <value>
        ///     The animations.
        /// </value>
        public Dictionary<string, Animation> Animations { get; set; }

        /// <summary>
        ///     Gets the current animation.
        /// </summary>
        ///
        /// <value>
        ///     The current animation.
        /// </value>
        public Animation CurrentAnimation => Animations[CurrentAnimationName];

        /// <summary>
        ///     Gets or sets the current animation name.
        /// </summary>
        ///
        /// <value>
        ///     The name of the current animation.
        /// </value>
        public string CurrentAnimationName { get; set; }

        /// <summary>
        ///     Gets the texture.
        /// </summary>
        ///
        /// <value>
        ///     The texture.
        /// </value>
        ///
        /// <seealso cref="Velentr.Sprite.Sprites.Base.VelentrSprite.Texture"/>
        internal override Textures.Texture Texture => Manager._textures[CurrentAnimation.CurrentFrame.Texture];

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
        /// <param name="gameTime"> The game time. </param>
        ///
        /// <seealso cref="Velentr.Sprite.Sprites.Base.VelentrSprite.Update(GameTime)"/>
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
    }
}