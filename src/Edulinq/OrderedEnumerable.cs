#region Copyright and license information
// Copyright 2010-2011 Jon Skeet
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion
using System;
using System.Collections;
using System.Collections.Generic;

namespace Edulinq
{
    internal class OrderedEnumerable<TElement> : IOrderedEnumerable<TElement>
    {
        private readonly IEnumerable<TElement> source;
        private readonly IComparer<TElement> currentComparer;

        internal OrderedEnumerable(IEnumerable<TElement> source,
            IComparer<TElement> comparer)
        {
            this.source = source;
            this.currentComparer = comparer;
        }

        public IOrderedEnumerable<TElement> CreateOrderedEnumerable<TKey>(Func<TElement, TKey> keySelector, IComparer<TKey> comparer, bool descending)
        {
            if (keySelector == null)
            {
                throw new ArgumentNullException("keySelector");
            }
            IComparer<TElement> secondaryComparer =
                new ProjectionComparer<TElement, TKey> (keySelector, comparer);
            if (descending)
            {
                secondaryComparer = new ReverseComparer<TElement>(secondaryComparer);
            }
            return new OrderedEnumerable<TElement>(source,
                new CompoundComparer<TElement>(currentComparer, secondaryComparer));
        }

        public IEnumerator<TElement> GetEnumerator()
        {
            // This is a truly sucky way of implementing it. It's the simplest I could think of to start with.
            // We'll come back to it!
            List<TElement> elements = source.ToList();
            while (elements.Count > 0)
            {
                TElement minElement = elements[0];
                int minIndex = 0;
                for (int i = 1; i < elements.Count; i++)
                {
                    if (currentComparer.Compare(minElement, elements[i]) < 0)
                    {
                        minElement = elements[i];
                        minIndex = i;
                    }
                }
                elements.RemoveAt(minIndex);
                yield return minElement;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
