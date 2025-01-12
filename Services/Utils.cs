
using System.ComponentModel.DataAnnotations;

namespace Services
{
    public class Utils
    {
        public static bool IsNameValid(string name)
        {
            if (name == "")
            {
                return false;
            }

            foreach (char c in name)
            {
                if (!char.IsLetter(c))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool IsEmailValid(string email)
        {
            if (email == "")
            {
                return false;
            }

            var emailAttribute = new EmailAddressAttribute();
            if (!emailAttribute.IsValid(email))
            {
                return false;
            }
            
            if (email.IndexOf('@') == 0 || email.IndexOf('.') == 0 || email.IndexOf('@') == email.Length - 1 || email.IndexOf('.') == email.Length - 1)
            {
                return false;
            }

            return true;
        }

        public static bool IsPasswordValid(string password)
        {
            if (password.Length < 8)
            {
                return false;
            }

            bool hasNumber = false;
            bool hasSpecialChar = false;
            foreach (char c in password)
            {
                if (char.IsDigit(c))
                {
                    hasNumber = true;
                }
                else if (!char.IsLetterOrDigit(c))
                {
                    hasSpecialChar = true;
                }
            }

            if (!hasNumber || !hasSpecialChar)
            {
                return false;
            }

            return true;
        }
    }
}