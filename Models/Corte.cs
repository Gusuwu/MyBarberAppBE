using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBarberAPI.Models
{
    public class Corte
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public byte[] Foto { get; set; }
    }
}
