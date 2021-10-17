using UnityEngine;

namespace Pager
{
    public class DefaultPageViewHolder : MonoBehaviour, IPageViewHolder
    {
        [SerializeField] private RectTransform target;
        private GameObject _targetGameObject;

        private void Awake()
        {
            if (target == null)
                gameObject.TryGetComponent(out target);

            if (target != null)
                _targetGameObject = target.gameObject;
            else
                Debug.LogError("Page view holder target transform is undefined!");
        }

        public void SetVisibility(bool visible) => _targetGameObject.SetActive(visible);

        public RectTransform RectTransform => target;
    }
}