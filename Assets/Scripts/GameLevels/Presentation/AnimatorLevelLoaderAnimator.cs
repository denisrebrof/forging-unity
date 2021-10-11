using System;
using UnityEngine;

namespace LevelLoading
{
    [RequireComponent(typeof(Animator))]
    public class AnimatorLevelLoaderAnimator : MonoBehaviour, ILevelLoadingTransition
    {
        private Animator cameraAnimator;
        private Action onSceneHided;
        private Action onCompleted;

        private void Start() => cameraAnimator = GetComponent<Animator>();

        public void StartAnimation(Action onSceneHidden = null, Action onCompleted = null)
        {
            this.onSceneHided = onSceneHidden;
            this.onCompleted = onCompleted;
            cameraAnimator.SetTrigger("SwitchLevel");
        }

        //Apply from animation
        public void OnSceneHided() => onSceneHided?.Invoke();
        public void OnCompleted() => onCompleted?.Invoke();
    }
}
