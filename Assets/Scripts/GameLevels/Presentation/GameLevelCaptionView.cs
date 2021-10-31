using System;
using GameLevels.Domain;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace GameLevels.Presentation
{
    public class GameLevelCaptionView : MonoBehaviour
    {
        [SerializeField] private Text caption;
        [SerializeField] private String prefix = "Level ";

        [Inject] private LevelManager.LevelManager _levelManager;

        private IDisposable _updateCaptionDisposable;

        void Start()
        {
            _updateCaptionDisposable = _levelManager.GetLoadedLevel().Subscribe(UpdateCaptionText);
        }

        private void UpdateCaptionText(GameLevel level)
        {
            caption.text = prefix + level.number;
        }

        private void OnDestroy()
        {
            _updateCaptionDisposable.Dispose();
        }
    }
}