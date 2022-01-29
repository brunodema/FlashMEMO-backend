using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Tools.Interfaces
{
    public interface IFlashMEMOCredentials
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }
}
