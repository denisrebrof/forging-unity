using System.Collections.Generic;
using ForgingDomain;
using Zenject;

namespace GameLevels.Domain
{
    public class GameLevelsUseCases
    {
        [Inject]
        private IGameLevelsRepository _levelsRepository;
        [Inject]
        private IBalanceRepository _balanceRepository;

        public GameLevel GetLevel(long id) => _levelsRepository.GetLevel(id);

        public List<GameLevel> GetLevelsPaged(int page, int pageSize) =>
            _levelsRepository.GetLevelsPaged(page, pageSize);

        public GameLevel GetCurrentLevel()
        {
            return _levelsRepository.GetCurrentLevel();
        }

        public void CompleteLevel(long id)
        {
            var level = _levelsRepository.GetLevel(id);
            if(!level.completed)
                _balanceRepository.AddBalance(level.coinsReward);
            _levelsRepository.CompleteLevel(id);
        }
    }
}