using System;

namespace GameLevels.Domain
{
    [Serializable]
    public struct GameLevel
    {
        public long id;
        public int coinsReward;
        public bool completed;
        public string imageUri;

        public GameLevel(long id, int coinsReward, bool completed, string image)
        {
            this.id = id;
            this.coinsReward = coinsReward;
            this.completed = completed;
            this.imageUri = image;
        }
    }
}