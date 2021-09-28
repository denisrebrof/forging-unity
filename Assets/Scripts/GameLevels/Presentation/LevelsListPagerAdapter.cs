using LevelLoader.Data;
using Pager;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameLevels.Presentation
{
    public class LevelsListPagerAdapter : DefaultPagerAdapter<DefaultPageViewHolder>
    {
        //TODO: provide via di by IGameLevelsRepository
        [FormerlySerializedAs("repository")] [SerializeField]
        private GameLevelsHardcodedRepository hardcodedRepository;

        [SerializeField] private GameObject levelPrefab;

        [SerializeField] private int levelsOnPage = 16;

        public override int GetPagesCount()
        {
            var pagesCount = hardcodedRepository.GetLevelsCount() / levelsOnPage;
            if (hardcodedRepository.GetLevelsCount() % levelsOnPage != 0)
                pagesCount += 1;
            return pagesCount;
        }
    }
}