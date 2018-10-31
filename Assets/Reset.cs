using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reset : MonoBehaviour {

	// Use this for initialization
	public void LoadLevel()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

}
