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
using Edulinq.TestSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Edulinq.Tests
{
    [TestFixture]
    public class ConcatTest
    {
        [Test]
        public void SimpleConcatenation()
        {
            IEnumerable<string> first = new string[] { "a", "b" };
            IEnumerable<string> second = new string[] { "c", "d" };
            first.Concat(second).AssertSequenceEqual("a", "b", "c", "d");
        }

        [Test]
        public void NullFirstThrowsNullArgumentException()
        {
            IEnumerable<string> first = null;
            IEnumerable<string> second = new string[] { "hello" };
            Assert.Throws<ArgumentNullException>(() => first.Concat(second));
        }

        [Test]
        public void NullSecondThrowsNullArgumentException()
        {
            IEnumerable<string> first = new string[] { "hello" };
            IEnumerable<string> second = null;
            Assert.Throws<ArgumentNullException>(() => first.Concat(second));
        }

        [Test]
        public void FirstSequenceIsntAccessedBeforeFirstUse()
        {
            IEnumerable<int> first = new ThrowingEnumerable();
            IEnumerable<int> second = new int[] { 5 };
            // No exception yet...
            var query = first.Concat(second);
            // Still no exception...
            using (var iterator = query.GetEnumerator())
            {
                // Now it will go bang
                Assert.Throws<InvalidOperationException>(() => iterator.MoveNext());
            }
        }

        [Test]
        public void SecondSequenceIsntAccessedBeforeFirstUse()
        {
            IEnumerable<int> first = new int[] { 5 };
            IEnumerable<int> second = new ThrowingEnumerable();
            // No exception yet...
            var query = first.Concat(second);
            // Still no exception...
            using (var iterator = query.GetEnumerator())
            {
                // First element is fine...
                Assert.IsTrue(iterator.MoveNext());
                Assert.AreEqual(5, iterator.Current);
                // Now it will go bang, as we move into the second sequence
                Assert.Throws<InvalidOperationException>(() => iterator.MoveNext());
            }
        }
    }
}
