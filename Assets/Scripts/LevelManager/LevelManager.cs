using Doozy.Engine;
using LevelManagement;
using UnityEngine;
using Zenject;

namespace LevelManager
{
    public class LevelManager : MonoBehaviour
    {
        [Inject]
        private GameLevels.Presentation.LevelLoader levelLoader;
        [Inject]
        private LevelManagementSettings levelSettings;

        private int currentLevelID = 0;

        private static string currentLevelIDPref = "currentLevelIDPref";

        public string LevelCompletedUIEvent = "LevelCompleted";
        public string LevelLoadedUIEvent = "OpenLevel";

        private void Start()
        {
            if(PlayerPrefs.HasKey(currentLevelIDPref))
                currentLevelID = PlayerPrefs.GetInt(currentLevelIDPref);
            LoadCurrentLevel();
        }

        [ContextMenu("CompleteLevel")]
        public void CompleteLevel()
        {
            if(levelSettings.smashingLevels.Length==0)
                return;
            currentLevelID = Mathf.Clamp(currentLevelID+1,0, levelSettings.smashingLevels.Length-1);
            PlayerPrefs.SetInt(currentLevelIDPref, currentLevelID);
            GameEventMessage.SendEvent(LevelCompletedUIEvent, gameObject);
        }

        [ContextMenu("LoadCurrentLevel")]
        public void LoadCurrentLevel()
        {
            levelLoader.LoadLevel(levelSettings.smashingLevels[currentLevelID]);
            Debug.Log("Loading level " + currentLevelID);
            GameEventMessage.SendEvent(LevelLoadedUIEvent, gameObject);
        }

        [ContextMenu("Clear progress")]
        public void ClearProgress() => PlayerPrefs.DeleteKey(currentLevelIDPref);
    }
}