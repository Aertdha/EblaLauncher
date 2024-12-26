using System.Security.Cryptography;
using System.Text;

namespace EblaLauncher.Services
{
    public interface IEncryptionService
    {
        string Encrypt(string text);
        string Decrypt(string cipherText);
    }

    // Сервис для шифрования и дешифрования данных с использованием AES.
    // Ключ шифрования генерируется на основе уникальных параметров системы.
    public class EncryptionService : IEncryptionService
    {
        private readonly byte[] _key;
        private readonly byte[] _iv;

        public EncryptionService()
        {
            using var deriveBytes = new Rfc2898DeriveBytes(
                GetMachineConstant(),
                Encoding.UTF8.GetBytes("EblaLauncherSalt"),
                1000,
                HashAlgorithmName.SHA256);

            _key = deriveBytes.GetBytes(32);
            _iv = deriveBytes.GetBytes(16);
        }

        // Формирует уникальный идентификатор машины на основе системных параметров
        private string GetMachineConstant()
        {
            var identifiers = new List<string>
            {
                Environment.MachineName,
                Environment.UserName,
                Environment.OSVersion.Version.ToString()
            };

            if (OperatingSystem.IsWindows())
            {
                var productId = GetWindowsProductId();
                if (!string.IsNullOrEmpty(productId))
                {
                    identifiers.Add(productId);
                }
            }
            
            if (OperatingSystem.IsLinux())
            {
                identifiers.Add(Environment.GetEnvironmentVariable("HOSTNAME") ?? "");
            }
            else if (OperatingSystem.IsMacOS())
            {
                identifiers.Add(Environment.GetEnvironmentVariable("LOGNAME") ?? "");
            }
            
            return string.Join("_", identifiers.Where(id => !string.IsNullOrEmpty(id)));
        }

        private string? GetWindowsProductId()
        {
            try
            {
                if (OperatingSystem.IsWindows())
                {
                    var key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(
                        @"SOFTWARE\Microsoft\Windows NT\CurrentVersion");
                    return key?.GetValue("ProductId")?.ToString();
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public string Encrypt(string text)
        {
            using var aes = Aes.Create();
            aes.Key = _key;
            aes.IV = _iv;

            using var encryptor = aes.CreateEncryptor();
            var plainBytes = Encoding.UTF8.GetBytes(text);
            var cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
            
            return Convert.ToBase64String(cipherBytes);
        }

        public string Decrypt(string cipherText)
        {
            using var aes = Aes.Create();
            aes.Key = _key;
            aes.IV = _iv;

            using var decryptor = aes.CreateDecryptor();
            var cipherBytes = Convert.FromBase64String(cipherText);
            var plainBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
            
            return Encoding.UTF8.GetString(plainBytes);
        }
    }
}