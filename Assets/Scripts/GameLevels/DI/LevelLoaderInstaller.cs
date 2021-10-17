using UnityEngine;
using Zenject;

namespace LevelLoading
{
    public class LevelLoaderInstaller : MonoInstaller
    {
        [SerializeField, Header("Level Loader instance")]
        private GameLevels.Presentation.LevelLoader levelLoader;
        [SerializeField, Header("AnimatorLevelLoaderAnimator")]
        private CameraRotateLevelLoadingAnimation animationController;
        public override void InstallBindings()
        {
            //Bind BackgroundController as single
            
        }
    }
}
