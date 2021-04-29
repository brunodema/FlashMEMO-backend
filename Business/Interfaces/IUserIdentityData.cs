using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public class UserInfo
    {
        // to be further expanded (if necessary)
        public string Email { get; set; }
    }
    public interface IUserIdentityData
    {
        UserInfo User { get; set; }
        IList<string> UserRoles { get; set; }
    }
}
