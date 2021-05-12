﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTests
{
    public interface IBaseRepositoryTests<TEntity>
    {
        public void CreateAsync_AssertThatItGetsProperlyCreated();
        public void UpdateAsync_AssertThatItGetsProperlyUpdated();
        public void RemoveAsync_AssertThatItGetsProperlyRemoved();

        // still missing: GetById, validation tests
    }

    public interface IAuthRepositoryTest
    {
        public void CreateUserAsync_AssertThatItGetsProperlyCreated();
        public void UpdateUserAsync_AssertThatItGetsProperlyUpdated();
        public void RemoveUserAsync_AssertThatItGetsProperlyRemoved();
        public void GetUserByIdAsync_AssertThatItGetsProperlyRemoved();
    }
}
