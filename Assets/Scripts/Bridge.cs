using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class Bridge : MonoBehaviour {

    bool activated = false;

    public Transform[] bridgePath;
    public FloorTile[] powerSourceTiles;

    public Vector3 nodeOffset;

    public GameObject bridgeNodePrefab;
    public GameObject creationEffect;

    public AnimationCurve positionCurve;
    public float animDuration = 0.4f;

    private void Update()
    {
        if (!activated && IsPowered())
        {
            Timing.RunCoroutine(_ActivateTile());
        }
    }

    bool IsPowered()
    {
        if (powerSourceTiles.Length > 0)
        {
            foreach (FloorTile floorTile in powerSourceTiles)
            {
                if (!floorTile.isOn)
                    return false;
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
        return tile;
    }

    IEnumerator<float> _ActivateTile()
    {
        activated = true;

        foreach(Transform node in bridgePath)
        {
            Vector3 startPosition = node.position + nodeOffset;
            GameObject nodeTile = CreateTile(startPosition);

            float journey = 0f;
            while (journey <= animDuration)
            {
                journey += Time.deltaTime;
                //Calculate the percentage the animation has completed
                float percent = (Mathf.Clamp01(journey / animDuration));

                //Evaluate Animation Curve
                float positionCurveValue = positionCurve.Evaluate(percent);

                //Interpolate Position
                nodeTile.transform.position = Vector3.LerpUnclamped(startPosition, node.position, positionCurveValue);

                yield return Timing.WaitForOneFrame;
            }

            Instantiate(creationEffect, node.position, Quaternion.identity);
        }
        
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
