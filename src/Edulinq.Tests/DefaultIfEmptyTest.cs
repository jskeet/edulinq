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
using System.Linq;
using NUnit.Framework;

namespace Edulinq.Tests
{
    [TestFixture]
    public class DefaultIfEmptyTest
    {
        [Test]
        public void NullSourceNoDefaultValue()
        {
            int[] source = null;
            Assert.Throws<ArgumentNullException>(() => source.DefaultIfEmpty());
        }

        [Test]
        public void NullSourceWithDefaultValue()
        {
            int[] source = null;
            Assert.Throws<ArgumentNullException>(() => source.DefaultIfEmpty(5));
        }

        [Test]
        public void EmptySequenceNoDefaultValue()
        {
            Enumerable.Empty<int>().DefaultIfEmpty().AssertSequenceEqual(0);
        }

        [Test]
        public void EmptySequenceWithDefaultValue()
        {
            Enumerable.Empty<int>().DefaultIfEmpty(5).AssertSequenceEqual(5);
        }

        [Test]
        public void NonEmptySequenceNoDefaultValue()
        {
            int[] source = { 3, 1, 4 };
            source.DefaultIfEmpty().AssertSequenceEqual(source);
        }

        [Test]
        public void NonEmptySequenceWithDefaultValue()
        {
            int[] source = { 3, 1, 4 };
            source.DefaultIfEmpty(5).AssertSequenceEqual(source);
        }
    }
}
