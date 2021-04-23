using System.Collections.Generic;
using Velentr.Sprite.Textures;

namespace Velentr.Sprite.Helpers
{
    /// <summary>
    ///     A collection helpers.
    /// </summary>
    public static class CollectionHelpers
    {
        /// <summary>
        ///     Checks if two lists are equivalent
        /// </summary>
        ///
        /// <typeparam name="T"> Generic type parameter. </typeparam>
        /// <param name="a"> A List&lt;T&gt; to process. </param>
        /// <param name="b"> A List&lt;T&gt; to process. </param>
        ///
        /// <returns>
        ///     True if it succeeds, false if it fails.
        /// </returns>
        public static bool ListsEquivalent<T>(List<T> a, List<T> b)
        {
            if (a.Count != b.Count)
            {
                return false;
            }

            for (var i = 0; i < a.Count && i < b.Count; i++)
            {
                if (!a[i].Equals(b[i]))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        ///     Quick sort textures.
        /// </summary>
        ///
        /// <param name="textures"> The textures. </param>
        /// <param name="low">      (Optional) The low. </param>
        /// <param name="high">     (Optional) The high. </param>
        public static void QuickSortTextures(List<Texture> textures, int? low = null, int? high = null)
        {
            Sort(textures, low ?? 0, high ?? textures.Count - 1);
        }

        /// <summary>
        ///     Partitions.
        /// </summary>
        ///
        /// <param name="textures"> The textures. </param>
        /// <param name="low">      The low. </param>
        /// <param name="high">     The high. </param>
        ///
        /// <returns>
        ///     An int.
        /// </returns>
        private static int Partition(List<Texture> textures, int low, int high)
        {
            var pivot = textures[high];
            var index = low - 1;
            Texture temp;

            for (var i = low; i < high; i++)
            {
                if (textures[i].CompareTo(pivot) < 1)
                {
                    index++;
                    temp = textures[index];
                    textures[index] = textures[i];
                    textures[i] = temp;
                }
            }

            temp = textures[index + 1];
            textures[index + 1] = textures[high];
            textures[high] = temp;

            return index + 1;
        }

        /// <summary>
        ///     Sorts.
        /// </summary>
        ///
        /// <param name="textures"> The textures. </param>
        /// <param name="low">      The low. </param>
        /// <param name="high">     The high. </param>
        private static void Sort(List<Texture> textures, int low, int high)
        {
            if (low < high)
            {
                var index = Partition(textures, low, high);
                Sort(textures, low, index - 1);
                Sort(textures, index + 1, high);
            }
        }
    }
}