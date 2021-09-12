using System;
using UnityEngine;
namespace Background
{
    [Serializable]
    public class Background
    {
        public Texture2D texture;
        public Color color;
        [Range(0f, 2f)]
        public float appearanceTime = 1f;
        public AnimationCurve appearanceCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    }
}

