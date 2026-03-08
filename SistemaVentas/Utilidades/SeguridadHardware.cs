using System;
using System.Collections.Generic;
using System.Text;
using System.Management;

namespace CapaPresentacion.Utilidades
{
    public class SeguridadHardware
    {
        public static string ObtenerHardwareID()
        {
            string id = "";
            try
            {
                using (var searcher = new ManagementObjectSearcher("SELECT SerialNumber FROM Win32_BaseBoard"))
                {
                    foreach (var obj in searcher.Get())
                    {
                        id = obj["SerialNumber"]?.ToString().Trim();
                    }
                }

                if (string.IsNullOrEmpty(id) || id.ToLower().Contains("default") || id.Contains("0000"))
                {
                    using (var searcher = new ManagementObjectSearcher("SELECT ProcessorId FROM Win32_Processor"))
                    {
                        foreach (var obj in searcher.Get())
                        {
                            id = obj["ProcessorId"]?.ToString().Trim();
                        }
                    }
                }
            }
            catch
            {
                id = Environment.MachineName;
            }

            return string.IsNullOrEmpty(id) ? "NS-UNKNOWN-ID" : id;
        }
        public static string GenerarHashConFecha(string hardwareID, string fechaStr)
        {
            string salt = "B23UFKS8453K@SLF3NOVA";

            using (System.Security.Cryptography.SHA256 sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(hardwareID + salt + fechaStr));
                string hash = BitConverter.ToString(hashBytes).Replace("-", "");
                return hash.Substring(0, 16);
            }
        }
    }
}
