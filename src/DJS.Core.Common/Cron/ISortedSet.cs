using System;
using System.Collections.Generic;
using System.Text;

namespace DJS.Core.Common
{
    public interface ISortedSet : ISet
    {
        /// <summary>
        /// Returns a portion of the list whose elements are greater than the limit object parameter.
        /// </summary>
        /// <param name="limit">The start element of the portion to extract.</param>
        /// <returns>The portion of the collection whose elements are greater than the limit object parameter.</returns>
        ISortedSet TailSet(object limit);
    }
}
