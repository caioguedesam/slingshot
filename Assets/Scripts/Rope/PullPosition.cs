using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullPosition : MonoBehaviour
{
    private Rope rope;
    private Vector2 preSlingPosition;
    private float maxSlingRadius;

    private void Start() {
        rope = transform.parent.GetComponent<Rope>();
        preSlingPosition = rope.preSlingPoint;
        maxSlingRadius = rope.maxSlingRadius;
    }

    private void Update() {

        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        float distance = Vector2.Distance(transform.position, preSlingPosition);

        if(distance > maxSlingRadius) {
            Vector2 dir = (Vector2)transform.position - preSlingPosition;
            dir *= maxSlingRadius / distance;
            transform.position = preSlingPosition + dir;
        }
    }
}
