using System;
using System.IO;
using System.Collections.Generic;
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
	public static GameObject playerPrefab, groundPrefab, enemyPrefab, gatePrefab, triggerPrefab; // Level building blocks
	public UnityEngine.UI.Button playerButton, groundButton, voidButton, enemyButton, gateButton, triggerButton, setNameButton;
	void Start()
	{
		WorkingMap = new Map();
		SelectedTool = ObjectTypes.Ground;
        // Level Editor UI
        playerButton.onClick.AddListener(delegate{ChangeTool(ObjectTypes.Player);});
        groundButton.onClick.AddListener(delegate{ChangeTool(ObjectTypes.Ground);});
        enemyButton.onClick.AddListener(delegate{ChangeTool(ObjectTypes.Enemy);});
        gateButton.onClick.AddListener(delegate { ChangeTool(ObjectTypes.Gate); });
        triggerButton.onClick.AddListener(delegate { ChangeTool(ObjectTypes.Trigger); });
        voidButton.onClick.AddListener(delegate { ChangeTool(ObjectTypes.Void); });
        setNameButton.onClick.AddListener(ChangeMapNameFromTextBox);
		// TODO play button, load from file, save to file.
	}

	public void ChangeTool(ObjectTypes target) // For buttons
	{
		SelectedTool = target;
	}

	public void ChangeMapNameFromTextBox() // Need textbox and setNameButton
	{
		
	}

	public void PlaceObject(int x, int y) // Place data into cell button
	{
		Cell thisCell = WorkingMap.GetCell(x, y);
		switch(SelectedTool) // Special considerations for certain objects
		{
			case ObjectTypes.Player:
				if(WorkingMap.PlayerSpawn != thisCell)
				{
					WorkingMap.PlayerSpawn = thisCell;
					WorkingMap.PlayerSpawnCoordinate = new int[] {x,y};
				}
				break;
			case ObjectTypes.Enemy:
				// Set up special and colour
				break;
			case ObjectTypes.Gate:
				// Set up color
				break;
			case ObjectTypes.Trigger:
				// Set up link and colour
				break;
			default:
				break;
		}
		thisCell.ObjectType = SelectedTool;
	}

	private static GameObject CreateObject(int x, int y, Cell cellToBuild, Transform parent) // Create objects in scene
	{
		float objectY = 0;
		Quaternion objectRotation = Quaternion.identity;
		GameObject prefabToPlace;
		switch(cellToBuild.ObjectType) // Special considerations for certain objects
		{
			case ObjectTypes.Player:
				objectY = 2.2f;
                prefabToPlace = playerPrefab;
				break;
			case ObjectTypes.Enemy:
                prefabToPlace = enemyPrefab;
				break;
			case ObjectTypes.Gate:
				objectY = 2.0f;
				objectRotation = Quaternion.Euler(0, cellToBuild.Direction * 90, 0);
                prefabToPlace = gatePrefab;
                break;
			case ObjectTypes.Trigger:
				objectY =1f;
                prefabToPlace = playerPrefab;
				break;
			case ObjectTypes.Ground:
                Instantiate(groundPrefab, new Vector3(2 * x, 0, 2 * y), Quaternion.identity, parent);
				goto default;
			case ObjectTypes.Void: // if no object exists in the cell
            default:
				return null;
		}
		Instantiate(groundPrefab, new Vector3(2 * x, 0, 2 * y), Quaternion.identity, parent); // The ground under object
		return Instantiate(prefabToPlace, new Vector3(2 * x, objectY, 2 * y), objectRotation, parent); // The object itself
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

				// Player spawn set
				if (!ValidateCoordinates(spawn, out x, out y))
				{
					Debug.Log("Invalid player spawn point.");
					return null;
				}
				loadedMap.PlayerSpawnCoordinate = new int[] {x,y};
				loadedMap.GetCell(x, y).ObjectType = ObjectTypes.Player;

				string lineRead = reader.ReadLine();
				int lineNumber = 3;
                while(lineRead !=null)
                {
					// Save Line Format: #,# ObjectType,Direction,LinkX,LinkY,Special
					// Mandatory: #,# ObjectType,Direction
					string[] lineSplit = lineRead.Split(' ');
					if (ValidateCoordinates(lineSplit[0].Split(','), out x, out y))
					{
						string[] cellSplit = lineSplit[1].Split(',');
						Cell loadCell= loadedMap.GetCell(x, y);
						if (loadCell != loadedMap.PlayerSpawn)
						{
							switch (cellSplit.Length)
							{
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
									short direction = 0;
									short.TryParse(cellSplit[1], out direction);
									loadCell.Direction = direction;
									goto case 1;
								case 1:
									loadCell.ObjectType = StringToObjectTypes(cellSplit[0]);
									break;
							}
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
			Debug.Log("File not found:" + "\"" + levelFile + "\"");
			Debug.Log(e.Message);
			return null;
		}
		catch (FileLoadException e)
		{
			Debug.Log("File load failed:" + "\"" + levelFile + "\"");
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

	public static bool SaveLevel (Map mapToSave, string savePath)
	{
		try
		{
			if (File.Exists(savePath))
			{
				// ask for user confirmation before overwriting
			}
			using (StreamWriter writer = new StreamWriter(savePath))
			{
				// Map name and spawn coordinates
				writer.WriteLine(mapToSave.Name);
				writer.WriteLine(mapToSave.PlayerSpawnCoordinate[0] +","+ mapToSave.PlayerSpawnCoordinate[1]);

				for (int x = 0; x < Map.MapSize[0]; x++)
				{
					for (int y = 0; y < Map.MapSize[1]; y++)
					{
						// Save Line Format: #,# ObjectType,Direction,LinkX,LinkY,Special
						// Mandatory: #,# ObjectType,Direction
						Cell cellToSave = mapToSave.GetCell(x,y);
						string saveLine = x+","+y;
						switch (cellToSave.ObjectType)
						{
							case ObjectTypes.Player:
							case ObjectTypes.Void:
							default:
								continue;
							case ObjectTypes.Gate:
								saveLine += " gate," + cellToSave.Direction;
								break;
							case ObjectTypes.Enemy:
								saveLine += " enemy," + cellToSave.Direction;
								break;
							case ObjectTypes.Trigger:
								saveLine += " trigger,0,"+cellToSave.Link[0]+","+cellToSave.Link[1];
								break;
							case ObjectTypes.Ground:
								saveLine += " ground";
								break;
						}
						writer.WriteLine(saveLine);
					}
				}
			}
		}
		catch (DirectoryNotFoundException e)
		{
			Debug.Log("Directory not found: " + "\"" + savePath + "\"");
			Debug.Log(e.Message);
			return false;
		}
		catch (DriveNotFoundException e)
		{
			Debug.Log("Drive not found: " + "\"" + savePath + "\"");
			Debug.Log(e.Message);
			return false;
		}
		catch (PathTooLongException e)
		{
			Debug.Log("Path is too long:" + "\"" + savePath + "\"");
			Debug.Log(e.Message);
			return false;
		}
		catch (Exception e)
		{
			Debug.Log(e.Message);
			return false;
		}
		return true;
	}

	public static bool BuildLevel(Map mapToBuild, Transform parent)
	{
        GameObject[,] objectMap = new GameObject[Map.MapSize[0], Map.MapSize[1]]; // For establishing object links
        Dictionary<int[], int[]> linkStorage = new Dictionary<int[], int[]>(); // Store link information from map

        try
		{
			for (int x = 0; x < Map.MapSize[0]; x++)
			{
				for (int y = 0; y < Map.MapSize[1]; y++)
				{
					Cell cellToBuild = mapToBuild.GetCell(x, y);
                    objectMap[x,y] = CreateObject(x, y, cellToBuild, parent);
					int[] objectLink = cellToBuild.Link; // Store link to link after all objects are built
					if (objectLink[0] >= 0 && objectLink[1] >= 0)
					{
						linkStorage.Add(new int[] {x, y}, objectLink); // Save coords of linked objects
					}
				}
			}
            for (int x = 0; x < Map.MapSize[0]; x++)
            {
                for (int y = 0; y < Map.MapSize[1]; y++)
                {
					int[] currentCoords = new int[] { x, y };
                    if(linkStorage.ContainsKey(currentCoords))
					{
						GameObject linkSource = objectMap[x,y]; // Get source object
						int [] linkCoords = linkStorage[currentCoords]; // Get coords of object its linked to
						GameObject linkDestination = objectMap[linkCoords[0], linkCoords[1]]; // Get linked object

						GateTrigger gateSource = linkSource.GetComponent<GateTrigger>(); // Check if source object is a gate
						if(gateSource == null) // If not gate then next cell
							continue;
						gateSource.Link = linkDestination; // Set object to disable by trigger
					}
                }
            }
		}
		catch (Exception e)
		{
			Debug.Log("Level build failed: " + e.Message);
			return false;
		}
		return true;
	}

	public Map WorkingMap { get; set; } // The map being edited
	public ObjectTypes SelectedTool { get; set; } // Current selected object type
}

public class Cell
{
	public Cell()
	{
		Direction = 0;
		ObjectType = ObjectTypes.Void;
		Link = new int[] {-1,-1};
		Special = null;
	}

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
		PlayerSpawnCoordinate = new int[2] { -1, -1 };
    }
	public Map(string name)
	{
		Name = name;
		PlayerSpawnCoordinate = new int[2] { -1, -1 };
	}

	public Cell GetCell(int x, int y) // Returns cell of selected coordinate on map
	{
		return map[x, y];
	}
	
	public string Name { get; set; }
	public Cell PlayerSpawn { get; set; } // To check if player spawn already exists
	public int [] PlayerSpawnCoordinate { get; set; }
	public static int[] MapSize
    {
        get { return mapSize; }
    }
}