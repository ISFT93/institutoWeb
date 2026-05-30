using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace instituto93.Domain.Interfaces
{
    public interface IUsuario
    {
        string Email { get; set; }
        string Password { get; set; }

        bool activo { get; set; }
    }
}
