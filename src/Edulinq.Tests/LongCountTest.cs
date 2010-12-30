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
using Edulinq.TestSupport;
using NUnit.Framework;

namespace Edulinq.Tests
{
    [TestFixture]
    public class LongCountTest
    {
        [Test]
        public void NonCollectionCount()
        {
            Assert.AreEqual(5, Enumerable.Range(2, 5).LongCount());
        }

        [Test]
        public void GenericOnlyCollectionCount()
        {
            Assert.AreEqual(5, new GenericOnlyCollection<int>(Enumerable.Range(2, 5)).LongCount());
        }

        [Test]
        public void SemiGenericCollectionCount()
        {
            Assert.AreEqual(5, new SemiGenericCollection(Enumerable.Range(2, 5)).LongCount());
        }

        [Test]
        public void RegularGenericCollectionCount()
        {
            Assert.AreEqual(5, new List<int>(Enumerable.Range(2, 5)).LongCount());
        }

        [Test]
        public void NullSourceThrowsArgumentNullException()
        {
            IEnumerable<int> source = null;
            Assert.Throws<ArgumentNullException>(() => source.LongCount());
        }

        [Test]
        public void PredicatedNullSourceThrowsArgumentNullException()
        {
            IEnumerable<int> source = null;
            Assert.Throws<ArgumentNullException>(() => source.LongCount(x => x == 1));
        }

        [Test]
        public void PredicatedNullPredicateThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new int[0].LongCount(null));
        }

        [Test]
        public void PredicatedCount()
        {
            // Counts even numbers within 2, 3, 4, 5, 6
            Assert.AreEqual(3, Enumerable.Range(2, 5).LongCount(x => x % 2 == 0));
        }

        [Test]
        [Ignore("Takes an enormous amount of time!")]
        public void CollectionBiggerThanMaxInt32CanBeCountedWithLongCount()
        {
            var hugeCollection = Enumerable.Range(0, int.MaxValue).Concat(Enumerable.Range(0, 1));
            Assert.AreEqual(int.MaxValue + 1L, hugeCollection.LongCount());
        }

        [Test]
        [Ignore("Takes an enormous amount of time!")]
        public void CollectionBiggerThanMaxInt32CanBeCountedWithLongCountWithPredicate()
        {
            var hugeCollection = Enumerable.Range(0, int.MaxValue).Concat(Enumerable.Range(0, 1));
            Assert.AreEqual(int.MaxValue + 1L, hugeCollection.LongCount(x => x >= 0));
        }
    }
}
