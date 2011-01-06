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

namespace Edulinq
{
    internal struct CompositeKey<TPrimary, TSecondary>
    {
        private readonly TPrimary primary;
        private readonly TSecondary secondary;

        internal TPrimary Primary { get { return primary; } }
        internal TSecondary Secondary{ get { return secondary; } }

        internal CompositeKey(TPrimary primary, TSecondary secondary)
        {
            this.primary = primary;
            this.secondary = secondary;
        }

        internal sealed class Comparer : IComparer<CompositeKey<TPrimary, TSecondary>>
        {
            private readonly IComparer<TPrimary> primaryComparer;
            private readonly IComparer<TSecondary> secondaryComparer;

            internal Comparer(IComparer<TPrimary> primaryComparer, IComparer<TSecondary> secondaryComparer)
            {
                this.primaryComparer = primaryComparer;
                this.secondaryComparer = secondaryComparer;
            }

            public int Compare(CompositeKey<TPrimary, TSecondary> x, CompositeKey<TPrimary, TSecondary> y)
            {
                int primaryResult = primaryComparer.Compare(x.Primary, y.Primary);
                if (primaryResult != 0)
                {
                    return primaryResult;
                }
                return secondaryComparer.Compare(x.Secondary, y.Secondary);
            }
        }
    }
}
