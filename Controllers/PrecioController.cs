using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyBarberAPI.Models;
using MyBarberAPI.ViewModels;

namespace MyBarberAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PrecioController : ControllerBase
    {
        public readonly BARBERAPIContext _dbContext;

        public PrecioController(BARBERAPIContext _context)
        {
            _dbContext = _context;
        }

        [HttpGet]
        public async Task<List<PrecioViewModel>> GetPrecios()
        {
            var precios = await _dbContext.Precio.ToListAsync();
            List<PrecioViewModel> preciosVM = new List<PrecioViewModel>();

            foreach (var p in precios)
            {
                PrecioViewModel precioVM = new PrecioViewModel
                {
                    Id = p.Id,
                    Titulo = p.Titulo,
                    Descripcion = p.Descripcion,
                    Valor = p.Valor,
                    NotDisponible = p.NotDisponible
                };

                preciosVM.Add(precioVM);
            }

            return preciosVM;
        }


        [HttpPut]
        [Route("UpdatePrecio")]
        public async Task<IActionResult> UpdatePrecio([FromBody] PrecioViewModel precioViewModel)
        {
            if (ModelState.IsValid)
            {
                var precio = await _dbContext.Precio.FindAsync(precioViewModel.Id);
                if (precio == null)
                {
                    return NotFound();
                }

                // Actualiza los campos necesarios
                precio.Valor = precioViewModel.Valor;
                
                _dbContext.Precio.Update(precio);
                await _dbContext.SaveChangesAsync();

                return Ok();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPut]
        [Route("NaPrecio")]
        public async Task<IActionResult> NAPrecio([FromBody] PrecioViewModel precioViewModel)
        {
            if (ModelState.IsValid)
            {
                var precio = await _dbContext.Precio.FindAsync(precioViewModel.Id);
                if (precio == null)
                {
                    return NotFound();
                }

                // Actualiza los campos necesarios
                precio.NotDisponible = precioViewModel.NotDisponible;

                _dbContext.Precio.Update(precio);
                await _dbContext.SaveChangesAsync();

                return Ok();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpDelete]
        [Route("DeletePrecio/{id}")]
        public async Task<IActionResult> DeletePrecio(int id)
        {
            var precio = await _dbContext.Precio.FindAsync(id);
            if (precio == null)
            {
                return NotFound();
            }

            _dbContext.Precio.Remove(precio);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
