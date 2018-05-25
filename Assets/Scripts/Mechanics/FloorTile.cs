﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTile : MonoBehaviour {

    float onTime = 2f;
    public bool isOn = false;
    public bool permanentltyOn;
    public GameObject lightTile;
    public Material baseMaterial;
    MeshRenderer meshRenderer;

    private void Start()
    {
        meshRenderer = lightTile.GetComponent<MeshRenderer>();
        meshRenderer.material = baseMaterial;
    }

    public virtual void LightUp(float onTime, Material material)
    {
        if (meshRenderer == null)
            return;

        meshRenderer.material = material;
        isOn = true;
        this.onTime = onTime;
        Invoke("TurnOff", this.onTime);
    }

    public virtual void TurnOff()
    {
        meshRenderer.material = baseMaterial;

        if (permanentltyOn)
            return;

        isOn = false;
    }
}