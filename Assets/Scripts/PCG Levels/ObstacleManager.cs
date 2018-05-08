using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class ObstacleManager : MonoBehaviour {
    [Serializable]
    public class Count
    {
        public int minimum, maximum;

        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    public GameObject[] wallArray, obstacleArray, goalArray;
    List<Vector3> gridPositions;

    public Count wallCount = new Count(4, 8);
    public Count obstacleCount = new Count(3, 7);

    void Start()
    {
        gridPositions = GetComponent<BoardManager>().gridPositions;
        PlaceObjects(goalArray, 1, 1);
       // PlaceObjects(obstacleArray, obstacleCount.minimum, obstacleCount.maximum);
        PlaceObjects(wallArray, wallCount.minimum, wallCount.maximum);
        
    }

    Vector3 RandomPosition()
    {
        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector3 randomPosition = gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex);
        return randomPosition;
    }

    void PlaceObjects(GameObject[] objectArray, int minimum, int maximum)
    {
        int objectCount = Random.Range(minimum, maximum + 1);
        
        for(int i = 0; i < objectCount; i++)
        {
            Vector3 randomPosition = RandomPosition();
            randomPosition = randomPosition + transform.position;

            GameObject objectChoice = objectArray[Random.Range(0, objectArray.Length)];
            GameObject instance = Instantiate(objectChoice, randomPosition, Quaternion.identity);
            instance.transform.SetParent(this.gameObject.transform);
            instance.transform.localPosition = instance.transform.localPosition + new Vector3(0f, 1f, 0f);
        }
    }
}
