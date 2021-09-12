using UnityEngine;

public class ParticleDestroy : MonoBehaviour
{
    private ParticleSystem[] particlesSystem;

    private void Start()
    {
        particlesSystem = this.GetComponentsInChildren<ParticleSystem>();
    }

    private void FixedUpdate()
    {
        bool particlesStopped = true;
        foreach (var ps in particlesSystem)
        {
            if ((ps != null) && (ps.particleCount != 0 || ps.isPlaying))
                particlesStopped = false;
        }
        if (particlesStopped)
            Destroy(this.gameObject);
    }
}
