using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Integration.Interfaces
{
    public interface IRepositoryControllerTests : IRepositoryControllerCreateTests, IRepositoryControllerUpdateTests, IRepositoryControllerDeleteTests, IRepositoryControllerGetTests
    {

    }
    public interface IRepositoryControllerCreateTests
    {
        void CreatesSuccessfully();
        void ReportsValidationErrorsWhenCreating();
    }
    public interface IRepositoryControllerUpdateTests
    {
        void UpdatesSuccessfully();
        void ReportsValidationErrorsWhenUpdating();
    }
    public interface IRepositoryControllerDeleteTests
    {
        void DeletesSuccessfully();
        void ReportsValidationErrorsWhenDeleting();
    }
    public interface IRepositoryControllerGetTests
    {
        void DeletesSuccessfully();
        void ReportsValidationErrorsWhenDeleting();
    }
}
