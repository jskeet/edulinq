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
#define IMPLEMENT_CONTAINS_COMPATIBILITY_MODE
using System;
using System.Collections.Generic;
namespace Edulinq
{
    public static partial class Enumerable
    {
// See blog post for why I've opted out of compatibility by default
#if IMPLEMENT_CONTAINS_COMPATIBILITY_MODE
        public static bool Contains<TSource>(
            this IEnumerable<TSource> source,
            TSource value)
        {
            ICollection<TSource> collection = source as ICollection<TSource>;
            if (collection != null)
            {
                return collection.Contains(value);
            }
            return Contains(source, value, EqualityComparer<TSource>.Default);
        }
#else
        public static bool Contains<TSource>(
            this IEnumerable<TSource> source,
            TSource value)
        {
            return Contains(source, value, EqualityComparer<TSource>.Default);
        }
#endif

#if IMPLEMENT_CONTAINS_WITH_ANY
        public static bool Contains<TSource>(
            this IEnumerable<TSource> source,
            TSource value,
            IEqualityComparer<TSource> comparer)
        {
            comparer = comparer ?? EqualityComparer<TSource>.Default;
            return source.Any(item => comparer.Equals(value, item));
        }
#else
        public static bool Contains<TSource>(
            this IEnumerable<TSource> source,
            TSource value,
            IEqualityComparer<TSource> comparer)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            comparer = comparer ?? EqualityComparer<TSource>.Default;

#if OPTIMIZE_CONTAINS_FOR_HASHSET
            HashSet<TSource> hashSet = source as HashSet<TSource>;
            if (hashSet != null && comparer.Equals(hashSet.Comparer))
            {
                return hashSet.Contains(value);
            }
#endif            
            foreach (TSource item in source)
            {
                if (comparer.Equals(value, item))
                {
                    return true;
                }
            }
            return false;
        }
#endif
    }
}
