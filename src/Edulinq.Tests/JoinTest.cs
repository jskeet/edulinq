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
using System.Linq;
using Edulinq.TestSupport;
using NUnit.Framework;

namespace Edulinq.Tests
{
    [TestFixture]
    public class JoinTest
    {
        [Test]
        public void ExecutionIsDeferred()
        {
            var outer = new ThrowingEnumerable();
            var inner = new ThrowingEnumerable();
            // No exception
            outer.Join(inner, x => x, y => y, (x, y) => x + y);
        }

        [Test]
        public void OuterSequenceIsStreamed()
        {
            var outer = new[] { 10, 0, 2 }.Select(x => 10 / x);
            var inner = new[] { 1, 2, 3 };
            var query = outer.Join(inner, x => x, y => y, (x, y) => x + y);

            using (var iterator = query.GetEnumerator())
            {
                // First element is fine
                Assert.IsTrue(iterator.MoveNext());
                Assert.AreEqual(2, iterator.Current);

                // Attempting to get to the second element causes division by 0
                Assert.Throws<DivideByZeroException>(() => iterator.MoveNext());
            }
        }

        [Test]
        public void InnerSequenceIsBuffered()
        {
            var outer = new[] { 1, 2, 3 };
            var inner = new[] { 10, 0, 2 }.Select(x => 10 / x);
            var query = outer.Join(inner, x => x, y => y, (x, y) => x + y);

            using (var iterator = query.GetEnumerator())
            {
                // Even though we could sensibly see the first element before anything
                // is returned, that doesn't happen: the inner sequence is read completely
                // before we start reading the outer sequence
                Assert.Throws<DivideByZeroException>(() => iterator.MoveNext());
            }
        }

        [Test]
        public void SimpleJoin()
        {
            // We're going to join on the first character in the outer sequence item
            // being equal to the second character in the inner sequence item
            string[] outer = { "first", "second", "third" };
            string[] inner = { "essence", "offer", "eating", "psalm" };

            var query = outer.Join(inner,
                                   outerElement => outerElement[0],
                                   innerElement => innerElement[1],
                                   (outerElement, innerElement) => outerElement + ":" + innerElement);

            // Note: no matches for "third"
            query.AssertSequenceEqual("first:offer", "second:essence", "second:psalm");
        }

        [Test]
        public void CustomComparer()
        {
            // We're going to match the start of the outer sequence item
            // with the end of the inner sequence item, in a case-insensitive manner
            string[] outer = { "ABCxxx", "abcyyy", "defzzz", "ghizzz" };
            string[] inner = { "000abc", "111gHi", "222333" };

            var query = outer.Join(inner,
                                   outerElement => outerElement.Substring(0, 3),
                                   innerElement => innerElement.Substring(3),
                                   (outerElement, innerElement) => outerElement + ":" + innerElement,
                                   StringComparer.OrdinalIgnoreCase);
            query.AssertSequenceEqual("ABCxxx:000abc", "abcyyy:000abc", "ghizzz:111gHi");
        }

        [Test]
        public void DifferentSourceTypes()
        {
            int[] outer = { 5, 3, 7 };
            string[] inner = { "bee", "giraffe", "tiger", "badger", "ox", "cat", "dog" };

            var query = outer.Join(inner,
                                   outerElement => outerElement,
                                   innerElement => innerElement.Length,
                                   (outerElement, innerElement) => outerElement + ":" + innerElement);
            query.AssertSequenceEqual("5:tiger", "3:bee", "3:cat", "3:dog", "7:giraffe");
        }

        // Note that LINQ to Objects ignores null keys for Join and GroupJoin
        [Test]
        public void NullKeys()
        {
            string[] outer = { "first", "null", "nothing", "second" };
            string[] inner = { "nuff", "second" };
            var query = outer.Join(inner,
                                   outerElement => outerElement.StartsWith("n") ? null : outerElement,
                                   innerElement => innerElement.StartsWith("n") ? null : innerElement,
                                   (outerElement, innerElement) => outerElement + ":" + innerElement);

            query.AssertSequenceEqual("second:second");
        }
    }
}
