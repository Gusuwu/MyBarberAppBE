using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBarberAPI.ViewModels
{
    public class PrecioViewModel
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public decimal Valor { get; set; }
        public bool NotDisponible { get; set; }
    }
}
