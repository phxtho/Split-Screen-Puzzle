using MEC;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]

public class RaycastController : MonoBehaviour {

    //MOVE
    public float moveDuration = 1f;
    public float hopHeight = 1f;

    public AnimationCurve hopCurve, subtractorCurve;
    public AnimationCurve moveCurve;
    private float startTime;

    public bool isMoving = false;

    public float distanceToGround;

    //RAYS
    public LayerMask collisionMask;

    //Array of vectors storing the vertices of these faces
    Vector3[,,] cubeVerts = new Vector3[2,2,2];

    const float skinWidth = 0.015f;
    const float skinWidthScaling = -1f;

    new BoxCollider collider;
    RaycastOrigins rayCastOrigins;
    public CollisionInfo collisions;

    struct RaycastOrigins
    {
        public Vector3 leftTopFront, leftTopBack, leftBottomFront, leftBottomBack;
        public Vector3 rightTopFront, rightTopBack, rightBottomFront, rightBottomBack;
    }

    public struct CollisionInfo
    {
        public bool right, left;
        public bool top, bottom;
        public bool front, back;

        public void Reset()
        {
            right = left = false;
            top = bottom = false;
            front = back = false;
        }
    }

    //VFX
    public GameObject cam;
    public GameObject landingParticle;

    void Start () {
        collider = GetComponent<BoxCollider>();
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

        cubeVerts[0, 0, 0] = rayCastOrigins.leftBottomBack;
        cubeVerts[0, 0, 1] = rayCastOrigins.leftBottomFront;
        cubeVerts[0, 1, 0] = rayCastOrigins.leftTopBack;
        cubeVerts[0, 1, 1] = rayCastOrigins.leftTopFront;
        cubeVerts[1, 0, 0] = rayCastOrigins.rightBottomBack;
        cubeVerts[1, 0, 1] = rayCastOrigins.rightBottomFront;
        cubeVerts[1, 1, 0] = rayCastOrigins.rightTopBack;
        cubeVerts[1, 1, 1] = rayCastOrigins.rightTopFront;
    }

    void VerticalCollisions(ref Vector3 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);

        float rayLength = Mathf.Abs(velocity.y) + skinWidth;

        int y = (directionY == -1) ? 0 : 1;
        for (int z = 0; z < 2; z++)
        {
            for (int x = 0; x < 2; x++)
            {
                Vector3 rayOrigin = cubeVerts[x, y, z];

                RaycastHit hit;

                if (Physics.Raycast(rayOrigin, Vector3.up * directionY, out hit, rayLength, collisionMask))
                {
                    velocity.y = (hit.distance - skinWidth) * directionY;
                    rayLength = hit.distance;
                    distanceToGround = rayLength - skinWidth;

                    collisions.top = directionY == 1;
                    collisions.bottom = directionY == -1;
                }

                Debug.DrawRay(rayOrigin, Vector3.up * directionY * rayLength, Color.green);
            }

        }
    }

    bool HorizontalCollisions(Vector3 targetPosition)
    {
        float directionX = (targetPosition - transform.position).normalized.x;
        float directionZ = (targetPosition - transform.position).normalized.z;
        float rayLength = 1 + skinWidth;

        int collisionCount = 0;

        if (directionX !=0)
        {
            int x = (directionX == -1) ? 0 : 1;
            for (int z = 0; z < 2; z++)
            {
                for (int y = 0; y < 2; y++)
                {
                    Vector3 rayOrigin = cubeVerts[x, y, z];

                    RaycastHit hit;

                    if (Physics.Raycast(rayOrigin, Vector3.right * directionX, out hit, rayLength, collisionMask))
                    {
                        collisionCount++;

                        collisions.right = directionX == 1;
                        collisions.left = directionX == -1;    
                    }
                    Debug.DrawRay(rayOrigin, Vector3.right * directionX * rayLength, Color.red);
                }
            }
        }
        else if (directionZ != 0)
        {
            int z = (directionZ == -1) ? 0 : 1;
            for (int y = 0; y < 2; y++)
            {
                for (int x = 0; x < 2; x++)
                {
                    Vector3 rayOrigin = cubeVerts[x, y, z];

                    RaycastHit hit;

                    if (Physics.Raycast(rayOrigin, Vector3.forward * directionZ, out hit, rayLength, collisionMask))
                    {
                        collisionCount++;

                        collisions.front = directionZ == 1;
                        collisions.back = directionZ == -1;
                    }
                    Debug.DrawRay(rayOrigin, Vector3.forward * directionZ * rayLength, Color.blue);
                }

            }
        }

        if (collisionCount == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Gravity(Vector3 velocity)
    {
        if (isMoving)
            return;

        collisions.Reset();

        UpdateRaycastOrigins();

        if (velocity.y != 0)
        {
            VerticalCollisions(ref velocity);
        }


        transform.Translate(velocity);
    }

    public void MoveTo(Vector3 targetPos, Quaternion targetRot)
    {
        UpdateRaycastOrigins();

        if (HorizontalCollisions(targetPos) && targetPos != transform.position && !isMoving)
        {
            Timing.RunCoroutine(_AnimatePosition(transform.position, targetPos, transform.rotation, targetRot, moveDuration));
        }
    }

    IEnumerator<float> _AnimatePosition(Vector3 originalPos, Vector3 targetPos, Quaternion originalRot, Quaternion targetRot, float duration)
    {
        isMoving = true;
        float journey = 0f;

        while (journey <= duration)
        {
            //Calculate percentage of the animation that has been completed
            journey = journey + Time.deltaTime;
            float percent = (Mathf.Clamp01(journey / duration));

            //Evaluate Animation Curves
            float jumpCurveValue = hopCurve.Evaluate(percent);
            float subtractorCurveValue = subtractorCurve.Evaluate(percent);
            float moveCurveValue = moveCurve.Evaluate(percent);

            //Interpolate Position Based on Curve Values
            transform.position = new Vector3(Mathf.LerpUnclamped(originalPos.x, targetPos.x, moveCurveValue),
                                             originalPos.y + hopHeight * (jumpCurveValue - subtractorCurveValue),
                                             Mathf.LerpUnclamped(originalPos.z, targetPos.z, moveCurveValue));

            //Interpolate Rotation
            transform.rotation = Quaternion.Slerp(originalRot, originalRot * targetRot, percent);

            yield return Timing.WaitForOneFrame;
        }
        //Activate Screen Shake
        cam.GetComponent<CameraShake>().StartShaking();
        Instantiate(landingParticle, transform.position - Vector3.up * 0.25f, Quaternion.identity);

        transform.rotation = Quaternion.identity;
        isMoving = false;
    }
}
