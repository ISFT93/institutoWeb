using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace instituto93.Domain.Models
{
    public class Pais
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Abreviatura { get; set; } = string.Empty;
    }
}
