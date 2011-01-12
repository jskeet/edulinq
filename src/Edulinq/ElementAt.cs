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
using System.Collections;

namespace Edulinq
{
    public static partial class Enumerable
    {
        public static TSource ElementAt<TSource>(
            this IEnumerable<TSource> source,
            int index)
        {
            TSource ret;
            if (!TryElementAt(source, index, out ret))
            {
                throw new ArgumentOutOfRangeException("index");
            }
            return ret;
        }

        private static bool TryElementAt<TSource>(
            IEnumerable<TSource> source,
            int index,
            out TSource element)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            element = default(TSource);
            if (index < 0)
            {
                return false;
            }
            ICollection<TSource> collection = source as ICollection<TSource>;
            if (collection != null)
            {
                int count = collection.Count;
                if (index >= count)
                {
                    return false;
                }
                // If it's a list, we know we're okay now - just return directly...
                IList<TSource> list = source as IList<TSource>;
                if (list != null)
                {
                    element = list[index];
                    return true;
                }
                // Okay, non-list collection: we'll have to iterate, but at least
                // we've caught any invalid index values early.
            }

            // For non-generic collections all we can do is an early bounds check.
            ICollection nonGenericCollection = source as ICollection;
            if (nonGenericCollection != null)
            {
                int count = nonGenericCollection.Count;
                if (index >= count)
                {
                    return false;
                }
            }
            // We don't need to fetch the current value each time - get to the right
            // place first.
            using (IEnumerator<TSource> iterator = source.GetEnumerator())
            {
                // Note use of -1 so that we start off my moving onto element 0.
                // Don't want to use i <= index in case index == int.MaxValue!
                for (int i = -1; i < index; i++)
                {
                    if (!iterator.MoveNext())
                    {
                        return false;
                    }
                }
                element = iterator.Current;
                return true;
            }
        }
    }
}
