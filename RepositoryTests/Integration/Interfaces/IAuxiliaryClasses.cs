using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Integration.Interfaces
{
    public interface IValidationErrorsWhenCreatingData<TEntiy>
    {
        public TEntiy Entiy { get; set; }
        public string[] Errors { get; set; }
    }
    public interface IGetsSpecifiedNumberOfRecordsPerPageData<TEntiy>
    {
        public int pageSize { get; set; }
        public int? pageNumber { get; set; }
        public int expectedNumberOfRecords { get; set; }
    }
}