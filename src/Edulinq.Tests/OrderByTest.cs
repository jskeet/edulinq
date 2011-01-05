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
                new { Value = 1, Key = "abc" },
                new { Value = 2, Key = "ghi" },
                new { Value = 3, Key = "def" }
            };
            var query = source.OrderBy(x => x.Key)
                              .Select(x => x.Value);
            query.AssertSequenceEqual(1, 3, 2);
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
            var query = source.OrderBy(x => x.Key)
                              .Select(x => x.Value);
            query.AssertSequenceEqual(2, 1, 3);
        }

        [Test]
        public void OrderingIsStable()
        {
            var source = new[]
            {
                new { Value = 1, Key = "abc" },
                new { Value = 2, Key = "def" },
                new { Value = 3, Key = "def" },
                new { Value = 4, Key = "abc" },
            };
            var query = source.OrderBy(x => x.Key)
                              .Select(x => x.Value);
            query.AssertSequenceEqual(1, 4, 2, 3);
        }

        [Test]
        public void CustomOrdinalComparer()
        {
            var source = new[]
            {
                new { Value = 1, Key = "abc" },
                new { Value = 2, Key = "GHI" },
                new { Value = 3, Key = "DEF" }
            };
            // Upper case comes before lower case in Unicode and thus
            // in the ordinal comparer
            var query = source.OrderBy(x => x.Key, StringComparer.Ordinal)
                              .Select(x => x.Value);
            query.AssertSequenceEqual(3, 2, 1);
        }

        [Test]
        public void CustomCaseInsensitiveComparer()
        {
            var source = new[]
            {
                new { Value = 1, Key = "abc" },
                new { Value = 2, Key = "GHI" },
                new { Value = 3, Key = "DEF" }
            };
            var query = source.OrderBy(x => x.Key, StringComparer.OrdinalIgnoreCase)
                              .Select(x => x.Value);
            query.AssertSequenceEqual(1, 3, 2);
        }
    }
}
