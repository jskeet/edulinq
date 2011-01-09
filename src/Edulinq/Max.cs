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
        public static int Max(this IEnumerable<int> source)
        {
            return PrimitiveMax(source);
        }

        public static int Max<TSource>(
	        this IEnumerable<TSource> source,
	        Func<TSource, int> selector)
        {
            // Select will validate the arguments
            return PrimitiveMax(source.Select(selector));
        }

        public static int? Max(this IEnumerable<int?> source)
        {
            return NullablePrimitiveMax(source);
        }

        public static int? Max<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, int?> selector)
        {
            // Select will validate the arguments
            return NullablePrimitiveMax(source.Select(selector));
        }
        #endregion

        #region Int64
        public static long Max(this IEnumerable<long> source)
        {
            return PrimitiveMax(source);
        }

        public static long Max<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, long> selector)
        {
            // Select will validate the arguments
            return PrimitiveMax(source.Select(selector));
        }

        public static long? Max(this IEnumerable<long?> source)
        {
            return NullablePrimitiveMax(source);
        }

        public static long? Max<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, long?> selector)
        {
            // Select will validate the arguments
            return NullablePrimitiveMax(source.Select(selector));
        }
        #endregion

        #region Double
        public static double Max(this IEnumerable<double> source)
        {
           return PrimitiveMax(source);
        }

        public static double Max<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, double> selector)
        {
            // Select will validate the arguments
            return PrimitiveMax(source.Select(selector));
        }

        public static double? Max(this IEnumerable<double?> source)
        {
            return NullablePrimitiveMax(source);
        }

        public static double? Max<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, double?> selector)
        {
            // Select will validate the arguments
            return NullablePrimitiveMax(source.Select(selector));
        }
        #endregion

        #region Single
        public static float Max(this IEnumerable<float> source)
        {
            return PrimitiveMax(source);
        }

        public static float Max<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, float> selector)
        {
            // Select will validate the arguments
            return PrimitiveMax(source.Select(selector));
        }

        public static float? Max(this IEnumerable<float?> source)
        {
            return NullablePrimitiveMax(source);
        }

        public static float? Max<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, float?> selector)
        {
            // Select will validate the arguments
            return NullablePrimitiveMax(source.Select(selector));
        }
        #endregion

        #region Decimal
        public static decimal Max(this IEnumerable<decimal> source)
        {
            return PrimitiveMax(source);
        }

        public static decimal Max<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, decimal> selector)
        {
            // Select will validate the arguments
            return PrimitiveMax(source.Select(selector));
        }

        public static decimal? Max(this IEnumerable<decimal?> source)
        {
            return NullablePrimitiveMax(source);
        }

        public static decimal? Max<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, decimal?> selector)
        {
            // Select will validate the arguments
            return NullablePrimitiveMax(source.Select(selector));
        }
        #endregion

        #region Generic methods
        public static TSource Max<TSource>(
            this IEnumerable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            // This condition will be true for reference types and nullable value types, and false for
            // non-nullable value types.
            return default(TSource) == null ? NullableGenericMax(source) : NonNullableGenericMax(source);
        }

        public static TResult Max<TSource, TResult>(
            this IEnumerable<TSource> source,
            Func<TSource, TResult> selector)
        {
            return Max(source.Select(selector));
        }

        /// <summary>
        /// Implements the generic behaviour for non-nullable value types.
        /// </summary>
        /// <remarks>
        /// Empty sequences will cause an InvalidOperationException to be thrown.
        /// Note that there's no *compile-time* validation in the caller that the type
        /// is a non-nullable value type, hence the lack of a constraint on T.
        /// </remarks>
        private static T NonNullableGenericMax<T>(IEnumerable<T> source)
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
                    if (comparer.Compare(max, item) < 0)
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
        private static T NullableGenericMax<T>(IEnumerable<T> source)
        {
            Comparer<T> comparer = Comparer<T>.Default;

            T max = default(T);
            foreach (T item in source)
            {
                if (item != null &&
                    (max == null || comparer.Compare(max, item) < 0))
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
        
        private static T PrimitiveMax<T>(IEnumerable<T> source) where T : struct, IComparable<T>
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
                    if (max.CompareTo(item) < 0)
                    {
                        max = item;
                    }
                }
                return max;
            }
        }

        private static T? NullablePrimitiveMax<T>(IEnumerable<T?> source) where T : struct, IComparable<T>
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            T? max = null;
            foreach (T? item in source)
            {
                if (item != null &&
                    (max == null || max.Value.CompareTo(item.Value) < 0))
                {
                    max = item;
                }
            }
            return max;
        }
        #endregion
    }
}
