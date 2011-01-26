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
using System.Collections;
using System.Collections.Generic;

namespace Edulinq
{
    /// <summary>
    /// Implementation of ILookup which is immutable to the public but mutable internally;
    /// we have to make sure we never mutate it after it becomes visible to a caller.
    /// </summary>
    internal sealed class Lookup<TKey, TElement> : ILookup<TKey, TElement>
    {
        private readonly NullKeyFriendlyDictionary<TKey, Grouping<TKey, TElement>> map;
        private readonly List<TKey> keys;

        internal Lookup(IEqualityComparer<TKey> comparer)
        {
            map = new NullKeyFriendlyDictionary<TKey, Grouping<TKey, TElement>>(comparer);
            keys = new List<TKey>();
        }

        internal void Add(TKey key, TElement element)
        {
            Grouping<TKey, TElement> group;
            if (!map.TryGetValue(key, out group))
            {
                group = new Grouping<TKey, TElement>(key);
                map[key] = group;
                keys.Add(key);
            }
            group.Add(element);
        }

        public int Count
        {
            get { return map.Count; }
        }

        public IEnumerable<TElement> this[TKey key]
        {
            get
            {
                Grouping<TKey, TElement> group;
                if (!map.TryGetValue(key, out group))
                {
                    return Enumerable.Empty<TElement>();
                }
                return group;
            }
        }

        public bool Contains(TKey key)
        {
            return map.ContainsKey(key);
        }

        public IEnumerator<IGrouping<TKey, TElement>> GetEnumerator()
        {
            return keys.Select<TKey, IGrouping<TKey, TElement>>(key => map[key])
                       .GetEnumerator();
        }
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
