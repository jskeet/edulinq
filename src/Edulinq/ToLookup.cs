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
using System.Collections.Generic;

namespace Edulinq
{
    public static partial class Enumerable
    {
        public static ILookup<TKey, TSource> ToLookup<TSource, TKey>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector)
        {
            return source.ToLookup(keySelector, element => element, EqualityComparer<TKey>.Default);
        }

        public static ILookup<TKey, TSource> ToLookup<TSource, TKey>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            IEqualityComparer<TKey> comparer)
        {
            return source.ToLookup(keySelector, element => element, comparer);
        }

        public static ILookup<TKey, TElement> ToLookup<TSource, TKey, TElement>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            Func<TSource, TElement> elementSelector)
        {
            return source.ToLookup(keySelector, elementSelector, EqualityComparer<TKey>.Default);
        }

        public static ILookup<TKey, TElement> ToLookup<TSource, TKey, TElement>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            Func<TSource, TElement> elementSelector,
            IEqualityComparer<TKey> comparer)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (keySelector == null)
            {
                throw new ArgumentNullException("keySelector");
            }
            if (elementSelector == null)
            {
                throw new ArgumentNullException("elementSelector");
            }

            Lookup<TKey, TElement> lookup = new Lookup<TKey, TElement>(comparer ?? EqualityComparer<TKey>.Default);

            foreach (TSource item in source)
            {
                TKey key = keySelector(item);
                TElement element = elementSelector(item);
                lookup.Add(key, element);
            }
            return lookup;
        }

        private static ILookup<TKey, TSource> ToLookupNoNullKeys<TSource, TKey>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            IEqualityComparer<TKey> comparer)
        {
            Lookup<TKey, TSource> lookup = new Lookup<TKey, TSource>(comparer ?? EqualityComparer<TKey>.Default);

            foreach (TSource item in source)
            {
                TKey key = keySelector(item);
                if (key != null)
                {
                    lookup.Add(key, item);
                }
            }
            return lookup;
        }
    }
}
