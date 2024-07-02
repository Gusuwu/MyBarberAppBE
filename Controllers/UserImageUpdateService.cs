using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using MyBarberAPI.Models;

public class UserImageUpdaterService
{
    private readonly BARBERAPIContext _context;

    public UserImageUpdaterService(BARBERAPIContext context)
    {
        _context = context;
    }

    public byte[] ReadImageFile(string imagePath)
    {
        byte[] imageBytes = null;
        FileInfo fileInfo = new FileInfo(imagePath);
        long imageFileLength = fileInfo.Length;
        FileStream fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
        BinaryReader br = new BinaryReader(fs);
        imageBytes = br.ReadBytes((int)imageFileLength);
        return imageBytes;
    }

    public void UpdateUserImage(int userId, string imagePath)
    {
        // Leer la imagen desde el archivo
        byte[] imageBytes = ReadImageFile(imagePath);

        // Buscar el usuario por su ID
        var usuario = _context.Usuario.SingleOrDefault(u => u.Id == userId);
        if (usuario != null)
        {
            // Asignar la imagen al usuario
            usuario.Foto = imageBytes;

            // Guardar los cambios en la base de datos
            _context.SaveChanges();
        }
    }
}