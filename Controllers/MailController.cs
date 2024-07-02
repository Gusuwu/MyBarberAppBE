using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MyBarberAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace MyBarberAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        public readonly BARBERAPIContext _dbContext;

        private readonly IConfiguration _configuration;

        public MailController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpPost]
        public async Task<IActionResult> SendMail([FromBody] ContactFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var smtpSettings = _configuration.GetSection("SmtpSettings");
            var message = new MailMessage
            {
                From = new MailAddress(smtpSettings["SenderEmail"], smtpSettings["SenderName"]),
                Subject = "Consulta Barberia",
                Body = $"Nombre: {model.Nombre}\nCorreo: {model.Correo}\nMensaje: {model.Mensaje}",
                IsBodyHtml = false
            };
            message.To.Add(new MailAddress("augustoerrobidarte@gmail.com"));

            using (var client = new SmtpClient(smtpSettings["Server"], int.Parse(smtpSettings["Port"])))
            {
                client.Credentials = new NetworkCredential(smtpSettings["Username"], smtpSettings["Password"]);
                client.EnableSsl = true;
                await client.SendMailAsync(message);
            }

            return Ok();
        }
    }

    public class ContactFormModel
    {
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Mensaje { get; set; }
    }
}
