using System.Text.RegularExpressions;

namespace Application
{
    public static class PasswordValidationCheck
    {
        public static bool PasswordContainsUpperAndLower(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return false;
            }
            bool hasUpper = false;
            bool hasLower = false;

            foreach (char c in password)
            {
                if (char.IsUpper(c))
                {
                    hasUpper = true;
                }
                if (char.IsLower(c))
                {
                    hasLower = true;
                }
                if (hasUpper && hasLower)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool PasswordContainsDigit(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return false;
            }
            return password.Any(c => char.IsDigit(c));
        }

        public static bool PasswordNotContainsSpaces(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return true;
            }
            return !password.Any(c => char.IsWhiteSpace(c));
        }

        public static bool PasswordContainsOnlyAllowedChars(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return false;
            }
            var regex = new Regex(@"^[a-zA-Z0-9~!?@#$%^&*_\-+()\[\]{}><\/\\|""'.,:;]+$");
            return regex.IsMatch(password);
        }
    }
}
