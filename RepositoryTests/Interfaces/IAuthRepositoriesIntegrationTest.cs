using Data.Interfaces;
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
        public void GetUserRolesAsync_CheckThatItRetrievesDataCorrectly();

        // User
        // CRUD
        public void User_CreateAsync_AssertThatItGetsProperlyCreated();
        public void User_UpdateAsync_AssertThatItGetsProperlyUpdated();
        public void User_RemoveAsync_AssertThatItGetsProperlyRemoved();
        public void User_GetByIdAsync_AssertThatItGetsProperlyRemoved();
        // AUX
        public void User_SearchAndOrderAsync_AssertThatItGetsProperlySorted(int numRecords, SortType sortType);
        //public void User_SearchAllAsync_AssertThatItWorksProperly(); // does not make sense to exist considering that the above method does everything better
        public void User_SearchFirstAsync_AssertThatItWorksProperly(string email, bool expectNull);
        //public void User_GetAllAsync_AssertThatItWorksProperly(); // also does not make much sense to test this, since it is used throughout other tests

        // Role
        // CRUD
        public void Role_CreateAsync_AssertThatItGetsProperlyCreated();
        public void Role_UpdateAsync_AssertThatItGetsProperlyUpdated();
        public void Role_RemoveAsync_AssertThatItGetsProperlyRemoved();
        public void Role_GetByIdAsync_AssertThatItGetsProperlyRemoved();
        // AUX
        public void Role_SearchAndOrderAsync_AssertThatItWorksProperly();
        public void Role_SearchAllAsync_AssertThatItWorksProperly();
        public void Role_SearchFirstAsync_AssertThatItWorksProperly();
        public void Role_GetAllAsync_AssertThatItWorksProperly();
    }
}
