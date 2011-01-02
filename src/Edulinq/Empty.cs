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
        public static IEnumerable<TResult> Empty<TResult>()
        {
            return EmptyEnumerable<TResult>.Instance;
        }

#if AVOID_RETURNING_ARRAYS
        private class EmptyEnumerable<T> : IEnumerable<T>, IEnumerator<T>
        {
            internal static IEnumerable<T> Instance = new EmptyEnumerable<T>();

            // Prevent construction elsewhere
            private EmptyEnumerable()
            {
            }

            public IEnumerator<T> GetEnumerator()
            {
                return this;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this;
            }

            public T Current
            {
                get { throw new InvalidOperationException(); }
            }

            object IEnumerator.Current
            {
                get { throw new InvalidOperationException(); }
            }

            public void Dispose()
            {
                // No-op
            }

            public bool MoveNext()
            {
                return false; // There's never a next entry
            }

            public void Reset()
            {
                // No-op
            }
        }

#else
        private static class EmptyEnumerable<T>
        {
            internal static readonly T[] Instance = new T[0];       
        }
#endif
    }
}
