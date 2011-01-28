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
#define EMULATE_LINQ_TO_OBJECTS_DISCARDING_NULL_KEYS

using System;
using System.Collections.Generic;

namespace Edulinq
{
    public static partial class Enumerable
    {
        public static IEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(
            this IEnumerable<TOuter> outer,
            IEnumerable<TInner> inner,
            Func<TOuter, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TOuter, TInner, TResult> resultSelector)
        {
            return Join(outer, inner, outerKeySelector, innerKeySelector, resultSelector,
                       EqualityComparer<TKey>.Default);
        }

        public static IEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(
            this IEnumerable<TOuter> outer,
            IEnumerable<TInner> inner,
            Func<TOuter, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TOuter, TInner, TResult> resultSelector,
            IEqualityComparer<TKey> comparer)
        {
            if (outer == null)
            {
                throw new ArgumentNullException("outer");
            }
            if (inner == null)
            {
                throw new ArgumentNullException("inner");
            }
            if (outerKeySelector == null)
            {
                throw new ArgumentNullException("outerKeySelector");
            }
            if (innerKeySelector == null)
            {
                throw new ArgumentNullException("innerKeySelector");
            }
            if (resultSelector == null)
            {
                throw new ArgumentNullException("resultSelector");
            }
            return JoinImpl(outer, inner, outerKeySelector, innerKeySelector, resultSelector,
                            comparer ?? EqualityComparer<TKey>.Default);
        }

#if IMPLEMENT_JOIN_USING_SELECTMANY

#if USE_LAZY_TO_DEFER_EVALUATION
        private static IEnumerable<TResult> JoinImpl<TOuter, TInner, TKey, TResult>(
            IEnumerable<TOuter> outer,
            IEnumerable<TInner> inner,
            Func<TOuter, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TOuter, TInner, TResult> resultSelector,
            IEqualityComparer<TKey> comparer)
        {
#if EMULATE_LINQ_TO_OBJECTS_DISCARDING_NULL_KEYS
            var lookup = new Lazy<ILookup<TKey, TInner>>(inner.ToLookupNoNullKeys(innerKeySelector, comparer));
#else
            var lookup = new Lazy<ILookup<TKey, TInner>>(inner.ToLookup(innerKeySelector, comparer));
#endif
            return outer.SelectMany(outerElement => lookup.Value[outerKeySelector(outerElement)],
                                    resultSelector);
        }
#else
        private static IEnumerable<TResult> JoinImpl<TOuter, TInner, TKey, TResult>(
            IEnumerable<TOuter> outer,
            IEnumerable<TInner> inner,
            Func<TOuter, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TOuter, TInner, TResult> resultSelector,
            IEqualityComparer<TKey> comparer)
        {
#if EMULATE_LINQ_TO_OBJECTS_DISCARDING_NULL_KEYS
            var lookup = inner.ToLookupNoNullKeys(innerKeySelector, comparer);
#else
            var lookup = inner.ToLookup(innerKeySelector, comparer);
#endif

            // If only we had "yield foreach"... we can't just return outer.SelectMany(...), as we'd
            // get immediate execution for the lookup.
            var results = outer.SelectMany(outerElement => lookup[outerKeySelector(outerElement)],
                                           resultSelector);
            foreach (var result in results)
            {
                yield return result;
            }
        }
#endif
#else
        private static IEnumerable<TResult> JoinImpl<TOuter, TInner, TKey, TResult>(
            IEnumerable<TOuter> outer,
            IEnumerable<TInner> inner,
            Func<TOuter, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TOuter, TInner, TResult> resultSelector,
            IEqualityComparer<TKey> comparer)
        {
#if EMULATE_LINQ_TO_OBJECTS_DISCARDING_NULL_KEYS
            var lookup = inner.ToLookupNoNullKeys(innerKeySelector, comparer);
#else
            var lookup = inner.ToLookup(innerKeySelector, comparer);
#endif
            foreach (var outerElement in outer)
            {
                var key = outerKeySelector(outerElement);
                foreach (var innerElement in lookup[key])
                {
                    yield return resultSelector(outerElement, innerElement);
                }
            }
        }
#endif
    }
}
