using System;
using System.IO;
using UnityEngine;

public enum ObjectTypes
{
	Player,
	Ground,
	Enemy,
	Gate,
	Trigger
}
public class LevelEditor : MonoBehaviour {

    public GameObject cellPrefab;   

	public void PlaceObject(int x, int y) 
	{
		Cell thisCell = WorkingMap.GetCell(x, y);
		switch(SelectedTool) // Special considerations for certain objects
		{
			case ObjectTypes.Player:
				WorkingMap.PlayerSpawn = null;
				break;
			case ObjectTypes.Enemy:
				// Set up special and colour
				break;
			case ObjectTypes.Gate:
				// Set up link? and colour
				break;
			case ObjectTypes.Trigger:
				// Set up link and colour
				break;
			default:
				break;
		}
		thisCell.ObjectType = SelectedTool;
	}

	public static Map LoadLevel (string levelFile) 
	{
        Map loadedMap = new Map();
		try
		{
            using (StreamReader reader = File.OpenText(levelFile))
            {
                loadedMap.Name = reader.ReadLine();
                string[] spawn = reader.ReadLine().Split(','); // Format: x,y
                int x, y = -1;
                if (int.TryParse(spawn[1],out x) && int.TryParse(spawn[1], out y)) // Loaded coordinate type check
                {
                    if(x < 0 || x > loadedMap.MapSize[0] || y < 0 || y > loadedMap.MapSize[1]) // Loaded coordinate range check
                    {
                        Debug.Log("Coordinates outside of map bounds.");
                        return null;
                    }
                }
                else
                {
                    Debug.Log("Line 2: Spawn Coordinates are not numeric.");
                    return null;
                }
                loadedMap.GetCell (x, y);
                string lineRead = reader.ReadLine();
                while(lineRead !=null)
                {
                    // Save Line Format: #,# ObjectType,Direction,Link,Special
					// Mandatory: #,# ObjectType,Direction
                    lineRead = reader.ReadLine();
                }
            }
		}
		catch (FileNotFoundException e)
		{
			Debug.Log("\"" + levelFile + "\"" + " does not exist.");
			Debug.Log(e.Message);
			return null;
		}
		catch (FileLoadException e)
		{
			Debug.Log("\"" + levelFile + "\"" + " cannot be loaded.");
			Debug.Log(e.Message);
			return null;
		}
		catch (Exception e)
		{
            Debug.Log(e.Message);
            return null;
		}
		return loadedMap;
	}

	public static bool SaveLevel (Map mapToSave)
	{

		return true;
	}

	public static bool BuildLevel(Map mapToBuild)
	{

		return true;
	}

	public Map WorkingMap { get; set; } // The map being edited
	public ObjectTypes SelectedTool { get; set; } // Current selected object type
}

public class Cell
{
	public short Direction { get; set; } // 0 Up, 1 Down, 2 Left, 3 Right 
	public ObjectTypes ObjectType { get; set; } // Null = empty cell
	public GameObject Link { get; set; } // For triggers
	public MonoBehaviour Special { get; set; } // Special enemies.. maybe consumables?
}

public class Map {
    private Cell[,] map = new Cell[12,12];
    private int[] mapSize = new int[] {12,12};

    public Map()
    {
        Name = "UntitledMap";
    }
	public Map(string name)
	{
		Name = name;
	}

	public Cell GetCell(int x, int y) // Returns cell of selected coordinate on map
	{
		return map[x, y];
	}
	
	public string Name { get; set; }
	public Cell PlayerSpawn { get; set; } // To check if player spawn already exists
    public int[] MapSize
    {
        get { return mapSize; }
    }
}