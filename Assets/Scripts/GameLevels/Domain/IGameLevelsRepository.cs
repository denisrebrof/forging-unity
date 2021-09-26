using System.Collections.Generic;

namespace GameLevels.Domain
{
    public interface IGameLevelsRepository
    {
        GameLevel GetLevel(long id);
        List<GameLevel> GetLevelsPaged(int page, int pageSize);
        int GetLevelsCount();
        void CompleteLevel(long id);
    }
}
