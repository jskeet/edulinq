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
using System;

namespace Edulinq
{
    /// <summary>
    /// Lightweight wrapper around a list. This is mutable within the assembly, but immutable externally;
    /// we use it to build up a lookup, which doesn't return any groupings until the whole lookup has
    /// been constructed.
    /// </summary>
    internal sealed class Grouping<TKey, TElement> : IGrouping<TKey, TElement>, IList<TElement>
    {
        private readonly TKey key;
        private readonly List<TElement> list;

        internal Grouping(TKey key)
        {
            this.key = key;
            this.list = new List<TElement>();
        }

        public TKey Key { get { return key; } }

        public IEnumerator<TElement> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Internal-only method to add items; do not call after a reference to this grouping
        /// has been returned publicly.
        /// </summary>
        /// <param name="item"></param>
        internal void Add(TElement item)
        {
            list.Add(item);
        }

        public int IndexOf(TElement item)
        {
            return list.IndexOf(item);
        }

        void IList<TElement>.Insert(int index, TElement item)
        {
            throw new NotSupportedException();
        }

        void IList<TElement>.RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        public TElement this[int index]
        {
            get { return list[index]; }
            set { throw new NotSupportedException(); }
        }

        void ICollection<TElement>.Add(TElement item)
        {
            throw new NotSupportedException();
        }

        void ICollection<TElement>.Clear()
        {
            throw new NotSupportedException();
        }

        public bool Contains(TElement item)
        {
            return list.Contains(item);
        }

        public void CopyTo(TElement[] array, int arrayIndex)
        {
            list.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return list.Count; }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        bool ICollection<TElement>.Remove(TElement item)
        {
            throw new NotSupportedException();
        }
    }
}
