using ForgingDomain;
using UnityEngine;

namespace Balance.Data
{
    public class PlayerPrefsBalanceRepository : ScriptableObject, IBalanceRepository
    {

        private const string BALANCE_PREF_KEY = "Balance";

        public int GetBalance()
        {
            return PlayerPrefs.GetInt(BALANCE_PREF_KEY, 0);
        }

        public int AddBalance(int value)
        {
            var balance = GetBalance() + value;

            if (balance < 0)
                balance = 0;
            
            PlayerPrefs.SetInt(BALANCE_PREF_KEY, balance);
            return balance;
        }
    }
}
