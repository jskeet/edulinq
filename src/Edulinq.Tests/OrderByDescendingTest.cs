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
    public class OrderByDescendingTest
    {
        [Test]
        public void ExecutionIsDeferred()
        {
            new ThrowingEnumerable().OrderByDescending(x => x);
        }

        [Test]
        public void NullSourceNoComparer()
        {
            int[] source = null;
            Func<int, int> keySelector = x => x;
            Assert.Throws<ArgumentNullException>(() => source.OrderByDescending(keySelector));
        }

        [Test]
        public void NullKeySelectorNoComparer()
        {
            int[] source = new int[0];
            Func<int, int> keySelector = null;
            Assert.Throws<ArgumentNullException>(() => source.OrderByDescending(keySelector));
        }

        [Test]
        public void NullSourceWithComparer()
        {
            int[] source = null;
            Func<int, int> keySelector = x => x;
            Assert.Throws<ArgumentNullException>(() => source.OrderByDescending(keySelector, Comparer<int>.Default));
        }

        [Test]
        public void NullKeySelectorWithComparer()
        {
            int[] source = new int[0];
            Func<int, int> keySelector = null;
            Assert.Throws<ArgumentNullException>(() => source.OrderByDescending(keySelector, Comparer<int>.Default));
        }

        [Test]
        public void SimpleUniqueKeys()
        {
            var source = new[]
            {
                new { Value = 1, Key = 10 },
                new { Value = 2, Key = 12 },
                new { Value = 3, Key = 11 }
            };
            var query = source.OrderByDescending(x => x.Key)
                              .Select(x => x.Value);
            query.AssertSequenceEqual(2, 3, 1);
        }

        [Test]
        public void NullsAreLast()
        {
            var source = new[]
            {
                new { Value = 1, Key = "abc" },
                new { Value = 2, Key = (string) null },
                new { Value = 3, Key = "def" }
            };
            var query = source.OrderByDescending(x => x.Key, StringComparer.Ordinal)
                              .Select(x => x.Value);
            query.AssertSequenceEqual(3, 1, 2);
        }

        [Test]
        public void OrderingIsStable()
        {
            var source = new[]
            {
                new { Value = 1, Key = 10 },
                new { Value = 2, Key = 11 },
                new { Value = 3, Key = 11 },
                new { Value = 4, Key = 10 },
            };
            var query = source.OrderByDescending(x => x.Key)
                              .Select(x => x.Value);
            query.AssertSequenceEqual(2, 3, 1, 4);
        }

        [Test]
        public void NullComparerIsDefault()
        {
            var source = new[]
            {
                new { Value = 1, Key = 15 },
                new { Value = 2, Key = -13 },
                new { Value = 3, Key = 11 }
            };
            var query = source.OrderByDescending(x => x.Key, null)
                              .Select(x => x.Value);
            query.AssertSequenceEqual(1, 3, 2);
        }

        [Test]
        public void CustomComparer()
        {
            var source = new[]
            {
                new { Value = 1, Key = 15 },
                new { Value = 2, Key = -13 },
                new { Value = 3, Key = 11 }
            };
            var query = source.OrderByDescending(x => x.Key, new AbsoluteValueComparer())
                              .Select(x => x.Value);
            query.AssertSequenceEqual(1, 2, 3);
        }

        [Test]
        [Ignore("Fails in LINQ to Objects. See Connect issue: http://goo.gl/p12su")]
        public void CustomExtremeComparer()
        {
            int[] values = { 1, 3, 2, 4, 8, 5, 7, 6 };
            var query = values.OrderByDescending(x => x, new ExtremeComparer());
            query.AssertSequenceEqual(8, 7, 6, 5, 4, 3, 2, 1);
        }

        // Comparer which is equivalent to the default comparer for int, but
        // always returns int.MinValue, 0, or int.MaxValue.
        private class ExtremeComparer : IComparer<int>
        {
            public int Compare(int x, int y)
            {
                return x == y ? 0
                    : x < y ? int.MinValue
                    : int.MaxValue;
            }
        }
    }
}
