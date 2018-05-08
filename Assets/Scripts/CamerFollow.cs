using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerFollow : MonoBehaviour {

    public Transform target;

    public float distance;
    public float angle;
    public float smoothSpeed = 0.325f;
    public Vector3 offset;

    Vector3 velocity = Vector3.zero;

    private void Start()
    {
        SetupCam();
        offset = transform.position;
    }

    private void FixedUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
        transform.position = smoothedPosition;
        //transform.LookAt(target);
    }

    //Sets the height and the angle of the camera
    void SetupCam()
    {
        transform.rotation = Quaternion.Euler(angle, 0, 0); //set the angle

        distance = Mathf.Abs(distance);
        float z = -(distance * Mathf.Cos(angle * Mathf.Deg2Rad));
        float y = distance * Mathf.Sin(angle * Mathf.Deg2Rad);
        transform.position = new Vector3(0, y,z);

        //Getting the x-distance from angle and height
        //float x = (height / (Mathf.Tan((angle - 180) * Mathf.Deg2Rad))) * -1f;

        //Getting the angle from x and y distance
        //float angle = (Mathf.PI - Mathf.Atan2(transform.position.y, transform.position.x)) * Mathf.Rad2Deg;

    }
}
