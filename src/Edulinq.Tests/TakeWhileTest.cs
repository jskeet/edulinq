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
    public class TakeWhileTest
    {
        [Test]
        public void ExecutionIsDeferred()
        {
            new ThrowingEnumerable().TakeWhile(x => x > 10);
        }

        [Test]
        public void NullSourceNoIndex()
        {
            int[] source = null;
            Assert.Throws<ArgumentNullException>(() => source.TakeWhile(x => x > 10));
        }

        [Test]
        public void NullSourceUsingIndex()
        {
            int[] source = null;
            Assert.Throws<ArgumentNullException>(() => source.TakeWhile((x, index) => x > 10));
        }

        [Test]
        public void NullPredicateNoIndex()
        {
            int[] source = { 1, 2 };
            Func<int, bool> predicate = null;
            Assert.Throws<ArgumentNullException>(() => source.TakeWhile(predicate));
        }

        [Test]
        public void NullPredicateUsingIndex()
        {
            int[] source = { 1, 2 };
            Func<int, int, bool> predicate = null;
            Assert.Throws<ArgumentNullException>(() => source.TakeWhile(predicate));
        }

        [Test]
        public void PredicateFailingFirstElement()
        {
            string[] source = { "zero", "one", "two", "three", "four", "five", "six" };
            source.TakeWhile(x => x.Length > 4).AssertSequenceEqual();
        }

        [Test]
        public void PredicateWithIndexFailingFirstElement()
        {
            string[] source = { "zero", "one", "two", "three", "four", "five" };
            source.TakeWhile((x, index) => index + x.Length > 4).AssertSequenceEqual();
        }

        [Test]
        public void PredicateMatchingSomeElements()
        {
            string[] source = { "zero", "one", "two", "three", "four", "five" };
            source.TakeWhile(x => x.Length < 5).AssertSequenceEqual("zero", "one", "two");
        }

        [Test]
        public void PredicateWithIndexMatchingSomeElements()
        {
            string[] source = { "zero", "one", "two", "three", "four", "five" };
            source.TakeWhile((x, index) => x.Length > index).AssertSequenceEqual("zero", "one", "two", "three");
        }

        [Test]
        public void PredicateMatchingAllElements()
        {
            string[] source = { "zero", "one", "two", "three", "four", "five" };
            source.TakeWhile(x => x.Length < 100).AssertSequenceEqual("zero", "one", "two", "three", "four", "five");
        }

        [Test]
        public void PredicateWithIndexMatchingAllElements()
        {
            string[] source = { "zero", "one", "two", "three", "four", "five" };
            source.TakeWhile((x, index) => x.Length < 100).AssertSequenceEqual("zero", "one", "two", "three", "four", "five");
        }
    }
}
