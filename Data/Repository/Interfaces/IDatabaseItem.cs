using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Interfaces
{
    public interface IDatabaseItem<TKey>
    {
        public abstract TKey GetId();

        public abstract Expression<Func<object>> GetSortColumnFromString(string column);
    }
}
