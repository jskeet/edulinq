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
    public class ExceptTest
    {
        [Test]
        public void NullFirstWithoutComparer()
        {
            string[] first = null;
            string[] second = { };
            Assert.Throws<ArgumentNullException>(() => first.Except(second));
        }

        [Test]
        public void NullSecondWithoutComparer()
        {
            string[] first = { };
            string[] second = null;
            Assert.Throws<ArgumentNullException>(() => first.Except(second));
        }

        [Test]
        public void NullFirstWithComparer()
        {
            string[] first = null;
            string[] second = { };
            Assert.Throws<ArgumentNullException>(() => first.Except(second, StringComparer.Ordinal));
        }

        [Test]
        public void NullSecondWithComparer()
        {
            string[] first = { };
            string[] second = null;
            Assert.Throws<ArgumentNullException>(() => first.Except(second, StringComparer.Ordinal));
        }

        [Test]
        public void NoComparerSpecified()
        {
            string[] first = { "A", "a", "b", "c", "b", "c" };
            string[] second = { "b", "a", "d", "a" };
            first.Except(second).AssertSequenceEqual("A", "c");
        }

        [Test]
        public void NullComparerSpecified()
        {
            string[] first = { "A", "a", "b", "c", "b", "c" };
            string[] second = { "b", "a", "d", "a" };
            first.Except(second, null).AssertSequenceEqual("A", "c");
        }

        [Test]
        public void CaseInsensitiveComparerSpecified()
        {
            string[] first = { "A", "a", "b", "c", "b" };
            string[] second = { "b", "a", "d", "a" };
            first.Except(second, StringComparer.OrdinalIgnoreCase).AssertSequenceEqual("c");
        }

        [Test]
        public void NoSequencesUsedBeforeIteration()
        {
            var first = new ThrowingEnumerable();
            var second = new ThrowingEnumerable();
            // No exceptions!
            var query = first.Union(second);
            // Still no exceptions... we're not calling MoveNext.
            using (var iterator = query.GetEnumerator())
            {
            }
        }

        [Test]
        public void SecondSequenceReadFullyOnFirstResultIteration()
        {
            int[] first = { 1 };
            var secondQuery = new[] { 10, 2, 0 }.Select(x => 10 / x);

            var query = first.Except(secondQuery);
            using (var iterator = query.GetEnumerator())
            {
                Assert.Throws<DivideByZeroException>(() => iterator.MoveNext());
            }
        }

        [Test]
        public void FirstSequenceOnlyReadAsResultsAreRead()
        {
            var firstQuery = new[] { 10, 2, 0, 2 }.Select(x => 10 / x);
            int[] second = { 1 };

            var query = firstQuery.Except(second);
            using (var iterator = query.GetEnumerator())
            {
                // We can get the first value with no problems
                Assert.IsTrue(iterator.MoveNext());
                Assert.AreEqual(5, iterator.Current);

                // Getting at the *second* value of the result sequence requires
                // reading from the first input sequence until the "bad" division
                Assert.Throws<DivideByZeroException>(() => iterator.MoveNext());
            }
        }
    }
}
