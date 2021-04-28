namespace Velentr.Sprite.Animations
{
    /// <summary>
    ///     An animation.
    /// </summary>
    public class Animation
    {
        /// <summary>
        ///     Constructor.
        /// </summary>
        ///
        /// <param name="name">      The name. </param>
        /// <param name="isLooping"> True if is looping, false if not. </param>
        /// <param name="frames">    A variable-length parameters list containing frames. </param>
        public Animation(string name, bool isLooping, params Frame[] frames)
        {
            Name = name;
            IsLooping = isLooping;
            IsComplete = false;
            Frames = frames;
            CurrentFrameId = 0;
        }

        /// <summary>
        ///     Constructor.
        /// </summary>
        ///
        /// <param name="from"> Source for the. </param>
        public Animation(Animation from)
        {
            Name = from.Name;
            IsLooping = from.IsLooping;
            IsComplete = from.IsComplete;
            Frames = new Frame[from.Frames.Length];
            from.Frames.CopyTo(Frames, 0);
            CurrentFrameId = from.CurrentFrameId;
        }

        /// <summary>
        ///     Gets the current frame.
        /// </summary>
        ///
        /// <value>
        ///     The current frame.
        /// </value>
        public Frame CurrentFrame => Frames[CurrentFrameId];

        /// <summary>
        ///     Gets or sets the current frame identifier.
        /// </summary>
        ///
        /// <value>
        ///     The identifier of the current frame.
        /// </value>
        public int CurrentFrameId { get; set; }

        /// <summary>
        ///     Gets the frames.
        /// </summary>
        ///
        /// <value>
        ///     The frames.
        /// </value>
        public Frame[] Frames { get; }

        /// <summary>
        ///     Gets or sets a value indicating whether this object is complete.
        /// </summary>
        ///
        /// <value>
        ///     True if this object is complete, false if not.
        /// </value>
        public bool IsComplete { get; private set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this object is looping.
        /// </summary>
        ///
        /// <value>
        ///     True if this object is looping, false if not.
        /// </value>
        public bool IsLooping { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        ///
        /// <value>
        ///     The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        ///     Increment frame identifier.
        /// </summary>
        public void IncrementFrameId()
        {
            CurrentFrameId++;
            if (CurrentFrameId >= Frames.Length)
            {
                if (IsLooping)
                {
                    CurrentFrameId = 0;
                }
                else
                {
                    IsComplete = true;
                    CurrentFrameId = Frames.Length - 1;
                }
            }
        }

        /// <summary>
        ///     Resets this object.
        /// </summary>
        public void Reset()
        {
            CurrentFrameId = 0;
            IsComplete = false;
        }
    }
}