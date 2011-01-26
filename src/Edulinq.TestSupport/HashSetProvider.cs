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

namespace Edulinq.TestSupport
{
    // LinqBridge doesn't provide HashSet<T>; tests are conditionalized too
#if !LINQBRIDGE
    public static class HashSetProvider
    {
        /// <summary>
        /// Allows a HashSet to be created in test classes, even though the Edulinq
        /// configuration of the tests project doesn't have a reference to System.Core.
        /// Basically it's a grotty hack.
        /// </summary>
        public static ICollection<T> NewHashSet<T>(IEqualityComparer<T> comparer, params T[] items)
        {
            return new HashSet<T>(items, comparer);
        }
    }
#endif
}
