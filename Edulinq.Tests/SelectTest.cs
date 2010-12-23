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
using System.Text;
using NUnit.Framework;

namespace Edulinq.Tests
{
    [TestFixture]
    public class SelectTest
    {
        [Test]
        public void NullSourceThrowsNullArgumentException()
        {
            IEnumerable<int> source = null;
            Assert.Throws<ArgumentNullException>(() => source.Select(x => x + 1));
        }

        [Test]
        public void NullProjectionThrowsNullArgumentException()
        {
            int[] source = { 1, 3, 7, 9, 10 };
            Func<int, int> projection = null;
            Assert.Throws<ArgumentNullException>(() => source.Select(projection));
        }

        [Test]
        public void WithIndexNullSourceThrowsNullArgumentException()
        {
            IEnumerable<int> source = null;
            Assert.Throws<ArgumentNullException>(() => source.Select((x, index) => x + index));
        }

        [Test]
        public void WithIndexNullPredicateThrowsNullArgumentException()
        {
            int[] source = { 1, 3, 7, 9, 10 };
            Func<int, int, int> projection = null;
            Assert.Throws<ArgumentNullException>(() => source.Select(projection));
        }

        [Test]
        public void SimpleProjection()
        {
            int[] source = { 1, 5, 2 };
            var result = source.Select(x => x * 2);
            result.AssertSequenceEqual(2, 10, 4);
        }

        [Test]
        public void SimpleProjectionWithQueryExpression()
        {
            int[] source = { 1, 5, 2 };
            var result = from x in source
                         select x * 2;
            result.AssertSequenceEqual(2, 10, 4);
        }

        [Test]
        public void EmptySource()
        {
            int[] source = new int[0];
            var result = source.Select(x => x * 2);
            result.AssertSequenceEqual();
        }

        [Test]
        public void ExecutionIsDeferred()
        {
            ThrowingEnumerable.AssertDeferred(src => src.Select(x => x * 2));
        }

        [Test]
        public void WithIndexSimpleProjection()
        {
            int[] source = { 1, 5, 2 };
            var result = source.Select((x, index) => x * 2);
            result.AssertSequenceEqual(2, 1);
        }

        [Test]
        public void WithIndexEmptySource()
        {
            int[] source = new int[0];
            var result = source.Where((x, index) => x < 4);
            result.AssertSequenceEqual();
        }

        [Test]
        public void WithIndexExecutionIsDeferred()
        {
            ThrowingEnumerable.AssertDeferred(src => src.Select((x, index) => x + index));
        }
    }
}
