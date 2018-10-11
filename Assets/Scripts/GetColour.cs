using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GetColour : MonoBehaviour {

    public GameObject button;
    private Color buttonColour;

	// Use this for initialization
	void Start () 
	{
		if(button==null)
		{
			return;
		}
		buttonColour = button.GetComponent<Renderer>().sharedMaterial.color;
		GetComponent<SpriteRenderer>().color = buttonColour;
	}
}
