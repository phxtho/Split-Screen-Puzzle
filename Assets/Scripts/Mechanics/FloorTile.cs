using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTile : MonoBehaviour {

    float onTime = 2f;
    public bool isOn = false;
    public bool permanentltyOn;
    public GameObject lightTile;
    public Material baseMaterial;
    Material  emissiveMaterial;
    MeshRenderer lightMeshRenderer;

    private void Start()
    {
        lightTile = transform.GetChild(0).gameObject;
        lightMeshRenderer = lightTile.GetComponent<MeshRenderer>();
        lightMeshRenderer.material = baseMaterial;
    }

    public virtual void LightUp(float onTime, Material material)
    {
        if (lightMeshRenderer == null)
        {
            lightTile = transform.GetChild(0).gameObject;
            lightMeshRenderer = lightTile.GetComponent<MeshRenderer>();
        }

        for(int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);

            BridgeSource bridgeSource = child.GetComponent<BridgeSource>();
            if (bridgeSource != null)
                bridgeSource.Activate();

        }

        if(emissiveMaterial == null)
            emissiveMaterial = GameObject.FindGameObjectWithTag("Player").GetComponent<MeshRenderer>().material;

        lightMeshRenderer.material = material;
        isOn = true;
        this.onTime = onTime;

        Invoke("TurnOff", this.onTime);
    }

    public virtual void TurnOff()
    {
        Time.timeScale = 1f;
        lightMeshRenderer.material = baseMaterial;

        if (permanentltyOn)
            return;

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);

            BridgeSource bridgeSource = child.GetComponent<BridgeSource>();
            if (bridgeSource != null)
                bridgeSource.DeActivate();

        }

        isOn = false;
    }
}
