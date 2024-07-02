using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBarberAPI.ViewModels
{
    public class TurnoViewModel
    {
        public int Id { get; set; }
        public int IdBarbero { get; set; }
        public string NombreBarbero { get; set; }
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public int IdPrecio { get; set; }
        public string DescripcionPrecio { get; set; }
        public decimal ValorPrecio { get; set; }
        public DateTime Dia { get; set; }
        public string Horario { get; set; }
    }
}
