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
using Edulinq.TestSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using NUnit.Framework;

namespace Edulinq.Tests
{
    [TestFixture]
    public class DistinctTest
    {
        // TestString1 and TestString2 are references to different but equal strings
        private static readonly string TestString1 = "test";
        private static readonly string TestString2 = new string(TestString1.ToCharArray());

        [Test]
        public void NullSourceNoComparer()
        {
            string[] source = null;
            Assert.Throws<ArgumentNullException>(() => source.Distinct());
        }

        [Test]
        public void NullSourceWithComparer()
        {
            string[] source = null;
            Assert.Throws<ArgumentNullException>(() => source.Distinct(StringComparer.Ordinal));
        }

        [Test]
        public void NullElementsArePassedToComparer()
        {
            IEqualityComparer<object> comparer = new SimpleEqualityComparer();
            Assert.Throws<NullReferenceException>(() => comparer.GetHashCode(null));
            Assert.Throws<NullReferenceException>(() => comparer.Equals(null, "xyz"));
            string[] source = { "xyz", null, "xyz", null, "abc" };
            var distinct = source.Distinct(comparer);
            Assert.Throws<NullReferenceException>(() => distinct.Count());
        }

        [Test]
        public void HashSetCopesWithNullElementsIfComparerDoes()
        {
            IEqualityComparer<string> comparer = EqualityComparer<string>.Default;
            Assert.AreEqual(comparer.GetHashCode(null), comparer.GetHashCode(null));
            Assert.IsTrue(comparer.Equals(null, null));
            string[] source = { "xyz", null, "xyz", null, "abc" };
            source.Distinct(comparer).AssertSequenceEqual("xyz", null, "abc");
        }

        [Test]
        public void NoComparerSpecifiedUsesDefault()
        {
            string[] source = { "xyz", TestString1, "XYZ", TestString2, "def" };
            source.Distinct().AssertSequenceEqual("xyz", TestString1, "XYZ", "def");
        }

        [Test]
        public void NullComparerUsesDefault()
        {
            string[] source = { "xyz", TestString1, "XYZ", TestString2, "def" };
            source.Distinct(null).AssertSequenceEqual("xyz", TestString1, "XYZ", "def");
        }

        [Test]
        public void DistinctStringsWithCaseInsensitiveComparer()
        {
            string[] source = { "xyz", TestString1, "XYZ", TestString2, "def" };
            source.Distinct(StringComparer.OrdinalIgnoreCase).AssertSequenceEqual("xyz", TestString1, "def");
        }

        [Test]
        public void DistinctStringsCustomComparer()
        {
            // This time we'll make sure that TestString1 and TestString2 are treated differently
            string[] source = { "xyz", TestString1, "XYZ", TestString2, TestString1 };
            source.Distinct(new ReferenceEqualityComparer())
                  .AssertSequenceEqual("xyz", TestString1, "XYZ", TestString2);
        }

        // Implementation of IEqualityComparer[T] which uses object identity
        private class ReferenceEqualityComparer : IEqualityComparer<object>
        {
            // Use explicit interface implementation to avoid warnings about hiding
            // the static object.Equals(object, object)
            bool IEqualityComparer<object>.Equals(object x, object y)
            {
                return object.ReferenceEquals(x, y);
            }

            public int GetHashCode(object obj)
            {
                return RuntimeHelpers.GetHashCode(obj);
            }
        }

        // Implementation of IEqualityComparer[T] which uses object's Equals/GetHashCode methods
        // in the simplest possible way, without any attempt to guard against NullReferenceException.
        private class SimpleEqualityComparer : IEqualityComparer<object>
        {
            // Use explicit interface implementation to avoid warnings about hiding
            // the static object.Equals(object, object)
            bool IEqualityComparer<object>.Equals(object x, object y)
            {
                return x.Equals(y);
            }

            public int GetHashCode(object obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}
