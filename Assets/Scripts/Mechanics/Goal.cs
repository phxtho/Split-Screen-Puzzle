using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("Collision Detectded");
        if (other.tag == "Player")
            GameManager.instance.ResetPlayer();
    }
}
