using System;
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
        public void GetByIdAsync_AssertThatItGetsProperlyRemoved();
    }
}
