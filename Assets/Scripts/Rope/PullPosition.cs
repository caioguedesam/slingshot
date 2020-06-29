using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullPosition : MonoBehaviour
{
    private Rope rope;
    private Vector2 preSlingPosition;
    private float maxSlingRadius;

    private void Start() {
        rope = GetComponentInParent<Rope>();
        preSlingPosition = rope.preSlingPoint;
        maxSlingRadius = rope.maxSlingRadius;
    }

    private void Update() {

        preSlingPosition = rope.preSlingPoint;

        /*transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        float distance = Vector2.Distance(transform.position, preSlingPosition);

        if(distance > maxSlingRadius) {
            Vector2 dir = (Vector2)transform.position - preSlingPosition;
            dir *= maxSlingRadius / distance;
            transform.position = preSlingPosition + dir;
        }*/

        Vector2 mousePosWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 distance = mousePosWorld - preSlingPosition;
        distance = Vector2.ClampMagnitude(distance, maxSlingRadius);
        transform.position = preSlingPosition + distance;
    }
}
