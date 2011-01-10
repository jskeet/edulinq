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
using NUnit.Framework;


namespace Edulinq.Tests
{
    [TestFixture]
    public class AverageTest
    {
        #region General Int32 tests
        [Test]
        public void NullSourceInt32NoSelector()
        {
            int[] source = null;
            Assert.Throws<ArgumentNullException>(() => source.Average());
        }

        [Test]
        public void NullSourceInt32WithSelector()
        {
            string[] source = null;
            Assert.Throws<ArgumentNullException>(() => source.Average(x => x.Length));
        }

        [Test]
        public void NullSourceInt32Selector()
        {
            string[] source = { "" };
            Func<string, int> selector = null;
            Assert.Throws<ArgumentNullException>(() => source.Average(selector));
        }

        [Test]
        public void NullSourceNullableInt32NoSelector()
        {
            int?[] source = null;
            Assert.Throws<ArgumentNullException>(() => source.Average());
        }

        [Test]
        public void NullSourceNullableInt32WithSelector()
        {
            string[] source = null;
            Assert.Throws<ArgumentNullException>(() => source.Average(x => (int?)x.Length));
        }

        [Test]
        public void NullSelectorNullableInt32()
        {
            string[] source = { "" };
            Func<string, int?> selector = null;
            Assert.Throws<ArgumentNullException>(() => source.Average(selector));
        }

        [Test]
        public void EmptySequenceInt32NoSelector()
        {
            int[] source = { };
            Assert.Throws<InvalidOperationException>(() => source.Average());
        }

        [Test]
        public void EmptySequenceInt32WithSelector()
        {
            string[] source = { };
            Assert.Throws<InvalidOperationException>(() => source.Average(x => x.Length));
        }

        [Test]
        public void EmptySequenceNullableInt32NoSelector()
        {
            int?[] source = { };
            Assert.IsNull(source.Average());
        }

        [Test]
        public void EmptySequenceNullableInt32WithSelector()
        {
            string[] source = { };
            Assert.IsNull(source.Average(x => (int?) x.Length));
        }

        [Test]
        public void AllNullsSequenceNullableInt32NoSelector()
        {
            int?[] source = { null, null, null };
            Assert.IsNull(source.Average());
        }

        [Test]
        public void AllNullsSequenceNullableInt32WithSelector()
        {
            string[] source = { "x", "y", "z" };
            Assert.IsNull(source.Average(x => (int?) null));
        }

        [Test]
        public void SimpleAverageInt32NoSelector()
        {
            // Note that 7.5 is exactly representable as a double, so we
            // shouldn't need to worry about floating-point inaccuracies
            int[] source = { 5, 10, 0, 15 };
            Assert.AreEqual(7.5d, source.Average());
        }

        [Test]
        public void SimpleAverageNullableInt32NoSelector()
        {
            int?[] source = { 5, 10, 0, 15 };
            Assert.AreEqual((double?) 7.5d, source.Average());
        }

        [Test]
        public void AverageIgnoresNullsNullableInt32NoSelector()
        {
            // The nulls here don't reduce the average
            int?[] source = { 5, null, 10, null, 0, null, 15 };
            Assert.AreEqual((double?) 7.5d, source.Average());
        }

        [Test]
        public void SimpleAverageInt32WithSelector()
        {
            string[] source = { "", "abcd", "a", "b" };
            Assert.AreEqual(1.5d, source.Average(x => x.Length));
        }

        [Test]
        public void SimpleAverageNullableInt32WithSelector()
        {
            string[] source = { "", "abcd", "a", "b" };
            Assert.AreEqual((double?) 1.5d, source.Average(x => (int?) x.Length));
        }

        [Test]
        public void AverageIgnoresNullsNullableInt32WithSelector()
        {
            // The nulls here don't reduce the average
            string[] source = { "", null, "abcd", null, "a", null, "b" };
            Assert.AreEqual((double?) 1.5d, source.Average(x => x == null ? null : (int?) x.Length));
        }
        #endregion

        [Test]
        public void SingleUsesDoubleAccumulator()
        {
            // All the values in the array are exactly representable as floats,
            // as is the correct average... but intermediate totals aren't.
            float[] array = { 20000000f, 1f, 1f, 2f };
            Assert.AreEqual(5000001f, array.Average());
        }

        #region Overflow tests
        [Test]
        public void Int32DoesNotOverflowAtInt32MaxValue()
        {
            int[] source = { int.MaxValue, int.MaxValue,
                             -int.MaxValue, -int.MaxValue};
            Assert.AreEqual(0, source.Average());
        }

        [Test]
        public void Int64OverflowsAtInt64MaxValue()
        {
            long[] source = { long.MaxValue, 1 };
            Assert.Throws<OverflowException>(() => source.Average());
        }

        [Test]
        public void SingleDoesNotOverflowAtSingleMaxValue()
        {
            float[] source = { float.MaxValue, float.MaxValue,
                               -float.MaxValue, -float.MaxValue };
            Assert.AreEqual(0f, source.Average());
        }

        [Test]
        public void DoubleOverflowsToInfinity()
        {
            double[] source = { double.MaxValue, double.MaxValue,
                               -double.MaxValue, -double.MaxValue };
            Assert.IsTrue(double.IsPositiveInfinity(source.Average()));
        }

        [Test]
        public void DoubleOverflowsToNegativeInfinity()
        {
            double[] source = { -double.MaxValue, -double.MaxValue,
                                double.MaxValue, double.MaxValue };
            Assert.IsTrue(double.IsNegativeInfinity(source.Average()));
        }

        [Test]
        public void DecimalOverflowsAtDecimalMaxValue()
        {
            decimal[] source = { decimal.MaxValue, 1m };
            Assert.Throws<OverflowException>(() => source.Average());
        }

        [Test]
        public void Int64KeepsPrecisionAtLargeValues()
        {
            // At long.MaxValue / 2, double precision doesn't get us
            // exact integers.
            long halfMax = long.MaxValue / 2;
            double halfMaxAsDouble = (double)halfMax;
            Assert.AreNotEqual(halfMax, (long)halfMaxAsDouble);

            long[] source = { halfMax, halfMax };
            Assert.AreEqual(halfMax, source.Average());
        }
        #endregion
    }
}
