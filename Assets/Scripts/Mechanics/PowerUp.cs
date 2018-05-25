using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {

	public enum PowerUpType { shrink, grow}
    public PowerUpType powerUpType;
    GameObject player;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            player = other.gameObject;
            ActivatePowerUp();
        }
    }

    void ActivatePowerUp()
    {
        //Spawn Particle Effect

        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;

        switch (powerUpType)
        {
            case PowerUpType.shrink:
                Shrink();
                break;

            case PowerUpType.grow:
                Grow();
                break;
        }

        Destroy(gameObject);
    }

    #region Power Ups

    void Shrink()
    {
        float multiplier = 0.5f;
        player.transform.localScale *= multiplier;
    }

    void Grow()
    {
        float multiplier = 1.5f;
        player.transform.localScale *= multiplier;
    }

    #endregion
}
