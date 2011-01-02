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
using System.Linq;
using Edulinq.TestSupport;
using NUnit.Framework;

namespace Edulinq.Tests
{
    [TestFixture]
    public class SelectManyTest
    {
        // I'm bored of writing argument validation tests now.

        [Test]
        public void SimpleFlatten()
        {
            int[] numbers = { 3, 5, 20, 15 };
            // The ToCharArray is unnecessary really, as string implements IEnumerable<char>
            var query = numbers.SelectMany(x => x.ToInvariantString().ToCharArray());
            query.AssertSequenceEqual('3', '5', '2', '0', '1', '5');
        }

        [Test]
        public void SimpleFlattenWithIndex()
        {
            int[] numbers = { 3, 5, 20, 15 };
            // The ToCharArray is unnecessary really, as string implements IEnumerable<char>
            var query = numbers.SelectMany((x, index) => (x + index).ToInvariantString().ToCharArray());
            // 3 => '3'
            // 5 => '6'
            // 20 => '2', '2'
            // 15 => '1', '8'
            query.AssertSequenceEqual('3', '6', '2', '2', '1', '8');
        }

        [Test]
        public void FlattenWithProjection()
        {
            int[] numbers = { 3, 5, 20, 15 };
            // Flatten each number to its constituent characters, but then project each character
            // to a string of the original element which is responsible for "creating" that character,
            // as well as the character itself. So 20 will go to "20: 2" and "20: 0".
            var query = numbers.SelectMany(x => x.ToInvariantString().ToCharArray(),
                                           (x, c) => x + ": " + c);
            query.AssertSequenceEqual("3: 3", "5: 5", "20: 2", "20: 0", "15: 1", "15: 5");
        }

        [Test]
        public void FlattenWithProjectionAndIndex()
        {
            int[] numbers = { 3, 5, 20, 15 };
            var query = numbers.SelectMany((x, index) => (x + index).ToInvariantString().ToCharArray(),
                                           (x, c) => x + ": " + c);
            // 3 => "3: 3"
            // 5 => "5: 6"
            // 20 => "20: 2", "20: 2"
            // 15 => "15: 1", "15: 8"
            query.AssertSequenceEqual("3: 3", "5: 6", "20: 2", "20: 2", "15: 1", "15: 8");
        }
    }
}
