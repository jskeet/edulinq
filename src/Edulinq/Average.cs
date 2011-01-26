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
        #region Int32
        public static double Average(this IEnumerable<int> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            checked
            {
                long count = 0;
                long total = 0;
                foreach (int item in source)
                {
                    total += item;
                    count++;
                }
                if (count == 0)
                {
                    throw new InvalidOperationException("Sequence was empty");
                }
                return (double)total / (double)count;
            }
        }

        public static double Average<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, int> selector)
        {
            return source.Select(selector).Average();
        }

        public static double? Average(this IEnumerable<int?> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            checked
            {
                long count = 0;
                long total = 0;
                foreach (int? item in source)
                {
                    if (item != null)
                    {
                        count++;
                        total += item.Value;
                    }
                }
                return count == 0 ? (double?)null : (double)total / (double)count;
            }
        }

        public static double? Average<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, int?> selector)
        {
            return source.Select(selector).Average();
        }
        #endregion

        #region Int64
        public static double Average(this IEnumerable<long> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            checked
            {
                long count = 0;
                long total = 0;
                foreach (long item in source)
                {
                    total += item;
                    count++;
                }
                if (count == 0)
                {
                    throw new InvalidOperationException("Sequence was empty");
                }
                return (double)total / (double)count;
            }
        }

        public static double Average<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, long> selector)
        {
            return source.Select(selector).Average();
        }

        public static double? Average(this IEnumerable<long?> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            checked
            {
                long count = 0;
                long total = 0;
                foreach (long? item in source)
                {
                    if (item != null)
                    {
                        count++;
                        total += item.Value;
                    }
                }
                return count == 0 ? (double?)null : (double)total / (double)count;
            }
        }

        public static double? Average<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, long?> selector)
        {
            return source.Select(selector).Average();
        }
        #endregion

        #region Double
        public static double Average(this IEnumerable<double> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            long count = 0;
            double total = 0;
            foreach (double item in source)
            {
                total += item;
                count++;
            }
            if (count == 0)
            {
                throw new InvalidOperationException("Sequence was empty");
            }
            return total / (double)count;
        }

        public static double Average<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, double> selector)
        {
            return source.Select(selector).Average();
        }

        public static double? Average(this IEnumerable<double?> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            long count = 0;
            double total = 0;
            foreach (double? item in source)
            {
                if (item != null)
                {
                    count++;
                    total += item.Value;
                }
            }
            return count == 0 ? (double?)null : total / (double)count;
        }

        public static double? Average<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, double?> selector)
        {
            return source.Select(selector).Average();
        }
        #endregion

        #region Single
        public static float Average(this IEnumerable<float> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            long count = 0;
            double total = 0;
            foreach (float item in source)
            {
                total += item;
                count++;
            }
            if (count == 0)
            {
                throw new InvalidOperationException("Sequence was empty");
            }
            return (float)(total / (double)count);
        }

        public static float Average<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, float> selector)
        {
            return source.Select(selector).Average();
        }

        public static float? Average(this IEnumerable<float?> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            long count = 0;
            double total = 0;
            foreach (float? item in source)
            {
                if (item != null)
                {
                    count++;
                    total += item.Value;
                }
            }
            return count == 0 ? (float?)null : (float)(total / (double)count);
        }

        public static double? Average<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, float?> selector)
        {
            return source.Select(selector).Average();
        }
        #endregion

        #region Decimal
        public static decimal Average(this IEnumerable<decimal> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            // Decimal operations are always checked
            long count = 0;
            decimal total = 0;
            foreach (decimal item in source)
            {
                total += item;
                count++;
            }
            if (count == 0)
            {
                throw new InvalidOperationException("Sequence was empty");
            }
            return total / (decimal)count;
        }

        public static decimal Average<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, decimal> selector)
        {
            return source.Select(selector).Average();
        }

        public static decimal? Average(this IEnumerable<decimal?> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            long count = 0;
            decimal total = 0;
            foreach (decimal? item in source)
            {
                if (item != null)
                {
                    count++;
                    total += item.Value;
                }
            }
            return count == 0 ? (decimal?)null : total / (decimal)count;
        }

        public static decimal? Average<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, decimal?> selector)
        {
            return source.Select(selector).Average();
        }
        #endregion
    }
}
