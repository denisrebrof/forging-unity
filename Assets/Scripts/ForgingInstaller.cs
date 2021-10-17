using LevelLoading;
using LevelManagement;
using System;
using System.Linq;
using Balance.Data;
using ForgingDomain;
using GameLevels.Data;
using GameLevels.Domain;
using UnityEngine;
using Zenject;

public class ForgingInstaller : MonoInstaller
{
    [SerializeField] 
    private TouchEvent forgingTouchEvent;
    [SerializeField]
    private ForgingPoint forgingPointPrefab;

    public override void InstallBindings()
    {
        var levelManager = FindObjectOfType<LevelManager.LevelManager>();
        Container.BindInterfacesAndSelfTo<LevelManager.LevelManager>().FromInstance(levelManager);

        var levelManagementSettings = Resources.LoadAll<LevelManagementSettings>(String.Empty).FirstOrDefault();
        Container.Bind<LevelManagementSettings>().FromInstance(levelManagementSettings);
        
        var levelImageRepository = Resources.LoadAll<GameLevelImageStubRepository>(String.Empty).FirstOrDefault();
        Container.Bind<ILevelItemImageRepository>().FromInstance(levelImageRepository);
        
        var levelsRepository = Resources.LoadAll<GameLevelsHardcodedRepository>(String.Empty).FirstOrDefault();
        Container.Bind<IGameLevelsRepository>().FromInstance(levelsRepository);
        
        var balanceRepository = Resources.LoadAll<PlayerPrefsBalanceRepository>(String.Empty).FirstOrDefault();
        Container.Bind<IBalanceRepository>().FromInstance(balanceRepository);

        var levelLoadingAnimation = FindObjectOfType<CameraRotateLevelLoadingAnimation>();
        Container.BindInterfacesAndSelfTo<CameraRotateLevelLoadingAnimation>().FromInstance(levelLoadingAnimation).AsSingle();

        var levelLoader = FindObjectOfType<GameLevels.Presentation.LevelLoader>();
        Container.Bind<GameLevels.Presentation.LevelLoader>().FromInstance(levelLoader);
        
        var levelsUseCases = new GameLevelsUseCases();
        Container.Bind<GameLevelsUseCases>().FromInstance(levelsUseCases);

        var hMapController = FindObjectOfType<HeatingMapController>();
        Container.Bind<HeatingMapController>().FromInstance(hMapController);

        var hammerSpawner = FindObjectOfType<HammerSpawner>();
        Container.Bind<HammerSpawner>().FromInstance(hammerSpawner);

        Container.Bind<TouchEvent>().FromInstance(forgingTouchEvent);

        Container.Bind<Camera>().FromInstance(Camera.main).WhenInjectedInto<ForgingPoint>();

        var forgeRaycaster = FindObjectOfType<ForgeRaycaster>();
        Container.Bind<ForgeRaycaster>().FromInstance(forgeRaycaster);

        var fpController = FindObjectOfType<ForgingPointController>();
        Container.Bind<ForgingPointController>().FromInstance(fpController);
        Container.BindFactory<ForgingPoint.Settings, ForgingPoint, ForgingPoint.Factory>().FromComponentInNewPrefab(forgingPointPrefab);
    }
}
