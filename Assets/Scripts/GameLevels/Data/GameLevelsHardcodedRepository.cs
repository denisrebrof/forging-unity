using System.Collections.Generic;
using GameLevels.Domain;
using UnityEngine;

namespace GameLevels.Data
{
    [CreateAssetMenu(fileName = "GameLevelsHardcodedRepository")]
    public class GameLevelsHardcodedRepository : ScriptableObject, IGameLevelsRepository
    {
        [SerializeField] private List<GameLevel> levels;
        
        public GameLevel GetLevel(long id)
        {
            return levels.Find(level => level.id == id);
        }

        public List<GameLevel> GetLevelsPaged(int page, int pageSize)
        {
            var startIndex = page * pageSize;
            if (startIndex >= levels.Count)
                return new List<GameLevel>();

            var size = Mathf.Min(levels.Count - startIndex, pageSize);
            return levels.GetRange(startIndex, size);
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