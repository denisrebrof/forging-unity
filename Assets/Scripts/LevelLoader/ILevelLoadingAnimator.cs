using System;

namespace LevelLoading
{
    interface ILevelLoadingTransition
    {
        void StartAnimation(Action onSceneHided = null, Action onCompleted = null);
    }
}
