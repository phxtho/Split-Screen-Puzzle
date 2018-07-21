using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RaycastController))]

public class PowerManagement : MonoBehaviour {
    public enum PowerUpType { none,shrink, armour }
    public PowerUpType powerUpType;

    public Cage[] cages;
    Vector3 baseScale;
    public float shrinkScaleMul = 0.5f;

    bool powerUpActive = false;

    [System.Serializable]
    public struct Cage
    {
        public PowerUpType type;
        public GameObject cageObject;
    }

    private void Start()
    {
        baseScale = transform.localScale;
        DeactivateCages();
    }

    private void Update()
    {
        switch (powerUpType)
        {
            case (PowerUpType.none):
                CheckCage(PowerUpType.none);
                transform.localScale = baseScale;
                break;

            case (PowerUpType.shrink):
                CheckCage(PowerUpType.shrink);
                break;

            case (PowerUpType.armour):
                CheckCage(PowerUpType.armour);
                transform.localScale = baseScale;
                break;
        }
    }

    public void ActivatePowerUp()
    {
        if (powerUpActive)
            return;

        switch (powerUpType)
        {
            case (PowerUpType.shrink):
                Shrink();
                break;

            case (PowerUpType.armour):
                //Armour Power Up
                break;
        }
    }

    void Shrink()
    {
        float duration = 20f;
        float progress = 0f;
        //Change Scale
        while (progress <= duration)
        {
            progress += Time.deltaTime;
            float percentage = progress / duration;
            transform.localScale = Vector3.Lerp(baseScale, baseScale * shrinkScaleMul, percentage);
        }  
    }
    #region Cage Control
    void CheckCage(PowerUpType type)
    {
        GameObject cage = GetCage(type);
        if (cage != null && cage.active == false)
        {
            DeactivateCages();
            ActivateCage(type);
        }
    }

    void ActivateCage(PowerUpType powerUpType)
    {
        foreach (Cage cage in cages)
        {
            if (cage.type == powerUpType)
            {
                cage.cageObject.SetActive(true);
                return;
            }
                
        }
        Debug.Log("Cage " + powerUpType + " not found");
    }

    GameObject GetCage(PowerUpType powerUpType)
    {
        foreach (Cage cage in cages)
        {
            if (cage.type == powerUpType)
            {
                return cage.cageObject;
            }

        }
        return null;
    }

    void DeactivateCages()
    {
        foreach (Cage cage in cages)
        {
            cage.cageObject.SetActive(false);
        }
    }
    #endregion
}
