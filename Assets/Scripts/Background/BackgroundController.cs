using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Background
{
    public class BackgroundController : MonoBehaviour
    {
        private BackgroundView backgroundView;

        private Stack<Background> backgroundsStack = new Stack<Background>();

        private bool isSwitchingBackground = false;

        [SerializeField]
        private Material skyboxMat;

        private void Awake()
        {
            RenderSettings.skybox = skyboxMat;
            backgroundView = new BackgroundView(RenderSettings.skybox);
        }

        public void ApplySkybox(Background background)
        {
            if(background!=null)
            {
                backgroundsStack.Push(background);
                if(!isSwitchingBackground)
                    StartCoroutine(ApplySkyboxes());
            }
            else
            {
                Debug.LogError("Trying to apply null background, ignored!");
            }
        }

        private IEnumerator ApplySkyboxes()
        {
            if(backgroundsStack.Count>0)
            {
                isSwitchingBackground = true;
                var backgroundToApply = backgroundsStack.Pop();
                backgroundView.ApplyBackground(backgroundToApply);
                var appearanceCurve = backgroundToApply.appearanceCurve;
                var appearanceTime = backgroundToApply.appearanceTime;
                var timer = 0.0f;
                while(timer<appearanceTime)
                {
                    timer+=Time.deltaTime;
                    backgroundView.SetProgress(appearanceCurve.Evaluate(timer/appearanceTime));
                    yield return null;
                }
                backgroundView.SetProgress(1f);
            }
            isSwitchingBackground = false;
        }

        [Serializable]
        class BackgroundView
        {

            private Material skyboxMaterial;

            //TODO inject/initialize start background
            private Background currentBackground;

            //TODO inject/initialize param names
            [SerializeField]
            private string stateParamName;
            [SerializeField]
            private string destTextureParamName;
            [SerializeField]
            private string sourceTextureParamName;
            [SerializeField]
            private string destColorParamName;
            [SerializeField]
            private string sourceColorParamName;

            public BackgroundView(Material skyboxMaterial) => this.skyboxMaterial=skyboxMaterial;

            private void RollBackBackground()
            {
                if(currentBackground==null)
                    return;
                skyboxMaterial.SetColor(destColorParamName, currentBackground.color);
                skyboxMaterial.SetTexture(destTextureParamName, currentBackground.texture);
                SetProgress(0.0f);
            }

            public void ApplyBackground(Background background)
            {
                RollBackBackground();
                skyboxMaterial.SetColor(sourceColorParamName, background.color);
                skyboxMaterial.SetTexture(sourceTextureParamName, background.texture);
                currentBackground = background;
            }

            public void SetProgress(float progress)
            {
                skyboxMaterial.SetFloat(stateParamName, Mathf.Clamp(0f, 1f, progress));
            }
        }
    }
}
