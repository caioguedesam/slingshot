using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patada.Events;

public class Rope : MonoBehaviour
{
    // References
    // Line renderer ref
    private LineRenderer lineRenderer;
    // Collider ref
    private EdgeCollider2D coll;

    // Rope variables
    // List of rope segments for line renderer to draw
    [HideInInspector] public List<RopeSegment> ropeSegments = new List<RopeSegment>();
    // Positions of points in rope segments
    private List<Vector2> currentPositions = new List<Vector2>();
    // Rope segment length
    [SerializeField] private float ropeSegLen = 0.25f;

    // Number of points
    public int segmentCount = 35;
    // Line width to draw rope
    private float lineWidth = 0.1f;

    // Sling variables
    private Vector2 preSlingPoint;
    private Vector2 slingDirection;
    [ReadOnly] [SerializeField] private bool isPreparingSling = false;
    [ReadOnly][SerializeField] private bool objectLanded = false;

    [Header("Rope extremes")]
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    [SerializeField] private Transform middlePoint;
    [Space(5)]

    [Header("Events")]
    [SerializeField] private SCOB_Vec3Event slingEvent;
    [SerializeField] private SCOB_BaseEvent prepareSlingEvent;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        coll = GetComponent<EdgeCollider2D>();
        Vector3 ropeStartPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        for(int i = 0; i < segmentCount; i++) {
            ropeSegments.Add(new RopeSegment(ropeStartPoint));
            ropeStartPoint.y -= ropeSegLen;
        }

        InitializePositionList();
    }

    private void InitializePositionList() {
        for(int i = 0; i < segmentCount; i++) {
            currentPositions.Add(Vector2.zero);
        }
    }

    // Update is called once per frame
    void Update()
    {
        DrawRope();
        //Time.timeScale = .1f;
        UpdateCollider();
    }

    private void FixedUpdate() {
        Simulate();
    }

    private void UpdateCollider() {
        coll.points = currentPositions.ToArray();
    }

    private void Simulate() {

        // SIMULATION
        Vector2 gravityForce = new Vector2(0f, -1f);

        // Verlet integration for calculating new position
        for(int i = 0; i < segmentCount; i++) {
            RopeSegment firstSegment = ropeSegments[i];
            Vector2 velocity = firstSegment.posNow - firstSegment.posOld;

            firstSegment.posOld = firstSegment.posNow;
            firstSegment.posNow += velocity;

            // Adding gravity force for movement calculation
            firstSegment.posNow += gravityForce * Time.deltaTime;

            ropeSegments[i] = firstSegment;
            currentPositions[i] = firstSegment.posNow;
        }

        // CONSTRAINTS
        for(int i = 0; i < 50; i++) {
            ApplyConstraints();
        }

        CheckForSling();
    }

    private void ApplyConstraints() {
        //Constrant to First Point 
        RopeSegment firstSegment = ropeSegments[0];
        firstSegment.posNow = startPoint.position;
        ropeSegments[0] = firstSegment;


        //Constrant to Second Point 
        RopeSegment endSegment = ropeSegments[ropeSegments.Count - 1];
        endSegment.posNow = endPoint.position;
        ropeSegments[ropeSegments.Count - 1] = endSegment;

        RopeSegment middleSegment = ropeSegments[(segmentCount - 1) / 2];
        // Use something like this for slingshot fx
        if (objectLanded && Input.GetMouseButton(0)) {
            if(!isPreparingSling) {
                isPreparingSling = true;
                prepareSlingEvent.Raise();
            }

            middleSegment.posNow = middlePoint.position;
            slingDirection = preSlingPoint - middleSegment.posNow;
            ropeSegments[(segmentCount - 1) / 2] = middleSegment;
        }
        else {
            preSlingPoint = middleSegment.posNow;
        }

        for (int i = 0; i < segmentCount - 1; i++) {
            RopeSegment firstSeg = ropeSegments[i];
            RopeSegment secondSeg = ropeSegments[i + 1];

            float dist = (firstSeg.posNow - secondSeg.posNow).magnitude;
            float error = Mathf.Abs(dist - ropeSegLen);
            Vector2 changeDir = Vector2.zero;

            if (dist > ropeSegLen) {
                changeDir = (firstSeg.posNow - secondSeg.posNow).normalized;
            }
            else if (dist < ropeSegLen) {
                changeDir = (secondSeg.posNow - firstSeg.posNow).normalized;
            }

            Vector2 changeAmount = changeDir * error;
            if (i != 0) {
                firstSeg.posNow -= changeAmount * 0.5f;
                ropeSegments[i] = firstSeg;
                secondSeg.posNow += changeAmount * 0.5f;
                ropeSegments[i + 1] = secondSeg;
            }
            else {
                secondSeg.posNow += changeAmount;
                ropeSegments[i + 1] = secondSeg;
            }
        }
    }

    private void DrawRope() {
        // Setting line width
        float lineWidth = this.lineWidth;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        // Setting rope positions for line renderer to draw
        Vector3[] ropePositions = new Vector3[segmentCount];
        for(int i = 0; i < segmentCount; i++) {
            ropePositions[i] = ropeSegments[i].posNow;
        }

        // Passing info to renderer
        lineRenderer.positionCount = ropePositions.Length;
        lineRenderer.SetPositions(ropePositions);
    }

    public void LandedOnRope() {
        objectLanded = true;
    }

    public void CheckForSling() {
        // Just slinged object away
        if(objectLanded && Input.GetMouseButtonUp(0)) {
            objectLanded = false;
            slingEvent.Raise(slingDirection);
            Debug.Log("Raised sling event");
        }
    }

    public struct RopeSegment {
        public Vector2 posNow;
        public Vector2 posOld;

        public RopeSegment(Vector2 pos) {
            posNow = posOld = pos;
        }
    }
}

/*
 private void ApplyConstraints() {

        RopeSegment firstSegment = ropeSegments[0];
        firstSegment.posNow = startPoint.position;
        ropeSegments[0] = firstSegment;

        RopeSegment lastSegment = ropeSegments[segmentCount - 1];
        lastSegment.posNow = endPoint.position;
        ropeSegments[segmentCount - 1] = lastSegment;

        // Use something like this for slingshot fx
        if(objectLanded && Input.GetMouseButton(0)) {
            RopeSegment middleSegment = ropeSegments[(segmentCount - 1) / 2];
            middleSegment.posNow = middlePoint.position;
            ropeSegments[(segmentCount - 1) / 2] = middleSegment;
        }

        CheckForSling();

        // Two points in the rope must always keep a certain distance apart
        for (int i = 0; i < segmentCount - 1; i++) {
            RopeSegment firstSeg = ropeSegments[i];
            RopeSegment secondSeg = ropeSegments[i + 1];

            float distance = (firstSeg.posNow - secondSeg.posNow).magnitude;
            float error = (distance - ropeSegLen);
            Vector2 changeDir = Vector2.zero;

            if(distance > ropeSegLen) {
                changeDir = (firstSeg.posNow - secondSeg.posNow).normalized;
            }
            else if (distance < ropeSegLen) {
                changeDir = (secondSeg.posNow - firstSeg.posNow).normalized;
            }

            Vector2 changeAmount = changeDir * error;
            if(i != 0) {
                firstSeg.posNow -= changeAmount * 0.5f;
                ropeSegments[i] = firstSeg;
                secondSeg.posNow += changeAmount * 0.5f;
                ropeSegments[i + 1] = secondSeg;
            }
            else {
                secondSeg.posNow += changeAmount;
                ropeSegments[i + 1] = secondSeg;
            }
        }
    }
*/