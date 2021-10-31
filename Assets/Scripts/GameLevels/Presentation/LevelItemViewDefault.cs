using UnityEngine;
using UnityEngine.UI;

namespace GameLevels.Presentation
{
    public class LevelItemViewDefault : LevelsListPagerAdapter.LevelItemView
    {
        [SerializeField] private Text text;
        [SerializeField] private Image background;
        [SerializeField] private Button loadButton;
        [SerializeField] private GameObject checkmark;

        public override void SetActive(bool active)
        {
            gameObject.SetActive(active);
            transform.SetParent(null);
            loadButton.onClick.RemoveAllListeners();
        }

        public override void Bind(int levelNumber, Sprite preview, bool completedState, Transform parent, long levelId)
        {
            transform.SetParent(parent);
            text.text = levelNumber.ToString();
            background.sprite = preview;
            checkmark.SetActive(completedState);
            transform.localScale = Vector3.one;
            loadButton.onClick.AddListener( () => Load(levelId) );
        }

        private void Load(long levelId)
        {
            var levelManager = FindObjectOfType<LevelManager.LevelManager>();
            levelManager.LoadLevel(levelId);
        }
    }
}