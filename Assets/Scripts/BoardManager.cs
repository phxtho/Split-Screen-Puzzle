using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {
    public int columns, rows;
    public GameObject floorTile, outerBoundry;
    public Vector3 offset;

    Transform boardHolder;
    [HideInInspector]
    public List<Vector3> gridPositions = new List<Vector3>();

    private void Awake()
    {
        offset = this.gameObject.transform.localPosition;
        InitializeList();
        BoardSetUp();
    }

    void InitializeList()
    {
        gridPositions.Clear();

        for(int x = 1; x < columns -1; x++)
        {
            for (int z = 1; z< rows-1; z++)
            {
                gridPositions.Add(new Vector3(x, 0f, z));
            }
        }
    }

    void BoardSetUp()
    {
        GameObject toInstantiate;

        for(int x = -1; x < columns + 1; x++)
        {
            for(int z = -1; z < rows + 1; z++)
            {
                Vector3 instancePosition = new Vector3(x, 0f, z);
                toInstantiate = floorTile;

                if (x == -1||x == columns || z == -1 || z == rows)
                    toInstantiate = outerBoundry;

                GameObject instance = Instantiate(toInstantiate, instancePosition + offset, Quaternion.identity) as GameObject;
                instance.transform.SetParent(this.gameObject.transform);

            }
        }
    }
}
