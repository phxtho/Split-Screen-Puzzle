using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RaycastController))]

public class RaycastPlayer : MonoBehaviour {

    float gravity = -20;
    Vector3 velocity;

    RaycastController rayController;

    private void Start()
    {
        rayController = GetComponent<RaycastController>();
    }

    private void Update()
    {
        velocity.y += gravity * Time.deltaTime;
        rayController.Move(velocity * Time.deltaTime);
    }

}
