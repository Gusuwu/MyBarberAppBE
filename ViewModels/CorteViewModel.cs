using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBarberAPI.ViewModels
{
    public class CorteViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public byte[] Foto { get; set; }
    }
}
