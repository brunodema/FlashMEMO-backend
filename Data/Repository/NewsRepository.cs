using Data.Interfaces;
using Data.Models;
using System.Threading.Tasks;

namespace Data.Repository
{
    public class NewsRepository : BaseRepository<News, FlashMEMOContext>
    {
        public NewsRepository(FlashMEMOContext context) : base(context) { }
    }
}
