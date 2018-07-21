using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class Bridge : MonoBehaviour {

    public GameObject sourceIndicatorPrefab;
    List<BridgeSource> sourceIndicators = new List<BridgeSource>();

    bool activated = false;
    public bool switchable;

    public Transform[] bridgePath;
    List<GameObject> bridgeTiles = new List<GameObject>();
    public FloorTile[] powerSourceTiles;

    public Vector3 nodeOffset;

    public GameObject bridgeNodePrefab;
    public Material switchableEmissiveMaterial, permanentEmissiveMaterial;
    public GameObject creationEffect;

    public AnimationCurve positionCurve;
    public float animDuration = 0.4f;

    private void Start()
    {
        if (sourceIndicatorPrefab != null)
        {
            foreach (FloorTile powerSourceTile in powerSourceTiles)
            {
                //Create the source indicator
                GameObject sourceIndicator = Instantiate(sourceIndicatorPrefab, powerSourceTile.transform.position + Vector3.up, Quaternion.identity);

                //Set source indicators active material
                Material sourceMaterial;

                if (switchable)
                    sourceMaterial = switchableEmissiveMaterial;
                else
                    sourceMaterial = permanentEmissiveMaterial;

                BridgeSource sourceIndicatorScript = sourceIndicator.GetComponent<BridgeSource>();
                sourceIndicatorScript.activeMaterial = sourceMaterial;

                sourceIndicators.Add(sourceIndicatorScript);

                //Parent indicator to the source tile
                sourceIndicator.transform.SetParent(powerSourceTile.transform);
            }
        }
    }

    private void Update()
    {
        if (!activated && IsPowered())
        {
            Timing.RunCoroutine(_ActivateTile());
        }

        if(switchable && activated && !IsPowered())
        {
            Timing.RunCoroutine(_DeactivateTile());
        }
    }

    bool IsPowered()
    {
        if (powerSourceTiles.Length > 0)
        {
            foreach (FloorTile sourceTile in powerSourceTiles)
            {
                if (!sourceTile.isOn)
                    return false;
            }

            //This ensures that permanent bridges sources stay down;
            foreach(BridgeSource sourceScript in sourceIndicators)
            {
               sourceScript.isSwitchable = switchable;
            }
            return true;
        }
        else
        {
            Debug.Log("Bridge has no power source");
            return false;
        }
    }

    GameObject CreateTile(Vector3 position)
    {
        GameObject tile = Instantiate(bridgeNodePrefab, position, Quaternion.identity, this.transform);
        Instantiate(creationEffect, position, Quaternion.identity);
        bridgeTiles.Add(tile);
        return tile;
    }

    IEnumerator<float> _ActivateTile()
    {
        activated = true;

        int count = 0;
        foreach(Transform node in bridgePath)
        {
            Vector3 startPosition = node.position + nodeOffset;

            //Create or Access the tile object
            GameObject nodeTile;
            if (bridgeTiles.Count < bridgePath.Length)
            {
                nodeTile = CreateTile(startPosition);
            }
            else
            {
                nodeTile = bridgeTiles[count];
            }

            //Set the material
            FloorTile tileScript= nodeTile.GetComponent<FloorTile>();
            Material material;
            if (switchable)
            {
                material = switchableEmissiveMaterial;
                tileScript.permanentltyOn = false;
            }
            else
            {
                material = permanentEmissiveMaterial;
                tileScript.permanentltyOn = true;
                tileScript.baseMaterial = permanentEmissiveMaterial;
            }

            //Animate
            float journey = 0f;
            float perTileDuration = animDuration/(bridgePath.Length-1);

            while (journey <= perTileDuration)
            {
                journey += Time.deltaTime;
                //Calculate the percentage the animation has completed
                float percent = (Mathf.Clamp01(journey / perTileDuration));

                //Evaluate Animation Curve
                float positionCurveValue = positionCurve.Evaluate(percent);

                //Interpolate Position
                nodeTile.transform.position = Vector3.LerpUnclamped(startPosition, node.position, positionCurveValue);

                tileScript.LightUp(Time.deltaTime, material);
         
                yield return Timing.WaitForOneFrame;
            }

            tileScript.LightUp(2f, material);
            count++;
        }

       

    }

    IEnumerator<float> _DeactivateTile()
    {
        int counter = 0;
        foreach (GameObject node in bridgeTiles)
        {
            Vector3 startPosition = node.transform.position;
            Vector3 endPosition = bridgePath[counter].position + nodeOffset;

            float journey = 0f;
            while (journey <= animDuration)
            {
                journey += Time.deltaTime;
                //Calculate the percentage the animation has completed
                float percent = (Mathf.Clamp01(journey / animDuration));

                //Evaluate Animation Curve
                float positionCurveValue = positionCurve.Evaluate(percent);

                //Interpolate Position
                node.transform.position = Vector3.LerpUnclamped(startPosition, endPosition, positionCurveValue);

                //Light Up
                node.GetComponent<FloorTile>().LightUp(Time.deltaTime, switchableEmissiveMaterial);

                yield return Timing.WaitForOneFrame;
            }
            counter++;
        }

        activated = false;

    }



    private void OnDrawGizmos()
    {
        if (bridgePath.Length==0)
            return;

        Vector3 startPosition = bridgePath[0].position;
        Vector3 previousPosition = startPosition;

        foreach (Transform node in bridgePath)
        {
            Gizmos.DrawSphere(node.position, 0.3f);
            Gizmos.DrawLine(previousPosition, node.position);
            previousPosition = node.position;
        }
    }
}
