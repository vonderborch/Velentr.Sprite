namespace Velentr.Sprite.Animations
{
    public class Animation
    {
        public Animation(string name, bool isLooping, params Frame[] frames)
        {
            Name = name;
            IsLooping = isLooping;
            IsComplete = false;
            Frames = frames;
            CurrentFrameId = 0;
        }

        public Animation(Animation from)
        {
            Name = from.Name;
            IsLooping = from.IsLooping;
            IsComplete = from.IsComplete;
            Frames = new Frame[from.Frames.Length];
            from.Frames.CopyTo(Frames, 0);
            CurrentFrameId = from.CurrentFrameId;
        }

        public string Name { get; set; }

        public bool IsLooping { get; set; }

        public bool IsComplete { get; private set; }

        public Frame[] Frames { get; }

        public int CurrentFrameId { get; set; }

        public Frame CurrentFrame => Frames[CurrentFrameId];

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

        public void Reset()
        {
            CurrentFrameId = 0;
            IsComplete = false;
        }

    }
}
