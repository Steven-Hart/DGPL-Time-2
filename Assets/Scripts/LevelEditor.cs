using System;
using System.IO;
using UnityEngine;

public enum ObjectTypes
{
	Player,
	Ground,
	Enemy,
	Gate,
	Trigger,
	Void
}
public class LevelEditor : MonoBehaviour {

    public GameObject cellPrefab;
	public GameObject playerPrefab, groundPrefab, enemyPrefab, gatePrefab, triggerPrefab; // Level building blocks

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

	private static bool ValidateCoordinates(string[] stringCoords, out int x, out int y)
	{
		x = y = -1;
		if (int.TryParse(stringCoords[0], out x) && int.TryParse(stringCoords[1], out y)) // Loaded coordinate type check
		{
			if (x < 0 || x > Map.MapSize[0] || y < 0 || y > Map.MapSize[1]) // Loaded coordinate range check
			{
				Debug.Log("Coordinates outside of map bounds.");
				return false;
			}
		}
		else
		{
			Debug.Log("Coordinates are not numeric.");
			return false;
		}
		return true;
	}

	private static ObjectTypes StringToObjectTypes(string stringType)
	{
		switch (stringType.ToLower())
		{
			case "player":
				return ObjectTypes.Player;
			case "enemy":
				return ObjectTypes.Enemy;
			case "gate":
				return ObjectTypes.Gate;
			case "trigger":
				return ObjectTypes.Trigger;
			case "ground":
				return ObjectTypes.Ground;
			case "void":
			default:
				return ObjectTypes.Void;
		}
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
                int x, y; // Coordinates

				if (!ValidateCoordinates(spawn, out x, out y))
				{
					Debug.Log("Invalid player spawn point.");
					return null;
				}
				loadedMap.GetCell(x, y).ObjectType = ObjectTypes.Player;

				string lineRead = reader.ReadLine();
				int lineNumber = 3;
                while(lineRead !=null)
                {
					// Save Line Format: #,# ObjectType,Direction,Link,Special
					// Mandatory: #,# ObjectType,Direction
					string[] lineSplit = lineRead.Split(' ');
					if (ValidateCoordinates(lineSplit[0].Split(','), out x, out y))
					{
						string[] cellSplit = lineSplit[1].Split(',');
						Cell loadCell= loadedMap.GetCell(x, y);
						switch (cellSplit.Length)
						{
							case 1:
								Debug.Log("Line " + lineNumber + ": Invalid Number of cell Attributes.");
								break;
							default:
							case 5:
								// Load special script (future-proofing)
								goto case 4;
							case 4:
								// Load object link
								if (!ValidateCoordinates(new string[] { cellSplit[2], cellSplit[3] }, out x, out y))
								{
									Debug.Log("Line " + lineNumber + ": Invalid link coordinates.");
								}
								else
								{
									loadCell.Link = new int[2] { x, y };
								}
								goto case 2; // The forbidden goto because of C# limitations
							case 3:
							case 2:
								// Load object direction
								short direction = 0;
								short.TryParse(cellSplit[1], out direction);
								loadCell.Direction = direction;

								// Load type
								loadCell.ObjectType = StringToObjectTypes(cellSplit[0]);
								break;
						}
					}
					else
					{
						Debug.Log("Line " + lineNumber + ": Invalid coordinates.");
					}

					lineNumber++;
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
	public int[] Link { get; set; } // For triggers
	public MonoBehaviour Special { get; set; } // Special enemies.. maybe consumables? Future-proofing
}

public class Map {
    private Cell[,] map = new Cell[12,12];
    private static int[] mapSize = new int[] {12,12}; // If we decide to change size limit later on.

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
    public static int[] MapSize
    {
        get { return mapSize; }
    }
}