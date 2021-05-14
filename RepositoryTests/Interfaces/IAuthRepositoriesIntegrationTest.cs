using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTests.Interfaces
{
    public interface IAuthRepositoriesIntegrationTest
    {
        public void AddUserToRole_CheckThatItGetsCorrectlyAdded();
        public void RemoveUserFromRole_CheckThatItGetsCorrectlyRemoved();
    }
}
