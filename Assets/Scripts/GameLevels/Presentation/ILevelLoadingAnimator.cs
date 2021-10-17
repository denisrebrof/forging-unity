using System;

namespace LevelLoading
{
    interface ILevelLoadingTransition
    {
        void StartAnimation(Action onSceneHidden = null, Action onCompleted = null);
    }
}
