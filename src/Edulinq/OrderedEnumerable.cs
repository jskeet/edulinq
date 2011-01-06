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
                new ProjectionComparer<TElement, TKey>(keySelector, comparer);
            if (descending)
            {
                secondaryComparer = new ReverseComparer<TElement>(secondaryComparer);
            }
            return new OrderedEnumerable<TElement>(source,
                new CompoundComparer<TElement>(currentComparer, secondaryComparer));
        }

        public IEnumerator<TElement> GetEnumerator()
        {
            // First copy the elements into an array: don't bother with a list, as we
            // want to use arrays for all the swapping around.
            int count;
            TElement[] data = source.ToBuffer(out count);
            TElement[] tmp = new TElement[count];
            
            MergeSort(data, tmp, 0, count - 1);
            for (int i = 0; i < count; i++)
            {
                yield return data[i];
            }
        }

        // Note: right is *inclusive*
        private void MergeSort(TElement[] data, TElement[] tmp, int left, int right)
        {
            if (right > left)
            {
                // Optimize the case where we've only got two elements. Conditionally swap them.
                if (right == left + 1)
                {
                    TElement leftElement = data[left];
                    TElement rightElement = data[right];
                    if (currentComparer.Compare(leftElement, rightElement) > 0)
                    {
                        data[left] = rightElement;
                        data[right] = leftElement;
                    }
                }
                else
                {
                    int mid = (left + right)/2;
                    MergeSort(data, tmp, left, mid);
                    MergeSort(data, tmp, mid + 1, right);
                    Merge(data, tmp, left, mid + 1, right);
                }
            }
        }

        // Merge lists [left, mid) and [mid, right]
        private void Merge(TElement[] data, TElement[] tmp, int left, int mid, int right)
        {
            int leftCursor = left;
            int rightCursor = mid;
            int tmpCursor = left;
            TElement leftElement = data[leftCursor];
            TElement rightElement = data[rightCursor];
            // By never merging empty lists, we know we'll always have valid starting points
            while (true)
            {
                // When equal, use the left element to achieve stability
                if (currentComparer.Compare(leftElement, rightElement) <= 0)
                {
                    tmp[tmpCursor++] = leftElement;
                    leftCursor++;
                    if (leftCursor < mid)
                    {
                        leftElement = data[leftCursor];
                    }
                    else
                    {
                        // Only the right list is still active. Therefore tmpCursor must equal rightCursor,
                        // so there's no point in copying the right list to tmp and back again. Just copy
                        // the already-sorted bits back into data.
                        Array.Copy(tmp, left, data, left, tmpCursor - left);
                        return;
                    }
                }
                else
                {
                    tmp[tmpCursor++] = rightElement;
                    rightCursor++;
                    if (rightCursor <= right)
                    {
                        rightElement = data[rightCursor];
                    }
                    else
                    {
                        // Only the left list is still active. Therefore we can copy the remainder of
                        // the left list directly to the appropriate place in data, and then copy the
                        // appropriate portion of tmp back.
                        Array.Copy(data, leftCursor, data, tmpCursor, mid - leftCursor);
                        Array.Copy(tmp, left, data, left, tmpCursor - left);
                        return;
                    }
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
