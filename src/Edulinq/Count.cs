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
    public static partial class Enumerable
    {
        public static int Count<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            int count;
            if (TryFastCount(source, out count))
            {
                return count;
            }

            // Do it the slow way - and make sure we overflow appropriately
            checked
            {
                using (var iterator = source.GetEnumerator())
                {
                    while (iterator.MoveNext())
                    {
                        count++;
                    }
                }
                return count;
            }
        }

        public static int Count<TSource>(this IEnumerable<TSource> source,
            Func<TSource, bool> predicate)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }

            // No way of optimizing this. Do it the slow way, with overflow.
            checked
            {
                int count = 0;
                foreach (TSource item in source)
                {
                    if (predicate(item))
                    {
                        count++;
                    }
                }
                return count;
            }
        }

        private static bool TryFastCount<TSource>(
            IEnumerable<TSource> source,
            out int count)
        {
            // Optimization for ICollection<T>
            ICollection<TSource> genericCollection = source as ICollection<TSource>;
            if (genericCollection != null)
            {
                count = genericCollection.Count;
                return true;
            }

            // Optimization for ICollection
            ICollection nonGenericCollection = source as ICollection;
            if (nonGenericCollection != null)
            {
                count = nonGenericCollection.Count;
                return true;
            }

            // Can't retrieve the count quickly. Oh well.
            count = 0;
            return false;
        }
    }
}
