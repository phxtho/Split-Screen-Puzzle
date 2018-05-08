using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]

public class GridMovement : MonoBehaviour {

    public float speed = 5.0f;
    Vector3 pos;
    Vector3 old;
    Transform tr;

    float inputX, inputY;
    Vector3 xMovement, zMovement;
    
    float gridDivision = 1f;

    bool isMoving = false;
    public AnimationCurve animCurve, otherCurve;
    private float startTime;

    void Start()
    {
        pos = transform.position;
        tr = transform;

        xMovement = Vector3.right / gridDivision;
        zMovement = new Vector3(0, 0, 1) / gridDivision;
    }

    private void Update()
    {
        //Movement Input
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        if ((inputY == 1) && (tr.position == pos))
        {
            pos += zMovement;
        }
        else if ((inputX == 1) && (tr.position == pos))
        {
            pos += xMovement;
        }
        else if ((inputY == -1) && (tr.position == pos))
        {
            pos -= zMovement;
        }
        else if ((inputX == -1) && (tr.position == pos))
        {
            pos -= xMovement;
        }

        if(pos != transform.position && !isMoving)
            MoveTo(pos, speed);
    }

    void MoveTo(Vector3 target, float duration)
    {
        StartCoroutine(AnimatePosition(transform.position, target, duration));
    }

    IEnumerator AnimatePosition(Vector3 origin, Vector3 target, float duration)
    {
        isMoving = true;
        float journey = 0f;
        while (journey <= duration)
        {
            journey = journey + Time.deltaTime;

            float percent = (Mathf.Clamp01(journey / duration));
            float curvePercent = animCurve.Evaluate(percent);
            float otherPercent = otherCurve.Evaluate(percent);

            //transform.position = Vector3.LerpUnclamped(origin, target, curvePercent);
            transform.position = new Vector3(Mathf.Lerp(origin.x, target.x, percent),
                                             origin.y + (curvePercent - otherPercent),
                                             Mathf.Lerp(origin.z, target.z, percent));

            yield return null;
        }
        isMoving = false;
    }
}
