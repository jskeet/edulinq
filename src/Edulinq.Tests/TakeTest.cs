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
    public class TakeTest
    {
        [Test]
        public void ExecutionIsDeferred()
        {
            new ThrowingEnumerable().Take(10);
        }

        [Test]
        public void NullSource()
        {
            string[] source = null;
            Assert.Throws<ArgumentNullException>(() => source.Take(10));
        }

        [Test]
        public void NegativeCount()
        {
            Enumerable.Range(0, 5).Take(-5).AssertSequenceEqual();
        }

        [Test]
        public void ZeroCount()
        {
            Enumerable.Range(0, 5).Take(-5).AssertSequenceEqual();
        }

        [Test]
        public void CountShorterThanSource()
        {
            Enumerable.Range(0, 5).Take(3).AssertSequenceEqual(0, 1, 2);
        }

        [Test]
        public void CountEqualToSourceLength()
        {
            Enumerable.Range(0, 5).Take(5).AssertSequenceEqual(0, 1, 2, 3, 4);
        }

        [Test]
        public void CountGreaterThanSourceLength()
        {
            Enumerable.Range(0, 5).Take(100).AssertSequenceEqual(0, 1, 2, 3, 4);
        }

        [Test]
        public void OnlyEnumerateTheGivenNumberOfElements()
        {
            int[] source = { 1, 2, 0 };
            // If we try to move onto the third element, we'll die.
            var query = source.Select(x => 10 / x);
            query.Take(2).AssertSequenceEqual(10, 5);
        }
    }
}
