using GameLevels.Domain;
using UnityEngine;

namespace LevelManagement
{
    [CreateAssetMenu(fileName = "LevelManagementSettings")]
    public class LevelManagementSettings : ScriptableObject
    {
        public GameLevel[] smashingLevels;
    }
}
