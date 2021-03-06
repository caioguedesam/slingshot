﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopePositioning : MonoBehaviour
{
    private Rope rope;
    private Transform startPoint;
    private Transform endPoint;

    [SerializeField] private bool isMidPositioning = false;
    private bool canReposition = true;
    [SerializeField] private bool disablePositioning = false;

    [Range(0f, 1f)] public float positioningCooldown = 0.15f;

    private void Start() {
        rope = GetComponent<Rope>();
        startPoint = rope.startPoint;
        endPoint = rope.endPoint;
    }

    private void Update() {
        if(!disablePositioning) {
            Vector2 mousePosWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (canReposition) {
                if (!isMidPositioning && !rope.objectLanded && Input.GetMouseButtonDown(0)) {
                    isMidPositioning = true;
                    startPoint.position = mousePosWorld;
                    endPoint.position = mousePosWorld;
                }

                if (isMidPositioning && Input.GetMouseButton(0)) {
                    endPoint.position = mousePosWorld;
                }

                if (isMidPositioning && Input.GetMouseButtonUp(0)) {
                    isMidPositioning = false;
                    endPoint.position = mousePosWorld;
                }
            }
        }
    }

    public IEnumerator PositioningCooldown() {
        canReposition = false;

        yield return new WaitForSeconds(positioningCooldown);

        canReposition = true;
    }

    public void StartCooldown() {
        if (canReposition) StartCoroutine(PositioningCooldown());
    }
}
