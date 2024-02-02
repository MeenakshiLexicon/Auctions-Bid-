using WebApplication_Inlamning_P_3.Models.Entities;

namespace WebApplication_Inlamning_P_3.Repository.Interfaces
{
    public interface ILoanRepo
    {
        void PostLoanForCustomer(int customerId, decimal amount, int duration, decimal payments,int accountId);
    }
}
