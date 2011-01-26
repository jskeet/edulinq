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
    public class GroupByTest
    {
        [Test]
        public void ExecutionIsPartiallyDeferred()
        {
            // No exception yet...
            new ThrowingEnumerable().GroupBy(x => x);
            // Note that for LINQ to Objects, calling GetEnumerator() starts iterating
            // over the input sequence, so we're not testing that...
        }

        [Test]
        public void SequenceIsReadFullyBeforeFirstResultReturned()
        {
            int[] numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
            // Final projection will throw
            var query = numbers.Select(x => 10 / x);

            var groups = query.GroupBy(x => x);
            // Either GetEnumerator or MoveNext will throw. See blog post for details.
            Assert.Throws<DivideByZeroException>(() =>
            {
                using (var iterator = groups.GetEnumerator())
                {
                    iterator.MoveNext();
                }
            });
        }

        [Test]
        public void SimpleGroupBy()
        {
            string[] source = { "abc", "hello", "def", "there", "four" };
            var groups = source.GroupBy(x => x.Length);

            var list = groups.ToList();
            Assert.AreEqual(3, list.Count);

            list[0].AssertSequenceEqual("abc", "def");
            Assert.AreEqual(3, list[0].Key);

            list[1].AssertSequenceEqual("hello", "there");
            Assert.AreEqual(5, list[1].Key);

            list[2].AssertSequenceEqual("four");
            Assert.AreEqual(4, list[2].Key);
        }

        [Test]
        public void GroupByWithElementProjection()
        {
            string[] source = { "abc", "hello", "def", "there", "four" };
            var groups = source.GroupBy(x => x.Length, x => x[0]);

            var list = groups.ToList();
            Assert.AreEqual(3, list.Count);

            list[0].AssertSequenceEqual('a', 'd');
            Assert.AreEqual(3, list[0].Key);

            list[1].AssertSequenceEqual('h', 't');
            Assert.AreEqual(5, list[1].Key);

            list[2].AssertSequenceEqual('f');
            Assert.AreEqual(4, list[2].Key);
        }

        [Test]
        public void GroupByWithCollectionProjection()
        {
            string[] source = { "abc", "hello", "def", "there", "four" };
            var groups = source.GroupBy(x => x.Length,
                                        (key, values) => key + ":" + StringEx.Join(";", values));

            groups.AssertSequenceEqual("3:abc;def", "5:hello;there", "4:four");
        }

        [Test]
        public void GroupByWithElementProjectionAndCollectionProjection()
        {
            string[] source = { "abc", "hello", "def", "there", "four" };
            // This time "values" will be an IEnumerable<char>, the first character of each
            // source string contributing to the group
            var groups = source.GroupBy(x => x.Length,
                                        x => x[0],
                                        (key, values) => key + ":" + StringEx.Join(";", values));

            groups.AssertSequenceEqual("3:a;d", "5:h;t", "4:f");
        }

        [Test]
        public void ChangesToSourceAreIgnoredInWhileIteratingOverResultsAfterFirstElementRetrieved()
        {
            var source = new List<string> { "a", "b", "c", "def" };

            var groups = source.GroupBy(x => x.Length);
            using (var iterator = groups.GetEnumerator())
            {
                Assert.IsTrue(iterator.MoveNext());
                iterator.Current.AssertSequenceEqual("a", "b", "c");

                // If GroupBy still needed to iterate over the source, this would cause a
                // InvalidOperationException when we next fetched an element from groups.
                source.Add("ghi");

                Assert.IsTrue(iterator.MoveNext());
                // ghi isn't in the group
                iterator.Current.AssertSequenceEqual("def");

                Assert.IsFalse(iterator.MoveNext());
            }

            // If we iterate again now - without calling GroupBy again - we'll see the difference:
            using (var iterator = groups.GetEnumerator())
            {
                Assert.IsTrue(iterator.MoveNext());
                iterator.Current.AssertSequenceEqual("a", "b", "c");

                Assert.IsTrue(iterator.MoveNext());
                iterator.Current.AssertSequenceEqual("def", "ghi");
            }
        }

        [Test]
        public void NullKeys()
        {
            string[] source = { "first", "null", "nothing", "second" };
            // This time "values" will be an IEnumerable<char>, the first character of each
            // source string contributing to the group
            var groups = source.GroupBy(x => x.StartsWith("n") ? null : x,
                                        (key, values) => key + ":" + StringEx.Join(";", values));

            groups.AssertSequenceEqual("first:first", ":null;nothing", "second:second");
        }
    }
}
