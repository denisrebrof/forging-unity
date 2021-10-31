using Doozy.Engine;
using GameLevels.Domain;
using GameLevels.Presentation;
using UnityEngine;
using Zenject;

namespace LevelManager
{
    public class LevelManager : MonoBehaviour
    {
        [Inject] private LevelLoader _levelLoader;
        [Inject] private GameLevelsUseCases _levelsUseCases;

        public string LevelCompletedUIEvent = "LevelCompleted";
        public string LevelLoadedUIEvent = "OpenLevel";

        private void Start()
        {
            LoadCurrentLevel();
        }

        [ContextMenu("CompleteLevel")]
        public void CompleteLevel()
        {
            var currentLevel = _levelsUseCases.GetCurrentLevel();
            _levelsUseCases.CompleteLevel(currentLevel.id);
            GameEventMessage.SendEvent(LevelCompletedUIEvent, gameObject);
        }

        [ContextMenu("LoadCurrentLevel")]
        public void LoadCurrentLevel()
        {
            var currentLevel = _levelsUseCases.GetCurrentLevel();
            LoadLevel(currentLevel);
        }

        public void LoadLevel(long levelId)
        {
            var level = _levelsUseCases.GetLevel(levelId);
            LoadLevel(level);
        }

        private void LoadLevel(GameLevel level)
        {
            _levelLoader.LoadLevel(level);
            GameEventMessage.SendEvent(LevelLoadedUIEvent, gameObject);
        }
    }
}