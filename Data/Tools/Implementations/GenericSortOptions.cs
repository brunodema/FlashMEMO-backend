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
        public string ColumnToSort
        {
            get { return ColumnToSort; }
            set
            {
                ColumnToSort = value;
                DetermineColumnToSortExpression(ColumnToSort);
            }
        }
        protected Expression<Func<TEntity, object>> ColumnToSortExprssion { get; set; } = null;

        public GenericSortOptions(SortType sortType = SortType.None, string columnToSort = "")
        {
            SortType = sortType;
            ColumnToSort = columnToSort;
        }
        public IEnumerable<TEntity> GetSortedResults(IQueryable<TEntity> elements)
        {
            if (SortType == SortType.Ascending)
            {
                return elements.OrderBy(ColumnToSortExprssion);
            }
            else if (SortType == SortType.Descending)
            {
                return elements.OrderByDescending(ColumnToSortExprssion);
            }
            return elements;
        }
        protected abstract void DetermineColumnToSortExpression(string columnToSort = "");

        public Expression<Func<TEntity, object>> GetColumnToSort()
        {
            return ColumnToSortExprssion;
        }
    }
}
