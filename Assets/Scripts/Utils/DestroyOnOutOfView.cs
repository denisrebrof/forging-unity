using UnityEngine;

public class DestroyOnOutOfView : MonoBehaviour
{
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
