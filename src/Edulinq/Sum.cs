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
        #region Int32
        public static int Sum(this IEnumerable<int> source)
        {
            return Sum(source, x => x);
        }

        public static int? Sum(this IEnumerable<int?> source)
        {
            return Sum(source, x => x);
        }

        public static int Sum<T>(
            this IEnumerable<T> source,
            Func<T, int> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (selector == null)
            {
                throw new ArgumentNullException("selector");
            }
            checked
            {
                int sum = 0;
                foreach (T item in source)
                {
                    sum += selector(item);
                }
                return sum;
            }
        }

        public static int? Sum<T>(
            this IEnumerable<T> source,
            Func<T, int?> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (selector == null)
            {
                throw new ArgumentNullException("selector");
            }
            checked
            {
                int sum = 0;
                foreach (T item in source)
                {
                    sum += selector(item).GetValueOrDefault();
                }
                return sum;
            }
        }
        #endregion Int32

        #region Int64
        public static long Sum(this IEnumerable<long> source)
        {
            return Sum(source, x => x);
        }

        public static long? Sum(this IEnumerable<long?> source)
        {
            return Sum(source, x => x);
        }

        public static long Sum<T>(
            this IEnumerable<T> source,
            Func<T, long> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (selector == null)
            {
                throw new ArgumentNullException("selector");
            }
            checked
            {
                long sum = 0;
                foreach (T item in source)
                {
                    sum += selector(item);
                }
                return sum;
            }
        }

        public static long? Sum<T>(
            this IEnumerable<T> source,
            Func<T, long?> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (selector == null)
            {
                throw new ArgumentNullException("selector");
            }
            checked
            {
                long sum = 0;
                foreach (T item in source)
                {
                    sum += selector(item).GetValueOrDefault();
                }
                return sum;
            }
        }
        #endregion Int64

        #region Decimal
        public static decimal Sum(this IEnumerable<decimal> source)
        {
            return Sum(source, x => x);
        }

        public static decimal? Sum(this IEnumerable<decimal?> source)
        {
            return Sum(source, x => x);
        }

        public static decimal Sum<T>(
            this IEnumerable<T> source,
            Func<T, decimal> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (selector == null)
            {
                throw new ArgumentNullException("selector");
            }
            // No need for checked here: decimal operations are always checked.
            decimal sum = 0;
            foreach (T item in source)
            {
                sum += selector(item);
            }
            return sum;
        }

        public static decimal? Sum<T>(
            this IEnumerable<T> source,
            Func<T, decimal?> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (selector == null)
            {
                throw new ArgumentNullException("selector");
            }
            // No need for checked here: decimal operations are always checked.
            decimal sum = 0;
            foreach (T item in source)
            {
                sum += selector(item).GetValueOrDefault();
            }
            return sum;
        }
        #endregion Decimal

        #region Single
        public static float Sum(this IEnumerable<float> source)
        {
            return Sum(source, x => x);
        }

        public static float? Sum(this IEnumerable<float?> source)
        {
            return Sum(source, x => x);
        }

        public static float Sum<T>(
            this IEnumerable<T> source,
            Func<T, float> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (selector == null)
            {
                throw new ArgumentNullException("selector");
            }
            double sum = 0;
            foreach (T item in source)
            {
                sum += selector(item);
            }
            return (float) sum;
        }

        public static float? Sum<T>(
            this IEnumerable<T> source,
            Func<T, float?> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (selector == null)
            {
                throw new ArgumentNullException("selector");
            }
            double sum = 0;
            foreach (T item in source)
            {
                sum += selector(item).GetValueOrDefault();
            }
            return (float) sum;
        }

        #endregion Single

        #region Double
        public static double Sum(this IEnumerable<double> source)
        {
            return Sum(source, x => x);
        }

        public static double? Sum(this IEnumerable<double?> source)
        {
            return Sum(source, x => x);
        }

        public static double Sum<T>(
            this IEnumerable<T> source,
            Func<T, double> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (selector == null)
            {
                throw new ArgumentNullException("selector");
            }
            double sum = 0;
            foreach (T item in source)
            {
                sum += selector(item);
            }
            return sum;
        }

        public static double? Sum<T>(
            this IEnumerable<T> source,
            Func<T, double?> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (selector == null)
            {
                throw new ArgumentNullException("selector");
            }
            double sum = 0;
            foreach (T item in source)
            {
                sum += selector(item).GetValueOrDefault();
            }
            return sum;
        }
        #endregion Double
    }
}
