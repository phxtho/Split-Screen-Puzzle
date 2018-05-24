using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RaycastController))]

public class RaycastPlayer : MonoBehaviour
{
    Vector3 targetPos;

    float inputX, inputY;
    Vector3 xMovement, zMovement;
    float rotationAmount = -180;

    float gridDivision = 1f;

    float gravity = -10;
    Vector3 velocity;

    RaycastController rayController;

    private void Start()
    {
        rayController = GetComponent<RaycastController>();
        xMovement = Vector3.right / gridDivision;
        zMovement = Vector3.forward / gridDivision;
    }

    private void Update()
    {
        //Gravity Tings
        if (rayController.collisions.top || rayController.collisions.bottom)
        {
            velocity.y = 0;
        }
        velocity.y += gravity * Time.deltaTime;
        rayController.Gravity(velocity * Time.deltaTime);

        //Movement Input
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");

        Quaternion rotation = Quaternion.identity;
        targetPos = base.transform.position;

        if ((inputY == 1) && (transform.position == targetPos))
        {
            targetPos += zMovement;
            rotation = Quaternion.AngleAxis(rotationAmount, Vector3.right);

        }
        else if ((inputX == 1) && (transform.position == targetPos))
        {
            targetPos += xMovement;
            rotation = Quaternion.AngleAxis(rotationAmount, Vector3.back);
        }
        else if ((inputY == -1) && (transform.position == targetPos))
        {
            targetPos -= zMovement;
            rotation = Quaternion.AngleAxis(rotationAmount, Vector3.left);

        }
        else if ((inputX == -1) && (transform.position == targetPos))
        {
            targetPos -= xMovement;
            rotation = Quaternion.AngleAxis(rotationAmount, Vector3.forward);
        }

        rayController.MoveTo(targetPos, rotation);
    }
}
