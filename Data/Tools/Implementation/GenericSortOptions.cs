using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Data.Tools.Implementation
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
        public string ColumnToSort { get; set; } = "";
        protected Expression<Func<TEntity, object>> ColumnToSortExpression { get; set; } = null;

        public GenericSortOptions(SortType sortType = SortType.None, string columnToSort = "")
        {
            SortType = sortType;
            ColumnToSort = columnToSort;
            DetermineColumnToSortExpression(ColumnToSort);
        }
        public IEnumerable<TEntity> GetSortedResults(IQueryable<TEntity> elements)
        {
            DetermineColumnToSortExpression(ColumnToSort);
            if (SortType == SortType.Ascending)
            {
                return elements.OrderBy(ColumnToSortExpression);
            }
            else if (SortType == SortType.Descending)
            {
                return elements.OrderByDescending(ColumnToSortExpression);
            }
            return elements;
        }
        protected abstract void DetermineColumnToSortExpression(string columnToSort = "");

        public Expression<Func<TEntity, object>> GetColumnToSort()
        {
            return ColumnToSortExpression;
        }
    }
}
