using System.Collections.Generic;
using UnityEngine;
using Zenject;
namespace Background
{
    public class BackgroundTest : MonoBehaviour
    {
        [Inject]
        private BackgroundController backgroundController;

        private int currentBackgroundID = 0;
        [SerializeField]
        private List<Background> backgrounds = new List<Background>();

        [ContextMenu("Apply Next")]
        public void ApplyNext()
        {
            if(backgrounds.Count>0)
            {
                currentBackgroundID = (currentBackgroundID+1)%backgrounds.Count;
                backgroundController.ApplySkybox(backgrounds[currentBackgroundID]);
            }
        }
    }
}
