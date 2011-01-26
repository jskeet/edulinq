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
    public class AggregateTest
    {
        [Test]
        public void NullSourceUnseeded()
        {
            int[] source = null;
            Assert.Throws<ArgumentNullException>(() => source.Aggregate((x, y) => x + y));
        }

        [Test]
        public void NullFuncUnseeded()
        {
            int[] source = { 1, 3 };
            Assert.Throws<ArgumentNullException>(() => source.Aggregate(null));
        }

        [Test]
        public void UnseededAggregation()
        {
            int[] source = { 1, 4, 5 };
            // First iteration: 0 * 2 + 1 = 1
            // Second iteration: 1 * 2 + 4 = 6
            // Third iteration: 6 * 2 + 5 = 17
            Assert.AreEqual(17, source.Aggregate((current, value) => current * 2 + value));
        }

        [Test]
        public void NullSourceSeeded()
        {
            int[] source = null;
            Assert.Throws<ArgumentNullException>(() => source.Aggregate(3, (x, y) => x + y));
        }

        [Test]
        public void NullFuncSeeded()
        {
            int[] source = { 1, 3 };
            Assert.Throws<ArgumentNullException>(() => source.Aggregate(5, null));
        }

        [Test]
        public void SeededAggregation()
        {
            int[] source = { 1, 4, 5 };
            int seed = 5;
            Func<int, int, int> func = (current, value) => current * 2 + value;
            // First iteration: 5 * 2 + 1 = 11
            // Second iteration: 11 * 2 + 4 = 26
            // Third iteration: 26 * 2 + 5 = 57
            Assert.AreEqual(57, source.Aggregate(seed, func));
        }

        [Test]
        public void NullSourceSeededWithResultSelector()
        {
            int[] source = null;
            Assert.Throws<ArgumentNullException>(() => source.Aggregate(3, (x, y) => x + y, result => result.ToInvariantString()));
        }

        [Test]
        public void NullFuncSeededWithResultSelector()
        {
            int[] source = { 1, 3 };
            Assert.Throws<ArgumentNullException>(() => source.Aggregate(5, null, result => result.ToInvariantString()));
        }

        [Test]
        public void NullProjectionSeededWithResultSelector()
        {
            int[] source = { 1, 3 };
            Func<int, string> resultSelector = null;
            Assert.Throws<ArgumentNullException>(() => source.Aggregate(5, (x, y) => x + y, resultSelector));
        }

        [Test]
        public void SeededAggregationWithResultSelector()
        {
            int[] source = { 1, 4, 5 };
            int seed = 5;
            Func<int, int, int> func = (current, value) => current * 2 + value;
            Func<int, string> resultSelector = result => result.ToInvariantString();
            // First iteration: 5 * 2 + 1 = 11
            // Second iteration: 11 * 2 + 4 = 26
            // Third iteration: 26 * 2 + 5 = 57
            // Result projection: 57.ToInvariantString() = "57"
            Assert.AreEqual("57", source.Aggregate(seed, func, resultSelector));
        }

        [Test]
        public void DifferentSourceAndAccumulatorTypes()
        {
            int largeValue = 2000000000;
            int[] source = { largeValue, largeValue, largeValue };
            long sum = source.Aggregate(0L, (acc, value) => acc + value);
            Assert.AreEqual(6000000000L, sum);
            // Just to prove we haven't missed off a zero...
            Assert.IsTrue(sum > int.MaxValue);
        }

        [Test]
        public void EmptySequenceUnseeded()
        {
            int[] source = { };
            Assert.Throws<InvalidOperationException>(() => source.Aggregate((x, y) => x + y));
        }

        [Test]
        public void EmptySequenceSeeded()
        {
            int[] source = { };
            Assert.AreEqual(5, source.Aggregate(5, (x, y) => x + y));
        }

        [Test]
        public void EmptySequenceSeededWithResultSelector()
        {
            int[] source = { };
            Assert.AreEqual("5", source.Aggregate(5, (x, y) => x + y, x => x.ToInvariantString()));
        }

        // Originally I'd thought it was the default value of TSource which was used as the seed...
        [Test]
        public void FirstElementOfInputIsUsedAsSeedForUnseededOverload()
        {
            int[] source = { 5, 3, 2 };
            Assert.AreEqual(30, source.Aggregate((acc, value) => acc * value));
        }
    }
}
