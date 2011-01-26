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
#if JON_IS_VERY_LAZY
        // Always creates an extra copy
        public static TSource[] ToArray<TSource>(this IEnumerable<TSource> source)
        {
            return source.ToList().ToArray();
        }
#elif JON_IS_MILDLY_LAZY
        // Creates an extra copy for non-collections
        public static TSource[] ToArray<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            ICollection<TSource> collection = source as ICollection<TSource>;
            if (collection != null)
            {
                TSource[] ret = new TSource[collection.Count];
                collection.CopyTo(ret, 0);
                return ret;
            }
            return new List<TSource>(source).ToArray();
        }
#else
        // Only creates an extra copy if has has to
        public static TSource[] ToArray<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            int count;
            TSource[] ret = source.ToBuffer(out count);
            // Now create another copy if we have to, in order to get an array of the
            // right size
            if (count != ret.Length)
            {
                Array.Resize(ref ret, count);
            }
            return ret;
        }
#endif
    }
}
