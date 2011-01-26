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
using NUnit.Framework;

namespace Edulinq.TestSupport
{
    /// <summary>
    /// This is originally from MoreLINQ:
    /// http://morelinq.googlecode.com
    /// 
    /// It may end up being expanded a bit though :)
    /// </summary>
    public static class TestExtensions
    {
        /// <summary>
        /// Make testing even easier - a params array makes for readable tests :)
        /// The sequence is evaluated exactly once.
        /// </summary>
        public static void AssertSequenceEqual<T>(this IEnumerable<T> actual, params T[] expected)
        {
            // Working with a copy means we can look over it more than once.
            // We're safe to do that with the array anyway.
            List<T> copy = new List<T>(actual);
            Assert.AreEqual(expected.Length, copy.Count, "Expected counts to be equal");

            for (int i = 0; i < copy.Count; i++)
            {
                if (!EqualityComparer<T>.Default.Equals(expected[i], copy[i]))
                {
                    Assert.Fail("Expected sequences differ at index " + i + ": expected " + expected[i]
                        + "; was " + copy[i]);
                }
            }
        }
    }
}