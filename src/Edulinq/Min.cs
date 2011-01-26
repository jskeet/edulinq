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
        public static int Min(this IEnumerable<int> source)
        {
            return PrimitiveMin(source);
        }

        public static int Min<TSource>(
	        this IEnumerable<TSource> source,
	        Func<TSource, int> selector)
        {
            // Select will validate the arguments
            return PrimitiveMin(source.Select(selector));
        }

        public static int? Min(this IEnumerable<int?> source)
        {
            return NullablePrimitiveMin(source);
        }

        public static int? Min<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, int?> selector)
        {
            // Select will validate the arguments
            return NullablePrimitiveMin(source.Select(selector));
        }
        #endregion

        #region Int64
        public static long Min(this IEnumerable<long> source)
        {
            return PrimitiveMin(source);
        }

        public static long Min<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, long> selector)
        {
            // Select will validate the arguments
            return PrimitiveMin(source.Select(selector));
        }

        public static long? Min(this IEnumerable<long?> source)
        {
            return NullablePrimitiveMin(source);
        }

        public static long? Min<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, long?> selector)
        {
            // Select will validate the arguments
            return NullablePrimitiveMin(source.Select(selector));
        }
        #endregion

        #region Double
        public static double Min(this IEnumerable<double> source)
        {
            return PrimitiveMin(source);
        }

        public static double Min<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, double> selector)
        {
            // Select will validate the arguments
            return PrimitiveMin(source.Select(selector));
        }

        public static double? Min(this IEnumerable<double?> source)
        {
            return NullablePrimitiveMin(source);
        }

        public static double? Min<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, double?> selector)
        {
            // Select will validate the arguments
            return NullablePrimitiveMin(source.Select(selector));
        }
        #endregion

        #region Single
        public static float Min(this IEnumerable<float> source)
        {
            return PrimitiveMin(source);
        }

        public static float Min<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, float> selector)
        {
            // Select will validate the arguments
            return PrimitiveMin(source.Select(selector));
        }

        public static float? Min(this IEnumerable<float?> source)
        {
            return NullablePrimitiveMin(source);
        }

        public static float? Min<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, float?> selector)
        {
            // Select will validate the arguments
            return NullablePrimitiveMin(source.Select(selector));
        }
        #endregion

        #region Decimal
        public static decimal Min(this IEnumerable<decimal> source)
        {
            return PrimitiveMin(source);
        }

        public static decimal Min<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, decimal> selector)
        {
            // Select will validate the arguments
            return PrimitiveMin(source.Select(selector));
        }

        public static decimal? Min(this IEnumerable<decimal?> source)
        {
            return NullablePrimitiveMin(source);
        }

        public static decimal? Min<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, decimal?> selector)
        {
            // Select will validate the arguments
            return NullablePrimitiveMin(source.Select(selector));
        }
        #endregion

        #region Generic methods
        public static TSource Min<TSource>(
            this IEnumerable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            // This condition will be true for reference types and nullable value types, and false for
            // non-nullable value types.
            return default(TSource) == null ? NullableGenericMin(source) : NonNullableGenericMin(source);
        }

        public static TResult Min<TSource, TResult>(
            this IEnumerable<TSource> source,
            Func<TSource, TResult> selector)
        {
            return Min(source.Select(selector));
        }

        /// <summary>
        /// Implements the generic behaviour for non-nullable value types.
        /// </summary>
        /// <remarks>
        /// Empty sequences will cause an InvalidOperationException to be thrown.
        /// Note that there's no *compile-time* validation in the caller that the type
        /// is a non-nullable value type, hence the lack of a constraint on T.
        /// </remarks>
        private static T NonNullableGenericMin<T>(IEnumerable<T> source)
        {
            Comparer<T> comparer = Comparer<T>.Default;

            using (IEnumerator<T> iterator = source.GetEnumerator())
            {
                if (!iterator.MoveNext())
                {
                    throw new InvalidOperationException("Sequence was empty");
                }
                T max = iterator.Current;
                while (iterator.MoveNext())
                {
                    T item = iterator.Current;
                    if (comparer.Compare(max, item) > 0)
                    {
                        max = item;
                    }
                }
                return max;
            }
        }

        /// <summary>
        /// Implements the generic behaviour for nullable types - both reference types and nullable
        /// value types.
        /// </summary>
        /// <remarks>
        /// Empty sequences and sequences comprising only of null values will cause the null value
        /// to be returned. Any sequence containing non-null values will return a non-null value.
        /// </remarks>
        private static T NullableGenericMin<T>(IEnumerable<T> source)
        {
            Comparer<T> comparer = Comparer<T>.Default;

            T max = default(T);
            foreach (T item in source)
            {
                if (item != null &&
                    (max == null || comparer.Compare(max, item) > 0))
                {
                    max = item;
                }
            }
            return max;
        }

        #endregion

        #region "Primitive" implementations
        // These are uses by all the overloads which use a known numeric type.
        // The term "primitive" isn't truly accurate here as decimal is not a primitive
        // type, but it captures the aim reasonably well.
        // The constraint of being a value type isn't really required, because we don't rely on
        // it within the method and only code which already knows it's a comparable value type
        // will call these methods anyway.
        
        private static T PrimitiveMin<T>(IEnumerable<T> source) where T : struct, IComparable<T>
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            using (IEnumerator<T> iterator = source.GetEnumerator())
            {
                if (!iterator.MoveNext())
                {
                    throw new InvalidOperationException("Sequence was empty");
                }
                T max = iterator.Current;
                while (iterator.MoveNext())
                {
                    T item = iterator.Current;
                    if (max.CompareTo(item) > 0)
                    {
                        max = item;
                    }
                }
                return max;
            }
        }

        private static T? NullablePrimitiveMin<T>(IEnumerable<T?> source) where T : struct, IComparable<T>
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            T? max = null;
            foreach (T? item in source)
            {
                if (item != null &&
                    (max == null || max.Value.CompareTo(item.Value) > 0))
                {
                    max = item;
                }
            }
            return max;
        }
        #endregion
    }
}
