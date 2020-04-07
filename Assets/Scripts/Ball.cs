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
    private Vector3 slingDirection;
    [Space(2)]

    [Header("Rope detection settings")]
    public LayerMask slingLayerMask;
    [Space(5)]

    [Header("Events")]
    [SerializeField] private SCOB_BaseEvent landedOnRopeEvent;

    private void Start() {
        coll = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        //Time.timeScale = .1f;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if(!landed && collision.collider.CompareTag("Rope")) {
            landedOnRopeEvent.Raise();
            landed = true;
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
    }

    private void Update() {

        if(landed && Input.GetMouseButtonUp(0)) {
            Rope.RopeSegment pullSegment = rope.ropeSegments[(rope.segmentCount - 1) / 2];

            StartCoroutine(SlingHandler());
            slingToggle = false;
        }

        /*if(landed) {
            //RaycastHit2D hit = Physics2D.CircleCast(transform.position, 1f, Vector2.up, 100f, slingLayerMask);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, 100f, slingLayerMask);
            Debug.DrawRay(transform.position, Vector2.up * 100f, Color.yellow, .1f);

            if (hit) {
                if (RopeCastPositionCheck(hit.point)) {
                    transform.position = new Vector2(rope.ropeSegments[(rope.segmentCount - 1) / 2].posNow.x, hit.point.y + coll.radius);
                    Debug.Log("Setting position according to rope");
                }
                else if(slingToggle) {
                    // Sling
                    slingToggle = false;
                    landed = false;
                    rb.velocity = slingDirection * jumpForce;
                    Debug.Log("Let go");
                }
            }
            else if(slingToggle) {
                // Sling
                slingToggle = false;
                landed = false;
                rb.velocity = slingDirection * jumpForce;
                Debug.Log("Let go");
            }
        }*/
    }

    public IEnumerator SlingHandler() {
        Rope.RopeSegment pullSegment = rope.ropeSegments[(rope.segmentCount - 1) / 2];
        Vector2 pullVelocity = pullSegment.posNow - pullSegment.posOld;
        Vector2 pastVelocity = pullVelocity;
        Debug.Log(pullVelocity + "velocity");

        while(pullVelocity.y <= 0) {
            Debug.Log(pullVelocity + "velocity");

            transform.position = new Vector2(pullSegment.posNow.x, pullSegment.posNow.y + coll.radius / 2);

            yield return new WaitForEndOfFrame();

            pullSegment = rope.ropeSegments[(rope.segmentCount - 1) / 2];
            pullVelocity = pullSegment.posNow - pullSegment.posOld;
        }

        while(pullVelocity.y > 0) {
            Debug.Log(pullVelocity + "velocity");

            transform.position = new Vector2(pullSegment.posNow.x, pullSegment.posNow.y + coll.radius / 2);

            yield return new WaitForEndOfFrame();

            pastVelocity = pullVelocity;
            pullSegment = rope.ropeSegments[(rope.segmentCount - 1) / 2];
            pullVelocity = pullSegment.posNow - pullSegment.posOld;
        }

        // Sling
        landed = false;
        rb.velocity = slingDirection * jumpForce;
        Debug.Log("Let go");
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

    public void Sling(Vector3 slingDirection) {
        Debug.Log("Sling with direction " + slingDirection.normalized);
        slingToggle = true;
        this.slingDirection = slingDirection.normalized;
        //this.slingDirection = slingDirection;
    }

}
