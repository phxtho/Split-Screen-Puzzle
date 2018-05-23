using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public Texture2D map;

    public ColourToPrefab[] colourMappings;

    private void Start()
    {
        GenerateLevel();
    }

    void GenerateLevel()
    {
        for(int x = 0; x < map.width; x++)
        {
            for(int z = 0; z< map.height; z++)
            {
                GenerateTile(x, z);
            }
        }
    }

    void GenerateTile(int x, int z)
    {
        Color pixelColour = map.GetPixel(x, z);

        //Skips transparent pixels
        if (pixelColour.a == 0)
            return;

        foreach(ColourToPrefab colourMapping in colourMappings)
        {
            if (colourMapping.colour.Equals(pixelColour))
            {
                Vector3 position = new Vector3(x, 0, z);
                Instantiate(colourMapping.prefab, position, Quaternion.identity, transform);
            }
        }
    }
}
