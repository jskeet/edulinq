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
    public class SkipTest
    {
        [Test]
        public void ExecutionIsDeferred()
        {
            new ThrowingEnumerable().Skip(10);
        }

        [Test]
        public void NullSource()
        {
            string[] source = null;
            Assert.Throws<ArgumentNullException>(() => source.Skip(10));
        }

        [Test]
        public void NegativeCount()
        {
            Enumerable.Range(0, 5).Skip(-5).AssertSequenceEqual(0, 1, 2, 3, 4);
        }

        [Test]
        public void ZeroCount()
        {
            Enumerable.Range(0, 5).Skip(0).AssertSequenceEqual(0, 1, 2, 3, 4);
        }

        [Test]
        public void NegativeCountWithArray()
        {
            new int[] { 0, 1, 2, 3, 4 }.Skip(-5).AssertSequenceEqual(0, 1, 2, 3, 4);
        }

        [Test]
        public void ZeroCountWithArray()
        {
            new int[] { 0, 1, 2, 3, 4 }.Skip(0).AssertSequenceEqual(0, 1, 2, 3, 4);
        }

        [Test]
        public void CountShorterThanSource()
        {
            Enumerable.Range(0, 5).Skip(3).AssertSequenceEqual(3, 4);
        }

        [Test]
        public void CountEqualToSourceLength()
        {
            Enumerable.Range(0, 5).Skip(5).AssertSequenceEqual();
        }

        [Test]
        public void CountGreaterThanSourceLength()
        {
            Enumerable.Range(0, 5).Skip(100).AssertSequenceEqual();
        }
    }
}
