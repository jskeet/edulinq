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
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MergeSortTest
{
    // Console application to test my merge sort implementation by running against lots of inputs
    // of varying sizes and with varying domains.
    public class Program
    {
        private static readonly int[] Domains = { 10, 100, 1000, 10000 };
        private static readonly int[] Sizes = { 10, 1000, 100000 };
        private const int IterationsPerCombination = 20;

        private static void Main()
        {
            // Use a fixed seed so we can reproduce failures
            Random rng = new Random(1);
            int totalFailures = 0;
            foreach (int domain in Domains)
            {
                foreach (int size in Sizes)
                {
                    Console.WriteLine("Testing domain={0} size={1}", domain, size);
                    int failures = 0;
                    for (int i = 0; i < IterationsPerCombination; i++)
                    {
                        if (!TestSort(rng, domain, size))
                        {
                            failures++;
                        }
                    }
                    if (failures != 0)
                    {
                        Console.WriteLine("{0} failures", failures);
                    }
                    totalFailures += failures;
                }
            }
            Console.WriteLine("Total failures: {0}", totalFailures);
            Console.WriteLine("Overhead: {0}", (double)totalMergeTicks / (double)totalFrameworkTicks);
        }

        static long totalFrameworkTicks = 0;
        static long totalMergeTicks = 0;
        static Stopwatch stopwatch = new Stopwatch();

        private static bool TestSort(Random rng, int domain, int size)
        {
            // Use a List<double> and a custom comparer which just compares the integer values,
            // so that we can easily test for stability
            List<double> input = Enumerable.Range(0, size)
                                           .Select(x => rng.Next(domain) + rng.NextDouble())
                                           .ToList();
            stopwatch.Reset();
            stopwatch.Start();
            List<double> actual = global::Edulinq.Enumerable.OrderBy(input, x => x, TruncatedDoubleComparer.Instance)
                                                                 .ToList();
            stopwatch.Stop();
            totalMergeTicks += stopwatch.ElapsedTicks;

            stopwatch.Reset();
            stopwatch.Start();
            List<double> expected = global::System.Linq.Enumerable.OrderBy(input, x => x, TruncatedDoubleComparer.Instance)
                                                                  .ToList();
            stopwatch.Stop();
            totalFrameworkTicks += stopwatch.ElapsedTicks;

            return expected.SequenceEqual(actual);
        }

        private static T[] MergeSort<T>(List<T> input, IComparer<T> comparer)
        {
            T[] data = input.ToArray();
            T[] tmp = new T[data.Length];
            MergeSort(data, tmp, 0, input.Count - 1, comparer);
            return data;
        }

        // Note: right is *inclusive*
        private static void MergeSort<T>(T[] data, T[] tmp, int left, int right, IComparer<T> comparer)
        {
            if (right > left)
            {
                // Optimize the case where we've only got two elements. Conditionally swap them.
                if (right == left + 1)
                {
                    T leftElement = data[left];
                    T rightElement = data[right];
                    if (comparer.Compare(leftElement, rightElement) > 0)
                    {
                        data[left] = rightElement;
                        data[right] = leftElement;
                    }
                }
                else
                {
                    int mid = (left + right)/2;
                    MergeSort(data, tmp, left, mid, comparer);
                    MergeSort(data, tmp, mid + 1, right, comparer);
                    Merge(data, tmp, left, mid + 1, right, comparer);
                }
            }
        }

        // Merge lists [left, mid) and [mid, right]
        private static void Merge<T>(T[] data, T[] tmp, int left, int mid, int right, IComparer<T> comparer)
        {
            int leftCursor = left;
            int rightCursor = mid;
            int tmpCursor = left;
            T leftElement = data[leftCursor];
            T rightElement = data[rightCursor];
            // By never merging tiny lists, we know we'll always have valid starting points
            bool bothListsActive = true;
            while (bothListsActive)
            {
                // When equal, use the left element to achieve stability
                if (comparer.Compare(leftElement, rightElement) <= 0)
                {
                    tmp[tmpCursor++] = leftElement;
                    leftCursor++;
                    bothListsActive = leftCursor < mid;
                    if (bothListsActive)
                    {
                        leftElement = data[leftCursor];
                    }
                    else
                    {
                        // Only the right list is still active. Therefore tmpCursor must equal rightCursor,
                        // so there's no point in copying the right list to tmp and back again. Just copy
                        // the already-sorted bits back into data.
                        Array.Copy(tmp, left, data, left, tmpCursor - left);
                    }
                }
                else
                {
                    tmp[tmpCursor++] = rightElement;
                    rightCursor++;
                    bothListsActive = rightCursor <= right;
                    if (bothListsActive)
                    {
                        rightElement = data[rightCursor];
                    }
                    else
                    {
                        // Only the left list is still active. Therefore we can copy the remainder of
                        // the left list directly to the appropriate place in data, and then copy the
                        // appropriate portion of tmp back.
                        Array.Copy(data, leftCursor, data, tmpCursor, mid - leftCursor);
                        Array.Copy(tmp, left, data, left, tmpCursor - left);
                    }
                }
            }
        }

        private class TruncatedDoubleComparer : IComparer<double>
        {
            internal static IComparer<double> Instance = new TruncatedDoubleComparer();

            public int Compare(double x, double y)
            {
                return ((int)x).CompareTo((int)y);
            }
        }
    }
}
