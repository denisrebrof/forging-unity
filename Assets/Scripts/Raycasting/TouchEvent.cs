using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchEvent : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Action<bool> OnTouchingStateChanged;
    private bool isTouching = false;

    public void OnDrag(PointerEventData eventData) { }
    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData) => SetTouchingState(true);
    void IEndDragHandler.OnEndDrag(PointerEventData eventData) => SetTouchingState(false);

    private void SetTouchingState(bool state)
    {
        if(isTouching!=state)
            OnTouchingStateChanged?.Invoke(state);
        isTouching = state;
    }
}
