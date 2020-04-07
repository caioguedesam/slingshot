using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollUp : MonoBehaviour {
    [SerializeField] private ScrollRect target = null;
    [SerializeField] private float speed = 10f;

    public void Scroll() {
        target.verticalNormalizedPosition = Mathf.Clamp(target.verticalNormalizedPosition, 0.0001f, 0.9999f);
        target.velocity = new Vector2(0f,-speed);
    }
}
