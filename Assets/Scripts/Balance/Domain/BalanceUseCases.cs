using System;
using ForgingDomain;
using UniRx;
using Zenject;

namespace Balance.Domain
{
    public class BalanceUseCases
    {
        [Inject] private IBalanceRepository _repository;

        private readonly ReactiveProperty<int> _balanceSubject = new ReactiveProperty<int>();

        public IObservable<int> GetBalanceFlow()
        {
            UpdateBalanceFlow();
            return _balanceSubject;
        }

        public void AddBalance(int balance)
        {
            _repository.AddBalance(balance);
            UpdateBalanceFlow();
        }

        private void UpdateBalanceFlow()
        {
            _balanceSubject.Value = _repository.GetBalance();
        }
    }
}