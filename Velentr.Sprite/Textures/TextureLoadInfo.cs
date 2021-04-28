using Microsoft.Xna.Framework;

namespace Velentr.Sprite.Textures
{
    /// <summary>   Information to use when loading a texture. </summary>
    public class TextureLoadInfo
    {
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="name">             The name. </param>
        /// <param name="path">             Full pathname of the file. </param>
        /// <param name="textureSize">      (Optional) Size of the texture. </param>
        /// <param name="forceOverride">    (Optional) True to force override. </param>
        public TextureLoadInfo(string name, string path, Point? textureSize = null, bool forceOverride = false)
        {
            Name = name;
            Path = path;
            TextureSize = textureSize;
            ForceOverride = forceOverride;
        }

        /// <summary>   Gets a value indicating whether the override should be forced. </summary>
        ///
        /// <value> True if force override, false if not. </value>
        public bool ForceOverride { get; }

        /// <summary>   Gets the name. </summary>
        ///
        /// <value> The name. </value>
        public string Name { get; }

        /// <summary>   Gets the full pathname of the file. </summary>
        ///
        /// <value> The full pathname of the file. </value>
        public string Path { get; }

        /// <summary>   Gets the texture size. </summary>
        ///
        /// <value> The size of the texture. </value>
        public Point? TextureSize { get; }
    }
}