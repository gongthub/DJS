using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace DJS.Core.Common
{
    public interface ISet : ICollection, IList
    {
        /// <summary>
        /// Adds a new element to the Collection if it is not already present.
        /// </summary>
        /// <param name="obj">The object to add to the collection.</param>
        /// <returns>Returns true if the object was added to the collection, otherwise false.</returns>
        new bool Add(object obj);

        /// <summary>
        /// Adds all the elements of the specified collection to the Set.
        /// </summary>
        /// <param name="c">Collection of objects to add.</param>
        /// <returns>true</returns>
        bool AddAll(ICollection c);

        /// <summary>
        /// Returns the first item in the set.
        /// </summary>
        /// <returns>First object.</returns>
        object First();
    }
}
