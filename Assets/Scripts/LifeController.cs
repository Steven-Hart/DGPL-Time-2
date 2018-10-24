using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeController : MonoBehaviour {

    public GameObject[] _lives;
    private int life_counter;
    public int LifeCount;
    // Use this for initialization
    void Start () {
            //GameObject.FindGameObjectsWithTag("Life");
        life_counter = 0;
    }
	
	// Update is called once per frame
	void Update () {
		
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
}
