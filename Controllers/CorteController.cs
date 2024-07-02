using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBarberAPI.Models;
using MyBarberAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBarberAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CorteController : ControllerBase
    {
        public readonly BARBERAPIContext _dbContext;

        public CorteController(BARBERAPIContext _context)
        {
            _dbContext = _context;
        }

        [HttpGet]
        [Route("GetCortes")]
        public async Task<List<CorteViewModel>> GetCortes()
        {
            var cortes = _dbContext.Corte.ToList();
            List<CorteViewModel> cortesVM = new List<CorteViewModel>();

            foreach (var c in cortes)
            {
                CorteViewModel corteVM = new CorteViewModel
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    Descripcion = c.Descripcion,
                    Foto = c.Foto
                    
                };

                cortesVM.Add(corteVM);
            }

            return cortesVM;
        }
    }
}
