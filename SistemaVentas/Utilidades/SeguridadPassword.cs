using System;
using System.Collections.Generic;
using System.Text;
using BCrypt.Net;

namespace CapaPresentacion.Utilidades
{
    public static class SeguridadPassword
    {
        // 1. Generar el Hash (Para cuando el usuario se registra o cambia clave)
        public static string HashPassword(string password)
        {
            // El workFactor 12 es el equilibrio perfecto entre seguridad y velocidad
            return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);
        }

        // 2. Verificar (Para el Login)
        public static bool ValidarPassword(string passwordIngresada, string passwordHasheada)
        {
            try
            {
                // Compara el texto plano con el hash guardado en la BD
                return BCrypt.Net.BCrypt.Verify(passwordIngresada, passwordHasheada);
            }
            catch
            {
                return false;
            }
        }
    }
}
