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
    public class ElementAtOrDefaultTest
    {
        [Test]
        public void NullSource()
        {
            int[] source = null;
            Assert.Throws<ArgumentNullException>(() => source.ElementAtOrDefault(0));
        }

        [Test]
        public void NegativeIndex()
        {
            int[] source = { 90, 91, 92 };
            Assert.AreEqual(0, source.ElementAtOrDefault(-1));
        }

        [Test]
        [Ignore("LINQ to Objects doesn't test for collection separately")]
        public void OvershootIndexOnCollection()
        {
            IEnumerable<int> source = new NonEnumerableCollection<int> { 90, 91, 92 };
            Assert.AreEqual(0, source.ElementAtOrDefault(3));
        }

        [Test]
        public void OvershootIndexOnList()
        {
            IEnumerable<int> source = new NonEnumerableList<int> { 90, 91, 92 };
            Assert.AreEqual(0, source.ElementAtOrDefault(3));
        }

        [Test]
        public void OvershootIndexOnLazySequence()
        {
            IEnumerable<int> source = Enumerable.Range(0, 3);
            Assert.AreEqual(0, source.ElementAtOrDefault(3));
        }

        [Test]
        public void ValidIndexOnList()
        {
            IEnumerable<int> source = new NonEnumerableList<int>(100, 56, 93, 22);
            Assert.AreEqual(93, source.ElementAtOrDefault(2));
        }

        [Test]
        public void ValidIndexOnLazySequence()
        {
            IEnumerable<int> source = Enumerable.Range(10, 5);
            Assert.AreEqual(12, source.ElementAtOrDefault(2));
        }
    }
}
