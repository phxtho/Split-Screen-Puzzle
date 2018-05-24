using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSystem : MonoBehaviour {

    public GameObject[] arrows;
    float x;
    float y;
    public float spoed;

    private void Update()
    {
        //Movement Input
        x = Input.GetAxis("Horizontal") *spoed ;
        y = Input.GetAxis("Vertical") * spoed;

        if(x == 0)
        {
            arrows[2].SetActive(false);
            arrows[0].SetActive(false);
        }

        if(y == 0)
        {
            arrows[1].SetActive(false);
            arrows[3].SetActive(false);
        }

        if (x > 0)
        {
            arrows[2].SetActive(false);
            arrows[0].SetActive(true);
        }
        else if(x < 0)
        {
            arrows[0].SetActive(false);
            arrows[2].SetActive(true);
        }

        if(y > 0)
        {
            arrows[3].SetActive(false);
            arrows[1].SetActive(true);
        }
        else if(y < 0)
        {
            arrows[1].SetActive(false);
            arrows[3].SetActive(true);
        }
    }
}
