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
    public class RepeatTest
    {
        [Test]
        public void SimpleRepeat()
        {
            Enumerable.Repeat("foo", 3).AssertSequenceEqual("foo", "foo", "foo");
        }

        [Test]
        public void EmptyRepeat()
        {
            Enumerable.Repeat("foo", 0).AssertSequenceEqual();
        }

        [Test]
        public void NullElement()
        {
            Enumerable.Repeat<string>(null, 2).AssertSequenceEqual(null, null);
        }

        [Test]
        public void NegativeCount()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Enumerable.Repeat("foo", -1));
        }
    }
}
