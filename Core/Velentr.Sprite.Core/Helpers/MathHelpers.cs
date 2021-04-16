﻿using System;
using Microsoft.Xna.Framework;

namespace Velentr.Sprite.Helpers
{
    /// <summary>   Various mathematics helpers. </summary>
    public static class MathHelpers
    {
        /// <summary>   Determines what the source rectangle should be relative to the parent area. </summary>
        ///
        /// <param name="source">       The source rectangle. </param>
        /// <param name="parentArea">   The parent area. </param>
        ///
        /// <returns>   The relative eectangle. </returns>
        public static Rectangle ScaleRectangle(Rectangle source, Rectangle parentArea)
        {
            var scale = new Vector2(source.Width / (float)parentArea.Width, source.Height / (float)parentArea.Height);
            var scaledOrigin = new Vector2(source.X * scale.X, source.Y * scale.Y);

            return new Rectangle((int)(source.X * scale.X - scaledOrigin.X), (int)(source.Y * scale.Y - scaledOrigin.Y), (int)(source.Width * scale.X), (int)(source.Height * scale.Y));
        }


        public static Rectangle ScaleRectangle(Rectangle source, Vector2 scale)
        {
            var scaledOrigin = new Vector2(source.X * scale.X, source.Y * scale.Y);

            return new Rectangle((int)(source.X * scale.X - scaledOrigin.X), (int)(source.Y * scale.Y - scaledOrigin.Y), (int)(source.Width * scale.X), (int)(source.Height * scale.Y));
        }
        public static Vector2 GetRectangleScale(Rectangle source, Rectangle parentArea)
        {
            return new Vector2(source.Width / (float)parentArea.Width, source.Height / (float)parentArea.Height);
        }

        public static bool FloatsAreEqual(float value1, float value2, float maxDifference = 0.00001f)
        {
            return Math.Abs(value1 - value2) <= maxDifference;
        }
    }
}