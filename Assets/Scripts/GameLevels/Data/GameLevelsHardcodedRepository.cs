using System.Collections.Generic;
using System.Linq;
using GameLevels.Domain;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameLevels.Data
{
    [CreateAssetMenu(fileName = "GameLevelsHardcodedRepository")]
    public class GameLevelsHardcodedRepository : ScriptableObject, IGameLevelsRepository
    {
        [SerializeField] private List<GameLevel> GameLevels;
        
        public GameLevel GetLevel(long id)
        {
            return GameLevels.Find(level => level.ID == id);
        }

        public List<GameLevel> GetLevelsPaged(int page, int pageSize)
        {
            var startIndex = page * pageSize;
            if (startIndex >= GameLevels.Count)
                return new List<GameLevel>();

            var size = Mathf.Min(GameLevels.Count - startIndex, pageSize);
            return GameLevels.GetRange(startIndex, size);
        }

        public int GetLevelsCount()
        {
            return GameLevels.Count;
        }

        public void CompleteLevel(long id)
        {
            var level = GetLevel(id);
            var arrayId = GameLevels.IndexOf(level);
            level.Completed = true;
            GameLevels[arrayId] = level;
        }

        public GameLevel GetCurrentLevel()
        {
            return GameLevels.FirstOrDefault(level => !level.Completed) ?? GameLevels.Last();
        }

        public List<GameLevel> GetLevels()
        {
            return GameLevels;
        }
    }
}