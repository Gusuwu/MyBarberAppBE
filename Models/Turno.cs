using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBarberAPI.Models
{
    public class Turno
    {
        public int Id { get; set; }
        public int IdBarbero { get; set; }
        public int IdUsuario { get; set; }
        public int IdPrecio { get; set; }
        public DateTime Dia { get; set; }
        public TimeSpan Horario { get; set; }

        // Relaciones
        public Usuario Barbero { get; set; }
        public Usuario Usuario { get; set; }
        public Precio Precio { get; set; }
    }

}
