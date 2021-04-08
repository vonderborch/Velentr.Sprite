using System;
using System.Collections.Generic;
using System.Text;

namespace Velentr.Input
{
    public class TextureInfo
    {

        public TextureInfo(string name, TextureAtlas atlas)
        {
            Name = name;

            Atlas = atlas;
        }

        public string Name { get; }

        public TextureAtlas Atlas { get; }
    }
}
