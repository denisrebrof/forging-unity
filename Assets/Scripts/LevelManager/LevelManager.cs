using Doozy.Engine;
using GameLevels.Domain;
using LevelManagement;
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

        private int _currentLevelID = 0;

        private static string currentLevelIDPref = "currentLevelIDPref";

        public string LevelCompletedUIEvent = "LevelCompleted";
        public string LevelLoadedUIEvent = "OpenLevel";

        private void Start()
        {
            if(PlayerPrefs.HasKey(currentLevelIDPref))
                _currentLevelID = PlayerPrefs.GetInt(currentLevelIDPref);
            LoadCurrentLevel();
        }

        [ContextMenu("CompleteLevel")]
        public void CompleteLevel()
        {
            if(_levelsUseCases.smashingLevels.Length==0)
                return;
            _currentLevelID = Mathf.Clamp(_currentLevelID+1,0, _levelsUseCases.get);
            PlayerPrefs.SetInt(currentLevelIDPref, _currentLevelID);
            GameEventMessage.SendEvent(LevelCompletedUIEvent, gameObject);
        }

        [ContextMenu("LoadCurrentLevel")]
        public void LoadCurrentLevel()
        {
            var currentLevel = _levelsUseCases.GetLevel(_currentLevelID);
            _levelLoader.LoadLevel(currentLevel);
            Debug.Log("Loading level " + _currentLevelID);
            GameEventMessage.SendEvent(LevelLoadedUIEvent, gameObject);
        }

        [ContextMenu("Clear progress")]
        public void ClearProgress() => PlayerPrefs.DeleteKey(currentLevelIDPref);
    }
}