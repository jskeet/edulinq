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
    public class GroupJoinTest
    {
        [Test]
        public void ExecutionIsDeferred()
        {
            var outer = new ThrowingEnumerable();
            var inner = new ThrowingEnumerable();
            outer.GroupJoin(inner, x => x, y => y, (x, y) => x + y.Count());
        }

        [Test]
        public void SimpleGroupJoin()
        {
            // We're going to join on the first character in the outer sequence item
            // being equal to the second character in the inner sequence item
            string[] outer = { "first", "second", "third" };
            string[] inner = { "essence", "offer", "eating", "psalm" };

            var query = outer.GroupJoin(inner,
                                   outerElement => outerElement[0],
                                   innerElement => innerElement[1],
                                   (outerElement, innerElements) => outerElement + ":" + StringEx.Join(";", innerElements));

            query.AssertSequenceEqual("first:offer", "second:essence;psalm", "third:");
        }

        [Test]
        public void CustomComparer()
        {
            // We're going to match the start of the outer sequence item
            // with the end of the inner sequence item, in a case-insensitive manner
            string[] outer = { "ABCxxx", "abcyyy", "defzzz", "ghizzz" };
            string[] inner = { "000abc", "111gHi", "222333", "333AbC" };

            var query = outer.GroupJoin(inner,
                                   outerElement => outerElement.Substring(0, 3),
                                   innerElement => innerElement.Substring(3),
                                   (outerElement, innerElements) => outerElement + ":" + StringEx.Join(";", innerElements),
                                   StringComparer.OrdinalIgnoreCase);
            // ABCxxx matches 000abc and 333AbC
            // abcyyy matches 000abc and 333AbC
            // defzzz doesn't match anything
            // ghizzz matches 111gHi
            query.AssertSequenceEqual("ABCxxx:000abc;333AbC", "abcyyy:000abc;333AbC", "defzzz:", "ghizzz:111gHi");
        }

        [Test]
        public void DifferentSourceTypes()
        {
            int[] outer = { 5, 3, 7, 4 };
            string[] inner = { "bee", "giraffe", "tiger", "badger", "ox", "cat", "dog" };

            var query = outer.GroupJoin(inner,
                                   outerElement => outerElement,
                                   innerElement => innerElement.Length,
                                   (outerElement, innerElements) => outerElement + ":" + StringEx.Join(";", innerElements));
            query.AssertSequenceEqual("5:tiger", "3:bee;cat;dog", "7:giraffe", "4:");
        }

        // Note that LINQ to Objects ignores null keys for Join and GroupJoin
        [Test]
        public void NullKeys()
        {
            string[] outer = { "first", null, "second" };
            string[] inner = { "first", "null", "nothing" };
            var query = outer.GroupJoin(inner,
                                   outerElement => outerElement,
                                   innerElement => innerElement.StartsWith("n") ? null : innerElement,
                                   (outerElement, innerElements) => outerElement + ":" + StringEx.Join(";", innerElements));
            // No matches for the null outer key
            query.AssertSequenceEqual("first:first", ":", "second:");
        }
    }
}
