using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyBarberAPI.Models
{
    public partial class Usuario
    {
        public int Id { get; set; }
        public string Usuario1 { get; set; }
        public string Nombre { get; set; }
        [MaxLength(256)]
        public string Contrasena { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string Servicio { get; set; }
        public string Notas { get; set; }
        public string Dias { get; set; }
        public string Horarios { get; set; }
        public int Role { get; set; }
        public byte[] Foto { get; set; }

    }
}
