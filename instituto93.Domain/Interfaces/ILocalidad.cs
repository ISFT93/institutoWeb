using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace instituto93.Domain.Interfaces
{
    public interface ILocalidad 
    {
        int Id { get; set; }
        string Nombre { get; set; }
    }
}
