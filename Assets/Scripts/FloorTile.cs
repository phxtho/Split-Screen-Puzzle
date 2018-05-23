using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTile : MonoBehaviour {

    public float waitTime = 2f;
    public GameObject lightTile;
    public Material baseMaterial, emissiveMaterial;
    MeshRenderer meshRenderer;

    private void Start()
    {
        meshRenderer = lightTile.GetComponent<MeshRenderer>();
        meshRenderer.material = baseMaterial;
    }

    public void LightUp()
    {
        meshRenderer.material = emissiveMaterial;
        Invoke("TurnOff", waitTime);
    }

    void TurnOff()
    {
        meshRenderer.material = baseMaterial;
    }
}
