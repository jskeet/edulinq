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
    public class RangeTest
    {
        [Test]
        public void NegativeCount()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Enumerable.Range(10, -1));
        }

        [Test]
        public void CountTooLarge()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Enumerable.Range(int.MaxValue, 2));
            Assert.Throws<ArgumentOutOfRangeException>(() => Enumerable.Range(2, int.MaxValue));
            // int.MaxValue is odd, hence the +3 instead of +2
            Assert.Throws<ArgumentOutOfRangeException>(() => Enumerable.Range(int.MaxValue / 2, (int.MaxValue / 2) + 3));
        }

        [Test]
        public void LargeButValidCount()
        {
            // Essentially the edge conditions for CountTooLarge, but just below the boundary
            Enumerable.Range(int.MaxValue, 1);
            Enumerable.Range(1, int.MaxValue);
            Enumerable.Range(int.MaxValue / 2, (int.MaxValue / 2) + 2);
        }

        [Test]
        public void ValidRange()
        {
            Enumerable.Range(5, 3).AssertSequenceEqual(5, 6, 7);
        }

        [Test]
        public void NegativeStart()
        {
            Enumerable.Range(-2, 5).AssertSequenceEqual(-2, -1, 0, 1, 2);
        }

        [Test]
        public void EmptyRange()
        {
            Enumerable.Range(100, 0).AssertSequenceEqual();
        }

        [Test]
        public void SingleValueOfMaxInt32()
        {
            Enumerable.Range(int.MaxValue, 1).AssertSequenceEqual(int.MaxValue);
        }

        [Test]
        public void EmptyRangeStartingAtMinInt32()
        {
            Enumerable.Range(int.MinValue, 0).AssertSequenceEqual();
        }
    }
}
