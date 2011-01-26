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
using System.Collections;
using System.Collections.Generic;

namespace Edulinq.TestSupport
{
    /// <summary>
    /// Collection class backed by a List[T] which delegates everything to the list
    /// *except* iteration - GetEnumerator() throwsNotSupportedException.
    /// </summary>
    public class NonEnumerableList<T> : IList<T>
    {
        private readonly List<T> backingList;

        public NonEnumerableList(params T[] items) : this((IEnumerable<T>) items)
        {
        }

        public NonEnumerableList(IEnumerable<T> items)
        {
            backingList = new List<T>(items);
        }

        public int IndexOf(T item)
        {
            return backingList.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            backingList.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            backingList.RemoveAt(index);
        }

        public T this[int index]
        {
            get
            {
                return backingList[index];
            }
            set
            {
                backingList[index] = value;
            }
        }

        public void Add(T item)
        {
            backingList.Add(item);
        }

        public void Clear()
        {
            backingList.Clear();
        }

        public bool Contains(T item)
        {
            return backingList.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            backingList.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return backingList.Count; }
        }

        public bool IsReadOnly
        {
            get { return ((ICollection<T>)backingList).IsReadOnly; }
        }

        public bool Remove(T item)
        {
            return backingList.Remove(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotSupportedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
