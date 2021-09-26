namespace GameLevels.Domain
{
    public class LevelsUseCases
    {
        private ILevelsRepository _levelsRepository;
        private IBalanceRepository _balanceRepository;

        GameLevel GetLevel(int id)
        {
            return _levelsRepository.GetLevel(id);
        }
        
        
        
        
    }
}