using UnityEngine;
using System.Collections.Generic;
using MEC;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]

public class GridMovement : MonoBehaviour {

    public float speed = 5.0f;
    public float jumpHeight = 1f;
    Vector3 targetPos;
    Vector3 targetRot;
    Transform tr;

    float inputX, inputY;
    Vector3 xMovement, zMovement;
    float rotationAmount = 90;
    
    float gridDivision = 1f;

    bool isMoving = false;
    public AnimationCurve jumpCurve, subtractorCurve;
    public AnimationCurve moveCurve;
    private float startTime;

    public GameObject camera;

    Vector3 rayOrigin;
    Vector3 rayDirection;
    float rayLength = 1f;

    public GameObject pS;

    void Start()
    {
        targetPos = transform.position;
        tr = transform;

        xMovement = Vector3.right / gridDivision;
        zMovement = new Vector3(0, 0, 1) / gridDivision;

    }

    private void Update()
    {
        //Movement Input
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    void FixedUpdate()
    {
        if (isMoving)
            return;

        Quaternion rotation = Quaternion.identity;
        targetPos = transform.position;

        if ((inputY == 1) && (tr.position == targetPos))
        {
            targetPos += zMovement;
            rotation = Quaternion.AngleAxis(rotationAmount, Vector3.forward);
           
        }
        else if ((inputX == 1) && (tr.position == targetPos))
        {
            targetPos += xMovement;
            rotation = Quaternion.AngleAxis(rotationAmount, Vector3.right);
        }
        else if ((inputY == -1) && (tr.position == targetPos))
        {
            targetPos -= zMovement;
            rotation = Quaternion.AngleAxis(rotationAmount, Vector3.back);
            
        }
        else if ((inputX == -1) && (tr.position == targetPos))
        {
            targetPos -= xMovement;
            rotation = Quaternion.AngleAxis(rotationAmount, Vector3.left);
        }
                
        rayOrigin = transform.position;
        rayDirection = (targetPos - rayOrigin).normalized;

        RaycastHit hit;
        //!Physics.Raycast(rayOrigin, rayDirection, out hit, rayLength) && 

        if (!Physics.Raycast(rayOrigin, rayDirection, out hit, rayLength) && targetPos != transform.position && !isMoving)
            MoveTo(targetPos,rotation, speed);
       


    }

    void MoveTo(Vector3 targetPos,Quaternion targetRot, float duration)
    {
        Timing.RunCoroutine(_AnimatePosition(tr.position, targetPos,tr.rotation,targetRot, duration));
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
            float jumpCurveValue = jumpCurve.Evaluate(percent);
            float subtractorCurveValue = subtractorCurve.Evaluate(percent);
            float moveCurveValue = moveCurve.Evaluate(percent);

            //Interpolate Position Based on Curve Values
            transform.position = new Vector3(Mathf.LerpUnclamped(originalPos.x, targetPos.x, moveCurveValue),
                                             originalPos.y + jumpHeight * (jumpCurveValue-subtractorCurveValue),
                                             Mathf.LerpUnclamped(originalPos.z, targetPos.z, moveCurveValue));

            //Interpolate Rotation
            transform.rotation = Quaternion.Slerp(originalRot, originalRot * targetRot, percent);

            yield return Timing.WaitForOneFrame;
        }
        //Activate Screen Shake
        camera.GetComponent<CameraShake>().StartShaking();
        Instantiate(pS, transform.position - Vector3.up * 0.25f, Quaternion.identity);

        isMoving = false;
    }

    bool Grounded()
    {
        if(Physics.Raycast(transform.position, Vector3.down))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
