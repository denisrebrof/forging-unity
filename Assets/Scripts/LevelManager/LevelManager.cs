using System;
using Doozy.Engine;
using GameLevels.Domain;
using GameLevels.Presentation;
using UniRx;
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

        private readonly Subject<GameLevel> _selectedLevel = new Subject<GameLevel>(); 

        private void Start() => LoadCurrentLevel();

        [ContextMenu("CompleteLevel")]
        public void CompleteLevel()
        {
            var currentLevel = _levelsUseCases.GetCurrentLevel();
            _levelsUseCases.CompleteLevel(currentLevel.id);
            GameEventMessage.SendEvent(LevelCompletedUIEvent, gameObject);
        }

        public IObservable<GameLevel> GetLoadedLevel() => _selectedLevel;

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
            _selectedLevel.OnNext(level);
        }
    }
}