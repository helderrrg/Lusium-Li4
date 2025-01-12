
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

        public static bool IsNifValid(string nif)
        {
            if (nif.Length != 9)
            {
                return false;
            }

            if (!int.TryParse(nif, out _))
            {
                return false;
            }

            return true;
        }

        public static bool IsNumAssocValid(string numAssoc)
        {
            if (numAssoc.Length != 6)
            {
                return false;
            }

            if (!int.TryParse(numAssoc, out _))
            {
                return false;
            }

            return true;
        }

        public static bool IsDateValid(DateOnly data) 
        {
            return !(data.Day < 1 || data.Day > 31 || data.Month < 1 || data.Month > 12 || data.Year < 1900 || data.Year > DateTime.Now.Year - 18);
        }
    }
}