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
using System.Linq;
using Edulinq.TestSupport;
using NUnit.Framework;

namespace Edulinq.Tests
{
    [TestFixture]
    public class OfTypeTest
    {
        [Test]
        public void NullSource()
        {
            IEnumerable source = null;
            Assert.Throws<ArgumentNullException>(() => source.OfType<string>());
        }

        [Test]
        public void ExecutionIsDeferred()
        {
            IEnumerable source = new ThrowingEnumerable();
            // No exception
            source.OfType<string>();
        }

        [Test]
        public void OriginalSourceNotReturnedForReferenceTypes()
        {
            IEnumerable strings = new List<string>();
            Assert.AreNotSame(strings, strings.OfType<string>());
        }

        [Test]
        public void OriginalSourceNotReturnedForNullableValueTypes()
        {
            IEnumerable nullableInts = new List<int?>();
            Assert.AreNotSame(nullableInts, nullableInts.OfType<int?>());
        }

        [Test]
        [Ignore("Fails in LINQ to Objects - see blog for design discussion")]
        public void OriginalSourceReturnedForSequenceOfCorrectNonNullableValueType()
        {
            IEnumerable ints = new List<int>();
            Assert.AreSame(ints, ints.OfType<int>());
        }

        [Test]
        public void SequenceWithAllValidValues()
        {
            IEnumerable strings = new object[] { "first", "second", "third" };
            strings.OfType<string>().AssertSequenceEqual("first", "second", "third");
        }

        [Test]
        public void NullsAreExcluded()
        {
            IEnumerable strings = new object[] { "first", null, "third" };
            strings.OfType<string>().AssertSequenceEqual("first", "third");
        }

        [Test]
        public void UnboxToInt32()
        {
            IEnumerable ints = new object[] { 10, 30, 50 };
            ints.OfType<int>().AssertSequenceEqual(10, 30, 50);
        }

        [Test]
        public void UnboxToNullableInt32WithNulls()
        {
            IEnumerable ints = new object[] { 10, null, 30, null, 50 };
            ints.OfType<int?>().AssertSequenceEqual(10, 30, 50);
        }

        [Test]
        public void WrongElementTypesAreIgnored()
        {
            IEnumerable objects = new object[] { "first", new object(), "third" };
            objects.OfType<string>().AssertSequenceEqual("first", "third");
            using (IEnumerator<string> iterator = objects.Cast<string>().GetEnumerator())
            {
                Assert.IsTrue(iterator.MoveNext());
                Assert.AreEqual("first", iterator.Current);
                Assert.Throws<InvalidCastException>(() => iterator.MoveNext());
            }
        }

        [Test]
        public void UnboxingWithWrongElementTypes()
        {
            IEnumerable objects = new object[] { 100L, 100, 300L };
            objects.OfType<long>().AssertSequenceEqual(100L, 300L);
        }
    }
}
