using System.Collections.Generic;
using ForgingDomain;

namespace GameLevels.Domain
{
    public class GameLevelsUseCases
    {
        private IGameLevelsRepository _levelsRepository;
        private IBalanceRepository _balanceRepository;

        public GameLevel GetLevel(int id) => _levelsRepository.GetLevel(id);

        public List<GameLevel> GetLevelsPaged(int page, int pageSize) =>
            _levelsRepository.GetLevelsPaged(page, pageSize);

        public void CompleteLevel(int id)
        {
            var level = _levelsRepository.GetLevel(id);
            if(!level.completed)
                _balanceRepository.AddBalance(level.coinsReward);
            _levelsRepository.CompleteLevel(id);
        }
    }
}