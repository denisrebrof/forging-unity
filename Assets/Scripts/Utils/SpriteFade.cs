using System.Collections;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class SpriteFade : MonoBehaviour
{
    private SpriteRenderer target;
    [SerializeField]
    private AnimationCurve fadeCurve;
    [SerializeField]
    private float duration=1f;

    private void Start()
    {
        target = GetComponent<SpriteRenderer>();
        StartCoroutine(FadeCoroutine());
    }

    private IEnumerator FadeCoroutine()
    {
        float timer = duration;
        while(timer>0)
        {
            timer-=Time.deltaTime;
            SetAlpha(fadeCurve.Evaluate(1f-timer/duration));
            yield return null;
        }
        SetAlpha(1);

        void SetAlpha(float alpha)
        {
            var color = target.color;
            color.a = alpha;
            target.color = color;
        }
    }
}
