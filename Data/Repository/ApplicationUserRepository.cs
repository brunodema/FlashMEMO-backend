using Data.Interfaces;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
    public class ApplicationUserRepository : BaseRepository<ApplicationUser, FlashMEMOContext>
    {
        public ApplicationUserRepository(FlashMEMOContext context) : base(context) { }
    }
}
