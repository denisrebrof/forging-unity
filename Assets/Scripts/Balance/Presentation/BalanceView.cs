using System;
using System.Collections;
using Balance.Domain;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Balance.Presentation
{
    public class BalanceView : MonoBehaviour
    {
        [SerializeField] private AnimationCurve scaleCurve = new AnimationCurve();
        [SerializeField] private float updateTime = 1f;
        [SerializeField] private Text balanceText;

        [Inject] private BalanceUseCases _balanceUseCases;

        private IDisposable updateBalanceViewDisposable;

        void Start()
        {
            updateBalanceViewDisposable = _balanceUseCases.GetBalanceFlow().Subscribe(UpdateBalanceView);
        }

        private void UpdateBalanceView(int balance)
        {
            StopAllCoroutines();
            StartCoroutine(UpdateBalanceCoroutine(balance));
        }

        private void OnDestroy()
        {
            updateBalanceViewDisposable.Dispose();
            StopAllCoroutines();
        }

        private IEnumerator UpdateBalanceCoroutine(int balance)
        {
            var timer = updateTime;
            var textUpdated = false;
            while (timer > 0)
            {
                yield return null;
                timer -= Time.deltaTime;
                var progress = timer / updateTime;
                transform.localScale = scaleCurve.Evaluate(1f - progress) * Vector3.one;
                if (timer < 0.5f && !textUpdated)
                {
                    balanceText.text = balance.ToString();
                    textUpdated = true;
                }
            }

            transform.localScale = Vector3.one;
        }
    }
}