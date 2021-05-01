using Data.Interfaces;
using Data.Models;
using System.Threading.Tasks;

namespace Data.Repository
{
    public class ApplicationUserRepository : BaseRepository<ApplicationUser, FlashMEMOContext>
    {
        public ApplicationUserRepository(FlashMEMOContext context) : base(context) { }
    }
}
