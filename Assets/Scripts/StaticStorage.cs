using System;
using UnityEngine;
using UnityEngine.UI;

public static class StaticStorage
{
	public static Map currentMap;

    public static GameObject playerPrefab, groundPrefab, enemyPrefab, gatePrefab, triggerPrefab, endPrefab; // Level building blocks
}