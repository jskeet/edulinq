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

namespace Edulinq.TestSupport
{
    /// <summary>
    /// Testing against LinqBridge, we can't use StringEx.Join(string, IEnumerable[T]) because it's
    /// in .NET 4. This is as simple an equivalent as we can easily achieve.
    /// </summary>
    public static class StringEx
    {
        public static string Join<T>(string delimiter, IEnumerable<T> source)
        {
            return string.Join(delimiter, source.Select(x => x.ToString()).ToArray());
        }
    }
}
