using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Velentr.Collections.Collections;
using Velentr.Sprite.Helpers;

namespace Velentr.Sprite.Sprites
{
    public class CompositeSprite : Sprite
    {

        private List<string> _lastSprites;

        private List<Vector2> _spriteScales;

        private List<Vector2> _spriteScaledSizes;

        private Vector2 _baseSize;

        public CompositeSprite(TextureManager manager, string name, Vector2 size, params Sprite[] sprites) : base(manager, name, size)
        {
            Sprites = sprites.ToList();
            _lastSprites = new List<string>();
            _baseSize = size;
        }

        public CompositeSprite(TextureManager manager, string name, Vector2 position, Color color, Vector2 size, float rotation, Vector2 origin, SpriteEffects effects, float layerDepth, params Sprite[] sprites) : base(manager, name, position, color, size, rotation, origin, effects, layerDepth)
        {
            Sprites = sprites.ToList();
            _lastSprites = new List<string>();
            _baseSize = size;
        }

        public List<Sprite> Sprites { get; set; }

        private Rectangle BaseRectangle => new Rectangle(DestinationRectangle.X, DestinationRectangle.Y, (int) _baseSize.X, (int) _baseSize.Y);

        private Vector2 CurrentScale => MathHelpers.GetRectangleScale(DestinationRectangle, BaseRectangle);

        private Bank<(Point, float, Vector2, SpriteEffects), List<Rectangle>> _cachedCalculations;

        public uint MaxCachedCalculations = 5;

        protected override Textures.Texture Texture => null;

        private void CalculateSpriteScales()
        {
            _spriteScales = new List<Vector2>(Sprites.Count);
            _spriteScaledSizes = new List<Vector2>(Sprites.Count);
            _lastSprites = new List<string>(Sprites.Count);
            var baseScale = CurrentScale;

            for (var i = 0; i < Sprites.Count; i++)
            {
                _lastSprites.Add(Sprites[i].Name);
                if (baseScale == Vector2.One)
                {
                    _spriteScales.Add(CurrentScale);
                    _spriteScaledSizes.Add(new Vector2(Sprites[i].Size.X, Sprites[i].Size.Y));
                }
                else
                {
                    _spriteScales.Add(CurrentScale);
                    _spriteScaledSizes.Add(new Vector2(CurrentScale.X * Sprites[i].Size.X, CurrentScale.Y * Sprites[i].Size.Y));
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            for (var i = 0; i < Sprites.Count; i++)
            {
                Sprites[i].Update(gameTime);
            }
        }

        public List<string> GetCurrentSpriteNames()
        {
            return Sprites.Select(x => x.Name).ToList();
        }

        public override void Draw(SpriteBatch spriteBatch, Rectangle destinationRectangle, Color color, float rotation, Vector2 origin, SpriteEffects effects, float layerDepth)
        {
            var key = (new Point(destinationRectangle.Width, destinationRectangle.Height), rotation, origin, effects);

            if (!CollectionHelpers.ListsEquivalent<string>(_lastSprites, GetCurrentSpriteNames()))
            {
                _cachedCalculations = new Bank<(Point, float, Vector2, SpriteEffects), List<Rectangle>>();
                CalculateSpriteScales();
            }

            if (!_cachedCalculations.Exists(key))
            {
                if (_cachedCalculations.Count >= MaxCachedCalculations)
                {
                    _cachedCalculations.RemoveItem(0);
                }

                var calculations = new List<Rectangle>(Sprites.Count);
                var overallScale = MathHelpers.GetRectangleScale(destinationRectangle, DestinationRectangle);
                var flipAdjustment = Vector2.Zero;
                var flippedVertically = effects.HasFlag(SpriteEffects.FlipVertically);
                var flippedHorizontally = effects.HasFlag(SpriteEffects.FlipHorizontally);

                // if we've flipped, handle adjusting our location as required
                if (flippedVertically || flippedHorizontally)
                {
                    if (flippedHorizontally)
                    {
                        origin.X *= -1;
                        flipAdjustment.X -= Size.X;
                    }

                    if (flippedVertically)
                    {
                        origin.Y *= -1;
                        flipAdjustment.Y -= Size.Y;
                    }
                }

                float cos = 0;
                float sin = 0;
                var xOrigin = flipAdjustment.X - origin.X;
                var yOrigin = flipAdjustment.Y - origin.Y;
                bool noRotation;
                if (MathHelpers.FloatsAreEqual(rotation, 0) || MathHelpers.FloatsAreEqual(rotation / MathHelper.TwoPi, 1))
                {
                    noRotation = true;
                }
                else
                {
                    noRotation = false;
                    cos = (float)Math.Cos(rotation);
                    sin = (float)Math.Sin(rotation);
                }

                for (var i = 0; i < Sprites.Count; i++)
                {
                    // Handle our rotation as required
                    var transformation = Matrix.Identity;
                    var xScale = (flippedHorizontally ? -_spriteScales[i].X : _spriteScales[i].X) * overallScale.X;
                    var yScale = (flippedVertically ? -_spriteScales[i].Y : _spriteScales[i].Y) * overallScale.Y;
                    if (noRotation)
                    {
                        transformation.M11 = xScale;
                        transformation.M22 = yScale;
                        transformation.M41 = xOrigin * transformation.M11 + Position.X;
                        transformation.M42 = yOrigin * transformation.M22 + Position.Y;
                    }
                    else
                    {
                        transformation.M11 = xScale * cos;
                        transformation.M12 = xScale * sin;
                        transformation.M21 = yScale * -sin;
                        transformation.M22 = yScale * cos;
                        transformation.M41 = (xOrigin * transformation.M11 + yOrigin * transformation.M21) + Position.X;
                        transformation.M42 = (xOrigin * transformation.M12 + yOrigin * transformation.M22) + Position.Y;
                    }

                    var actualPosition = Sprites[i].Position;
                    Vector2.Transform(ref actualPosition, ref transformation, out actualPosition);
                    var actualDestinationRectangle = new Rectangle((int)actualPosition.X, (int)actualPosition.Y, (int)_spriteScaledSizes[i].X, (int)_spriteScaledSizes[i].Y);
                    calculations.Add(actualDestinationRectangle);

                    Sprites[i].Draw(spriteBatch, actualDestinationRectangle, color, rotation, Sprites[i].Origin, effects, layerDepth);
                }

                _cachedCalculations.AddItem(key, calculations);
            }
            else
            {
                var calculations = _cachedCalculations.GetItem(key);
                for (var i = 0; i < Sprites.Count; i++)
                {
                    Sprites[i].Draw(spriteBatch, calculations[i], color, rotation, Sprites[i].Origin, effects, layerDepth);
                }
            }
        }

    }
}
