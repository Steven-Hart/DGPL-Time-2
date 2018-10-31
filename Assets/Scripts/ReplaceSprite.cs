using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ReplaceSprite : MonoBehaviour {

    private SpriteRenderer[] sprites;
    public Sprite sprite;
    public Color colour;

	// Use this for initialization
	void Start () {
        sprites = GetComponentsInChildren<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		foreach (SpriteRenderer s in sprites)
        {
            s.sprite = sprite;
            s.color = colour;
        }
	}
}
