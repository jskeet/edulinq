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
        public static IEnumerable<TSource> Reverse<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            return ReverseImpl(source);
        }

#if IMPLEMENT_REVERSE_WITH_STACK
        private static IEnumerable<TSource> ReverseImpl<TSource>(IEnumerable<TSource> source)
        {
            Stack<TSource> stack = new Stack<TSource>(source);
            foreach (TSource item in stack)
            {
                yield return item;
            }
        }
#elif IMPLEMENT_REVERSE_WITH_LINKED_LIST
        private static IEnumerable<TSource> ReverseImpl<TSource>(IEnumerable<TSource> source)
        {
            LinkedList<TSource> list = new LinkedList<TSource>(source);
            LinkedListNode<TSource> node = list.Last; // Property, not method!
            while (node != null)
            {
                yield return node.Value;
                node = node.Previous;
            }
        }
#else
        private static IEnumerable<TSource> ReverseImpl<TSource>(IEnumerable<TSource> source)
        {
            int count;
            TSource[] array = source.ToBuffer(out count);
            for (int i = count - 1; i >= 0; i--)
            {
                yield return array[i];
            }
        }
#endif
    }
}
