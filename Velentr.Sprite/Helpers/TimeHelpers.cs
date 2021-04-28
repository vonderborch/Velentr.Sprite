using System;

namespace Velentr.Sprite.Helpers
{
    /// <summary>
    ///     Various time helpers.
    /// </summary>
    public static class TimeHelpers
    {
        /// <summary>
        ///     Elapsed milli seconds.
        /// </summary>
        ///
        /// <param name="startTime"> The start time. </param>
        /// <param name="endTime">   The end time. </param>
        ///
        /// <returns>
        ///     An uint.
        /// </returns>
        public static uint ElapsedMilliSeconds(TimeSpan startTime, TimeSpan endTime)
        {
            return Convert.ToUInt32(Math.Abs((endTime - startTime).TotalMilliseconds));
        }
    }
}