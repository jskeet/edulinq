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

#define SEQUENCE_EQUALS_IMPLEMENTATION_2

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Edulinq
{
    public static partial class Enumerable
    {
        public static bool SequenceEqual<TSource>(
            this IEnumerable<TSource> first,
            IEnumerable<TSource> second)
        {
            return first.SequenceEqual(second, EqualityComparer<TSource>.Default);
        }

        public static bool SequenceEqual<TSource>(
            this IEnumerable<TSource> first,
            IEnumerable<TSource> second,
            IEqualityComparer<TSource> comparer)
        {
            if (first == null)
            {
                throw new ArgumentNullException("first");
            }
            if (second == null)
            {
                throw new ArgumentNullException("second");
            }

            int count1;
            int count2;
            if (TryFastCount(first, out count1) && TryFastCount(second, out count2))
            {
                if (count1 != count2)
                {
                    return false;
                }
            }

            comparer = comparer ?? EqualityComparer<TSource>.Default;
#if SEQUENCE_EQUALS_IMPLEMENTATION_1
            using (IEnumerator<TSource> iterator1 = first.GetEnumerator(),
                   iterator2 = second.GetEnumerator())
            {
                while (iterator1.MoveNext())
                {
                    // second is shorter than first
                    if (!iterator2.MoveNext())
                    {
                        return false;
                    }
                    if (!comparer.Equals(iterator1.Current, iterator2.Current))
                    {
                        return false;
                    }
                }
                // If we can get to the next element, first was shorter than second.
                // Otherwise, the sequences are equal.
                return !iterator2.MoveNext();
            }
#elif SEQUENCE_EQUALS_IMPLEMENTATION_2
            using (IEnumerator<TSource> iterator1 = first.GetEnumerator(),
                   iterator2 = second.GetEnumerator())
            {
                while (true)
                {
                    bool next1 = iterator1.MoveNext();
                    bool next2 = iterator2.MoveNext();
                    // Sequences aren't of same length. We don't
                    // care which way round
                    if (next1 != next2)
                    {
                        return false;
                    }
                    // Both sequences have finished - done
                    if (!next1)
                    {
                        return true;
                    }
                    if (!comparer.Equals(iterator1.Current, iterator2.Current))
                    {
                        return false;
                    }
                }
            }
#elif SEQUENCE_EQUALS_IMPLEMENTATION_3
            using (IEnumerator<TSource> iterator2 = second.GetEnumerator())
            {
                foreach (TSource item1 in first)
                {
                    // second is shorter than first
                    if (!iterator2.MoveNext())
                    {
                        return false;
                    }
                    if (!comparer.Equals(item1, iterator2.Current))
                    {
                        return false;
                    }
                }
                // If we can get to the next element, first was shorter than second.
                // Otherwise, the sequences are equal.
                return !iterator2.MoveNext();
            }
#else
#error You must define which implementation of SequenceEquals to use!
#endif
        }
    }
}
