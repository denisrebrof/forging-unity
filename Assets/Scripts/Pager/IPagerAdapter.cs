using UnityEngine;

namespace Pager
{
    public interface IPagerAdapter
    {
        RectTransform CreatePage();
        void OnBind(GameObject page, int pageNumber);
        void OnRecycle(GameObject page);
        int GetPagesCount();
    }
}
