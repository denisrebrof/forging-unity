using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ForgingPointController : MonoBehaviour
{
    [Inject]
    private TouchEvent touchEvent;
    [SerializeField]
    private ForgingPoint.Settings pointSettings;
    [Inject]
    private ForgingPoint.Factory pointFactory;

    public ForgingPoint currentPoint;

    private void Start()
    {
        touchEvent.OnTouchingStateChanged+=(state) => { if(state) SpawnPoint(); };
    }

    private void SpawnPoint()
    {
        currentPoint = pointFactory.Create(pointSettings);
    }
}
