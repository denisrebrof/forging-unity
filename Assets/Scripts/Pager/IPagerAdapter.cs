using UnityEngine;

namespace Pager
{
    public interface IPagerAdapter
    {
        IPageViewHolder CreatePage();
        void OnBind(IPageViewHolder page, int pageNumber);
        void OnRecycle(IPageViewHolder page);
        int GetPagesCount();
    }
}
