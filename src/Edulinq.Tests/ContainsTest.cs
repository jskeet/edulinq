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
    [TestFixture]
    public class ContainsTest
    {
        [Test]
        public void NullSourceNoComparer()
        {
            string[] source = null;
            Assert.Throws<ArgumentNullException>(() => source.Contains("x"));
        }

        [Test]
        public void NullSourceWithComparer()
        {
            string[] source = null;
            Assert.Throws<ArgumentNullException>(() => source.Contains("x", StringComparer.Ordinal));
        }

        [Test]
        public void NoMatchNoComparer()
        {
            // Default equality comparer is ordinal
            string[] source = { "foo", "bar", "baz" };
            Assert.IsFalse(source.Contains("BAR"));
        }

        [Test]
        public void MatchNoComparer()
        {
            // Default equality comparer is ordinal
            string[] source = { "foo", "bar", "baz" };
            // Clone the string to verify it's not just using reference identity
            string barClone = new String("bar".ToCharArray());
            Assert.IsTrue(source.Contains(barClone));
        }

        [Test]
        public void NoMatchNullComparer()
        {
            // Default equality comparer is ordinal
            string[] source = { "foo", "bar", "baz" };
            Assert.IsFalse(source.Contains("BAR", null));
        }

        [Test]
        public void MatchNullComparer()
        {
            // Default equality comparer is ordinal
            string[] source = { "foo", "bar", "baz" };
            // Clone the string to verify it's not just using reference identity
            string barClone = new String("bar".ToCharArray());
            Assert.IsTrue(source.Contains(barClone, null));
        }

        [Test]
        public void NoMatchWithCustomComparer()
        {
            // Default equality comparer is ordinal
            string[] source = { "foo", "bar", "baz" };
            Assert.IsFalse(source.Contains("gronk", StringComparer.OrdinalIgnoreCase));
        }

        [Test]
        public void MatchWithCustomComparer()
        {
            // Default equality comparer is ordinal
            string[] source = { "foo", "bar", "baz" };
            Assert.IsTrue(source.Contains("BAR", StringComparer.OrdinalIgnoreCase));
        }

        [Test]
        public void ImmediateReturnWhenMatchIsFound()
        {
            int[] source = { 10, 1, 5, 0 };
            var query = source.Select(x => 10 / x);
            // If we continued past 2, we'd see a division by zero exception
            Assert.IsTrue(query.Contains(2));
        }

#if !LINQBRIDGE
        /// <summary>
        /// I dislike this test. It tests for what I consider to be broken behaviour :(
        /// See the blog post on Contains for more information.
        /// </summary>
        [Test]
        public void SetWithDifferentComparer()
        {
            ICollection<string> sourceAsCollection = HashSetProvider.NewHashSet
                (StringComparer.OrdinalIgnoreCase, "foo", "bar", "baz");
            IEnumerable<string> sourceAsSequence = sourceAsCollection;
            Assert.IsTrue(sourceAsCollection.Contains("BAR"));
            Assert.IsTrue(sourceAsSequence.Contains("BAR")); // This is the line that concerns me
            Assert.IsFalse(sourceAsSequence.Contains("BAR", null));
            Assert.IsFalse(sourceAsSequence.Contains("BAR", StringComparer.Ordinal));
        }
#endif
    }
}
