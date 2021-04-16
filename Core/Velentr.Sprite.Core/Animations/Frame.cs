using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Velentr.Sprite.Animations
{
    public struct Frame
    {

        public Frame(string name, string texture, int durationMilliseconds)
        {
            Name = name;
            Texture = texture;
            DurationMilliseconds = durationMilliseconds;
        }

        public string Name { get; set; }

        public string Texture { get; set; }

        public int DurationMilliseconds { get; set; }
    }
}
