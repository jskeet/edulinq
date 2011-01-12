using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Edulinq.TestSupport
{
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
}
