using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterpolationAnim : MonoBehaviour {

    public AnimationCurve animCurve;

    IEnumerator AnimatePosition(Vector3 origin, Vector3 target, float duration)
    {
        float journey = 0f;
        while (journey <= duration)
        {
            journey = journey + Time.deltaTime;

            float percent = (Mathf.Clamp01(journey / duration));
            float curvePercent = animCurve.Evaluate(percent);

            transform.position = Vector3.Lerp(origin, target, curvePercent);

            yield return null;
        }
    }
}
