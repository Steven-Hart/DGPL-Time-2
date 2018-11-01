using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeController : MonoBehaviour {

    public GameObject[] _lives;
	public GameObject[] moves;
	public int moveCounter;
    public int lifeCounter = 0;
    public int LifeCount;

    // Use this for initialization
    void Start () {
        //GameObject.FindGameObjectsWithTag("Life");
    }
	
	// Update is called once per frame
	public void SetStartingMoves()
	{
		/*
        if(moves[0] == null)
        {
            MovesReset();
            //LinkMoves();
        }
		*/
        if (StartingMoves < moves.Length)
		{
			for (int i = moves.Length-1; i > StartingMoves-1; i--)
			{
				moves[i].SetActive(false);
			}
		}
		moveCounter = StartingMoves-1;
		MovesReset();
	}

	public void MinusMove()
	{
		moves[moveCounter].GetComponent<MoveCounter>().LoseMove();
		moveCounter--;
	}

	public void MovesReset()
	{
		moveCounter=StartingMoves-1;
		for (int i = 0; i < StartingMoves; i++)
		{
			moves[i].SetActive(true);
			moves[i].GetComponent<MoveCounter>().ResetMove();
		}
	}
    //Disables game object one by one
    public void MinusLife()
    {
        if(lifeCounter < _lives.Length)
        {
            _lives[lifeCounter].GetComponent<Lifeline>().LoseLife();
			lifeCounter++;
        }
    }

	public int StartingMoves {get; set;}

    public void LinkMoves()
    {
        int[] moveNo = new int[] {0,1,4,2,5,8,3,6,9,12,7,10,13,11,14,15};
        for(int i = 0; i<moves.Length;i++)
        {
            
            string moveName = string.Format("Move ({0})", moveNo[i]);
            moves[i] = GameObject.Find(moveName);

            
        }
    }

    public void transferLink()
    {
        for (int i = 0; i < moves.Length; i++)
        {
            moves[i].SetActive(true);
            moves[i].GetComponent<MoveCounter>().ResetMove();
        }
    }

    public void ResetCanvas()
    {
		lifeCounter = 0;
        MovesReset();
    }
}
