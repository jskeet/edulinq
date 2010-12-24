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
// This time we can't just use Enumerable, as that will always use Edulinq.Enumerable
#if NORMAL_LINQ
using RangeClass = System.Linq.Enumerable;
#else
using RangeClass = Edulinq.Enumerable;
#endif

using NUnit.Framework;

namespace Edulinq.Tests
{
    [TestFixture]
    public class RangeTest
    {
        [Test]
        public void NegativeCount()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => RangeClass.Range(10, -1));
        }

        [Test]
        public void CountTooLarge()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => RangeClass.Range(int.MaxValue, 2));
            Assert.Throws<ArgumentOutOfRangeException>(() => RangeClass.Range(2, int.MaxValue));
            // int.MaxValue is odd, hence the +3 instead of +2
            Assert.Throws<ArgumentOutOfRangeException>(() => RangeClass.Range(int.MaxValue / 2, (int.MaxValue / 2) + 3));
        }

        [Test]
        public void LargeButValidCount()
        {
            // Essentially the edge conditions for CountTooLarge, but just below the boundary
            RangeClass.Range(int.MaxValue, 1);
            RangeClass.Range(1, int.MaxValue);
            RangeClass.Range(int.MaxValue / 2, (int.MaxValue / 2) + 2);
        }

        [Test]
        public void ValidRange()
        {
            RangeClass.Range(5, 3).AssertSequenceEqual(5, 6, 7);
        }
    }
}
