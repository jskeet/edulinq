using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Edulinq
{
    internal class OrderedEnumerable<TElement> : IOrderedEnumerable<TElement>
    {
        private readonly IEnumerable<TElement> source;
        private readonly IComparer<TElement> comparer;

        internal OrderedEnumerable(IEnumerable<TElement> source,
            IComparer<TElement> comparer)
        {
            this.source = source;
            this.comparer = comparer;
        }

        public IOrderedEnumerable<TElement> CreateOrderedEnumerable<TKey>(Func<TElement, TKey> keySelector, IComparer<TKey> comparer, bool descending)
        {
            IComparer<TElement> secondaryComparer = new ProjectionComparer<TElement, TKey> (keySelector, comparer);
            if (descending)
            {
                secondaryComparer = new ReverseComparer<TElement>(secondaryComparer);
            }
            return new OrderedEnumerable<TElement>(source,
                new CompoundComparer<TElement>(this.comparer, secondaryComparer));
        }

        public IEnumerator<TElement> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
