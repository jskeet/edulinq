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
    public class ToLookupTest
    {
        [Test]
        public void SourceSequenceIsReadEagerly()
        {
            var source = new ThrowingEnumerable();
            Assert.Throws<InvalidOperationException>(() => source.ToLookup(x => x));
        }

        [Test]
        public void ChangesToSourceSequenceAfterToLookupAreNotNoticed()
        {
            List<string> source = new List<string> { "abc" };
            var lookup = source.ToLookup(x => x.Length);
            Assert.AreEqual(1, lookup.Count);

            // Potential new key is ignored
            source.Add("x");
            Assert.AreEqual(1, lookup.Count);

            // Potential new value for existing key is ignored
            source.Add("xyz");
            lookup[3].AssertSequenceEqual("abc");
        }

        [Test]
        public void LookupWithNoComparerOrElementSelector()
        {
            string[] source = { "abc", "def", "x", "y", "ghi", "z", "00" };
            var lookup = source.ToLookup(value => value.Length);
            lookup[3].AssertSequenceEqual("abc", "def", "ghi");
            lookup[1].AssertSequenceEqual("x", "y", "z");
            lookup[2].AssertSequenceEqual("00");

            Assert.AreEqual(3, lookup.Count);

            // An unknown key returns an empty sequence
            lookup[100].AssertSequenceEqual();
        }

        [Test]
        public void LookupWithComparerButNoElementSelector()
        {
            string[] source = { "abc", "def", "ABC" };
            var lookup = source.ToLookup(value => value, StringComparer.OrdinalIgnoreCase);
            lookup["abc"].AssertSequenceEqual("abc", "ABC");
            lookup["def"].AssertSequenceEqual("def");
        }

        [Test]
        public void LookupWithNullComparerButNoElementSelector()
        {
            string[] source = { "abc", "def", "ABC" };
            var lookup = source.ToLookup(value => value, null);
            lookup["abc"].AssertSequenceEqual("abc");
            lookup["ABC"].AssertSequenceEqual("ABC");
            lookup["def"].AssertSequenceEqual("def");
        }

        [Test]
        public void LookupWithElementSelectorButNoComparer()
        {
            string[] source = { "abc", "def", "x", "y", "ghi", "z", "00" };
            // Use the length as the key selector, and the first character as the element
            var lookup = source.ToLookup(value => value.Length, value => value[0]);
            lookup[3].AssertSequenceEqual('a', 'd', 'g');
            lookup[1].AssertSequenceEqual('x', 'y', 'z');
            lookup[2].AssertSequenceEqual('0');
        }

        [Test]
        public void LookupWithComparareAndElementSelector()
        {
            var people = new[] {
                new { First = "Jon", Last = "Skeet" },
                new { First = "Tom", Last = "SKEET" }, // Note upper-cased name
                new { First = "Juni", Last = "Cortez" },
                new { First = "Holly", Last = "Skeet" },
                new { First = "Abbey", Last = "Bartlet" },
                new { First = "Carmen", Last = "Cortez" },                
                new { First = "Jed", Last = "Bartlet" }
            };

            var lookup = people.ToLookup(p => p.Last, p => p.First, StringComparer.OrdinalIgnoreCase);

            lookup["Skeet"].AssertSequenceEqual("Jon", "Tom", "Holly");
            lookup["Cortez"].AssertSequenceEqual("Juni", "Carmen");
            // The key comparer is used for lookups too
            lookup["BARTLET"].AssertSequenceEqual("Abbey", "Jed");

            lookup.Select(x => x.Key).AssertSequenceEqual("Skeet", "Cortez", "Bartlet");
        }

        [Test]
        public void FindByNullKeyNonePresent()
        {
            string[] source = { "first", "second" };
            var lookup = source.ToLookup(x => x);
            lookup[null].AssertSequenceEqual();
        }

        [Test]
        public void FindByNullKeyWhenPresent()
        {
            string[] source = { "first", "null", "nothing", "second" };
            var lookup = source.ToLookup(x => x.StartsWith("n") ? null : x);
            lookup[null].AssertSequenceEqual("null", "nothing");
            lookup.Select(x => x.Key).AssertSequenceEqual("first", null, "second");
            Assert.AreEqual(3, lookup.Count);
            lookup[null].AssertSequenceEqual("null", "nothing");
        }
    }
}
