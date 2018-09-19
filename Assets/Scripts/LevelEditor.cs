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

	public void PlaceObject(int x, int y) 
	{
		Cell thisCell = workingMap.GetCell(x, y);
		switch(SelectedTool) // Special considerations for certain objects
		{
			case ObjectTypes.Player:
				workingMap.PlayerSpawn = ObjectTypes.Ground;
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

	public bool LoadLevel (string levelFile) 
	{
		bool loaded = true; // For level load fails

		try
		{
			if (!File.Exists(levelFile))
				break;
		}
		catch (Exception e)
		{
			// Need to work on more specific exception catching..
		}

		return loaded;
	}
	public Map workingMap { get; set; } // The map being edited
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
	private Cell[,] map = new Cell[15, 15]; // 
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
}