using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ObjectTypes
{
	Player,
	Ground,
	Enemy,
	Gate,
	Trigger,
	End,
	Void
}
public class LevelEditor : MonoBehaviour {

	public enum ToolTypes
	{
		Player,
		Ground,
		Enemy,
		Gate,
		Trigger,
		End,
		Void,
		Edit,
		Link,
		Linking,
		Rotate
	}
    public GameObject cellPrefab, mapParent, editDialog;
    [Space(1)]
    [Header("Level Test")]
	public Transform testLevelGroup;
	public GameObject MainMenu;
    [Space(1)]
    [Header("Map File Buttons")]
	public Button loadButton;
	public Button saveButton;
	[Space(1)]
	[Header("Map Editor Buttons")]
	public Button playerButton;
    public Button groundButton;
    public Button voidButton;
    public Button enemyButton;
    public Button gateButton;
    public Button triggerButton;
	public Button editButton;
    public Button rotateButton;
    public Button setNameButton;
    public Button testButton;
    public Button endButton;
    public Button linkButton;
	public Text nameText;
	public Text mapNameText;
	public Text currentToolText;
    [Space(1)]
    [Header("Builder Prefabs")]
    public GameObject playerPrefab;
	public GameObject groundPrefab;
	public GameObject enemyPrefab;
	public GameObject gatePrefab;
	public GameObject triggerPrefab;
	public GameObject endPrefab;

    public static GameObject playerBlock, groundBlock, enemyBlock, gateBlock, triggerBlock, endBlock; // Level building blocks

    private GameObject[,] cellButtons = new GameObject[Map.MapSize[0], Map.MapSize[1]]; // Cell buttons store for script access
	private int[] linkSource = new int[] {-1,-1}; // Store current link source
	private static Color32 playerColour = new Color32(13, 144, 19, 255), groundColour = new Color32(131, 131, 131, 255), gateColour = new Color32(24,236,255,255), enemyColour = new Color32(237, 28, 36, 255), triggerColour = new Color32(255,156,23, 255), endColour = new Color32(0,255,0, 255);
	private List<int[]> linkStorage = new List<int[]>();
	private string mapPath;

    void Start()
	{
		WorkingMap = new Map();
		SelectedTool = ToolTypes.Ground;
		BuildUI();
        // Level Editor UI
        playerButton.onClick.AddListener(delegate{ChangeTool(ToolTypes.Player);});
        groundButton.onClick.AddListener(delegate{ChangeTool(ToolTypes.Ground);});
        enemyButton.onClick.AddListener(delegate{ChangeTool(ToolTypes.Enemy);});
        gateButton.onClick.AddListener(delegate{ChangeTool(ToolTypes.Gate);});
        triggerButton.onClick.AddListener(delegate{ChangeTool(ToolTypes.Trigger);});
		editButton.onClick.AddListener(delegate{ChangeTool(ToolTypes.Edit);});
		rotateButton.onClick.AddListener(delegate{ChangeTool(ToolTypes.Rotate);});
        endButton.onClick.AddListener(delegate {ChangeTool(ToolTypes.End);});
        linkButton.onClick.AddListener(delegate { ChangeTool(ToolTypes.Link); });
        voidButton.onClick.AddListener(delegate{ChangeTool(ToolTypes.Void);});
        setNameButton.onClick.AddListener(ChangeMapNameFromTextBox);
		testButton.onClick.AddListener(delegate{BuildLevel(WorkingMap, testLevelGroup);});
		// Level Editor building blocks init
		playerBlock = playerPrefab;
		groundBlock = groundPrefab;
		enemyBlock = enemyPrefab;
		gateBlock = gatePrefab;
		triggerBlock = triggerPrefab;
		endBlock = endPrefab;
        // Map file buttons
		mapPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + WorkingMap.Name + ".lsmap"; // lifespan map
        saveButton.onClick.AddListener(SaveMap);
        loadButton.onClick.AddListener(LoadMap);
		nameText.rectTransform.parent.GetComponent<InputField>().text = WorkingMap.Name;
	}

	void SaveMap() // For save button
    {
		SaveLevel(WorkingMap, Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + WorkingMap.Name + ".lsmap");
	}

	void LoadMap() // For load button
	{
		WorkingMap = LoadLevel(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + nameText.text + ".lsmap");
	}

	void MapTextChecker()
	{
		if(nameText.text != WorkingMap.Name)
		{
			nameText.rectTransform.parent.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
		} else 
		{
            nameText.rectTransform.parent.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
		}
	}

    void ChangeMapNameFromTextBox() // Set map name
    {
        WorkingMap.Name = nameText.text;
        mapNameText.text = "Map name:\n" + WorkingMap.Name;
    }

    private void BuildUI()
	{
		//Vector3 startCellPosition = new Vector3(-774f, -646f,0f); // x spacing: 105, y spacing: 105
		for (int y = 0; y < Map.MapSize[1]; y++)
		{
			for (int x = 0; x < Map.MapSize[0]; x++)
			{
				GameObject builtCell = Instantiate(cellPrefab, transform, false); // Instantiate cell
				cellButtons[x,y] = builtCell;
				builtCell.transform.localPosition =new Vector3(-774f + (105 * x), -646f + (105 * y), 0); // Set position relative to parent
				int newX =x, newY = y;
				GameObject currentCell = builtCell;
				builtCell.GetComponent<Button>().onClick.AddListener(delegate{EditCell(newX, newY, currentCell);}); // Set button function
			}
		}
	}

	public void ChangeTool(ToolTypes target) // For buttons
	{
		SelectedTool = target;
		currentToolText.text = "Current tool:\n" + target.ToString();
		if (target == ToolTypes.Linking)
		{
			currentToolText.text += " [" + linkSource[0] + ","+linkSource[1]+"]";
		} else
		{
            linkSource = new int[] { -1, -1 }; // Reset link if changed during linking stage
        }
		Debug.Log("Current Tool: " + SelectedTool);
	}

	public void EditCell(int x, int y, GameObject cellClicked) // Place data into cell button
	{
		Debug.Log("Coords:" + x + "," + y + " SelectedTool: " + SelectedTool);
		Cell thisCell = WorkingMap.GetCell(x, y);

		switch(SelectedTool) // Special considerations for certain objects
		{
			case ToolTypes.Player:
				if (WorkingMap.PlayerSpawnCoordinate[1] < cellButtons.GetLength(1) && WorkingMap.PlayerSpawnCoordinate[0] < cellButtons.GetLength(0) && WorkingMap.PlayerSpawnCoordinate[0] >= 0&& WorkingMap.PlayerSpawnCoordinate[0] >= 0)
				{ // Remove old spawn point
                    GameObject oldSpawn = cellButtons[WorkingMap.PlayerSpawnCoordinate[0], WorkingMap.PlayerSpawnCoordinate[1]];
                    oldSpawn.GetComponent<Image>().color = groundColour;
				}
				if(WorkingMap.PlayerSpawn != thisCell)
				{
					if (WorkingMap.PlayerSpawn != null)
                    {
                        WorkingMap.PlayerSpawn.ObjectType = ObjectTypes.Ground; // Replacing old spawn points with ground
					}
					WorkingMap.PlayerSpawn = thisCell;
					WorkingMap.PlayerSpawnCoordinate = new int[] {x,y};
					thisCell.ObjectType = ObjectTypes.Player;
				}
                cellClicked.transform.GetChild(1).gameObject.SetActive(false);
				cellClicked.GetComponent<Image>().color = playerColour;
				// Change button image display
				break;
			case ToolTypes.Enemy:
				if (thisCell.ObjectType == ObjectTypes.Enemy)
				{
					return;
				}
				thisCell.ObjectType = ObjectTypes.Enemy;

                cellClicked.transform.GetChild(1).gameObject.SetActive(true);
				cellClicked.GetComponent<Image>().color = enemyColour;
				// Set up special and colour
				break;
			case ToolTypes.Gate:
                cellClicked.transform.GetChild(1).gameObject.SetActive(true);
				if (thisCell.ObjectType == ObjectTypes.Gate)
				{
					return;
				}
				thisCell.ObjectType = ObjectTypes.Gate;

                cellClicked.GetComponent<Image>().color = gateColour;
                cellClicked.transform.GetChild(1).gameObject.SetActive(true);
				// Set up color
				break;
			case ToolTypes.Trigger:
				if (thisCell.ObjectType == ObjectTypes.Trigger)
				{
					return;
				}
				thisCell.ObjectType = ObjectTypes.Trigger;

                cellClicked.transform.GetChild(1).gameObject.SetActive(false);
                cellClicked.GetComponent<Image>().color = triggerColour;
				// Set up link and colour
				break;
			case ToolTypes.Ground:
				if (thisCell.ObjectType == ObjectTypes.Ground)
				{
					return;
				}
				thisCell.ObjectType = ObjectTypes.Ground;

                cellClicked.transform.GetChild(1).gameObject.SetActive(false);
                cellClicked.GetComponent<Image>().color = groundColour;
				// Ground with no object on it.
				break;
            case ToolTypes.End:
                if (WorkingMap.EndCellCoordinate[1] < cellButtons.GetLength(1) && WorkingMap.EndCellCoordinate[0] < cellButtons.GetLength(0) && WorkingMap.EndCellCoordinate[0] >= 0 && WorkingMap.EndCellCoordinate[0] >= 0)
                {	// Remove old end point
                    GameObject oldEnd = cellButtons[WorkingMap.EndCellCoordinate[0], WorkingMap.EndCellCoordinate[1]];
                    oldEnd.GetComponent<Image>().color = groundColour;
                }
                
                if (WorkingMap.EndCell != thisCell)
                {
					if (WorkingMap.EndCell != null)
                    {
                        WorkingMap.EndCell.ObjectType = ObjectTypes.Ground; // Replace old end point with ground
					}
					WorkingMap.EndCell = thisCell;
					WorkingMap.EndCellCoordinate = new int[] {x,y};
                }
                thisCell.ObjectType = ObjectTypes.End;

                cellClicked.transform.GetChild(1).gameObject.SetActive(false);
                cellClicked.GetComponent<Image>().color = endColour;
                // Ground with no object on it.
                break;
			case ToolTypes.Void:
				if (thisCell.ObjectType == ObjectTypes.Void)
				{
					return;
				}
				thisCell.ObjectType = ObjectTypes.Void;

                cellClicked.transform.GetChild(1).gameObject.SetActive(false);
                cellClicked.GetComponent<Image>().color = Color.white;
				// Clear button and cell on map
				break;
			case ToolTypes.Edit:
				if (thisCell.ObjectType != ObjectTypes.Enemy && thisCell.ObjectType != ObjectTypes.Gate && thisCell.ObjectType != ObjectTypes.Trigger)
				{
					return;
				}

				editDialog.SetActive(true);

				// TODO: Edit tool - colour
				return;
			case ToolTypes.Link:
				if (thisCell.ObjectType != ObjectTypes.Trigger)
					return;
				
				linkSource = new int[] {x, y};

                ChangeTool(ToolTypes.Linking); // Set to link destination selection state
				return;
			case ToolTypes.Linking:
                if (linkSource[0] != -1 && linkSource[1] != -1 && !(linkSource[0] == x && linkSource[1] == y))
                {
                    if (thisCell.ObjectType != ObjectTypes.Gate)
                    {
						Debug.Log("Link destination is not a gate");
                        return;
                    }
                    // check if link already exists (listStorage.Contains() does not match arrays properly)
                    int[] newLink = new int[] { x, y, linkSource[0], linkSource[1]};
                    Text sourceText = cellButtons[linkSource[0], linkSource[1]].transform.GetChild(0).GetComponent<Text>();
                    Text destinationText = cellButtons[x, y].transform.GetChild(0).GetComponent<Text>();
					// Cell link
					thisCell.Link = linkSource.Clone() as int[];
                    WorkingMap.GetCell(linkSource[0], linkSource[1]).Link = new int[] {x,y};
                    int linkIndex = -1;
                    foreach (int[] linked in linkStorage)
                    {
						if (linked[0] == newLink[0] && linked[1] == newLink[1] && linked[2] == newLink[2] && linked[3] == newLink[3])
                        {
                            Debug.Log("Link already exists. Removing link");
							linkIndex = linkStorage.IndexOf(linked);
							
							// Remove link number from button
							sourceText.text = LinkTextModifier(sourceText.text, linkIndex, false);
                            destinationText.text = LinkTextModifier(destinationText.text, linkIndex, false);
							// Modify existing links to the correct number (links that are > removed link is shifted down 1) after removal
							for (int i = linkStorage.Count-1; i > linkIndex; i--)
							{
								Debug.Log(i);
								Text sourceMod = cellButtons[linkStorage[i][0], linkStorage[i][1]].transform.GetChild(0).GetComponent<Text>();
                                Text destinationMod = cellButtons[linkStorage[i][2], linkStorage[i][3]].transform.GetChild(0).GetComponent<Text>();

								sourceMod.text = LinkTextModifier(sourceMod.text, i);
								destinationMod.text = LinkTextModifier(destinationMod.text, i);
							}

							linkStorage.Remove(linked); // Remove link from list
							ChangeTool(ToolTypes.Link); // Reset tool to link
                            return;
						}
                    }
                    // Destination cell link display
                    linkStorage.Add(newLink);
					linkIndex = linkStorage.IndexOf(newLink,0);
                    if (destinationText.text != "")
                    {
                        destinationText.text += ", ";
                    }
                    destinationText.text += linkIndex.ToString();
                    // Source cell link display
                    if (sourceText.text != "")
					{
						sourceText.text += ", ";
					}
					sourceText.text += linkIndex.ToString();
                    ChangeTool(ToolTypes.Link); // Set tool to original state
                    return;
                }
				break;
			case ToolTypes.Rotate:
				if(thisCell.ObjectType != ObjectTypes.Enemy && thisCell.ObjectType != ObjectTypes.Gate)
				{
					Debug.Log("RotateTool: incorrect type");
					return;
				}

				thisCell.Direction += 1; // Rotate clockwise
				if (thisCell.Direction > 3)
				{
					thisCell.Direction = 0; // Wrap around to 0
				}

                cellClicked.transform.GetChild(1).gameObject.SetActive(true);
				cellClicked.transform.GetChild(1).rotation = Quaternion.Euler(0,0,45 - (thisCell.Direction * 90));
				// Rotate button image
				// Rotate tool - only for certain objects
				return;
			default:
				Debug.Log("EditCell: reached default case");
				return;
		}
		thisCell.Direction = 0;
		thisCell.Link = new int[] { -1, -1 }; // Need to remove possible links to this object
		thisCell.Special = null;
	}

	private static string LinkTextModifier (string textDisplay, int linkToRemove, bool modify = true)
	{
        if (textDisplay == linkToRemove.ToString())
        {
            if (modify)
            {
                return (linkToRemove - 1).ToString();
            }
            else
            {
                return "";
            }
        }
        else if (textDisplay.Contains(", " + linkToRemove + ",")) // For "#, linkIndex, "
        {
			if (modify)
			{
                return textDisplay.Replace(", " + linkToRemove + ",", ", " + (linkToRemove - 1) + ",");
			}else
			{
                return textDisplay.Replace(", " + linkToRemove + ",", ",");
			}
        }
        else if (textDisplay.EndsWith(", " + linkToRemove))
        {

            string result = textDisplay.Substring(0, textDisplay.Length - (", " + linkToRemove).Length);
            if (modify)
            {
                return result + ", " + (linkToRemove - 1);
            }
            else
            {
                return result;
            }
        }
        else if (textDisplay.StartsWith(linkToRemove + ", ")) // For "linkIndex, #"
        {
            string result = textDisplay.Substring((linkToRemove + ", ").Length, textDisplay.Length - (linkToRemove + ", ").Length);
            if (modify)
            {
                return (linkToRemove-1) + ", " + result;
            }
            else
            {
                return result;
            }
        }
        else
        {
            return "error";
        }
	}

	private static GameObject CreateObject(int x, int y, Cell cellToBuild, Transform parent) // Create objects in scene
	{
		float objectY = 0;
		Quaternion objectRotation = Quaternion.identity;
		GameObject prefabToPlace;
		switch(cellToBuild.ObjectType) // Special considerations for certain objects
		{
			case ObjectTypes.Player:
				objectY = 1f;
                prefabToPlace = playerBlock;
				break;
			case ObjectTypes.Enemy:
                prefabToPlace = enemyBlock;
				break;
			case ObjectTypes.Gate:
				objectY = 1f;
				objectRotation = Quaternion.Euler(0, (cellToBuild.Direction + 1)* 90, 0);
                prefabToPlace = gateBlock;
                break;
			case ObjectTypes.Trigger:
				objectY =0.03f;
                prefabToPlace = triggerBlock;
				break;
            case ObjectTypes.End:
                objectY = 0.05f;
                prefabToPlace = endBlock;
                break;
			case ObjectTypes.Ground:
                Instantiate(groundBlock, new Vector3(2 * x, 0, 2 * y), Quaternion.identity, parent);
				goto default;
			case ObjectTypes.Void: // if no object exists in the cell
            default:
				return null;
		}
		Instantiate(groundBlock, new Vector3(2 * x, 0, 2 * y), Quaternion.identity, parent); // The ground under object
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
            case "end":
                return ObjectTypes.End;
			case "void":
			default:
				return ObjectTypes.Void;
		}
	}

	public static Map LoadLevel (string levelFile)  // TODO: end object load
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
									int linkX = -1, linkY = -1;
									if (!ValidateCoordinates(new string[] { cellSplit[2], cellSplit[3] }, out linkX, out linkY))
									{
										Debug.Log("Line " + lineNumber + ": Invalid link coordinates.");
									}
									else
									{
										loadCell.Link = new int[2] { linkX, linkY };
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

	public static bool SaveLevel (Map mapToSave, string savePath) // TODO: end object save
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
                            case ObjectTypes.End:
                                saveLine += " end";
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
                    Debug.Log(linkStorage.Count);
					if (objectLink[0] >= 0 && objectLink[1] >= 0)
					{
                        Debug.Log(linkStorage.Count);
						linkStorage.Add(new int[] {x, y}, new int[] {objectLink[0], objectLink[1]}); // Save coords of linked objects
					}
				}
			}
			Debug.Log(linkStorage.Count);
			foreach (KeyValuePair<int[],int[]> link in linkStorage) // Link triggers and gates
			{
				Debug.Log("entered kvp loop");
                GameObject linkSource = objectMap[link.Key[0], link.Key[1]]; // Get source object
                GameObject linkDestination = objectMap[link.Value[0], link.Value[1]]; // Get linked object
                Debug.Log(linkSource);
                GateTrigger gateSource = linkSource.GetComponent<GateTrigger>(); // Check if source object is a gate
                if (gateSource == null) // If not gate then next cell
				{
                    Debug.Log("iteration skipped");
                    continue;
				}
				gateSource.Link = linkDestination; // Set object to disable by trigger
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
	public ToolTypes SelectedTool { get; set; } // Current selected object type
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

	public short Direction { get; set; } // 0 Up, 1 Right, 2 Down, 3 Left 
	public ObjectTypes ObjectType { get; set; } // Null = empty cell
	public int[] Link { get; set; } // For triggers
	public MonoBehaviour Special { get; set; } // Special enemies.. maybe consumables? Future-proofing
}

public class Map {
    private Cell[,] map = new Cell[12, 12];
    private static int[] mapSize = new int[] {12,12}; // If we decide to change size limit later on.

	public Map()
    {
        Name = "UntitledMap";
		PlayerSpawnCoordinate = EndCellCoordinate = new int[2] { -1, -1 };
		for (int x = 0; x < mapSize[0]; x++)
		{
            for (int y = 0; y < mapSize[0]; y++)
            {
				map[x,y] = new Cell();
            }
		}
    }
	public Map(string name)
	{
		Name = name;
        PlayerSpawnCoordinate = EndCellCoordinate = new int[2] { -1, -1 };
        for (int x = 0; x < mapSize[0]; x++)
        {
            for (int y = 0; y < mapSize[0]; y++)
            {
                map[x, y] = new Cell();
            }
        }
	}

	public Cell GetCell(int x, int y) // Returns cell of selected coordinate on map
	{
		return map[x, y];
	}
	
	public string Name { get; set; }
	public Cell PlayerSpawn { get; set; } // To check if player spawn already exists
	public int [] PlayerSpawnCoordinate { get; set; }
	public Cell EndCell {get; set;}
	public int[] EndCellCoordinate {get; set;}
	public static int[] MapSize
    {
        get { return mapSize; }
    }
}