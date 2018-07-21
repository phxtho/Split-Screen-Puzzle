using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour {
    Transform player;
    public GameObject deathParticle;
    float distance;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        distance = (player.position - transform.position).magnitude;
        if (distance <= 1f)
        {
            Instantiate(deathParticle, player.position, Quaternion.identity);
            GameManager.instance.ResetPlayer();
        }
    }
}
