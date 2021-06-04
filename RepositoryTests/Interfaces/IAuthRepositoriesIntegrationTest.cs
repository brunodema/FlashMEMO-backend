﻿using Data.Repository.Interfaces;
using Data.Models;
using System;
using Data.Tools;
using System.Linq.Expressions;

namespace RepositoryTests.Interfaces
{
    public interface IAuthRepositoriesIntegrationTest
    {
        // User
        // CRUD
        public void User_CreateAsync_AssertThatItGetsProperlyCreated();
        public void User_UpdateAsync_AssertThatItGetsProperlyUpdated();
        public void User_RemoveAsync_AssertThatItGetsProperlyRemoved();
        public void User_GetByIdAsync_AssertThatItGetsProperlyRemoved();
        // AUX
        public void User_SearchAndOrderAsync_AssertThatItGetsProperlySorted(int numRecords, SortType sortType);
        public void User_SearchAndOrderAsync_AssertThatPredicateIsConsidered(Expression<Func<ApplicationUser, bool>> predicate, int numRecords, SortType sortType, int expectedNumberOfRecordsReturned);
        public void User_SearchFirstAsync_AssertThatItWorksProperly(string email, bool expectNull);

        // Role
        // CRUD
        public void Role_CreateAsync_AssertThatItGetsProperlyCreated();
        public void Role_UpdateAsync_AssertThatItGetsProperlyUpdated();
        public void Role_RemoveAsync_AssertThatItGetsProperlyRemoved();
        public void Role_GetByIdAsync_AssertThatItGetsProperlyRemoved();
        // AUX
        public void Role_SearchAndOrderAsync_AssertThatItWorksProperly(int numRecords, SortType sortType);
        public void Role_SearchAndOrderAsync_AssertThatPredicateIsConsidered(Expression<Func<ApplicationRole, bool>> predicate, int numRecords, SortType sortType, int expectedNumberOfRecordsReturned);
        public void Role_SearchFirstAsync_AssertThatItWorksProperly(string roleName, bool expectNull);
    }
}
