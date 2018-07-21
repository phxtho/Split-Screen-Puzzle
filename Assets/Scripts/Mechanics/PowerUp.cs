using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {

	public enum PowerUpType { shrink, armour}
    public PowerUpType powerUpType;
    public GameObject activationPS;
    GameObject player;
    PowerManagement playerPM;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerPM = player.GetComponent<PowerManagement>();
    }

    private void Update()
    {
        if (player == null)
            return;

        float distance = (player.transform.position - transform.position).magnitude;
        if (distance <= 0.8f)
            EnablePower();
    }

    void EnablePower()
    {
        //Spawn Particle Effect
        Instantiate(activationPS, transform.position, Quaternion.identity);

        GetComponent<MeshRenderer>().enabled = false;
       // GetComponent<Collider>().enabled = false;

        switch (powerUpType)
        {
            case PowerUpType.shrink:
                playerPM.powerUpType = PowerManagement.PowerUpType.shrink;
                break;

            case PowerUpType.armour:
                playerPM.powerUpType = PowerManagement.PowerUpType.armour;
                break;
        }

        Destroy(gameObject);
    }
}
