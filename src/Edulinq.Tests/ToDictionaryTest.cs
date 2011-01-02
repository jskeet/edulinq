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
    public class ToDictionaryTest
    {
        [Test]
        public void NullSourceNoComparerNoElementSelector()
        {
            string[] source = null;
            Assert.Throws<ArgumentNullException>(() => source.ToDictionary(x => x));
        }

        [Test]
        public void NullKeySelectorNoComparerNoElementSelector()
        {
            string[] source = { };
            Func<string, string> keySelector = null;
            Assert.Throws<ArgumentNullException>(() => source.ToDictionary(keySelector));
        }

        [Test]
        public void NullSourceWithComparerNoElementSelector()
        {
            string[] source = null;
            Assert.Throws<ArgumentNullException>(() => source.ToDictionary(x => x, StringComparer.OrdinalIgnoreCase));
        }

        [Test]
        public void NullKeySelectorWithComparerNoElementSelector()
        {
            string[] source = { };
            Func<string, string> keySelector = null;
            Assert.Throws<ArgumentNullException>(() => source.ToDictionary(keySelector, StringComparer.OrdinalIgnoreCase));
        }

        [Test]
        public void NullSourceNoComparerWithElementSelector()
        {
            string[] source = null;
            Assert.Throws<ArgumentNullException>(() => source.ToDictionary(x => x, x => x));
        }

        [Test]
        public void NullKeySelectorNoComparerWithElementSelector()
        {
            string[] source = { };
            Func<string, string> keySelector = null;
            Assert.Throws<ArgumentNullException>(() => source.ToDictionary(keySelector, x => x));
        }

        [Test]
        public void NullElementSelectorNoComparer()
        {
            string[] source = { };
            Func<string, string> elementSelector = null;
            Assert.Throws<ArgumentNullException>(() => source.ToDictionary(x => x, elementSelector));
        }

        [Test]
        public void NullSourceWithComparerWithElementSelector()
        {
            string[] source = null;
            Assert.Throws<ArgumentNullException>(() => source.ToDictionary(x => x, x => x, StringComparer.OrdinalIgnoreCase));
        }

        [Test]
        public void NullKeySelectorWithComparerWithElementSelector()
        {
            string[] source = { };
            Func<string, string> keySelector = null;
            Assert.Throws<ArgumentNullException>(() => source.ToDictionary(keySelector, x => x, StringComparer.OrdinalIgnoreCase));
        }

        [Test]
        public void NullElementSelectorWithComparer()
        {
            string[] source = { };
            Func<string, string> elementSelector = null;
            Assert.Throws<ArgumentNullException>(() => source.ToDictionary(x => x, elementSelector, StringComparer.OrdinalIgnoreCase));
        }

        [Test]
        public void JustKeySelector()
        {
            string[] source = { "zero", "one", "two" };
            var result = source.ToDictionary(x => x[0]);

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual("zero", result['z']);
            Assert.AreEqual("one", result['o']);
            Assert.AreEqual("two", result['t']);
        }

        [Test]
        public void KeyAndElementSelector()
        {
            // Map the first character of each string to the string's length
            string[] source = { "zero", "one", "two" };
            var result = source.ToDictionary(x => x[0], x => x.Length);

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(4, result['z']); // Length of "zero"
            Assert.AreEqual(3, result['o']); // Length of "one"
            Assert.AreEqual(3, result['t']); // Length of "two"
        }

        [Test]
        public void CustomEqualityComparer()
        {
            // Map the first character of each string (*as* a string) to the string's length,
            // using a case-insensitive comparer
            string[] source = { "zero", "One", "Two" };
            var result = source.ToDictionary(x => x.Substring(0, 1),
                                             x => x.Length,
                                             StringComparer.OrdinalIgnoreCase);

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(4, result["z"]); // Length of "zero"
            // These two rely on the dictionary being case-insensitive
            Assert.AreEqual(3, result["o"]); // Length of "one"
            Assert.AreEqual(3, result["t"]); // Length of "two"            
        }

        [Test]
        public void DuplicateKey()
        {
            // Oh no! "Two" and "three" start with the same letter (case-insensitively)
            string[] source = { "zero", "One", "Two", "three" };

            Assert.Throws<ArgumentException>(() =>
                source.ToDictionary(x => x.Substring(0, 1), StringComparer.OrdinalIgnoreCase));
        }

        [Test]
        public void NullEntryKeyCausesException()
        {
            string[] source = { "a", "b", null };
            Assert.Throws<ArgumentNullException>(() => source.ToDictionary(x => x));
        }

        [Test]
        public void NullEntryValueIsAllowed()
        {
            string[] source = { "a", "b" };
            var result = source.ToDictionary(x => x, x => (string) null);
            Assert.AreEqual(2, result.Count);
            Assert.IsNull(result["a"]);
            Assert.IsNull(result["b"]);
        }

        [Test]
        public void NullComparerMeansDefault()
        {
            // The default string comparer is case-sensitive
            string[] source = { "zero", "One", "Two", "three" };

            var result = source.ToDictionary(x => x.Substring(0, 1), null);
            Assert.AreEqual(4, result.Count);
            Assert.AreEqual("Two", result["T"]);
            Assert.AreEqual("three", result["t"]);
        }
    }
}
