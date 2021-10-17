using System;
using UnityEngine.Serialization;

namespace GameLevels.Domain
{
    [Serializable]
    public class GameLevel
    {
        public long ID;
        public int Number;
        public int CoinsReward;
        public bool Completed;
        public string ImageUri;
        public string LevelObjectUri;
    }
}