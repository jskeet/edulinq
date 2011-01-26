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
using System.Linq;
using Edulinq.TestSupport;
using NUnit.Framework;

namespace Edulinq.Tests
{
    [TestFixture]
    public class ReverseTest
    {
        [Test]
        public void NullSource()
        {
            string[] source = null;
            Assert.Throws<ArgumentNullException>(() => source.Reverse());
        }

        [Test]
        public void ExecutionIsDeferred()
        {
            new ThrowingEnumerable().Reverse();
        }

        [Test]
        public void InputIsBuffered()
        {
            int[] values = { 10, 0, 20 };
            var query = values.Select(x => 10 / x).Reverse();
            Assert.Throws<DivideByZeroException>(() => {
                using (var iterator = query.GetEnumerator())
                {
                    iterator.MoveNext();
                }
            });
        }

        [Test]
        public void ArraysAreBuffered()
        {
            // A sneaky implementation may try to optimize for the case where the collection
            // implements IList or (even more "reliable") is an array: it mustn't do this,
            // as otherwise the results can be tainted by side-effects within iteration
            int[] source = { 0, 1, 2, 3 };

            var query = source.Reverse();
            source[1] = 99; // This change *will* be seen due to deferred execution
            using (var iterator = query.GetEnumerator())
            {
                iterator.MoveNext();
                Assert.AreEqual(3, iterator.Current);

                source[2] = 100; // This change *won't* be seen                
                iterator.MoveNext();
                Assert.AreEqual(2, iterator.Current);

                iterator.MoveNext();
                Assert.AreEqual(99, iterator.Current);

                iterator.MoveNext();
                Assert.AreEqual(0, iterator.Current);
            }
        }

        [Test]
        public void ReversedRange()
        {
            var query = Enumerable.Range(5, 5).Reverse();
            query.AssertSequenceEqual(9, 8, 7, 6, 5);
        }

        [Test]
        public void ReversedList()
        {
            // Note: mustn't give source a compile-time type of List<int> as
            // List<T> has a Reverse method.
            IEnumerable<int> source = new List<int> { 5, 6, 7, 8, 9 };
            var query = source.Reverse();
            query.AssertSequenceEqual(9, 8, 7, 6, 5);
        }

        [Test]
        public void EmptyInput()
        {
            int[] input = { };
            input.Reverse().AssertSequenceEqual();
        }
    }
}
