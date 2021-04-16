using System;
using System.Collections.Generic;
using System.Text;

namespace Velentr.Sprite.Helpers
{
    public static class CollectionHelpers
    {

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
    }
}
