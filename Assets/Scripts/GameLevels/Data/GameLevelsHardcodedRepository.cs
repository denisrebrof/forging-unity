using System.Collections.Generic;
using GameLevels.Domain;
using UnityEngine;

namespace LevelLoader.Data
{
    public class GameLevelsHardcodedRepository : MonoBehaviour, IGameLevelsRepository
    {
        [SerializeField] private List<GameLevel> levels;
        
        public GameLevel GetLevel(long id)
        {
            return levels.Find(level => level.id == id);
        }

        public List<GameLevel> GetLevelsPaged(int page, int pageSize)
        {
            return levels.GetRange(page * pageSize, pageSize);
        }

        public int GetLevelsCount()
        {
            return levels.Count;
        }

        public void CompleteLevel(long id)
        {
            var level = GetLevel(id);
            var arrayId = levels.IndexOf(level);
            level.completed = true;
            levels[arrayId] = level;
        }
    }
}