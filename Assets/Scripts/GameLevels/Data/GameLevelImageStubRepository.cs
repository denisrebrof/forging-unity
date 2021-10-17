using GameLevels.Domain;
using UnityEngine;

namespace GameLevels.Data
{
    [CreateAssetMenu(fileName = "GameLevelImageStubRepository")]
    public class GameLevelImageStubRepository : ScriptableObject, ILevelItemImageRepository
    {
        [SerializeField] private Sprite stubBackground;


        public Sprite GetSpriteForLevel(long levelId)
        {
            return stubBackground;
        }
    }
}
