using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class BridgeSource : MonoBehaviour {
    float offset = 0.3f;
    float animDuration = 1f;
    public AnimationCurve positionCurve;
    bool activated;

    Vector3 inactivePos, activePos;

    [HideInInspector]
    public bool isSwitchable = true;

    MeshRenderer meshRenderer;

    [HideInInspector]
    public Material baseMaterial, activeMaterial;

    private void Start()
    {
        inactivePos = transform.position;
        activePos = transform.position - Vector3.up * offset;
        isSwitchable = true;
        meshRenderer = GetComponent<MeshRenderer>();
        baseMaterial = meshRenderer.material;

        if (activeMaterial != null)
            meshRenderer.material = activeMaterial;
    }

    public void Activate()
    {
        if (!activated)
        {
            activated = true;
            meshRenderer.material = activeMaterial;
            Timing.RunCoroutine(_Animate(activePos));
        }
    }

    public void DeActivate()
    {
        if (isSwitchable)
        {
            Timing.RunCoroutine(_Animate(inactivePos));
            meshRenderer.material = activeMaterial;
            activated = false;
        }
    }

	IEnumerator<float> _Animate(Vector3 targetPos)
    {
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = targetPos;
        float journey = 0f;

        while (journey <= animDuration)
        {
            journey += Time.deltaTime;
            //Calculate the percentage the animation has completed
            float percent = (Mathf.Clamp01(journey / animDuration));

            //Evaluate Animation Curve
            float positionCurveValue = positionCurve.Evaluate(percent);

            //Interpolate Position
            transform.position = Vector3.LerpUnclamped(startPosition, targetPosition, positionCurveValue);

            yield return Timing.WaitForOneFrame;
        }
    }
}
