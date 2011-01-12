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
    public class CastTest
    {
        [Test]
        public void NullSource()
        {
            IEnumerable source = null;
            Assert.Throws<ArgumentNullException>(() => source.Cast<string>());
        }

        [Test]
        public void ExecutionIsDeferred()
        {
            IEnumerable source = new ThrowingEnumerable();
            // No exception
            source.Cast<string>();
        }

        [Test]
        public void OriginalSourceReturnedForObviousNoOp()
        {
            IEnumerable strings = new List<string>();
            Assert.AreSame(strings, strings.Cast<string>());
        }

        [Test]
        public void OriginalSourceReturnedDueToGenericCovariance()
        {
            IEnumerable strings = new List<string>();
            Assert.AreSame(strings, strings.Cast<object>());
        }

        [Test]
        public void OriginalSourceReturnedForInt32ArrayToUInt32SequenceConversion()
        {
            IEnumerable enums = new int[10];
            Assert.AreSame(enums, enums.Cast<uint>());
        }

        [Test]
        public void OriginalSourceReturnedForEnumArrayToInt32SequenceConversion()
        {
            IEnumerable enums = new DayOfWeek[10];
            Assert.AreSame(enums, enums.Cast<int>());
        }

        [Test]
        public void SequenceWithAllValidValues()
        {
            IEnumerable strings = new object[] { "first", "second", "third" };
            strings.Cast<string>().AssertSequenceEqual("first", "second", "third");
        }

        [Test]
        public void NullsAreIncluded()
        {
            IEnumerable strings = new object[] { "first", null, "third" };
            strings.Cast<string>().AssertSequenceEqual("first", null, "third");
        }

        [Test]
        public void UnboxToInt32()
        {
            IEnumerable ints = new object[] { 10, 30, 50 };
            ints.Cast<int>().AssertSequenceEqual(10, 30, 50);
        }

        [Test]
        public void UnboxToNullableInt32WithNulls()
        {
            IEnumerable ints = new object[] { 10, null, 30, null, 50 };
            ints.Cast<int?>().AssertSequenceEqual(10, null, 30, null, 50);
        }

        [Test]
        public void CastExceptionOnWrongElementType()
        {
            IEnumerable objects = new object[] { "first", new object(), "third" };
            using (IEnumerator<string> iterator = objects.Cast<string>().GetEnumerator())
            {
                Assert.IsTrue(iterator.MoveNext());
                Assert.AreEqual("first", iterator.Current);
                Assert.Throws<InvalidCastException>(() => iterator.MoveNext());
            }
        }

        [Test]
        public void CastExceptionWhenUnboxingWrongType()
        {
            IEnumerable objects = new object[] { 100L, 100 };
            using (IEnumerator<long> iterator = objects.Cast<long>().GetEnumerator())
            {
                Assert.IsTrue(iterator.MoveNext());
                Assert.AreEqual(100L, iterator.Current);
                Assert.Throws<InvalidCastException>(() => iterator.MoveNext());
            }
        }
    }
}
