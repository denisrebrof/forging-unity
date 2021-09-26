using LevelLoader.Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace Pager
{
    public class LevelsListPagerAdapter : MonoBehaviour, IPagerAdapter
    {
        //TODO: provide via di by IGameLevelsRepository
        [FormerlySerializedAs("repository")] [SerializeField]
        private GameLevelsHardcodedRepository hardcodedRepository;
        
        [SerializeField] private GameObject pagePrefab;
        [SerializeField] private GameObject levelPrefab;

        [SerializeField] private int levelsOnPage = 16;

        public RectTransform CreatePage()
        {
            var page = Instantiate(pagePrefab);
            for (var i = 0; i < levelsOnPage; i++)
                Instantiate(levelPrefab, pagePrefab.transform);
            return page.GetComponent<RectTransform>();
        }

        public void OnBind(GameObject page, int pageNumber)
        {
            
        }

        public void OnRecycle(GameObject page)
        {
            
        }

        public int GetPagesCount()
        {
            return 5;
        }
    }
}
