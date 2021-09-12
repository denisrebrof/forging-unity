using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugSceneReload : MonoBehaviour
{
    public void Reload()
    {
        SceneManager.LoadScene(0);
    }
}
