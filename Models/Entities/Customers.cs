using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Reflection;
using System.Text.Json.Serialization;

namespace WebApplication_Inlamning_P_3.Models.Entities
{
    public class Customer
    {
       // [BindNever]
        //[JsonIgnore]
        public int CustomerId { get; set; } 
        public string Givenname { get; set; }
        public string Surname { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string Streetaddress { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Zipcode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string CountryCode { get; set; }= string.Empty;
        public DateTime Birthday { get; set; } = DateTime.Now;
        public string Telephonecountrycode { get; set; } = string.Empty;
        public string Telephonenumber { get; set; } = string.Empty;
        public string Emailaddress { get; set; } = string.Empty;

        public Customer(int customerId, string givenname, string surname, string gender, string streetaddress, string city, string zipcode, string country, string countryCode, DateTime birthday, string telephonecountrycode, string telephonenumber, string emailaddress)
        {
            CustomerId = customerId;
            Givenname = givenname;
            Surname = surname;
            Gender = gender;
            Streetaddress = streetaddress;
            City = city;
            Zipcode = zipcode;
            Country = country;
            CountryCode = countryCode;
            Birthday = birthday;
            Telephonecountrycode = telephonecountrycode;
            Telephonenumber = telephonenumber;
            Emailaddress = emailaddress;
        }

        public Customer()
        {
            
        }
    }
}
