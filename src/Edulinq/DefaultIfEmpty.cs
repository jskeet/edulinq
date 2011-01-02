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

namespace Edulinq
{
    public static partial class Enumerable
    {
        public static IEnumerable<TSource> DefaultIfEmpty<TSource>(
	        this IEnumerable<TSource> source)
        {
            // This will perform an appropriate test for source being null first.
            return source.DefaultIfEmpty(default(TSource));
        }

        public static IEnumerable<TSource> DefaultIfEmpty<TSource>(
	        this IEnumerable<TSource> source,
	        TSource defaultValue)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            return DefaultIfEmptyImpl(source, defaultValue);
        }

        private static IEnumerable<TSource> DefaultIfEmptyImpl<TSource>(
	        IEnumerable<TSource> source,
	        TSource defaultValue)
        {
            bool foundAny = false;
            foreach (TSource item in source)
            {
                yield return item;
                foundAny = true;
            }
            if (!foundAny)
            {
                yield return defaultValue;
            }
        }
    }
}
