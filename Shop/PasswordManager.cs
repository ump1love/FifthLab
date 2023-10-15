using System.Security.Cryptography;
using System.Text;

class PasswordManager
{
    public static string GenerateSalt()
    {
        byte[] saltBytes = new byte[16];
        using (var rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(saltBytes);
        }

        return BitConverter.ToString(saltBytes).Replace("-", "");
    }
    public static string HashPassword(string password, string salt)
    {
        using (var sha256 = SHA256.Create())
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password + salt);
            byte[] hashedBytes = sha256.ComputeHash(passwordBytes);

            string hashedPassword = BitConverter.ToString(hashedBytes).Replace("-", "");
            return hashedPassword;
        }
    }

    public static bool VerifyPassword(string enteredPassword, string storedHash, string salt)
    {
        string enteredPasswordHash = HashPassword(enteredPassword, salt);
        return enteredPasswordHash == storedHash;
    }
}