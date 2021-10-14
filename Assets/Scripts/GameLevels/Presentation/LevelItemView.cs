using UnityEngine;
using UnityEngine.UI;

namespace GameLevels.Presentation
{
    public class LevelItemView : LevelsListPagerAdapter.LevelItemView
    {
        [SerializeField] private Text text;
        [SerializeField] private Image background;
        [SerializeField] private GameObject checkmark;

        public override void SetActive(bool active)
        {
            gameObject.SetActive(active);
            transform.SetParent(null);
        }

        public override void Bind(int levelNumber, Sprite preview, bool completedState, Transform parent)
        {
            transform.SetParent(parent);
            text.text = levelNumber.ToString();
            background.sprite = preview;
            checkmark.SetActive(completedState);
            transform.localScale = Vector3.one;
        }
    }
}