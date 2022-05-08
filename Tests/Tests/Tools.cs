using Data.Tools.Sorting;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Tests
{
    public class Tools
    {
        /// <summary>
        /// This class is only here because C# is dumb and does not allow Expressions to be declared in loco. Therefore, I must create a strongly-typed object so that the lambda (Func) I declare later is automatically converted into an Expression.
        /// </summary>
        public class ValidateFilteringTestData<T>
        {
            public List<T> entities { get; set; } = new List<T>();
            public Expression<Func<T, bool>> predicate { get; set; }
            public Expression<Func<T, object>> sortPredicate { get; set; }
            public SortType sortType { get; set; } = SortType.None;
        }
    }
}
