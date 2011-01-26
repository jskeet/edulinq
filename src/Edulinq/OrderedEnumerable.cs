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
    internal class OrderedEnumerable<TElement, TCompositeKey> : IOrderedEnumerable<TElement>
    {
        private readonly IEnumerable<TElement> source;
        private readonly Func<TElement, TCompositeKey> compositeSelector;
        private readonly IComparer<TCompositeKey> compositeComparer;

        internal OrderedEnumerable(IEnumerable<TElement> source,
            Func<TElement, TCompositeKey> compositeSelector,
            IComparer<TCompositeKey> compositeComparer)
        {
            this.source = source;
            this.compositeSelector = compositeSelector;
            this.compositeComparer = compositeComparer;
        }

        public IOrderedEnumerable<TElement> CreateOrderedEnumerable<TKey>(Func<TElement, TKey> keySelector, IComparer<TKey> comparer, bool descending)
        {
            if (keySelector == null)
            {
                throw new ArgumentNullException("keySelector");
            }
            comparer = comparer ?? Comparer<TKey>.Default;
            if (descending)
            {
                comparer = new ReverseComparer<TKey>(comparer);
            }

            // Copy to a local variable so we don't need to capture "this"
            Func<TElement, TCompositeKey> primarySelector = compositeSelector;
            Func<TElement, CompositeKey<TCompositeKey, TKey>> newKeySelector = 
                element => new CompositeKey<TCompositeKey, TKey>(primarySelector(element), keySelector(element));

            IComparer<CompositeKey<TCompositeKey, TKey>> newKeyComparer =
                new CompositeKey<TCompositeKey, TKey>.Comparer(compositeComparer, comparer);

            return new OrderedEnumerable<TElement, CompositeKey<TCompositeKey, TKey>>
                (source, newKeySelector, newKeyComparer);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<TElement> GetEnumerator()
        {
            // First copy the elements into an array: don't bother with a list, as we
            // want to use arrays for all the swapping around.
            int count;
            TElement[] data = source.ToBuffer(out count);

            int[] indexes = new int[count];
            for (int i = 0; i < indexes.Length; i++)
            {
                indexes[i] = i;
            }

            TCompositeKey[] keys = new TCompositeKey[count];
            for (int i = 0; i < keys.Length; i++)
            {
                keys[i] = compositeSelector(data[i]);
            }

            int nextYield = 0;

            var stack = new Stack<LeftRight>();
            stack.Push(new LeftRight(0, count - 1));
            while (stack.Count > 0)
            {
                LeftRight leftRight = stack.Pop();
                int left = leftRight.left;
                int right = leftRight.right;
                if (right > left)
                {
                    // Note: not just (left + right) / 2 in order to avoid a common bug: http://goo.gl/d4d4
                    int pivot = left + (right - left) / 2;
                    int pivotPosition = Partition(indexes, keys, left, right, pivot);
                    // Push the right sublist first, so that we *pop* the
                    // left sublist first
                    stack.Push(new LeftRight(pivotPosition + 1, right));
                    stack.Push(new LeftRight(left, pivotPosition - 1));
                }
                else
                {
                    while (nextYield <= right)
                    {
                        yield return data[indexes[nextYield]];
                        nextYield++;
                    }
                }
            }
        }

        private struct LeftRight
        {
            internal readonly int left, right;
            internal LeftRight(int left, int right)
            {
                this.left = left;
                this.right = right;
            }
        }

        private int Partition(int[] indexes, TCompositeKey[] keys, int left, int right, int pivot)
        {
            // Remember the current index (into the keys/elements arrays) of the pivot location
            int pivotIndex = indexes[pivot];
            TCompositeKey pivotKey = keys[pivotIndex];

            // Swap the pivot value to the end
            indexes[pivot] = indexes[right];
            indexes[right] = pivotIndex;
            int storeIndex = left;
            for (int i = left; i < right; i++)
            {
                int candidateIndex = indexes[i];
                TCompositeKey candidateKey = keys[candidateIndex];
                int comparison = compositeComparer.Compare(candidateKey, pivotKey);
                if (comparison < 0 || (comparison == 0 && candidateIndex < pivotIndex))
                {
                    // Swap storeIndex with the current location
                    indexes[i] = indexes[storeIndex];
                    indexes[storeIndex] = candidateIndex;
                    storeIndex++;
                }
            }
            // Move the pivot to its final place
            int tmp = indexes[storeIndex];
            indexes[storeIndex] = indexes[right];
            indexes[right] = tmp;
            return storeIndex;
        }
    }
}
