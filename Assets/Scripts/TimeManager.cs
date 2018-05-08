using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM : MonoBehaviour {
    public float myDelta;
    public float myFixedDelta;
    public float myTimeScale = 1;

    public static GM instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        myFixedDelta = Time.fixedDeltaTime * myTimeScale;
    }

    private void Update()
    {
        myDelta = Time.deltaTime * myTimeScale;
    }



}
