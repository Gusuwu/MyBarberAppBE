using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBarberAPI.Models;
using MyBarberAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MyBarberAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TurnoController : ControllerBase
    {
        public readonly BARBERAPIContext _dbContext;

        public TurnoController(BARBERAPIContext _context)
        {
            _dbContext = _context;
        }

        [HttpGet("CheckDisponibilidad")]
        public async Task<IActionResult> CheckDisponibilidad(int idBarbero, DateTime dia, string horario)
        {
            if (!TimeSpan.TryParse(horario, out TimeSpan horarioTimeSpan))
            {
                return BadRequest("Horario inválido");
            }

            var turnoExistente = _dbContext.Turno
                .Any(t => t.IdBarbero == idBarbero && t.Dia == dia && t.Horario == horarioTimeSpan);

            if (turnoExistente)
            {
                return Ok(false);
            }
            return Ok(true);
        }

        [HttpPost("PostTurno")]
        public async Task<IActionResult> PostTurno([FromBody] TurnoViewModel turnoViewModel)
        {
            if (ModelState.IsValid)
            {
                TimeSpan horario;
                if (!TimeSpan.TryParse(turnoViewModel.Horario, out horario))
                {
                    return BadRequest("Formato de horario no válido.");
                }

                var nuevoTurno = new Turno
                {
                    IdBarbero = turnoViewModel.IdBarbero,
                    IdUsuario = turnoViewModel.IdUsuario,
                    IdPrecio = turnoViewModel.IdPrecio,
                    Dia = turnoViewModel.Dia,
                    Horario = horario
                };

                _dbContext.Turno.Add(nuevoTurno);
                await _dbContext.SaveChangesAsync();

                return Ok(nuevoTurno);
            }

            return BadRequest(ModelState);
        }

        [HttpGet("GetTurnosByUsuario/{id}")]
        public async Task<List<TurnoViewModel>> GetTurnosByUsuario(int id)
        {

            var turnos = _dbContext.Turno
                .Include(t => t.Barbero)
                .Include(t => t.Precio)
                .Where(t => t.IdUsuario == id).ToList();
            List<TurnoViewModel> turnosList = new List<TurnoViewModel>();

            foreach (var t in turnos)
            {
                TurnoViewModel turnoViewModel = new TurnoViewModel
                {
                    Id = t.Id,
                    IdBarbero = t.IdBarbero,
                    IdPrecio = t.IdUsuario,
                    Horario = t.Horario.ToString(@"hh\:mm"),
                    Dia = t.Dia,
                    ValorPrecio = t.Precio.Valor,
                    NombreBarbero = t.Barbero.Nombre,
                    DescripcionPrecio = t.Precio.Titulo,
                };

                turnosList.Add(turnoViewModel);
            }
            return turnosList;
        }

        [HttpGet("GetTurnosByBarbero")]
        public async Task<IActionResult> GetTurnosByBarbero(int id, DateTime dia)
        {
            dia = new DateTime(dia.Year, dia.Month, dia.Day, 0, 0, 0);
            var turnos = await _dbContext.Turno
        .Include(t => t.Barbero)
        .Include(t => t.Precio)
        .Include(t => t.Usuario)
        .Where(t => t.IdBarbero == id && t.Dia.Date == dia.Date)
        .OrderBy(t => t.Horario) // Ordena por horario
        .ToListAsync();

            if (turnos == null || !turnos.Any())
            {
                return NotFound("No turnos found for the specified criteria.");
            }

            List<TurnoViewModel> turnosList = new List<TurnoViewModel>();

            foreach (var t in turnos) {
                TurnoViewModel turnoViewModel = new TurnoViewModel
                {
                    Id = t.Id,
                    IdBarbero = t.IdBarbero,
                    IdUsuario = t.IdUsuario,
                    IdPrecio = t.IdPrecio,
                    Horario = t.Horario.ToString(@"hh\:mm"),
                    Dia = t.Dia,
                    ValorPrecio = t.Precio.Valor,
                    NombreBarbero = t.Barbero.Nombre,
                    DescripcionPrecio = t.Precio.Titulo,
                    NombreUsuario = t.Usuario.Nombre
                };

                turnosList.Add(turnoViewModel);
            }

            return Ok(turnosList);
        }

    }
}