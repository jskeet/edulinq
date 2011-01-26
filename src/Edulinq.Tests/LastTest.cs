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
    /// <summary>
    /// This class uses LinkedList[T] for most of the tests, as that doesn't implement IList[T] so won't
    /// go through any optimizations. There are then a couple of tests at the bottom for lists. It's
    /// possible this is overkill, particularly for the predicate tests where we actually test that
    /// it *isn't* optimized anyway... but it keeps the class consistent.
    /// </summary>
    [TestFixture]
    public class LastTest
    {
        [Test]
        public void NullSourceWithoutPredicate()
        {
            int[] source = null;
            Assert.Throws<ArgumentNullException>(() => source.Last());
        }

        [Test]
        public void NullSourceWithPredicate()
        {
            int[] source = null;
            Assert.Throws<ArgumentNullException>(() => source.Last(x => x > 3));
        }

        [Test]
        public void NullPredicate()
        {
            var source = new LinkedList<int>(new int[] { 1, 3, 5 });
            Assert.Throws<ArgumentNullException>(() => source.Last(null));
        }

        [Test]
        public void EmptySequenceWithoutPredicate()
        {
            var source = new LinkedList<int>();
            Assert.Throws<InvalidOperationException>(() => source.Last());
        }

        [Test]
        public void SingleElementSequenceWithoutPredicate()
        {
            var source = new LinkedList<int>(new int[] { 5 });
            Assert.AreEqual(5, source.Last());
        }

        [Test]
        public void MultipleElementSequenceWithoutPredicate()
        {
            var source = new LinkedList<int>(new int[] { 5, 10 });
            Assert.AreEqual(10, source.Last());
        }

        [Test]
        public void EmptySequenceWithPredicate()
        {
            var source = new LinkedList<int>();
            Assert.Throws<InvalidOperationException>(() => source.Last(x => x > 3));
        }

        [Test]
        public void SingleElementSequenceWithMatchingPredicate()
        {
            var source = new LinkedList<int>(new int[] { 5 });
            Assert.AreEqual(5, source.Last(x => x > 3));
        }

        [Test]
        public void SingleElementSequenceWithNonMatchingPredicate()
        {
            var source = new LinkedList<int>(new int[] { 2 });
            Assert.Throws<InvalidOperationException>(() => source.Last(x => x > 3));
        }

        [Test]
        public void MultipleElementSequenceWithNoPredicateMatches()
        {
            var source = new LinkedList<int>(new int[] { 1, 2, 2, 1 });
            Assert.Throws<InvalidOperationException>(() => source.Last(x => x > 3));
        }

        [Test]
        public void MultipleElementSequenceWithSinglePredicateMatch()
        {
            var source = new LinkedList<int>(new int[] { 1, 2, 5, 2, 1 });
            Assert.AreEqual(5, source.Last(x => x > 3));
        }

        [Test]
        public void MultipleElementSequenceWithMultiplePredicateMatches()
        {
            var source = new LinkedList<int>(new int[] { 1, 2, 5, 10, 2, 1 });
            Assert.AreEqual(10, source.Last(x => x > 3));
        }

        [Test]
        public void ListWithoutPredicateDoesntIterate()
        {
            var source = new NonEnumerableList<int>(1, 5, 10, 3);
            Assert.AreEqual(3, source.Last());
        }

        // See discussion in blog post around this... it could be optimized, but the framework doesn't.
        [Test]
        public void ListWithPredicateStillIterates()
        {
            var source = new NonEnumerableList<int>(1, 5, 10, 3);
            Assert.Throws<NotSupportedException>(() => source.Last(x => x > 3));
        }
    }
}
