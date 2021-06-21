﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Tools.Interfaces
{
    public interface IQueryFilterOptions<TEntiy> where TEntiy : class
    {
        public IQueryable<TEntiy> GetFilteredResults(IQueryable<TEntiy> elements);
    }
}
