using UnityEngine;

namespace Pager
{
    public abstract class DefaultPagerAdapter<TViewHolder> : MonoBehaviour, IPagerAdapter
        where TViewHolder : DefaultPageViewHolder
    {
        [SerializeField] private TViewHolder pagePrefab;

        public virtual IPageViewHolder CreatePage()
        {
            var page = Instantiate(pagePrefab).GetComponent<TViewHolder>();
            page.SetVisibility(false);
            return page;
        }

        public void OnBind(IPageViewHolder page, int pageNumber)
        {
            if (page is DefaultPageViewHolder viewHolder)
                Bind(viewHolder, pageNumber);
        }

        public void OnRecycle(IPageViewHolder page)
        {
            if (page is DefaultPageViewHolder viewHolder)
                Recycle(viewHolder);
        }

        protected virtual void Recycle(DefaultPageViewHolder viewHolder) => viewHolder.SetVisibility(false);

        protected virtual void Bind(DefaultPageViewHolder viewHolder, int pageNumber) => viewHolder.SetVisibility(true);

        public abstract int GetPagesCount();
    }
}