using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace MyBarberAPI.ViewModels
{
    public class UsuarioViewModel
    {
        public int Id { get; set; }
        public string Usuario { get; set; }
        public string Nombre { get; set; }
        public string Contrasena { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string? Servicio { get; set; }
        public string? Notas { get; set; }
        public int[]? Dias { get; set; }
        public int[]? Horarios { get; set; }
        public byte[]? Foto { get; set; }
        public string? Token { get; set; }
        public string? FotoUrl { get; set; }
        public int? Role { get; set; }
    }
}
