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
using NUnit.Framework;

namespace Edulinq.Tests
{
    [TestFixture]
    public class SumTest
    {
        #region Int32
        [Test]
        public void NullSourceInt32NoSelector()
        {
            int[] source = null;
            Assert.Throws<ArgumentNullException>(() => source.Sum());
        }

        [Test]
        public void NullSourceNullableInt32NoSelector()
        {
            int?[] source = null;
            Assert.Throws<ArgumentNullException>(() => source.Sum());
        }

        [Test]
        public void NullSourceInt32WithSelector()
        {
            string[] source = null;
            Assert.Throws<ArgumentNullException>(() => source.Sum(x => x.Length));
        }

        [Test]
        public void NullSourceNullableInt32WithSelector()
        {
            string[] source = null;
            Assert.Throws<ArgumentNullException>(() => source.Sum(x => (int?) x.Length));
        }

        [Test]
        public void NullSelectorInt32()
        {
            string[] source = { };
            Func<string, int> selector = null;
            Assert.Throws<ArgumentNullException>(() => source.Sum(selector));
        }

        [Test]
        public void NullSelectorNullableInt32()
        {
            string[] source = { };
            Func<string, int?> selector = null;
            Assert.Throws<ArgumentNullException>(() => source.Sum(selector));
        }

        [Test]
        public void EmptySequenceInt32()
        {
            int[] source = { };
            Assert.AreEqual(0, source.Sum());
        }

        [Test]
        public void EmptySequenceNullableInt32()
        {
            int?[] source = { };
            Assert.AreEqual(0, source.Sum());
        }

        [Test]
        public void SequenceOfNullsNullableInt32()
        {
            int?[] source = { null, null };
            Assert.AreEqual(0, source.Sum());
        }

        [Test]
        public void EmptySequenceInt32WithSelector()
        {
            string[] source = { };
            Assert.AreEqual(0, source.Sum(x => x.Length));
        }

        [Test]
        public void EmptySequenceNullableInt32WithSelector()
        {
            string[] source = { };
            Assert.AreEqual(0, source.Sum(x => (int?) x.Length));
        }

        [Test]
        public void SequenceOfNullsNullableInt32WithSelector()
        {
            string[] source = { "x", "y" };
            Assert.AreEqual(0, source.Sum(x => (int?)null));
        }

        [Test]
        public void SimpleSumInt32()
        {
            int[] source = { 1, 3, 2 };
            Assert.AreEqual(6, source.Sum());
        }

        [Test]
        public void SimpleSumNullableInt32()
        {
            int?[] source = { 1, 3, 2 };
            Assert.AreEqual(6, source.Sum());
        }

        [Test]
        public void SimpleSumNullableInt32IncludingNulls()
        {
            int?[] source = { 1, null, 3, null, 2 };
            Assert.AreEqual(6, source.Sum());
        }

        [Test]
        public void SimpleSumInt32WithSelector()
        {
            string[] source = { "x", "abc", "de" };
            Assert.AreEqual(6, source.Sum(x => x.Length));
        }

        [Test]
        public void SimpleSumNullableInt32WithSelector()
        {
            string[] source = { "x", "abc", "de" };
            Assert.AreEqual(6, source.Sum(x => (int?) x.Length));
        }

        [Test]
        public void SimpleSumNullableInt32WithSelectorIncludingNulls()
        {
            string[] source = { "x", "null", "abc", "null", "de" };
            Assert.AreEqual(6, source.Sum(x => x == "null" ? null : (int?) x.Length));
        }

        [Test]
        public void NegativeOverflowInt32()
        {
            // Only test this once per type - the other overflow tests should be enough
            // for different method calls
            int[] source = { int.MinValue, int.MinValue };
            Assert.Throws<OverflowException>(() => source.Sum());
        }

        [Test]
        public void OverflowInt32()
        {
            int[] source = { int.MaxValue, int.MaxValue };
            Assert.Throws<OverflowException>(() => source.Sum());
        }

        [Test]
        public void OverflowInt32WithSelector()
        {
            string[] source = { "x", "y" };
            Assert.Throws<OverflowException>(() => source.Sum(x => int.MaxValue));
        }

        [Test]
        public void OverflowNullableInt32()
        {
            int?[] source = { int.MaxValue, int.MaxValue };
            Assert.Throws<OverflowException>(() => source.Sum());
        }

        [Test]
        public void OverflowNullableInt32WithSelector()
        {
            string[] source = { "x", "y" };
            Assert.Throws<OverflowException>(() => source.Sum(x => (int?) int.MaxValue));
        }

        [Test]
        public void OverflowOfComputableSumInt32()
        {
            int[] source = { int.MaxValue, 1, -1, -int.MaxValue };
            // In a world where we summed using a long accumulator, the
            // result would be 0.
            Assert.Throws<OverflowException>(() => source.Sum());
        }
        #endregion

        #region Int64
        [Test]
        public void NullSourceInt64NoSelector()
        {
            long[] source = null;
            Assert.Throws<ArgumentNullException>(() => source.Sum());
        }

        [Test]
        public void NullSourceNullableInt64NoSelector()
        {
            long?[] source = null;
            Assert.Throws<ArgumentNullException>(() => source.Sum());
        }

        [Test]
        public void NullSourceInt64WithSelector()
        {
            string[] source = null;
            Assert.Throws<ArgumentNullException>(() => source.Sum(x => (long) x.Length));
        }

        [Test]
        public void NullSourceNullableInt64WithSelector()
        {
            string[] source = null;
            Assert.Throws<ArgumentNullException>(() => source.Sum(x => (long?)x.Length));
        }

        [Test]
        public void NullSelectorInt64()
        {
            string[] source = { };
            Func<string, long> selector = null;
            Assert.Throws<ArgumentNullException>(() => source.Sum(selector));
        }

        [Test]
        public void NullSelectorNullableInt64()
        {
            string[] source = { };
            Func<string, long?> selector = null;
            Assert.Throws<ArgumentNullException>(() => source.Sum(selector));
        }

        [Test]
        public void EmptySequenceInt64()
        {
            long[] source = { };
            Assert.AreEqual(0, source.Sum());
        }

        [Test]
        public void EmptySequenceNullableInt64()
        {
            long?[] source = { };
            Assert.AreEqual(0, source.Sum());
        }

        [Test]
        public void SequenceOfNullsNullableInt64()
        {
            long?[] source = { null, null };
            Assert.AreEqual(0, source.Sum());
        }

        [Test]
        public void EmptySequenceInt64WithSelector()
        {
            string[] source = { };
            Assert.AreEqual(0, source.Sum(x => (long) x.Length));
        }

        [Test]
        public void EmptySequenceNullableInt64WithSelector()
        {
            string[] source = { };
            Assert.AreEqual(0, source.Sum(x => (long?)x.Length));
        }

        [Test]
        public void SequenceOfNullsNullableInt64WithSelector()
        {
            string[] source = { "x", "y" };
            Assert.AreEqual(0, source.Sum(x => (long?)null));
        }

        [Test]
        public void SimpleSumInt64()
        {
            long[] source = { 1, 3, 2 };
            Assert.AreEqual(6, source.Sum());
        }

        [Test]
        public void SimpleSumNullableInt64()
        {
            long?[] source = { 1, 3, 2 };
            Assert.AreEqual(6, source.Sum());
        }

        [Test]
        public void SimpleSumNullableInt64IncludingNulls()
        {
            long?[] source = { 1, null, 3, null, 2 };
            Assert.AreEqual(6, source.Sum());
        }

        [Test]
        public void SimpleSumInt64WithSelector()
        {
            string[] source = { "x", "abc", "de" };
            Assert.AreEqual(6, source.Sum(x => (long) x.Length));
        }

        [Test]
        public void SimpleSumNullableInt64WithSelector()
        {
            string[] source = { "x", "abc", "de" };
            Assert.AreEqual(6, source.Sum(x => (long?)x.Length));
        }

        [Test]
        public void SimpleSumNullableInt64WithSelectorIncludingNulls()
        {
            string[] source = { "x", "null", "abc", "null", "de" };
            Assert.AreEqual(6, source.Sum(x => x == "null" ? null : (long?)x.Length));
        }

        [Test]
        public void NegativeOverflowInt64()
        {
            // Only test this once per type - the other overflow tests should be enough
            // for different method calls
            long[] source = { long.MinValue, long.MinValue };
            Assert.Throws<OverflowException>(() => source.Sum());
        }

        [Test]
        public void OverflowInt64()
        {
            long[] source = { long.MaxValue, long.MaxValue };
            Assert.Throws<OverflowException>(() => source.Sum());
        }

        [Test]
        public void OverflowInt64WithSelector()
        {
            string[] source = { "x", "y" };
            Assert.Throws<OverflowException>(() => source.Sum(x => long.MaxValue));
        }

        [Test]
        public void OverflowNullableInt64()
        {
            long?[] source = { long.MaxValue, long.MaxValue };
            Assert.Throws<OverflowException>(() => source.Sum());
        }

        [Test]
        public void OverflowNullableInt64WithSelector()
        {
            string[] source = { "x", "y" };
            Assert.Throws<OverflowException>(() => source.Sum(x => (long?)long.MaxValue));
        }
        #endregion

        #region Decimal
        [Test]
        public void NullSourceDecimalNoSelector()
        {
            decimal[] source = null;
            Assert.Throws<ArgumentNullException>(() => source.Sum());
        }

        [Test]
        public void NullSourceNullableDecimalNoSelector()
        {
            decimal?[] source = null;
            Assert.Throws<ArgumentNullException>(() => source.Sum());
        }

        [Test]
        public void NullSourceDecimalWithSelector()
        {
            string[] source = null;
            Assert.Throws<ArgumentNullException>(() => source.Sum(x => (decimal) x.Length));
        }

        [Test]
        public void NullSourceNullableDecimalWithSelector()
        {
            string[] source = null;
            Assert.Throws<ArgumentNullException>(() => source.Sum(x => (decimal?)x.Length));
        }

        [Test]
        public void NullSelectorDecimal()
        {
            string[] source = { };
            Func<string, decimal> selector = null;
            Assert.Throws<ArgumentNullException>(() => source.Sum(selector));
        }

        [Test]
        public void NullSelectorNullableDecimal()
        {
            string[] source = { };
            Func<string, decimal?> selector = null;
            Assert.Throws<ArgumentNullException>(() => source.Sum(selector));
        }

        [Test]
        public void EmptySequenceDecimal()
        {
            decimal[] source = { };
            Assert.AreEqual(0, source.Sum());
        }

        [Test]
        public void EmptySequenceNullableDecimal()
        {
            decimal?[] source = { };
            Assert.AreEqual(0, source.Sum());
        }

        [Test]
        public void SequenceOfNullsNullableDecimal()
        {
            decimal?[] source = { null, null };
            Assert.AreEqual(0, source.Sum());
        }

        [Test]
        public void EmptySequenceDecimalWithSelector()
        {
            string[] source = { };
            Assert.AreEqual(0, source.Sum(x => (decimal) x.Length));
        }

        [Test]
        public void EmptySequenceNullableDecimalWithSelector()
        {
            string[] source = { };
            Assert.AreEqual(0, source.Sum(x => (decimal?)x.Length));
        }

        [Test]
        public void SequenceOfNullsNullableDecimalWithSelector()
        {
            string[] source = { "x", "y" };
            Assert.AreEqual(0, source.Sum(x => (decimal?)null));
        }

        [Test]
        public void SimpleSumDecimal()
        {
            decimal[] source = { 1, 3, 2 };
            Assert.AreEqual(6, source.Sum());
        }

        [Test]
        public void SimpleSumNullableDecimal()
        {
            decimal?[] source = { 1, 3, 2 };
            Assert.AreEqual(6, source.Sum());
        }

        [Test]
        public void SimpleSumNullableDecimalIncludingNulls()
        {
            decimal?[] source = { 1, null, 3, null, 2 };
            Assert.AreEqual(6, source.Sum());
        }

        [Test]
        public void SimpleSumDecimalWithSelector()
        {
            string[] source = { "x", "abc", "de" };
            Assert.AreEqual(6, source.Sum(x => (decimal) x.Length));
        }

        [Test]
        public void SimpleSumNullableDecimalWithSelector()
        {
            string[] source = { "x", "abc", "de" };
            Assert.AreEqual(6, source.Sum(x => (decimal?)x.Length));
        }

        [Test]
        public void SimpleSumNullableDecimalWithSelectorIncludingNulls()
        {
            string[] source = { "x", "null", "abc", "null", "de" };
            Assert.AreEqual(6, source.Sum(x => x == "null" ? null : (decimal?)x.Length));
        }

        [Test]
        public void NegativeOverflowDecimal()
        {
            // Only test this once per type - the other overflow tests should be enough
            // for different method calls
            decimal[] source = { decimal.MinValue, decimal.MinValue };
            Assert.Throws<OverflowException>(() => source.Sum());
        }

        [Test]
        public void OverflowDecimal()
        {
            decimal[] source = { decimal.MaxValue, decimal.MaxValue };
            Assert.Throws<OverflowException>(() => source.Sum());
        }

        [Test]
        public void OverflowDecimalWithSelector()
        {
            string[] source = { "x", "y" };
            Assert.Throws<OverflowException>(() => source.Sum(x => decimal.MaxValue));
        }

        [Test]
        public void OverflowNullableDecimal()
        {
            decimal?[] source = { decimal.MaxValue, decimal.MaxValue };
            Assert.Throws<OverflowException>(() => source.Sum());
        }

        [Test]
        public void OverflowNullableDecimalWithSelector()
        {
            string[] source = { "x", "y" };
            Assert.Throws<OverflowException>(() => source.Sum(x => (decimal?)decimal.MaxValue));
        }
        #endregion

        #region Single
        [Test]
        public void NullSourceSingleNoSelector()
        {
            float[] source = null;
            Assert.Throws<ArgumentNullException>(() => source.Sum());
        }

        [Test]
        public void NullSourceNullableSingleNoSelector()
        {
            float?[] source = null;
            Assert.Throws<ArgumentNullException>(() => source.Sum());
        }

        [Test]
        public void NullSourceSingleWithSelector()
        {
            string[] source = null;
            Assert.Throws<ArgumentNullException>(() => source.Sum(x => (float) x.Length));
        }

        [Test]
        public void NullSourceNullableSingleWithSelector()
        {
            string[] source = null;
            Assert.Throws<ArgumentNullException>(() => source.Sum(x => (float?)x.Length));
        }

        [Test]
        public void NullSelectorSingle()
        {
            string[] source = { };
            Func<string, float> selector = null;
            Assert.Throws<ArgumentNullException>(() => source.Sum(selector));
        }

        [Test]
        public void NullSelectorNullableSingle()
        {
            string[] source = { };
            Func<string, float?> selector = null;
            Assert.Throws<ArgumentNullException>(() => source.Sum(selector));
        }

        [Test]
        public void EmptySequenceSingle()
        {
            float[] source = { };
            Assert.AreEqual(0, source.Sum());
        }

        [Test]
        public void EmptySequenceNullableSingle()
        {
            float?[] source = { };
            Assert.AreEqual(0, source.Sum());
        }

        [Test]
        public void SequenceOfNullsNullableSingle()
        {
            float?[] source = { null, null };
            Assert.AreEqual(0, source.Sum());
        }

        [Test]
        public void EmptySequenceSingleWithSelector()
        {
            string[] source = { };
            Assert.AreEqual(0, source.Sum(x => (float) x.Length));
        }

        [Test]
        public void EmptySequenceNullableSingleWithSelector()
        {
            string[] source = { };
            Assert.AreEqual(0, source.Sum(x => (float?)x.Length));
        }

        [Test]
        public void SequenceOfNullsNullableSingleWithSelector()
        {
            string[] source = { "x", "y" };
            Assert.AreEqual(0, source.Sum(x => (float?)null));
        }

        [Test]
        public void SimpleSumSingle()
        {
            float[] source = { 1, 3, 2 };
            Assert.AreEqual(6, source.Sum());
        }

        [Test]
        public void SumWithNaNSingle()
        {
            float[] source = { 1, 3, float.NaN, 2 };
            Assert.IsNaN(source.Sum());
        }

        [Test]
        public void SimpleSumNullableSingle()
        {
            float?[] source = { 1, 3, 2 };
            Assert.AreEqual(6, source.Sum());
        }

        [Test]
        public void SumWithNaNNullableSingle()
        {
            float[] source = { 1, 3, float.NaN, 2 };
            Assert.IsNaN(source.Sum());
        }

        [Test]
        public void SimpleSumNullableSingleIncludingNulls()
        {
            float?[] source = { 1, null, 3, null, 2 };
            Assert.AreEqual(6, source.Sum());
        }

        [Test]
        public void SimpleSumSingleWithSelector()
        {
            string[] source = { "x", "abc", "de" };
            Assert.AreEqual(6, source.Sum(x => (float) x.Length));
        }

        [Test]
        public void SimpleSumSingleWithSelectorWithNan()
        {
            string[] source = { "x", "abc", "de" };
            Assert.IsNaN(source.Sum(x => x.Length == 3 ? x.Length : float.NaN));
        }

        [Test]
        public void SimpleSumNullableSingleWithSelector()
        {
            string[] source = { "x", "abc", "de" };
            Assert.AreEqual(6, source.Sum(x => (float?)x.Length));
        }

        [Test]
        public void SimpleSumNullableSingleWithSelectorIncludingNulls()
        {
            string[] source = { "x", "null", "abc", "null", "de" };
            Assert.AreEqual(6, source.Sum(x => x == "null" ? null : (float?)x.Length));
        }

        [Test]
        public void SimpleSumNullableSingleWithSelectorWithNan()
        {
            string[] source = { "x", "abc", "de" };
            Assert.IsNaN(source.Sum(x => x.Length == 3 ? x.Length : (float?) float.NaN));
        }

        [Test]
        public void OverflowToNegativeInfinitySingle()
        {
            // Only test this once per type - the other overflow tests should be enough
            // for different method calls
            float[] source = { float.MinValue, float.MinValue };
            Assert.IsTrue(float.IsNegativeInfinity(source.Sum()));
        }

        [Test]
        public void OverflowToInfinitySingle()
        {
            float[] source = { float.MaxValue, float.MaxValue };
            Assert.IsTrue(float.IsPositiveInfinity(source.Sum()));
        }

        [Test]
        public void OverflowToInfinitySingleWithSelector()
        {
            string[] source = { "x", "y" };
            Assert.IsTrue(float.IsPositiveInfinity(source.Sum(x => float.MaxValue)));
        }

        [Test]
        public void OverflowToInfinityNullableSingle()
        {
            float?[] source = { float.MaxValue, float.MaxValue };
            Assert.IsTrue(float.IsPositiveInfinity(source.Sum().Value));
        }

        [Test]
        public void OverflowToInfinityNullableSingleWithSelector()
        {
            string[] source = { "x", "y" };
            Assert.IsTrue(float.IsPositiveInfinity(source.Sum(x => (float?)float.MaxValue).Value));
        }

        [Test]
        public void NonOverflowOfComputableSumSingle()
        {
            float[] source = { float.MaxValue, float.MaxValue,
                              -float.MaxValue, -float.MaxValue };
            // In a world where we summed using a float accumulator, the
            // result would be infinity.
            Assert.AreEqual(0f, source.Sum());
        }

        [Test]
        public void AccumulatorAccuracyForSingle()
        {
            // 20000000 and 20000004 are both exactly representable as
            // float values, but 20000001 is not. Therefore if we use
            // a float accumulator, we'll end up with 20000000. However,
            // if we use a double accumulator, we'll get the right value.
            float[] array = { 20000000f, 1f, 1f, 1f, 1f };
            Assert.AreEqual(20000004f, array.Sum());
        }
        #endregion

        #region Double
        [Test]
        public void NullSourceDoubleNoSelector()
        {
            double[] source = null;
            Assert.Throws<ArgumentNullException>(() => source.Sum());
        }

        [Test]
        public void NullSourceNullableDoubleNoSelector()
        {
            double?[] source = null;
            Assert.Throws<ArgumentNullException>(() => source.Sum());
        }

        [Test]
        public void NullSourceDoubleWithSelector()
        {
            string[] source = null;
            Assert.Throws<ArgumentNullException>(() => source.Sum(x => (double) x.Length));
        }

        [Test]
        public void NullSourceNullableDoubleWithSelector()
        {
            string[] source = null;
            Assert.Throws<ArgumentNullException>(() => source.Sum(x => (double?)x.Length));
        }

        [Test]
        public void NullSelectorDouble()
        {
            string[] source = { };
            Func<string, double> selector = null;
            Assert.Throws<ArgumentNullException>(() => source.Sum(selector));
        }

        [Test]
        public void NullSelectorNullableDouble()
        {
            string[] source = { };
            Func<string, double?> selector = null;
            Assert.Throws<ArgumentNullException>(() => source.Sum(selector));
        }

        [Test]
        public void EmptySequenceDouble()
        {
            double[] source = { };
            Assert.AreEqual(0, source.Sum());
        }

        [Test]
        public void EmptySequenceNullableDouble()
        {
            double?[] source = { };
            Assert.AreEqual(0, source.Sum());
        }

        [Test]
        public void SequenceOfNullsNullableDouble()
        {
            double?[] source = { null, null };
            Assert.AreEqual(0, source.Sum());
        }

        [Test]
        public void EmptySequenceDoubleWithSelector()
        {
            string[] source = { };
            Assert.AreEqual(0, source.Sum(x => (double) x.Length));
        }

        [Test]
        public void EmptySequenceNullableDoubleWithSelector()
        {
            string[] source = { };
            Assert.AreEqual(0, source.Sum(x => (double?)x.Length));
        }

        [Test]
        public void SequenceOfNullsNullableDoubleWithSelector()
        {
            string[] source = { "x", "y" };
            Assert.AreEqual(0, source.Sum(x => (double?)null));
        }

        [Test]
        public void SimpleSumDouble()
        {
            double[] source = { 1, 3, 2 };
            Assert.AreEqual(6, source.Sum());
        }

        [Test]
        public void SumWithNaNDouble()
        {
            double[] source = { 1, 3, double.NaN, 2 };
            Assert.IsNaN(source.Sum());
        }

        [Test]
        public void SimpleSumNullableDouble()
        {
            double?[] source = { 1, 3, 2 };
            Assert.AreEqual(6, source.Sum());
        }

        [Test]
        public void SumWithNaNNullableDouble()
        {
            double[] source = { 1, 3, double.NaN, 2 };
            Assert.IsNaN(source.Sum());
        }

        [Test]
        public void SimpleSumNullableDoubleIncludingNulls()
        {
            double?[] source = { 1, null, 3, null, 2 };
            Assert.AreEqual(6, source.Sum());
        }

        [Test]
        public void SimpleSumDoubleWithSelector()
        {
            string[] source = { "x", "abc", "de" };
            Assert.AreEqual(6, source.Sum(x => (double) x.Length));
        }

        [Test]
        public void SimpleSumDoubleWithSelectorWithNan()
        {
            string[] source = { "x", "abc", "de" };
            Assert.IsNaN(source.Sum(x => x.Length == 3 ? x.Length : double.NaN));
        }

        [Test]
        public void SimpleSumNullableDoubleWithSelector()
        {
            string[] source = { "x", "abc", "de" };
            Assert.AreEqual(6, source.Sum(x => (double?)x.Length));
        }

        [Test]
        public void SimpleSumNullableDoubleWithSelectorIncludingNulls()
        {
            string[] source = { "x", "null", "abc", "null", "de" };
            Assert.AreEqual(6, source.Sum(x => x == "null" ? null : (double?)x.Length));
        }

        [Test]
        public void SimpleSumNullableDoubleWithSelectorWithNan()
        {
            string[] source = { "x", "abc", "de" };
            Assert.IsNaN(source.Sum(x => x.Length == 3 ? x.Length : (double?)double.NaN));
        }

        [Test]
        public void OverflowToNegativeInfinityDouble()
        {
            // Only test this once per type - the other overflow tests should be enough
            // for different method calls
            double[] source = { double.MinValue, double.MinValue };
            Assert.IsTrue(double.IsNegativeInfinity(source.Sum()));
        }

        [Test]
        public void OverflowToInfinityDouble()
        {
            double[] source = { double.MaxValue, double.MaxValue };
            Assert.IsTrue(double.IsPositiveInfinity(source.Sum()));
        }

        [Test]
        public void OverflowToInfinityDoubleWithSelector()
        {
            string[] source = { "x", "y" };
            Assert.IsTrue(double.IsPositiveInfinity(source.Sum(x => double.MaxValue)));
        }

        [Test]
        public void OverflowToInfinityNullableDouble()
        {
            double?[] source = { double.MaxValue, double.MaxValue };
            Assert.IsTrue(double.IsPositiveInfinity(source.Sum().Value));
        }

        [Test]
        public void OverflowToInfinityNullableDoubleWithSelector()
        {
            string[] source = { "x", "y" };
            Assert.IsTrue(double.IsPositiveInfinity(source.Sum(x => (double?)double.MaxValue).Value));
        }
        #endregion
    }
}
