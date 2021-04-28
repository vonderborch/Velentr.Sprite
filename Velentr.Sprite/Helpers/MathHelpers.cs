using Microsoft.Xna.Framework;
using System;

namespace Velentr.Sprite.Helpers
{
    /// <summary>
    ///     Various mathematics helpers.
    /// </summary>
    public static class MathHelpers
    {
        /// <summary>
        ///     Floats are equal.
        /// </summary>
        ///
        /// <param name="value1">        The first value. </param>
        /// <param name="value2">        The second value. </param>
        /// <param name="maxDifference"> (Optional) The maximum difference. </param>
        ///
        /// <returns>
        ///     True if it succeeds, false if it fails.
        /// </returns>
        public static bool FloatsAreEqual(float value1, float value2, float maxDifference = 0.00001f)
        {
            return Math.Abs(value1 - value2) <= maxDifference;
        }

        /// <summary>
        ///     Gets rectangle scale.
        /// </summary>
        ///
        /// <param name="source">     The source rectangle. </param>
        /// <param name="parentArea"> The parent area. </param>
        ///
        /// <returns>
        ///     The rectangle scale.
        /// </returns>
        public static Vector2 GetRectangleScale(Rectangle source, Rectangle parentArea)
        {
            return new Vector2(source.Width / (float)parentArea.Width, source.Height / (float)parentArea.Height);
        }

        /// <summary>
        ///     Gets rounded rectangle from vectors.
        /// </summary>
        ///
        /// <param name="position"> The position. </param>
        /// <param name="size">     The size. </param>
        ///
        /// <returns>
        ///     The rounded rectangle from vectors.
        /// </returns>
        public static Rectangle GetRoundedRectangleFromVectors(Vector2 position, Vector2 size)
        {
            return new Rectangle(RoundFloatToInt(position.X), RoundFloatToInt(position.Y), RoundFloatToInt(size.X), RoundFloatToInt(size.Y));
        }

        /// <summary>
        ///     Round float to int.
        /// </summary>
        ///
        /// <param name="value"> The value. </param>
        ///
        /// <returns>
        ///     An int.
        /// </returns>
        public static int RoundFloatToInt(float value)
        {
            return (int)Math.Round(value);
        }

        /// <summary>
        ///     Determines what the source rectangle should be relative to the parent area.
        /// </summary>
        ///
        /// <param name="source">     The source rectangle. </param>
        /// <param name="parentArea"> The parent area. </param>
        ///
        /// <returns>
        ///     The relative rectangle.
        /// </returns>
        public static Rectangle ScaleRectangle(Rectangle source, Rectangle parentArea)
        {
            var scale = new Vector2(source.Width / (float)parentArea.Width, source.Height / (float)parentArea.Height);
            var scaledOrigin = new Vector2(source.X * scale.X, source.Y * scale.Y);

            return new Rectangle((int)(source.X * scale.X - scaledOrigin.X), (int)(source.Y * scale.Y - scaledOrigin.Y), (int)(source.Width * scale.X), (int)(source.Height * scale.Y));
        }

        /// <summary>
        ///     Determines what the source rectangle should be relative to the parent area.
        /// </summary>
        ///
        /// <param name="source"> The source rectangle. </param>
        /// <param name="scale">  The scale. </param>
        ///
        /// <returns>
        ///     The relative rectangle.
        /// </returns>
        public static Rectangle ScaleRectangle(Rectangle source, Vector2 scale)
        {
            var scaledOrigin = new Vector2(source.X * scale.X, source.Y * scale.Y);

            return new Rectangle((int)(source.X * scale.X - scaledOrigin.X), (int)(source.Y * scale.Y - scaledOrigin.Y), (int)(source.Width * scale.X), (int)(source.Height * scale.Y));
        }
    }
}