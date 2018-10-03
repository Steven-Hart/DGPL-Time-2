using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FadeFix : MonoBehaviour {

    private Material m;

	// Use this for initialization
	void Start () {
        m = GetComponent<Renderer>().sharedMaterial;
	}
	
	// Update is called once per frame
	void Update () {
        m.SetInt("_ZWrite", 1);
	}
}
