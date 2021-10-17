using GameLevels.Domain;
using LevelLoading;
using UnityEngine;
using Zenject;

namespace GameLevels.Presentation
{
    public class LevelLoader : MonoBehaviour
    {
        [SerializeField] private Transform levelHolder;
        [Inject] private ILevelLoadingTransition levelSwitchAnimator;
        private GameLevel levelToLoad = null;
        private bool isLevelSwitching = false;

        private void Start()
        {
            if (levelHolder != null) return;
            var foundedLevelHolder = GameObject.FindWithTag("LevelHolder");
            if (foundedLevelHolder != null)
                levelHolder = foundedLevelHolder.transform;
        }

        public void LoadLevel(GameLevel level)
        {
            levelToLoad = level;
            if (isLevelSwitching)
                return;

            isLevelSwitching = true;
            levelSwitchAnimator.StartAnimation(
                SpawnLevel,
                TryLoadLevel
            );
        }

        private void TryLoadLevel()
        {
            isLevelSwitching = false;
            if (levelToLoad != null)
                LoadLevel(levelToLoad);
        }

        private void SpawnLevel()
        {
            while (levelHolder.childCount > 0)
                DestroyImmediate(levelHolder.GetChild(0).gameObject);
            Debug.Log(levelToLoad.LevelObjectUri);
            var levelObject = Resources.Load(levelToLoad.LevelObjectUri) as GameObject;
            Instantiate(levelObject, levelHolder.position, levelHolder.rotation, levelHolder);
            levelToLoad = null;
        }
    }
}