using System;
using System.Collections;
using UnityEngine;
using Zenject;

public class ForgingPoint : MonoBehaviour
{
    [Inject]
    private ForgeRaycaster raycaster;
    [Inject]
    private TouchEvent touchEvent;

    [SerializeField]
    private Animator animator;
    [Inject]
    private Camera camera;

    private Settings settings;

    private bool isDestroying = false;

    [Inject]
    public void Construct(ForgeRaycaster raycaster, TouchEvent touchEvent, Camera camera, Settings settings)
    {
        this.raycaster=raycaster;
        this.touchEvent=touchEvent;
        this.camera=camera;
        this.settings=settings;
    }

    private void Start()
    {
        touchEvent.OnTouchingStateChanged+=OnTouchingStateChanged;
        raycaster.raycastStateChanged+=OnRaycastStateChanged;
        StartCoroutine(PointLifecycle());
    }

    private void OnTouchingStateChanged(bool state)
    {
        if(!state)
            Destroy();
    }
    private void OnRaycastStateChanged(bool state)
    {
        animator.SetBool("Forging", state);
    }

    private IEnumerator PointLifecycle()
    {
        yield return FlyToFinger();
        yield return FollowFinger();
        yield return Die();
    }

    private IEnumerator FlyToFinger()
    {
        float timer = settings.floatingTime;

        while(timer>0 && !isDestroying)
        {
            timer-=Time.deltaTime;

            var fingerPoint = GetWorldPoint(Input.mousePosition);
            var targetPoint = GetWorldPoint(PointPos);
            transform.position = Vector3.Lerp(fingerPoint, targetPoint, 1-timer/settings.floatingTime);
            yield return null;
        }

        if(!isDestroying)
            animator.SetTrigger("Borned");
    }
    private IEnumerator FollowFinger()
    {
        while(!isDestroying)
        {
            transform.position = GetWorldPoint(PointPos);
            yield return null;
        }
    }

    private IEnumerator Die()
    {
        animator.SetTrigger("Die");
        yield return settings.dieTime;
        Destroy(gameObject);
    }

    private void Destroy()
    {
        isDestroying = true;
        touchEvent.OnTouchingStateChanged-=OnTouchingStateChanged;
        raycaster.raycastStateChanged-=OnRaycastStateChanged;
    }

    public Vector3 PointPos => Input.mousePosition + (Vector3.up* settings.yOffset);
    private Vector3 GetWorldPoint(Vector3 screenPoint)
    {
        return camera.ScreenPointToRay(screenPoint).GetPoint(settings.zOffset);
    }

    [Serializable]
    public class Settings
    {
        [Header("World Offsets")]
        public float zOffset = 0.5f;
        public float yOffset = 100;
        [Header("Timers")]
        public float floatingTime = 0.5f;
        public float dieTime = 0.5f;
    }

    public class Factory : PlaceholderFactory<Settings, ForgingPoint>{ }
}
