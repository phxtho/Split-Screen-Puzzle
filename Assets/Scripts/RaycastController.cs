using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastController : MonoBehaviour {

    public LayerMask collisionMask;

    const float skinWidth = 0.15f;
    const float skinWidthScaling = -1f;
    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;

    float horizontalRaySpacing;
    float verticalRaySpacing;

    Vector3 rayOrigin;
    Vector3 targetDir;
    new BoxCollider collider;
    RaycastOrigins rayCastOrigins;

    struct RaycastOrigins
    {
        public Vector3 leftTopFront, leftTopBack, leftBottomFront, leftBottomBack;
        public Vector3 rightTopFront, rightTopBack, rightBottomFront, rightBottomBack;
    }

    void Start () {
        collider = GetComponent<BoxCollider>();
        CalculateRaySpacing();
    }

    public void Move(Vector3 velocity)
    {
        UpdateRaycastOrigins();

        if (velocity.y != 0)
        {
            VerticalCollisions(ref velocity);
        }
        

        transform.Translate(velocity);
    }

  /*  void HorizontalCollisions(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector3 rayOrigin = (directionX == -1) ? rayCastOrigins.leftBottomFront : rayCastOrigins.leftTopFront;
            rayOrigin += Vector3.right * (verticalRaySpacing * i + velocity.x);
            RaycastHit hit;

            if (Physics.Raycast(rayOrigin, Vector3.up * directionX, out hit, rayLength, collisionMask))
            {
                velocity.y = (hit.distance - skinWidth) * directionX;
                rayLength = hit.distance;
            }
            Debug.DrawRay(rayOrigin, Vector3.up * directionX, Color.blue);
        }
    }*/

    void VerticalCollisions(ref Vector3 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;

        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector3 rayOrigin = (directionY == -1) ? rayCastOrigins.leftBottomFront : rayCastOrigins.leftTopFront;
            rayOrigin += Vector3.right * (verticalRaySpacing * i + velocity.x);
            RaycastHit hit;

            if(Physics.Raycast(rayOrigin, Vector3.up * directionY, out hit, rayLength, collisionMask))
            {
                velocity.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;
            }
            Debug.DrawRay(rayOrigin, Vector3.up * directionY * rayLength, Color.blue);
        }
    }

    void UpdateRaycastOrigins()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * skinWidthScaling);

        rayCastOrigins.leftTopFront = new Vector3(bounds.min.x, bounds.max.y, bounds.max.z);
        rayCastOrigins.leftTopBack = new Vector3(bounds.min.x, bounds.max.y, bounds.min.z);
        rayCastOrigins.leftBottomFront = new Vector3(bounds.min.x, bounds.min.y, bounds.max.z);
        rayCastOrigins.leftBottomBack = new Vector3(bounds.min.x, bounds.min.y, bounds.min.z);

        rayCastOrigins.rightTopFront = new Vector3(bounds.max.x, bounds.max.y, bounds.max.z);
        rayCastOrigins.rightTopBack = new Vector3(bounds.max.x, bounds.max.y, bounds.min.z);
        rayCastOrigins.rightBottomFront = new Vector3(bounds.max.x, bounds.min.y, bounds.max.z);
        rayCastOrigins.rightBottomBack = new Vector3(bounds.max.x, bounds.min.y, bounds.min.z);
    }

    void CalculateRaySpacing()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * skinWidthScaling);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }
}
