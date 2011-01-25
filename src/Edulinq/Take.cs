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
        public static IEnumerable<TSource> Take<TSource>(
            this IEnumerable<TSource> source,
            int count)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            return TakeImpl(source, count);
        }

        private static IEnumerable<TSource> TakeImpl<TSource>(
            this IEnumerable<TSource> source,
            int count)
        {
            using (IEnumerator<TSource> iterator = source.GetEnumerator())
            {
                for (int i = 0; i < count && iterator.MoveNext(); i++)
                {
                    yield return iterator.Current;
                }
            }
        }
    }
}
