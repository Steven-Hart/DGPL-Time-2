﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeController : MonoBehaviour {

    public GameObject[] _lives;
	public GameObject[] moves;
	private int moveCounter;
    private int life_counter;
    public int LifeCount;
    // Use this for initialization
    void Start () {
		
            //GameObject.FindGameObjectsWithTag("Life");
        life_counter = 0;
    }
	
	// Update is called once per frame
	public void SetStartingMoves()
	{
		if (StartingMoves < moves.Length)
		{
			for (int i = moves.Length-1; i > StartingMoves-1; i--)
			{
				moves[i].SetActive(false);
			}
		}
		moveCounter = StartingMoves-1;
	}

	public void MinusMove()
	{
		moves[moveCounter].SetActive(false);
		moveCounter--;
	}

	public void MovesReset()
	{
		moveCounter=StartingMoves-1;
		for (int i = 0; i < StartingMoves; i++)
		{
			moves[i].SetActive(true);
		}
	}
    //Disables game object one by one
    public void MinusLife()
    {
        if(life_counter <= _lives.Length)
        {
            _lives[life_counter].SetActive(false);
            life_counter++;
        }
    }


	public int StartingMoves {get; set;}
}
