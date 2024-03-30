using System;

namespace LegacyApp
{
    public class UserService
    {
        private ClientRepository _clientRepository = new ClientRepository();
        private UserCreditService _userCreditService = new UserCreditService();
        
        public bool AddUser(string firstName, string lastName, string email, DateTime dateOfBirth, int clientId)
        {
            if (IsFirstNameValid(firstName) || IsLastNameValid(lastName))
            {
                return false;
            }

            if (CheckEmail(email))
            {
                return false;
            }

            var age = CalculateAgeFromBirthDate(dateOfBirth);

            if (age < 21)
            {
                return false;
            }

            var client = _clientRepository.GetById(clientId);

            var user = new User
            {
                Client = client,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                FirstName = firstName,
                LastName = lastName
            };

            if (client.Type == "VeryImportantClient")
            {
                user.HasCreditLimit = false;
            }
            else if (client.Type == "ImportantClient")
            {
                user.HasCreditLimit = true;
                
                int creditLimit = _userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth); 
                user.CreditLimit = creditLimit * 2;
            }
            else
            {
                user.HasCreditLimit = true;
                
                int creditLimit = _userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth); 
                user.CreditLimit = creditLimit;
            }

            if (user.HasCreditLimit && user.CreditLimit < 500)
            {
                return false;
            }

            UserDataAccess.AddUser(user);
            return true;
        }

        private static int CalculateAgeFromBirthDate(DateTime dateOfBirth)
        {
            var now = DateTime.Now;
            int age = now.Year - dateOfBirth.Year;
            
            var birthMonthBiggerThanCurrentMonth = now.Month < dateOfBirth.Month;
            var birthMonthAndCurrentMonthSame = now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day;
            if (birthMonthBiggerThanCurrentMonth || birthMonthAndCurrentMonthSame) age--;
            return age;
        }

        public static bool CheckEmail(string email)
        {
            return !email.Contains("@") && !email.Contains(".");
        }

        public static bool IsFirstNameValid(string firstName)
        {
            return string.IsNullOrEmpty(firstName);
        }
        
        public static bool IsLastNameValid(string lastName)
        {
            return string.IsNullOrEmpty(lastName);
        }
    }
}
