using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface ICredentials
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }

    public class Credentials : ICredentials
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }
}
