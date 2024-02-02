using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace WebApplication_Inlamning_P_3.Models.Entities
{
    public class User
    {
        [BindNever]
        [JsonIgnore]
        public int UserId { get; set; } = 0;
        public string UserName { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; }
        //public string AccountType { get; set; }

        [BindNever]
        [JsonIgnore]
        public string ExecuterRole { get; set;} = string.Empty;
        [BindNever]
        [JsonIgnore]
        public string ExecuterName { get; set; } = string.Empty;

        public User(int userId, string userName, string password, string userType, string executerRole, string executerName)
        {
            UserId = userId;
            UserName = userName;
            Password = password;
            UserType = userType;
           // AccountType = accountType;
            ExecuterRole = executerRole;
            ExecuterName = executerName;
        }

        public User()
        {
        }

        //---------FOR ADMIN USE TO ADD NEW USER,CUSTOMER,ACCOUNT------------------------
        public class UserRegistration
        {
            public User User { get; set; }
            public Customer Customer { get; set; }
            public Account Account { get; set; }
            
        }
        
        //---------FOR USER USE TO GET ALL DETAILS OF USER,CUSTOMER,ACCOUNT------------------------
        public class UserAccount
        {
            public int UserId { get; set; }
            public string UserName { get; set; }

            public int AccountId { get; set; }
            public string AccountType { get; set; }
            public decimal AccountBalance { get; set; }

            public int CustomerId { get; set; }
            public string GivenName { get; set; }
            public string Surname { get; set; }
            public string StreetAddress { get; set; }
            public string City { get; set; }
            public string ZipCode { get; set; }
            public string Country { get; set; }
            public string CountryCode { get; set; }
            public DateTime Birthday { get; set; }
            public string TelephoneCountryCode { get; set; }
            public string TelephoneNumber { get; set; }
            public string EmailAddress { get; set; }

            public UserAccount(int userId, string userName, int accountId, string accountType, decimal accountBalance, int customerId, string givenName, string surname, string streetAddress, string city, string zipCode, string country, string countryCode, DateTime birthday, string telephoneCountryCode, string telephoneNumber, string emailAddress)
            {
                UserId = userId;
                UserName = userName;
                AccountId = accountId;
                AccountType = accountType;
                AccountBalance = accountBalance;
                CustomerId = customerId;
                GivenName = givenName;
                Surname = surname;
                StreetAddress = streetAddress;
                City = city;
                ZipCode = zipCode;
                Country = country;
                CountryCode = countryCode;
                Birthday = birthday;
                TelephoneCountryCode = telephoneCountryCode;
                TelephoneNumber = telephoneNumber;
                EmailAddress = emailAddress;
            }
        }
        

    }
}
