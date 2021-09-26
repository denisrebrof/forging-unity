using UnityEngine;

namespace Pager
{
    public interface IPager
    {
        RectTransform CreatePage();
        void OnBind(GameObject page, int pageNumber);
        void OnRecycle(GameObject page);
        int GetPagesCount();
    }
}
