using System.Collections.Generic;
using System.Linq;

namespace Data.Tools.Interfaces
{
    public interface IQueryFilterOptions<TEntiy> where TEntiy : class
    {
        /// <summary>
        /// Returns the full URL to be used for the query, according to TEntity's properties and specific filters (ex: FromDate <= x <= ToDate)
        /// </summary>
        /// <returns>Full URL to be used for the query.</returns>
        public string BuildQueryURL();

        public IEnumerable<TEntiy> GetFilteredResults(IQueryable<TEntiy> elements);
    }
}
