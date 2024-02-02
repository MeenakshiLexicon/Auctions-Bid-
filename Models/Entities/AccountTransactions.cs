using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json.Serialization;

namespace WebApplication_Inlamning_P_3.Models.Entities
{
    public class AccountTransactions
    {
        public int TransactionId { get; set; } = 0;
        public int AccountId { get; set; } = 0;
        public int SourceAccountId { get; set; }
        public int DestinationAccountId { get; set; }

        public DateTime Date { get; set; } = DateTime.UtcNow;
        public string Type { get; set; } = string.Empty;
        public string Operation { get; set; } = string.Empty;
        public decimal Amount { get; set; } = 0;
        
       
        public decimal Balance { get; set; } = 0;
        
        public string Symbol { get; set; } = string.Empty;
       
        public string Bank { get; set; } = string.Empty;
        [BindNever]
        [JsonIgnore]
        public string Account { get; set; } = string.Empty;
        public AccountTransactions()
        {

        }

        public AccountTransactions(int transactionId, int accountId, int sourceAccountId, int destinationAccountId, DateTime date, string type, string operation, decimal amount, decimal balance, string symbol, string bank, string account)
        {
            TransactionId = transactionId;
            AccountId = accountId;
            SourceAccountId = sourceAccountId;
            DestinationAccountId = destinationAccountId;
            Date = date;
            Type = type;
            Operation = operation;
            Amount = amount;
            Balance = balance;
            Symbol = symbol;
            Bank = bank;
            Account = account;
        }
    }

}
