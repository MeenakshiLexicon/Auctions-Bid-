using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json.Serialization;

namespace WebApplication_Inlamning_P_3.Models.Entities
{
    public class Loan
    {
        [BindNever]
        [JsonIgnore]
        public int LoanId { get; set; } = 0;
        public int AccountId { get; set; }
       
        public decimal Amount { get; set;}

        public int Duration { get; set; }
        public decimal Payments { get; set; }
       
        public int CustomerId { get; set; }


        public Loan(int loanId, int accountId,  decimal amount, int duration, decimal payments,  int customerId)
        {
            LoanId = loanId;
            AccountId = accountId;
           
            Amount = amount;
            Duration = duration;
            Payments = payments;
            
            CustomerId = customerId;
        }
    }
}
