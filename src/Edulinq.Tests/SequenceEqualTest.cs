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
using Edulinq.TestSupport;

namespace Edulinq.Tests
{
    [TestFixture]
    public class SequenceEqualTest
    {
        // TestString1 and TestString2 are distinct but equal strings
        private static readonly string TestString1 = "test";
        private static readonly string TestString2 = new string(TestString1.ToCharArray());

        [Test]
        public void FirstSourceNull()
        {
            string[] first = null;
            string[] second = { };
            Assert.Throws<ArgumentNullException>(() => first.SequenceEqual(second));
        }

        [Test]
        public void SecondSourceNull()
        {
            string[] first = { };
            string[] second = null;
            Assert.Throws<ArgumentNullException>(() => first.SequenceEqual(second));
        }

        [Test]
        public void NullComparerUsesDefault()
        {
            string[] first = { TestString1 };
            string[] second = { TestString2 };
            Assert.IsTrue(first.SequenceEqual(second, null));
            // Check it's not defaulting to case-insensitive matching...
            first = new[] { "FOO" };
            second = new[] { "BAR" };
            Assert.IsFalse(first.SequenceEqual(second, null));
        }

        [Test]
        public void UnequalLengthsBothArrays()
        {
            int[] first = { 1, 5, 3 };
            int[] second = { 1, 5, 3, 10 };
            Assert.IsFalse(first.SequenceEqual(second));
        }

        [Test]
        public void UnequalLengthsBothRangesFirstLonger()
        {
            var first = Enumerable.Range(0, 11);
            var second = Enumerable.Range(0, 10);
            Assert.IsFalse(first.SequenceEqual(second));
        }

        [Test]
        public void UnequalLengthsBothRangesSecondLonger()
        {
            var first = Enumerable.Range(0, 10);
            var second = Enumerable.Range(0, 11);
            Assert.IsFalse(first.SequenceEqual(second));
        }

        [Test]
        public void UnequalData()
        {
            int[] first = { 1, 5, 3, 9 };
            int[] second = { 1, 5, 3, 10 };
            Assert.IsFalse(first.SequenceEqual(second));
        }

        [Test]
        public void EqualDataBothArrays()
        {
            int[] first = { 1, 5, 3, 10 };
            int[] second = { 1, 5, 3, 10 };
            Assert.IsTrue(first.SequenceEqual(second));
        }

        [Test]
        public void EqualDataBothRanges()
        {
            var first = Enumerable.Range(0, 10);
            var second = Enumerable.Range(0, 10);
            Assert.IsTrue(first.SequenceEqual(second));
        }

        [Test]
        public void IdentityNonEqualityOfSequences()
        {
            // In this case we just use side-effects. The sequence could be
            // a sequence of random numbers though.
            int counter = 0;
            var source = Enumerable.Range(0, 5).Select(x => counter++);
            Assert.IsFalse(source.SequenceEqual(source));
        }

        [Test]
        public void InfiniteSequenceFirst()
        {
            var first = GetInfiniteSequence();
            int[] second = { 1, 1, 1 }; // Same elements to start with, but we stop
            Assert.IsFalse(first.SequenceEqual(second));
        }

        [Test]
        public void InfiniteSequenceSecond()
        {
            int[] first = { 1, 1, 1 }; // Same elements to start with, but we stop
            var second = GetInfiniteSequence();
            Assert.IsFalse(first.SequenceEqual(second));
        }

        [Test]
        public void CustomEqualityComparer()
        {
            string[] first = { "foo", "BAR", "baz" };
            string[] second = { "FOO", "bar", "Baz" };
            Assert.IsTrue(first.SequenceEqual(second, StringComparer.OrdinalIgnoreCase));
        }

        [Test]
        public void OrderMatters()
        {
            int[] first = { 1, 2 };
            int[] second = { 2, 1 };
            Assert.IsFalse(first.SequenceEqual(second));
        }

        [Test]
        [Ignore("LINQ to Objects doesn't optimize by count")]
        public void CountOptimization()
        {
            // The counts are different, so we don't need to iterate
            var first = new NonEnumerableCollection<int> { 1, 2, 3 };
            var second = new NonEnumerableCollection<int> { 1, 2 };
            Assert.IsFalse(first.SequenceEqual(second));
        }

#if !LINQBRIDGE
        [Test]
        public void DefaultComparerOfTypeIsUsedRegardlessOfCollection()
        {
            ICollection<string> set = HashSetProvider.NewHashSet<string>(
                StringComparer.OrdinalIgnoreCase, "abc");
            Assert.IsTrue(set.Contains("ABC"));
            Assert.AreEqual(1, set.Count);
            Assert.IsFalse(set.SequenceEqual(new[] { "ABC" }));
        }
#endif

        [Test]
        public void ReturnAtFirstDifference()
        {
            int[] source1 = { 1, 5, 10, 2, 0 };
            int[] source2 = { 1, 5, 10, 1, 0 };
            var query1 = source1.Select(x => 10 / x);
            var query2 = source2.Select(x => 10 / x);
            // If we ever needed to get to the final elements, we'd go bang
            Assert.IsFalse(query1.SequenceEqual(query2));
        }
        
        private static IEnumerable<int> GetInfiniteSequence()
        {
            while (true)
            {
                yield return 1;
            }
        }
    }
}
