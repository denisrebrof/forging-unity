using System;

namespace GameLevels.Domain
{
    [Serializable]
    public class GameLevel
    {
        public long id;
        public int number;
        public int coinsReward;
        public bool completed;
        public string imageUri;
        public string levelObjectUri;
    }
}