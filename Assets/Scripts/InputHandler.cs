using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour {

    float targetScale;
    float lerpSpeed = 2;

    private void Update()
    {
        /*//Mouse Input
         * 
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        if(mouseX != 0 && mouseY != 0)
        {
            targetScale = 0.5f;
            lerpSpeed = 10;
        }
        */

        //Keyboard Input
        float keyInputX = Input.GetAxis("Horizontal");
        float keyInputY = Input.GetAxis("Vertical");

        if (keyInputX != 0 || keyInputY != 0)
        {
            targetScale = 1;
            lerpSpeed = 10;
        }
        else
        {
            targetScale = 0;
            lerpSpeed = 4;
        }

        GameManager.instance.timeManager.myTimeScale = Mathf.Lerp(GameManager.instance.timeManager.myTimeScale, targetScale, Time.deltaTime * lerpSpeed);

    }

}
