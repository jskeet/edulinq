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
using Edulinq.Tests;
using NUnit.Framework;

namespace Edulinq.Tests
{
    /// <summary>
    /// Tests for query expressions which use multiple operators
    /// (and thus don't belong in a single operator's test case).
    /// </summary>
    [TestFixture]
    public class QueryExpressionTest
    {
        [Test]
        public void WhereAndSelect()
        {
            int[] source = { 1, 3, 4, 2, 8, 1 };
            var result = from x in source
                         where x < 4
                         select x * 2;
            result.AssertSequenceEqual(2, 6, 4, 2);
        }
    }
}
