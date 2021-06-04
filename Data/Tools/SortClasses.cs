using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Data.Tools
{
    public enum SortType
    {
        None,
        Ascending,
        Descending
    }
    public class SortOptions<TEntity, TKey>
    {
        public SortType SortType { get; set; } = SortType.None;
        public Expression<Func<TEntity, TKey>> ColumnToSort { get; set; } = null;
    }
}
