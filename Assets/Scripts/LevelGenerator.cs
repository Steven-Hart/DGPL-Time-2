﻿using UnityEngine;

public class LevelGenerator : MonoBehaviour {

    public Texture2D map;
    public ColorToPrefab[] colorMappings;

	// Use this for initialization
	void Start () {
        GenerateLevel();
	}

    void GenerateLevel()
    {
        for (int x = 0; x < map.width; x++)
        {
            for (int z = 0; z < map.height; z++)
            {
                GenerateTile(x, z);
            }
        }
    }

    void GenerateTile(int x, int z)
    {
        Color pixelColor = map.GetPixel(x,z);

        if (pixelColor.a == 0)
        {
            // The pixel is transparent, igore.
            return;
        }

        foreach (ColorToPrefab colorMapping in colorMappings)
        {
            if (colorMapping.color.Equals(pixelColor))
            {
                Vector3 position = new Vector3(x, 0, z);
                Instantiate(colorMapping.prefab, position, Quaternion.identity, transform);
            }
        }
        Debug.Log(pixelColor);
    }
}
