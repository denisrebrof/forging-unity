using GameLevels.Domain;
using LevelLoading;
using UnityEngine;
using Zenject;

public class LevelLoaderTest : MonoBehaviour
{
    [SerializeField,Inject]
    private GameLevels.Presentation.LevelLoader levelLoader;
    [SerializeField]
    private GameLevel level;

    [ContextMenu("Load Level")]
    public void LoadLevel()
    {
        levelLoader.LoadLevel(level);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
            LoadLevel();
    }

}
