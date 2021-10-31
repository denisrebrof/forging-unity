using GameLevels.Domain;
using LevelLoading;
using UnityEngine;
using Zenject;

namespace GameLevels.Presentation
{
    public class LevelLoader : MonoBehaviour
    {
        [SerializeField] private Transform levelHolder;
        [Inject] private ILevelLoadingTransition _levelSwitchAnimator;
        private GameLevel _levelToLoad;
        private bool _isLevelSwitching;

        private void Start()
        {
            if (levelHolder != null) return;
            var foundedLevelHolder = GameObject.FindWithTag("LevelHolder");
            if (foundedLevelHolder != null)
                levelHolder = foundedLevelHolder.transform;
        }

        public void LoadLevel(GameLevel level)
        {
            _levelToLoad = level;
            if (_isLevelSwitching)
                return;

            _isLevelSwitching = true;
            _levelSwitchAnimator.StartAnimation(
                SpawnLevel,
                TryLoadLevel
            );
        }

        private void TryLoadLevel()
        {
            _isLevelSwitching = false;
            if (_levelToLoad != null)
                LoadLevel(_levelToLoad);
        }

        private void SpawnLevel()
        {
            while (levelHolder.childCount > 0)
                DestroyImmediate(levelHolder.GetChild(0).gameObject);
            Debug.Log(_levelToLoad.LevelObjectUri);
            var levelObject = Resources.Load(_levelToLoad.LevelObjectUri) as GameObject;
            Instantiate(levelObject, levelHolder.position, levelHolder.rotation, levelHolder);
            _levelToLoad = null;
        }
    }
}