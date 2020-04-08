using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopePositioning : MonoBehaviour
{
    private Rope rope;
    private Transform startPoint;
    private Transform endPoint;

    private bool isMidPositioning = false;

    private void Start() {
        rope = GetComponent<Rope>();
        startPoint = rope.startPoint;
        endPoint = rope.endPoint;
    }

    private void Update() {
        Vector2 mousePosWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (!isMidPositioning && !rope.objectLanded && Input.GetMouseButtonDown(0)) {
            isMidPositioning = true;
            startPoint.position = mousePosWorld;
        }

        if(isMidPositioning && !rope.objectLanded && Input.GetMouseButton(0)) {
            endPoint.position = mousePosWorld;
        }

        if(isMidPositioning && !rope.objectLanded && Input.GetMouseButtonUp(0)) {
            isMidPositioning = false;
            endPoint.position = mousePosWorld;
        }
    }
}
