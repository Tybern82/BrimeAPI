using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrimeAPI.com.brimelive.api {

    /// <summary>
    /// Defines a sorting order when making requests which may retrieve a subset of all available items
    /// </summary>
    public enum SortOrder {
        /// <summary>
        /// Sort responses from oldest to newest
        /// </summary>
        ASC,
        /// <summary>
        /// SOrt responses from newest to oldest
        /// </summary>
        DESC
    }

    /// <summary>
    /// Helper methods for working with SortOrder enum.
    /// </summary>
    public static class SortOrderUtil {

        /// <summary>
        /// Retrieve the parameter to use in query for sort order
        /// </summary>
        /// <param name="order">SortOrder to process</param>
        /// <returns>"asc" for SortOrder.ASC, otherwise "desc"</returns>
        public static string GetSortString(this SortOrder order) => order switch {
            SortOrder.ASC => "asc",
            _ => "desc",
        };
    }
}
