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
using System.Linq;
using NUnit.Framework;

namespace Edulinq.Tests
{
    [TestFixture]
    public class UnionTest
    {
        [Test]
        public void NullFirstWithoutComparer()
        {
            string[] first = null;
            string[] second = { };
            Assert.Throws<ArgumentNullException>(() => first.Union(second));
        }
        
        [Test]
        public void NullSecondWithoutComparer()
        {
            string[] first = { };
            string[] second = null;
            Assert.Throws<ArgumentNullException>(() => first.Union(second));
        }

        [Test]
        public void NullFirstWithComparer()
        {
            string[] first = null;
            string[] second = { };
            Assert.Throws<ArgumentNullException>(() => first.Union(second, StringComparer.Ordinal));
        }

        [Test]
        public void NullSecondWithComparer()
        {
            string[] first = { };
            string[] second = null;
            Assert.Throws<ArgumentNullException>(() => first.Union(second, StringComparer.Ordinal));
        }

        [Test]
        public void UnionWithoutComparer()
        {
            string[] first = { "a", "b", "B", "c", "b" };
            string[] second = { "d", "e", "d", "a" };
            first.Union(second).AssertSequenceEqual("a", "b", "B", "c", "d", "e");
        }

        [Test]
        public void UnionWithNullComparer()
        {
            string[] first = { "a", "b", "B", "c", "b" };
            string[] second = { "d", "e", "d", "a" };
            first.Union(second, null).AssertSequenceEqual("a", "b", "B", "c", "d", "e");
        }

        [Test]
        public void UnionWithCaseInsensitiveComparer()
        {
            string[] first = { "a", "b", "B", "c", "b" };
            string[] second = { "d", "e", "d", "a" };
            first.Union(second, StringComparer.OrdinalIgnoreCase)
                 .AssertSequenceEqual("a", "b", "c", "d", "e");
        }

        [Test]
        public void UnionWithEmptyFirstSequence()
        {
            string[] first = {  };
            string[] second = { "d", "e", "d", "a" };
            first.Union(second).AssertSequenceEqual("d", "e", "a");
        }

        [Test]
        public void UnionWithEmptySecondSequence()
        {
            string[] first = { "a", "b", "B", "c", "b" };
            string[] second = { };
            first.Union(second).AssertSequenceEqual("a", "b", "B", "c");
        }

        [Test]
        public void UnionWithTwoEmptySequences()
        {
            string[] first = { };
            string[] second = { };
            first.Union(second).AssertSequenceEqual();
        }

        [Test]
        public void FirstSequenceIsNotUsedUntilQueryIsIterated()
        {
            var first = new ThrowingEnumerable();
            int[] second = { 2 };
            var query = first.Union(second);
            using (var iterator = query.GetEnumerator())
            {
                // Still no exception... until we call MoveNext
                Assert.Throws<InvalidOperationException>(() => iterator.MoveNext());
            }
        }

        [Test]
        public void SecondSequenceIsNotUsedUntilFirstIsExhausted()
        {
            int[] first = { 3, 5, 3 };
            var second = new ThrowingEnumerable();
            using (var iterator = first.Union(second).GetEnumerator())
            {
                // Check the first two elements...
                Assert.IsTrue(iterator.MoveNext());
                Assert.AreEqual(3, iterator.Current);
                Assert.IsTrue(iterator.MoveNext());
                Assert.AreEqual(5, iterator.Current);

                // But as soon as we move past the first sequence, bang!
                Assert.Throws<InvalidOperationException>(() => iterator.MoveNext());
            }
        }
    }
}
