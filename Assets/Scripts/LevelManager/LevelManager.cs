using Doozy.Engine;
using GameLevels.Domain;
using UnityEngine;
using Zenject;

namespace LevelManager
{
    public class LevelManager : MonoBehaviour
    {
        [Inject]
        private GameLevels.Presentation.LevelLoader _levelLoader;
        [Inject]
        private GameLevelsUseCases _levelsUseCases;

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
            _levelLoader.LoadLevel(currentLevel);
            GameEventMessage.SendEvent(LevelLoadedUIEvent, gameObject);
        }
    }
}