using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour {
    public float waitTime = 1f;

    private void Start()
    {
        Invoke("Destruct", waitTime);
    }

    void Destruct()
    {
        Destroy(this.gameObject);
    }

}
