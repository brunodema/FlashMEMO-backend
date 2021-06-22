using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.Tools.Implementations
{
    public enum SortType
    {
        None,
        Ascending,
        Descending
    }
    public abstract class GenericSortOptions<TEntity>
    {
        public SortType SortType { get; set; } = SortType.None;
        public Expression<Func<TEntity, object>> ColumnToSort { get; set; } = null;
        public GenericSortOptions(SortType sortType = SortType.None, string columnToSort = "")
        {
            SortType = sortType;
            DetermineColumnToSort(columnToSort);
        }
        public IEnumerable<TEntity> GetSortedResults(IQueryable<TEntity> elements)
        {
            DetermineColumnToSort();
            if (SortType == SortType.Ascending)
            {
                return elements.OrderBy(ColumnToSort);
            }
            else if (SortType == SortType.Descending)
            {
                return elements.OrderByDescending(ColumnToSort);
            }
            return elements;
        }
        public abstract void DetermineColumnToSort(string columnToSort = "");
    }
}
