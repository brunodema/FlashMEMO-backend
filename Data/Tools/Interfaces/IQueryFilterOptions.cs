using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Tools.Interfaces
{
    public interface IQueryFilterOptions<TEntiy> where TEntiy : class
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Content { get; set; }

        public IEnumerable<TEntiy> GetFilteredResults(IQueryable<TEntiy> elements);
    }
}
