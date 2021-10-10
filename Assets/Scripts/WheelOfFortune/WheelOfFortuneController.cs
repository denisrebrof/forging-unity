using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class WheelOfFortuneController : MonoBehaviour
{
    [SerializeField]
    private Vector2Int TurnRandomRange = new Vector2Int(1, 3);
    public Button PlayButton;
    public RectTransform Wheel;
    public float Duration = 3f;
    
    public event Action<int> OnIndexDropped;

    private Vector3 targetRotation;
    private int targetTurns;
    private int randomResult;
    
    private Tweener rotationTweener = null;
    
    private void Awake()
    {
        PlayButton.onClick.AddListener(OnPlayPressed);

        OnIndexDropped += (val) => {Debug.Log($"Wheel value: {val} ({val + 1})"); };
    }

    private void OnEnable()
    {
        PlayButton.interactable = true;
    }

    private void OnPlayPressed()
    {
        PlayButton.interactable = false;

        Wheel.rotation = Quaternion.identity;
        
        randomResult = Random.Range(0, 8);
        targetTurns = Random.Range(TurnRandomRange.x, TurnRandomRange.y);
        targetRotation = new Vector3(0f, 0f, targetTurns * 360f + randomResult * 45f + Random.Range(-20f, 20f) + 15f);
        
        rotationTweener = Wheel.DORotate(targetRotation, Duration, RotateMode.FastBeyond360);
        rotationTweener.onComplete += () =>
        {
            rotationTweener = Wheel.DORotate(new Vector3(0f, 0f, Wheel.rotation.eulerAngles.z - 15f), 2f, RotateMode.FastBeyond360);
            rotationTweener.onComplete += () =>
            {
                OnIndexDropped?.Invoke(randomResult);
                PlayButton.interactable = true;
                rotationTweener = null;
            };
        };
    }

    private void OnDisable()
    {
        rotationTweener?.Kill();
        rotationTweener = null;
    }
}
