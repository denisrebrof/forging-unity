using UnityEngine;

namespace GameLevels.Domain
{
    public interface ILevelItemImageRepository
    {
        Sprite GetSpriteForLevel(long levelId);
    }
}
