using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {

    public float speed = 25;//rotations per sec

    private void Update()
    {
        transform.Rotate(Vector3.one * speed * Time.deltaTime);
    }
}
