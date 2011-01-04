using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Edulinq
{
    internal class ReverseComparer<T> : IComparer<T>
    {
        private readonly IComparer<T> forwardComparer;

        internal ReverseComparer(IComparer<T> forwardComparer)
        {
            this.forwardComparer = forwardComparer;
        }

        public int Compare(T x, T y)
        {
            return forwardComparer.Compare(y, x);
        }
    }
}
