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
    /// <summary>
    /// Dictionary which additionally allows null keys. This doesn't implement
    /// IDictionary[TKey, TValue] as we only use it from Lookup, and don't need many
    /// of the features.
    /// </summary>
    internal sealed class NullKeyFriendlyDictionary<TKey, TValue>
    {
        private readonly Dictionary<TKey, TValue> map;
        private bool haveNullKey = false;
        private TValue valueForNullKey;

        internal NullKeyFriendlyDictionary(IEqualityComparer<TKey> comparer)
        {
            map = new Dictionary<TKey, TValue>(comparer);
        }

        internal bool TryGetValue(TKey key, out TValue value)
        {
            if (key == null)
            {
                // This will be default(TValue) if haveNullKey is false,
                // which is what we want.
                value = valueForNullKey;
                return haveNullKey;
            }
            return map.TryGetValue(key, out value);
        }

        internal TValue this[TKey key]
        {
            get
            {
                if (key == null)
                {
                    if (haveNullKey)
                    {
                        return valueForNullKey;
                    }
                    throw new KeyNotFoundException("No null key");
                }
                return map[key];
            }
            set
            {
                if (key == null)
                {
                    haveNullKey = true;
                    valueForNullKey = value;
                }
                else
                {
                    map[key] = value;
                }
            }
        }

        internal int Count
        {
            get { return map.Count + (haveNullKey ? 1 : 0); }
        }

        internal bool ContainsKey(TKey key)
        {
            return key == null ? haveNullKey : map.ContainsKey(key);
        }
    }
}
