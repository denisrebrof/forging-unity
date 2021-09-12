using System;
using UnityEngine;
using Zenject;

public class ForgeRaycaster : Raycaster
{
    [SerializeField]
    private TouchEvent touchEvent;
    [Inject]
    private ForgingPointController fpController;
    [Inject]
    private HammerSpawner spawner;

    private bool raycastState;
    public Action<bool> raycastStateChanged;

    [SerializeField]
    private float raycastTimerDuration = 2f;
    private float raycastTimer = 0f;

    private void SetRaycastState(bool state)
    {
        if(raycastState!=state)
            raycastStateChanged?.Invoke(state);
        raycastState = state;
    }

    protected override void OnRaycasted(RaycastHit hit)
    {
        spawner.SpawnHammer(hit.point);
        raycastTimer = 0f;
    }

    protected override void Start()
    {
        base.Start();
        touchEvent.OnTouchingStateChanged+=(touching) => SetRaycasting(touching);
    }

    protected override void Update()
    {
        base.Update();
        raycastTimer+=Time.deltaTime;
        SetRaycastState(raycastTimer<raycastTimerDuration);
    }

    protected override Vector2 GetRaycastPosition() => fpController.currentPoint.PointPos;
}
