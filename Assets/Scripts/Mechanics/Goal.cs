using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {
    public GameObject goalPS;
    int callbackCount;

    private void OnTriggerStay(Collider other)
    {
        callbackCount++;

        if (other.tag == "Player" && callbackCount == 1)
        {

            if (goalPS != null)
                Instantiate(goalPS, other.transform.position, Quaternion.identity);

            GameManager.instance.LevelComplete();
        }
    }
}
