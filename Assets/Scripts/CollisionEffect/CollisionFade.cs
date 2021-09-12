using System.Collections;
using System.Linq;
using UnityEngine;

public class CollisionFade : CollisionEffect
{
    private bool isFading = false;
    private Material[] materials;
    [SerializeField, Range(0f,2f)]
    private float fadeDelay = 0.5f;

    private void Start()
    {
        materials = GetComponentsInChildren<MeshRenderer>().
            SelectMany(mr => mr.materials).
            ToArray();
    }

    public override void OnCollision(Vector3 point)
    {
        if(!isFading)
        {
            isFading = true;
            StartCoroutine(FadeCoroutine());
        }
    }

    private IEnumerator FadeCoroutine()
    {
        float timer = fadeDelay;
        while(timer>0)
        {
            timer-=Time.deltaTime;
            float progress = timer/fadeDelay;
            foreach(var material in materials)
                material.SetFloat("transparency", progress);
            yield return null;
        }
        Destroy(gameObject);
    }
}
