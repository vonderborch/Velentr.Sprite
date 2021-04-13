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
        public static Rectangle RelativeRectangle(Rectangle source, Rectangle parentArea)
        {
            var scale = new Vector2(parentArea.Width / (float)source.Width, parentArea.Height / (float)source.Height);
            var scaledOrigin = new Vector2(source.X * scale.X, source.Y * scale.Y);

            return new Rectangle((int)(source.X * scale.X - scaledOrigin.X), (int)(source.Y * scale.Y - scaledOrigin.Y), (int)(source.Width * scale.X), (int)(source.Height * scale.Y));
        }
    }
}