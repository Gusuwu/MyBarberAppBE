using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyBarberAPI.Models;
using MyBarberAPI.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNet.Identity;
using System.Security.Cryptography;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;



namespace MyBarberAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        public readonly BARBERAPIContext _dbContext;
        public readonly IConfiguration _Configuration;

        public UsuarioController(BARBERAPIContext _context, IConfiguration configuration) {
            _dbContext = _context;
            _Configuration = configuration;
        }

        [HttpGet]
        [Route("GetUsuarios")]
        public async Task<List<UsuarioViewModel>> GetUsuarios() {
            var usuarios = await _dbContext.Usuario.ToListAsync();
            List<UsuarioViewModel> usuariosVM = new List<UsuarioViewModel>();

            foreach (var u in usuarios)
            {
                int[] dias = null;
                int[] horarios = null;
                if (u.Dias != null && u.Dias != "")
                {
                    dias = u.Dias.Split(',').Select(int.Parse).ToArray();
                }

                if (u.Horarios != null && u.Horarios != "")
                {
                    horarios = u.Horarios.Split(',').Select(int.Parse).ToArray();
                }
                UsuarioViewModel usuarioVM = new UsuarioViewModel
                {
                    Id = u.Id,
                    Nombre = u.Nombre,
                    Correo = u.Correo,
                    Telefono = u.Telefono,
                    Usuario = u.Usuario1,
                    Contrasena = u.Contrasena,
                    Notas = u.Notas,
                    Servicio = u.Servicio,
                    Dias = dias,
                    Horarios = horarios
                };

                usuariosVM.Add(usuarioVM);
            }

            // Devuelve la lista de modelos de vista de usuario
            return usuariosVM;
        }

        [HttpGet]
        [Route("GetBarberos")]
        public async Task<List<UsuarioViewModel>> GetBarberos()
        {
            var usuarios = await _dbContext.Usuario.Where(b => b.Role == 1).ToListAsync();
            List<UsuarioViewModel> usuariosVM = new List<UsuarioViewModel>();

            foreach (var u in usuarios)
            {
                string base64Image = "";
                if (u.Foto != null)
                {
                    base64Image = Convert.ToBase64String(u.Foto);
                }

                int[] dias = null;
                int[] horarios = null;
                if (u.Dias != null && u.Dias != "")
                {
                    dias = u.Dias.Split(',').Select(int.Parse).ToArray();
                }

                if (u.Horarios != null && u.Horarios != "")
                {
                    horarios = u.Horarios.Split(',').Select(int.Parse).ToArray();
                }

                UsuarioViewModel usuarioVM = new UsuarioViewModel
                {
                    Id = u.Id,
                    Nombre = u.Nombre,
                    Correo = u.Correo,
                    Telefono = u.Telefono,
                    Usuario = u.Usuario1,
                    Contrasena = u.Contrasena,
                    Notas = u.Notas,
                    Servicio = u.Servicio,
                    Dias = dias,
                    Horarios = horarios,
                    Foto = u.Foto,
                    FotoUrl = base64Image
                };

                usuariosVM.Add(usuarioVM);
            }

            return usuariosVM;
        }

        [HttpPost]
        [Route("PostUsuario")]
        public async Task<IActionResult> PostUsuario([FromBody] UsuarioViewModel usuarioViewModel)
        {
            if (ModelState.IsValid)
            {
                usuarioViewModel.Contrasena = encriptarContrasena(usuarioViewModel.Contrasena);
                var nuevoUsuario = new Usuario
                {
                    Nombre = usuarioViewModel.Nombre,
                    Correo = usuarioViewModel.Correo,
                    Usuario1 = usuarioViewModel.Usuario,
                    Contrasena = usuarioViewModel.Contrasena,
                    Telefono = usuarioViewModel.Telefono,
                    Servicio = usuarioViewModel.Servicio,
                    Dias = string.Join(",", usuarioViewModel.Dias), // Convertir arreglo de enteros a cadena separada por comas
                    Horarios = string.Join(",", usuarioViewModel.Horarios),
                    Notas = usuarioViewModel.Notas,
                    Role = 0
                };

                _dbContext.Usuario.Add(nuevoUsuario);
                await _dbContext.SaveChangesAsync();

                return Ok();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        [Route("LoginUsuario")]
        public async Task<UsuarioViewModel> Login([FromBody] LoginViewModel userLogin)
        {
            var user = _dbContext.Usuario.Where(u => u.Usuario1 == userLogin.Usuario).FirstOrDefault();

            if (user != null)
            {
                var pass = encriptarContrasena(userLogin.Contrasena);
                if(user.Contrasena == pass)
                {

                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.UTF8.GetBytes(_Configuration["JwtSettings:SecretKey"]);

                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                            new Claim(ClaimTypes.Name, user.Usuario1)
                        }),
                        Expires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(_Configuration["JwtSettings:ExpiresInMinutes"])),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };

                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    var tokenString = tokenHandler.WriteToken(token);

                    int[] dias = null;
                    int[] horarios = null;
                    if (user.Dias != null && user.Dias != "")
                    {
                        dias = user.Dias.Split(',').Select(int.Parse).ToArray();
                    }

                    if (user.Horarios != null && user.Horarios != "") {
                        horarios = user.Horarios.Split(',').Select(int.Parse).ToArray();
                    }

                    string base64Image = "";
                    if (user.Foto != null)
                    {
                        base64Image = Convert.ToBase64String(user.Foto);
                    }

                    UsuarioViewModel usuarioVM = new UsuarioViewModel
                    {
                        Id = user.Id,
                        Nombre = user.Nombre,
                        Correo = user.Correo,
                        Usuario = user.Usuario1,
                        Token = tokenString,
                        Role = user.Role
                    };
                    return usuarioVM;
                }
                
            }

            return new UsuarioViewModel();
        }

        [HttpPut]
        [Route("UpdateUsuario")]
        public async Task<IActionResult> UpdateUsuario([FromBody] UsuarioViewModel usuarioViewModel)
        {
            if (ModelState.IsValid)
            {
                var usuarioExistente = await _dbContext.Usuario.FindAsync(usuarioViewModel.Id);
                if (usuarioExistente == null)
                {
                    return NotFound();
                }

                // Actualiza los campos necesarios
                usuarioExistente.Nombre = usuarioViewModel.Nombre;
                usuarioExistente.Correo = usuarioViewModel.Correo;
                usuarioExistente.Usuario1 = usuarioViewModel.Usuario;
                usuarioExistente.Telefono = usuarioViewModel.Telefono;
                usuarioExistente.Notas = usuarioViewModel.Notas;

                _dbContext.Usuario.Update(usuarioExistente);
                await _dbContext.SaveChangesAsync();

                return Ok();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpGet]
        [Route("GetUsuario/{id}")]
        public async Task<UsuarioViewModel> GetUsuario(int id)
        {
            var u = await _dbContext.Usuario.Where(u => u.Id == id).FirstOrDefaultAsync();
            string base64Image = "";
                int[] dias = null;
                int[] horarios = null;
                if (u.Dias != null && u.Dias != "")
                {
                    dias = u.Dias.Split(',').Select(int.Parse).ToArray();
                }

                if (u.Horarios != null && u.Horarios != "")
                {
                    horarios = u.Horarios.Split(',').Select(int.Parse).ToArray();
                }

                if (u.Foto != null) {
                    base64Image = Convert.ToBase64String(u.Foto);
                }   

                UsuarioViewModel usuarioVM = new UsuarioViewModel
                {
                    Id = u.Id,
                    Nombre = u.Nombre,
                    Correo = u.Correo,
                    Telefono = u.Telefono,
                    Usuario = u.Usuario1,
                    Notas = u.Notas,
                    Servicio = u.Servicio,
                    Dias = dias,
                    Horarios = horarios,
                    Foto = u.Foto,
                    FotoUrl = base64Image,
                    Role = u.Role
                };

            return usuarioVM;
        }

        [HttpPost]
        [Route("UploadImage/{id}")]
        public async Task<IActionResult> UploadImage(int id, [FromForm] IFormFile file)
        {
            var usuarioExistente = await _dbContext.Usuario.Where(u => u.Id == id).FirstOrDefaultAsync();
            if (usuarioExistente == null)
            {
                return NotFound();
            }

            // Convierte la imagen a un arreglo de bytes
            using (var memoryStream = new System.IO.MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                usuarioExistente.Foto = memoryStream.ToArray();
            }

            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        public string encriptarContrasena(string pass) {
            SHA256 sha = SHA256Managed.Create();
            ASCIIEncoding encode = new ASCIIEncoding();
            byte[] stream = null;
            stream = sha.ComputeHash(encode.GetBytes(pass));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }
    }
}
