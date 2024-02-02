using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json.Serialization;

namespace WebApplication_Inlamning_P_3.Models.Entities
{
    public class Account
    {

   
        public int AccountId { get; set; } = 0;
        //[BindNever]
        //[JsonIgnore]
        //public string UserName { get; set; } = string.Empty;
        public int CustomerId { get; set; }
        public string AccountType { get; set; } = "Savings account";
        public string Frequency { get; set; } = "Monthly";
        public decimal Balance { get; set; } = 0;
        


        public Account(int accountId, int customerId, string accountType, string frequency,  decimal balance)
        {

            AccountId = accountId;
            CustomerId = customerId;
            AccountType = accountType;
            Frequency = frequency;
            Balance = balance;
            
            
        }
        public Account()
        {
            
        }

    }
}
