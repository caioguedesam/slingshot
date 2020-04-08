using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patada.Events;

public class Ball : MonoBehaviour
{
    [Header("References")]
    // Rope reference
    [SerializeField] private Rope rope;
    // Component references
    private CircleCollider2D coll;
    private Rigidbody2D rb;
    [Space(2)]

    [Header("Jump variables")]
    public float jumpForce = 5f;
    [SerializeField] private bool slingToggle = false;
    private bool landed = false;
    private bool falling = false;
    private Vector3 slingDirection;
    [Space(2)]

    [Header("Rope detection settings")]
    public LayerMask slingLayerMask;
    [Space(5)]

    [Header("Events")]
    [SerializeField] private SCOB_BaseEvent fallingEvent;
    [SerializeField] private SCOB_BaseEvent landedOnRopeEvent;

    private void Start() {
        coll = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        //Time.timeScale = .5f;
    }

    /*private void OnCollisionEnter2D(Collision2D collision) {
        if(!landed && collision.collider.CompareTag("Rope")) {
            landedOnRopeEvent.Raise();
            landed = true;
            // Do I need this?
            rb.velocity = Vector2.zero;
        }
    }*/

    private void OnCollisionEnter2D(Collision2D collision) {
        /*if (!landed && collision.collider.CompareTag("Rope") && Vector2.Distance(rb.velocity, Vector2.zero) < 0.3) {
            landedOnRopeEvent.Raise();
            landed = true;
            // Do I need this?
            rb.velocity = Vector2.zero;
        }*/
        if (!landed && collision.collider.CompareTag("Rope") && !Input.GetMouseButton(0)) {
            landedOnRopeEvent.Raise();
            landed = true;
            // Do I need this?
            rb.velocity = Vector2.zero;
        }
    }

    private void OnCollisionStay2D(Collision2D collision) {
        /*if (!landed && collision.collider.CompareTag("Rope") && Vector2.Distance(rb.velocity, Vector2.zero) < 0.3) {
            landedOnRopeEvent.Raise();
            landed = true;
            // Do I need this?
            rb.velocity = Vector2.zero;
        }*/
        if (!landed && collision.collider.CompareTag("Rope") && !Input.GetMouseButton(0)) {
            landedOnRopeEvent.Raise();
            landed = true;
            // Do I need this?
            rb.velocity = Vector2.zero;
        }
    }

    private void FixedUpdate() {
        /*RaycastHit2D hit = Physics2D.CircleCast(transform.position, .1f, Vector2.up, 100f, slingLayerMask);
        if(hit) {
            if(RopeCastPositionCheck(hit.point)) {
                transform.position = new Vector2(rope.ropeSegments[(rope.segmentCount - 1)/2].posNow.x, hit.point.y + coll.radius);
                Debug.Log("Setting position according to rope");
            }
        }
        
        if(slingToggle) {
            slingToggle = false;
            landed = false;
            rb.velocity = slingDirection * jumpForce;
            Debug.Log("Let go");
        }*/

        if(!falling && !landed && rb.velocity.y < 0) {
            falling = true;
            fallingEvent.Raise();
        }
    }

    private void Update() {

        slingDirection = rope.slingDirection.normalized;

        // IMPORTANT: Fix input! Use swipe manager
        /*if(landed && Input.GetMouseButtonUp(0)) {
            Rope.RopeSegment pullSegment = rope.ropeSegments[(rope.segmentCount - 1) / 2];

            StartCoroutine(SlingHandler());
            slingToggle = false;
        }*/

        /*if (landed) {
            RaycastHit2D hit = Physics2D.CircleCast(transform.position, coll.radius + 0.01f, Vector2.zero, 0f, slingLayerMask);
            if(!hit) {
                landed = false;
            }
        }*/
    }

    public IEnumerator SlingHandler(Vector3 slingDirection) {
        Rope.RopeSegment pullSegment = rope.ropeSegments[(rope.segmentCount - 1) / 2];
        Vector2 pullVelocity = pullSegment.posNow - pullSegment.posOld;
        Vector2 pastVelocity = pullVelocity;

        bool moveRight = (transform.position.x < 0);

        while(pullVelocity.y <= 0) {
            Debug.Log(pullVelocity + "velocity");

            transform.position = new Vector2(pullSegment.posNow.x, pullSegment.posNow.y + coll.radius / 2);

            // Not sure if I need THIS
            rb.velocity = slingDirection * jumpForce;

            yield return new WaitForEndOfFrame();

            pullSegment = rope.ropeSegments[(rope.segmentCount - 1) / 2];
            pullVelocity = pullSegment.posNow - pullSegment.posOld;
        }

        while(pullVelocity.y > 0) {

            if (moveRight)
                transform.position = new Vector2(Mathf.Max(transform.position.x, pullSegment.posNow.x), pullSegment.posNow.y + coll.radius / 2);
            else
                transform.position = new Vector2(Mathf.Min(transform.position.x, pullSegment.posNow.x), pullSegment.posNow.y + coll.radius / 2);

            // Not sure if I need THIS
            rb.velocity = slingDirection * jumpForce;

            yield return new WaitForEndOfFrame();

            pastVelocity = pullVelocity;
            pullSegment = rope.ropeSegments[(rope.segmentCount - 1) / 2];
            pullVelocity = pullSegment.posNow - pullSegment.posOld;
        }

        // Sling
        falling = false;
        landed = false;
        rb.velocity = slingDirection.normalized * (slingDirection.magnitude / rope.maxSlingRadius) * jumpForce;
        Debug.Log("Let go factor: " + (slingDirection.magnitude / rope.maxSlingRadius));
    }

    public bool RopeCastPositionCheck(Vector2 pos) {
        /*if(pos.x < 0) {
            //Debug.Log("Condition: " + (pos.y > transform.position.y && pos.x < transform.position.x));
            return (pos.y > transform.position.y && pos.x < transform.position.x);
        }
        else {
            //Debug.Log("Condition: " + (pos.y > transform.position.y && pos.x > transform.position.x));
            return (pos.y > transform.position.y && pos.x > transform.position.x);
        }*/

        return pos.y > transform.position.y;
    }

    /*public void Sling(Vector3 slingDirection) {
        Debug.Log("Sling with direction " + slingDirection.normalized);
        slingToggle = true;
        this.slingDirection = slingDirection.normalized;
    }*/

    public void Sling(Vector3 slingDirection) {
        Rope.RopeSegment pullSegment = rope.ropeSegments[(rope.segmentCount - 1) / 2];

        StartCoroutine(SlingHandler(slingDirection));
        slingToggle = false;
    }


    private void OnDrawGizmos() {
        if(Application.isPlaying) {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, coll.radius + 0.01f);
        }
    }
}
