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
#if !LINQBRIDGE && !DOTNET35_ONLY
    [TestFixture]
    public class ZipTest
    {
        [Test]
        public void NullFirst()
        {
            string[] first = null;
            int[] second = { };
            Func<string, int, string> resultSelector = (x, y) => x + ":" + y;
            Assert.Throws<ArgumentNullException>(() => first.Zip(second, resultSelector));
        }

        [Test]
        public void NullSecond()
        {
            string[] first = { };
            int[] second = null;
            Func<string, int, string> resultSelector = (x, y) => x + ":" + y;
            Assert.Throws<ArgumentNullException>(() => first.Zip(second, resultSelector));
        }

        [Test]
        public void NullResultSelector()
        {
            string[] first = { };
            int[] second = { };
            Func<string, int, string> resultSelector = null;
            Assert.Throws<ArgumentNullException>(() => first.Zip(second, resultSelector));
        }

        [Test]
        public void ExecutionIsDeferred()
        {
            var first = new ThrowingEnumerable();
            var second = new ThrowingEnumerable();
            first.Zip(second, (x, y) => x + y);
        }

        [Test]
        public void ShortFirst()
        {
            string[] first = { "a", "b", "c" };
            var second = Enumerable.Range(5, 10);
            Func<string, int, string> resultSelector = (x, y) => x + ":" + y;
            var query = first.Zip(second, resultSelector);
            query.AssertSequenceEqual("a:5", "b:6", "c:7");
        }

        [Test]
        public void ShortSecond()
        {
            string[] first = { "a", "b", "c", "d", "e" };
            var second = Enumerable.Range(5, 3);
            Func<string, int, string> resultSelector = (x, y) => x + ":" + y;
            var query = first.Zip(second, resultSelector);
            query.AssertSequenceEqual("a:5", "b:6", "c:7");
        }

        [Test]
        public void EqualLengthSequences()
        {
            string[] first = { "a", "b", "c" };
            var second = Enumerable.Range(5, 3);
            Func<string, int, string> resultSelector = (x, y) => x + ":" + y;
            var query = first.Zip(second, resultSelector);
            query.AssertSequenceEqual("a:5", "b:6", "c:7");
        }

        [Test]
        public void AdjacentElements()
        {
            string[] elements = { "a", "b", "c", "d", "e" };
            var query = elements.Zip(elements.Skip(1), (x, y) => x + y);
            query.AssertSequenceEqual("ab", "bc", "cd", "de");
        }
    }
#endif
}
