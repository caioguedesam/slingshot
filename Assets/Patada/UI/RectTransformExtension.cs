using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Patada.Animation.Extensions {
    [RequireComponent(typeof(RectTransform))]
    public class RectTransformExtension : MonoBehaviour {
        private RectTransform rt = null;

        public void Awake() {
            rt = GetComponent<RectTransform>();
        }

        public void SetPosition(string positionString) {
            var tokens = positionString.Split(',');
            rt.anchoredPosition = new Vector2(float.Parse(tokens[0]), float.Parse(tokens[1]));
        }
    }
}
