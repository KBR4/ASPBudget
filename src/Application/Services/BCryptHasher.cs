namespace Application.Services
{
    public class BCryptHasher : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            var hashedBytes = BCrypt.Net.BCrypt.HashPassword(password);
            return hashedBytes;
        }

        public bool VerifyPassword(string password, string? storedHash)
        {
            if (string.IsNullOrEmpty(storedHash))
            {
                return false;
            }

            var hashedPassword = HashPassword(password);
            return BCrypt.Net.BCrypt.Verify(password, storedHash);
        }
    }
}
