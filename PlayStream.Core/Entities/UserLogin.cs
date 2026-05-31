using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayStream.Core.Entities
{
    public class UserLogin
    {
        public string User { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}