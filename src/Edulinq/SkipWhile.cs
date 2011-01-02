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
        public static IEnumerable<TSource> SkipWhile<TSource>(
            this IEnumerable<TSource> source,
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
            return SkipWhileImpl(source, predicate);
        }

        private static IEnumerable<TSource> SkipWhileImpl<TSource>(
            IEnumerable<TSource> source,
            Func<TSource, bool> predicate)
        {
            using (IEnumerator<TSource> iterator = source.GetEnumerator())
            {
                bool hasNext;
                while ((hasNext = iterator.MoveNext()) && predicate(iterator.Current))
                {
                    // Ignore element
                }
                if (!hasNext)
                {
                    yield break;
                }
                yield return iterator.Current;
                while (iterator.MoveNext())
                {
                    yield return iterator.Current;
                }
            }
        }

        public static IEnumerable<TSource> SkipWhile<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, int, bool> predicate)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }
            return SkipWhileImpl(source, predicate);
        }

        private static IEnumerable<TSource> SkipWhileImpl<TSource>(
            IEnumerable<TSource> source,
            Func<TSource, int, bool> predicate)
        {
            using (IEnumerator<TSource> iterator = source.GetEnumerator())
            {
                int index = 0;
                bool hasNext;
                while ((hasNext = iterator.MoveNext()) && predicate(iterator.Current, index))
                {
                    // Ignore element
                    index++;
                }
                if (!hasNext)
                {
                    yield break;
                }
                yield return iterator.Current;
                while (iterator.MoveNext())
                {
                    yield return iterator.Current;
                }
            }
        }
    }
}
