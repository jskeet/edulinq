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
#define SEPARATE_SKIP_IMPLEMENTATION
#define OPTIMIZE_SKIP_FOR_NO_OP
using System;
using System.Collections.Generic;

namespace Edulinq
{
    public static partial class Enumerable
    {
#if SEPARATE_SKIP_IMPLEMENTATION
        public static IEnumerable<TSource> Skip<TSource>(
            this IEnumerable<TSource> source,
            int count)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
#if OPTIMIZE_SKIP_FOR_NO_OP
            if (count <= 0)
            {
                return source;
            }
#endif
            return SkipImpl(source, count);
        }

        private static IEnumerable<TSource> SkipImpl<TSource>(
            this IEnumerable<TSource> source,
            int count)
        {
            using (IEnumerator<TSource> iterator = source.GetEnumerator())
            {
                for (int i = 0; i < count; i++)
                {
                    if (!iterator.MoveNext())
                    {
                        yield break;
                    }
                }
                while (iterator.MoveNext())
                {
                    yield return iterator.Current;
                }
            }
        }
#else
        public static IEnumerable<TSource> Skip<TSource>(
            this IEnumerable<TSource> source,
            int count)
        {
            return source.SkipWhile((x, index) => index < count);
        }
#endif
    }
}
