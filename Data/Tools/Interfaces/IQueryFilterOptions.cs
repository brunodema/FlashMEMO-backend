using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Tools.Interfaces
{
    public interface IQueryFilterOptions<TEntiy> where TEntiy : class
    {
        public IEnumerable<TEntiy> GetFilteredResults(IQueryable<TEntiy> elements);
    }
}
