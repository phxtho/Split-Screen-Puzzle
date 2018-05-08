using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    public Vector3 originalPos;
    public float shakeDuration;
    public float shakeAmount;
    public AnimationCurve decreaseFactor;

    private float shakeTime = 0.4f;
    private bool isShaking = false;
    private float startTime;

	void Start () {
        originalPos = transform.localPosition;
	}
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
            StartShaking();

        if (isShaking)
        {
            float t = (Time.time - startTime) / shakeDuration;
            float amount = shakeAmount * decreaseFactor.Evaluate(t);
            Vector3 randomVector = Random.insideUnitSphere;
            Vector3 targetPos = originalPos + randomVector * amount;
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, t);
        }
        if(Time.time >= startTime + shakeTime)
        {
            transform.localPosition = originalPos;
            isShaking = false;
        }
	}

    public void StartShaking()
    {
        startTime = Time.time;
        isShaking = true;
    }
}
