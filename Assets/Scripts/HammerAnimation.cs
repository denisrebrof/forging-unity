using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class HammerAnimation : MonoBehaviour
{
    [SerializeField]
    private Animator hammerAnimator;
    [Inject]
    private TouchEvent touchEvent;

    private void Start()
    {
        touchEvent.OnTouchingStateChanged+=OnTouchingStateChanged;
    }

    private void OnTouchingStateChanged(bool state)
    {
        Debug.Log("state: " + state);
        hammerAnimator.SetBool("Shown", !state);
    }
}
