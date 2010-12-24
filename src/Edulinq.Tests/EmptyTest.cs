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
#if NORMAL_LINQ
using EmptyClass = System.Linq.Enumerable;
#else
using EmptyClass = Edulinq.Enumerable;
#endif
using NUnit.Framework;

namespace Edulinq.Tests
{
    [TestFixture]
    public class EmptyTest
    {
        [Test]
        public void EmptyContainsNoElements()
        {
            using (var empty = EmptyClass.Empty<int>().GetEnumerator())
            {
                Assert.IsFalse(empty.MoveNext());
            }
        }

        [Test]
        public void EmptyIsASingletonPerElementType()
        {
            Assert.AreSame(EmptyClass.Empty<int>(), EmptyClass.Empty<int>());
            Assert.AreSame(EmptyClass.Empty<long>(), EmptyClass.Empty<long>());
            Assert.AreSame(EmptyClass.Empty<string>(), EmptyClass.Empty<string>());
            Assert.AreSame(EmptyClass.Empty<object>(), EmptyClass.Empty<object>());

            Assert.AreNotSame(EmptyClass.Empty<long>(), EmptyClass.Empty<int>());
            Assert.AreNotSame(EmptyClass.Empty<string>(), EmptyClass.Empty<object>());
        }
    }
}
