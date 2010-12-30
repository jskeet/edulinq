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

namespace Edulinq.TestSupport
{
    /// <summary>
    /// Enumerable which counts 0...4, but records how many times it has been
    /// iterated over, and how many iterators have been disposed.
    /// </summary>
    class CountingEnumerable : IEnumerable<int>
    {
        public int EnumeratorsCreated { get; private set; }
        public int EnumeratorsDisposed { get; private set; }

        public IEnumerator<int> GetEnumerator()
        {
            return new CountingEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private class CountingEnumerator : IEnumerator<int>
        {
            private readonly CountingEnumerable countingEnumerable;
            private int position = -1;
            private bool disposed = false;

            public CountingEnumerator(CountingEnumerable countingEnumerable)
            {
                this.countingEnumerable = countingEnumerable;
            }

            public int Current
            {
                get
                {
                    if (position < 0)
                    {
                        throw new InvalidOperationException();
                    }
                    return position;
                }
            }

            public void Dispose()
            {
                if (!disposed)
                {
                    position = -2;
                    countingEnumerable.EnumeratorsDisposed++;
                }
                disposed = true;
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }

            public bool MoveNext()
            {
                if (position >= -1 && position < 4)
                {
                    position++;
                    return false;
                }
                position = -2;
                return false;
            }

            public void Reset()
            {
                throw new NotImplementedException();
            }
        }
    }
}
