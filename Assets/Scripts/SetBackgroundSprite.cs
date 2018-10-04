using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SetBackgroundSprite : MonoBehaviour {

    private SpriteRenderer sr;
    private Camera cam;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        cam = Camera.main;
    }

    void Start()
    {
        Resize();
        Recolour();
    }

    //Set the sprite colour to the camera background colour
    private void Recolour()
    {
        sr.color = cam.backgroundColor;
    }

    //Scale sprite to fit the camera bounds
    //Method from https://answers.unity.com/questions/620699/scaling-my-background-sprite-to-fill-screen-2d-1.html
    private void Resize()
    {
        if (sr == null) return;
        transform.localScale = new Vector3(1, 1, 1);

        float width = sr.sprite.bounds.size.x;
        float height = sr.sprite.bounds.size.y;


        float worldScreenHeight = cam.orthographicSize * 2f;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        Vector3 xWidth = transform.localScale;
        xWidth.x = worldScreenWidth / width;
        transform.localScale = xWidth;
        //transform.localScale.x = worldScreenWidth / width;
        Vector3 yHeight = transform.localScale;
        yHeight.y = worldScreenHeight / height;
        transform.localScale = yHeight;
        //transform.localScale.y = worldScreenHeight / height;

    }
}
