using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Interfaces
{
    public interface IDatabaseItem<TKey>
    {
        public TKey GetId();
    }
}
