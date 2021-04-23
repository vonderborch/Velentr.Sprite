namespace Velentr.Sprite.Animations
{
    /// <summary>
    ///     A frame of an animation.
    /// </summary>
    public struct Frame
    {
        /// <summary>
        ///     Constructor.
        /// </summary>
        ///
        /// <param name="name">                 The name. </param>
        /// <param name="texture">              The texture. </param>
        /// <param name="durationMilliseconds"> The duration milliseconds. </param>
        public Frame(string name, string texture, int durationMilliseconds)
        {
            Name = name;
            Texture = texture;
            DurationMilliseconds = durationMilliseconds;
        }

        /// <summary>
        ///     Gets or sets the duration milliseconds.
        /// </summary>
        ///
        /// <value>
        ///     The duration milliseconds.
        /// </value>
        public int DurationMilliseconds { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        ///
        /// <value>
        ///     The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the texture.
        /// </summary>
        ///
        /// <value>
        ///     The texture.
        /// </value>
        public string Texture { get; set; }
    }
}