using System.Collections.Generic;
using Balance.Domain;
using ForgingDomain;
using Zenject;

namespace GameLevels.Domain
{
    public class GameLevelsUseCases
    {
        [Inject]
        private IGameLevelsRepository _levelsRepository;
        [Inject]
        private BalanceUseCases _balanceUseCases;

        public GameLevel GetLevel(long id) => _levelsRepository.GetLevel(id);

        public List<GameLevel> GetLevelsPaged(int page, int pageSize) =>
            _levelsRepository.GetLevelsPaged(page, pageSize);

        public GameLevel GetCurrentLevel() => _levelsRepository.GetCurrentLevel();

        public int GetLevelsCount() => _levelsRepository.GetLevelsCount();

        public void CompleteLevel(long id)
        {
            var level = _levelsRepository.GetLevel(id);
            if(!level.Completed)
                _balanceUseCases.AddBalance(level.CoinsReward);
            _levelsRepository.CompleteLevel(id);
        }
    }
}
