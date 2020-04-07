using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Controller2D : MonoBehaviour {
    // Skin width to fire rays from within object
    const float skinWidth = 0.015f;
    // Number of rays in each axis
    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;
    // Layers to collide with
    public LayerMask collisionMask;
    // Max angle able to climb/descend slope
    public float maxClimbAngle = 80f;
    public float maxDescendAngle = 75f;

    // Collider
    private BoxCollider2D coll;
    // Ray origins
    public RaycastOrigins raycastOrigins;
    // Object collision information
    public CollisionInfo collisions;
    // Movement amount past frame information
    public Vector3 moveAmountPast;

    // Space between collision raycasts
    public float horizontalRaySpacing, verticalRaySpacing;

    private void Awake() {
        // Setting references
        coll = GetComponent<BoxCollider2D>();
    }

    private void Start() {
        CalculateRaySpacing();
        collisions.Reset();
    }

    /// <summary>
    /// Updates raycast origins based on current collider bounds and skin width.
    /// </summary>
    private void UpdateRaycastOrigins() {
        Bounds bounds = coll.bounds;
        bounds.Expand(-2 * skinWidth);

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    /// <summary>
    /// Calculates collision raycast spacing based on ray count and collider size.
    /// </summary>
    public void CalculateRaySpacing() {
        Bounds bounds = coll.bounds;
        bounds.Expand(-2 * skinWidth);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }

    public void Move(Vector3 moveAmount) {
        // Recalculate ray origins and collision data every move call
        UpdateRaycastOrigins();
        collisions.Reset();
        moveAmountPast = moveAmount;

        // If y move amount is < 0, could be descending slope. Check and change if so
        if (moveAmount.y < 0) {
            DescendSlope(ref moveAmount);
        }
        // Get collision information based on move amount
        if (moveAmount.x != 0) {
            HorizontalCollisions(ref moveAmount);
        }
        if (moveAmount.y != 0) {
            VerticalCollisions(ref moveAmount);
        }

        // Move based on move amount
        transform.Translate(moveAmount);
    }

    /// <summary>
    /// Changes the movement amount based on object's horizontal collisions.
    /// </summary>
    /// <param name="moveAmount">Movement amount to be changed.</param>
    private void HorizontalCollisions(ref Vector3 moveAmount) {
        // Movement direction (-1 down, 1 up)
        float directionX = Mathf.Sign(moveAmount.x);
        // Length of ray
        float rayLength = Mathf.Abs(moveAmount.x) + skinWidth;

        for (int i = 0; i < horizontalRayCount; i++) {
            // Ray origin on bottom left if moving down, top left if moving up
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            // Ray origin updated each iteration (also vertical rays calculated after x movement amount)
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);

            // Cast the given ray
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);
            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

            // If ray hits something,
            if (hit) {
                // Getting collision angle
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                // If angle is climbable, climb
                if (i == 0 && slopeAngle <= maxClimbAngle) {
                    // If was descending a slope before, not anymore. Fixes slowdown when descending and suddenly climbing.
                    if (collisions.descendingSlope) {
                        collisions.descendingSlope = false;
                        moveAmount = moveAmountPast;
                    }

                    float distanceToSlopeStart = 0f;
                    // If starting to climb a new slope, move distance to
                    // slope start (since ray collides before movement is done)
                    if (slopeAngle != collisions.slopeAnglePast) {
                        distanceToSlopeStart = hit.distance - skinWidth;
                        moveAmount.x -= distanceToSlopeStart * directionX;
                    }
                    ClimbSlope(ref moveAmount, slopeAngle);
                    // Adding move distance to slope start back
                    moveAmount.x += distanceToSlopeStart * directionX;
                }

                // Only check other rays after first if not climbing slope
                // or if slope is too steep.
                if (!collisions.climbingSlope || slopeAngle > maxClimbAngle) {
                    // Next update x movement will be only the distance between object and ray collided target
                    moveAmount.x = (hit.distance - skinWidth) * directionX;
                    // Changes ray distance to not hit objects further away from foremost hit
                    rayLength = hit.distance;

                    // If slope angle is too steep, y move amount must be set as well
                    if (collisions.climbingSlope) {
                        // Setting y move amount following y = tan(angle) * move X (set AFTER slope climb)
                        moveAmount.y = Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(moveAmount.x);
                    }

                    // Set collision information depending on direction
                    collisions.left = (directionX == -1);
                    collisions.right = (directionX == 1);
                }
            }
        }
    }

    /// <summary>
    /// Changes the movement amount based on object's vertical collisions.
    /// </summary>
    /// <param name="moveAmount">Movement amount to be changed.</param>
    private void VerticalCollisions(ref Vector3 moveAmount) {
        // Movement direction (-1 down, 1 up)
        float directionY = Mathf.Sign(moveAmount.y);
        // Length of ray
        float rayLength = Mathf.Abs(moveAmount.y) + skinWidth;

        for (int i = 0; i < verticalRayCount; i++) {
            // Ray origin on bottom left if moving down, top left if moving up
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            // Ray origin updated each iteration (also vertical rays calculated after x movement amount)
            rayOrigin += Vector2.right * (verticalRaySpacing * i + moveAmount.x);

            // Cast the given ray
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);
            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.magenta);

            // If ray hits something,
            if (hit) {
                // Next update y movement will be only the distance between object and ray collided target
                moveAmount.y = (hit.distance - skinWidth) * directionY;
                // Changes ray distance to not hit objects further away from foremost hit
                rayLength = hit.distance;

                // If climbing slope and vertical collision happens, x needs to be set
                if (collisions.climbingSlope) {
                    moveAmount.x = moveAmount.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(moveAmount.x);
                }

                // Set collision information depending on direction (only after slope check)
                collisions.below = (directionY == -1);
                collisions.above = (directionY == 1);
            }
        }

        // To fix small stop when climbing a slope,
        if (collisions.climbingSlope) {
            float directionX = Mathf.Sign(moveAmount.x);
            rayLength = Mathf.Abs(moveAmount.x) + skinWidth;
            // Cast a ray from origin where next y movement will go to (in order to see if there's a new slope when the current climb happens)
            Vector2 rayOrigin = ((directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight) + Vector2.up * moveAmount.y;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            if (hit) {
                // Getting new slope angle
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if (slopeAngle != collisions.slopeAngle) {
                    // Next update x movement will be only the distance between object and ray collided target slope
                    moveAmount.x = (hit.distance - skinWidth) * directionX;
                    collisions.slopeAngle = slopeAngle;
                }
            }
        }
    }

    /// <summary>
    /// Method that handles slope climbing. Used in horizontal collisions.
    /// </summary>
    /// <param name="moveAmount">Amount to move in slope.</param>
    /// <param name="slopeAngle">Given slope angle.</param>
    public void ClimbSlope(ref Vector3 moveAmount, float slopeAngle) {
        // The X original move amount is the new full move amount on slope;
        // Calculating new X and Y movement based on this and slope angle.
        float moveDistance = Mathf.Abs(moveAmount.x);
        // y = sin(angle) * move amount (moveAmount.x)
        float slopeYMoveAmount = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
        // x = cos(angle) * move amount (moveAmount.x)
        // on X calculation, keep the sign to indicate direction of slope movement
        float slopeXMoveAmount = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(moveAmount.x);

        if (moveAmount.y <= slopeYMoveAmount) {
            moveAmount.y = slopeYMoveAmount;
            moveAmount.x = slopeXMoveAmount;
            // Since slope is being climbed, object is grounded
            collisions.below = true;
            // And is climbing
            collisions.climbingSlope = true;
            collisions.slopeAngle = slopeAngle;
        }
    }

    public void DescendSlope(ref Vector3 moveAmount) {
        float directionX = Mathf.Sign(moveAmount.x);
        // Ray origin is always bottom side touching slope. If moving left, bottom right. If moving right, bottom left.
        Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
        // Cast ray to check for descending slopes
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, Mathf.Infinity, collisionMask);
        if (hit) {
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
            // If target is not a flat surface and angle is not over max descend angle,
            if (slopeAngle != 0 && slopeAngle <= maxDescendAngle) {
                // If slope is facing the same direction as player (descending slope)
                if (Mathf.Sign(hit.normal.x) == directionX) {
                    // And if player is within range of slope (calculating y slope movement amount based on x movement and slope angle)
                    // (complicated (but not really that complicated) trigonometry)
                    if (hit.distance - skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad * Mathf.Abs(moveAmount.x))) {
                        // y = sin(angle) * move amount (moveAmount.x)
                        float moveDistance = Mathf.Abs(moveAmount.x);
                        float slopeYMoveAmount = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
                        // x = cos(angle) * move amount (moveAmount.x)
                        // on X calculation, keep the sign to indicate direction of slope movement
                        float slopeXMoveAmount = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(moveAmount.x);

                        // Movement in y is negative because it's going down the slope
                        moveAmount.y -= slopeYMoveAmount;
                        moveAmount.x = slopeXMoveAmount;

                        collisions.slopeAngle = slopeAngle;
                        collisions.descendingSlope = true;
                        collisions.below = true;
                    }
                }
            }
        }
    }
}

public struct RaycastOrigins {
    public Vector2 topLeft, topRight, bottomLeft, bottomRight;
}

[System.Serializable]
public struct CollisionInfo {
    public bool above, below, left, right;
    public bool climbingSlope, descendingSlope;
    public float slopeAngle, slopeAnglePast;

    public void Reset() {
        above = below = left = right = climbingSlope = descendingSlope = false;

        slopeAnglePast = slopeAngle;
        slopeAngle = 0f;
    }
}
