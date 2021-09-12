using System;
using System.Collections;
using UnityEngine;
namespace LevelLoading
{
    public class CameraRotateLevelLoadingAnimation : MonoBehaviour, ILevelLoadingTransition
    {
        [SerializeField]
        private Transform cameraTransform;
        private float defaultRotation;
        [SerializeField, Range(0f, 2f)]
        private float rotateDuration = 1f;

        private void Awake()
        {
            cameraTransform = GameObject.FindWithTag("MainCamera").transform;
            defaultRotation = cameraTransform.rotation.eulerAngles.y;
        }

        public void StartAnimation(Action onSceneHided = null, Action onCompleted = null)
        {
            StopAllCoroutines();
            StartCoroutine(LevelLoadAnimation(onSceneHided, onCompleted));
        }

        private IEnumerator LevelLoadAnimation(Action onSceneHided = null, Action onCompleted = null)
        {
            float halfRotateDuration = rotateDuration/2f;
            float timer = halfRotateDuration;
            var camRotation = cameraTransform.rotation.eulerAngles;
            while(timer>0)
            {
                yield return null;
                timer-=Time.deltaTime;
                camRotation.y = Mathf.Lerp(defaultRotation, defaultRotation+180f, 1f-(timer/halfRotateDuration));
                cameraTransform.rotation = Quaternion.Euler(camRotation);
            }
            onSceneHided.Invoke();
            timer = halfRotateDuration;
            while(timer>0)
            {
                yield return null;
                timer-=Time.deltaTime;
                camRotation.y = Mathf.Lerp(defaultRotation+180f, defaultRotation+360f, 1f-(timer/halfRotateDuration));
                cameraTransform.rotation = Quaternion.Euler(camRotation);
            }
            camRotation.y = defaultRotation;
            cameraTransform.rotation = Quaternion.Euler(camRotation);
            onCompleted.Invoke();
        }
    }
}

