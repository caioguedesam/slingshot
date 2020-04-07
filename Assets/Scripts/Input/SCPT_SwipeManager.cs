using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Patada.Events;

// Component to manage swipe and touch input.
// Author: Caio Guedes - 03/2020
public class SCPT_SwipeManager : MonoBehaviour {
    [Header("Swipe variables")]
    // Should swipes be processed?
    [SerializeField] private bool swipesBlocked = false;
    // Minimum swipe threshold
    [SerializeField] private float minDistanceForSwipe = 20f;
    // Percentage of screen for max swipe
    [SerializeField] [Range(0f, 1f)] private float swipeOffset = 0.65f;
    // Is the swipe currently being done?
    [HideInInspector] public bool isMidSwipe = false;
    // Finger positions during swipe
    [HideInInspector] public Vector2 fingerEndPosition, fingerStartPosition;
    // Swipe timer variables
    private float timerStart = 0f, timerEnd = 0f, swipeDuration = 0f;
    // Max height of swipe
    [HideInInspector] public float maxHeight = 0f;
    [Space(2)]

    [Header("Touch Variables")]
    // Should touches be processed?
    [SerializeField] private bool touchesBlocked = false;
    // Touch distance tolerance
    [SerializeField] private float touchDistanceTolerance = 10f;
    [Space(2)]

    [Header("Events")]
    // Event to fire when swipe is done (with swipe direction)
    [SerializeField] private SCOB_SwipeEvent swipeEvent;
    // Event to fire when swipe starts
    [SerializeField] private SCOB_Vec3Event swipeStartEvent;
    // Event to fire when swipe is cancelled
    [SerializeField] private SCOB_BaseEvent swipeCancelled;
    // Event to fire incomplete swipes
    [SerializeField] private SCOB_SwipeEvent incompleteSwipeEvent;
    // Event to fire when simple touch is made
    [SerializeField] private SCOB_Vec3Event touchEvent;

    private void Start() {
        maxHeight = Screen.height * swipeOffset;

        // Touch maximum tolerance should never be above minimum swipe tolerance
        touchDistanceTolerance = Mathf.Min(touchDistanceTolerance, minDistanceForSwipe);
    }

    private void Update() {
        // When player begins swipe, store positions
        if (Input.GetMouseButtonDown(0) && !isMidSwipe) {
            fingerStartPosition = fingerEndPosition = Input.mousePosition;
            isMidSwipe = true;
            timerStart = Time.time;

            // Fire swipe start event if swipes are not blocked
            if (!swipesBlocked) {
                swipeStartEvent.Raise(fingerStartPosition);
                Debug.Log("Raised " + swipeStartEvent.name + " " + fingerStartPosition);
            }
        }
        // Raise incomplete swipes
        if(Input.GetMouseButton(0) && isMidSwipe) {
            DetectIncompleteSwipe();
        }
        // When player ends swipe, store positions again
        if (Input.GetMouseButtonUp(0) && isMidSwipe) {
            fingerEndPosition = Input.mousePosition;
            isMidSwipe = false;
            timerEnd = Time.time;

            // Detect a swipe. If a swipe is not detected, detect a touch.
            if (!DetectSwipe()) DetectTouch();
        }
    }

    // Stops input if pointer is over UI element (can be extended)
    private bool IsMouseOverUI() {
        return EventSystem.current.IsPointerOverGameObject();
    }

    // Using finger start/end data, check if swipe is valid and register if so.
    private bool DetectSwipe() {
        // Only perform these tests if swipes can be processed
        if(!swipesBlocked) {
            // Raise cancel event if finger is released or swipe distance isn't met
            if (!isSwipeDistanceMet()) {
                swipeDuration = timerEnd - timerStart;
                swipeCancelled.Raise();
                return false;
            }
            // Send swipe event with swipe direction when swipe distance is met
            else {
                swipeDuration = timerEnd - timerStart;

                Vector3 direction = fingerEndPosition - fingerStartPosition;
                // Fires normalized swipe based on screen height
                direction = NormalizeToHeight(direction, maxHeight);
                swipeEvent.Raise(new Swipe(direction, swipeDuration));
                Debug.Log("Swipe direction: " + direction + ", duration: " + swipeDuration + "sec");
                return true;
            }
        }
        return false;
    }

    // Register an incomplete swipe.
    private bool DetectIncompleteSwipe() {

        if(!swipesBlocked) {
            Vector3 direction = Input.mousePosition - (Vector3)fingerStartPosition;
            // Fires normalized swipe based on screen height
            direction = NormalizeToHeight(direction, maxHeight);
            incompleteSwipeEvent.Raise(new Swipe(direction, Time.time - timerStart));
            return true;
        }

        return false;
    }

    // Register a touch if it's valid.
    private bool DetectTouch() {
        // Only perform these tests if touches can be processed
        if(!touchesBlocked && Vector2.Distance(fingerEndPosition, fingerStartPosition) <= touchDistanceTolerance) {
            // Raise rouch event at finger start position
            Vector3 touchPos = new Vector3(fingerStartPosition.x, fingerStartPosition.y, 0f);
            touchEvent.Raise(touchPos);
            Debug.Log("Touch at " + touchPos);

            return true;
        }
        return false;
    }

    // Method to normalize swipe vector based on screen height and offset
    public Vector3 NormalizeToHeight(Vector3 dir, float maxHeight) {
        dir = dir / maxHeight;
        return Vector3.Min(dir, Vector3.one);
    }

    // Verifies if current swipe goes over minimum swipe threshold
    private bool isSwipeDistanceMet() {
        return VerticalMoveDistance() >= minDistanceForSwipe || HorizontalMoveDistance() >= minDistanceForSwipe;
    }

    // Returns swipe vertical movement distance
    private float VerticalMoveDistance() {
        return Mathf.Abs(fingerEndPosition.y - fingerStartPosition.y);
    }
    // Returns swipe horizontal movement distance
    private float HorizontalMoveDistance() {
        return Mathf.Abs(fingerEndPosition.x - fingerStartPosition.x);
    }

    public void BlockSwipes() {
        swipesBlocked = true;
    }
}

// Swipe class with direction and duration
[System.Serializable]
public class Swipe {
    public Vector3 swipe;
    public float duration;

    public Swipe(Vector3 _swipe, float _duration) {
        swipe = _swipe;
        duration = _duration;
    }
}
