using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Edulinq
{
    internal class ProjectionComparer<TElement, TKey> : IComparer<TElement>
    {
        private readonly Func<TElement, TKey> keySelector;
        private IComparer<TKey> comparer;

        internal ProjectionComparer(Func<TElement, TKey> keySelector,
            IComparer<TKey> comparer)
        {
            this.keySelector = keySelector;
            this.comparer = comparer;
        }

        public int Compare(TElement x, TElement y)
        {
            TKey keyX = keySelector(x);
            TKey keyY = keySelector(y);
            return comparer.Compare(keyX, keyY);
        }
    }
}
