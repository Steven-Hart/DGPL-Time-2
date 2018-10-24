using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeController : MonoBehaviour {

    private GameObject[] _lives;
    private int life_counter;
    // Use this for initialization
    void Start () {
        
        _lives = GameObject.FindGameObjectsWithTag("Life");
        life_counter = _lives.Length - 1;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    //Disables game object one by one
    public void MinusLife()
    {
        if(life_counter >= 0)
        {
            _lives[life_counter].SetActive(false);
            life_counter--;
        }
    }
}
