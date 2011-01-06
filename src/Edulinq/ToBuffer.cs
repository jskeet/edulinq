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

namespace Edulinq
{
    public static partial class Enumerable
    {
        internal static TSource[] ToBuffer<TSource>(this IEnumerable<TSource> source, out int count)
        {
            // Optimize for ICollection<T>
            ICollection<TSource> collection = source as ICollection<TSource>;
            if (collection != null)
            {
                count = collection.Count;
                TSource[] tmp = new TSource[count];
                collection.CopyTo(tmp, 0);
                return tmp;
            }

            // We'll have to loop through, creating and copying arrays as we go
            TSource[] ret = new TSource[16];
            int tmpCount = 0;
            foreach (TSource item in source)
            {
                // Need to expand...
                if (tmpCount == ret.Length)
                {
                    Array.Resize(ref ret, ret.Length * 2);
                }
                ret[tmpCount++] = item;
            }
            count = tmpCount;
            return ret;
        }
    }
}
