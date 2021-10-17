namespace ForgingDomain
{
    public interface IBalanceRepository
    {
        int GetBalance();
        int AddBalance(int value);
    }
}