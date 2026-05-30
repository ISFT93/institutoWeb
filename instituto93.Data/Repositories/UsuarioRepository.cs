using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using instituto93.Domain.Models;

namespace instituto93.Data.Repositories
{
    public class UsuarioRepository
    {
        // Constantes PBKDF2
        private const int Iterations = 100_000;
        private const int SaltSize = 16; // bytes
        private const int KeySize = 32;  // bytes

        // Ejemplo: almacenamiento en memoria para pruebas.
        // En producción reemplazar por _context (EF Core) u otro almacenamiento.
        private readonly List<Usuario> _usuarios = new();

        public Usuario GetUsuarioByEmail(string email)
        {
            // Si usas EF Core: return _context.Usuarios.FirstOrDefault(u => u.Email == email);
            return _usuarios.FirstOrDefault(u => string.Equals(u.Email, email, StringComparison.OrdinalIgnoreCase));
        }

        public void AddUsuario(Usuario usuario)
        {
            if (usuario == null) throw new ArgumentNullException(nameof(usuario));
            if (string.IsNullOrEmpty(usuario.Password)) throw new ArgumentException("Password requerido.", nameof(usuario));

            // Hashear la contraseña antes de persistir
            if (!IsPasswordHashed(usuario.Password))
            {
                usuario.Password = HashPassword(usuario.Password);
            }

            // Persistir: aquí ejemplo en memoria; reemplazar por _context.Usuarios.Add(usuario); _context.SaveChanges();
            usuario.Id = _usuarios.Any() ? _usuarios.Max(u => u.Id) + 1 : 1;
            _usuarios.Add(usuario);
        }

        public void UpdateUsuario(Usuario usuario)
        {
            if (usuario == null) throw new ArgumentNullException(nameof(usuario));

            var existing = _usuarios.FirstOrDefault(u => u.Id == usuario.Id);
            if (existing == null) return;

            // Si la contraseña enviada está en claro (no comienza con el prefijo), la hasheamos.
            if (!string.IsNullOrEmpty(usuario.Password) && !IsPasswordHashed(usuario.Password))
            {
                existing.Password = HashPassword(usuario.Password);
            }

            // Actualizar otros campos
            existing.Nombre = usuario.Nombre;
            existing.Apellido = usuario.Apellido;
            existing.Email = usuario.Email;
            existing.FechaNacimiento = usuario.FechaNacimiento;
            existing.Dni = usuario.Dni;
            existing.Telefono = usuario.Telefono;
            existing.Direccion = usuario.Direccion;
            existing.LocalidadId = usuario.LocalidadId;

            // Si usas EF Core, aquí harías _context.Usuarios.Update(existing); _context.SaveChanges();
        }

        public void DeleteUsuario(int id)
        {
            // Ejemplo en memoria
            _usuarios.RemoveAll(u => u.Id == id);

            // Si usas EF Core:
            // var usuario = _context.Usuarios.Find(id);
            // if (usuario != null) { _context.Usuarios.Remove(usuario); _context.SaveChanges(); }
        }

        public List<Usuario> GetAllUsuarios()
        {
            // En producción: return _context.Usuarios.ToList();
            return _usuarios.ToList();
        }

        public bool ValidateUser(string email, string password)
        {
            var usuario = GetUsuarioByEmail(email);
            if (usuario == null) return false;
            if (string.IsNullOrEmpty(usuario.Password)) return false;

            return VerifyPassword(usuario.Password, password);
        }

        // --- Helpers PBKDF2 ---

        private static string HashPassword(string password)
        {
            var salt = new byte[SaltSize];
            RandomNumberGenerator.Fill(salt);

            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
            var key = pbkdf2.GetBytes(KeySize);

            // Formato: PBKDF2$V1$iterations$saltBase64$keyBase64
            return $"PBKDF2$V1${Iterations}${Convert.ToBase64String(salt)}${Convert.ToBase64String(key)}";
        }

        private static bool VerifyPassword(string storedHash, string providedPassword)
        {
            try
            {
                if (string.IsNullOrEmpty(storedHash) || string.IsNullOrEmpty(providedPassword)) return false;

                var parts = storedHash.Split('$');
                if (parts.Length != 5) return false;
                if (!string.Equals(parts[0], "PBKDF2", StringComparison.Ordinal)) return false;
                if (!string.Equals(parts[1], "V1", StringComparison.Ordinal)) return false;

                var iterations = int.Parse(parts[2]);
                var salt = Convert.FromBase64String(parts[3]);
                var key = Convert.FromBase64String(parts[4]);

                using var pbkdf2 = new Rfc2898DeriveBytes(providedPassword, salt, iterations, HashAlgorithmName.SHA256);
                var attempted = pbkdf2.GetBytes(key.Length);

                // Comparación en tiempo fijo
                return CryptographicOperations.FixedTimeEquals(attempted, key);
            }
            catch
            {
                return false;
            }
        }

        private static bool IsPasswordHashed(string password)
        {
            return !string.IsNullOrEmpty(password) && password.StartsWith("PBKDF2$V1$", StringComparison.Ordinal);
        }
    }
}