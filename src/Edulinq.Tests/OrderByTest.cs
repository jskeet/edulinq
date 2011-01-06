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
using System.Collections.Generic;

namespace Edulinq.Tests
{
    [TestFixture]
    public class OrderByTest
    {
        [Test]
        public void ExecutionIsDeferred()
        {
            new ThrowingEnumerable().OrderBy(x => x);
        }

        [Test]
        public void NullSourceNoComparer()
        {
            int[] source = null;
            Func<int, int> keySelector = x => x;
            Assert.Throws<ArgumentNullException>(() => source.OrderBy(keySelector));
        }

        [Test]
        public void NullKeySelectorNoComparer()
        {
            int[] source = new int[0];
            Func<int, int> keySelector = null;
            Assert.Throws<ArgumentNullException>(() => source.OrderBy(keySelector));
        }

        [Test]
        public void NullSourceWithComparer()
        {
            int[] source = null;
            Func<int, int> keySelector = x => x;
            Assert.Throws<ArgumentNullException>(() => source.OrderBy(keySelector, Comparer<int>.Default));
        }

        [Test]
        public void NullKeySelectorWithComparer()
        {
            int[] source = new int[0];
            Func<int, int> keySelector = null;
            Assert.Throws<ArgumentNullException>(() => source.OrderBy(keySelector, Comparer<int>.Default));
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
            var query = source.OrderBy(x => x.Key)
                              .Select(x => x.Value);
            query.AssertSequenceEqual(1, 3, 2);
        }

        [Test]
        public void NullsAreFirst()
        {
            var source = new[]
            {
                new { Value = 1, Key = "abc" },
                new { Value = 2, Key = (string) null },
                new { Value = 3, Key = "def" }
            };
            var query = source.OrderBy(x => x.Key, StringComparer.Ordinal)
                              .Select(x => x.Value);
            query.AssertSequenceEqual(2, 1, 3);
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
            var query = source.OrderBy(x => x.Key)
                              .Select(x => x.Value);
            query.AssertSequenceEqual(1, 4, 2, 3);
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
            var query = source.OrderBy(x => x.Key, null)
                              .Select(x => x.Value);
            query.AssertSequenceEqual(2, 3, 1);
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
            var query = source.OrderBy(x => x.Key, new AbsoluteValueComparer())
                              .Select(x => x.Value);
            query.AssertSequenceEqual(3, 2, 1);
        }

        [Test]
        public void KeySelectorIsCalledExactlyOncePerElement()
        {
            int[] values = { 1, 5, 4, 2, 3, 7, 6, 8, 9 };
            int count = 0;
            var query = values.OrderBy(x => { count++; return x; });
            query.AssertSequenceEqual(1, 2, 3, 4, 5, 6, 7, 8, 9);
            Assert.AreEqual(9, count);
        }
    }
}
