using UnityEngine;
using Zenject;

namespace LevelLoading
{
    public class LevelLoader : MonoBehaviour
    {
        [SerializeField]
        private Transform levelHolder;
        [Inject]
        private ILevelLoadingTransition levelSwitchAnimator;
        private GameLevel levelToLoad = null;
        private bool isLevelSwitching = false;

        private void Start()
        {
            if(levelHolder==null)
            {
                var foundedLevelHolder = GameObject.FindWithTag("LevelHolder");
                if(foundedLevelHolder!=null)
                    levelHolder = foundedLevelHolder.transform;
            }
        }

        public void LoadLevel(GameLevel level)
        {
            levelToLoad = level;
            if(!isLevelSwitching)
            {
                isLevelSwitching = true;
                levelSwitchAnimator.StartAnimation(
                    delegate
                    {
                        SpawnLevel();
                    },
                    delegate
                    {
                        TryLoadLevel();
                    });
            }
        }

        private void TryLoadLevel()
        {
            isLevelSwitching = false;
            if(levelToLoad!=null)
                LoadLevel(levelToLoad);
        }

        private void SpawnLevel()
        {
            while(levelHolder.childCount>0)
                DestroyImmediate(levelHolder.GetChild(0).gameObject);
            Instantiate(levelToLoad.levelObject, levelHolder.position, levelHolder.rotation, levelHolder);
            levelToLoad = null;
        }
    }
}
