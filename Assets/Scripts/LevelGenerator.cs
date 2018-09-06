using UnityEngine;

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
        Color pixelColor = map.GetPixel(x,z); // Color of pixel in image 

        if (pixelColor.a == 0)
        {
            // The pixel is transparent, ignore.
            return;
        }

        foreach (ColorToPrefab colorMapping in colorMappings)
        {
            if (colorMapping.color.Equals(pixelColor))
            {
                Vector3 position = new Vector3(2*x, -1.1f, 2*z); // Coordinate offsets need to be set for each object type
                Instantiate(colorMapping.prefab, position, Quaternion.identity, transform);
				return; // Break loop after match
            }
        }
        //Debug.Log(pixelColor);
    }
}

[System.Serializable]
public class ColorToPrefab
{
    public Color color;
    public GameObject prefab;
}
