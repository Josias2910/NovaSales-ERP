using System;
using System.Collections.Generic;
using System.Text;
using BCrypt.Net;

namespace CapaPresentacion.Utilidades
{
    public static class SeguridadPassword
    {
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);
        }

        public static bool ValidarPassword(string passwordIngresada, string passwordHasheada)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(passwordIngresada, passwordHasheada);
            }
            catch
            {
                return false;
            }
        }
    }
}
