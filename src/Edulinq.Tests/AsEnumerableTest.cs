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
using System.Collections.Generic;
using System.Linq;
using Edulinq.TestSupport;
using NUnit.Framework;

namespace Edulinq.Tests
{
#if !LINQBRIDGE // In the build I've got, AsEnumerable isn't an extension method
    [TestFixture]
    public class AsEnumerableTest
    {
        [Test]
        public void NullSourceIsPermitted()
        {
            IEnumerable<string> source = null;
            Assert.IsNull(source.AsEnumerable());
        }

        [Test]
        public void DoesNotCallGetEnumerator()
        {
            var source = new ThrowingEnumerable();
            Assert.AreSame(source, source.AsEnumerable());
        }

        [Test]
        public void NormalSequence()
        {
            var range = Enumerable.Range(0, 10);
            Assert.AreSame(range, range.AsEnumerable());
        }

        [Test]
        public void AnonymousType()
        {
            var list = new[] { 
                new { FirstName = "Jon", Surname = "Skeet" },
                new { FirstName = "Holly", Surname = "Skeet" }
            }.ToList();

            // We can't cast to IEnumerable<T> as we can't express T.
            var sequence = list.AsEnumerable();
            // This will now use Enumerable.Contains instead of List.Contains
            Assert.IsFalse(sequence.Contains(new { FirstName = "Tom", Surname = "Skeet" }));
        }
    }
#endif
}
