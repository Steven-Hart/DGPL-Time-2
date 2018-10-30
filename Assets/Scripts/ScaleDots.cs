using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ScaleDots : MonoBehaviour {

    private Transform[] dots;
    public float scale;
    public Color colour;

	// Use this for initialization
	void Start () {
        dots = GetComponentsInChildren<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 1; i < dots.Length; i++)
        {
            dots[i].localScale = new Vector2(scale, scale);
            dots[i].gameObject.GetComponent<Image>().color = colour;
        }
	}
}
